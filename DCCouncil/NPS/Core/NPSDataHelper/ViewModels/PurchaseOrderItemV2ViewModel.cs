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
    public class PurchaseOrderItemV2ViewModel
    {
        public long PurchaseOrdersV2Id { get; set; }
        public string VendorName { get; set; }
        public DateTime AccountingDate { get; set; }
        public string PONumber { get; set; }
        public string PODescription { get; set; }
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
        public long? AlternateOfficeID { get; set; }


        private static string GetDbConnectionString()
        {
            return AppSettings.DbConnectionString;
        }

        public static PurchaseOrderItemV2ViewModel GetByPurchaseOrderID(long purchaseOrderID)
        {
            return new SafeDBExecute<PurchaseOrderItemV2ViewModel>(delegate (DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[Proc_PURCHASEORDER_V2_GetByPurchaseOrderId]";

                param = cmd.Parameters.Add("@PurchaseOrderId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(purchaseOrderID);

                PurchaseOrderItemV2ViewModel objPurchaseOrder = null;
                try
                {
                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        objPurchaseOrder = _Bind(reader);
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        DBContext.CloseReader(reader);
                    }
                }

                return objPurchaseOrder;

            }).DoExecute(GetDbConnectionString());
        }

        public static void Update(long PurchaseOrdersV2Id,long ExpenditureSubCategoryId,long AlternateOfficeID,bool IsTrainingExpense,string description)
        {
            new SafeDBExecute<bool>(delegate (DBContext dbContext)
            {
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_PURCHASEORDER_V2_Update";

                param = cmd.Parameters.Add("@PurchaseOrderId", SqlDbType.NVarChar);
                param.Value = BasicConverter.DoubleToDbValue(PurchaseOrdersV2Id);

                param = cmd.Parameters.Add("@ExpenditureSubCategoryId", SqlDbType.BigInt);
                param.Value = BasicConverter.DoubleToDbValue(ExpenditureSubCategoryId);

                param = cmd.Parameters.Add("@IsTrainingExpense", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(IsTrainingExpense);

                param = cmd.Parameters.Add("@AlternateOfficeID", SqlDbType.NVarChar);
                param.Value = BasicConverter.DoubleToDbValue(AlternateOfficeID);

                param = cmd.Parameters.Add("@PODescription", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(description);

                cmd.ExecuteNonQuery();

                return true;
            }).DoExecute(GetDbConnectionString());
        }

        public static void Delete(long PurchaseOrdersV2Id)
        {
            new SafeDBExecute<bool>(delegate (DBContext dbContext)
            {
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_PURCHASEORDER_V2_Delete";

                param = cmd.Parameters.Add("@PurchaseOrderId", SqlDbType.NVarChar);
                param.Value = BasicConverter.DoubleToDbValue(PurchaseOrdersV2Id);

                cmd.ExecuteNonQuery();

                return true;
            }).DoExecute(GetDbConnectionString());
        }

        private static PurchaseOrderItemV2ViewModel _Bind(SqlDataReader reader)
        {
            var objSummary = new PurchaseOrderItemV2ViewModel();
            objSummary.PurchaseOrdersV2Id = BasicConverter.DbToLongValue(reader["PurchaseOrdersV2Id"]);
            objSummary.VendorName = BasicConverter.DbToStringValue(reader["VendorName"]);
            objSummary.AccountingDate = BasicConverter.DbToDateValue(reader["AccountingDate"]);
            objSummary.PONumber = BasicConverter.DbToStringValue(reader["PONumber"]);
            objSummary.PODescription = BasicConverter.DbToStringValue(reader["PODescription"]);
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
            objSummary.AlternateOfficeID = BasicConverter.DbToLongValue(reader["AlternateOfficeID"]);

            return objSummary;
        }
    }
}
