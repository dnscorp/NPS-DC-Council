using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.PRIFACTBase.SQLHelpers;

namespace PRIFACT.DCCouncil.NPS.Core.NPSDataHelper
{
    [Serializable]
    public class FiscalYear : IDataHelper
    {
        public long FiscalYearID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int Year
        {
            get;
            set;
        }

        public DateTime StartDate
        {
            get;
            set;
        }

        public DateTime EndDate
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
        
        public static ResultInfo GetAll(int? iPageSize, int? iPageNumber, NPSCommon.Enums.SortFields.FiscalYearSortFields sortField, NPSCommon.Enums.OrderByDirection orderByDirection)
        {
            return new SafeDBExecute<ResultInfo>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_FISCALYEAR_GetAll";

                param = cmd.Parameters.Add("@PageSize", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageSize);

                param = cmd.Parameters.Add("@PageNumber", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageNumber);

                param = cmd.Parameters.Add("@SortField", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(sortField));

                param = cmd.Parameters.Add("@SortDirection", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(orderByDirection));

                List<IDataHelper> lstFiscalYears = new List<IDataHelper>();
                ResultInfo objResultInfo = new ResultInfo();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        FiscalYear objFiscalYear = new FiscalYear();
                        objFiscalYear = _Bind(reader);
                        lstFiscalYears.Add(objFiscalYear);
                    }
                    objResultInfo.Items = lstFiscalYears;
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

        private static FiscalYear _Bind(SqlDataReader reader)
        {
            FiscalYear objFiscalYear = new FiscalYear();
            objFiscalYear.CreatedDate = BasicConverter.DbToDateValue(reader["CreatedDate"]);
            objFiscalYear.FiscalYearID = BasicConverter.DbToLongValue(reader["FiscalYearID"]);            
            objFiscalYear.Name = BasicConverter.DbToStringValue(reader["Name"]);
            objFiscalYear.StartDate = BasicConverter.DbToDateValue(reader["StartDate"]);
            objFiscalYear.EndDate = BasicConverter.DbToDateValue(reader["EndDate"]);
            objFiscalYear.UpdatedDate = BasicConverter.DbToNullableDateValue(reader["UpdatedDate"]);
            objFiscalYear.Year = BasicConverter.DbToIntValue(reader["Year"]);            
            return objFiscalYear;
        }

        public static ResultInfo Search(string strSearchText, int? iPageSize, int? iPageNumber, NPSCommon.Enums.SortFields.FiscalYearSortFields sortField, NPSCommon.Enums.OrderByDirection orderByDirection)
        {
            return new SafeDBExecute<ResultInfo>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_FISCALYEAR_Search";

                param = cmd.Parameters.Add("@SearchText", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strSearchText);

                param = cmd.Parameters.Add("@PageSize", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageSize);

                param = cmd.Parameters.Add("@PageNumber", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageNumber);

                param = cmd.Parameters.Add("@SortField", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(sortField));

                param = cmd.Parameters.Add("@SortDirection", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(orderByDirection));

                List<IDataHelper> lstFiscalYears = new List<IDataHelper>();
                ResultInfo objResultInfo = new ResultInfo();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        FiscalYear objFiscalYear = new FiscalYear();
                        objFiscalYear = _Bind(reader);
                        lstFiscalYears.Add(objFiscalYear);
                    }
                    objResultInfo.Items = lstFiscalYears;
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

        public static FiscalYear GetByFiscalYearID(long lFiscalYearId)
        {
            return new SafeDBExecute<FiscalYear>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_FISCALYEAR_GetByFiscalYearId";

                param = cmd.Parameters.Add("@FiscalYearId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(lFiscalYearId);

                FiscalYear objFiscalYear = null;
                try
                {
                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        objFiscalYear = _Bind(reader);
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        DBContext.CloseReader(reader);
                    }
                }
                return objFiscalYear;
            }
            ).DoExecute(GetDbConnectionString());
        }

        public void Update()
        {
            new SafeDBExecute<bool>(delegate(DBContext dbContext)
            {
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_FISCALYEAR_Update";

                param = cmd.Parameters.Add("@FiscalYearId", SqlDbType.Int);
                param.Value = BasicConverter.LongToDbValue(this.FiscalYearID);

                param = cmd.Parameters.Add("@Name", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(this.Name);
                
                param = cmd.Parameters.Add("@Year", SqlDbType.Int);
                param.Value = BasicConverter.IntToDbValue(this.Year);

                param = cmd.Parameters.Add("@StartDate", SqlDbType.DateTime);
                param.Value = BasicConverter.DateToDbValue(this.StartDate);

                param = cmd.Parameters.Add("@EndDate", SqlDbType.DateTime);
                param.Value = BasicConverter.DateToDbValue(this.EndDate);

                cmd.ExecuteNonQuery();

                return true;
            }
           ).DoExecute(GetDbConnectionString());
        }

        public static void Create(string strName, int year, DateTime dtStartDate,DateTime dtEndDate)
        {
            new SafeDBExecute<bool>(delegate(DBContext dbContext)
            {
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_FISCALYEAR_Create";

                param = cmd.Parameters.Add("@Name", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strName);

                param = cmd.Parameters.Add("@Year", SqlDbType.Int);
                param.Value = BasicConverter.IntToDbValue(year);

                param = cmd.Parameters.Add("@StartDate", SqlDbType.DateTime);
                param.Value = BasicConverter.DateToDbValue(dtStartDate);

                param = cmd.Parameters.Add("@EndDate", SqlDbType.DateTime);
                param.Value = BasicConverter.DateToDbValue(dtEndDate);
                
                cmd.ExecuteNonQuery();

                return true;
            }
               ).DoExecute(GetDbConnectionString());
        }

        private static string GetDbConnectionString()
        {
            return AppSettings.DbConnectionString;
        }

        public static FiscalYear Bind(SqlDataReader reader)
        {
            FiscalYear objFiscalYear = new FiscalYear();
            objFiscalYear.CreatedDate = BasicConverter.DbToDateValue(reader["FiscalYearCreatedDate"]);
            objFiscalYear.FiscalYearID = BasicConverter.DbToLongValue(reader["FiscalYearID"]);
            objFiscalYear.Name = BasicConverter.DbToStringValue(reader["FiscalYearName"]);
            objFiscalYear.StartDate = BasicConverter.DbToDateValue(reader["FiscalYearStartDate"]);
            objFiscalYear.EndDate = BasicConverter.DbToDateValue(reader["FiscalYearEndDate"]);
            objFiscalYear.UpdatedDate = BasicConverter.DbToNullableDateValue(reader["FiscalYearUpdatedDate"]);
            objFiscalYear.Year = BasicConverter.DbToIntValue(reader["FiscalYearYear"]);            
            return objFiscalYear;
        }
    }
}
