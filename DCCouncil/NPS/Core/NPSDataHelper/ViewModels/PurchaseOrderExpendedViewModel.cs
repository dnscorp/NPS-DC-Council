using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.PRIFACTBase.SQLHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.ViewModels
{
    public class PurchaseOrderExpendedViewModel
    {
        public string VendorName { get; set; }
        public double ExpendedAmount { get; set; }

        private static string GetDbConnectionString()
        {
            return AppSettings.DbConnectionString;
        }

        public static List<PurchaseOrderExpendedViewModel> GetAll(long fiscalYearId, long officeId, string reportType, DateTime asOfDate)
        {
            //ReportType: 'NPS_ONLY','TRAINING_ONLY','NPS_AND_TRAINING'
            return new SafeDBExecute<List<PurchaseOrderExpendedViewModel>>(delegate (DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_NPS_Report_ExpenditureSummary";

                param = cmd.Parameters.Add("@FiscalYearId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(fiscalYearId);

                param = cmd.Parameters.Add("@OfficeId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(officeId);

                param = cmd.Parameters.Add("@ReportType", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(reportType);

                param = cmd.Parameters.Add("@AsOfDate", SqlDbType.Date);
                param.Value = BasicConverter.DateToDbValue(asOfDate);

                var lstPurchaseOrderImportSummaries = new List<PurchaseOrderExpendedViewModel>();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        PurchaseOrderExpendedViewModel objHelper = _Bind(reader);
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

        private static PurchaseOrderExpendedViewModel _Bind(SqlDataReader reader)
        {
            var objSummary = new PurchaseOrderExpendedViewModel();
            objSummary.VendorName = BasicConverter.DbToStringValue(reader["VendorName"]);
            objSummary.ExpendedAmount = BasicConverter.DbToDoubleValue(reader["ExpendedAmount"]);

            return objSummary;
        }
    }
}
