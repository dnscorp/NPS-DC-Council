using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.PRIFACTBase.SQLHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIFACT.DCCouncil.NPS.Core.NPSDataHelper
{
    public class RecurringTransactions : IDataHelper
    {
        public long RecurringID
        {
            get;
            set;
        }

        public string RecurringCategory
        {
            get;
            set;
        }

        public string VendorName
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        
        public Double Amount
        {
            get;
            set;
        }

        public bool IsDeleted
        {
            get;
            set;
        }

        public string Comments
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

        public long BudgetID
        {
            get;
            set;
        }

        public Budget Budget
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


        public static RecurringTransactions GetByRecurringID(long lRecurringId)
        {
            return new SafeDBExecute<RecurringTransactions>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_RECURRING_GetByRecurringId";

                param = cmd.Parameters.Add("@RecurringId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(lRecurringId);

                RecurringTransactions objRecurring = null;
                try
                {
                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        objRecurring = _Bind(reader);
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        DBContext.CloseReader(reader);
                    }
                }
                return objRecurring;
            }
            ).DoExecute(GetDbConnectionString());
        }

        public static ResultInfo GetAll(string strSearchText, long? strOfficeId, long? fiscalYearId, string strRecurringCategory, DateTime? asOfdate, int? iPageSize, int? iPageNumber, NPSCommon.Enums.SortFields.RecurringSortFields sortField, NPSCommon.Enums.OrderByDirection orderByDirection)
        {
            return new SafeDBExecute<ResultInfo>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_RECURRING_GetAll";

                param = cmd.Parameters.Add("@SearchText", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strSearchText);

                param = cmd.Parameters.Add("@OfficeId", SqlDbType.BigInt);
                param.Value = BasicConverter.NullableLongToDbValue(strOfficeId);

                param = cmd.Parameters.Add("@AsOfDate", SqlDbType.Date);
                param.Value = BasicConverter.NullableDateToDbValue(asOfdate);

                param = cmd.Parameters.Add("@FiscalYearId", SqlDbType.BigInt);
                param.Value = BasicConverter.NullableLongToDbValue(fiscalYearId);

                param = cmd.Parameters.Add("@RecurringCategory", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strRecurringCategory);

                param = cmd.Parameters.Add("@PageSize", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageSize);

                param = cmd.Parameters.Add("@PageNumber", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageNumber);

                param = cmd.Parameters.Add("@SortField", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(sortField));

                param = cmd.Parameters.Add("@SortDirection", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(orderByDirection));

                List<IDataHelper> lstExpenditures = new List<IDataHelper>();
                ResultInfo objResultInfo = new ResultInfo();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        RecurringTransactions objRecurring = new RecurringTransactions();
                        objRecurring = _Bind(reader);
                        lstExpenditures.Add(objRecurring);
                    }
                    objResultInfo.Items = lstExpenditures;
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

       
        public static void Delete(long lRecurringID)
        {
            new SafeDBExecute<bool>(delegate(DBContext dbContext)
            {
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_RECURRING_Delete";

                param = cmd.Parameters.Add("@RecurringId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(lRecurringID);
                cmd.ExecuteNonQuery();

                return true;
            }
               ).DoExecute(GetDbConnectionString());
        }
        public static void Update(long lRecurringID ,string strRecurringCategory, string strVendorName, string strDescription, double dblAmount, long lOfficeId, string strComments, long lFiscalYearId, long lBudgetId, bool blnIsDeleted)
        {
            new SafeDBExecute<bool>(delegate(DBContext dbContext)
            {
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_RECURRING_Update";

                param = cmd.Parameters.Add("@RecurringId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(lRecurringID);

                param = cmd.Parameters.Add("@RecurringCategory", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strRecurringCategory);

                param = cmd.Parameters.Add("@VendorName", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strVendorName);

                param = cmd.Parameters.Add("@Description", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strDescription);

                param = cmd.Parameters.Add("@Amount", SqlDbType.Float);
                param.Value = dblAmount;

                param = cmd.Parameters.Add("@OfficeId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(lOfficeId);

                param = cmd.Parameters.Add("@Comments", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strComments);

                param = cmd.Parameters.Add("@FiscalYearId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(lFiscalYearId);

                param = cmd.Parameters.Add("@BudgetId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(lBudgetId);

                param = cmd.Parameters.Add("@IsDeleted", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(blnIsDeleted);

                cmd.ExecuteNonQuery();

                return true;
            }
               ).DoExecute(GetDbConnectionString());
        }

        private static string GetDbConnectionString()
        {
            return PRIFACT.DCCouncil.NPS.Core.NPSCommon.AppSettings.DbConnectionString;
        }

        public static void Create(string strRecurringCategory, string strVendorName, string strDescription, double dblAmount, long lOfficeId, string strComments, long lFiscalYearId, long lBudgetId, bool blnIsDeleted)
        {
            new SafeDBExecute<bool>(delegate(DBContext dbContext)
            {
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_RECURRING_Create";

                param = cmd.Parameters.Add("@RecurringCategory", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strRecurringCategory);

                param = cmd.Parameters.Add("@VendorName", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strVendorName);

                param = cmd.Parameters.Add("@Description", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strDescription);

                param = cmd.Parameters.Add("@Amount", SqlDbType.Float);
                param.Value = dblAmount;

                param = cmd.Parameters.Add("@OfficeId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(lOfficeId);

                param = cmd.Parameters.Add("@Comments", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strComments);

                param = cmd.Parameters.Add("@FiscalYearId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(lFiscalYearId);

                param = cmd.Parameters.Add("@BudgetId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(lBudgetId);

                param = cmd.Parameters.Add("@IsDeleted", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(blnIsDeleted);

                cmd.ExecuteNonQuery();

                return true;
            }
               ).DoExecute(GetDbConnectionString());
        }
        private static RecurringTransactions _Bind(SqlDataReader reader)
        {
            RecurringTransactions objRecurring = new RecurringTransactions();
            objRecurring.Amount = BasicConverter.DbToDoubleValue(reader["Amount"]);
            objRecurring.BudgetID = BasicConverter.DbToLongValue(reader["BudgetID"]);
            objRecurring.Budget = Budget.Bind(reader);
            objRecurring.Comments = BasicConverter.DbToStringValue(reader["Comments"]);
            objRecurring.CreatedDate = BasicConverter.DbToDateValue(reader["CreatedDate"]);
            objRecurring.Description = BasicConverter.DbToStringValue(reader["Description"]);
            objRecurring.RecurringCategory = BasicConverter.DbToStringValue(reader["RecurringCategory"]);
            objRecurring.RecurringID= BasicConverter.DbToLongValue(reader["RecurringID"]);
            objRecurring.FiscalYearID = BasicConverter.DbToLongValue(reader["FiscalYearID"]);
            objRecurring.FiscalYear = FiscalYear.Bind(reader);
            objRecurring.IsDeleted = BasicConverter.DbToBoolValue(reader["IsDeleted"]);
            objRecurring.OfficeID = BasicConverter.DbToLongValue(reader["OfficeID"]);
            objRecurring.Office = Office.Bind(reader);
            objRecurring.UpdatedDate = BasicConverter.DbToDateValue(reader["UpdatedDate"]);
            objRecurring.VendorName = BasicConverter.DbToStringValue(reader["VendorName"]);
            return objRecurring;
        }

        internal static RecurringTransactions Bind(SqlDataReader reader)
        {
            RecurringTransactions objRecurring = new RecurringTransactions();
            objRecurring.Amount = BasicConverter.DbToDoubleValue(reader["Amount"]);
            objRecurring.BudgetID = BasicConverter.DbToLongValue(reader["BudgetID"]);
            objRecurring.Budget = Budget.Bind(reader);
            objRecurring.Comments = BasicConverter.DbToStringValue(reader["Comments"]);
            objRecurring.CreatedDate = BasicConverter.DbToDateValue(reader["CreatedDate"]);
            objRecurring.Description = BasicConverter.DbToStringValue(reader["Description"]);
            objRecurring.RecurringCategory = BasicConverter.DbToStringValue(reader["RecurringCategory"]);
            objRecurring.RecurringID = BasicConverter.DbToLongValue(reader["RecurringID"]);
            objRecurring.FiscalYearID = BasicConverter.DbToLongValue(reader["FiscalYearID"]);
            objRecurring.FiscalYear = FiscalYear.Bind(reader);
            objRecurring.IsDeleted = BasicConverter.DbToBoolValue(reader["IsDeleted"]);
            objRecurring.OfficeID = BasicConverter.DbToLongValue(reader["OfficeID"]);
            objRecurring.Office = Office.Bind(reader);
            objRecurring.UpdatedDate = BasicConverter.DbToDateValue(reader["UpdatedDate"]);
            objRecurring.VendorName = BasicConverter.DbToStringValue(reader["VendorName"]);
            return objRecurring;
        }
    }

    
}
