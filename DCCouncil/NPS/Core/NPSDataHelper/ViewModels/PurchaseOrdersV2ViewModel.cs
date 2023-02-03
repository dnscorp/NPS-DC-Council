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
    public class PurchaseOrdersV2ViewModel
    {
        public long PurchaseOrdersV2Id { get; set; }
        public string VendorName { get; set; }
        public DateTime AccountingDate { get; set; }
        public string PONumber { get; set; }
        public short POLineNumber { get; set; }
        public string POLineItemDescription { get; set; }
        public double POAmount { get; set; }
        public int Fund { get; set; }
        public string Program { get; set; }
        public string CostCenter { get; set; }
        public string NaturalAccount { get; set; }
        public string Project { get; set; }
        public string Award { get; set; }
        public string FundingSource { get; set; }
        public double ExpendedAmount { get; set; }
        public double POBalance { get; set; }
        public long? FiscalYearId { get; set; }
        public long? ExpenditureSubCategoryId { get; set; }
        public bool? IsTrainingExpense { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long OfficeId { get; set; }
        public string OfficeName { get; set; }
        public string ExpenditureSubCategoryName { get; set; }


        private static string GetDbConnectionString()
        {
            return AppSettings.DbConnectionString;
        }
        public static PurchaseOrdersV2ResultsViewModel GetAll(long fiscalYearId, string strSearchText, long? officeId, int? iPageSize, int? iPageNumber, NPSCommon.Enums.SortFields.PurchaseOrderV2SortField sortField, NPSCommon.Enums.OrderByDirection orderByDirection)
        {
            return new SafeDBExecute<PurchaseOrdersV2ResultsViewModel>(delegate (DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_POIMPORTSUMMARY_V2_GetAll";

                param = cmd.Parameters.Add("@FiscalYearId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(fiscalYearId);

                param = cmd.Parameters.Add("@SearchText", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strSearchText);

                param = cmd.Parameters.Add("@OfficeId", SqlDbType.BigInt);
                param.Value = BasicConverter.NullableLongToDbValue(officeId);

                param = cmd.Parameters.Add("@PageSize", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageSize);

                param = cmd.Parameters.Add("@PageNumber", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageNumber);

                param = cmd.Parameters.Add("@SortField", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(sortField));

                param = cmd.Parameters.Add("@SortDirection", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(orderByDirection));

                var lstPurchaseOrderImportSummaries = new List<PurchaseOrdersV2ViewModel>();
                var objResultInfo = new PurchaseOrdersV2ResultsViewModel();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        PurchaseOrdersV2ViewModel objHelper = _Bind(reader); ;
                        lstPurchaseOrderImportSummaries.Add(objHelper);
                    }
                    objResultInfo.Items = lstPurchaseOrderImportSummaries;
                    reader.NextResult();
                    if (reader.Read())
                    {
                        objResultInfo.RowCount = BasicConverter.DbToIntValue(reader["TotalRowCount"]);
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        DBContext.CloseReader(reader);
                    }
                }
                return objResultInfo;

            }).DoExecute(GetDbConnectionString());
        }               

        private static PurchaseOrdersV2ViewModel _Bind(SqlDataReader reader)
        {
            var objSummary = new PurchaseOrdersV2ViewModel();
            objSummary.PurchaseOrdersV2Id = BasicConverter.DbToLongValue(reader["PurchaseOrdersV2Id"]);
            objSummary.VendorName = BasicConverter.DbToStringValue(reader["VendorName"]);
            objSummary.AccountingDate = BasicConverter.DbToDateValue(reader["AccountingDate"]);
            objSummary.PONumber = BasicConverter.DbToStringValue(reader["PONumber"]);
            objSummary.POLineNumber = BasicConverter.DbToShortValue(reader["POLineNumber"]);
            objSummary.POLineItemDescription = BasicConverter.DbToStringValue(reader["POLineItemDescription"]);
            objSummary.POAmount = BasicConverter.DbToDoubleValue(reader["POAmount"]);
            objSummary.Fund = BasicConverter.DbToIntValue(reader["Fund"]);
            objSummary.Program = BasicConverter.DbToStringValue(reader["Program"]);
            objSummary.CostCenter = BasicConverter.DbToStringValue(reader["CostCenter"]);
            objSummary.NaturalAccount = BasicConverter.DbToStringValue(reader["NaturalAccount"]);
            objSummary.Project = BasicConverter.DbToStringValue(reader["Project"]);
            objSummary.Award = BasicConverter.DbToStringValue(reader["Award"]);
            objSummary.FundingSource = BasicConverter.DbToStringValue(reader["FundingSource"]);
            objSummary.ExpendedAmount = BasicConverter.DbToDoubleValue(reader["ExpendedAmount"]);
            objSummary.POBalance = BasicConverter.DbToDoubleValue(reader["POBalance"]);
            objSummary.FiscalYearId = BasicConverter.DbToLongValue(reader["FiscalYearId"]);
            objSummary.ExpenditureSubCategoryId = BasicConverter.DbToLongValue(reader["ExpenditureSubCategoryId"]);
            objSummary.IsTrainingExpense = BasicConverter.DbToBoolValue(reader["IsTrainingExpense"]);
            objSummary.CreatedDate = BasicConverter.DbToDateValue(reader["CreatedDate"]);
            objSummary.UpdatedDate = BasicConverter.DbToDateValue(reader["UpdatedDate"]);

            objSummary.OfficeId = BasicConverter.DbToLongValue(reader["OfficeId"]);
            objSummary.OfficeName = BasicConverter.DbToStringValue(reader["OfficeName"]);
            objSummary.ExpenditureSubCategoryName = BasicConverter.DbToStringValue(reader["ExpenditureSubCategoryName"]);

            return objSummary;
        }
    }
}
