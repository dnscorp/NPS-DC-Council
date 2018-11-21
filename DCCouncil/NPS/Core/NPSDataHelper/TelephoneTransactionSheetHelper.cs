using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums;
using PRIFACT.PRIFACTBase.SQLHelpers;

namespace PRIFACT.DCCouncil.NPS.Core.NPSDataHelper
{
    public class TelephoneTransactionSheetImportHelper
    {
        public string FoundationAccount { get; set; }
        public string BillingAccount { get; set; }
        public string Username { get; set; }
        public string WirelessNumber { get; set; }
        public DateTime MarketCycleEndDate { get; set; }
        public string MarketCycleEndDateFieldValue { get; set; }
        public string TotalUsage { get; set; }
        public string NumberOfEvents { get; set; }
        public string MOUUsage { get; set; }
        public double TotalCurrentCharges { get; set; }
        public string TotalCurrentChargesFieldValue { get; set; }
        public TelephoneChargesStatusBeforeImport ImportStatusBeforeImport { get; set; }

        internal static TelephoneTransactionSheetImportHelper Bind(System.Data.SqlClient.SqlDataReader reader)
        {
            TelephoneTransactionSheetImportHelper TelephoneTransactionSheetImport = new TelephoneTransactionSheetImportHelper();
            TelephoneTransactionSheetImport.FoundationAccount = BasicConverter.DbToStringValue(reader["FoundationAccount"]);
            TelephoneTransactionSheetImport.BillingAccount = BasicConverter.DbToStringValue(reader["BillingAccount"]);
            TelephoneTransactionSheetImport.WirelessNumber = BasicConverter.DbToStringValue(reader["WirelessNumber"]);
            TelephoneTransactionSheetImport.Username = BasicConverter.DbToStringValue(reader["UserName"]);
            TelephoneTransactionSheetImport.MarketCycleEndDate = BasicConverter.DbToDateValue(reader["MarketCycleEndDate"]);
            TelephoneTransactionSheetImport.TotalUsage = BasicConverter.DbToStringValue(reader["TotalKBUsage"]);
            TelephoneTransactionSheetImport.NumberOfEvents = BasicConverter.DbToStringValue(reader["TotalNumberofEvents"]);
            TelephoneTransactionSheetImport.MOUUsage = BasicConverter.DbToStringValue(reader["TotalMOUUsage"]);
            TelephoneTransactionSheetImport.TotalCurrentCharges = BasicConverter.DbToDoubleValue(reader["TotalCurrentCharges"]);
            return TelephoneTransactionSheetImport;
        }
    }
    
}
