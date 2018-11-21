using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary.Utilities
{
    public class DateHelper
    {
        public static string GetShortDateString(DateTime dtDate)
        {
            return dtDate.ToString("MMMM dd, yyyy");
        }

        internal static string GetMonthAndYear(DateTime dtDate)
        {
            return dtDate.ToString("MMMM yyyy");
        }
    }
}
