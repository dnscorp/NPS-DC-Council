using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary.Helper
{
    public class VendorHelper
    {
        public string VendorName
        {
            get;
            set;
        }

        public double Amount
        {
            get;
            set;
        }

        public VendorHelper(string strVendorName, double dblAmount)
        {
            this.VendorName = strVendorName;
            this.Amount = dblAmount;
        }
    }
}
