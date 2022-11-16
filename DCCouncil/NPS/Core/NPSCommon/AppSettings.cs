using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRIFACT.PRIFACTBase.ConfigHelpers;

namespace PRIFACT.DCCouncil.NPS.Core.NPSCommon
{
    public static class AppSettings
    {
        public static bool IsSSLEnabled
        {
            get
            {
                return (!string.IsNullOrEmpty(ConfigReader.GetValue("IsSSLEnabled")) && ConfigReader.GetValue("IsSSLEnabled") == "1") ? true : false;
            }
        }

        public static string DbConnectionString
        {
            get
            {
                return ConfigReader.GetValue("DbConnectionString");
            }
        }

        public static string DateFormat
        {
            get
            {
                return ConfigReader.GetValue("DateFormat");
            }
        }

        public static string DateSeparator
        {
            get
            {
                return ConfigReader.GetValue("DateSeparator");
            }
        }

        public static string DefaultCurrencySymbol
        {
            get
            {
                return ConfigReader.GetValue("DefaultCurrencySymbol");
            }
        }

        public static string ExcelTempLocationPath
        {
            get
            {
                return ConfigReader.GetValue("ExcelTempLocationPath");
            }
        }

        public static string ExcelImportTempLocationPath
        {
            get
            {
                return ConfigReader.GetValue("ExcelImportTempLocationPath");
            }
        }

        public static int MaxUploadFileSize
        {
            get
            {
                return Convert.ToInt32(ConfigReader.GetValue("MaxUploadFileSize"));
            }
        }

        public static string PurchaseOrderImportSheet3Name
        {
            get
            {
                return ConfigReader.GetValue("PurchaseOrderImportSheet3Name");
            }
        }
        public static string AgingBalanceSheetName
        {
            get
            {
                return ConfigReader.GetValue("AgingBalanceSheetName");
            }
        }
        public static string CloseoutBalanceSheetName
        {
            get
            {
                return ConfigReader.GetValue("CloseoutBalanceSheetName");
            }
        }
        public static string FirstRowFirstColumnHeaderName
        {
            get { return ConfigReader.GetValue("FirstRowFirstColumnHeaderName"); }
        }
        public static string ExcelSheetPrefix
        {
            get
            {
                return ConfigReader.GetValue("ExcelSheetPrefix");

            }
        }
        public static string ExcelAdhocSheetPrefix
        {
            get
            {
                return ConfigReader.GetValue("ExcelAdhocSheetPrefix");

            }
        }
        public static string ExcelExpenditureSheetPrefix
        {
            get
            {
                return ConfigReader.GetValue("ExcelExpenditureSheetPrefix");

            }
        }
        public static string PasswordComplexityRegEx
        {
            get
            {
                return ConfigReader.GetValue("PasswordComplexityRegEx");

            }
        }
        public static string PasswordComplexityErrorMessage
        {
            get
            {
                return ConfigReader.GetValue("PasswordComplexityErrorMessage");

            }
        }

        public static int FiscalYearStartMonth
        {
            get
            {
                return Convert.ToInt32(ConfigReader.GetValue("FiscalYearStartMonth"));
            }
        }

        public static int FiscalYearStartDay
        {
            get
            {
                return Convert.ToInt32(ConfigReader.GetValue("FiscalYearStartDay"));
            }
        }

        public static int FiscalYearMonthDuration
        {
            get
            {
                return Convert.ToInt32(ConfigReader.GetValue("FiscalYearMonthDuration"));
            }
        }
        public static string DefaultCSVFileSavePath
        {
            get
            {
                return ConfigReader.GetValue("DefaultCSVFileSavePath");

            }
        }

        public static string TelephoneChargesImportDateFormat
        {
            get
            {
                return ConfigReader.GetValue("TelephoneChargesImportDateFormat");
            }
        }
        public static string CommentTextForImport
        {
            get
            {
                return ConfigReader.GetValue("CommentTextForImport");
            }
        }

        public static string ReportType_NPS_Only = "NPS_ONLY";
        public static string ReportType_Training_Only = "TRAINING_ONLY";
        public static string ReportType_NPS_And_Training = "NPS_AND_TRAINING";

    }
}
