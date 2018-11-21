using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.PRIFACTBase.ClosedXmlHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary
{
    public class AdHocReportHelper
    {
        public static Guid GenerateAdHocReport(FiscalYear objFiscalYear,List<long> lstOfficeIds, List<long> lstExpenditureCategoryIds, DateTime dtAsOfDate, List<IDataHelper> lstExpenditures,bool blnIsYearWise,bool blnIsMonthWise,bool blnCustomReportSheet,DateTime? dtStartDate)
        {
            object oMissing = Type.Missing;
            //Gett all the offices selected
            List<Office> lstOffices = new List<Office>();
            foreach (long lOfficeId in lstOfficeIds)
            {
                lstOffices.Add(Office.GetByOfficeID(lOfficeId));
            }

            List<ExpenditureCategory> lstExpenditureCategories = new List<ExpenditureCategory>();
            foreach (long lExpenditureCategoryId in lstExpenditureCategoryIds)
            {
                lstExpenditureCategories.Add(ExpenditureCategory.GetByID(lExpenditureCategoryId));
            }

            Guid downloadFileGuid = Guid.NewGuid();
            string newStrSaveFilePath = AppSettings.ExcelTempLocationPath + downloadFileGuid.ToString() + ".xlsx";            
            using (ExcelGenerator objExcelGenerator = new ExcelGenerator())
            {
                _GenerateWorkBook(objExcelGenerator, lstOffices, lstExpenditureCategories, dtAsOfDate, lstExpenditures, objFiscalYear, blnIsYearWise, blnIsMonthWise, blnCustomReportSheet, dtStartDate);

                objExcelGenerator.Save(newStrSaveFilePath);
            }
            return downloadFileGuid;
        }

        private static void _GenerateWorkBook(ExcelGenerator objExcelGenerator, List<Office> lstOffices, List<ExpenditureCategory> lstExpenditureCategories, DateTime dtAsOfDate, List<IDataHelper> lstExpenditures, FiscalYear objFiscalYear,bool blnIsYearWise,bool blnIsMonthWise,bool blnCustomReportSheet,DateTime? dtStartDate)
        {
            foreach (ExpenditureCategory objExpenditureCategory in lstExpenditureCategories)
            {
                //var lstExpendituresCategoryWise = lstExpenditures.Where(q => q.ExpenditureCategoryID == objExpenditureCategory.ExpenditureCategoryID);
                if (objExpenditureCategory != null )
                {
                    //if(objExpenditureCategory.ExpenditureCategoryID!=16)
                        _GenerateSheets(objExcelGenerator, lstOffices, objExpenditureCategory, dtAsOfDate, objFiscalYear, lstExpenditures, blnIsYearWise, blnIsMonthWise, dtStartDate);
                    
                }
            }

            ////PO Block Starts
            //foreach(Office objOffice in lstOffices)
            //{
            //    //Get all the Purchase orders for the selected office and fiscal year
            //    List<IDataHelper> lstPurchaseOrders = PurchaseOrder.GetAll(string.Empty, objOffice.OfficeID, objFiscalYear.FiscalYearID, dtAsOfDate, -1, null, Core.NPSCommon.Enums.SortFields.PurchaseOrderSortField.DateOfTransaction, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items;

            //    //All the purchase orders
            //    List<PurchaseOrder> lstPurchaseOrderConverted = lstPurchaseOrders.ConvertAll(q => (PurchaseOrder)q);

            //    IncludePOInAdHocReport.GenerateTransactionSheet(objExcelGenerator, objOffice, objFiscalYear, dtAsOfDate, lstPurchaseOrderConverted);    
            //}
            ////PO Block Ends

            if (blnCustomReportSheet)
            {
                if (lstExpenditureCategories != null && lstExpenditureCategories.Count > 0)
                {
                    _GenerateCustomReport(objExcelGenerator, lstOffices, lstExpenditureCategories, dtAsOfDate, lstExpenditures, objFiscalYear, dtStartDate);

                }
            }


        }

        private static void _GenerateCustomReport(ExcelGenerator objExcelGenerator, List<Office> lstOffices, List<ExpenditureCategory> lstExpenditureCategories, DateTime dtAsOfDate, List<IDataHelper> lstExpenditures, FiscalYear objFiscalYear,DateTime? dtStartDate)
        {
            AdHocReportSummarySheetHelper.GenerateCustomReportSheet(objExcelGenerator, lstOffices, lstExpenditureCategories, dtAsOfDate, objFiscalYear, lstExpenditures, dtStartDate);
        }

        private static void _GenerateSheets(ExcelGenerator objExcelGenerator, List<Office> lstOffices, ExpenditureCategory objExpenditureCategory, DateTime dtAsOfDate, FiscalYear objFiscalYear, List<IDataHelper> lstExpendituresCategoryWise,bool blnIsYearWise,bool blnIsMonthWise,DateTime? dtStartDate)
        {
            if (objExpenditureCategory.IsFixed)
            {
                AdHocReportSummarySheetHelper.GenerateSummarySheetForFixedCategory(objExcelGenerator, lstOffices, objExpenditureCategory, dtAsOfDate, objFiscalYear, lstExpendituresCategoryWise,blnIsYearWise,blnIsMonthWise, dtStartDate);
            }
            else
            {
                AdHocReportSummarySheetHelper.GenerateSummarySheetForNonFixedCategory(objExcelGenerator, lstOffices, objExpenditureCategory, dtAsOfDate, objFiscalYear, lstExpendituresCategoryWise,blnIsYearWise,blnIsMonthWise, dtStartDate);
            }
        }
    }
}
