using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.PRIFACTBase.SQLHelpers;
using System;
using System.Data;
using System.Data.SqlClient;

namespace PRIFACT.DCCouncil.NPS.Core.NPSDataHelper
{
    public class PurchaseOrderImport_V2
    {
        public int ImportBatchID { get; set; }
        public Guid ImportGuid {get;set;}
        public DateTime ImportDate { get; set; }

        public static void Insert(Guid importGuid)
        {
            new SafeDBExecute<bool>(delegate (DBContext dbContext)
            {

                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_PURCHASEORDERIMPORT_Insert";

                param = cmd.Parameters.Add("@ImportGuid", SqlDbType.UniqueIdentifier);
                param.Value = BasicConverter.GuidToDbValue(importGuid);

                cmd.ExecuteNonQuery();

                return true;
            }).DoExecute(GetDbConnectionString());

        }

        private static string GetDbConnectionString()
        {
            return AppSettings.DbConnectionString;
        }
    }

    
}
