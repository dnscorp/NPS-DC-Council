using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary.Utilities;
using PRIFACT.PRIFACTBase.ClosedXmlHelpers;
using ClosedXML.Excel;

namespace PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary
{
    class IncludePOInAdHocReport
    {
        public static void GenerateTransactionSheet(ExcelGenerator objExcelGenerator, FiscalYear objFiscalYear, DateTime dtAsOfDate, List<PurchaseOrder> lstPurchaseOrders)
        {
            _AddWorkSheet(objExcelGenerator, dtAsOfDate);

            //string strNPSTransactionSummaryHeaderText = "TRANSACTION DETAILS-" + objOffice.Name;
            //Setting the Sheet Main header
            //_SetHeader(objExcelGenerator, strNPSTransactionSummaryHeaderText);
            //TODO: Move to AppSettings
            //Setting the Sheet subheadings
            _SetSubHeading(objExcelGenerator, " FY " + objFiscalYear.Year.ToString() + " Purchase Orders");
            //_SetSubHeading2(objExcelGenerator, "As of " + DateHelper.GetShortDateString(dtAsOfDate));

            //Setting Column Headers-date,type,transactiondetails,amount
            _SetColumnHeading(objExcelGenerator, "DATE", "A5", "A5");
            _SetColumnHeading(objExcelGenerator, "TYPE", "B5", "B5");
            _SetColumnHeading(objExcelGenerator, "VENDOR NAME", "C5", "C5");
            _SetColumnHeading(objExcelGenerator, "OBJ", "D5", "D5");
            _SetColumnHeading(objExcelGenerator, "INDEX", "E5", "E5");
            _SetColumnHeading(objExcelGenerator, "DESCRIPTION", "F5", "F5");
            _SetColumnHeading(objExcelGenerator, "AMOUNT", "G5", "G5");
            _SetColumnHeading(objExcelGenerator, "TOTAL", "H5", "H5");

            //Row number hardcoded here.
            int iRowNumber = 5;
            iRowNumber += 2;
            int iStartingRowNumber = iRowNumber;

            //Setting Purchase orders      
            List<PurchaseOrder> lstExpendedPurchaseOrders = _GetExpendedPurchaseOrders(lstPurchaseOrders);
            //Setting Expended purchase orders
            iRowNumber = _SetPurchaseOrderSummary(objExcelGenerator, lstExpendedPurchaseOrders, iRowNumber, "Purchase Orders - Expended", true);

            List<PurchaseOrder> lstObligatedPurchaseOrders = _GetObligatedPurchaseOrders(lstPurchaseOrders);
            iRowNumber = _SetPurchaseOrderSummary(objExcelGenerator, lstObligatedPurchaseOrders, iRowNumber, "Purchase Orders - Obligated", false);

            List<String> lstStarredComments = new List<string>();

            _SetTotalFooter(objExcelGenerator, iRowNumber, iStartingRowNumber);

            if (lstStarredComments.Count > 0)
            {
                _SetStarredComments(objExcelGenerator, iRowNumber + 2, iStartingRowNumber, lstStarredComments);
            }
            //Setting the column widths
            _SetColumnWidth(objExcelGenerator);

        }

        public static void GenerateTransactionSheetMonthly(ExcelGenerator objExcelGenerator, FiscalYear objFiscalYear, DateTime dtAsOfDate, List<PurchaseOrder> lstPurchaseOrders, DateTime dtlocAsOfDate)
        {
            string strAsOfDate = dtlocAsOfDate.ToString("MMM");

            //_AddWorkSheetMonthly(objExcelGenerator, objOffice, dtAsOfDate, strAsOfDate);
            _AddWorkSheetMonthly(objExcelGenerator, dtAsOfDate, strAsOfDate);

            //string strNPSTransactionSummaryHeaderText = "TRANSACTION DETAILS-"; //+ objOffice.Name;
            //Setting the Sheet Main header
            //_SetHeader(objExcelGenerator, strNPSTransactionSummaryHeaderText);
            //TODO: Move to AppSettings
            //Setting the Sheet subheadings
           // _SetSubHeading(objExcelGenerator, " Non-Personal Services Transactions FY " + objFiscalYear.Year.ToString());
            //_SetSubHeading2(objExcelGenerator, "As of " + DateHelper.GetShortDateString(dtAsOfDate));

            //Setting Column Headers-date,type,transactiondetails,amount
            _SetColumnHeading(objExcelGenerator, "DATE", "A2", "A2");
            _SetColumnHeading(objExcelGenerator, "TYPE", "B2", "B2");
            _SetColumnHeading(objExcelGenerator, "VENDOR NAME", "C2", "C2");
            _SetColumnHeading(objExcelGenerator, "OBJ", "D2", "D2");
            _SetColumnHeading(objExcelGenerator, "INDEX", "E2", "E2");
            _SetColumnHeading(objExcelGenerator, "DESCRIPTION", "F2", "F2");
            _SetColumnHeading(objExcelGenerator, "AMOUNT", "G2", "G2");
            _SetColumnHeading(objExcelGenerator, "TOTAL", "H2", "H2");

            //Row number hardcoded here.
            int iRowNumber = 2;
            iRowNumber += 2;
            int iStartingRowNumber = iRowNumber;

            //Setting Purchase orders      
            List<PurchaseOrder> lstExpendedPurchaseOrders = _GetExpendedPurchaseOrdersMonthly(lstPurchaseOrders,dtAsOfDate);
            //Setting Expended purchase orders
            iRowNumber = _SetPurchaseOrderSummary(objExcelGenerator, lstExpendedPurchaseOrders, iRowNumber, "Purchase Orders - Expended", true);

            List<PurchaseOrder> lstObligatedPurchaseOrders = _GetObligatedPurchaseOrdersMonthly(lstPurchaseOrders,dtAsOfDate);
            iRowNumber = _SetPurchaseOrderSummary(objExcelGenerator, lstObligatedPurchaseOrders, iRowNumber, "Purchase Orders - Obligated", false);

            List<String> lstStarredComments = new List<string>();

            _SetTotalFooter(objExcelGenerator, iRowNumber, iStartingRowNumber);

            if (lstStarredComments.Count > 0)
            {
                _SetStarredComments(objExcelGenerator, iRowNumber + 2, iStartingRowNumber, lstStarredComments);
            }
            //Setting the column widths
            _SetColumnWidth(objExcelGenerator);

        }
        private static void _SetStarredComments(ExcelGenerator objExcelGenerator, int iRowNumber, int iStartingRowNumber, List<string> lstStarredComments)
        {
            int Count = 1;
            foreach (string str in lstStarredComments)
            {
                if (str != "From Import")
                {
                    string strCellNumber = "A" + iRowNumber;
                    string strMergeEndCellNumber = "H" + iRowNumber;

                    var cell = objExcelGenerator.ActiveSheet.Cell(strCellNumber);
                    cell.RichText.AddText(Count.ToString()).SetVerticalAlignment(XLFontVerticalTextAlignmentValues.Superscript);
                    cell.RichText.AddText(str);
                    //objExcelGenerator.SetText(str, strCellNumber, blnShouldMerge, strMergeEndCellNumber);
                    Count++;
                    var Range = objExcelGenerator.ActiveSheet.Range(strCellNumber, strMergeEndCellNumber);
                    Range.Merge();
                    Range.Value = cell;
                    iRowNumber++;
                }
                //cell.RichText.AddText(Count.ToString()).SetVerticalAlignment(XLFontVerticalTextAlignmentValues.Superscript);
                //cell.RichText.AddText(str);



                //objExcelGenerator.SetText(str, strCellNumber, blnShouldMerge, strMergeEndCellNumber);
                //objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForMultipleBudgetEntry());

            }
        }
        private static List<PurchaseOrder> _GetObligatedPurchaseOrders(List<PurchaseOrder> lstPurchaseOrders)
        {
            var lstVarObligatedPurchaseOrders = from q in lstPurchaseOrders
                                                where Math.Round(q.POBalSum, 2) != 0
                                                select q;
            List<PurchaseOrder> lstObligatedPurchaseOrders = new List<PurchaseOrder>();
            if (lstVarObligatedPurchaseOrders != null && lstVarObligatedPurchaseOrders.Count() > 0)
            {
                lstObligatedPurchaseOrders = lstVarObligatedPurchaseOrders.ToList();
            }
            return lstObligatedPurchaseOrders;
        }
        private static List<PurchaseOrder> _GetExpendedPurchaseOrders(List<PurchaseOrder> lstPurchaseOrders)
        {
            var lstVarExpendedPurchaseOrders = from q in lstPurchaseOrders
                                               where Math.Round(q.VoucherAmtSum, 2) != 0
                                               select q;
            List<PurchaseOrder> lstExpendedPurchaseOrders = new List<PurchaseOrder>();
            if (lstVarExpendedPurchaseOrders != null && lstVarExpendedPurchaseOrders.Count() > 0)
            {
                lstExpendedPurchaseOrders = lstVarExpendedPurchaseOrders.ToList();
            }
            return lstExpendedPurchaseOrders;
        }

        private static List<PurchaseOrder> _GetExpendedPurchaseOrdersMonthly(List<PurchaseOrder> lstPurchaseOrders, DateTime dtAsOfDate)
        {
            var lstVarExpendedPurchaseOrders = from q in lstPurchaseOrders
                                               where Math.Round(q.VoucherAmtSum, 2) != 0
                                               && q.DateOfTransaction<=dtAsOfDate.Date
                                               select q;
            List<PurchaseOrder> lstExpendedPurchaseOrders = new List<PurchaseOrder>();
            if (lstVarExpendedPurchaseOrders != null && lstVarExpendedPurchaseOrders.Count() > 0)
            {
                lstExpendedPurchaseOrders = lstVarExpendedPurchaseOrders.ToList();
            }
            return lstExpendedPurchaseOrders;
        }
        private static List<PurchaseOrder> _GetObligatedPurchaseOrdersMonthly(List<PurchaseOrder> lstPurchaseOrders, DateTime dtAsOfDate)
        {
            var lstVarObligatedPurchaseOrders = from q in lstPurchaseOrders
                                                where Math.Round(q.POBalSum, 2) != 0
                                                && q.DateOfTransaction<=dtAsOfDate.Date
                                                select q;
            List<PurchaseOrder> lstObligatedPurchaseOrders = new List<PurchaseOrder>();
            if (lstVarObligatedPurchaseOrders != null && lstVarObligatedPurchaseOrders.Count() > 0)
            {
                lstObligatedPurchaseOrders = lstVarObligatedPurchaseOrders.ToList();
            }
            return lstObligatedPurchaseOrders;
        }
        private static void _SetColumnWidth(ExcelGenerator objExcelGenerator)
        {
            objExcelGenerator.SetColumnWidth("A", 15);
            objExcelGenerator.SetColumnWidth("B", 15);
            objExcelGenerator.SetColumnWidth("C", 25);
            objExcelGenerator.SetColumnWidth("D", 15);
            objExcelGenerator.SetColumnWidth("E", 15);
            objExcelGenerator.SetColumnWidth("F", 15);
            objExcelGenerator.SetColumnWidth("G", 15);
            objExcelGenerator.SetColumnWidth("H", 15);
        }
        private static int _SetPurchaseOrderSummary(ExcelGenerator objExcelGenerator, List<PurchaseOrder> lstPurchaseOrders, int iRowNumber, string strHeaderText, bool blnIsExpended)
        {
            //TODO split Expended and Obligated purchase orders
            int iStartingRowNumber = iRowNumber + 1;
            _SetPurchaseOrderSideHeader(objExcelGenerator, iRowNumber, strHeaderText);
            iRowNumber++;
            if (lstPurchaseOrders != null && lstPurchaseOrders.Count > 0)
            {
                foreach (PurchaseOrder objPurchaseOrder in lstPurchaseOrders)
                {
                    _SetPurchaseOrderList(objExcelGenerator, objPurchaseOrder, iRowNumber, blnIsExpended);
                    iRowNumber++;
                }
            }
            else
            {
                iRowNumber++;
                _SetValueZero(objExcelGenerator, iRowNumber);
            }
            //Set the border for the last entry and set the formula for the sum
            iRowNumber = _SetBorderAndFormula(iRowNumber, iStartingRowNumber, objExcelGenerator);
            return iRowNumber;
        }
        private static void _SetPurchaseOrderList(ExcelGenerator objExcelGenerator, PurchaseOrder objPurchaseOrder, int iRowNumber, bool blnIsExpended)
        {
            string strCellNumber = "A" + iRowNumber;
            bool blnShouldMerge = false;
            string strFormat = "{0}";
            objExcelGenerator.SetText(string.Format(strFormat, objPurchaseOrder.DateOfTransaction.ToShortDateString()), strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetFirstTransactionSheetColumnValue());
            //to add attribute to add TYPE
            strCellNumber = "B" + iRowNumber;
            objExcelGenerator.SetText(objPurchaseOrder.PONumber, strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetSecondTransactionSheetColumnValue());

            strCellNumber = "C" + iRowNumber;
            objExcelGenerator.SetText(objPurchaseOrder.VendorName, strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetThirdTransactionSheetColumnValue());

            strCellNumber = "D" + iRowNumber;
            objExcelGenerator.SetText(objPurchaseOrder.OBJCode, strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetFourthTransactionSheetColumnValue());

            strCellNumber = "E" + iRowNumber;
            objExcelGenerator.SetText(objPurchaseOrder.Office.IndexCode, strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetFourthTransactionSheetColumnValue());

            strCellNumber = "F" + iRowNumber;
            if (objPurchaseOrder.PurchaseOrderDescription != null)
            {
                if (objPurchaseOrder.PurchaseOrderDescription.DescriptionText != null)
                {
                    objExcelGenerator.SetText(objPurchaseOrder.PurchaseOrderDescription.DescriptionText.ToString(), strCellNumber, blnShouldMerge, string.Empty);
                }
                else
                {
                    objExcelGenerator.SetText(string.Empty, strCellNumber, blnShouldMerge, string.Empty);
                }
            }
            else
            {
                objExcelGenerator.SetText(string.Empty, strCellNumber, blnShouldMerge, string.Empty);
            }
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetFourthTransactionSheetColumnValue());

            strCellNumber = "G" + iRowNumber;
            if (blnIsExpended)
            {
                objExcelGenerator.SetText(objPurchaseOrder.VoucherAmtSum.ToString(), strCellNumber, blnShouldMerge, string.Empty);
            }
            else
            {
                objExcelGenerator.SetText(objPurchaseOrder.POBalSum.ToString(), strCellNumber, blnShouldMerge, string.Empty);
            }
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.GetCurrencyCellFormat());
        }
        private static void _SetPurchaseOrderSideHeader(ExcelGenerator objExcelGenerator, int iRowNumber, string strHeaderText)
        {
            string strCellNumber = "A" + iRowNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetText(strHeaderText, strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForExpenditureHeaders());

        }
        private static void _AddWorkSheet(ExcelGenerator objExcelGenerator, DateTime dtAsOfDate)
        {
            string strFormat = "{0} {1}";
            string strNPSReportSummarySheetPrefix = string.Empty;

            //strNPSReportSummarySheetPrefix = objOffice.GetAttribute("NPSReportSummarySheetPrefix");
            //if (strNPSReportSummarySheetPrefix.Length > 15)
            //{
            //    strNPSReportSummarySheetPrefix = strNPSReportSummarySheetPrefix.Substring(0, 15);
           // }
            string strSuffix = "Purchase Orders-All";
            objExcelGenerator.AddWorkSheet(string.Format(strFormat, strNPSReportSummarySheetPrefix, strSuffix));
        }
        private static void _AddWorkSheetMonthly(ExcelGenerator objExcelGenerator, DateTime dtAsOfDate, string strAsOfDate)
        {
            string strFormat = "{0} {1}";
            string strNPSReportSummarySheetPrefix = string.Empty;

            //strNPSReportSummarySheetPrefix = objOffice.GetAttribute("NPSReportSummarySheetPrefix");
            if (strNPSReportSummarySheetPrefix.Length > 15)
            {
                strNPSReportSummarySheetPrefix = strNPSReportSummarySheetPrefix.Substring(0, 15);
            }
            string strSuffix = "Purchase Orders " + strAsOfDate ;
            objExcelGenerator.AddWorkSheet(string.Format(strFormat, strNPSReportSummarySheetPrefix, strSuffix));
        }
        private static void _SetHeader(ExcelGenerator objExcelGenerator, string strHeaderText)
        {
            string strCellNumber = "A2";
            string strMergeEndCellNumber = "H2";
            bool blnShouldMerge = true;
            objExcelGenerator.SetText(strHeaderText, strCellNumber, blnShouldMerge, strMergeEndCellNumber);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetHeaderCellFormat());
            //Set the font,bold,Capitalize
            //Set the border for the merged cells
            //Align text to centre
        }
        private static void _SetSubHeading(ExcelGenerator objExcelGenerator, string strHeaderText)
        {
            string strCellNumber = "A3";
            string strMergeEndCellNumber = "H3";
            bool blnShouldMerge = true;
            objExcelGenerator.SetText(strHeaderText, strCellNumber, blnShouldMerge, strMergeEndCellNumber);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetSubHeaderCellFormat());
            //TODO            
            //Set the font,bold
            //Set the border for the merged cells
            //Align text to centre
        }
        private static void _SetSubHeading2(ExcelGenerator objExcelGenerator, string strText)
        {
            string strCellNumber = "A4";
            string strMergeEndCellNumber = "E4";
            bool blnShouldMerge = true;
            objExcelGenerator.SetText(strText, strCellNumber, blnShouldMerge, strMergeEndCellNumber);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetSubHeading2CellFormat());
            //TODO     
            //Set the font,bold
            //Set the border for the merged cells
            //Align text to centre
        }
        private static void _SetColumnHeading(ExcelGenerator objExcelGenerator, string strText, string strcellNumber, string strMergeEndCellNumber)
        {
            string strStartCellNumber = strcellNumber;
            string strEndMergeCellNumber = strMergeEndCellNumber;
            bool blnShouldMerge = true;
            objExcelGenerator.SetText(strText, strStartCellNumber, blnShouldMerge, strMergeEndCellNumber);
            objExcelGenerator.SetCellFormat(strcellNumber, CellFormatHelper.SetColumnHeading());
            //Set the font,bold
            //Set the border for the merged cells
            //Align text to centre

        }
        private static void _SetExpenditureCategorySideHeader(ExcelGenerator objExcelGenerator, int iRowNumber, ExpenditureCategory objExp)
        {
            string strCellNumber = "A" + iRowNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetText(objExp.Name, strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForExpenditureHeaders());
        }
        private static void _SetExpenditureList(ExcelGenerator objExcelGenerator, Expenditure objExpenditure, ExpenditureCategory objExpenditureCategory, int iRowNumber, int iCommentCount, string strComment)
        {
            string strCellNumber = "A" + iRowNumber;
            bool blnShouldMerge = false;
            string strFormat = "{0}";
            if (!String.IsNullOrEmpty(strComment))
            {
                if (strComment != "From Import")
                {
                    var cell = objExcelGenerator.ActiveSheet.Cell(strCellNumber);
                    cell.RichText.AddText(iCommentCount.ToString()).SetVerticalAlignment(XLFontVerticalTextAlignmentValues.Superscript);
                    cell.RichText.AddText(objExpenditure.DateOfTransaction.ToShortDateString());
                }
                else
                    objExcelGenerator.SetText(string.Format(strFormat, objExpenditure.DateOfTransaction.ToShortDateString()), strCellNumber, blnShouldMerge, string.Empty);
                //objExcelGenerator.SetText(string.Format(strFormat, strComment + objExpenditure.DateOfTransaction.ToShortDateString()), strCellNumber, blnShouldMerge, string.Empty);
            }
            else
            {
                objExcelGenerator.SetText(string.Format(strFormat, objExpenditure.DateOfTransaction.ToShortDateString()), strCellNumber, blnShouldMerge, string.Empty);
            }
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetFirstTransactionSheetColumnValue());

            //to add attribute to add TYPE , objExpenditure.VendorName
            strCellNumber = "B" + iRowNumber;
            if (objExpenditureCategory.AppendMonth)
            {
                objExcelGenerator.SetText(string.Format("{0}-{1}", objExpenditure.ExpenditureCategory.GetAttribute("TransactionsTypeName"), objExpenditure.DateOfTransaction.ToString("MMM")), strCellNumber, blnShouldMerge, string.Empty);
                objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetSecondTransactionSheetColumnValue());
            }
            else
            {
                objExcelGenerator.SetText(objExpenditure.ExpenditureCategory.GetAttribute("TransactionsTypeName"), strCellNumber, blnShouldMerge, string.Empty);
                objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetSecondTransactionSheetColumnValue());
            }

            if (objExpenditureCategory.IsFixed)
            {
                strCellNumber = "C" + iRowNumber;
                objExcelGenerator.SetText(objExpenditure.ExpenditureCategory.GetAttribute("FixedVendorName"), strCellNumber, blnShouldMerge, string.Empty);
                objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetThirdTransactionSheetColumnValue());
            }
            else
            {
                strCellNumber = "C" + iRowNumber;
                objExcelGenerator.SetText(string.Format(strFormat, objExpenditure.VendorName), strCellNumber, blnShouldMerge, string.Empty);
                objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetThirdTransactionSheetColumnValue());
            }
            strCellNumber = "D" + iRowNumber;
            objExcelGenerator.SetText(objExpenditure.Description, strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetFourthTransactionSheetColumnValue());

            strCellNumber = "E" + iRowNumber;
            objExcelGenerator.SetText(objExpenditure.Amount.ToString(), strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.GetCurrencyCellFormat());
        }
        private static List<string> _SetExpendituresSummary(ExcelGenerator objExcelGenerator, List<Expenditure> lstExpenditures, ref int iRowNumber)
        {
            //selecting distinct expenditure categories
            List<string> lstStarredComments = new List<string>();
            List<IDataHelper> lstExpenditureCategories = ExpenditureCategory.GetAll(string.Empty, -1, null, NPSCommon.Enums.SortFields.ExpenditureCategorySortFields.Name, NPSCommon.Enums.OrderByDirection.Ascending).Items;
            List<ExpenditureCategory> lstExpenditureCategoryConverted = lstExpenditureCategories.ConvertAll(x => (ExpenditureCategory)x);
            List<long> lstExpenditureCategoryIds = lstExpenditureCategoryConverted.Select(q => q.ExpenditureCategoryID).ToList();
            //Setting ExpenditureCategory Headers
            foreach (long categoryid in lstExpenditureCategoryIds)
            {

                int iStartingRowNumber = iRowNumber + 1;
                ExpenditureCategory objCategory = lstExpenditureCategoryConverted.Where(q => q.ExpenditureCategoryID == categoryid).SingleOrDefault();
                if (objCategory != null)
                {
                    _SetExpenditureCategorySideHeader(objExcelGenerator, iRowNumber, objCategory);
                    iRowNumber++;
                }

                var lstExpendituresForEachCategory = lstExpenditures.Where(q => q.ExpenditureCategory.ExpenditureCategoryID == categoryid);
                if (lstExpendituresForEachCategory != null && lstExpendituresForEachCategory.Count() > 0)
                {
                    int iStarCount = 0;
                    lstExpendituresForEachCategory = lstExpendituresForEachCategory.ToList().OrderBy(q => q.DateOfTransaction).ToList();
                    foreach (Expenditure objExpenditure in lstExpendituresForEachCategory)
                    {


                        string strComment = string.Empty;

                        if (objExpenditure.ExpenditureCategory.Code.ToLower() == "tc")
                        {
                            if (!String.IsNullOrEmpty(objExpenditure.Comments))
                            {
                                //  strStarText = _GenerateHighlightforComments(iStarCount);

                                if (objExpenditure.Comments != "From Import")
                                {
                                    strComment = objExpenditure.Comments;
                                    lstStarredComments.Add(strComment);
                                    iStarCount++;
                                }




                            }
                        }
                        _SetExpenditureList(objExcelGenerator, objExpenditure, ExpenditureCategory.GetByID(categoryid), iRowNumber, iStarCount, strComment);
                        iRowNumber++;
                    }
                }
                else
                {
                    iRowNumber++;
                    _SetValueZero(objExcelGenerator, iRowNumber);
                }

                //Set the border for the last entry and set the formula for the sum
                iRowNumber = _SetBorderAndFormula(iRowNumber, iStartingRowNumber, objExcelGenerator);

            }
            return lstStarredComments;
        }

        private static string _GenerateHighlightforComments(int iStarCount)
        {

            char star = '*';
            string strstar = string.Empty;

            for (int i = 0; i < iStarCount; i++)
            {
                strstar = strstar + star.ToString();
            }
            return strstar;
        }
        private static void _SetValueZero(ExcelGenerator objExcelGenerator, int iRowNumber)
        {
            string strCellNumber = "G" + (iRowNumber - 1).ToString();
            bool blnShouldMerge = false;
            objExcelGenerator.SetText("0.00", strCellNumber, blnShouldMerge, string.Empty);

        }
        private static void _SetBorderToLastEntryValue(ExcelGenerator objExcelGenerator, int iRowNumber)
        {
            string strCellNumber = "G" + (iRowNumber - 1).ToString();
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetBottomBorderToCell());
        }
        private static int _SetBorderAndFormula(int iRowNumber, int iStartingRowNumber, ExcelGenerator objExcelGenerator)
        {
            if (iRowNumber == iStartingRowNumber)
            {
                iRowNumber += 1; //Adding 1 blank
            }
            _SetBorderToLastEntryValue(objExcelGenerator, iRowNumber);
            _SetSumFormula(objExcelGenerator, "G" + iStartingRowNumber, "G" + (iRowNumber - 1), "H" + iRowNumber, false);
            iRowNumber += 2; //Adding 2 blank Rows
            return iRowNumber;
        }
        private static void _SetSumFormula(ExcelGenerator objExcelGenerator, string fromCell, string toCell, string sumCell, bool bInCurrencyFormat)
        {
            string strCellNumber = sumCell;
            bool blnShouldMerge = false;
            string strFormula = "=1*SUM(" + fromCell + ":" + toCell + ")";

            objExcelGenerator.SetFormula(strFormula, strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.GetCurrencyCellFormat());
            if (bInCurrencyFormat)
                objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForAllocatedBudget());
        }
        private static void _SetTotalFooter(ExcelGenerator objExcelGenerator, int iRowNumber, int isStartRowingNumber)
        {
            //Range rngWorkSheetRange = null;
            string strCellNumber = "G" + iRowNumber;
            bool blnShouldMerge = false;
            //TODO Style
            string strNPSReportTotaFieldText = "TOTAL TRANSACTIONS";
            objExcelGenerator.SetText(strNPSReportTotaFieldText, strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForFooter());
            strCellNumber = "H" + (iRowNumber - 2);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetBottomBorderToCell());
            _SetSumFormula(objExcelGenerator, "H" + (isStartRowingNumber), "H" + (iRowNumber - 1), "H" + iRowNumber, true);
        }
        private static void _SetExpenditureSummary(ExcelGenerator objExcelGenerator, Expenditure objExpenditure, int iRowNumber)
        {
            string strCellNumber = "A" + iRowNumber;
            bool blnShouldMerge = false;
            string strFormat = "{0}";
            objExcelGenerator.SetText(string.Format(strFormat, objExpenditure.Description), strCellNumber, blnShouldMerge, string.Empty);

            strCellNumber = "B" + iRowNumber;
            objExcelGenerator.SetText(objExpenditure.Amount.ToString(), strCellNumber, blnShouldMerge, string.Empty);
        }
        private static string _GetSheetName(Office objOffice, DateTime dtAsOfDate, Budget objBudget)
        {
            string strFormat = "{0} {1}";
            string strNPSReportSummarySheetPrefix = string.Empty;
            strNPSReportSummarySheetPrefix = objOffice.GetAttribute("NPSReportSummarySheetPrefix");
            if (strNPSReportSummarySheetPrefix.Length > 15)
            {
                strNPSReportSummarySheetPrefix = strNPSReportSummarySheetPrefix.Substring(0, 15);
            }
            string strSuffix = "PO";
            return string.Format(strFormat, strNPSReportSummarySheetPrefix, strSuffix);
        }
    }
}
