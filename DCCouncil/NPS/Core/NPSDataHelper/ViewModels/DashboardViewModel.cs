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
    public class DashboardViewModel
    {
        public string OfficeName { get; set; }
        public double BudgetAmount { get; set; }
        public double TotalExpense { get; set; }
        public double BurnRate { get; set; }

        private static string GetDbConnectionString()
        {
            return AppSettings.DbConnectionString;
        }

        public static List<DashboardViewModel> GetAllByReportType(long fiscalYearId,string reportType)
        {
            return new SafeDBExecute<List<DashboardViewModel>>(delegate (DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_GetDashboardStatsByFiscalYearId_V2";

                param = cmd.Parameters.Add("@FiscalYearId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(fiscalYearId);

                param = cmd.Parameters.Add("@ReportType", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(reportType);

                var lstPurchaseOrderImportSummaries = new List<DashboardViewModel>();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        DashboardViewModel objHelper = _Bind(reader);
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

        private static DashboardViewModel _Bind(SqlDataReader reader)
        {
            var objSummary = new DashboardViewModel();
            objSummary.OfficeName = BasicConverter.DbToStringValue(reader["OfficeName"]);
            objSummary.TotalExpense = BasicConverter.DbToDoubleValue(reader["TotalExpense"]);
            objSummary.BudgetAmount = BasicConverter.DbToDoubleValue(reader["BudgetAmount"]);
            objSummary.BurnRate = BasicConverter.DbToDoubleValue(reader["BurnRate"]);
            
            return objSummary;
        }
    }
}
