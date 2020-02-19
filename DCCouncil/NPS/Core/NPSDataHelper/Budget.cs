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
    public class Budget :IDataHelper
    {
        public long BudgetID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public bool IsDefault
        {
            get;
            set;
        }

        public double Amount
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
        public bool IsDeduct
        {
            get;
            set;
        }

        public bool IsTrainingExpense
        {
            get;
            set;
        }

        public static Budget Bind(System.Data.SqlClient.SqlDataReader reader)
        {
            Budget objBudget = new Budget();
            objBudget.Amount = BasicConverter.DbToDoubleValue(reader["BudgetAmount"]);
            objBudget.BudgetID = BasicConverter.DbToLongValue(reader["BudgetID"]);
            objBudget.CreatedDate = BasicConverter.DbToDateValue(reader["BudgetCreatedDate"]);
            objBudget.FiscalYearID = BasicConverter.DbToLongValue(reader["FiscalYearID"]);
            objBudget.FiscalYear = FiscalYear.Bind(reader);
            objBudget.IsDefault = BasicConverter.DbToBoolValue(reader["BudgetIsDefault"]);
            objBudget.IsDeleted = BasicConverter.DbToBoolValue(reader["BudgetIsDeleted"]);
            objBudget.Name = BasicConverter.DbToStringValue(reader["BudgetName"]);
            objBudget.OfficeID = BasicConverter.DbToLongValue(reader["OfficeID"]);
            objBudget.IsDeduct = BasicConverter.DbToBoolValue(reader["BudgetIsDeduct"]);
            objBudget.IsTrainingExpense = BasicConverter.DbToBoolValue(reader["IsTrainingExpense"]);
            objBudget.Office = Office.Bind(reader);
            objBudget.UpdatedDate = BasicConverter.DbToNullableDateValue(reader["BudgetUpdatedDate"]);
            return objBudget;
        }

        private static string GetDbConnectionString()
        {
            return PRIFACT.DCCouncil.NPS.Core.NPSCommon.AppSettings.DbConnectionString;
        }

        public static ResultInfo GetAll(string strSearchText,long lOfficeId,long? lFiscalYearId, int? iPageSize, int? iPageNumber, BudgetSortFields sortField, OrderByDirection orderByDirection)
        {
            return new SafeDBExecute<ResultInfo>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_BUDGET_GetAll";

                param = cmd.Parameters.Add("@SearchText", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strSearchText); 

                param = cmd.Parameters.Add("@OfficeId", SqlDbType.Int);
                param.Value = BasicConverter.LongToDbValue(lOfficeId);

                param = cmd.Parameters.Add("@FiscalYearId", SqlDbType.Int);
                param.Value = BasicConverter.NullableLongToDbValue(lFiscalYearId);

                param = cmd.Parameters.Add("@PageSize", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageSize);

                param = cmd.Parameters.Add("@PageNumber", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageNumber);

                param = cmd.Parameters.Add("@SortField", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(sortField));

                param = cmd.Parameters.Add("@SortDirection", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(orderByDirection));

                List<IDataHelper> lstBudgets = new List<IDataHelper>();
                ResultInfo objResultInfo = new ResultInfo();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Budget objUser = new Budget();
                        objUser = _Bind(reader);
                        lstBudgets.Add(objUser);
                    }
                    objResultInfo.Items = lstBudgets;
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
       

        public static Budget GetByBudgetId(long lBudgetId)
        {
            return new SafeDBExecute<Budget>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_BUDGET_GetByBudgetId";

                param = cmd.Parameters.Add("@BudgetId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(lBudgetId);

                Budget objBudget = null;
                try
                {
                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        objBudget = _Bind(reader);
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        DBContext.CloseReader(reader);
                    }
                }
                return objBudget;
            }
           ).DoExecute(GetDbConnectionString());
        }

        private static Budget _Bind(SqlDataReader reader)
        {
            Budget objBudget = new Budget();
            objBudget.Amount = BasicConverter.DbToDoubleValue(reader["Amount"]);
            objBudget.BudgetID = BasicConverter.DbToLongValue(reader["BudgetID"]);
            objBudget.CreatedDate = BasicConverter.DbToDateValue(reader["CreatedDate"]);
            objBudget.FiscalYearID = BasicConverter.DbToLongValue(reader["FiscalYearID"]);
            objBudget.FiscalYear = FiscalYear.Bind(reader);
            objBudget.IsDefault = BasicConverter.DbToBoolValue(reader["IsDefault"]);
            objBudget.IsDeleted = BasicConverter.DbToBoolValue(reader["IsDeleted"]);
            objBudget.Name = BasicConverter.DbToStringValue(reader["Name"]);
            objBudget.OfficeID = BasicConverter.DbToLongValue(reader["OfficeID"]);
            objBudget.IsDeduct = BasicConverter.DbToBoolValue(reader["IsDeduct"]);
            objBudget.IsTrainingExpense = BasicConverter.DbToBoolValue(reader["IsTrainingExpense"]);
            objBudget.Office = Office.Bind(reader);
            objBudget.UpdatedDate = BasicConverter.DbToNullableDateValue(reader["UpdatedDate"]);
            return objBudget;
        }


        public static void Create(string strBudgetName, double dblAmount, bool blnIsDefault, long lFiscalYearId, long lOfficeId, bool blnIsDeleted, bool blnIsDeduct, bool IsTrainingExpense)
        {
            new SafeDBExecute<bool>(delegate(DBContext dbContext)
            {

                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_BUDGET_Create";

                param = cmd.Parameters.Add("@Name", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strBudgetName);

                param = cmd.Parameters.Add("@Amount", SqlDbType.Float);
                param.Value = dblAmount;

                param = cmd.Parameters.Add("@FiscalYearId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(lFiscalYearId);

                param = cmd.Parameters.Add("@OfficeId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(lOfficeId);

                param = cmd.Parameters.Add("@IsDefault", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(blnIsDefault);

                param = cmd.Parameters.Add("@IsDeleted", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(blnIsDeleted);

                param = cmd.Parameters.Add("@IsDeduct", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(blnIsDeduct);

                param = cmd.Parameters.Add("@IsTrainingExpense", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(IsTrainingExpense);

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
                cmd.CommandText = "Proc_BUDGET_Update";

                param = cmd.Parameters.Add("@BudgetId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(this.BudgetID);

                param = cmd.Parameters.Add("@Name", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(this.Name);

                param = cmd.Parameters.Add("@Amount", SqlDbType.Float);
                param.Value = this.Amount;

                param = cmd.Parameters.Add("@FiscalYearId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(this.FiscalYearID);

                param = cmd.Parameters.Add("@IsDefault", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(this.IsDefault);
                
                param = cmd.Parameters.Add("@IsDeleted", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(this.IsDeleted);

                param = cmd.Parameters.Add("@IsTrainingExpense", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(this.IsTrainingExpense);

                cmd.ExecuteNonQuery();

                return true;
            }).DoExecute(GetDbConnectionString());
        }
        
    }
}
