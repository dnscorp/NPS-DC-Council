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
    [Serializable]
    public class User : IDataHelper
    {
        public long UserID
        {
            get;
            set;
        }

        public Guid UserGuid
        {
            get;
            set;
        }

        public string Username
        {
            get;
            set;
        }

        public string PasswordHash
        {
            set;
            get;
        }

        public bool IsActive
        {
            get;
            set;
        }

        public bool IsDeleted
        {
            get;
            set;
        }

        public long LastFiscalYearSelectedID
        {
            get;
            set;
        }

        public FiscalYear LastFiscalYearSelected
        {
            get;
            set;
        }

        public long UserProfileID
        {
            get;
            set;
        }

        public UserProfile UserProfile
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

        public static User Validate(string strUsername, string strPasswordHash)
        {
            return new SafeDBExecute<User>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_USER_Validate";

                param = cmd.Parameters.Add("@Username", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strUsername);

                param = cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strPasswordHash);

                User objUser = null;
                try
                {
                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        objUser = _Bind(reader);
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        DBContext.CloseReader(reader);
                    }
                }
                return objUser;
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
                cmd.CommandText = "Proc_USER_Update";

                param = cmd.Parameters.Add("@UserId", SqlDbType.Int);
                param.Value = BasicConverter.LongToDbValue(this.UserID);

                param = cmd.Parameters.Add("@Username", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(this.Username);

                param = cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(this.PasswordHash);

                param = cmd.Parameters.Add("@IsActive", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(this.IsActive);

                param = cmd.Parameters.Add("@IsDeleted", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(this.IsDeleted);

                param = cmd.Parameters.Add("@LastFiscalYearSelectedId", SqlDbType.Int);
                param.Value = BasicConverter.LongToDbValue(this.LastFiscalYearSelected.FiscalYearID);

                param = cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(this.UserProfile.FirstName);

                param = cmd.Parameters.Add("@LastName", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(this.UserProfile.LastName);

                cmd.ExecuteNonQuery();

                return true;
            }
           ).DoExecute(GetDbConnectionString());

        }

        public static void Create(string strUsername, string strPasswordHash, bool blnIsActive, bool blnIsDeleted, string strFirstName, string strLastName)
        {
            new SafeDBExecute<bool>(delegate(DBContext dbContext)
                {
                    SqlParameter param = null;
                    SqlCommand cmd = dbContext.ContextSqlCommand;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Proc_USER_Create";

                    param = cmd.Parameters.Add("@Username", SqlDbType.NVarChar);
                    param.Value = BasicConverter.StringToDbValue(strUsername);

                    param = cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar);
                    param.Value = BasicConverter.StringToDbValue(strPasswordHash);

                    param = cmd.Parameters.Add("@IsActive", SqlDbType.Bit);
                    param.Value = BasicConverter.BoolToDbValue(blnIsActive);

                    param = cmd.Parameters.Add("@IsDeleted", SqlDbType.Bit);
                    param.Value = BasicConverter.BoolToDbValue(blnIsDeleted);

                    param = cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar);
                    param.Value = BasicConverter.StringToDbValue(strFirstName);

                    param = cmd.Parameters.Add("@LastName", SqlDbType.NVarChar);
                    param.Value = BasicConverter.StringToDbValue(strLastName);

                    cmd.ExecuteNonQuery();

                    return true;
                }
               ).DoExecute(GetDbConnectionString());
        }

        public static ResultInfo GetAll(string strSearchText, int? iPageSize, int? iPageNumber, UserSortFields sortField, OrderByDirection orderByDirection)
        {
            return new SafeDBExecute<ResultInfo>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_USER_GetAll";

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

                List<IDataHelper> lstUsers = new List<IDataHelper>();
                ResultInfo objResultInfo = new ResultInfo();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        User objUser = new User();
                        objUser = _Bind(reader);
                        lstUsers.Add(objUser);
                    }
                    objResultInfo.Items = lstUsers;
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

        public static User GetByUserName(string strUsername)
        {
            return new SafeDBExecute<User>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_USER_GetByUserName";

                param = cmd.Parameters.Add("@Username", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strUsername);

                User objUser = null;
                try
                {
                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        objUser = _Bind(reader);
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        DBContext.CloseReader(reader);
                    }
                }
                return objUser;
            }
            ).DoExecute(GetDbConnectionString());
        }

        public static User GetByUserID(long lUserId)
        {
            return new SafeDBExecute<User>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_USER_GetByUserId";

                param = cmd.Parameters.Add("@UserId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(lUserId);

                User objUser = null;
                try
                {
                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        objUser = _Bind(reader);
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        DBContext.CloseReader(reader);
                    }
                }
                return objUser;
            }
            ).DoExecute(GetDbConnectionString());
        }


        internal static User _Bind(SqlDataReader reader)
        {
            User objUser = new User();
            objUser.CreatedDate = BasicConverter.DbToDateValue(reader["CreatedDate"]);
            objUser.IsActive = BasicConverter.DbToBoolValue(reader["IsActive"]);
            objUser.IsDeleted = BasicConverter.DbToBoolValue(reader["IsDeleted"]);
            objUser.UserProfileID = BasicConverter.DbToLongValue(reader["UserProfileID"]);
            objUser.UserProfile = UserProfile.Bind(reader);
            objUser.LastFiscalYearSelectedID = BasicConverter.DbToLongValue(reader["FiscalYearID"]);
            objUser.LastFiscalYearSelected = FiscalYear.Bind(reader);
            objUser.UpdatedDate = BasicConverter.DbToNullableDateValue(reader["UpdatedDate"]);
            objUser.UserGuid = BasicConverter.DbToGuidValue(reader["UserGuid"]);
            objUser.UserID = BasicConverter.DbToLongValue(reader["UserID"]);
            objUser.Username = BasicConverter.DbToStringValue(reader["Username"]);
            objUser.PasswordHash = BasicConverter.DbToStringValue(reader["PasswordHash"]);
            return objUser;
        }


        private static string GetDbConnectionString()
        {
            return AppSettings.DbConnectionString;
        }
    }
}
