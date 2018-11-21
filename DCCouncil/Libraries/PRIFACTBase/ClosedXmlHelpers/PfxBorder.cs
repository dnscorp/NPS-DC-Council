using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace PRIFACT.PRIFACTBase.ClosedXmlHelpers
{
    public class PfxBorder
    {
        public XLBorderStyleValues PfxLineStyle
        {
            get;
            set;
        }

        public XLColor PfxLineColor
        {
            get;
            set;
        }

        public bool  LeftBorder
        {
            get;
            set;
        }

        public bool TopBorder
        {
            get;
            set;
        }

        public bool BottomBorder
        {
            get;
            set;
        }

        public bool RightBorder
        {
            get;
            set;
        }

        public PfxBorder()
        {
            this.PfxLineStyle = XLBorderStyleValues.None;
            this.PfxLineColor = XLColor.Black;
            this.LeftBorder = true;
            this.TopBorder = true;
            this.BottomBorder = true;
            this.RightBorder = true;
        }
    }
}
