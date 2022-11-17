using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.PRIFACTBase.SQLHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.ViewModels
{
    public class PurchaseOrderAdhocReportSummaryViewModel
    {
        public double Amount { get; set; }
        public string EntryType { get; set; }

        private static string GetDbConnectionString()
        {
            return AppSettings.DbConnectionString;
        }

        public static List<PurchaseOrderAdhocReportSummaryViewModel> GetSummary(long fiscalYearId, long officeId, DateTime asOfDate, DateTime? startDate)
        {
            //ReportType: 'NPS_ONLY','TRAINING_ONLY','NPS_AND_TRAINING'
            return new SafeDBExecute<List<PurchaseOrderAdhocReportSummaryViewModel>>(delegate (DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_Adhoc_Report_PurchaseOrder_Summary";

                param = cmd.Parameters.Add("@FiscalYearId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(fiscalYearId);

                param = cmd.Parameters.Add("@OfficeId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(officeId);
                                
                param = cmd.Parameters.Add("@AsOfDate", SqlDbType.Date);
                param.Value = BasicConverter.DateToDbValue(asOfDate);

                if (startDate.HasValue)
                {
                    param = cmd.Parameters.Add("@StartDate", SqlDbType.Date);
                    param.Value = BasicConverter.DateToDbValue(startDate.Value);
                }

                var lstPurchaseOrderImportSummaries = new List<PurchaseOrderAdhocReportSummaryViewModel>();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        PurchaseOrderAdhocReportSummaryViewModel objHelper = _Bind(reader);
                        lstPurchaseOrderImportSummaries.Add(objHelper);
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        DBContext.CloseReader(reader);
                    }
                }
                return lstPurchaseOrderImportSummaries;

            }).DoExecute(GetDbConnectionString());
        }

        private static PurchaseOrderAdhocReportSummaryViewModel _Bind(SqlDataReader reader)
        {
            var objSummary = new PurchaseOrderAdhocReportSummaryViewModel();            
            objSummary.Amount = BasicConverter.DbToDoubleValue(reader["Amount"]);
            objSummary.EntryType = BasicConverter.DbToStringValue(reader["EntryType"]);

            return objSummary;
        }
    }
}
