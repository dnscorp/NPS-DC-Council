using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace PRIFACT.PRIFACTBase.ClosedXmlHelpers
{
    public class PfxFill
    {
        public XLColor BackgroundColor
        {
            get;
            set;
        }
        public PfxFill()
        {
            this.BackgroundColor = XLColor.Transparent;         
        }
    }
}
