using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary.Utilities;
using PRIFACT.PRIFACTBase.ClosedXmlHelpers;

namespace PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary
{
    public class NPSReportStaffLevelExpenditureSheetHelper
    {
        internal static void GenerateStaffLevelExpenditure(ExpenditureCategory objExpenditureCategory, ExcelGenerator objExcelGenerator, DateTime dtAsOfDate, List<NPSDataHelper.Expenditure> lstExpenditures, NPSDataHelper.Office objOffice, FiscalYear objFiscalYear)
        {
            if (lstExpenditures != null && lstExpenditures.Count > 0)
            {
                Dictionary<long, int> dicStaffIdColumnIndex = new Dictionary<long, int>();
                List<IDataHelper> lstStaff = Staff.GetAllByOfficeId(objOffice.OfficeID, true, -1, null, NPSCommon.Enums.SortFields.StaffSortFields.FirstName, NPSCommon.Enums.OrderByDirection.Ascending).Items;
                //Row number hard coded here
                int iRowNumber = 8;
                int iColIndex = 0;
                if (lstStaff != null && lstStaff.Count > 0)
                {
                    int staffCount = lstStaff.Count;
                    object oMissing = Type.Missing;
                    _AddWorkSheet(objExcelGenerator, objOffice, objExpenditureCategory);
                    string strNPSReportSummaryHeaderText = objOffice.GetAttribute("NPSReportSummaryHeaderText");
                    //Setting the Sheet Main header
                    _SetHeader(objExcelGenerator, strNPSReportSummaryHeaderText, staffCount);
                    //TODO: Move to AppSettings
                    //Setting the Sheet subheadings

                    _SetSubHeading(objExcelGenerator, "Non-Personal Services Spending Report FY " + objFiscalYear.Year.ToString(), staffCount);
                    _SetSubHeading2(objExcelGenerator, objExpenditureCategory.GetAttribute("StaffLevelSheetHeaderText"), staffCount);
                    _SetSubHeading3(objExcelGenerator, "As of " + DateHelper.GetShortDateString(dtAsOfDate), staffCount);


                    _SetDateColumnHeader(objExcelGenerator, iRowNumber);
                    dicStaffIdColumnIndex = _SetStaffHeaders(objExcelGenerator, lstStaff, iRowNumber, ref iColIndex);
                    _SetTotalHeader(objExcelGenerator, iColIndex, iRowNumber);
                    int iRowStartIndex = iRowNumber + 1;
                    foreach (Expenditure objExpenditure in lstExpenditures)
                    {
                        iRowNumber++;
                        _SetTransactionDate(objExcelGenerator, objExpenditure.DateOfTransaction, iRowNumber);
                        List<StaffLevelExpenditure> lstStaffLevelExpenditures = StaffLevelExpenditure.GetAllByExpenditureId(objExpenditure.ExpenditureID);
                        if (lstStaffLevelExpenditures != null && lstStaffLevelExpenditures.Count > 0)
                        {
                            foreach (StaffLevelExpenditure objStaffLevelExpenditure in lstStaffLevelExpenditures)
                            {
                                _SetStaffLevelExpenditure(objExcelGenerator, objStaffLevelExpenditure, dicStaffIdColumnIndex, iRowNumber);
                            }
                        }
                        _SetRowLevelFormula(objExcelGenerator, iRowNumber, iColIndex);
                    }
                    iRowNumber++;
                    if (lstExpenditures.Count < 13)
                    {
                        int x = 13 - lstExpenditures.Count;
                        for (int i = 0; i < x; i++)
                        {
                            _SetRowLevelFormula(objExcelGenerator, iRowNumber, iColIndex);
                            iRowNumber++;
                        }
                    }
                    _SetTotalFooter(objExcelGenerator, iRowNumber);
                    _SetFooterFormula(objExcelGenerator, iRowNumber, iColIndex, iRowStartIndex);
                    _SetColumnWidths(objExcelGenerator);
                }                
            }
           
        }
        private static void _SetColumnWidths(ExcelGenerator objExcelGenerator)
        {
            objExcelGenerator.SetColumnWidth("A", 20);
            objExcelGenerator.SetColumnWidth("B", 20);
            objExcelGenerator.SetColumnWidth("C", 20);
            objExcelGenerator.SetColumnWidth("D", 20);

        }
        private static void _AddWorkSheet(ExcelGenerator objExcelGenerator, Office objOffice, ExpenditureCategory objCategory)
        {
            string strFormat = "{0} {1}";
            string strNPSReportSummarySheetPrefix = string.Empty;
            strNPSReportSummarySheetPrefix = objOffice.GetAttribute("NPSReportSummarySheetPrefix");
            if (strNPSReportSummarySheetPrefix.Length > 15)
            {
                strNPSReportSummarySheetPrefix = strNPSReportSummarySheetPrefix.Substring(0, 15);
            }
            string strSuffix = objCategory.GetAttribute("TransactionsTypeName");
            objExcelGenerator.AddWorkSheet(string.Format(strFormat, strNPSReportSummarySheetPrefix, strSuffix));
        }
        private static void _SetFooterFormula(ExcelGenerator objExcelGenerator, int iRowNumber, int iColIndex, int iRowStartIndex)
        {
            for (int i = 2; i <= iColIndex; i++)
            {
                string strFormula = "=SUM(" + MiscHelper.GetColumnNameFromNumber(i) + iRowStartIndex + ":" + MiscHelper.GetColumnNameFromNumber(i) + (iRowNumber - 1) + ")";
                string strCellNumber = MiscHelper.GetColumnNameFromNumber(i) + iRowNumber;
                string strMergeEndCellNumber = MiscHelper.GetColumnNameFromNumber(i) + iRowNumber;
                bool blnShouldMerge = false;
                objExcelGenerator.SetFormula(strFormula, strCellNumber, blnShouldMerge, strMergeEndCellNumber);
                objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForPhoneFooterSum());
            }
        }
        private static void _SetTotalFooter(ExcelGenerator objExcelGenerator, int iRowNumber)
        {
            string strCellNumber = "A" + iRowNumber;
            string strMergeEndCellNumber = "A" + iRowNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetText("Total", strCellNumber, blnShouldMerge, strMergeEndCellNumber);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForFooter());
        }
        private static void _SetRowLevelFormula(ExcelGenerator objExcelGenerator, int iRowNumber, int colIndex)
        {
            string strFormula = "=SUM(B" + iRowNumber + ":" + MiscHelper.GetColumnNameFromNumber(colIndex - 1) + iRowNumber + ")";
            string strCellNumber = MiscHelper.GetColumnNameFromNumber(colIndex) + iRowNumber;
            string strMergeEndCellNumber = MiscHelper.GetColumnNameFromNumber(colIndex) + iRowNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetFormula(strFormula, strCellNumber, blnShouldMerge, strMergeEndCellNumber);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.GetCurrencyCellFormat());

        }
        private static void _SetTransactionDate(ExcelGenerator objExcelGenerator, DateTime dateTime, int iRowNumber)
        {
            string strCellNumber = "A" + iRowNumber;
            string strMergeEndCellNumber = "A" + iRowNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetText(DateHelper.GetShortDateString(dateTime), strCellNumber, blnShouldMerge, strMergeEndCellNumber);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetFirstTransactionSheetColumnValue());
        }
        private static void _SetStaffLevelExpenditure(ExcelGenerator objExcelGenerator, StaffLevelExpenditure objStaffLevelExpenditure, Dictionary<long, int> dicStaffIdColumnIndex, int iRowNumber)
        {
            if (dicStaffIdColumnIndex.ContainsKey(objStaffLevelExpenditure.StaffID))
            {
                int colIndex = dicStaffIdColumnIndex[objStaffLevelExpenditure.StaffID];
                string strCellNumber = MiscHelper.GetColumnNameFromNumber(colIndex) + iRowNumber;
                string strMergeEndCellNumber = MiscHelper.GetColumnNameFromNumber(colIndex) + iRowNumber;
                bool blnShouldMerge = false;
                objExcelGenerator.SetText(objStaffLevelExpenditure.Amount.ToString(), strCellNumber, blnShouldMerge, strMergeEndCellNumber);
                objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.GetCurrencyCellFormat());
            }            
        }
        private static void _SetTotalHeader(ExcelGenerator objExcelGenerator, int iColIndex, int iRowNumber)
        {
            string strCellNumber = MiscHelper.GetColumnNameFromNumber(iColIndex) + iRowNumber;
            string strMergeEndCellNumber = MiscHelper.GetColumnNameFromNumber(iColIndex) + iRowNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetText("Total", strCellNumber, blnShouldMerge, strMergeEndCellNumber);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForPhoneColumnHeaders());

        }
        private static Dictionary<long, int> _SetStaffHeaders(ExcelGenerator objExcelGenerator, List<IDataHelper> lstStaff, int iRowNumber, ref int colIndex)
        {
            List<Staff> lstStaffConverted = lstStaff.ConvertAll(q => (Staff)q);
            colIndex = 2;
            Dictionary<long, int> dictStaffIdColumnIndex = new Dictionary<long, int>();
            foreach (Staff objStaff in lstStaffConverted)
            {
                string strCellNumber = MiscHelper.GetColumnNameFromNumber(colIndex) + iRowNumber;
                string strMergeEndCellNumber = MiscHelper.GetColumnNameFromNumber(colIndex) + iRowNumber;
                bool blnShouldMerge = false;
                objExcelGenerator.SetText(objStaff.FirstName +" "+ objStaff.LastName, strCellNumber, blnShouldMerge, strMergeEndCellNumber);
                objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForPhoneColumnHeaders());
                dictStaffIdColumnIndex.Add(objStaff.StaffId, colIndex);
                colIndex++;
            }
            return dictStaffIdColumnIndex;
        }
        private static void _SetDateColumnHeader(ExcelGenerator objExcelGenerator, int iRowNumber)
        {
            string strCellNumber = "A" + iRowNumber;
            string strMergeEndCellNumber = "A" + iRowNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetText("Date", strCellNumber, blnShouldMerge, strMergeEndCellNumber);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForPhoneColumnHeaders());
        }
        private static void _SetHeader(ExcelGenerator objExcelGenerator, string strNPSReportSummaryHeaderText, int staffCount)
        {
            string strCellNumber = "A2";
            string strMergeEndCellNumber = MiscHelper.GetColumnNameFromNumber(staffCount + 2) + "2";
            bool blnShouldMerge = true;
            objExcelGenerator.SetText(strNPSReportSummaryHeaderText, strCellNumber, blnShouldMerge, strMergeEndCellNumber);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForPhoneHeaders());
            //Set the font,bold,Capitalize
            //Set the border for the merged cells
            //Align text to centre
        }
        private static void _SetSubHeading(ExcelGenerator objExcelGenerator, string strHeaderText, int staffCount)
        {
            string strCellNumber = "A3";
            string strMergeEndCellNumber = MiscHelper.GetColumnNameFromNumber(staffCount + 2) + "3";
            bool blnShouldMerge = true;
            objExcelGenerator.SetText(strHeaderText, strCellNumber, blnShouldMerge, strMergeEndCellNumber);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForPhoneHeaders());
            //TODO            
            //Set the font,bold
            //Set the border for the merged cells
            //Align text to centre
        }
        private static void _SetSubHeading2(ExcelGenerator objExcelGenerator, string strText, int staffCount)
        {
            string strCellNumber = "A4";
            string strMergeEndCellNumber = MiscHelper.GetColumnNameFromNumber(staffCount + 2) + "4";
            bool blnShouldMerge = true;
            objExcelGenerator.SetText(strText, strCellNumber, blnShouldMerge, strMergeEndCellNumber);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForPhoneHeaders());
            //Set the font,bold
            //Set the border for the merged cells
            //Align text to centre
        }
        private static void _SetSubHeading3(ExcelGenerator objExcelGenerator, string strText, int staffCount)
        {
            string strCellNumber = "A5";
            string strMergeEndCellNumber = MiscHelper.GetColumnNameFromNumber(staffCount + 2) + "5";
            bool blnShouldMerge = true;
            objExcelGenerator.SetText(strText, strCellNumber, blnShouldMerge, strMergeEndCellNumber);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForPhoneHeaders());
            //Set the font,bold
            //Set the border for the merged cells
            //Align text to centre
        }
    }
}
