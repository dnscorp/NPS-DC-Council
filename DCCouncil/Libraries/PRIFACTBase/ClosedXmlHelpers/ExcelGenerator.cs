using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace PRIFACT.PRIFACTBase.ClosedXmlHelpers
{
    public class ExcelGenerator : IDisposable
    {
        XLWorkbook _Workbook = null;
        public XLWorkbook WorkBook
        {
            get
            {
                return _Workbook;
            }
        }

        private IXLWorksheet _ActiveSheet;
        public IXLWorksheet ActiveSheet
        {
            get
            {
                return _ActiveSheet;
            }
        }

        public ExcelGenerator()
        {
            _Workbook = new XLWorkbook();
        }

        public ExcelGenerator(string strFileName)
        {
            _Workbook = new XLWorkbook(strFileName);
        }

        #region IDisposable Members

        public void Dispose()
        {
            _Workbook.Dispose();
        }

        #endregion

        public void Save(string strFilePath)
        {
            _Workbook.SaveAs(strFilePath);
        }
        public void SetText(string strText, string strCellNumber, bool blnShouldMerge, string strMergeEndCellNumber)
        {
            _ActiveSheet.Cell(strCellNumber).Value = strText;
            if (blnShouldMerge)
            {
                IXLRange objRange = _ActiveSheet.Range(strCellNumber + ":" + strMergeEndCellNumber);
                objRange.Row(1).Merge();
            }
        }

        public void SetFormula(string strText, string strCellNumber, bool blnShouldMerge, string strMergeEndCellNumber)
        {
            _ActiveSheet.Cell(strCellNumber).FormulaA1 = strText;
            if (blnShouldMerge)
            {
                IXLRange objRange = _ActiveSheet.Range(strCellNumber + ":" + strMergeEndCellNumber);
                objRange.Row(1).Merge();
            }
        }

        public void AddWorkSheet(string strSheetName)
        {
            _ActiveSheet = _Workbook.Worksheets.Add(strSheetName);
        }

        public void SetColumnWidth(string strColumnName, int iWidth)
        {
            _ActiveSheet.Column(strColumnName).Width = iWidth;
        }

        public void SetCellFormat(string strCellNumber, PfxCellFormat objCellFormat)
        {
            IXLCell objRange = _ActiveSheet.Cell(strCellNumber);
            _SetNumberFormat(objRange, objCellFormat.PfxNumberFormat);
            _SetAlignment(objRange, objCellFormat.PfxAlignment);
            _SetFont(objRange, objCellFormat.PfxFont);
            _SetFill(objRange, objCellFormat.PfxFill);
            _SetBorder(objRange, objCellFormat.PfxBorder);            
        }

        private void _SetFill(IXLCell objRange, PfxFill pfxFill)
        {
            if (pfxFill.BackgroundColor != XLColor.Transparent)
            {
                objRange.Style.Fill.BackgroundColor = pfxFill.BackgroundColor;
            }            
        }

        private void _SetBorder(IXLCell objRange, PfxBorder pfxBorder)
        {
            if (pfxBorder.PfxLineStyle != XLBorderStyleValues.None)
            {
                XLBorderStyleValues lineStyle = pfxBorder.PfxLineStyle;
                if (pfxBorder.TopBorder)
                {
                    objRange.Style.Border.TopBorder = lineStyle;
                }
                if (pfxBorder.BottomBorder)
                {
                    objRange.Style.Border.BottomBorder = lineStyle;
                }
                if (pfxBorder.LeftBorder)
                {
                    objRange.Style.Border.LeftBorder = lineStyle;
                }
                if (pfxBorder.RightBorder)
                {
                    objRange.Style.Border.RightBorder = lineStyle;
                }
            }            
        }

        private void _SetFont(IXLCell objRange, PfxFont pfxFont)
        {
            objRange.Style.Font.FontName = pfxFont.PfxFontName;
            objRange.Style.Font.FontColor = pfxFont.PfxColor;
            _SetFontEffects(objRange, pfxFont.PfxEffects);
            _SetFontStyle(objRange, pfxFont.PfxFontStyle);
            objRange.Style.Font.FontSize = pfxFont.PfxSize;
            objRange.Style.Font.Underline= pfxFont.PfxUnderLine;
        }

        private void _SetFontStyle(IXLCell objRange, PfxFontStyle pfxFontStyle)
        {
            switch (pfxFontStyle)
            {
                case PfxFontStyle.Bold:
                    objRange.Style.Font.Bold  = true;
                    break;
                case PfxFontStyle.BoldItalic:
                    objRange.Style.Font.Bold = true;
                    objRange.Style.Font.Italic = true;
                    break;
                case PfxFontStyle.Italic:
                    objRange.Style.Font.Italic = true;
                    break;
                case PfxFontStyle.Regular:
                    objRange.Style.Font.Bold = false;
                    objRange.Style.Font.Italic = false;
                    break;
            }
        }

        private void _SetFontEffects(IXLCell objRange, PfxEffects pfxEffects)
        {
            objRange.Style.Font.Strikethrough = pfxEffects.PfxStrikethrough;
            if(pfxEffects.PfxSuperscript){
                 objRange.Style.Font.VerticalAlignment = XLFontVerticalTextAlignmentValues.Superscript;
            }
             if(pfxEffects.PfxSubscript){
                 objRange.Style.Font.VerticalAlignment = XLFontVerticalTextAlignmentValues.Subscript;
            }            
        }

        private void _SetAlignment(IXLCell objRange, PfxAlignment alignment)
        {
            _SetTextAlignment(objRange, alignment.TextAlignment);
            _SetTextControl(objRange, alignment.TextControl);
        }

        private void _SetTextControl(IXLCell objRange, PfxTextControl textControl)
        {
            objRange.Style.Alignment.WrapText = textControl.PfxWrapText ? true : false;
            objRange.Style.Alignment.ShrinkToFit = textControl.PfxShrinkToFit ? true : false;            
        }

        private void _SetTextAlignment(IXLCell objRange, PfxTextAlignmnet textAlignmnet)
        {
            _SetHorizontalAlignment(objRange, textAlignmnet.Horizontal, textAlignmnet.Indent);
            _SetVerticalAlignment(objRange, textAlignmnet.Vertical);
        }

        private void _SetVerticalAlignment(IXLCell objRange, XLAlignmentVerticalValues vertical)
        {
            objRange.Style.Alignment.Vertical = vertical;
        }

        private void _SetHorizontalAlignment(IXLCell objRange, XLAlignmentHorizontalValues horizontal, int indent)
        {
            objRange.Style.Alignment.Horizontal = horizontal;
            if (indent > 0)
            {
                objRange.Style.Alignment.Indent = indent;
            }
        }

        private void _SetNumberFormat(IXLCell objRange, string strNumberFormat)
        {
            objRange.Style.NumberFormat.Format = strNumberFormat;
        }
    }
}
