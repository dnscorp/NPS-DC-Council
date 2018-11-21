using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.SortFields;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.PRIFACTBase.SQLHelpers;

namespace PRIFACT.DCCouncil.NPS.Core.NPSDataHelper
{
    public class Vendor : IDataHelper
    {
        public long VendorID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string DefaultDescription
        {
            get;
            set;
        }

        public long FiscalYearID
        {
            get;
            set;
        }

        public FiscalYear FiscalYear
        {
            get;
            set;
        }

        public long OfficeID
        {
            get;
            set;
        }

        public Office Office
        {
            get;
            set;
        }

        public bool IsRolledUp
        {
            get;
            set;
        }

        public bool IsDeleted
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

        private static string GetDbConnectionString()
        {
            return AppSettings.DbConnectionString;
        }

        public void Update()
        {
            new SafeDBExecute<bool>(delegate(DBContext dbContext)
            {
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_VENDOR_Update";

                param = cmd.Parameters.Add("@VendorID", SqlDbType.BigInt);
                param.Value = BasicConverter.DoubleToDbValue(this.VendorID);

                param = cmd.Parameters.Add("@OfficeID", SqlDbType.BigInt);
                param.Value = BasicConverter.DoubleToDbValue(this.OfficeID);

                param = cmd.Parameters.Add("@FiscalYearID", SqlDbType.BigInt);
                param.Value = BasicConverter.DoubleToDbValue(this.FiscalYearID);

                param = cmd.Parameters.Add("@DefaultDescription", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(this.DefaultDescription);

                param = cmd.Parameters.Add("@Name", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(this.Name);

                param = cmd.Parameters.Add("@IsRolledUp", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(this.IsRolledUp);

                param = cmd.Parameters.Add("@IsDeleted", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(this.IsDeleted);

                cmd.ExecuteNonQuery();

                return true;
            }).DoExecute(GetDbConnectionString());
        }

        public static ResultInfo GetAll(string strSearchText, string strOfficeXml, int? iPageSize, int? iPageNumber, VendorSortFields sortField, OrderByDirection orderByDirection)
        {
            return new SafeDBExecute<ResultInfo>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_VENDOR_GetAll";

                param = cmd.Parameters.Add("@SearchText", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strSearchText);

                param = cmd.Parameters.Add("@OfficeIds", SqlDbType.Xml);
                param.Value = BasicConverter.StringToDbValue(strOfficeXml);

                param = cmd.Parameters.Add("@PageSize", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageSize);

                param = cmd.Parameters.Add("@PageNumber", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageNumber);

                param = cmd.Parameters.Add("@SortField", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(sortField));

                param = cmd.Parameters.Add("@SortDirection", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(orderByDirection));

                List<IDataHelper> lstOffices = new List<IDataHelper>();
                ResultInfo objResultInfo = new ResultInfo();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Vendor objVendor = new Vendor();
                        objVendor = _Bind(reader);
                        lstOffices.Add(objVendor);
                    }
                    objResultInfo.Items = lstOffices;
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

        private static Vendor _Bind(SqlDataReader reader)
        {
            Vendor objVendor = new Vendor();
            objVendor.CreatedDate = BasicConverter.DbToDateValue(reader["CreatedDate"]);
            objVendor.DefaultDescription = BasicConverter.DbToStringValue(reader["DefaultDescription"]);
            objVendor.IsDeleted = BasicConverter.DbToBoolValue(reader["IsDeleted"]);
            objVendor.IsRolledUp = BasicConverter.DbToBoolValue(reader["IsRolledUp"]);
            objVendor.Name = BasicConverter.DbToStringValue(reader["Name"]);
            objVendor.Office = Office.Bind(reader);
            objVendor.FiscalYearID = BasicConverter.DbToLongValue(reader["FiscalYearID"]);
            objVendor.FiscalYear = FiscalYear.Bind(reader);
            objVendor.OfficeID = BasicConverter.DbToLongValue(reader["OfficeID"]);
            objVendor.UpdatedDate = BasicConverter.DbToNullableDateValue(reader["UpdatedDate"]);
            objVendor.VendorID = BasicConverter.DbToLongValue(reader["VendorID"]);
            return objVendor;
        }

        public static void Create(string strVendorName, string strDescription, long lOfficeID, long lFiscalYearID, bool bIsRolledUp, bool blnIsDeleted)
        {
            new SafeDBExecute<bool>(delegate(DBContext dbContext)
            {

                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_VENDOR_Create";

                param = cmd.Parameters.Add("@VendorName", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strVendorName);

                param = cmd.Parameters.Add("@Description", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strDescription);

                param = cmd.Parameters.Add("@OfficeID", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(lOfficeID);

                param = cmd.Parameters.Add("@FiscalYearId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(lFiscalYearID);
                
                param = cmd.Parameters.Add("@IsRolledUp", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(bIsRolledUp);

                param = cmd.Parameters.Add("@IsDeleted", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(blnIsDeleted);

                cmd.ExecuteNonQuery();

                return true;
            }).DoExecute(GetDbConnectionString());

        }
        public static Vendor GetByID(long lVendorID)
        {
            return new SafeDBExecute<Vendor>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_VENDOR_GetById";

                param = cmd.Parameters.Add("@VendorID", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(lVendorID);

                Vendor objVendor = null;
                try
                {
                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        objVendor = _Bind(reader);
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        DBContext.CloseReader(reader);
                    }
                }
                return objVendor;
            }
            ).DoExecute(GetDbConnectionString());
        }


        public static Vendor GetByNameAndOffice(string strVendorName, long lOfficeId)
        {
            return new SafeDBExecute<Vendor>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_VENDOR_GetByNameAndOffice";

                param = cmd.Parameters.Add("@VendorName", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strVendorName);

                param = cmd.Parameters.Add("@OfficeID", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(lOfficeId);

                Vendor objVendor = null;
                try
                {
                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        objVendor = _Bind(reader);
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        DBContext.CloseReader(reader);
                    }
                }
                return objVendor;
            }
            ).DoExecute(GetDbConnectionString());
        }
    }
}
