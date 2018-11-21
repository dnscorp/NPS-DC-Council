using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRIFACT.PRIFACTBase.SQLHelpers;

namespace PRIFACT.DCCouncil.NPS.Core.NPSDataHelper
{
    public class ExpenditureCategoryAttribute
    {
        public long ExpenditureCategoryAttributeID
        {
            get;
            set;
        }

        public long ExpenditureCategoryAttributeLookupID
        {
            get;
            set;
        }

        public ExpenditureCategoryAttributeLookup ExpenditureCategoryAttributeLookup
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

        private static ExpenditureCategoryAttribute _Bind(System.Data.SqlClient.SqlDataReader reader)
        {
            ExpenditureCategoryAttribute objExpenditureCategoryAttribute = new ExpenditureCategoryAttribute();
            objExpenditureCategoryAttribute.AttributeValue = BasicConverter.DbToStringValue(reader["AttributeValue"]);
            objExpenditureCategoryAttribute.CreatedDate = BasicConverter.DbToDateValue(reader["CreatedDate"]);
            objExpenditureCategoryAttribute.ExpenditureCategoryAttributeID = BasicConverter.DbToLongValue(reader["ExpenditureCategoryAttributeID"]);
            objExpenditureCategoryAttribute.ExpenditureCategoryAttributeLookupID = BasicConverter.DbToLongValue(reader["ExpenditureCategoryAttributeLookupID"]);
            objExpenditureCategoryAttribute.ExpenditureCategoryAttributeLookup = ExpenditureCategoryAttributeLookup.Bind(reader);
            objExpenditureCategoryAttribute.UpdatedDate = BasicConverter.DbToNullableDateValue(reader["UpdatedDate"]);
            return objExpenditureCategoryAttribute;
        }

        public static List<ExpenditureCategoryAttribute> GetByCategoryId(long expenditureCategoryId)
        {
            return new SafeDBExecute<List<ExpenditureCategoryAttribute>>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_EXPENDITURECATEGORYATTRIBUTE_GetByExpenditureCategoryID";

                param = cmd.Parameters.Add("@ExpenditureCategoryID", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(expenditureCategoryId);

                List<ExpenditureCategoryAttribute> lstExpenditureCategoryAttributes = new List<ExpenditureCategoryAttribute>();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ExpenditureCategoryAttribute objExpenditureCategoryAttribute = _Bind(reader);
                        lstExpenditureCategoryAttributes.Add(objExpenditureCategoryAttribute);
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        DBContext.CloseReader(reader);
                    }
                }
                return lstExpenditureCategoryAttributes;
            }
           ).DoExecute(GetDbConnectionString());
        }

        private static string GetDbConnectionString()
        {
            return PRIFACT.DCCouncil.NPS.Core.NPSCommon.AppSettings.DbConnectionString;
        }
    }
}
