using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.PRIFACTBase.ClosedXmlHelpers;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using System.IO;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary.Utilities;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.SortFields;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.ViewModels;

namespace PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary
{
    public class NPSReportV2Helper
    {
        public static Guid GenerateNPSReport(Office objOffice, FiscalYear objFiscalYear, DateTime dtAsOfDate, List<IDataHelper> lstExpenditures, List<PurchaseOrderExpendedViewModel> lstExpended, List<PurchaseOrderObligatedViewModel> lstObligated,List<PurchaseOrderTransactionsViewModel> transactions)
        {
            Guid downloadFileGuid = Guid.NewGuid();

            //File location to save the temporary file
            string newStrSaveFilePath = AppSettings.ExcelTempLocationPath + downloadFileGuid.ToString() + ".xlsx";

            using (ExcelGenerator objExcelGenerator = new ExcelGenerator())
            {
                //Generating the Summary and Transaction sheets
                _GenerateSummaryAndTransactionSheets(objExcelGenerator, objOffice, objFiscalYear, dtAsOfDate, lstExpenditures,lstExpended,lstObligated,transactions);
                objExcelGenerator.Save(newStrSaveFilePath);
            }
            return downloadFileGuid;
        }

        private static void _GenerateSummaryAndTransactionSheets(ExcelGenerator objExcelGenerator, Office objOffice, FiscalYear objFiscalYear, DateTime dtAsOfDate, List<IDataHelper> lstExpenditures, List<PurchaseOrderExpendedViewModel> lstExpended, List<PurchaseOrderObligatedViewModel> lstObligated, List<PurchaseOrderTransactionsViewModel> transactions)
        {
            //All the expenditures
            List<Expenditure> lstExpendituresConverted = lstExpenditures.ConvertAll(q => (Expenditure)q);

            ////All the purchase orders
            //List<PurchaseOrder> lstPurchaseOrderConverted = lstPurchaseOrders.ConvertAll(q => (PurchaseOrder)q);
            
            //Generating summary sheet
            NPSReportSummarySheetV2Helper.GenerateSummarySheet(objExcelGenerator, objOffice,objFiscalYear, dtAsOfDate, lstExpendituresConverted,lstExpended,lstObligated);

            //Generating transaction sheet
            NPSReportTransactionSheetV2Helper.GenerateTransactionSheet(objExcelGenerator, objOffice, objFiscalYear,dtAsOfDate, lstExpendituresConverted,transactions);


            ResultInfo resultInfo = ExpenditureCategory.GetAll(String.Empty, -1, -1, ExpenditureCategorySortFields.Name, OrderByDirection.Ascending);
            List<IDataHelper> lstExpenditureCategories = resultInfo.Items;

            List<ExpenditureCategory> lstExpenditureCategoryConverted = lstExpenditureCategories.ConvertAll(x => (ExpenditureCategory)x);
            var lstStaffLevelExpenditureCategories = lstExpenditureCategoryConverted.Where(q => q.IsStaffLevel == true);
            if (lstStaffLevelExpenditureCategories != null && lstStaffLevelExpenditureCategories.Count() > 0)
            {
                foreach (ExpenditureCategory objCategory in lstStaffLevelExpenditureCategories)
                {
                    NPSReportStaffLevelExpenditureSheetHelper.GenerateStaffLevelExpenditure(objCategory, objExcelGenerator, dtAsOfDate, lstExpendituresConverted.Where(q => q.ExpenditureCategoryID == objCategory.ExpenditureCategoryID).ToList(), objOffice, objFiscalYear);
                }
            }
        }

    }
}
