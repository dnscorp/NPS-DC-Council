using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;

namespace PRIFACT.DCCouncil.NPS.Core.NPSDataHelper
{
    public class ResultInfo
    {
        public int RowCount
        {
            get;
            set;
        }

        public List<IDataHelper> Items
        {
            get;
            set;
        }
    }

    public class POSummaryResultInfo
    {
        public Double POExpended
        {
            get;
            set;
        }
        public Double POObligated
        {
            get;
            set;
        }
    }
}
