using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Helpers;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.PRIFACTBase.ClosedXmlHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary
{
    public class ExpenditureSubCategoryReportHelper
    {
        public static Guid GenerateExpenditureSubCategoryReport(FiscalYear objFiscalYear, List<long> lstOfficeIds, List<long> lstExpenditureSubCategoryIds, DateTime dtAsOfDate, List<ExpenditureSubCategorySummaryHelper> lstExpenditures, DateTime? dtStartDate)
        {
            //Gett all the offices selected
            List<Office> lstOffices = new List<Office>();
            foreach (long lOfficeId in lstOfficeIds)
            {
                lstOffices.Add(Office.GetByOfficeID(lOfficeId));
            }

            List<ExpenditureSubCategory> lstExpenditureCategories = new List<ExpenditureSubCategory>();
            foreach (long lExpenditureSubCategoryId in lstExpenditureSubCategoryIds)
            {
                lstExpenditureCategories.Add(ExpenditureSubCategory.GetByID(lExpenditureSubCategoryId));
            }

            Guid downloadFileGuid = Guid.NewGuid();
            string newStrSaveFilePath = AppSettings.ExcelTempLocationPath + downloadFileGuid.ToString() + ".xlsx";
            using (ExcelGenerator objExcelGenerator = new ExcelGenerator())
            {
                _GenerateWorkBook(objExcelGenerator, lstOffices, lstExpenditureCategories, dtAsOfDate, lstExpenditures, objFiscalYear, dtStartDate);

                objExcelGenerator.Save(newStrSaveFilePath);
            }
            return downloadFileGuid;
        }

        private static void _GenerateWorkBook(ExcelGenerator objExcelGenerator, List<Office> lstOffices, List<ExpenditureSubCategory> lstExpenditureSubCategories, DateTime dtAsOfDate, List<ExpenditureSubCategorySummaryHelper> lstExpenditures, FiscalYear objFiscalYear, DateTime? dtStartDate)
        {
            if (lstExpenditureSubCategories != null && lstExpenditureSubCategories.Count > 0)
            {
                _GenerateCustomReport(objExcelGenerator, lstOffices, lstExpenditureSubCategories, dtAsOfDate, lstExpenditures, objFiscalYear, dtStartDate);

            }
            
        }

        private static void _GenerateCustomReport(ExcelGenerator objExcelGenerator, List<Office> lstOffices, List<ExpenditureSubCategory> lstExpenditureSubCategories, DateTime dtAsOfDate, List<ExpenditureSubCategorySummaryHelper> lstExpenditures, FiscalYear objFiscalYear, DateTime? dtStartDate)
        {
            ExpenditureSubCategoryReportSummarySheetHelper.GenerateCustomReportSheet(objExcelGenerator, lstOffices, lstExpenditureSubCategories, dtAsOfDate, objFiscalYear, lstExpenditures, dtStartDate);
        }

    }
}
