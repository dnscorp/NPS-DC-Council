using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRIFACT.PRIFACTBase.ClosedXmlHelpers
{
    public class PfxEffects
    {
        public bool PfxStrikethrough
        {
            get;
            set;
        }

        public bool PfxSuperscript
        {
            get;
            set;
        }

        public bool PfxSubscript
        {
            get;
            set;
        }

        public PfxEffects()
        {
            this.PfxStrikethrough = false;
            this.PfxSubscript = false;
            this.PfxSuperscript = false;
        }
        public PfxEffects(bool pfxStrikeThrough, bool pfxSubscript, bool pfxSuperscript)
        {
            this.PfxStrikethrough = pfxStrikeThrough;
            this.PfxSubscript = pfxSubscript;
            this.PfxSuperscript = pfxSuperscript;
        }
    }
}
