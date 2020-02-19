using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Helpers;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary.Utilities;
using PRIFACT.PRIFACTBase.ClosedXmlHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary
{
    public class ExpenditureSubCategoryReportSummarySheetHelper
    {
        internal static void GenerateCustomReportSheet(ExcelGenerator objExcelGenerator, List<Office> lstOffices, List<ExpenditureSubCategory> lstExpenditureSubCategories, DateTime dtAsOfDate, FiscalYear objFiscalYear, List<ExpenditureSubCategorySummaryHelper> lstExpenditures, DateTime? dtStartDate)
        {
            object oMissing = Type.Missing;

            string dates = string.Empty;
            if (dtStartDate == null)
                dates = objFiscalYear.StartDate.ToShortDateString() + " - " + dtAsOfDate.ToShortDateString();
            else
                dates = DateTime.Parse(dtStartDate.ToString()).ToShortDateString() + " - " + dtAsOfDate.ToShortDateString();

            _AddWorkSetSheet(objExcelGenerator, "NPS Report");
            string strCategoryWiseReportSheetHeader = "Summary of Expenditures (" + dates + ")";
            _SetHeaderforFixedCustomExpenditureSubCategoryReports(objExcelGenerator, strCategoryWiseReportSheetHeader, lstExpenditureSubCategories.Count + 3);
            _SetColumnHeading(objExcelGenerator, "OfficeName", "A2", "A2");
            _SetColumnHeading(objExcelGenerator, "IndexCode", "B2", "B2");
            int iColNumber = 3;

            iColNumber = _setColumnHeading(objExcelGenerator, lstExpenditureSubCategories, iColNumber);
            
            //Add Total header
            _SetReportColumnHeading(objExcelGenerator, "Total", iColNumber);
            objExcelGenerator.SetColumnWidth(MiscHelper.GetColumnNameFromNumber(iColNumber), 16);

            int iRowNumber = 3;
            
            foreach (Office objOffice in lstOffices)
            {
                iColNumber = 3;

                _SetOfficeCode(objExcelGenerator, objOffice, iRowNumber);
                _SetOfficeIndexCode(objExcelGenerator, objOffice, iRowNumber);

                Double grandTotal = 0;
                foreach (ExpenditureSubCategory item in lstExpenditureSubCategories)
                {
                    Double TotalAmount = 0;
                    
                    foreach (ExpenditureSubCategorySummaryHelper objExpenditure in lstExpenditures)
                    {
                        if (dtStartDate != null)
                        {
                            if (objExpenditure.OfficeID == objOffice.OfficeID)
                            {
                                if (objExpenditure.DateOfTransaction >= dtStartDate.Value && objExpenditure.DateOfTransaction <= dtAsOfDate)
                                {
                                    if (objExpenditure.ExpenditureSubCategoryID == item.ExpenditureSubCategoryID)
                                    {
                                        TotalAmount = TotalAmount + objExpenditure.Amount;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (objExpenditure.OfficeID == objOffice.OfficeID)
                            {
                                if (objExpenditure.DateOfTransaction <= dtAsOfDate)
                                {
                                    if (objExpenditure.ExpenditureSubCategoryID == item.ExpenditureSubCategoryID)
                                    {
                                        TotalAmount = TotalAmount + objExpenditure.Amount;
                                    }
                                }
                            }
                        }
                    }
                    
                    grandTotal += TotalAmount;
                    _SetCategoryWiseExpenditure(objExcelGenerator, TotalAmount, iColNumber, iRowNumber);
                    iColNumber++;

                }
                //Add Total as last column
                _SetCategoryWiseExpenditure(objExcelGenerator, grandTotal, iColNumber, iRowNumber);
                iRowNumber++;

            }

            //Add Total Expenditures row at the bottom
            int arrayCount = iColNumber;
            double[] totalExpenditures = new double[arrayCount];
            _SetTotalExpenditures(objExcelGenerator, "Total Expenditures", iRowNumber);

            for (int i = 3; i <= arrayCount; i++)
            {
                string strCellName = MiscHelper.GetColumnNameFromNumber(i);
                _SetRowLevelFormula(objExcelGenerator, strCellName,3, iRowNumber);
            }
            

            _SetColumnWidth(objExcelGenerator);

        }

        //public class ExpenditureSubCategorySummary
        //{
        //    public long ExpenditureSubCategoryID { get; set; }
        //    public long OfficeID { get; set; }
        //    public DateTime DateOfTransaction { get; set; }
        //    public Double Amount { get; set; }
        //}
        private static void _SetRowLevelFormula(ExcelGenerator objExcelGenerator, string columnName, int startingRowNumber, int iRowNumber)
        {
            string strFormula = "=SUM("+ columnName + startingRowNumber + ":" + columnName + (iRowNumber-1) + ")";
            string strCellNumber = columnName + iRowNumber;
            string strMergeEndCellNumber = columnName + iRowNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetFormula(strFormula, strCellNumber, blnShouldMerge, strMergeEndCellNumber);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.GetFixedAdhocReportCurrencyCellFormat());
        }
        private static void _SetTotalExpenditures(ExcelGenerator objExcelGenerator, string name, int iRowNumber)
        {
            {
                string strCellNumber = "A" + iRowNumber;
                string strEndMergeCellNumber = "B" + iRowNumber;
                bool blnShouldMerge = true;
                objExcelGenerator.SetText(name, strCellNumber, blnShouldMerge, strEndMergeCellNumber);
                objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetOfficeNameFieldsForFixedExpCategory());
                objExcelGenerator.SetCellFormat(strEndMergeCellNumber, CellFormatHelper.SetOfficeNameFieldsForFixedExpCategory());
            }
        }
        private static void _SetCategoryWiseExpenditure(ExcelGenerator objExcelGenerator, double TotalAmount, int iColNumber, int iRowNumber)
        {
            string strCellNumber = MiscHelper.GetColumnNameFromNumber(iColNumber) + iRowNumber;

            string strEndMergeCellNumber = strCellNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetText(TotalAmount.ToString(), strCellNumber, blnShouldMerge, strEndMergeCellNumber);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.GetCustomReportCurrencyCellFormat());
        }
        private static void _SetHeaderforFixedCustomExpenditureSubCategoryReports(ExcelGenerator objExcelGenerator, string strCategoryWiseReportSheetHeader, int iColNumber)
        {
            string strCellNumber = "A1";
            string strMergeEndCellNumber = MiscHelper.GetColumnNameFromNumber(iColNumber) + 2;

            bool blnShouldMerge = true;
            objExcelGenerator.SetText(strCategoryWiseReportSheetHeader, strCellNumber, blnShouldMerge, strMergeEndCellNumber);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetHeaderCellFormat());
        }

        private static void _AddWorkSetSheet(ExcelGenerator objExcelGenerator, string strExpenditureCategoryName)
        {
            string strFormat = "{0}";
            string strNPSReportSummarySheetPrefix = string.Empty;
            objExcelGenerator.AddWorkSheet(string.Format(strFormat, strExpenditureCategoryName));
        }

        private static void _SetColumnWidth(ExcelGenerator objExcelGenerator)
        {
            for (char c = 'A'; c <= 'Z'; c++)
            {
                //do something with letter 
                objExcelGenerator.SetColumnWidth(c.ToString(), 15);
            }
        }

        private static void _SetOfficeCode(ExcelGenerator objExcelGenerator, Office objOffice, int iRowNumber)
        {
            {
                string strCellNumber = "A" + iRowNumber;
                string strEndMergeCellNumber = strCellNumber;
                bool blnShouldMerge = false;
                objExcelGenerator.SetText(objOffice.Name, strCellNumber, blnShouldMerge, strEndMergeCellNumber);
                objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetOfficeNameFieldsForFixedExpCategory());
            }
        }
        private static void _SetOfficeIndexCode(ExcelGenerator objExcelGenerator, Office objOffice, int iRowNumber)
        {
            {
                string strCellNumber = "B" + iRowNumber;
                string strEndMergeCellNumber = strCellNumber;
                bool blnShouldMerge = false;
                objExcelGenerator.SetText(objOffice.IndexCode, strCellNumber, blnShouldMerge, strEndMergeCellNumber);
                objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetOfficeNameFieldsForFixedExpCategory());
            }
        }

        private static int _setColumnHeading(ExcelGenerator objExcelGenerator, List<ExpenditureSubCategory> lstExpenditureSubCategories, int iColNumber)
        {
            foreach (ExpenditureSubCategory item in lstExpenditureSubCategories)
            {
                _SetReportColumnHeading(objExcelGenerator, item.Name, iColNumber);
                iColNumber++;
            }
            return iColNumber;
        }

        private static void _SetReportColumnHeading(ExcelGenerator objExcelGenerator, string strText, int iColNumber)
        {
            int iRowNumber = 2;
            string strCellNumber = MiscHelper.GetColumnNameFromNumber(iColNumber) + iRowNumber;

            string strEndMergeCellNumber = strCellNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetText(strText, strCellNumber, blnShouldMerge, strEndMergeCellNumber);
            // objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetOfficeNameFieldsForFixedExpCategory());
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetColumnHeadingForFixedExpCategory());
        }

        private static void _SetColumnHeading(ExcelGenerator objExcelGenerator, string strText, string strcellNumber, string strMergeEndCellNumber)
        {
            string strStartCellNumber = strcellNumber;
            string strEndMergeCellNumber = strMergeEndCellNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetText(strText, strStartCellNumber, blnShouldMerge, strMergeEndCellNumber);
            objExcelGenerator.SetCellFormat(strcellNumber, CellFormatHelper.SetColumnHeadingForFixedExpCategory());
            //Set the font,bold
            //Set the border for the merged cells
            //Align text to centre
        }
    }
}
