using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.Utilities
{
    public class ReportHelper
    {
        public static Guid GenerateNPSReport(Office objOffice,FiscalYear objFiscalYear,DateTime dtAsOfDate){                        

            //Generating the list of Office Ids, which is passed to get all the Expenditures
            List<long> lstOfficeIds = new List<long>();
            lstOfficeIds.Add(objOffice.OfficeID);
            string  strOfficeIdsXml = Office.GenerateXml(lstOfficeIds);

            //Get all the expenditures for the selected office and fiscal year
            List<IDataHelper> lstExpenditures = Expenditure.GetAll(string.Empty, strOfficeIdsXml, objFiscalYear.FiscalYearID, null, dtAsOfDate, -1, null, Core.NPSCommon.Enums.SortFields.ExpenditureSortFields.DateOfTransaction, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items;

            //Get all the Purchase orders for the selected office and fiscal year
            List<IDataHelper> lstPurchaseOrders = PurchaseOrder.GetAll_with_alternateOffices(string.Empty, objOffice.OfficeID, objFiscalYear.FiscalYearID, dtAsOfDate, -1, null, Core.NPSCommon.Enums.SortFields.PurchaseOrderSortField.DateOfTransaction, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items;

            //Generate the NPS Report
            Guid downloadGuid = NPSReportHelper.GenerateNPSReport(objOffice, objFiscalYear, dtAsOfDate, lstExpenditures,lstPurchaseOrders);
            return downloadGuid;
        }
    }
}