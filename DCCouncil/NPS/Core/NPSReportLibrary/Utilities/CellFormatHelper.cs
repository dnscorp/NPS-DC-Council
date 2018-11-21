using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using PRIFACT.PRIFACTBase.ClosedXmlHelpers;

namespace PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary.Utilities
{
    public class CellFormatHelper
    {
        //Used for the items in the summary
        public static PfxCellFormat GetIndentedGeneralTextCellFormat()
        {
            PfxCellFormat objCellFormat =
                new PfxCellFormat(
                    PfxNumberFormat.TEXT,
                    new PfxAlignment(
                            new PfxTextAlignmnet(XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Bottom,
                                3), new PfxTextControl())
                            ,
                            new PfxFont()
                            );
            return objCellFormat;
        }

        public static PfxCellFormat GetCurrencyCellFormat()
        {
            PfxCellFormat objCellFormat =
                new PfxCellFormat(
                    PfxNumberFormat.DECIMAL,
                        new PfxAlignment(
                            new PfxTextAlignmnet(ClosedXML.Excel.XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Bottom, 0),
                            new PfxTextControl()
                        )
                    );
            return objCellFormat;
        }

        public static PfxCellFormat SetHeaderCellFormat()
        {
            PfxTextControl objPfxTextControl = new PfxTextControl(false, false, true);
            PfxTextAlignmnet objPfxTextAlignment = new PfxTextAlignmnet(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Bottom, 0);
            PfxAlignment objPfxAlignment = new PfxAlignment(objPfxTextAlignment, objPfxTextControl);
            PfxFont objPfxFont = new PfxFont("Cambria", XLColor.Black, new PfxEffects(), PfxFontStyle.Bold, 14, XLFontUnderlineValues.None);
            PfxBorder objPfxBorder = new PfxBorder();
            //objPfxBorder.PfxLineStyle = PfxLineStyle.Continuous;
            PfxFill objPfxFill = new PfxFill();
            string pfxNumberFormat = PfxNumberFormat.TEXT;

            PfxCellFormat objCellFormat = new PfxCellFormat(pfxNumberFormat, objPfxAlignment, objPfxFont, objPfxBorder, objPfxFill);
            return objCellFormat;
        }
        public static PfxCellFormat SetSubHeaderCellFormat()
        {
            PfxTextControl objPfxTextControl = new PfxTextControl(false, false, true);
            PfxTextAlignmnet objPfxTextAlignment = new PfxTextAlignmnet(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Bottom, 0);
            PfxAlignment objPfxAlignment = new PfxAlignment(objPfxTextAlignment, objPfxTextControl);
            PfxFont objPfxFont = new PfxFont("Cambria", XLColor.Black, new PfxEffects(), PfxFontStyle.Bold, 10, XLFontUnderlineValues.None);
            PfxBorder objPfxBorder = new PfxBorder();

            PfxFill objPfxFill = new PfxFill();
            string pfxNumberFormat = PfxNumberFormat.TEXT;

            PfxCellFormat objCellFormat = new PfxCellFormat(pfxNumberFormat, objPfxAlignment, objPfxFont, objPfxBorder, objPfxFill);
            return objCellFormat;

        }
        public static PfxCellFormat SetSubHeading2CellFormat()
        {
            PfxTextControl objPfxTextControl = new PfxTextControl(false, false, true);
            PfxTextAlignmnet objPfxTextAlignment = new PfxTextAlignmnet(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Bottom, 0);
            PfxAlignment objPfxAlignment = new PfxAlignment(objPfxTextAlignment, objPfxTextControl);
            PfxFont objPfxFont = new PfxFont("Cambria", XLColor.Black, new PfxEffects(), PfxFontStyle.Bold, 10, XLFontUnderlineValues.None);
            PfxBorder objPfxBorder = new PfxBorder();

            PfxFill objPfxFill = new PfxFill();
            string pfxNumberFormat = PfxNumberFormat.TEXT;

            PfxCellFormat objCellFormat = new PfxCellFormat(pfxNumberFormat, objPfxAlignment, objPfxFont, objPfxBorder, objPfxFill);
            return objCellFormat;

        }

        public static PfxCellFormat SetCellFormatForBudgetEntry()
        {
            PfxTextControl objPfxTextControl = new PfxTextControl(false, false, true);
            PfxTextAlignmnet objPfxTextAlignment = new PfxTextAlignmnet(XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Bottom, 0);
            PfxAlignment objPfxAlignment = new PfxAlignment(objPfxTextAlignment, objPfxTextControl);
            PfxFont objPfxFont = new PfxFont("Calibri", XLColor.Black, new PfxEffects(), PfxFontStyle.Bold, 11, XLFontUnderlineValues.None);
            PfxBorder objPfxBorder = new PfxBorder();

            PfxFill objPfxFill = new PfxFill();
            string pfxNumberFormat = PfxNumberFormat.TEXT;

            PfxCellFormat objCellFormat = new PfxCellFormat(pfxNumberFormat, objPfxAlignment, objPfxFont, objPfxBorder, objPfxFill);
            return objCellFormat;
        }

        public static PfxCellFormat SetCellFormatForAllocatedBudget()
        {
            PfxTextControl objPfxTextControl = new PfxTextControl(false, false, true);
            PfxTextAlignmnet objPfxTextAlignment = new PfxTextAlignmnet();
            PfxAlignment objPfxAlignment = new PfxAlignment(objPfxTextAlignment, objPfxTextControl);
            PfxFont objPfxFont = new PfxFont("Calibri", XLColor.White, new PfxEffects(), PfxFontStyle.Bold, 11, XLFontUnderlineValues.None);
            PfxBorder objPfxBorder = new PfxBorder();

            PfxFill objPfxFill = new PfxFill();
            objPfxFill.BackgroundColor = XLColor.FromArgb(0, 113, 191);
            string pfxNumberFormat = PfxNumberFormat.CURRENCY;

            PfxCellFormat objCellFormat = new PfxCellFormat(pfxNumberFormat, objPfxAlignment, objPfxFont, objPfxBorder, objPfxFill);
            return objCellFormat;
        }
        public static PfxCellFormat SetCellFormatForExpenditureHeaders()
        {

            PfxTextControl objPfxTextControl = new PfxTextControl(false, false, true);
            PfxTextAlignmnet objPfxTextAlignment = new PfxTextAlignmnet(XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Bottom, 0);
            PfxAlignment objPfxAlignment = new PfxAlignment(objPfxTextAlignment, objPfxTextControl);
            PfxFont objPfxFont = new PfxFont("Calibri", XLColor.Black, new PfxEffects(), PfxFontStyle.Bold, 11, XLFontUnderlineValues.None);
            PfxBorder objPfxBorder = new PfxBorder();

            PfxFill objPfxFill = new PfxFill();
            string pfxNumberFormat = PfxNumberFormat.TEXT;

            PfxCellFormat objCellFormat = new PfxCellFormat(pfxNumberFormat, objPfxAlignment, objPfxFont, objPfxBorder, objPfxFill);
            return objCellFormat;
        }
        public static PfxCellFormat SetCellFormatForDescription()
        {
            PfxTextControl objPfxTextControl = new PfxTextControl(false, false, true);
            PfxTextAlignmnet objPfxTextAlignment = new PfxTextAlignmnet();
            PfxAlignment objPfxAlignment = new PfxAlignment(objPfxTextAlignment, objPfxTextControl);
            PfxFont objPfxFont = new PfxFont("Calibri", XLColor.Black, new PfxEffects(), PfxFontStyle.Bold, 11, XLFontUnderlineValues.Single);
            PfxBorder objPfxBorder = new PfxBorder();

            PfxFill objPfxFill = new PfxFill();

            string pfxNumberFormat = PfxNumberFormat.TEXT;

            PfxCellFormat objCellFormat = new PfxCellFormat(pfxNumberFormat, objPfxAlignment, objPfxFont, objPfxBorder, objPfxFill);
            return objCellFormat;

        }
        public static PfxCellFormat SetBottomBorderToCell()
        {
            PfxBorder objPfxBorder = new PfxBorder();
            objPfxBorder.PfxLineStyle = XLBorderStyleValues.Thick;
            objPfxBorder.BottomBorder = true;
            objPfxBorder.TopBorder = false;
            objPfxBorder.RightBorder = false;
            objPfxBorder.LeftBorder = false;
            string pfxNumberFormat = PfxNumberFormat.DECIMAL;
            PfxFill objPfxFill = new PfxFill();
            PfxTextAlignmnet objPfxTextAlignment = new PfxTextAlignmnet(XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Bottom, 0);
            PfxTextControl objPfxTextControl = new PfxTextControl(false, false, true);
            PfxAlignment objPfxAlignment = new PfxAlignment(objPfxTextAlignment, objPfxTextControl);
            PfxCellFormat objCellFormat = new PfxCellFormat(pfxNumberFormat, objPfxAlignment, new PfxFont(), objPfxBorder, objPfxFill);

            return objCellFormat;
        }
        public static PfxCellFormat SetCellFormatForFooter()
        {
            PfxTextControl objPfxTextControl = new PfxTextControl(false, false, true);
            PfxTextAlignmnet objPfxTextAlignment = new PfxTextAlignmnet(XLAlignmentHorizontalValues.Right, XLAlignmentVerticalValues.Bottom, 0);
            PfxAlignment objPfxAlignment = new PfxAlignment(objPfxTextAlignment, objPfxTextControl);
            PfxFont objPfxFont = new PfxFont("Calibri", XLColor.Black, new PfxEffects(), PfxFontStyle.Bold, 11, XLFontUnderlineValues.None);
            PfxBorder objPfxBorder = new PfxBorder();

            PfxFill objPfxFill = new PfxFill();

            string pfxNumberFormat = PfxNumberFormat.TEXT;

            PfxCellFormat objCellFormat = new PfxCellFormat(pfxNumberFormat, objPfxAlignment, objPfxFont, objPfxBorder, objPfxFill);
            return objCellFormat;


        }
        public static PfxCellFormat SetColumnHeading()
        {

            PfxTextControl objPfxTextControl = new PfxTextControl(false, false, true);
            PfxTextAlignmnet objPfxTextAlignment = new PfxTextAlignmnet(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Bottom, 0);
            PfxAlignment objPfxAlignment = new PfxAlignment(objPfxTextAlignment, objPfxTextControl);
            PfxFont objPfxFont = new PfxFont("Cambria", XLColor.Black, new PfxEffects(), PfxFontStyle.Bold, 11, XLFontUnderlineValues.None);
            PfxBorder objPfxBorder = new PfxBorder();

            PfxFill objPfxFill = new PfxFill();

            string pfxNumberFormat = PfxNumberFormat.TEXT;

            PfxCellFormat objCellFormat = new PfxCellFormat(pfxNumberFormat, objPfxAlignment, objPfxFont, objPfxBorder, objPfxFill);
            return objCellFormat;
        }

        public static PfxCellFormat SetFirstTransactionSheetColumnValue()
        {

            PfxTextControl objPfxTextControl = new PfxTextControl(false, false, true);
            PfxTextAlignmnet objPfxTextAlignment = new PfxTextAlignmnet(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Bottom, 0);
            PfxAlignment objPfxAlignment = new PfxAlignment(objPfxTextAlignment, objPfxTextControl);
            PfxFont objPfxFont = new PfxFont("Calibri", XLColor.Black, new PfxEffects(), PfxFontStyle.Regular, 11, XLFontUnderlineValues.None);
            PfxBorder objPfxBorder = new PfxBorder();

            PfxFill objPfxFill = new PfxFill();

            string pfxNumberFormat = PfxNumberFormat.DATE;

            PfxCellFormat objCellFormat = new PfxCellFormat(pfxNumberFormat, objPfxAlignment, objPfxFont, objPfxBorder, objPfxFill);
            return objCellFormat;
        }
        public static PfxCellFormat SetSecondTransactionSheetColumnValue()
        {

            PfxTextControl objPfxTextControl = new PfxTextControl(false, false, true);
            PfxTextAlignmnet objPfxTextAlignment = new PfxTextAlignmnet(XLAlignmentHorizontalValues.General, XLAlignmentVerticalValues.Bottom, 0);
            PfxAlignment objPfxAlignment = new PfxAlignment(objPfxTextAlignment, objPfxTextControl);
            PfxFont objPfxFont = new PfxFont("Calibri", XLColor.Black, new PfxEffects(), PfxFontStyle.Regular, 11, XLFontUnderlineValues.None);
            PfxBorder objPfxBorder = new PfxBorder();

            PfxFill objPfxFill = new PfxFill();

            string pfxNumberFormat = PfxNumberFormat.TEXT;

            PfxCellFormat objCellFormat = new PfxCellFormat(pfxNumberFormat, objPfxAlignment, objPfxFont, objPfxBorder, objPfxFill);
            return objCellFormat;
        }
        public static PfxCellFormat SetThirdTransactionSheetColumnValue()
        {

            PfxTextControl objPfxTextControl = new PfxTextControl(false, false, true);
            PfxTextAlignmnet objPfxTextAlignment = new PfxTextAlignmnet(XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Bottom, 0);
            PfxAlignment objPfxAlignment = new PfxAlignment(objPfxTextAlignment, objPfxTextControl);
            PfxFont objPfxFont = new PfxFont("Calibri", XLColor.Black, new PfxEffects(), PfxFontStyle.Regular, 11, XLFontUnderlineValues.None);
            PfxBorder objPfxBorder = new PfxBorder();

            PfxFill objPfxFill = new PfxFill();

            string pfxNumberFormat = PfxNumberFormat.TEXT;

            PfxCellFormat objCellFormat = new PfxCellFormat(pfxNumberFormat, objPfxAlignment, objPfxFont, objPfxBorder, objPfxFill);
            return objCellFormat;
        }

        public static PfxCellFormat SetFourthTransactionSheetColumnValue()
        {

            PfxTextControl objPfxTextControl = new PfxTextControl(false, false, true);
            PfxTextAlignmnet objPfxTextAlignment = new PfxTextAlignmnet(XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Bottom, 0);
            PfxAlignment objPfxAlignment = new PfxAlignment(objPfxTextAlignment, objPfxTextControl);
            PfxFont objPfxFont = new PfxFont("Calibri", XLColor.Black, new PfxEffects(), PfxFontStyle.Regular, 11, XLFontUnderlineValues.None);
            PfxBorder objPfxBorder = new PfxBorder();

            PfxFill objPfxFill = new PfxFill();

            string pfxNumberFormat = PfxNumberFormat.TEXT;

            PfxCellFormat objCellFormat = new PfxCellFormat(pfxNumberFormat, objPfxAlignment, objPfxFont, objPfxBorder, objPfxFill);
            return objCellFormat;
        }

        public static PfxCellFormat SetCellFormatForPhoneHeaders()
        {

            PfxTextControl objPfxTextControl = new PfxTextControl(false, false, true);
            PfxTextAlignmnet objPfxTextAlignment = new PfxTextAlignmnet(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Bottom, 0);
            PfxAlignment objPfxAlignment = new PfxAlignment(objPfxTextAlignment, objPfxTextControl);
            PfxFont objPfxFont = new PfxFont("Arial", XLColor.Black, new PfxEffects(), PfxFontStyle.Bold, 10, XLFontUnderlineValues.None);
            PfxBorder objPfxBorder = new PfxBorder();

            PfxFill objPfxFill = new PfxFill();

            string pfxNumberFormat = PfxNumberFormat.TEXT;

            PfxCellFormat objCellFormat = new PfxCellFormat(pfxNumberFormat, objPfxAlignment, objPfxFont, objPfxBorder, objPfxFill);
            return objCellFormat;
        }


        public static PfxCellFormat SetCellFormatForPhoneColumnHeaders()
        {

            PfxTextControl objPfxTextControl = new PfxTextControl(false, false, true);
            PfxTextAlignmnet objPfxTextAlignment = new PfxTextAlignmnet(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Bottom, 0);
            PfxAlignment objPfxAlignment = new PfxAlignment(objPfxTextAlignment, objPfxTextControl);
            PfxFont objPfxFont = new PfxFont("Calibri", XLColor.White, new PfxEffects(), PfxFontStyle.Bold, 11, XLFontUnderlineValues.None);
            PfxBorder objPfxBorder = new PfxBorder();

            PfxFill objPfxFill = new PfxFill();
            objPfxFill.BackgroundColor = XLColor.FromArgb(0, 113, 191);

            string pfxNumberFormat = PfxNumberFormat.TEXT;

            PfxCellFormat objCellFormat = new PfxCellFormat(pfxNumberFormat, objPfxAlignment, objPfxFont, objPfxBorder, objPfxFill);
            return objCellFormat;
        }

        public static PfxCellFormat SetCellFormatForPhoneFooterSum()
        {

            PfxTextControl objPfxTextControl = new PfxTextControl(false, false, true);
            PfxTextAlignmnet objPfxTextAlignment = new PfxTextAlignmnet(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Bottom, 0);
            PfxAlignment objPfxAlignment = new PfxAlignment(objPfxTextAlignment, objPfxTextControl);
            PfxFont objPfxFont = new PfxFont("Calibri", XLColor.White, new PfxEffects(), PfxFontStyle.Bold, 11, XLFontUnderlineValues.None);
            PfxBorder objPfxBorder = new PfxBorder();

            PfxFill objPfxFill = new PfxFill();
            objPfxFill.BackgroundColor = XLColor.FromArgb(0, 113, 191);

            string pfxNumberFormat = PfxNumberFormat.DECIMAL;

            PfxCellFormat objCellFormat = new PfxCellFormat(pfxNumberFormat, objPfxAlignment, objPfxFont, objPfxBorder, objPfxFill);
            return objCellFormat;
        }

        internal static PfxCellFormat SetCellFormatForMultipleAllocatedBudget()
        {
            PfxTextControl objPfxTextControl = new PfxTextControl(false, false, true);
            PfxTextAlignmnet objPfxTextAlignment = new PfxTextAlignmnet();
            PfxAlignment objPfxAlignment = new PfxAlignment(objPfxTextAlignment, objPfxTextControl);
            PfxFont objPfxFont = new PfxFont("Calibri", XLColor.Black, new PfxEffects(), PfxFontStyle.Regular, 11, XLFontUnderlineValues.None);
            PfxBorder objPfxBorder = new PfxBorder();

            PfxFill objPfxFill = new PfxFill();

            string pfxNumberFormat = PfxNumberFormat.CURRENCY;

            PfxCellFormat objCellFormat = new PfxCellFormat(pfxNumberFormat, objPfxAlignment, objPfxFont, objPfxBorder, objPfxFill);
            return objCellFormat;
        }
        public static PfxCellFormat SetColumnHeadingForFixedExpCategory()
        {
            PfxTextControl objPfxTextControl = new PfxTextControl(true, false, true);
            PfxTextAlignmnet objPfxTextAlignment = new PfxTextAlignmnet(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Bottom, 0);
            PfxAlignment objPfxAlignment = new PfxAlignment(objPfxTextAlignment, objPfxTextControl);
            PfxFont objPfxFont = new PfxFont("Calibri", XLColor.Black, new PfxEffects(), PfxFontStyle.Bold, 10, XLFontUnderlineValues.None);
            PfxBorder objPfxBorder = new PfxBorder();
            objPfxBorder.PfxLineStyle = XLBorderStyleValues.Thin;
            objPfxBorder.BottomBorder = true;
            objPfxBorder.TopBorder = true;
            objPfxBorder.RightBorder = true;
            objPfxBorder.LeftBorder = true;
            

            PfxFill objPfxFill = new PfxFill();
            objPfxFill.BackgroundColor = XLColor.FromArgb(242, 189, 15);


            string pfxNumberFormat = PfxNumberFormat.TEXT;

            PfxCellFormat objCellFormat = new PfxCellFormat(pfxNumberFormat, objPfxAlignment, objPfxFont, objPfxBorder, objPfxFill);

            return objCellFormat;
        }

        internal static PfxCellFormat SetOfficeNameFieldsForFixedExpCategory()
        {
            PfxTextControl objPfxTextControl = new PfxTextControl(true, false, true);
            PfxTextAlignmnet objPfxTextAlignment = new PfxTextAlignmnet(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Bottom, 0);
            PfxAlignment objPfxAlignment = new PfxAlignment(objPfxTextAlignment, objPfxTextControl);
            PfxFont objPfxFont = new PfxFont("Calibri", XLColor.Black, new PfxEffects(), PfxFontStyle.Regular, 10, XLFontUnderlineValues.None);
            PfxBorder objPfxBorder = new PfxBorder();
            objPfxBorder.PfxLineStyle = XLBorderStyleValues.Thin;
            objPfxBorder.BottomBorder = true;
            objPfxBorder.TopBorder = true;
            objPfxBorder.RightBorder = true;
            objPfxBorder.LeftBorder = true;
            

            PfxFill objPfxFill = new PfxFill();


            string pfxNumberFormat = PfxNumberFormat.TEXT;

            PfxCellFormat objCellFormat = new PfxCellFormat(pfxNumberFormat, objPfxAlignment, objPfxFont, objPfxBorder, objPfxFill);

            return objCellFormat;
        }

        internal static PfxCellFormat SetCellFormatForMultipleBudgetEntry()
        {
            PfxTextControl objPfxTextControl = new PfxTextControl(false, false, true);
            PfxTextAlignmnet objPfxTextAlignment = new PfxTextAlignmnet(XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Bottom, 0);
            PfxAlignment objPfxAlignment = new PfxAlignment(objPfxTextAlignment, objPfxTextControl);
            PfxFont objPfxFont = new PfxFont("Calibri", XLColor.Black, new PfxEffects(), PfxFontStyle.Regular, 11, XLFontUnderlineValues.None);
            PfxBorder objPfxBorder = new PfxBorder();

            PfxFill objPfxFill = new PfxFill();
            string pfxNumberFormat = PfxNumberFormat.TEXT;

            PfxCellFormat objCellFormat = new PfxCellFormat(pfxNumberFormat, objPfxAlignment, objPfxFont, objPfxBorder, objPfxFill);
            return objCellFormat;
        }

        public static PfxCellFormat GetFixedAdhocReportCurrencyCellFormat()
        {
            PfxTextControl objPfxTextControl = new PfxTextControl(true, true, true);
            PfxTextAlignmnet objPfxTextAlignment = new PfxTextAlignmnet(XLAlignmentHorizontalValues.Left, XLAlignmentVerticalValues.Bottom, 3);
            PfxAlignment objPfxAlignment = new PfxAlignment(objPfxTextAlignment, objPfxTextControl);
            PfxFont objPfxFont = new PfxFont("Calibri", XLColor.Black, new PfxEffects(), PfxFontStyle.Regular, 10, XLFontUnderlineValues.None);
           
            PfxFill objPfxFill = new PfxFill();
            string pfxNumberFormat = PfxNumberFormat.CURRENCY;
            
            PfxBorder objPfxBorder = new PfxBorder();
            objPfxBorder.PfxLineStyle = XLBorderStyleValues.Thin;
            objPfxBorder.BottomBorder = true;
            objPfxBorder.TopBorder = true;
            objPfxBorder.RightBorder = true;
            objPfxBorder.LeftBorder = true;

            PfxCellFormat objCellFormat = new PfxCellFormat(pfxNumberFormat, objPfxAlignment, objPfxFont, objPfxBorder, objPfxFill);
            return objCellFormat;
          
            
        }


        internal static PfxCellFormat GetCustomReportCurrencyCellFormat()
        {

            PfxTextControl objPfxTextControl = new PfxTextControl(true, true, true);
            PfxTextAlignmnet objPfxTextAlignment = new PfxTextAlignmnet(XLAlignmentHorizontalValues.Center, XLAlignmentVerticalValues.Bottom, 0);
            PfxAlignment objPfxAlignment = new PfxAlignment(objPfxTextAlignment, objPfxTextControl);
            PfxFont objPfxFont = new PfxFont("Calibri", XLColor.Black, new PfxEffects(), PfxFontStyle.Regular, 10, XLFontUnderlineValues.None);

            PfxFill objPfxFill = new PfxFill();
            string pfxNumberFormat = PfxNumberFormat.CURRENCY;

            PfxBorder objPfxBorder = new PfxBorder();
            objPfxBorder.PfxLineStyle = XLBorderStyleValues.Thin;
            objPfxBorder.BottomBorder = true;
            objPfxBorder.TopBorder = true;
            objPfxBorder.RightBorder = true;
            objPfxBorder.LeftBorder = true;

            PfxCellFormat objCellFormat = new PfxCellFormat(pfxNumberFormat, objPfxAlignment, objPfxFont, objPfxBorder, objPfxFill);
            return objCellFormat;
        }
    }
}
