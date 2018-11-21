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
    public class Staff : IDataHelper
    {
        public long StaffId
        {
            get;
            set;
        }

        public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }
        public DateTime ActiveFrom
        {
            get;
            set;
        }

        public DateTime? ActiveTo
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

        public bool HasStaffLevelExpenditures
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
        public string WirelessNumber
        {
            get;
            set;
        }

        private static string GetDbConnectionString()
        {
            return AppSettings.DbConnectionString;
        }

        public static void Create(string strFirstName, string strLastName, bool blnHasStaffLevelExpenditures, long lngOfficeId, DateTime dtActiveFrom, DateTime? dtActiveTo, bool blnIsDeleted, string strWirelessNumber)
        {
            new SafeDBExecute<bool>(delegate(DBContext dbContext)
            {
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_STAFF_Create";

                param = cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strFirstName);

                param = cmd.Parameters.Add("@LastName", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strLastName);

                param = cmd.Parameters.Add("@HasStaffLevelExpenditures", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(blnHasStaffLevelExpenditures);

                param = cmd.Parameters.Add("@OfficeID", SqlDbType.BigInt);
                param.Value = BasicConverter.DbToLongValue(lngOfficeId);

                param = cmd.Parameters.Add("@ActiveFrom", SqlDbType.DateTime);
                param.Value = BasicConverter.DateToDbValue(dtActiveFrom);

                param = cmd.Parameters.Add("@ActiveTo", SqlDbType.DateTime);
                param.Value = BasicConverter.NullableDateToDbValue(dtActiveTo);

                param = cmd.Parameters.Add("@IsDeleted", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(blnIsDeleted);

                param = cmd.Parameters.Add("@WirelessNumber", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strWirelessNumber);

                cmd.ExecuteNonQuery();
                return true;
            }).DoExecute(GetDbConnectionString());
        }

        public void Update()
        {
            new SafeDBExecute<bool>(delegate(DBContext dbContext)
            {
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_STAFF_Update";

                param = cmd.Parameters.Add("@StaffID", SqlDbType.BigInt);
                param.Value = BasicConverter.DoubleToDbValue(this.StaffId);

                param = cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(this.FirstName);

                param = cmd.Parameters.Add("@LastName", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(this.LastName);

                param = cmd.Parameters.Add("@HasStaffLevelExpenditures", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(this.HasStaffLevelExpenditures);

                param = cmd.Parameters.Add("@ActiveFrom", SqlDbType.DateTime);
                param.Value = BasicConverter.DateToDbValue(this.ActiveFrom);

                param = cmd.Parameters.Add("@ActiveTo", SqlDbType.DateTime);
                param.Value = BasicConverter.NullableDateToDbValue(this.ActiveTo);

                param = cmd.Parameters.Add("@OfficeID", SqlDbType.BigInt);
                param.Value = BasicConverter.DoubleToDbValue(this.Office.OfficeID);

                param = cmd.Parameters.Add("@IsDeleted", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(this.IsDeleted);

                param = cmd.Parameters.Add("@WirelessNumber", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(this.WirelessNumber);


                cmd.ExecuteNonQuery();

                return true;
            }).DoExecute(GetDbConnectionString());
        }

        public static ResultInfo GetAllStaffs()
        {
            return new SafeDBExecute<ResultInfo>(delegate(DBContext dbContext)
                {
                    SqlDataReader reader = null;
                    SqlCommand cmd = dbContext.ContextSqlCommand;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[Proc_STAFF_GetAllStaffs]";

                    List<IDataHelper> lstStaff = new List<IDataHelper>();
                    ResultInfo objResultInfo = new ResultInfo();
                    try
                    {
                        reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            Staff objStaff = new Staff();
                            objStaff = _Bind(reader);
                            lstStaff.Add(objStaff);
                        }
                        objResultInfo.Items = lstStaff;
                        reader.NextResult();

                    }
                    finally
                    {
                        if (reader != null)
                        {
                            DBContext.CloseReader(reader);
                        }
                    }
                    return objResultInfo;

                }

                ).DoExecute(GetDbConnectionString());

        }

        public static ResultInfo GetAllByOfficeId(long lOfficeId, bool blnOnlyStaffWithStaffLevelExpenditure, int? iPageSize, int? iPageNumber, StaffSortFields sortField, OrderByDirection orderByDirection, int FiscalYear = 0, DateTime? AsOfDate = null)
        {
            return new SafeDBExecute<ResultInfo>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_STAFF_GetAll";

                param = cmd.Parameters.Add("@OfficeId", SqlDbType.BigInt);
                param.Value = BasicConverter.DoubleToDbValue(lOfficeId);

                param = cmd.Parameters.Add("@blnOnlyStaffWithStaffLevelExpenditure", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(blnOnlyStaffWithStaffLevelExpenditure);

                param = cmd.Parameters.Add("@PageSize", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageSize);

                param = cmd.Parameters.Add("@PageNumber", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageNumber);

                param = cmd.Parameters.Add("@SortField", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(sortField));

                param = cmd.Parameters.Add("@SortDirection", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(orderByDirection));

                param = cmd.Parameters.Add("@FiscalYear", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(FiscalYear));

                param = cmd.Parameters.Add("@AsOfDate", SqlDbType.DateTime);
                param.Value = BasicConverter.NullableDateToDbValue(AsOfDate);

                List<IDataHelper> lstStaff = new List<IDataHelper>();
                ResultInfo objResultInfo = new ResultInfo();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Staff objStaff = new Staff();
                        objStaff = _Bind(reader);
                        lstStaff.Add(objStaff);
                    }
                    objResultInfo.Items = lstStaff;
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

        private static Staff _Bind(SqlDataReader reader)
        {
            Staff objStaff = new Staff();
            objStaff.StaffId = BasicConverter.DbToLongValue(reader["StaffID"]);
            objStaff.FirstName = BasicConverter.DbToStringValue(reader["FirstName"]);
            objStaff.LastName = BasicConverter.DbToStringValue(reader["LastName"]);
            objStaff.HasStaffLevelExpenditures = BasicConverter.DbToBoolValue(reader["HasStaffLevelExpenditures"]);
            objStaff.ActiveTo = BasicConverter.DbToNullableDateValue(reader["ActiveTo"]);
            objStaff.ActiveFrom = BasicConverter.DbToDateValue(reader["ActiveFrom"]);
            objStaff.IsDeleted = BasicConverter.DbToBoolValue(reader["IsDeleted"]);
            objStaff.OfficeID = BasicConverter.DbToLongValue(reader["OfficeID"]);
            objStaff.WirelessNumber = BasicConverter.DbToStringValue(reader["WirelessNumber"]);
            objStaff.Office = Office.Bind(reader);
            objStaff.CreatedDate = BasicConverter.DbToDateValue(reader["CreatedDate"]);
            objStaff.UpdatedDate = BasicConverter.DbToNullableDateValue(reader["UpdatedDate"]);
            return objStaff;
        }

        public static Staff GetByStaffID(long lStaffId)
        {
            return new SafeDBExecute<Staff>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_STAFF_GetByStaffId";

                param = cmd.Parameters.Add("@StaffId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(lStaffId);

                Staff objStaff = null;
                try
                {
                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        objStaff = _Bind(reader);
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        DBContext.CloseReader(reader);
                    }
                }
                return objStaff;
            }
            ).DoExecute(GetDbConnectionString());
        }

        public static ResultInfo Search(string strSearchText, long lOfficeId, int? iPageSize, int? iPageNumber, StaffSortFields sortField, OrderByDirection orderByDirection)
        {
            return new SafeDBExecute<ResultInfo>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_STAFF_Search";

                param = cmd.Parameters.Add("@SearchText", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strSearchText);

                param = cmd.Parameters.Add("@OfficeId", SqlDbType.BigInt);
                param.Value = BasicConverter.DoubleToDbValue(lOfficeId);

                param = cmd.Parameters.Add("@PageSize", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageSize);

                param = cmd.Parameters.Add("@PageNumber", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageNumber);

                param = cmd.Parameters.Add("@SortField", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(sortField));

                param = cmd.Parameters.Add("@SortDirection", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(orderByDirection));

                List<IDataHelper> lstStaff = new List<IDataHelper>();
                ResultInfo objResultInfo = new ResultInfo();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Staff objStaff = new Staff();
                        objStaff = _Bind(reader);
                        lstStaff.Add(objStaff);
                    }
                    objResultInfo.Items = lstStaff;
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

        internal static Staff Bind(SqlDataReader reader)
        {
            Staff objStaff = new Staff();
            objStaff.StaffId = BasicConverter.DbToLongValue(reader["StaffID"]);
            objStaff.FirstName = BasicConverter.DbToStringValue(reader["StaffFirstName"]);
            objStaff.LastName = BasicConverter.DbToStringValue(reader["StaffLastName"]);
            objStaff.HasStaffLevelExpenditures = BasicConverter.DbToBoolValue(reader["HasStaffLevelExpenditures"]);
            objStaff.ActiveTo = BasicConverter.DbToNullableDateValue(reader["StaffActiveTo"]);
            objStaff.ActiveFrom = BasicConverter.DbToDateValue(reader["StaffActiveFrom"]);
            objStaff.IsDeleted = BasicConverter.DbToBoolValue(reader["StaffIsDeleted"]);
            objStaff.OfficeID = BasicConverter.DbToLongValue(reader["OfficeID"]);
            objStaff.WirelessNumber = BasicConverter.DbToStringValue(reader["StaffWirelessNumber"]);
            objStaff.Office = Office.Bind(reader);
            objStaff.CreatedDate = BasicConverter.DbToDateValue(reader["StaffCreatedDate"]);
            objStaff.UpdatedDate = BasicConverter.DbToNullableDateValue(reader["StaffUpdatedDate"]);
            return objStaff;
        }
    }
}
