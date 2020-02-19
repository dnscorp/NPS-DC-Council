using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Helpers
{
    public class ExpenditureSubCategorySummaryHelper
    {
        public long ExpenditureSubCategoryID { get; set; }
        public long OfficeID { get; set; }
        public DateTime DateOfTransaction { get; set; }
        public Double Amount { get; set; }
    }
}
