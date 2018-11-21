using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRIFACT.PRIFACTBase.ClosedXmlHelpers
{
    public class PfxTextControl
    {
        public bool PfxWrapText
        {
            get;
            set;
        }

        public bool PfxShrinkToFit
        {
            get;
            set;
        }
       

        public PfxTextControl()
        {
            this.PfxWrapText = false;
            this.PfxShrinkToFit = false;            
        }
        public PfxTextControl(bool wrapText, bool shrinkToFit, bool mergeCells)
        {
            this.PfxWrapText = wrapText;
            this.PfxShrinkToFit = shrinkToFit;            
        }
    }
}
