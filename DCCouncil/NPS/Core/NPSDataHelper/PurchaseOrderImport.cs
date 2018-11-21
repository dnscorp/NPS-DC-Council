using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.PRIFACTBase.SQLHelpers;

namespace PRIFACT.DCCouncil.NPS.Core.NPSDataHelper
{
    public class PurchaseOrderImport
    {
        public long? ImportID
        {
            get;
            set;
        }

        public Guid? ImportGUID
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            set;
        }

        public DateTime? CreatedDate
        {
            get;
            set;
        }

        public DateTime? UpdatedDate
        {
            get;
            set;
        }

        public static void Insert(Guid importGuid,string strFileName,string strItemsXml,byte[] fileBuffer)
        {
            new SafeDBExecute<bool>(delegate(DBContext dbContext)
            {

                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_PURCHASEORDERIMPORT_Insert";

                param = cmd.Parameters.Add("@ImportGUID", SqlDbType.UniqueIdentifier);
                param.Value = BasicConverter.GuidToDbValue(importGuid);

                param = cmd.Parameters.Add("@FileName", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strFileName);

                param = cmd.Parameters.Add("@ItemsXml", SqlDbType.Xml);
                param.Value = BasicConverter.StringToDbValue(strItemsXml);

                param = cmd.Parameters.Add("@ImportFile", SqlDbType.VarBinary);
                param.Value = fileBuffer; 

                cmd.ExecuteNonQuery();

                return true;
            }).DoExecute(GetDbConnectionString());

        }

        private static string GetDbConnectionString()
        {
            return AppSettings.DbConnectionString;
        }

        internal static PurchaseOrderImport Bind(SqlDataReader reader)
        {
            PurchaseOrderImport objImport = new PurchaseOrderImport();
            objImport.CreatedDate = BasicConverter.DbToNullableDateValue(reader["PurchaseOrderImportCreatedDate"]);
            objImport.FileName = BasicConverter.DbToStringValue(reader["PurchaseOrderImportFileName"]);
            objImport.ImportGUID = BasicConverter.DbToNullableGuidValue(reader["PurchaseOrderImportGUID"]);
            objImport.ImportID = BasicConverter.DbToNullableLongValue(reader["ImportID"]);
            objImport.UpdatedDate = BasicConverter.DbToNullableDateValue(reader["PurchaseOrderImportUpdatedDate"]);
            return objImport;
        }
    }
}
