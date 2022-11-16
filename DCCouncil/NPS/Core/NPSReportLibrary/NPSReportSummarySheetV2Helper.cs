using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.ViewModels;
using PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary.Helper;
using PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary.Utilities;
using PRIFACT.PRIFACTBase.ClosedXmlHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary
{
    public static class NPSReportSummarySheetV2Helper
    {
        
        public static void GenerateSummarySheet(ExcelGenerator objExcelGenerator, Office objOffice, FiscalYear objFiscalYear, DateTime dtAsOfDate, List<Expenditure> lstExpenditures, List<PurchaseOrderExpendedViewModel> lstExpended, List<PurchaseOrderObligatedViewModel> lstObligated)
        {
            //Get all the Budgets for this office
            List<IDataHelper> lstBudgets = objOffice.Budgets(objFiscalYear.FiscalYearID);
            if (lstBudgets != null && lstBudgets.Count > 0)
            {
                List<Budget> lstBudgetsConverted = lstBudgets.ConvertAll(q => (Budget)q);
                _AddWorkSheet(objExcelGenerator, objOffice, dtAsOfDate);
                string strNPSReportSummaryHeaderText = objOffice.GetAttribute("NPSReportSummaryHeaderText");
                //Setting the Sheet Main header
                _SetHeader(objExcelGenerator, strNPSReportSummaryHeaderText);
                //TODO: Move to AppSettings
                //Setting the Sheet subheadings
                _SetSubHeading(objExcelGenerator, "Non-Personal Services Spending Report FY " + objFiscalYear.Year.ToString());
                _SetSubHeading2(objExcelGenerator, "As of " + DateHelper.GetShortDateString(dtAsOfDate));
                //Setting the Budget Entry
                int iRowNumber = _SetBudgetEntry(objExcelGenerator, lstBudgetsConverted);
                int budgetRowNumber = iRowNumber;
                //The Row number where the entries start are hard coded here, can be moved to app settings
                iRowNumber = iRowNumber + 2;

                //Setting the Expenditure Summary                
                iRowNumber = _SetExpendituresSummary(objExcelGenerator, lstExpenditures, lstExpended, iRowNumber);
            
                //Setting the Purchase Order Summary
                
                //List<RecurringDescriptionAndAmount> lstRecurringTransactions = new List<RecurringDescriptionAndAmount>;
                //ResultInfo lstRecurringTransactions = RecurringTransactions.GetByFiscalYearAndAsOfDate(objFiscalYear.FiscalYearID, dtAsOfDate, objOffice.OfficeID);

                List<IDataHelper> lstRecurringTransactions = RecurringTransactionForReport.GetByFiscalYearAndAsOfDate(objFiscalYear.FiscalYearID, dtAsOfDate, objOffice.OfficeID).Items;                
                iRowNumber = _SetObligatedPurchaseOrdersSummary(objExcelGenerator, lstObligated, lstRecurringTransactions, iRowNumber);
                _SetTotalFooter(objExcelGenerator, iRowNumber, objOffice, budgetRowNumber);
                //Setting the column widths for the summary sheet
                _SetColumnWidths(objExcelGenerator);
            }
        }

        private static void _AddWorkSheet(ExcelGenerator objExcelGenerator, Office objOffice, DateTime dtAsOfDate)
        {
            string strFormat = "{0}-{1}";
            string strNPSReportSummarySheetPrefix = string.Empty;
            strNPSReportSummarySheetPrefix = objOffice.GetAttribute("NPSReportSummarySheetPrefix");
            if (strNPSReportSummarySheetPrefix.Length > 15)
            {
                strNPSReportSummarySheetPrefix = strNPSReportSummarySheetPrefix.Substring(0, 15);
            }
            string strSuffix = DateHelper.GetMonthAndYear(dtAsOfDate);
            objExcelGenerator.AddWorkSheet(string.Format(strFormat, strNPSReportSummarySheetPrefix, strSuffix));
        }

        private static void _SetColumnWidths(ExcelGenerator objExcelGenerator)
        {
            objExcelGenerator.SetColumnWidth("A", 60);
            objExcelGenerator.SetColumnWidth("B", 18);
            objExcelGenerator.SetColumnWidth("C", 25);
        }

        private static int _SetExpendituresSummary(ExcelGenerator objExcelGenerator, List<Expenditure> lstExpenditures, List<PurchaseOrderExpendedViewModel> lstExpended, int iRowNumber)
        {
            //Setting the the Expenditure Header
            iRowNumber = _SetExpendituresHeader(objExcelGenerator, iRowNumber);

            int iStartingRowNumber = iRowNumber;
            iRowNumber++;                       

            if (lstExpended != null && lstExpended.Count > 0)
            {
                iRowNumber = _SetExpendedPurchaseOrderSummary(objExcelGenerator, iRowNumber, lstExpended);
            }
            //if (lstExpenditures != null && lstExpenditures.Count > 0)
            //{
            //    if (lstExpenditures.Where(q => q.ExpenditureCategory.IsFixed == false) != null)
            //    {
            //        iRowNumber = _SetNonOfficeLevelExpenditureSummary(objExcelGenerator, iRowNumber, lstExpenditures.Where(q => q.ExpenditureCategory.IsFixed == false).ToList());
            //    }
            //    if (lstExpenditures.Where(q => q.ExpenditureCategory.IsFixed == true) != null)
            //    {
            //        iRowNumber = _SetOfficeLevelExpenditureSummary(objExcelGenerator, iRowNumber, lstExpenditures);
            //    }
            //}
            //Set the border for the last entry and set the formula for the sum
            iRowNumber = _SetBorderAndFormula(iRowNumber, iStartingRowNumber, objExcelGenerator);
            return iRowNumber;
        }

        private static int _SetExpendedPurchaseOrderSummary(ExcelGenerator objExcelGenerator, int iRowNumber, List<PurchaseOrderExpendedViewModel> lstExpended)
        {
            if (lstExpended != null && lstExpended.Count > 0)
            {
                foreach (var keyValue in lstExpended)
                {
                    if (Math.Round(keyValue.ExpendedAmount, 2) != 0)
                    {
                        _SetPurchaseOrderSummary(objExcelGenerator, keyValue.VendorName, keyValue.ExpendedAmount, iRowNumber);
                        iRowNumber++;
                    }
                }
            }
            return iRowNumber;
        }

        private static int _SetObligatedPurchaseOrdersSummary(ExcelGenerator objExcelGenerator, List<PurchaseOrderObligatedViewModel> lstObligated, List<IDataHelper> lstRecurringTransactions, int iRowNumber)
        {
            //Seetting the Purchase order header                
            iRowNumber = _SetObligatedPurchaseOrdersHeader(objExcelGenerator, iRowNumber);
            int iPurchaseOrderStartRow = iRowNumber;
            if (lstObligated != null && lstObligated.Count > 0)
            {
                foreach (var keyValue in lstObligated)
                {
                    if (Math.Round(keyValue.ObligatedAmount, 2) != 0)
                    {
                        _SetPurchaseOrderSummary(objExcelGenerator, keyValue.VendorName, keyValue.ObligatedAmount, iRowNumber);
                        iRowNumber++;
                    }
                }
                
            }
            ////For Recurring Transactions
            //if (lstRecurringTransactions != null && lstRecurringTransactions.Count > 0)
            //{
            //    foreach (RecurringTransactionForReport val in lstRecurringTransactions)
            //    {
            //        _SetPurchaseOrderSummary(objExcelGenerator, val.VendorName, val.Amount, iRowNumber);
            //        iRowNumber++;
            //    }
            //}

            iRowNumber = _SetBorderAndFormula(iRowNumber, iPurchaseOrderStartRow, objExcelGenerator);
            return iRowNumber;
        }

        private static void _SetPurchaseOrderSummary(ExcelGenerator objExcelGenerator, string strVendorname, double dblVoucherAmt, int iRowNumber)
        {
            string strCellNumber = "A" + iRowNumber;
            bool blnShouldMerge = false;
            string strFormat = "{0}";
            objExcelGenerator.SetText(string.Format(strFormat, strVendorname), strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.GetIndentedGeneralTextCellFormat());

            strCellNumber = "B" + iRowNumber;
            objExcelGenerator.SetText(dblVoucherAmt.ToString(), strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.GetCurrencyCellFormat());

        }

        private static int _SetNonOfficeLevelExpenditureSummary(ExcelGenerator objExcelGenerator, int iRowNumber, List<Expenditure> lstNonPurchaseOrderExpenditures)
        {
            //Expenditures which are Non Office level
            List<Expenditure> lstNonOfficeLevelExpenditures = lstNonPurchaseOrderExpenditures.Where(q => q.ExpenditureCategory.IsFixed == false).ToList().OrderBy(q => q.DateOfTransaction).ToList();
            if (lstNonOfficeLevelExpenditures != null && lstNonOfficeLevelExpenditures.Count > 0)
            {
                List<Expenditure> lstNonOfficeLevelExpendituresFilteredList;               
                // when vendorstaff = false and vendorstaffandother =false ,ie while adding this type of expenditurecategory item,it can be assigned new vendor. 
                //Eg:P-Card Transactions
                lstNonOfficeLevelExpendituresFilteredList = lstNonOfficeLevelExpenditures.Where(q => (q.ExpenditureCategory.IsVendorStaff == false && q.ExpenditureCategory.IsVendorStaffAndOther == false)).OrderBy(q => q.DateOfTransaction).ToList();

                if (lstNonOfficeLevelExpendituresFilteredList != null && lstNonOfficeLevelExpendituresFilteredList.Count > 0)
                {
                    List<string> lstVendorNames = new List<string>();
                    List<VendorHelper> voucherVendorSum = new List<VendorHelper>();
                    List<Expenditure> lstStaffExpenditures = new List<Expenditure>();
                    _GetVendorHelpers(lstNonOfficeLevelExpendituresFilteredList, lstVendorNames, voucherVendorSum, lstStaffExpenditures,false);
                    _SetExpenditureSmmaryForVendorHelpers(objExcelGenerator, ref iRowNumber, ref voucherVendorSum);
                }

                // when vendorstaff = true and vendorstaffandother =false ,ie while adding this type of expenditurecategory item,it can be assigned to staff of that office only. 
                //Eg:Petty Cash Transactions
                lstNonOfficeLevelExpendituresFilteredList = lstNonOfficeLevelExpenditures.Where(q => (q.ExpenditureCategory.IsVendorStaff == true && q.ExpenditureCategory.IsVendorStaffAndOther == false)).OrderBy(q => q.DateOfTransaction).ToList();
                if (lstNonOfficeLevelExpendituresFilteredList != null && lstNonOfficeLevelExpendituresFilteredList.Count > 0)
                {
                    iRowNumber = _SetExpenditureSummaryForKeyValuePairs(objExcelGenerator, iRowNumber, lstNonOfficeLevelExpendituresFilteredList);
                }

                // when vendorstaff = true and vendorstaffandother =true ,ie while adding this type of expenditurecategory item,it can be assigned to staff of that office as well as can add a new vendor.
                //Eg:Direct Vouchers
                lstNonOfficeLevelExpendituresFilteredList = lstNonOfficeLevelExpenditures.Where(q => (q.ExpenditureCategory.IsVendorStaff == true && q.ExpenditureCategory.IsVendorStaffAndOther == true)).OrderBy(q => q.DateOfTransaction).ToList();
                if (lstNonOfficeLevelExpendituresFilteredList != null && lstNonOfficeLevelExpendituresFilteredList.Count > 0)
                {

                    List<string> lstVendorNames = new List<string>();
                    List<VendorHelper> voucherVendorSum = new List<VendorHelper>();
                    List<Expenditure> lstStaffExpenditures = new List<Expenditure>();
                    _GetVendorHelpers(lstNonOfficeLevelExpendituresFilteredList, lstVendorNames, voucherVendorSum, lstStaffExpenditures,true);
                    _SetExpenditureSmmaryForVendorHelpers(objExcelGenerator, ref iRowNumber, ref voucherVendorSum);
                    if (lstStaffExpenditures != null && lstStaffExpenditures.Count > 0)
                    {
                        iRowNumber = _SetExpenditureSummaryForKeyValuePairs(objExcelGenerator, iRowNumber, lstStaffExpenditures);
                    }                   
                }
            }
            return iRowNumber;
        }

        private static int _SetExpenditureSummaryForKeyValuePairs(ExcelGenerator objExcelGenerator, int iRowNumber, List<Expenditure> lstNonOfficeLevelExpendituresFilteredList)
        {
            var lstExpenditureKeyValuePairs = from q in lstNonOfficeLevelExpendituresFilteredList
                                              group q by q.VendorName into poBalVendorSumObj
                                              select new
                                              {
                                                  VendorName = poBalVendorSumObj.Key,
                                                  POBalAmtSum = poBalVendorSumObj.Sum(q => q.Amount)
                                              };

            if (lstExpenditureKeyValuePairs != null && lstExpenditureKeyValuePairs.Count() > 0)
            {
                foreach (var keyValue in lstExpenditureKeyValuePairs)
                {
                    if (Math.Round(keyValue.POBalAmtSum, 2) != 0)
                    {
                        _SetExpenditureSummaryList(objExcelGenerator, keyValue.POBalAmtSum, keyValue.VendorName, iRowNumber);
                        iRowNumber++;
                    }
                }
            }
            return iRowNumber;
        }

        private static void _SetExpenditureSmmaryForVendorHelpers(ExcelGenerator objExcelGenerator, ref int iRowNumber, ref List<VendorHelper> voucherVendorSum)
        {
            if (voucherVendorSum != null && voucherVendorSum.Count() > 0)
            {
                voucherVendorSum = voucherVendorSum.OrderBy(q => q.VendorName).ToList();
                foreach (var keyValue in voucherVendorSum)
                {
                    if (Math.Round(keyValue.Amount, 2) != 0)
                    {
                        _SetExpenditureSummaryList(objExcelGenerator, keyValue.Amount, keyValue.VendorName, iRowNumber);
                        iRowNumber++;
                    }
                }
            }
        }

        private static void _GetVendorHelpers(List<Expenditure> lstNonOfficeLevelExpendituresFilteredList, List<string> lstVendorNames, List<VendorHelper> voucherVendorSum,List<Expenditure> lstStaffExpenditures,bool blnAddToStaffExpenditures)
        {
            foreach (Expenditure objExpitem in lstNonOfficeLevelExpendituresFilteredList)
            {
                Vendor objVendor = Vendor.GetByNameAndOffice(objExpitem.VendorName,objExpitem.OfficeID);
                if (objVendor != null)
                {
                    if (!lstVendorNames.Contains(objVendor.Name))
                    {
                        if (objVendor.IsRolledUp)
                        {
                            List<Expenditure> lstSelected = lstNonOfficeLevelExpendituresFilteredList.Where(q => q.VendorName.ToUpper().Equals(objVendor.Name.ToUpper())).ToList();
                            double dblPOBalSum = lstSelected.Sum(q => q.Amount);
                            voucherVendorSum.Add(new VendorHelper(objVendor.Name, dblPOBalSum));
                            lstVendorNames.Add(objVendor.Name);
                        }
                        else
                        {
                            voucherVendorSum.Add(new VendorHelper(objVendor.Name, objExpitem.Amount));
                        }
                    }
                }
                else
                {
                    if (!blnAddToStaffExpenditures)
                    {
                        voucherVendorSum.Add(new VendorHelper(objExpitem.VendorName, objExpitem.Amount));
                    }
                    else
                    {
                        List<StaffLevelExpenditure> lstStaffLevelExp = objExpitem.StaffLevelExpenditures();
                        if (lstStaffLevelExp.Count != 0)
                        {
                            lstStaffExpenditures.Add(objExpitem);
                        }
                        else
                        {
                            voucherVendorSum.Add(new VendorHelper(objExpitem.VendorName, objExpitem.Amount));
                        }
                    }                    
                }
            }
        }

        private static void _SetExpenditureSummaryList(ExcelGenerator objExcelGenerator, double dblTotalExpAmount, string strVendorName, int iRowNumber)
        {
            string strCellNumber = "A" + iRowNumber;
            bool blnShouldMerge = false;
            string strFormat = "{0} {1}";
            objExcelGenerator.SetText(string.Format(strFormat, strVendorName, string.Empty), strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.GetIndentedGeneralTextCellFormat());

            //PfxTextControl objPfxTextControl = new PfxTextControl(false, false, true);
            //PfxTextAlignmnet objPfxTextAlignment = new PfxTextAlignmnet(PfxHorizontal.Left, PfxVertical.Bottom,  3);
            //PfxAlignment objPfxAlignment = new PfxAlignment(objPfxTextAlignment, objPfxTextControl);
            //PfxFont objPfxFont = new PfxFont("Calibri", System.Drawing.Color.Black, new PfxEffects(), PfxFontStyle.Regular, 11, false);
            //PfxBorder objPfxBorder = new PfxBorder();

            //PfxFill objPfxFill = new PfxFill();
            //string pfxNumberFormat = PfxNumberFormat.TEXT;

            //PfxCellFormat objCellFormat = new PfxCellFormat(pfxNumberFormat, objPfxAlignment, objPfxFont, objPfxBorder, objPfxFill);
            //objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.GetIndentedGeneralTextCellFormat());


            strCellNumber = "B" + iRowNumber;
            objExcelGenerator.SetText(dblTotalExpAmount.ToString(), strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.GetCurrencyCellFormat());

        }

        private static void _SetExpenditureSummary(ExcelGenerator objExcelGenerator, Expenditure objExpenditure, int iRowNumber)
        {
            string strCellNumber = "A" + iRowNumber;
            bool blnShouldMerge = false;
            string strFormat = "{0} ({1})";
            objExcelGenerator.SetText(string.Format(strFormat, objExpenditure.VendorName, objExpenditure.Description), strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.GetIndentedGeneralTextCellFormat());

            //PfxTextControl objPfxTextControl = new PfxTextControl(false, false, true);
            //PfxTextAlignmnet objPfxTextAlignment = new PfxTextAlignmnet(PfxHorizontal.Left, PfxVertical.Bottom,  3);
            //PfxAlignment objPfxAlignment = new PfxAlignment(objPfxTextAlignment, objPfxTextControl);
            //PfxFont objPfxFont = new PfxFont("Calibri", System.Drawing.Color.Black, new PfxEffects(), PfxFontStyle.Regular, 11, false);
            //PfxBorder objPfxBorder = new PfxBorder();

            //PfxFill objPfxFill = new PfxFill();
            //string pfxNumberFormat = PfxNumberFormat.TEXT;

            //PfxCellFormat objCellFormat = new PfxCellFormat(pfxNumberFormat, objPfxAlignment, objPfxFont, objPfxBorder, objPfxFill);
            //objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.GetIndentedGeneralTextCellFormat());


            strCellNumber = "B" + iRowNumber;
            objExcelGenerator.SetText(objExpenditure.Amount.ToString(), strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.GetCurrencyCellFormat());


        }

        #region Office Level Expenditure
        private static int _SetOfficeLevelExpenditureSummary(ExcelGenerator objExcelGenerator, int iRowNumber, List<Expenditure> lstNonPurchaseOrderExpenditures)
        {
            //Expenditures which are Office level
            List<Expenditure> lstOfficeLevelExpenditures = lstNonPurchaseOrderExpenditures.Where(q => q.ExpenditureCategory.IsFixed == true).ToList();
            List<long> lstExpenditureCategoryIds = lstOfficeLevelExpenditures.Select(q => q.ExpenditureCategory.ExpenditureCategoryID).Distinct().ToList();
            foreach (long lExpenditureCategoryId in lstExpenditureCategoryIds)
            {
                if (lstOfficeLevelExpenditures.Where(q => q.ExpenditureCategory.ExpenditureCategoryID == lExpenditureCategoryId) != null)
                {
                    //Expenditures split category wise
                    List<Expenditure> lstCategoryWise = lstOfficeLevelExpenditures.Where(q => q.ExpenditureCategory.ExpenditureCategoryID == lExpenditureCategoryId).ToList();
                    if (lstCategoryWise.Count > 0)
                    {
                        //Grouping the amount of an Office level expenditure
                        double dblAmount = _GetToalAmount(lstCategoryWise);
                        ExpenditureCategory objExpenditureCategory = lstCategoryWise[0].ExpenditureCategory;
                        //Getting the Group Name to be displayed
                        string strNPSReportSummaryGroupName = objExpenditureCategory.GetAttribute("NPSReportSummaryGroupName");
                        //Adding a row for each expenditure
                        __SetOfficeLevelExpenditureSummary(objExcelGenerator, strNPSReportSummaryGroupName, dblAmount, iRowNumber);
                        iRowNumber++;
                    }
                }
            }
            return iRowNumber;
        }

        private static void __SetOfficeLevelExpenditureSummary(ExcelGenerator objExcelGenerator, string strNPSReportSummaryGroupName, double dblAmount, int iRowNumber)
        {
            string strCellNumber = "A" + iRowNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetText(strNPSReportSummaryGroupName, strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.GetIndentedGeneralTextCellFormat());

            strCellNumber = "B" + iRowNumber;
            objExcelGenerator.SetText(dblAmount.ToString(), strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.GetCurrencyCellFormat());

        }

        private static double _GetToalAmount(List<Expenditure> lstCategoryWise)
        {
            double dblTotal = 0;
            foreach (Expenditure objExpenditure in lstCategoryWise)
            {
                dblTotal += objExpenditure.Amount;
            }
            return dblTotal;
        }
        #endregion

        private static int _SetBorderAndFormula(int iRowNumber, int iStartingRowNumber, ExcelGenerator objExcelGenerator)
        {
            if (iRowNumber == iStartingRowNumber)
            {
                iRowNumber += 4; //Adding 4 blankRows
            }
            _SetBorderToLastEntryValue(objExcelGenerator, iRowNumber);
            _SetSumFormula(objExcelGenerator, "B" + iStartingRowNumber, "B" + (iRowNumber - 1), "C" + iRowNumber, false, true);
            iRowNumber += 2; //Adding 2 blank Rows
            return iRowNumber;
        }

        private static void _SetTotalFooter(ExcelGenerator objExcelGenerator, int iRowNumber, Office objOffice, int isStartRowingNumber)
        {
            string strCellNumber = "B" + iRowNumber;
            bool blnShouldMerge = false;
            //TODO Style
            string strNPSReportTotaFieldText = objOffice.GetAttribute("NPSReportTotalFieldText");
            objExcelGenerator.SetText(strNPSReportTotaFieldText, strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForFooter());
            strCellNumber = "C" + (iRowNumber - 2);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetBottomBorderToCell());
            _SetSumFormula(objExcelGenerator, "C" + (isStartRowingNumber), "C" + (iRowNumber - 1), "C" + iRowNumber, true, false);
        }

        private static int _SetObligatedPurchaseOrdersHeader(ExcelGenerator objExcelGenerator, int iRowNumber)
        {
            string strCellNumber = "A" + iRowNumber;
            bool blnShouldMerge = false;
            //TODO Move to AppSettings,Style
            objExcelGenerator.SetText("Obligated Funds for Goods & Services Procured", strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForExpenditureHeaders());
            iRowNumber++;

            strCellNumber = "A" + iRowNumber;
            objExcelGenerator.SetText("Description", strCellNumber, blnShouldMerge, string.Empty);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForDescription());
            //Style - Underline
            iRowNumber++;
            return iRowNumber;
        }



        private static void _SetSumFormula(ExcelGenerator objExcelGenerator, string fromCell, string toCell, string sumCell, bool bInCurrencyFormat, bool blnMultiplyByMinusOne)
        {
            string strCellNumber = sumCell;
            bool blnShouldMerge = false;
            string strFormula = string.Empty;
            if (blnMultiplyByMinusOne)
            {
                //strFormula = "=-1*SUM(" + fromCell + ":" + toCell + ")";
                strFormula = "=-SUM(" + fromCell + ":" + toCell + ")";
            }
            else
            {
                strFormula = "=SUM(" + fromCell + ":" + toCell + ")";
            }
            objExcelGenerator.SetFormula(strFormula, strCellNumber, blnShouldMerge, string.Empty);

            //CellFormatHelperOld objCellFormatHelper = new CellFormatHelperOld();
            //objCellFormatHelper.FontSize = 11;
            //objCellFormatHelper.IsBold = true;

            //objCellFormatHelper.IsUnderline = false;
            //objCellFormatHelper.FontName = "Calibri";
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.GetCurrencyCellFormat());
            if (bInCurrencyFormat)
            {
                objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForAllocatedBudget());

            }
        }

        private static void _SetBorderToLastEntryValue(ExcelGenerator objExcelGenerator, int iRowNumber)
        {
            string strCellNumber = "B" + (iRowNumber - 1).ToString();
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetBottomBorderToCell());
        }

        private static int _SetExpendituresHeader(ExcelGenerator objExcelGenerator, int iRowNumber)
        {
            string strCellNumber = "A" + iRowNumber;
            bool blnShouldMerge = false;
            //TODO Move to AppSettings,Style
            objExcelGenerator.SetText("Expenditures for Goods & Services Procured", strCellNumber, blnShouldMerge, string.Empty);
            iRowNumber++;

            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForExpenditureHeaders());

            strCellNumber = "A" + iRowNumber;
            objExcelGenerator.SetText("Description", strCellNumber, blnShouldMerge, string.Empty);
            //Style - Underline

            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForDescription());
            return iRowNumber;
        }

        private static int _SetBudgetEntry(ExcelGenerator objExcelGenerator, List<Budget> lstBudgets)
        {
            int iRowNumber = 6;
            int iStartRowNumber = 6;
            string strCellNumber = string.Empty;
            string strMergeEndCellNumber = string.Empty;
            bool blnShouldMerge = true;
            if (lstBudgets.Count == 1)
            {
                strCellNumber = "A6";
                strMergeEndCellNumber = "B6";
                blnShouldMerge = true;
                objExcelGenerator.SetText("Non-Personal Services Budget", strCellNumber, blnShouldMerge, strMergeEndCellNumber);
                //TODO - Set the font,bold, Move to AppSettings

                objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForBudgetEntry());


                strCellNumber = "C6";
                strMergeEndCellNumber = "C6";
                blnShouldMerge = false;
                objExcelGenerator.SetText(lstBudgets[0].Amount.ToString(), strCellNumber, blnShouldMerge, strMergeEndCellNumber);
                //TODO Set Style   
                objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForAllocatedBudget());
            }
            else
            {
                foreach (Budget objBudget in lstBudgets)
                {
                    strCellNumber = "A" + iRowNumber;
                    strMergeEndCellNumber = "B" + iRowNumber;
                    blnShouldMerge = false;

                    if (objBudget.IsDefault)
                        objExcelGenerator.SetText(objBudget.Name, strCellNumber, blnShouldMerge, strMergeEndCellNumber);
                    //TODO - Set the font,bold, Move to AppSettings

                    else if (objBudget.IsDeduct)
                    {
                        string strFormat = "{0} :{1}";
                        objExcelGenerator.SetText(string.Format(strFormat, "Less", objBudget.Name), strCellNumber, blnShouldMerge, strMergeEndCellNumber);
                    }
                    else
                    {
                        string strFormat = "{0} :{1}";
                        objExcelGenerator.SetText(string.Format(strFormat, "Add", objBudget.Name), strCellNumber, blnShouldMerge, strMergeEndCellNumber);
                    }

                    objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForMultipleBudgetEntry());

                    strCellNumber = "B" + iRowNumber;
                    strMergeEndCellNumber = "B" + iRowNumber;
                    blnShouldMerge = false;
                    objExcelGenerator.SetText(objBudget.Amount.ToString(), strCellNumber, blnShouldMerge, strMergeEndCellNumber);
                    //TODO Set Style   
                    objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForMultipleAllocatedBudget());
                    iRowNumber = iRowNumber + 1;
                }


                strCellNumber = "A" + iRowNumber;
                strMergeEndCellNumber = "B" + iRowNumber;
                blnShouldMerge = true;
                objExcelGenerator.SetText("Non-Personal Services Budget:Revised", strCellNumber, blnShouldMerge, strMergeEndCellNumber);
                //TODO - Set the font,bold, Move to AppSettings

                objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForBudgetEntry());


                strCellNumber = "C" + iRowNumber;
                strMergeEndCellNumber = "C" + iRowNumber;
                blnShouldMerge = false;
                _SetSumFormula(objExcelGenerator, "B" + iStartRowNumber, "B" + (iRowNumber - 1), "C" + iRowNumber, false, false);
                //objExcelGenerator.SetText(lstBudgets[].Amount.ToString(), strCellNumber, blnShouldMerge, strMergeEndCellNumber);
                //TODO Set Style   
                objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetCellFormatForAllocatedBudget());



            }

            return iRowNumber; //Row in which the values where inserted
        }

        private static void _SetSubHeading2(ExcelGenerator objExcelGenerator, string strText)
        {
            string strCellNumber = "A4";
            string strMergeEndCellNumber = "C4";
            bool blnShouldMerge = true;
            objExcelGenerator.SetText(strText, strCellNumber, blnShouldMerge, strMergeEndCellNumber);
            //Set the font,bold
            //Set the border for the merged cells
            //Align text to centre

            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetSubHeading2CellFormat());
        }

        private static void _SetSubHeading(ExcelGenerator objExcelGenerator, string strHeaderText)
        {
            string strCellNumber = "A3";
            string strMergeEndCellNumber = "C3";
            bool blnShouldMerge = true;
            objExcelGenerator.SetText(strHeaderText, strCellNumber, blnShouldMerge, strMergeEndCellNumber);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetSubHeaderCellFormat());

            //TODO            
            //Set the font,bold
            //Set the border for the merged cells
            //Align text to centre
        }

        private static void _SetHeader(ExcelGenerator objExcelGenerator, string strHeaderText)
        {

            string strCellNumber = "A2";
            string strMergeEndCellNumber = "C2";
            bool blnShouldMerge = true;
            objExcelGenerator.SetText(strHeaderText, strCellNumber, blnShouldMerge, strMergeEndCellNumber);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetHeaderCellFormat());
            //Set the font,bold,Capitalize
            //Set the border for the merged cells
            //Align text to centre
        }

        private static string _GetSheetName(Office objOffice, DateTime dtAsOfDate, Budget objBudget)
        {
            string strFormat = "{0}-{1}";
            string strNPSReportSummarySheetPrefix = string.Empty;
            strNPSReportSummarySheetPrefix = objOffice.GetAttribute("NPSReportSummarySheetPrefix");
            if (strNPSReportSummarySheetPrefix.Length > 15)
            {
                strNPSReportSummarySheetPrefix = strNPSReportSummarySheetPrefix.Substring(0, 15);
            }
            string strSuffix = DateHelper.GetMonthAndYear(dtAsOfDate);
            return string.Format(strFormat, strNPSReportSummarySheetPrefix, strSuffix);
        }
    }
}
