using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums;
using PRIFACT.PRIFACTBase.SQLHelpers;

namespace PRIFACT.DCCouncil.NPS.Core.NPSDataHelper
{
    public class TelephoneTransactionImportItems
    {
        public int ImportItemId { get; set; }
        public TelephoneTransactionSheetImportHelper TelephoneTransactionSheetImport { get; set; }
        public TelephoneChargesStatusAfterImport ImportStatus { get; set; }
        public Guid ImportGuid { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public static void Insert(Guid importGuid, string strImportXml, int iImportStatus)
        {
            new SafeDBExecute<bool>(delegate(DBContext dbContext)
            {

                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_TelephoneTransactionImportItems_Insert";

                param = cmd.Parameters.Add("@ImportGUID", SqlDbType.UniqueIdentifier);
                param.Value = BasicConverter.GuidToDbValue(importGuid);

                param = cmd.Parameters.Add("@StrImportXML", SqlDbType.Xml);
                param.Value = BasicConverter.StringToDbValue(strImportXml);

                param = cmd.Parameters.Add("@ImportStatus", SqlDbType.Int);
                param.Value = BasicConverter.IntToDbValue(iImportStatus);


                cmd.ExecuteNonQuery();

                return true;
            }).DoExecute(GetDbConnectionString());
        }

        public static void Process(Guid importGuid, long lFiscalYearID, string strCommentTxtForImport)
        {
            new SafeDBExecute<bool>(delegate(DBContext dbContext)
            {

                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[Proc_EXPENDITURE_ImportTelephoneTransactions]";

                param = cmd.Parameters.Add("@ImportGuid", SqlDbType.UniqueIdentifier);
                param.Value = BasicConverter.GuidToDbValue(importGuid);

                param = cmd.Parameters.Add("@FiscalYearID", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(lFiscalYearID);

                param = cmd.Parameters.Add("@CommentTxtForImport", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strCommentTxtForImport);

                cmd.ExecuteNonQuery();

                return true;
            }).DoExecute(GetDbConnectionString());
        }

        public static List<TelephoneTransactionImportItems> GetAllByGuid(Guid importGuid, int? importstatus)
        {
            return new SafeDBExecute<List<TelephoneTransactionImportItems>>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_TelephoneTransactionImportItems_GetAllByGuid";

                param = cmd.Parameters.Add("@ImportGUID", SqlDbType.UniqueIdentifier);
                param.Value = BasicConverter.GuidToDbValue(importGuid);

                param = cmd.Parameters.Add("@ImportStatus", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(importstatus);


                List<TelephoneTransactionImportItems> lstTelephoneTransactionImportItems = new List<TelephoneTransactionImportItems>();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        TelephoneTransactionImportItems objHelper = _Bind(reader); ;
                        lstTelephoneTransactionImportItems.Add(objHelper);
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        DBContext.CloseReader(reader);
                    }
                }
                return lstTelephoneTransactionImportItems;

            }).DoExecute(GetDbConnectionString());
        }

        public void Update()
        {
            new SafeDBExecute<bool>(delegate(DBContext dbContext)
            {
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_TelephoneTransactionImportItems_Update";

                param = cmd.Parameters.Add("@ImportId", SqlDbType.Int);
                param.Value = BasicConverter.IntToDbValue(this.ImportItemId);

                param = cmd.Parameters.Add("@ImportStatus", SqlDbType.Int);
                param.Value = BasicConverter.IntToDbValue(Convert.ToInt32(this.ImportStatus));

                param = cmd.Parameters.Add("@ImportGuid", SqlDbType.UniqueIdentifier);
                param.Value = BasicConverter.GuidToDbValue(this.ImportGuid);

                param = cmd.Parameters.Add("@FoundationAccount", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(this.TelephoneTransactionSheetImport.FoundationAccount);

                param = cmd.Parameters.Add("@BillingAccount", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(this.TelephoneTransactionSheetImport.BillingAccount);

                param = cmd.Parameters.Add("@WirelessNumber", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(this.TelephoneTransactionSheetImport.WirelessNumber);

                param = cmd.Parameters.Add("@UserName", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(this.TelephoneTransactionSheetImport.Username);

                param = cmd.Parameters.Add("@MarketCycleEndDate", SqlDbType.DateTime);
                param.Value = BasicConverter.DateToDbValue(this.TelephoneTransactionSheetImport.MarketCycleEndDate);

                param = cmd.Parameters.Add("@TotalKBUsage", SqlDbType.Float);
                param.Value = this.TelephoneTransactionSheetImport.TotalUsage;

                param = cmd.Parameters.Add("@TotalCurrentCharges", SqlDbType.Float);
                param.Value = this.TelephoneTransactionSheetImport.TotalCurrentCharges;

                param = cmd.Parameters.Add("@TotalNumberofEvents", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(this.TelephoneTransactionSheetImport.NumberOfEvents);

                param = cmd.Parameters.Add("@TotalMOUUsage", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(this.TelephoneTransactionSheetImport.MOUUsage);

                cmd.ExecuteNonQuery();

                return true;
            }).DoExecute(GetDbConnectionString());


        }
        private static TelephoneTransactionImportItems _Bind(SqlDataReader reader)
        {
            TelephoneTransactionImportItems objPhoneImportItems = new TelephoneTransactionImportItems();
            objPhoneImportItems.ImportItemId = BasicConverter.DbToIntValue(reader["ItemID"]);
            objPhoneImportItems.ImportGuid = BasicConverter.DbToGuidValue(reader["ImportGuid"]);
            objPhoneImportItems.TelephoneTransactionSheetImport = TelephoneTransactionSheetImportHelper.Bind(reader);
            objPhoneImportItems.ImportStatus = (TelephoneChargesStatusAfterImport)BasicConverter.DbToIntValue(reader["ImportStatus"]);
            objPhoneImportItems.CreatedDate = BasicConverter.DbToDateValue(reader["CreatedDate"]);
            objPhoneImportItems.UpdatedDate = BasicConverter.DbToNullableDateValue(reader["UpdatedDate"]);
            return objPhoneImportItems;
        }

        private static string GetDbConnectionString()
        {
            return AppSettings.DbConnectionString;
        }
    }

}
