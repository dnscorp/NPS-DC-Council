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
    public class PurchaseOrderAdhocReportViewModel
    {
        public DateTime AccountingDate { get; set; }
        public string PONumber { get; set; }
        public string VendorName { get; set; }
        public string IndexCode { get; set; }
        public string CostCenter { get; set; }
        public string PODescription { get; set; }
        public double Amount { get; set; }
        public string EntryType { get; set; }

        private static string GetDbConnectionString()
        {
            return AppSettings.DbConnectionString;
        }

        public static List<PurchaseOrderAdhocReportViewModel> GetAll(long fiscalYearId, List<long> officeIds, DateTime asOfDate)
        {
            return new SafeDBExecute<List<PurchaseOrderAdhocReportViewModel>>(delegate (DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_Adhoc_Report_PurchaseOrder_Yearly";

                param = cmd.Parameters.Add("@FiscalYearId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(fiscalYearId);

                param = cmd.Parameters.Add("@OfficeIds", SqlDbType.Structured);
                param.TypeName = "dbo.OfficeIdList";
                
                var dt = new DataTable();
                dt.Columns.Add("Id");
                foreach (var item in officeIds)
                {
                    dt.Rows.Add(item);
                }
                param.Value = dt;
                                
                param = cmd.Parameters.Add("@AsOfDate", SqlDbType.Date);
                param.Value = BasicConverter.DateToDbValue(asOfDate);

                var lstPurchaseOrderImportSummaries = new List<PurchaseOrderAdhocReportViewModel>();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        PurchaseOrderAdhocReportViewModel objHelper = _Bind(reader);
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

        private static PurchaseOrderAdhocReportViewModel _Bind(SqlDataReader reader)
        {
            var objSummary = new PurchaseOrderAdhocReportViewModel();
            objSummary.AccountingDate = BasicConverter.DbToDateValue(reader["AccountingDate"]);
            objSummary.PONumber = BasicConverter.DbToStringValue(reader["PONumber"]);
            objSummary.PODescription = BasicConverter.DbToStringValue(reader["PODescription"]);
            objSummary.VendorName = BasicConverter.DbToStringValue(reader["VendorName"]);
            objSummary.IndexCode = BasicConverter.DbToStringValue(reader["IndexCode"]);
            objSummary.CostCenter = BasicConverter.DbToStringValue(reader["CostCenter"]);
            objSummary.Amount = BasicConverter.DbToDoubleValue(reader["Amount"]);
            objSummary.EntryType = BasicConverter.DbToStringValue(reader["EntryType"]);

            return objSummary;
        }
    }
}
