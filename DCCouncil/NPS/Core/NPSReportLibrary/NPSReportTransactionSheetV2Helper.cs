using ClosedXML.Excel;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.ViewModels;
using PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary.Utilities;
using PRIFACT.PRIFACTBase.ClosedXmlHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary
{
    public class NPSReportTransactionSheetV2Helper
    {
        public static void GenerateTransactionSheet(ExcelGenerator objExcelGenerator, Office objOffice, FiscalYear objFiscalYear, DateTime dtAsOfDate, List<Expenditure> lstExpenditures, List<PurchaseOrderTransactionsViewModel> transactions)
        {
            _AddWorkSheet(objExcelGenerator, objOffice, dtAsOfDate);
         
            string strNPSTransactionSummaryHeaderText = "TRANSACTION DETAILS-" + objOffice.Name;
            //Setting the Sheet Main header
            _SetHeader(objExcelGenerator, strNPSTransactionSummaryHeaderText);
            //TODO: Move to AppSettings
            //Setting the Sheet subheadings
            _SetSubHeading(objExcelGenerator, " Non-Personal Services Transactions FY " + objFiscalYear.Year.ToString());
            _SetSubHeading2(objExcelGenerator, "As of " + DateHelper.GetShortDateString(dtAsOfDate));

            //Setting Column Headers-date,type,transactiondetails,amount
            _SetColumnHeading(objExcelGenerator, "DATE", "A6", "A6");
            _SetColumnHeading(objExcelGenerator, "TYPE", "B6", "B6");
            _SetColumnHeading(objExcelGenerator, "VENDOR NAME", "C6", "C6");
            _SetColumnHeading(objExcelGenerator, "DESCRIPTION", "D6", "D6");
            _SetColumnHeading(objExcelGenerator, "AMOUNT", "E6", "E6");
            _SetColumnHeading(objExcelGenerator, "TOTAL", "F6", "F6");

            //Row number hardcoded here.
            int iRowNumber = 6;
            iRowNumber += 2;
            int iStartingRowNumber = iRowNumber;

            //Setting Purchase orders      
            //var transactions = PurchaseOrderTransactionsViewModel.GetAll(objFiscalYear.FiscalYearID, objOffice.OfficeID, AppSettings.ReportType_NPS_Only, dtAsOfDate);
            var lstExpended = transactions.Where(s=>s.EntryType=="E").ToList();
            //Setting Expended purchase orders
            iRowNumber = _SetPurchaseOrderSummary(objExcelGenerator, lstExpended, iRowNumber, "Purchase Orders - Expended");

            var lstObligated = transactions.Where(s => s.EntryType == "O").ToList();
            iRowNumber = _SetPurchaseOrderSummary(objExcelGenerator, lstObligated, iRowNumber, "Purchase Orders - Obligated");

            //Set Recurring Transactions List
            List<IDataHelper> lstRecurringTransactions = RecurringTransactionForReport.GetByFiscalYearAndAsOfDateTransactions(objFiscalYear.FiscalYearID, dtAsOfDate, objOffice.OfficeID).Items;

            List<String> lstStarredComments = new List<string>();

            //Setting Expenditures
            lstStarredComments = _SetExpendituresSummary(objExcelGenerator, lstExpenditures, lstRecurringTransactions, ref iRowNumber);

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
                    string strMergeEndCellNumber = "F" + iRowNumber;

                    var cell = objExcelGenerator.ActiveSheet.Cell(strCellNumber);
                    cell.RichText.AddText(Count.ToString()).SetVerticalAlignment(XLFontVerticalTextAlignmentValues.Superscript);
                    cell.RichText.AddText(str);
                    cell.Style.Alignment.SetWrapText(true);
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
        private static void _SetColumnWidth(ExcelGenerator objExcelGenerator)
        {
            objExcelGenerator.SetColumnWidth("A", 15);
            objExcelGenerator.SetColumnWidth("B", 15);
            objExcelGenerator.SetColumnWidth("C", 25);
            objExcelGenerator.SetColumnWidth("D", 15);
            objExcelGenerator.SetColumnWidth("E", 15);
            objExcelGenerator.SetColumnWidth("F", 15);
        }
        private static int _SetPurchaseOrderSummary(ExcelGenerator objExcelGenerator, List<PurchaseOrderTransactionsViewModel> transactions, int iRowNumber, string strHeaderText)
        {
            //TODO split Expended and Obligated purchase orders
            int iStartingRowNumber = iRowNumber + 1;
            _SetPurchaseOrderSideHeader(objExcelGenerator, iRowNumber, strHeaderText);
            iRowNumber++;
            if (transactions != null && transactions.Count > 0)
            {
                foreach (var transaction in transactions)
                {
                    _SetPurchaseOrderList(objExcelGenerator, transaction, iRowNumber);
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
        private static void _SetPurchaseOrderList(ExcelGenerator objExcelGenerator, PurchaseOrderTransactionsViewModel transaction, int iRowNumber)
        {
            string strCellNumber = "A" + iRowNumber;
            bool blnShouldMerge = false;
            string strFormat = "{0}";
            objExcelGenerator.SetText(string.Format(strFormat, transaction.AccountingDate.ToShortDateString()), strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetFirstTransactionSheetColumnValue());
            //to add attribute to add TYPE
            strCellNumber = "B" + iRowNumber;
            objExcelGenerator.SetText(transaction.PONumber, strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetSecondTransactionSheetColumnValue());

            strCellNumber = "C" + iRowNumber;
            objExcelGenerator.SetText(transaction.VendorName, strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetThirdTransactionSheetColumnValue());

            strCellNumber = "D" + iRowNumber;
            if (!string.IsNullOrEmpty(transaction.PODescription))
            {
                objExcelGenerator.SetText(transaction.PODescription.ToString(), strCellNumber, blnShouldMerge, string.Empty);
            }
            else
            {
                objExcelGenerator.SetText(string.Empty, strCellNumber, blnShouldMerge, string.Empty);
            }
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetFourthTransactionSheetColumnValue());

            strCellNumber = "E" + iRowNumber;
            objExcelGenerator.SetText(transaction.Amount.ToString(), strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.GetCurrencyCellFormat());
        }
        private static void _SetPurchaseOrderSideHeader(ExcelGenerator objExcelGenerator, int iRowNumber, string strHeaderText)
        {
            string strCellNumber = "A" + iRowNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetText(strHeaderText, strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForExpenditureHeaders());

        }
        private static void _AddWorkSheet(ExcelGenerator objExcelGenerator, Office objOffice, DateTime dtAsOfDate)
        {
            string strFormat = "{0} {1}";
            string strNPSReportSummarySheetPrefix = string.Empty;
            strNPSReportSummarySheetPrefix = objOffice.GetAttribute("NPSReportSummarySheetPrefix");
            if (strNPSReportSummarySheetPrefix.Length > 15)
            {
                strNPSReportSummarySheetPrefix = strNPSReportSummarySheetPrefix.Substring(0, 15);
            }
            string strSuffix = "Transactions";
            objExcelGenerator.AddWorkSheet(string.Format(strFormat, strNPSReportSummarySheetPrefix, strSuffix));
        }
        private static void _SetHeader(ExcelGenerator objExcelGenerator, string strHeaderText)
        {
            string strCellNumber = "A2";
            string strMergeEndCellNumber = "E2";
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
            string strMergeEndCellNumber = "E3";
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

        private static void _SetObligatedRecurringTransactionSideHeader(ExcelGenerator objExcelGenerator, int iRowNumber, string HeaderText)
        {
            string strCellNumber = "A" + iRowNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetText(HeaderText, strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForExpenditureHeaders());
        }

        private static void _SetRecurringTransactionList(ExcelGenerator objExcelGenerator, RecurringTransactionForReport lstRecurringTransaction, int iRowNumber)
        {
            string strCellNumber = "A" + iRowNumber;
            bool blnShouldMerge = false;
            
            objExcelGenerator.SetText("NA", strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetFirstTransactionSheetColumnValue());

            strCellNumber = "B" + iRowNumber;
            objExcelGenerator.SetText(lstRecurringTransaction.RecurringCategory, strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetSecondTransactionSheetColumnValue());

            strCellNumber = "C" + iRowNumber;
            objExcelGenerator.SetText(lstRecurringTransaction.VendorName, strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetThirdTransactionSheetColumnValue());


            //if (objExpenditureCategory.AppendMonth)
            //{
            //    objExcelGenerator.SetText(string.Format("{0}-{1}", objExpenditure.ExpenditureCategory.GetAttribute("TransactionsTypeName"), objExpenditure.DateOfTransaction.ToString("MMM")), strCellNumber, blnShouldMerge, string.Empty);
            //    objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetSecondTransactionSheetColumnValue());
            //}
            //else
            //{
            //    objExcelGenerator.SetText(objExpenditure.ExpenditureCategory.GetAttribute("TransactionsTypeName"), strCellNumber, blnShouldMerge, string.Empty);
            //    objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetSecondTransactionSheetColumnValue());
            //}

            
            strCellNumber = "D" + iRowNumber;
            objExcelGenerator.SetText(lstRecurringTransaction.Description, strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetFourthTransactionSheetColumnValue());

            strCellNumber = "E" + iRowNumber;
            objExcelGenerator.SetText(lstRecurringTransaction.Amount.ToString(), strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.GetCurrencyCellFormat());
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
        private static List<string> _SetExpendituresSummary(ExcelGenerator objExcelGenerator, List<Expenditure> lstExpenditures, List<IDataHelper> lstRecurringTransactions, ref int iRowNumber)
        {
            //selecting distinct expenditure categories
            List<string> lstStarredComments = new List<string>();
            List<IDataHelper> lstExpenditureCategories = ExpenditureCategory.GetAll(string.Empty, -1, null, NPSCommon.Enums.SortFields.ExpenditureCategorySortFields.Name, NPSCommon.Enums.OrderByDirection.Ascending).Items;
            List<ExpenditureCategory> lstExpenditureCategoryConverted = lstExpenditureCategories.ConvertAll(x => (ExpenditureCategory)x);
            List<long> lstExpenditureCategoryIds = lstExpenditureCategoryConverted.Select(q => q.ExpenditureCategoryID).ToList();
            //Setting ExpenditureCategory Headers
            int iStarCount = 0;
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
                    //int iStarCount = 0;
                    lstExpendituresForEachCategory = lstExpendituresForEachCategory.ToList().OrderBy(q => q.DateOfTransaction).ToList();
                    foreach (Expenditure objExpenditure in lstExpendituresForEachCategory)
                    {


                        string strComment = string.Empty;

                        if (objExpenditure.ExpenditureCategory.Code.ToLower() == "tc" || objExpenditure.ExpenditureCategory.Code.ToLower() == "pc")
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

                //Set PCard Obligated Recurring Transaction
                if (categoryid == 12)
                {
                    iStartingRowNumber = iRowNumber + 1;

                    _SetObligatedRecurringTransactionSideHeader(objExcelGenerator, iRowNumber, "P Card Obligated");
                    iRowNumber++;

                    List<RecurringTransactionForReport> lstPCard = lstRecurringTransactions.Cast<RecurringTransactionForReport>().ToList();
                    lstPCard = lstPCard.FindAll(e => e.RecurringCategory == "PCard").ToList();

                    if (lstPCard != null && lstPCard.Count > 0)
                    {
                        foreach (RecurringTransactionForReport val in lstPCard)
                        {
                            //if(val.RecurringCategory=="PCard")
                            //{ 
                                _SetRecurringTransactionList(objExcelGenerator, val, iRowNumber);
                                iRowNumber++;
                            //}
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

                //Set Telephone Obligated Recurring Transaction
                if (categoryid == 15)
                {
                    iStartingRowNumber = iRowNumber + 1;

                    _SetObligatedRecurringTransactionSideHeader(objExcelGenerator, iRowNumber, "Telephone Obligated");
                    iRowNumber++;

                    List<RecurringTransactionForReport> lstTel = lstRecurringTransactions.Cast<RecurringTransactionForReport>().ToList();
                    lstTel = lstTel.FindAll(e=>e.RecurringCategory=="Phone").ToList();

                    if (lstTel != null && lstTel.Count > 0)
                    {
                        foreach (RecurringTransactionForReport vals in lstTel)
                        {
                            //if (vals.RecurringCategory == "Phone")
                            //{
                                _SetRecurringTransactionList(objExcelGenerator, vals, iRowNumber);
                                iRowNumber++;
                            //}
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
            string strCellNumber = "E" + (iRowNumber - 1).ToString();
            bool blnShouldMerge = false;
            objExcelGenerator.SetText("0.00", strCellNumber, blnShouldMerge, string.Empty);

        }
        private static void _SetBorderToLastEntryValue(ExcelGenerator objExcelGenerator, int iRowNumber)
        {
            string strCellNumber = "E" + (iRowNumber - 1).ToString();
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetBottomBorderToCell());
        }
        private static int _SetBorderAndFormula(int iRowNumber, int iStartingRowNumber, ExcelGenerator objExcelGenerator)
        {
            if (iRowNumber == iStartingRowNumber)
            {
                iRowNumber += 1; //Adding 1 blank
            }
            _SetBorderToLastEntryValue(objExcelGenerator, iRowNumber);
            _SetSumFormula(objExcelGenerator, "E" + iStartingRowNumber, "E" + (iRowNumber - 1), "F" + iRowNumber, false);
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
            string strCellNumber = "E" + iRowNumber;
            bool blnShouldMerge = false;
            //TODO Style
            string strNPSReportTotaFieldText = "TOTAL TRANSACTIONS";
            objExcelGenerator.SetText(strNPSReportTotaFieldText, strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForFooter());
            strCellNumber = "F" + (iRowNumber - 2);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetBottomBorderToCell());
            _SetSumFormula(objExcelGenerator, "F" + (isStartRowingNumber), "F" + (iRowNumber - 1), "F" + iRowNumber, true);
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
            string strSuffix = "Transactions";
            return string.Format(strFormat, strNPSReportSummarySheetPrefix, strSuffix);
        }
    }
}
