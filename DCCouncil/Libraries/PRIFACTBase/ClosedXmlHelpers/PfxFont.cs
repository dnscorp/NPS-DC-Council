using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClosedXML.Excel;

namespace PRIFACT.PRIFACTBase.ClosedXmlHelpers
{
    public class PfxFont
    {
        public string PfxFontName
        {
            get;
            set;
        }

        public PfxFontStyle PfxFontStyle
        {
            get;
            set;
        }

        public double PfxSize
        {
            get;
            set;
        }

        public XLFontUnderlineValues PfxUnderLine
        {
            get;
            set;
        }

        public XLColor PfxColor
        {
            get;
            set;
        }        

        public PfxEffects PfxEffects
        {
            get;
            set;
        }

        public PfxFont()
        {
            //Get from Config
            this.PfxFontName = "Calibri";
            this.PfxColor = XLColor.Black;
            this.PfxEffects = new PfxEffects();
            this.PfxFontStyle = PfxFontStyle.Regular;            
            this.PfxSize = 11;
            this.PfxUnderLine = XLFontUnderlineValues.None;            
        }

        public PfxFont(string pfxFontName, XLColor pfxColor, PfxEffects pfxEffects, PfxFontStyle pfxFontStyle, double pfxSize, XLFontUnderlineValues pfxUnderLine)
        {
            this.PfxFontName = pfxFontName;
            this.PfxColor = pfxColor;
            this.PfxEffects = pfxEffects;
            this.PfxFontStyle = pfxFontStyle;            
            this.PfxSize = pfxSize;
            this.PfxUnderLine = pfxUnderLine;   
        }
    }
}
