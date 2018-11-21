using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRIFACT.PRIFACTBase.ClosedXmlHelpers
{
    public class PfxNumberFormat
    {
        public const string TEXT = "General";
        public const string DECIMAL = @"_(* #,##0.00_);_(* (#,##0.00);_(* "" - ""??_);_(@_)";
        public const string CURRENCY = @"_($* #,##0.00_);_($* (#,##0.00);_($* "" - ""??_);_(@_)";
        //public const string CURRENCY = "&quot;$&quot;#,##0.00";
        public const string DATE = "mm/dd/yy";
    }
}
