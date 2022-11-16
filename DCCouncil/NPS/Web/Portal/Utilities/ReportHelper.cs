using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.ViewModels;
using PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary;
using System;
using System.Collections.Generic;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.Utilities
{
    public class ReportHelper
    {
        public static Guid GenerateNPSReport(Office objOffice,FiscalYear objFiscalYear,DateTime dtAsOfDate,string rdoFilters){                        

            //Generating the list of Office Ids, which is passed to get all the Expenditures
            List<long> lstOfficeIds = new List<long>();
            lstOfficeIds.Add(objOffice.OfficeID);
            string  strOfficeIdsXml = Office.GenerateXml(lstOfficeIds);

            //Get all the expenditures for the selected office and fiscal year
            List<IDataHelper> lstExpenditures = Expenditure.GetAll(string.Empty, strOfficeIdsXml, objFiscalYear.FiscalYearID, null, dtAsOfDate, -1, null, Core.NPSCommon.Enums.SortFields.ExpenditureSortFields.DateOfTransaction, Core.NPSCommon.Enums.OrderByDirection.Ascending,rdoFilters).Items;


            var downloadGuid = new Guid();
            //Generate the NPS Report
            if (objFiscalYear.Year>=2023)
            {
                string reportType = AppSettings.ReportType_NPS_And_Training;
                if (rdoFilters == "0")
                    reportType = AppSettings.ReportType_NPS_And_Training;
                else if (rdoFilters == "1")
                    reportType = AppSettings.ReportType_NPS_Only;
                else if (rdoFilters == "2")
                    reportType = AppSettings.ReportType_Training_Only;

                var lstExpended = PurchaseOrderExpendedViewModel.GetAll(objFiscalYear.FiscalYearID, objOffice.OfficeID, reportType, dtAsOfDate);
                var lstObligated = PurchaseOrderObligatedViewModel.GetAll(objFiscalYear.FiscalYearID, objOffice.OfficeID, reportType, dtAsOfDate);
                var transactions = PurchaseOrderTransactionsViewModel.GetAll(objFiscalYear.FiscalYearID, objOffice.OfficeID, reportType, dtAsOfDate);
                downloadGuid = NPSReportV2Helper.GenerateNPSReport(objOffice, objFiscalYear, dtAsOfDate, lstExpenditures,lstExpended,lstObligated,transactions);
            }
            else
            {
                //Get all the Purchase orders for the selected office and fiscal year
                List<IDataHelper> lstPurchaseOrders = PurchaseOrder.GetAll_with_alternateOffices(string.Empty, objOffice.OfficeID, objFiscalYear.FiscalYearID, dtAsOfDate, -1, null, Core.NPSCommon.Enums.SortFields.PurchaseOrderSortField.DateOfTransaction, Core.NPSCommon.Enums.OrderByDirection.Ascending, rdoFilters).Items;
                downloadGuid = NPSReportHelper.GenerateNPSReport(objOffice, objFiscalYear, dtAsOfDate, lstExpenditures, lstPurchaseOrders);
            }
            
            return downloadGuid;
        }
    }
}