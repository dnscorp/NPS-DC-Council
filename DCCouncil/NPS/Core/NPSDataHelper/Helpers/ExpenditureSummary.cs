using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.SortFields;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.PRIFACTBase.SQLHelpers;

namespace PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Helpers
{
    public class ExpenditureSummary : IDataHelper
    {
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

        public double TotalExpenditureAmount
        {
            get;
            set;
        }

        public double TotalBudgetAmount
        {
            get;
            set;
        }

        public double BurnRate
        {
            get;
            set;
        }

        public static ResultInfo GetAllByFiscalYearId(long lFiscalYearId, int? iPageSize, int? iPageNumber, ExpenditureSummarySortField sortField, OrderByDirection orderByDirection)
        {
            return new SafeDBExecute<ResultInfo>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_EXPENDITURESUMMARY_GetAllByFiscalYearId";

                param = cmd.Parameters.Add("@FiscalYearId", SqlDbType.BigInt);
                param.Value = BasicConverter.NullableLongToDbValue(lFiscalYearId);

                param = cmd.Parameters.Add("@PageSize", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageSize);

                param = cmd.Parameters.Add("@PageNumber", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageNumber);

                param = cmd.Parameters.Add("@SortField", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(sortField));

                param = cmd.Parameters.Add("@SortDirection", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(orderByDirection));

                List<IDataHelper> lstExpenditureSummary = new List<IDataHelper>();
                ResultInfo objResultInfo = new ResultInfo();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ExpenditureSummary objExpenditureSummary = new ExpenditureSummary();
                        objExpenditureSummary = _Bind(reader);
                        lstExpenditureSummary.Add(objExpenditureSummary);
                    }
                    objResultInfo.Items = lstExpenditureSummary;
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

        public static ResultInfo SearchByFiscalYearId(string strSearchText,long lFiscalYearId, int? iPageSize, int? iPageNumber, ExpenditureSummarySortField sortField, OrderByDirection orderByDirection,string ReportFilters="0")
        {
            return new SafeDBExecute<ResultInfo>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_EXPENDITURESUMMARY_SearchByFiscalYear";

                param = cmd.Parameters.Add("@SearchText", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strSearchText);

                param = cmd.Parameters.Add("@FiscalYearId", SqlDbType.BigInt);
                param.Value = BasicConverter.NullableLongToDbValue(lFiscalYearId);

                param = cmd.Parameters.Add("@PageSize", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageSize);

                param = cmd.Parameters.Add("@PageNumber", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageNumber);

                param = cmd.Parameters.Add("@SortField", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(sortField));

                param = cmd.Parameters.Add("@SortDirection", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(orderByDirection));

                param = cmd.Parameters.Add("@ReportFilters", SqlDbType.VarChar);
                param.Value = BasicConverter.StringToDbValue(ReportFilters);

                List<IDataHelper> lstExpenditureSummary = new List<IDataHelper>();
                ResultInfo objResultInfo = new ResultInfo();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ExpenditureSummary objExpenditureSummary = new ExpenditureSummary();
                        objExpenditureSummary = _Bind(reader);
                        lstExpenditureSummary.Add(objExpenditureSummary);
                    }
                    objResultInfo.Items = lstExpenditureSummary;
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

        private static ExpenditureSummary _Bind(SqlDataReader reader)
        {
            ExpenditureSummary objExpenditureSummary = new ExpenditureSummary();            
            objExpenditureSummary.OfficeID = BasicConverter.DbToLongValue(reader["OfficeID"]);
            objExpenditureSummary.Office = Office.Bind(reader);
            objExpenditureSummary.TotalBudgetAmount = BasicConverter.DbToDoubleValue(reader["TotalBudgetAmount"]);
            objExpenditureSummary.TotalExpenditureAmount = BasicConverter.DbToDoubleValue(reader["TotalExpenditureAmount"]);
            objExpenditureSummary.BurnRate = BasicConverter.DbToDoubleValue(reader["BurnRate"]);
            return objExpenditureSummary;
        }

        private static string GetDbConnectionString()
        {
            return PRIFACT.DCCouncil.NPS.Core.NPSCommon.AppSettings.DbConnectionString;
        }
    }
}
