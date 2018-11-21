using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary.Utilities
{
    public class MiscHelper
    {
        public static string GetColumnNameFromNumber(int columnNumber)
        {
            int i = columnNumber / 27;
            int j = columnNumber - (i * 26);
            string strPrefix = string.Empty;
            string strSuffix = string.Empty;
            if (i > 0)
            {
                 strPrefix = _GetChar(i);
            }
            if (j > 0)
            {
                strSuffix = _GetChar(j);
            }            
            return (strPrefix + strSuffix).ToUpper();
        }

        private static string _GetChar(int i)
        {
            char c = 'a';
            c+=(char)(i-1);
            return c.ToString();
        }
    }
}
