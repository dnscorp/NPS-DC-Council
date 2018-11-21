using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRIFACT.PRIFACTBase.ClosedXmlHelpers
{
    public class PfxAlignment
    {
        public PfxTextAlignmnet TextAlignment
        {
            get;
            set;
        }

        public PfxTextControl TextControl
        {
            get;
            set;
        }

        public PfxAlignment()
        {
            this.TextAlignment = new PfxTextAlignmnet();
            this.TextControl = new PfxTextControl();
        }
        
        public PfxAlignment(PfxTextAlignmnet alignmnet, PfxTextControl textControl)
        {
            this.TextAlignment = alignmnet;
            this.TextControl = textControl;
        }
    }
}
