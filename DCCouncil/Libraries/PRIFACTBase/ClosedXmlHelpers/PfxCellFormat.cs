using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIFACT.PRIFACTBase.ClosedXmlHelpers
{
    public class PfxCellFormat
    {
        public string PfxNumberFormat
        {
            get;
            set;
        }

        public PfxAlignment PfxAlignment
        {
            get;
            set;
        }

        public PfxFont PfxFont
        {
            get;
            set;
        }

        public PfxBorder PfxBorder
        {
            get;
            set;
        }

        public PfxFill PfxFill
        {
            get;
            set;
        }


        public PfxCellFormat()
        {
            this.PfxNumberFormat = ClosedXmlHelpers.PfxNumberFormat.TEXT;
            this.PfxAlignment = new PfxAlignment();
            this.PfxFont = new PfxFont();
            this.PfxBorder = new PfxBorder();
            this.PfxFill = new PfxFill();
        }
        public PfxCellFormat(string pfxNumberFormat, PfxAlignment pfxAlignment)
        {
            this.PfxNumberFormat = pfxNumberFormat;
            this.PfxAlignment = pfxAlignment;
            this.PfxFont = new PfxFont();
            this.PfxBorder = new PfxBorder();
            this.PfxFill = new PfxFill();
        }
        public PfxCellFormat(string pfxNumberFormat, PfxAlignment pfxAlignment, PfxFont pfxFont)
        {
            this.PfxNumberFormat = pfxNumberFormat;
            this.PfxAlignment = pfxAlignment;
            this.PfxFont = pfxFont;
            this.PfxBorder = new PfxBorder();
            this.PfxFill = new PfxFill();
        }
        public PfxCellFormat(string pfxNumberFormat, PfxAlignment pfxAlignment, PfxFont pfxFont,PfxBorder pfxBorder)
        {
            this.PfxNumberFormat = pfxNumberFormat;
            this.PfxAlignment = pfxAlignment;
            this.PfxFont = pfxFont;
            this.PfxBorder = pfxBorder;
            this.PfxFill = new PfxFill();
        }
        public PfxCellFormat(string pfxNumberFormat, PfxAlignment pfxAlignment, PfxFont pfxFont, PfxBorder pfxBorder,PfxFill pfxFill)
        {
            this.PfxNumberFormat = pfxNumberFormat;
            this.PfxAlignment = pfxAlignment;
            this.PfxFont = pfxFont;
            this.PfxBorder = pfxBorder;
            this.PfxFill = pfxFill;
        }
        public PfxCellFormat(PfxBorder pfxBorder)
        {
            this.PfxNumberFormat = ClosedXmlHelpers.PfxNumberFormat.TEXT;
            this.PfxAlignment = new PfxAlignment();
            this.PfxFont = new PfxFont();
            this.PfxFill = new PfxFill();
            this.PfxBorder = pfxBorder;         
        }
    }
}
