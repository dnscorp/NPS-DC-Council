using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClosedXML.Excel;

namespace PRIFACT.PRIFACTBase.ClosedXmlHelpers
{
    public class PfxTextAlignmnet
    {
        public XLAlignmentHorizontalValues Horizontal
        {
            get;
            set;
        }

        public XLAlignmentVerticalValues Vertical
        {
            get;
            set;
        }

        public int Indent
        {
            get;
            set;
        }

        public PfxTextAlignmnet()
        {
            this.Horizontal = XLAlignmentHorizontalValues.General;
            this.Vertical = XLAlignmentVerticalValues.Bottom;
            this.Indent = 0;
        }
        public PfxTextAlignmnet(XLAlignmentHorizontalValues horizontal, XLAlignmentVerticalValues vertical, int indent)
        {
            this.Horizontal = horizontal;
            this.Vertical = vertical;
            this.Indent = indent;
        }
    }
}
