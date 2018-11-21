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
    public class StaffLevelExpenditure
    {
        public long StaffLevelExpenditureID
        {
            get;
            set;
        }

        public long ExpenditureID
        {
            get;
            set;
        }

        public Expenditure Expenditure
        {
            get;
            set;
        }

        public long StaffID
        {
            get;
            set;
        }

        public Staff Staff
        {
            get;
            set;
        }

        public double Amount
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


        public static List<StaffLevelExpenditure> GetAllByExpenditureId(long lExpenditureId)
        {
            return new SafeDBExecute<List<StaffLevelExpenditure>>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_STAFFLEVELEXPENDITURE_GetAllByExpenditureId";

                param = cmd.Parameters.Add("@ExpenditureId", SqlDbType.NVarChar);
                param.Value = BasicConverter.LongToDbValue(lExpenditureId);

                StaffLevelExpenditure objStaffLevelExpenditure = null;
                List<StaffLevelExpenditure> lstStaffLevelExpenditure = new List<StaffLevelExpenditure>();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        objStaffLevelExpenditure = _Bind(reader);
                        lstStaffLevelExpenditure.Add(objStaffLevelExpenditure);
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        DBContext.CloseReader(reader);
                    }
                }
                return lstStaffLevelExpenditure;
            }
           ).DoExecute(GetDbConnectionString());
        }

        public static List<StaffLevelExpenditure> GetAllByStaffId(long lStaffId)
        {
            return new SafeDBExecute<List<StaffLevelExpenditure>>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_STAFFLEVELEXPENDITURE_GetAllByStaffId";

                param = cmd.Parameters.Add("@StaffId", SqlDbType.NVarChar);
                param.Value = BasicConverter.LongToDbValue(lStaffId);

                StaffLevelExpenditure objStaffLevelExpenditure = null;
                List<StaffLevelExpenditure> lstStaffLevelExpenditure = new List<StaffLevelExpenditure>();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        objStaffLevelExpenditure = _Bind(reader);
                        lstStaffLevelExpenditure.Add(objStaffLevelExpenditure);
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        DBContext.CloseReader(reader);
                    }
                }
                return lstStaffLevelExpenditure;
            }
           ).DoExecute(GetDbConnectionString());
        }

        private static StaffLevelExpenditure _Bind(SqlDataReader reader)
        {
            StaffLevelExpenditure objStaffLevelExpenditure = new StaffLevelExpenditure();
            objStaffLevelExpenditure.Amount = BasicConverter.DbToDoubleValue(reader["Amount"]);
            objStaffLevelExpenditure.CreatedDate = BasicConverter.DbToDateValue(reader["CreatedDate"]);
            objStaffLevelExpenditure.ExpenditureID = BasicConverter.DbToLongValue(reader["ExpenditureID"]);
            objStaffLevelExpenditure.Expenditure = Expenditure.Bind(reader);
            objStaffLevelExpenditure.StaffID = BasicConverter.DbToLongValue(reader["StaffID"]);
            objStaffLevelExpenditure.Staff = Staff.Bind(reader);
            objStaffLevelExpenditure.StaffLevelExpenditureID = BasicConverter.DbToLongValue(reader["StaffLevelExpenditureID"]);
            objStaffLevelExpenditure.UpdatedDate = BasicConverter.DbToNullableDateValue(reader["UpdatedDate"]);
            return objStaffLevelExpenditure;
        }
        private static string GetDbConnectionString()
        {
            return AppSettings.DbConnectionString;
        }
    }
}
