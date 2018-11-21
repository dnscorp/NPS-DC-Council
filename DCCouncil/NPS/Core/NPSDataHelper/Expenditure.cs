using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.PRIFACTBase.SQLHelpers;

namespace PRIFACT.DCCouncil.NPS.Core.NPSDataHelper
{
    public class Expenditure : IDataHelper
    {
        public long ExpenditureID
        {
            get;
            set;
        }

        public long ExpenditureCategoryID
        {
            get;
            set;
        }

        public ExpenditureCategory ExpenditureCategory
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

        public string OBJCode
        {
            get;
            set;
        }

        public DateTime DateOfTransaction
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
        private bool _IsStaffLevelExpendituresLoaded;
        private List<StaffLevelExpenditure> _StaffLevelExpenditures;
        public List<StaffLevelExpenditure> StaffLevelExpenditures()
        {
            if (_IsStaffLevelExpendituresLoaded)
            {
                return _StaffLevelExpenditures;
            }
            else
            {
                _StaffLevelExpenditures = StaffLevelExpenditure.GetAllByExpenditureId(this.ExpenditureID);
                _IsStaffLevelExpendituresLoaded = true;
                return _StaffLevelExpenditures;
            }
        }
        public static Expenditure GetByExpenditureID(long lExpendureId)
        {
            return new SafeDBExecute<Expenditure>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_EXPENDITURE_GetByExpenditureId";

                param = cmd.Parameters.Add("@ExpenditureId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(lExpendureId);

                Expenditure objExpenditure = null;
                try
                {
                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        objExpenditure = _Bind(reader);
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        DBContext.CloseReader(reader);
                    }
                }
                return objExpenditure;
            }
            ).DoExecute(GetDbConnectionString());
        }

        public static ResultInfo GetAll(string strSearchText,string strOfficeIdsXml,long? fiscalYearId, string strExpenditureCategoryIdsXml, DateTime? asOfdate, int? iPageSize, int? iPageNumber, NPSCommon.Enums.SortFields.ExpenditureSortFields sortField, NPSCommon.Enums.OrderByDirection orderByDirection)
        {
            return new SafeDBExecute<ResultInfo>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_EXPENDITURE_GetAll";

                param = cmd.Parameters.Add("@SearchText", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strSearchText);

                param = cmd.Parameters.Add("@OfficeIds", SqlDbType.Xml);
                param.Value = BasicConverter.StringToDbValue(strOfficeIdsXml);

                param = cmd.Parameters.Add("@AsOfDate", SqlDbType.Date);
                param.Value = BasicConverter.NullableDateToDbValue(asOfdate);

                param = cmd.Parameters.Add("@FiscalYearId", SqlDbType.BigInt);
                param.Value = BasicConverter.NullableLongToDbValue(fiscalYearId);

                param = cmd.Parameters.Add("@ExpenditureCategoryIds", SqlDbType.Xml);
                param.Value = BasicConverter.StringToDbValue(strExpenditureCategoryIdsXml);

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
                        Expenditure objExpenditure = new Expenditure();
                        objExpenditure = _Bind(reader);
                        lstExpenditures.Add(objExpenditure);
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

        public void Update(string strStaffLevelExpendituresXml)
        {
            new SafeDBExecute<bool>(delegate(DBContext dbContext)
            {
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_EXPENDITURE_Update";

                param = cmd.Parameters.Add("@ExpenditureId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(this.ExpenditureID);

                param = cmd.Parameters.Add("@ExpenditureCategoryId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(this.ExpenditureCategory.ExpenditureCategoryID);

                param = cmd.Parameters.Add("@VendorName", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(this.VendorName);

                param = cmd.Parameters.Add("@Description", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(this.Description);

                param = cmd.Parameters.Add("@OBJCode", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(this.OBJCode);

                param = cmd.Parameters.Add("@DateOfTransaction", SqlDbType.DateTime);
                param.Value = BasicConverter.DateToDbValue(this.DateOfTransaction);

                param = cmd.Parameters.Add("@Amount", SqlDbType.Float);
                param.Value = this.Amount;

                param = cmd.Parameters.Add("@OfficeId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(this.Office.OfficeID);

                param = cmd.Parameters.Add("@Comments", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(this.Comments);

                param = cmd.Parameters.Add("@FiscalYearId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(this.FiscalYear.FiscalYearID);

                param = cmd.Parameters.Add("@BudgetId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(this.Budget.BudgetID);

                param = cmd.Parameters.Add("@IsDeleted", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(this.IsDeleted);

                param = cmd.Parameters.Add("@StaffLevelExpenditures", SqlDbType.Xml);
                param.Value = BasicConverter.StringToDbValue(strStaffLevelExpendituresXml);                
                
                cmd.ExecuteNonQuery();

                return true;
            }
               ).DoExecute(GetDbConnectionString());
        }

        private static string GetDbConnectionString()
        {
            return PRIFACT.DCCouncil.NPS.Core.NPSCommon.AppSettings.DbConnectionString;
        }

        public static void Create(long expenditureCategoryId, string strVendorName, string strDescription, string strObjCode, DateTime dtDateOfTransaction, double dblAmount, long lOfficeId, string strComments, long lFiscalYearId, long lBudgetId, bool blnIsDeleted,string strStaffLevelExpenditureXml)
        {
            new SafeDBExecute<bool>(delegate(DBContext dbContext)
            {
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_EXPENDITURE_Create";

                param = cmd.Parameters.Add("@ExpenditureCategoryId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(expenditureCategoryId);

                param = cmd.Parameters.Add("@VendorName", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strVendorName);

                param = cmd.Parameters.Add("@Description", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strDescription);

                param = cmd.Parameters.Add("@OBJCode", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strObjCode);

                param = cmd.Parameters.Add("@DateOfTransaction", SqlDbType.DateTime);
                param.Value = BasicConverter.DateToDbValue(dtDateOfTransaction);

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

                param = cmd.Parameters.Add("@StaffLevelExpenditures", SqlDbType.Xml);
                param.Value = BasicConverter.StringToDbValue(strStaffLevelExpenditureXml);                

                cmd.ExecuteNonQuery();



                return true;
            }
               ).DoExecute(GetDbConnectionString());
        }
        private static Expenditure _Bind(SqlDataReader reader)
        {
            Expenditure objExpenditure = new Expenditure();
            objExpenditure.Amount = BasicConverter.DbToDoubleValue(reader["Amount"]);
            objExpenditure.BudgetID = BasicConverter.DbToLongValue(reader["BudgetID"]);
            objExpenditure.Budget = Budget.Bind(reader);
            objExpenditure.Comments = BasicConverter.DbToStringValue(reader["Comments"]);
            objExpenditure.CreatedDate = BasicConverter.DbToDateValue(reader["CreatedDate"]);
            objExpenditure.DateOfTransaction = BasicConverter.DbToDateValue(reader["DateOfTransaction"]);
            objExpenditure.Description = BasicConverter.DbToStringValue(reader["Description"]);
            objExpenditure.ExpenditureCategoryID = BasicConverter.DbToLongValue(reader["ExpenditureCategoryID"]);
            objExpenditure.ExpenditureCategory = ExpenditureCategory.Bind(reader);
            objExpenditure.ExpenditureID = BasicConverter.DbToLongValue(reader["ExpenditureID"]);
            objExpenditure.FiscalYearID = BasicConverter.DbToLongValue(reader["FiscalYearID"]);
            objExpenditure.FiscalYear = FiscalYear.Bind(reader);
            objExpenditure.IsDeleted = BasicConverter.DbToBoolValue(reader["IsDeleted"]);
            objExpenditure.OBJCode = BasicConverter.DbToStringValue(reader["OBJCode"]);
            objExpenditure.OfficeID = BasicConverter.DbToLongValue(reader["OfficeID"]);
            objExpenditure.Office = Office.Bind(reader);
            objExpenditure.UpdatedDate = BasicConverter.DbToDateValue(reader["UpdatedDate"]);
            objExpenditure.VendorName = BasicConverter.DbToStringValue(reader["VendorName"]);            
            return objExpenditure;
        }

        internal static Expenditure Bind(SqlDataReader reader)
        {
            Expenditure objExpenditure = new Expenditure();
            objExpenditure.Amount = BasicConverter.DbToDoubleValue(reader["ExpenditureAmount"]);
            objExpenditure.BudgetID = BasicConverter.DbToLongValue(reader["BudgetID"]);
            objExpenditure.Budget = Budget.Bind(reader);
            objExpenditure.Comments = BasicConverter.DbToStringValue(reader["ExpenditureComments"]);
            objExpenditure.CreatedDate = BasicConverter.DbToDateValue(reader["ExpenditureCreatedDate"]);
            objExpenditure.DateOfTransaction = BasicConverter.DbToDateValue(reader["ExpenditureDateOfTransaction"]);
            objExpenditure.Description = BasicConverter.DbToStringValue(reader["ExpenditureDescription"]);
            objExpenditure.ExpenditureCategoryID = BasicConverter.DbToLongValue(reader["ExpenditureCategoryID"]);
            objExpenditure.ExpenditureCategory = ExpenditureCategory.Bind(reader);
            objExpenditure.ExpenditureID = BasicConverter.DbToLongValue(reader["ExpenditureID"]);
            objExpenditure.FiscalYearID = BasicConverter.DbToLongValue(reader["FiscalYearID"]);
            objExpenditure.FiscalYear = FiscalYear.Bind(reader);
            objExpenditure.IsDeleted = BasicConverter.DbToBoolValue(reader["ExpenditureIsDeleted"]);
            objExpenditure.OBJCode = BasicConverter.DbToStringValue(reader["ExpenditureOBJCode"]);
            objExpenditure.OfficeID = BasicConverter.DbToLongValue(reader["OfficeID"]);
            objExpenditure.Office = Office.Bind(reader);
            objExpenditure.UpdatedDate = BasicConverter.DbToDateValue(reader["ExpenditureUpdatedDate"]);
            objExpenditure.VendorName = BasicConverter.DbToStringValue(reader["ExpenditureVendorName"]);
            return objExpenditure;
        }
    }
}
