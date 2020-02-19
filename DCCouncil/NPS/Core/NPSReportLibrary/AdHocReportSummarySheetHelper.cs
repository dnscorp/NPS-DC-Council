using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRIFACT.PRIFACTBase.ClosedXmlHelpers;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.SortFields;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums;

namespace PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary
{
    public class AdHocReportSummarySheetHelper
    {

        internal static void GenerateSummarySheetForFixedCategory(ExcelGenerator objExcelGenerator, List<NPSDataHelper.Office> lstOffices, NPSDataHelper.ExpenditureCategory objExpenditureCategory, DateTime dtAsOfDate, NPSDataHelper.FiscalYear objFiscalYear, List<IDataHelper> lstExpenditures, bool blnIsYearWise, bool blnIsMonthWise, DateTime? dtStartDate)
        {
            //throw new NotImplementedException();
            if (blnIsYearWise || blnIsMonthWise)
                _GenerateCategoryWiseSummarySheet(objExcelGenerator, lstOffices, objExpenditureCategory, dtAsOfDate, objFiscalYear, lstExpenditures, blnIsYearWise, blnIsMonthWise, dtStartDate);
        }
        internal static void GenerateCustomReportSheet(ExcelGenerator objExcelGenerator, List<Office> lstOffices, List<ExpenditureCategory> lstExpenditureCategories, DateTime dtAsOfDate, FiscalYear objFiscalYear, List<IDataHelper> lstExpenditures, DateTime? dtStartDate)
        {
            object oMissing = Type.Missing;
            _AddWorkSetSheet(objExcelGenerator, "NPS Report");
            string strCategoryWiseReportSheetHeader = "NPS Report";
            _SetHeaderforFixedCustomAdhocReports(objExcelGenerator, strCategoryWiseReportSheetHeader, lstExpenditureCategories.Count+2);
            _SetColumnHeading(objExcelGenerator, "OfficeName", "A2", "A2");
            _SetColumnHeading(objExcelGenerator, "IndexCode", "B2", "B2");
            int iColNumber = 3;

            iColNumber = _setColumnHeading(objExcelGenerator, lstExpenditureCategories, iColNumber);

            int iRowNumber = 3;

            foreach (Office objOffice in lstOffices)
            {
                iColNumber = 3;

                _SetOfficeCode(objExcelGenerator, objOffice, iRowNumber);
                _SetOfficeIndexCode(objExcelGenerator, objOffice, iRowNumber);

                foreach (ExpenditureCategory item in lstExpenditureCategories)
                {
                    Double TotalAmount = 0;
                    Double TotalPOObligated = 0;

                    if (item.ExpenditureCategoryID != 16)
                    {
                        foreach (Expenditure objExpenditure in lstExpenditures)
                        {
                            if (dtStartDate != null)
                            {
                                if (objExpenditure.OfficeID == objOffice.OfficeID)
                                {
                                    if (objExpenditure.DateOfTransaction >= dtStartDate.Value && objExpenditure.DateOfTransaction <= dtAsOfDate)
                                    {
                                        if (objExpenditure.ExpenditureCategoryID == item.ExpenditureCategoryID)
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
                                        if (objExpenditure.ExpenditureCategoryID == item.ExpenditureCategoryID)
                                        {
                                            TotalAmount = TotalAmount + objExpenditure.Amount;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //PO Block Starts
                        //Get all the Purchase orders for the selected office and fiscal year
                        POSummaryResultInfo POresults;
                        POresults = PurchaseOrder.GetAllForAdhocPOSummary(string.Empty, objOffice.OfficeID, objFiscalYear.FiscalYearID, dtAsOfDate, dtStartDate, -1, null, Core.NPSCommon.Enums.SortFields.PurchaseOrderSortField.DateOfTransaction, Core.NPSCommon.Enums.OrderByDirection.Ascending);
                        TotalAmount = POresults.POExpended;

                        _SetHeaderforFixedCustomAdhocReports(objExcelGenerator, strCategoryWiseReportSheetHeader, lstExpenditureCategories.Count + 3);
                        _SetReportColumnHeading(objExcelGenerator, "Purchase Orders - Expended", iColNumber);
                        
                        TotalPOObligated = POresults.POObligated;
                        //PO Block Ends
                    }
                    _SetCategoryWiseExpenditure(objExcelGenerator, TotalAmount, iColNumber, iRowNumber);
                    iColNumber++;

                    if (TotalPOObligated != 0)
                    {
                        _SetReportColumnHeading(objExcelGenerator, "Purchase Orders - Obligated", iColNumber);
                        _SetCategoryWiseExpenditure(objExcelGenerator, TotalPOObligated, iColNumber, iRowNumber);
                        iColNumber++;
                    }

                }
                iRowNumber++;

            }

            _SetColumnWidth(objExcelGenerator);


        }

        private static void _SetHeaderforFixedCustomAdhocReports(ExcelGenerator objExcelGenerator, string strCategoryWiseReportSheetHeader, int iColNumber)
        {
            string strCellNumber = "A1";
            string strMergeEndCellNumber = MiscHelper.GetColumnNameFromNumber(iColNumber) + 2;

            bool blnShouldMerge = true;
            objExcelGenerator.SetText(strCategoryWiseReportSheetHeader, strCellNumber, blnShouldMerge, strMergeEndCellNumber);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetHeaderCellFormat());
        }

        private static void _SetCategoryWiseExpenditure(ExcelGenerator objExcelGenerator, double TotalAmount, int iColNumber, int iRowNumber)
        {
            string strCellNumber = MiscHelper.GetColumnNameFromNumber(iColNumber) + iRowNumber;

            string strEndMergeCellNumber = strCellNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetText(TotalAmount.ToString(), strCellNumber, blnShouldMerge, strEndMergeCellNumber);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.GetCustomReportCurrencyCellFormat());


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

        private static int _setColumnHeading(ExcelGenerator objExcelGenerator, List<ExpenditureCategory> lstExpenditureCategories, int iColNumber)
        {
            foreach (ExpenditureCategory item in lstExpenditureCategories)
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


        internal static void GenerateSummarySheetForNonFixedCategory(ExcelGenerator objExcelGenerator, List<NPSDataHelper.Office> lstOffices, NPSDataHelper.ExpenditureCategory objExpenditureCategory, DateTime dtAsOfDate, NPSDataHelper.FiscalYear objFiscalYear, List<IDataHelper> lstExpenditures, bool blnIsYearWise, bool blnIsMonthWise, DateTime? dtStartDate)
        {
            List<IDataHelper> lstMonthWiseExpenditures = null;
            if (blnIsYearWise)
            {
                if (objExpenditureCategory.ExpenditureCategoryID != 16)
                    _GenerateYearWiseSummarySheet(objExcelGenerator, lstOffices, objExpenditureCategory, dtAsOfDate, objFiscalYear, lstExpenditures, dtStartDate);
                else
                { 
                    //PO Block Starts
                    string officeIDs = "";
                    foreach (Office objOffice in lstOffices)
                    {
                        if (officeIDs == "")
                            officeIDs = objOffice.OfficeID.ToString();
                        else
                            officeIDs = officeIDs + "," + objOffice.OfficeID.ToString();
                    }
                
                        //Get all the Purchase orders for the selected office and fiscal year
                    List<IDataHelper> lstPurchaseOrders = PurchaseOrder.GetAllForAdhocPOYearly(string.Empty, officeIDs, objFiscalYear.FiscalYearID, dtAsOfDate, dtStartDate, -1, null, Core.NPSCommon.Enums.SortFields.PurchaseOrderSortField.DateOfTransaction, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items;

                        //All the purchase orders
                        List<PurchaseOrder> lstPurchaseOrderConverted = lstPurchaseOrders.ConvertAll(q => (PurchaseOrder)q);

                        IncludePOInAdHocReport.GenerateTransactionSheet(objExcelGenerator, objFiscalYear, dtAsOfDate, lstPurchaseOrderConverted);
                
                    //PO Block Ends
                }
            }
                
            DateTime dtlocAsOfDate = objFiscalYear.StartDate;
            TimeSpan dtTimeRange = dtAsOfDate.Subtract(objFiscalYear.StartDate);
            int iDateRange=((dtAsOfDate.Year - objFiscalYear.StartDate.Year) * 12) + dtAsOfDate.Month - objFiscalYear.StartDate.Month +1;
            if (blnIsMonthWise)
            {
                if (dtStartDate == null)
                {
                    if (dtAsOfDate >= objFiscalYear.StartDate)
                    {
                        for (int i = 1; i <= iDateRange; i++)
                        {
                            lstMonthWiseExpenditures = new List<IDataHelper>();
                            if (dtlocAsOfDate.Month == dtAsOfDate.Month)
                            {

                                foreach (Expenditure Item in lstExpenditures)
                                {

                                    if (Item.DateOfTransaction.Month == i)
                                    {
                                        if (Item.DateOfTransaction.Day <= dtAsOfDate.Day)
                                        {
                                            lstMonthWiseExpenditures.Add(Item);
                                        }
                                    }

                                }

                            }
                            else
                            {
                                foreach (Expenditure Item in lstExpenditures)
                                {
                                    if (Item.DateOfTransaction.Month == dtlocAsOfDate.Month)
                                    {
                                        lstMonthWiseExpenditures.Add(Item);
                                    }

                                }
                            }

                            string strAsOfDate = dtlocAsOfDate.ToString("MMM");
                            dtlocAsOfDate = dtlocAsOfDate.AddMonths(1);

                            if(objExpenditureCategory.ExpenditureCategoryID!=16)
                                _GenerateMonthWiseSummarySheet(objExcelGenerator, lstOffices, objExpenditureCategory, dtAsOfDate, objFiscalYear, lstMonthWiseExpenditures, strAsOfDate, dtStartDate);
                            else
                            { 
                                //PO Block Starts
                                string officeIDs="";
                                foreach (Office objOffice in lstOffices)
                                {
                                    if (officeIDs == "")
                                        officeIDs = objOffice.OfficeID.ToString();
                                    else
                                        officeIDs = officeIDs + "," + objOffice.OfficeID.ToString();
                                }
                                    //Get all the Purchase orders for the selected office and fiscal year
                                List<IDataHelper> lstPurchaseOrders = PurchaseOrder.GetAllForAdhocPOMonthly(string.Empty, officeIDs, objFiscalYear.FiscalYearID, dtAsOfDate, dtStartDate, dtlocAsOfDate.AddMonths(-1), -1, null, Core.NPSCommon.Enums.SortFields.PurchaseOrderSortField.DateOfTransaction, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items;

                                    //All the purchase orders
                                    List<PurchaseOrder> lstPurchaseOrderConverted = lstPurchaseOrders.ConvertAll(q => (PurchaseOrder)q);

                                    IncludePOInAdHocReport.GenerateTransactionSheetMonthly(objExcelGenerator, objFiscalYear, dtAsOfDate, lstPurchaseOrderConverted, dtlocAsOfDate.AddMonths(-1));
                            
                                //PO Block Ends
                            }
                        }
                        // Generate Monthly Sheet
                    }

                }
                else
                {
                    dtlocAsOfDate = dtStartDate.Value;
                    if (dtStartDate >= objFiscalYear.StartDate)
                    {
                        for (int i = dtStartDate.Value.Month; i <= dtAsOfDate.Month; i++)
                        {
                            lstMonthWiseExpenditures = new List<IDataHelper>();
                            if (i == dtAsOfDate.Month)
                            {

                                foreach (Expenditure Item in lstExpenditures)
                                {

                                    if (Item.DateOfTransaction.Month == i)
                                    {
                                        if (Item.DateOfTransaction.Day <= dtAsOfDate.Day)
                                        {
                                            lstMonthWiseExpenditures.Add(Item);
                                        }
                                    }

                                }

                            }
                            else
                            {
                                foreach (Expenditure Item in lstExpenditures)
                                {
                                    if (Item.DateOfTransaction.Month == i)
                                    {
                                        lstMonthWiseExpenditures.Add(Item);
                                    }

                                }
                            }

                            string strAsOfDate = dtlocAsOfDate.ToString("MMM");
                            dtlocAsOfDate = dtlocAsOfDate.AddMonths(1);

                            if (objExpenditureCategory.ExpenditureCategoryID != 16)
                                _GenerateMonthWiseSummarySheet(objExcelGenerator, lstOffices, objExpenditureCategory, dtAsOfDate, objFiscalYear, lstMonthWiseExpenditures, strAsOfDate, dtStartDate);
                            else
                            {
                                //PO Block Starts
                                string officeIDs = "";
                                foreach (Office objOffice in lstOffices)
                                {
                                    if (officeIDs == "")
                                        officeIDs = objOffice.OfficeID.ToString();
                                    else
                                        officeIDs = officeIDs + "," + objOffice.OfficeID.ToString();
                                }
                                //Get all the Purchase orders for the selected office and fiscal year
                                List<IDataHelper> lstPurchaseOrders = PurchaseOrder.GetAllForAdhocPOMonthly(string.Empty, officeIDs, objFiscalYear.FiscalYearID, dtAsOfDate, dtStartDate, dtlocAsOfDate.AddMonths(-1), -1, null, Core.NPSCommon.Enums.SortFields.PurchaseOrderSortField.DateOfTransaction, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items;

                                //All the purchase orders
                                List<PurchaseOrder> lstPurchaseOrderConverted = lstPurchaseOrders.ConvertAll(q => (PurchaseOrder)q);

                                IncludePOInAdHocReport.GenerateTransactionSheetMonthly(objExcelGenerator, objFiscalYear, dtAsOfDate, lstPurchaseOrderConverted, dtlocAsOfDate.AddMonths(-1));

                                //PO Block Ends
                            }
                        }
                        // Generate Monthly Sheet
                    }

                }

            }
        }
        private static void _GenerateCategoryWiseSummarySheet(ExcelGenerator objExcelGenerator, List<Office> lstOffices, ExpenditureCategory objExpenditureCategory, DateTime dtAsOfDate, FiscalYear objFiscalYear, List<IDataHelper> lstExpenditures, bool blnIsYearWise, bool blnIsMonthWise, DateTime? dtStartDate)
        {
            object oMissing = Type.Missing;
            int totalMonths;
            //objExcelGenerator.AddWorkSheet();
            if (dtStartDate == null)
            {
                // totalMonths = dtAsOfDate.Month - objFiscalYear.StartDate.Month;
                  totalMonths = ((dtAsOfDate.Year - objFiscalYear.StartDate.Year) * 12) + dtAsOfDate.Month - objFiscalYear.StartDate.Month + 1;
            }

            else
            {
                 totalMonths = dtAsOfDate.Month - dtStartDate.Value.Month;
                
            }
            totalMonths = totalMonths + 7;
            

            _AddWorkSetSheet(objExcelGenerator, objExpenditureCategory.Name);

            string strCategoryWiseReportSheetHeader = objFiscalYear.Name + " " + objExpenditureCategory.Name + " for NPS Accounting";
            _SetHeaderforFixedAdhocReports(objExcelGenerator, strCategoryWiseReportSheetHeader,totalMonths);
            _SetColumnHeading(objExcelGenerator, objExpenditureCategory.GetAttribute("VendorNameFieldText"), "A3", "A3");
            _SetColumnHeading(objExcelGenerator, "Description", "B3", "B3");
            _SetColumnHeading(objExcelGenerator, "OBJ", "C3", "C3");
            _SetColumnHeading(objExcelGenerator, "INDEX", "D3", "D3");
            _SetColumnHeading(objExcelGenerator, "PCA", "E3", "E3");
            _GenerateMonthNameColumnHeader(objExcelGenerator, dtAsOfDate, objFiscalYear, dtStartDate);

            _GenerateCategoryWiseReportEntry(objExcelGenerator, lstExpenditures, lstOffices, dtAsOfDate, objExpenditureCategory, objFiscalYear, dtStartDate);
            _SetColumnWidth(objExcelGenerator);

        }

        private static void _GenerateMonthNameColumnHeader(ExcelGenerator objExcelGenerator, DateTime dtAsOfDate, FiscalYear objFiscalYear, DateTime? dtEnteredDate)
        {
            int colindex = 6;
            DateTime dtStartDate;
            if (dtEnteredDate == null)
            {
                int totalMonths = ((dtAsOfDate.Year - objFiscalYear.StartDate.Year) * 12) + dtAsOfDate.Month - objFiscalYear.StartDate.Month + 1;
                dtStartDate = objFiscalYear.StartDate;
                for (int i = 1; i <= totalMonths; i++)
                {
                    string strCellNumber = MiscHelper.GetColumnNameFromNumber(colindex) + 3;
                    colindex++;
                    _SetColumnHeading(objExcelGenerator, dtStartDate.ToString("MMM"), strCellNumber, strCellNumber);
                    dtStartDate = dtStartDate.AddMonths(1);
                }
                string strNumber = MiscHelper.GetColumnNameFromNumber(colindex) + 3;
                _SetColumnHeading(objExcelGenerator, "TOTAL", strNumber, strNumber);


            }
            else
            {
                dtStartDate = dtEnteredDate.Value;
                for (int i = dtStartDate.Month; i <= dtAsOfDate.Month; i++)
                {
                    string strCellNumber = MiscHelper.GetColumnNameFromNumber(colindex) + 3;
                    colindex++;
                    _SetColumnHeading(objExcelGenerator, dtStartDate.ToString("MMM"), strCellNumber, strCellNumber);
                    dtStartDate = dtStartDate.AddMonths(1);
                }

                string strNumber = MiscHelper.GetColumnNameFromNumber(colindex) + 3;
                _SetColumnHeading(objExcelGenerator, "TOTAL", strNumber, strNumber);


            }



        }
        private static void _GenerateCategoryWiseReportEntry(ExcelGenerator objExcelGenerator, List<IDataHelper> lstExpenditures, List<Office> lstOffices, DateTime dtAsOfDate, ExpenditureCategory objExpenditureCategory, FiscalYear objFiscalYear, DateTime? dtEnteredDate)
        {
            int iRowNumber = 4;
            int colIndex = 6;
            int iDateRange;
            DateTime dtFiscalYearStartDate;
            if (dtEnteredDate == null)
            {
                dtFiscalYearStartDate = objFiscalYear.StartDate;
               
            }
            else
                dtFiscalYearStartDate = dtEnteredDate.Value;
            Double dblStaffLevelExpenditure = 0;

            foreach (Office objOffice in lstOffices)
            {
                dblStaffLevelExpenditure = 0;
                dtFiscalYearStartDate = objFiscalYear.StartDate;
                _SetOfficeName(objExcelGenerator, objOffice, iRowNumber);
                _SetINDEX(objExcelGenerator, objOffice, iRowNumber);
                _SetPCA(objExcelGenerator, objOffice, iRowNumber);
                _SetDescription(objExcelGenerator, objExpenditureCategory.GetAttribute("PageHeader"), iRowNumber);
                _setOBJ(objExcelGenerator, objExpenditureCategory.GetAttribute("FixedOBJCode"), iRowNumber);
                colIndex = 6;
                if (dtEnteredDate == null)
                {
                    iDateRange = ((dtAsOfDate.Year - objFiscalYear.StartDate.Year) * 12) + dtAsOfDate.Month - objFiscalYear.StartDate.Month + 1;
                    for (int i = 1; i <= iDateRange; i++)
                    {

                        foreach (Expenditure objExpenditure in lstExpenditures)
                        {

                            if (objExpenditure.ExpenditureCategoryID == objExpenditureCategory.ExpenditureCategoryID)
                            {
                                if (objExpenditure.OfficeID == objOffice.OfficeID)
                                {
                                    if (dtFiscalYearStartDate.Month == dtAsOfDate.Month)
                                    {
                                        if (objExpenditure.DateOfTransaction.Month == dtAsOfDate.Month)
                                        {
                                            if (objExpenditure.DateOfTransaction.Date <= dtAsOfDate.Date)
                                            {
                                                dblStaffLevelExpenditure = dblStaffLevelExpenditure + objExpenditure.Amount;
                                            }
                                        }

                                    }
                                    else if (objExpenditure.DateOfTransaction.Month == dtFiscalYearStartDate.Month)
                                    {
                                        dblStaffLevelExpenditure = dblStaffLevelExpenditure + objExpenditure.Amount;
                                    }
                                }
                            }
                        }
                        _SetMonthWiseExpenditure(objExcelGenerator, iRowNumber, colIndex, dblStaffLevelExpenditure);
                        dtFiscalYearStartDate = dtFiscalYearStartDate.AddMonths(1);
                        dblStaffLevelExpenditure = 0;
                        colIndex = colIndex + 1;

                    }
                }
                else
                {
                    dtFiscalYearStartDate = dtEnteredDate.Value;
                    for (int i = dtEnteredDate.Value.Month; i <= dtAsOfDate.Month; i++)
                    {
                        foreach (Expenditure objExpenditure in lstExpenditures)
                        {

                            if (objExpenditure.ExpenditureCategoryID == objExpenditureCategory.ExpenditureCategoryID)
                            {
                                if (objExpenditure.OfficeID == objOffice.OfficeID)
                                {

                                    if (objExpenditure.DateOfTransaction.Month == dtFiscalYearStartDate.Month)
                                    {
                                        dblStaffLevelExpenditure = dblStaffLevelExpenditure + objExpenditure.Amount;
                                    }
                                }
                            }
                        }
                        _SetMonthWiseExpenditure(objExcelGenerator, iRowNumber, colIndex, dblStaffLevelExpenditure);
                        dtFiscalYearStartDate = dtFiscalYearStartDate.AddMonths(1);
                        dblStaffLevelExpenditure = 0;
                        colIndex = colIndex + 1;

                    }
                }

                _SetRowLevelFormula(objExcelGenerator, iRowNumber, colIndex);
                iRowNumber++;
            }

        }

        private static void _SetRowLevelFormula(ExcelGenerator objExcelGenerator, int iRowNumber, int colIndex)
        {
            string strFormula = "=SUM(F" + iRowNumber + ":" + MiscHelper.GetColumnNameFromNumber(colIndex - 1) + iRowNumber + ")";
            string strCellNumber = MiscHelper.GetColumnNameFromNumber(colIndex) + iRowNumber;
            string strMergeEndCellNumber = MiscHelper.GetColumnNameFromNumber(colIndex) + iRowNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetFormula(strFormula, strCellNumber, blnShouldMerge, strMergeEndCellNumber);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.GetFixedAdhocReportCurrencyCellFormat());

        }

        private static void _SetPCA(ExcelGenerator objExcelGenerator, Office objOffice, int iRowNumber)
        {
            string strCellNumber = "E" + iRowNumber;
            string strEndMergeCellNumber = strCellNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetText(objOffice.PCA, strCellNumber, blnShouldMerge, strEndMergeCellNumber);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetOfficeNameFieldsForFixedExpCategory());
        }

        private static void _SetINDEX(ExcelGenerator objExcelGenerator, Office objOffice, int iRowNumber)
        {
            string strCellNumber = "D" + iRowNumber;
            string strEndMergeCellNumber = strCellNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetText(objOffice.IndexCode, strCellNumber, blnShouldMerge, strEndMergeCellNumber);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetOfficeNameFieldsForFixedExpCategory());
        }

        private static void _setOBJ(ExcelGenerator objExcelGenerator, string strOBJCode, int iRowNumber)
        {
            string strCellNumber = "C" + iRowNumber;
            string strEndMergeCellNumber = strCellNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetText(strOBJCode, strCellNumber, blnShouldMerge, strEndMergeCellNumber);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetOfficeNameFieldsForFixedExpCategory());
        }

        private static void _SetDescription(ExcelGenerator objExcelGenerator, string strDescription, int iRowNumber)
        {
            string strCellNumber = "B" + iRowNumber;
            string strEndMergeCellNumber = strCellNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetText(strDescription, strCellNumber, blnShouldMerge, strEndMergeCellNumber);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetOfficeNameFieldsForFixedExpCategory());
        }

        private static void _SetMonthWiseExpenditure(ExcelGenerator objExcelGenerator, int iRowNumber, int colIndex, double dblStaffLevelExpenditure)
        {
            string strCellNumber = MiscHelper.GetColumnNameFromNumber(colIndex) + iRowNumber;
            colIndex++;
            _SetMonthWiseExpenditure(objExcelGenerator, dblStaffLevelExpenditure.ToString(), strCellNumber, strCellNumber);
        }

        private static void _SetMonthWiseExpenditure(ExcelGenerator objExcelGenerator, string strText, string strcellNumber, string strMergeEndCellNumber)
        {
            string strStartCellNumber = strcellNumber;
            string strEndMergeCellNumber = strMergeEndCellNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetText(strText, strStartCellNumber, blnShouldMerge, strMergeEndCellNumber);
            objExcelGenerator.SetCellFormat(strcellNumber, CellFormatHelper.GetFixedAdhocReportCurrencyCellFormat());
        }

        private static void _SetOfficeName(ExcelGenerator objExcelGenerator, Office objOffice, int iRowNumber)
        {
            string strCellNumber = "A" + iRowNumber;
            string strEndMergeCellNumber = strCellNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetText(objOffice.Name, strCellNumber, blnShouldMerge, strEndMergeCellNumber);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetOfficeNameFieldsForFixedExpCategory());
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


        private static void _GenerateMonthWiseSummarySheet(ExcelGenerator objExcelGenerator, List<NPSDataHelper.Office> lstOffices, NPSDataHelper.ExpenditureCategory objExpenditureCategory, DateTime dtAsOfDate, NPSDataHelper.FiscalYear objFiscalYear, List<IDataHelper> lstExpenditures, string strSheetName, DateTime? dtStartDate)
        {
            object oMissing = Type.Missing;
            _AddWorkSetSheet(objExcelGenerator, objExpenditureCategory.Name + "-" + strSheetName);
            string strAdhocReportHeader = objFiscalYear.Name + " " + objExpenditureCategory.Name;
            _SetHeader(objExcelGenerator, strAdhocReportHeader);
            _SetColumnHeading(objExcelGenerator, objExpenditureCategory.GetAttribute("DateOfTransactionFieldText"), "A3", "A3");
            _SetColumnHeading(objExcelGenerator, objExpenditureCategory.GetAttribute("VendorNameFieldText"), "B3", "B3");
            _SetColumnHeading(objExcelGenerator, "Description", "C3", "C3");
            _SetColumnHeading(objExcelGenerator, "OBJ", "D3", "D3");
            _SetColumnHeading(objExcelGenerator, "INDEX", "E3", "E3");
            _SetColumnHeading(objExcelGenerator, "PCA", "F3", "F3");
            _SetColumnHeading(objExcelGenerator, "Amount", "G3", "G3");
            _SetColumnHeading(objExcelGenerator, "Comments", "H3", "H3");
            _GenerateAdhocReportEntry(objExcelGenerator, lstExpenditures, objExpenditureCategory, dtAsOfDate, dtStartDate);
            _SetColumnWidth(objExcelGenerator);
        }

        private static void _GenerateMonthWiseSummarySheetForPO(ExcelGenerator objExcelGenerator, List<NPSDataHelper.Office> lstOffices, NPSDataHelper.ExpenditureCategory objExpenditureCategory, DateTime dtAsOfDate, NPSDataHelper.FiscalYear objFiscalYear, List<IDataHelper> lstExpenditures, string strSheetName, DateTime? dtStartDate)
        {
            object oMissing = Type.Missing;
            _AddWorkSetSheet(objExcelGenerator, objExpenditureCategory.Name + "-" + strSheetName);
            string strAdhocReportHeader = objFiscalYear.Name + " " + objExpenditureCategory.Name;
            _SetHeader(objExcelGenerator, strAdhocReportHeader);

            //Setting Column Headers-date,type,transactiondetails,amount
            _SetColumnHeading(objExcelGenerator, "DATE", "A3", "A3");
            _SetColumnHeading(objExcelGenerator, "TYPE", "B3", "B3");
            _SetColumnHeading(objExcelGenerator, "VENDOR NAME", "C3", "C3");
            _SetColumnHeading(objExcelGenerator, "DESCRIPTION", "D3", "D3");
            _SetColumnHeading(objExcelGenerator, "AMOUNT", "E3", "E3");
            _SetColumnHeading(objExcelGenerator, "TOTAL", "F3", "F3");

            //_SetColumnHeading(objExcelGenerator, objExpenditureCategory.GetAttribute("DateOfTransactionFieldText"), "A3", "A3");
            //_SetColumnHeading(objExcelGenerator, objExpenditureCategory.GetAttribute("VendorNameFieldText"), "B3", "B3");
            //_SetColumnHeading(objExcelGenerator, "Description", "C3", "C3");
            //_SetColumnHeading(objExcelGenerator, "OBJ", "D3", "D3");
            //_SetColumnHeading(objExcelGenerator, "INDEX", "E3", "E3");
            //_SetColumnHeading(objExcelGenerator, "PCA", "F3", "F3");
            //_SetColumnHeading(objExcelGenerator, "Amount", "G3", "G3");
            //_SetColumnHeading(objExcelGenerator, "Comments", "H3", "H3");

            _GenerateAdhocReportEntry(objExcelGenerator, lstExpenditures, objExpenditureCategory, dtAsOfDate, dtStartDate);
            _SetColumnWidth(objExcelGenerator);
        }

        private static void _GenerateYearWiseSummarySheet(ExcelGenerator objExcelGenerator, List<NPSDataHelper.Office> lstOffices, NPSDataHelper.ExpenditureCategory objExpenditureCategory, DateTime dtAsOfDate, NPSDataHelper.FiscalYear objFiscalYear, List<IDataHelper> lstExpenditures, DateTime? dtStartDate)
        {
            object oMissing = Type.Missing;
            _AddWorkSetSheet(objExcelGenerator, objExpenditureCategory.Name + "-All");
            string strAdhocReportHeader = objFiscalYear.Name + " " + objExpenditureCategory.Name;
            _SetHeader(objExcelGenerator, strAdhocReportHeader);

            //Setting Column Headers-date,type,transactiondetails,amount
            //_SetColumnHeading(objExcelGenerator, "DATE", "A3", "A3");
            //_SetColumnHeading(objExcelGenerator, "TYPE", "B3", "B3");
            //_SetColumnHeading(objExcelGenerator, "VENDOR NAME", "C3", "C3");
            //_SetColumnHeading(objExcelGenerator, "DESCRIPTION", "D3", "D3");
            //_SetColumnHeading(objExcelGenerator, "AMOUNT", "E3", "E3");
            //_SetColumnHeading(objExcelGenerator, "TOTAL", "F3", "F3");

            _SetColumnHeading(objExcelGenerator, objExpenditureCategory.GetAttribute("DateOfTransactionFieldText"), "A3", "A3");
            _SetColumnHeading(objExcelGenerator, objExpenditureCategory.GetAttribute("VendorNameFieldText"), "B3", "B3");
            _SetColumnHeading(objExcelGenerator, "Description", "C3", "C3");
            _SetColumnHeading(objExcelGenerator, "OBJ", "D3", "D3");
            _SetColumnHeading(objExcelGenerator, "INDEX", "E3", "E3");
            _SetColumnHeading(objExcelGenerator, "PCA", "F3", "F3");
            _SetColumnHeading(objExcelGenerator, "Amount", "G3", "G3");
            _SetColumnHeading(objExcelGenerator, "Comments", "H3", "H3");


            _GenerateAdhocReportEntry(objExcelGenerator, lstExpenditures, objExpenditureCategory, dtAsOfDate, dtStartDate);
            _SetColumnWidth(objExcelGenerator);
        }

        private static void _SetColumnWidth(ExcelGenerator objExcelGenerator)
        {

            for (char c = 'A'; c <= 'R'; c++)
            {
                //do something with letter 
                objExcelGenerator.SetColumnWidth(c.ToString(), 12);
            }

        }

       
        private static void _GenerateAdhocReportEntry(ExcelGenerator objExcelGenerator, List<IDataHelper> lstExpenditures, ExpenditureCategory objExpenditureCategory, DateTime dtAsOfDate, DateTime? dtEnteredDate)
        {
            int iRowNumber = 4;
            if (dtEnteredDate == null)
            {
                if (lstExpenditures != null && lstExpenditures.Count > 0)
                {
                    foreach (Expenditure Item in lstExpenditures)
                    {
                        if (Item.DateOfTransaction.Date <= dtAsOfDate.Date)
                        {
                            if (Item.ExpenditureCategoryID == objExpenditureCategory.ExpenditureCategoryID)
                            {
                                _setTransactionDate(Item.DateOfTransaction.ToShortDateString(), objExcelGenerator, iRowNumber);
                                _setVendorName(Item.VendorName, objExcelGenerator, iRowNumber);
                                _setDescription(Item.Description, objExcelGenerator, iRowNumber);
                                _setOBJCode(Item.OBJCode, objExcelGenerator, iRowNumber);
                                _setIndex(Item.Office.IndexCode, objExcelGenerator, iRowNumber);
                                _setPCA(Item.Office.PCA, objExcelGenerator, iRowNumber);
                                _setAmount(Item.Amount, objExcelGenerator, iRowNumber);
                                _setComments(Item.Comments, objExcelGenerator, iRowNumber);
                                iRowNumber++;
                            }
                        }

                    }
                }
            }
            else
            {
                if (lstExpenditures != null && lstExpenditures.Count > 0)
                {
                    foreach (Expenditure Item in lstExpenditures)
                    {
                        if (Item.DateOfTransaction.Date >= dtEnteredDate.Value.Date && Item.DateOfTransaction.Date <= dtAsOfDate.Date)
                        {
                            if (Item.ExpenditureCategoryID == objExpenditureCategory.ExpenditureCategoryID)
                            {
                                _setTransactionDate(Item.DateOfTransaction.ToShortDateString(), objExcelGenerator, iRowNumber);
                                _setVendorName(Item.VendorName, objExcelGenerator, iRowNumber);
                                _setDescription(Item.Description, objExcelGenerator, iRowNumber);
                                _setOBJCode(Item.OBJCode, objExcelGenerator, iRowNumber);
                                _setIndex(Item.Office.IndexCode, objExcelGenerator, iRowNumber);
                                _setPCA(Item.Office.PCA, objExcelGenerator, iRowNumber);
                                _setAmount(Item.Amount, objExcelGenerator, iRowNumber);
                                _setComments(Item.Comments, objExcelGenerator, iRowNumber);
                                iRowNumber++;
                            }
                        }

                    }
                }

            }
        }


        private static void _AddWorkSetSheet(ExcelGenerator objExcelGenerator, string strExpenditureCategoryName)
        {
            string strFormat = "{0}";
            string strNPSReportSummarySheetPrefix = string.Empty;
            objExcelGenerator.AddWorkSheet(string.Format(strFormat, strExpenditureCategoryName));
        }

        private static void _setComments(string strText, ExcelGenerator objExcelGenerator, int iRowNumber)
        {
            string strCellNumber = "H" + iRowNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetText(strText, strCellNumber, blnShouldMerge, string.Empty);
        }

        private static void _setAmount(double strText, ExcelGenerator objExcelGenerator, int iRowNumber)
        {
            string strCellNumber = "G" + iRowNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetText(strText.ToString(), strCellNumber, blnShouldMerge, string.Empty);
        }

        private static void _setPCA(string strText, ExcelGenerator objExcelGenerator, int iRowNumber)
        {
            string strCellNumber = "F" + iRowNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetText(strText, strCellNumber, blnShouldMerge, string.Empty);
        }

        private static void _setIndex(string strText, ExcelGenerator objExcelGenerator, int iRowNumber)
        {
            string strCellNumber = "E" + iRowNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetText(strText, strCellNumber, blnShouldMerge, string.Empty);
        }

        private static void _setOBJCode(string strText, ExcelGenerator objExcelGenerator, int iRowNumber)
        {
            string strCellNumber = "D" + iRowNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetText(strText, strCellNumber, blnShouldMerge, string.Empty);
        }

        private static void _setDescription(string strText, ExcelGenerator objExcelGenerator, int iRowNumber)
        {
            string strCellNumber = "C" + iRowNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetText(strText, strCellNumber, blnShouldMerge, string.Empty);
        }

        private static void _setVendorName(string strText, ExcelGenerator objExcelGenerator, int iRowNumber)
        {
            string strCellNumber = "B" + iRowNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetText(strText, strCellNumber, blnShouldMerge, string.Empty);
        }

        private static void _setTransactionDate(string strText, ExcelGenerator objExcelGenerator, int iRowNumber)
        {
            string strCellNumber = "A" + iRowNumber;
            bool blnShouldMerge = false;
            objExcelGenerator.SetText(strText, strCellNumber, blnShouldMerge, string.Empty);

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

        private static void _SetHeader(ExcelGenerator objExcelGenerator, string strAdhocReportHeader)
        {
            string strCellNumber = "A2";
            string strMergeEndCellNumber = "H2";
            bool blnShouldMerge = true;
            objExcelGenerator.SetText(strAdhocReportHeader, strCellNumber, blnShouldMerge, strMergeEndCellNumber);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetHeaderCellFormat());
            //Set the font,bold,Capitalize
            //Set the border for the merged cells
            //Align text to centre

        }
        private static void _SetHeaderforFixedAdhocReports(ExcelGenerator objExcelGenerator, string strAdhocReportHeader,int iColNumber)
        {
            string strCellNumber = "A2";
            string strMergeEndCellNumber = MiscHelper.GetColumnNameFromNumber(iColNumber) + 2;
            
            bool blnShouldMerge = true;
            objExcelGenerator.SetText(strAdhocReportHeader, strCellNumber, blnShouldMerge, strMergeEndCellNumber);
            objExcelGenerator.SetCellFormat(strCellNumber, CellFormatHelper.SetHeaderCellFormat());
            //Set the font,bold,Capitalize
            //Set the border for the merged cells
            //Align text to centre

        }


    }
}
