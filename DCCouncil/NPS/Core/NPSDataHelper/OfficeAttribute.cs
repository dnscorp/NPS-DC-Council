using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.PRIFACTBase.SQLHelpers;

namespace PRIFACT.DCCouncil.NPS.Core.NPSDataHelper
{
    public class OfficeAttribute : IDataHelper
    {
        public long OfficeAttributeID
        {
            get;
            set;
        }

        public long OfficeAttributeLookupID
        {
            get;
            set;
        }

        public OfficeAttributeLookup OfficeAttributeLookup
        {
            get;
            set;
        }

        public string AttributeValue
        {
            get;
            set;
        }
        public DateTime CreatedDate
        {
            get;
            set;
        }

        public DateTime? UpdatedDate
        {
            get;
            set;
        }



        internal static List<OfficeAttribute> GetAllByOfficeId(long lOfficeId)
        {
            return new SafeDBExecute<List<OfficeAttribute>>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_OFFICEATTRIBUTE_GetByExpenditureCategoryID";

                param = cmd.Parameters.Add("@OfficeID", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(lOfficeId);

                OfficeAttribute objOfficeAttribute = null;
                List<OfficeAttribute> lstOfficeAttributes = new List<OfficeAttribute>();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        objOfficeAttribute = _Bind(reader);
                        lstOfficeAttributes.Add(objOfficeAttribute);
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        DBContext.CloseReader(reader);
                    }
                }
                return lstOfficeAttributes;
            }
           ).DoExecute(GetDbConnectionString());
        }

        private static string GetDbConnectionString()
        {
            return PRIFACT.DCCouncil.NPS.Core.NPSCommon.AppSettings.DbConnectionString;
        }

        private static OfficeAttribute _Bind(System.Data.SqlClient.SqlDataReader reader)
        {
            OfficeAttribute objOfficeAttribute = new OfficeAttribute();
            objOfficeAttribute.AttributeValue = BasicConverter.DbToStringValue(reader["AttributeValue"]);
            objOfficeAttribute.CreatedDate = BasicConverter.DbToDateValue(reader["CreatedDate"]);
            objOfficeAttribute.OfficeAttributeID = BasicConverter.DbToLongValue(reader["OfficeAttributeID"]);
            objOfficeAttribute.OfficeAttributeLookupID = BasicConverter.DbToLongValue(reader["OfficeAttributeLookupID"]);
            objOfficeAttribute.OfficeAttributeLookup = OfficeAttributeLookup.Bind(reader);
            objOfficeAttribute.UpdatedDate = BasicConverter.DbToNullableDateValue(reader["UpdatedDate"]);
            return objOfficeAttribute;
        }
    }
}
