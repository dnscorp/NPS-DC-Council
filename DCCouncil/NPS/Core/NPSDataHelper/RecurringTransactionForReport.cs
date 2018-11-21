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
    public class RecurringTransactionForReport : IDataHelper
    {
            public double Amount { get; set; }
            public string Description { get; set; }

            public string RecurringCategory { get; set; }
            public string VendorName { get; set; }


            public static ResultInfo GetByFiscalYearAndAsOfDate(long fiscalYearId, DateTime asOfdate, long OfficeId)
            {
                return new SafeDBExecute<ResultInfo>(delegate(DBContext dbContext)
                {
                    SqlDataReader reader = null;
                    SqlParameter param = null;
                    SqlCommand cmd = dbContext.ContextSqlCommand;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Proc_RECURRING_GetByFiscalYearAndAsOfDate";

                    param = cmd.Parameters.Add("@FiscalYearId", SqlDbType.BigInt);
                    param.Value = BasicConverter.NullableLongToDbValue(fiscalYearId);

                    param = cmd.Parameters.Add("@AsOfDate", SqlDbType.Date);
                    param.Value = BasicConverter.NullableDateToDbValue(asOfdate);

                    param = cmd.Parameters.Add("@OfficeId", SqlDbType.Int);
                    param.Value = BasicConverter.NullableLongToDbValue(OfficeId);

                    List<IDataHelper> lstExpenditures = new List<IDataHelper>();
                    ResultInfo objResultInfo = new ResultInfo();
                    try
                    {
                        reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            RecurringTransactionForReport objRecurring = new RecurringTransactionForReport();
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

            public static ResultInfo GetByFiscalYearAndAsOfDateTransactions(long fiscalYearId, DateTime asOfdate, long OfficeId)
            {
                return new SafeDBExecute<ResultInfo>(delegate(DBContext dbContext)
                {
                    SqlDataReader reader = null;
                    SqlParameter param = null;
                    SqlCommand cmd = dbContext.ContextSqlCommand;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Proc_RECURRING_GetByFiscalYearAndAsOfDateTransactions";

                    param = cmd.Parameters.Add("@FiscalYearId", SqlDbType.BigInt);
                    param.Value = BasicConverter.NullableLongToDbValue(fiscalYearId);

                    param = cmd.Parameters.Add("@AsOfDate", SqlDbType.Date);
                    param.Value = BasicConverter.NullableDateToDbValue(asOfdate);

                    param = cmd.Parameters.Add("@OfficeId", SqlDbType.Int);
                    param.Value = BasicConverter.NullableLongToDbValue(OfficeId);

                    List<IDataHelper> lstExpenditures = new List<IDataHelper>();
                    ResultInfo objResultInfo = new ResultInfo();
                    try
                    {
                        reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            RecurringTransactionForReport objRecurring = new RecurringTransactionForReport();
                            objRecurring = _BindTransactions(reader);
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

            private static string GetDbConnectionString()
            {
                return PRIFACT.DCCouncil.NPS.Core.NPSCommon.AppSettings.DbConnectionString;
            }

            private static RecurringTransactionForReport _Bind(SqlDataReader reader)
            {
                RecurringTransactionForReport objRecurring = new RecurringTransactionForReport();
                objRecurring.Amount = BasicConverter.DbToDoubleValue(reader["Amount"]);
                objRecurring.VendorName = BasicConverter.DbToStringValue(reader["VendorName"]);
                return objRecurring;
            }

            private static RecurringTransactionForReport _BindTransactions(SqlDataReader reader)
            {
                RecurringTransactionForReport objRecurring = new RecurringTransactionForReport();
                objRecurring.RecurringCategory = BasicConverter.DbToStringValue(reader["RecurringCategory"]);
                objRecurring.VendorName = BasicConverter.DbToStringValue(reader["VendorName"]);
                objRecurring.Amount = BasicConverter.DbToDoubleValue(reader["Amount"]);
                objRecurring.Description = BasicConverter.DbToStringValue(reader["Description"]);
                return objRecurring;
            }
    }
}
