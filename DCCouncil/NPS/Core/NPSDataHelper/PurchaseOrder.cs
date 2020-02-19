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
    public class PurchaseOrder : IDataHelper
    {
        public long PurchaseOrderID
        {
            get;
            set;
        }

        public string VendorName
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

        public string PONumber
        {
            get;
            set;
        }

        public double POAmtSum
        {
            get;
            set;
        }

        public double POAdjAmtSum
        {
            get;
            set;
        }

        public double flPOAdjAmtSum
        {
            get;
            set;
        }

        public double VoucherAmtSum
        {
            get;
            set;
        }

        public double POBalSum
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

        public long ExpenditureSubCategoryId
        {
            get;
            set;
        }
        public ExpenditureSubCategory ExpenditureSubCategory
        {
            get;
            set;
        }

        public bool IsTrainingExpense
        {
            get;
            set;
        }
        public bool IsDeleted
        {
            get;
            set;
        }

        public long ImportID
        {
            get;
            set;
        }

        public PurchaseOrderImport PurchaseOrderImport
        {
            get;
            set;
        }

        public PurchaseOrderDescription PurchaseOrderDescription
        {
            get;
            set;
        }

        public long AlternateOfficeID
        { get; set; }

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

        public Vendor GetVendorDetails()
        {
            return Vendor.GetByNameAndOffice(this.VendorName,this.OfficeID);
        }

        public static void Create(string strVendorName, string strOBJCode, DateTime dtDateOfTransaction, string strPONumber, float flPOAmtSum, float flPOAdjAmtSum, float flVoucherAmtSum, float flPOBalSum, double dblOfficeID, double dblBudgetID,double dblExpenditureSubCategoryId, double? dblImportID, double dblFiscalYearId, bool blnIsDeleted,bool IsTrainingExpense)
        {
            new SafeDBExecute<bool>(delegate(DBContext dbContext)
            {

                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_PURCHASEORDER_Create";

                param = cmd.Parameters.Add("@VendorName", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strVendorName);

                param = cmd.Parameters.Add("@OBJCode", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strOBJCode);

                param = cmd.Parameters.Add("@DateOfTransaction", SqlDbType.DateTime);
                param.Value = BasicConverter.DateToDbValue(dtDateOfTransaction);

                param = cmd.Parameters.Add("@PONumber", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strPONumber);

                param = cmd.Parameters.Add("@POAmtSum", SqlDbType.Float);
                param.Value = flPOAmtSum;

                param = cmd.Parameters.Add("@POAdjAmtSum", SqlDbType.Float);
                param.Value = flPOAdjAmtSum;

                param = cmd.Parameters.Add("@VoucherAmtSum", SqlDbType.Float);
                param.Value = flVoucherAmtSum;

                param = cmd.Parameters.Add("@POBalSum", SqlDbType.Float);
                param.Value = flPOBalSum;

                param = cmd.Parameters.Add("@OfficeID", SqlDbType.BigInt);
                param.Value = BasicConverter.DoubleToDbValue(dblOfficeID);

                param = cmd.Parameters.Add("@BudgetID", SqlDbType.BigInt);
                param.Value = BasicConverter.DoubleToDbValue(dblBudgetID);

                param = cmd.Parameters.Add("@ExpenditureSubCategoryId", SqlDbType.BigInt);
                param.Value = BasicConverter.DoubleToDbValue(dblExpenditureSubCategoryId);

                param = cmd.Parameters.Add("@IsTrainingExpense", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(IsTrainingExpense);

                param = cmd.Parameters.Add("@ImportID", SqlDbType.BigInt);
                param.Value = BasicConverter.NullableDoubleToDbValue(dblImportID);

                param = cmd.Parameters.Add("@FiscalYearID", SqlDbType.BigInt);
                param.Value = BasicConverter.DoubleToDbValue(dblFiscalYearId);

                param = cmd.Parameters.Add("@IsDeleted", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(blnIsDeleted);

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
                cmd.CommandText = "Proc_PURCHASEORDER_Update";

                param = cmd.Parameters.Add("@PurchaseOrderId", SqlDbType.NVarChar);
                param.Value = BasicConverter.DoubleToDbValue(this.PurchaseOrderID);

                param = cmd.Parameters.Add("@VendorName", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(this.VendorName);

                param = cmd.Parameters.Add("@OBJCode", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(this.OBJCode);

                param = cmd.Parameters.Add("@DateOfTransaction", SqlDbType.DateTime);
                param.Value = BasicConverter.DateToDbValue(this.DateOfTransaction);

                param = cmd.Parameters.Add("@PONumber", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(this.PONumber);

                param = cmd.Parameters.Add("@POAdjAmtSum", SqlDbType.Float);
                param.Value = this.POAdjAmtSum;

                param = cmd.Parameters.Add("@POAmtSum", SqlDbType.Float);
                param.Value = this.POAmtSum;

                param = cmd.Parameters.Add("@VoucherAmtSum", SqlDbType.Float);
                param.Value = this.VoucherAmtSum;

                param = cmd.Parameters.Add("@POBalSum", SqlDbType.Float);
                param.Value = this.POBalSum;

                //   param = cmd.Parameters.Add("@OfficeID", SqlDbType.BigInt);
                //  param.Value = BasicConverter.DoubleToDbValue(this.OfficeID);

                param = cmd.Parameters.Add("@BudgetID", SqlDbType.BigInt);
                param.Value = BasicConverter.DoubleToDbValue(this.BudgetID);

                param = cmd.Parameters.Add("@ExpenditureSubCategoryId", SqlDbType.BigInt);
                param.Value = BasicConverter.DoubleToDbValue(this.ExpenditureSubCategoryId);

                param = cmd.Parameters.Add("@IsTrainingExpense", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(this.IsTrainingExpense);

                param = cmd.Parameters.Add("@ImportID", SqlDbType.BigInt);
                param.Value = BasicConverter.DoubleToDbValue(this.ImportID);

                param = cmd.Parameters.Add("@FiscalYearID", SqlDbType.BigInt);
                param.Value = BasicConverter.DoubleToDbValue(this.FiscalYearID);

                param = cmd.Parameters.Add("@IsDeleted", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(this.IsDeleted);

                param = cmd.Parameters.Add("@Description", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(this.PurchaseOrderDescription.DescriptionText);

                param = cmd.Parameters.Add("@AlternateOfficeID", SqlDbType.NVarChar);
                param.Value = BasicConverter.DoubleToDbValue(this.AlternateOfficeID);

                
                cmd.ExecuteNonQuery();

                return true;
            }).DoExecute(GetDbConnectionString());


        }
        public static ResultInfo GetAll(string strSearchText, long? officeId, long? fiscalYearId, DateTime? asOfdate, int? iPageSize, int? iPageNumber, NPSCommon.Enums.SortFields.PurchaseOrderSortField sortField, NPSCommon.Enums.OrderByDirection orderByDirection, string npsReportFilters = "0")
        {
            return new SafeDBExecute<ResultInfo>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_PURCHASEORDER_GetAll";

                param = cmd.Parameters.Add("@SearchText", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strSearchText);

                param = cmd.Parameters.Add("@OfficeId", SqlDbType.BigInt);
                param.Value = BasicConverter.NullableLongToDbValue(officeId);

                param = cmd.Parameters.Add("@AsOfDate", SqlDbType.Date);
                param.Value = BasicConverter.NullableDateToDbValue(asOfdate);

                param = cmd.Parameters.Add("@FiscalYearId", SqlDbType.BigInt);
                param.Value = BasicConverter.NullableLongToDbValue(fiscalYearId);

                param = cmd.Parameters.Add("@PageSize", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageSize);

                param = cmd.Parameters.Add("@PageNumber", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageNumber);

                param = cmd.Parameters.Add("@SortField", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(sortField));

                param = cmd.Parameters.Add("@SortDirection", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(orderByDirection));

                param = cmd.Parameters.Add("@ReportFilters", SqlDbType.VarChar);
                param.Value = BasicConverter.StringToDbValue(npsReportFilters);

                List<IDataHelper> lstPurchaseOrders = new List<IDataHelper>();
                ResultInfo objResultInfo = new ResultInfo();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        PurchaseOrder objPurchaseOrder = new PurchaseOrder();
                        objPurchaseOrder = _Bind(reader);
                        lstPurchaseOrders.Add(objPurchaseOrder);
                    }
                    objResultInfo.Items = lstPurchaseOrders;
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


        public static ResultInfo GetAll_with_alternateOffices(string strSearchText, long? officeId, long? fiscalYearId, DateTime? asOfdate, int? iPageSize, int? iPageNumber, NPSCommon.Enums.SortFields.PurchaseOrderSortField sortField, NPSCommon.Enums.OrderByDirection orderByDirection, string npsReportFilters = "0")
        {
            return new SafeDBExecute<ResultInfo>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_PURCHASEORDER_GetAll_with_alternateOffices";

                param = cmd.Parameters.Add("@SearchText", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strSearchText);

                param = cmd.Parameters.Add("@OfficeId", SqlDbType.BigInt);
                param.Value = BasicConverter.NullableLongToDbValue(officeId);

                param = cmd.Parameters.Add("@AsOfDate", SqlDbType.Date);
                param.Value = BasicConverter.NullableDateToDbValue(asOfdate);

                param = cmd.Parameters.Add("@FiscalYearId", SqlDbType.BigInt);
                param.Value = BasicConverter.NullableLongToDbValue(fiscalYearId);

                param = cmd.Parameters.Add("@PageSize", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageSize);

                param = cmd.Parameters.Add("@PageNumber", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageNumber);

                param = cmd.Parameters.Add("@SortField", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(sortField));

                param = cmd.Parameters.Add("@SortDirection", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(orderByDirection));

                param = cmd.Parameters.Add("@ReportFilters", SqlDbType.VarChar);
                param.Value = BasicConverter.StringToDbValue(npsReportFilters);

                List<IDataHelper> lstPurchaseOrders = new List<IDataHelper>();
                ResultInfo objResultInfo = new ResultInfo();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        PurchaseOrder objPurchaseOrder = new PurchaseOrder();
                        objPurchaseOrder = _Bind(reader);
                        lstPurchaseOrders.Add(objPurchaseOrder);
                    }
                    objResultInfo.Items = lstPurchaseOrders;
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

        public static ResultInfo GetAllForAdhocPOMonthly(string strSearchText, string officeIds, long? fiscalYearId, DateTime? asOfdate, DateTime? Startdate, DateTime? CurrentMonth, int? iPageSize, int? iPageNumber, NPSCommon.Enums.SortFields.PurchaseOrderSortField sortField, NPSCommon.Enums.OrderByDirection orderByDirection)
        {
            return new SafeDBExecute<ResultInfo>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_PURCHASEORDER_GetAllForAdhocPOMonthly";

                param = cmd.Parameters.Add("@SearchText", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strSearchText);

                param = cmd.Parameters.Add("@OfficeIds", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(officeIds);

                param = cmd.Parameters.Add("@AsOfDate", SqlDbType.Date);
                param.Value = BasicConverter.NullableDateToDbValue(asOfdate);

                param = cmd.Parameters.Add("@StartDate", SqlDbType.Date);
                param.Value = BasicConverter.NullableDateToDbValue(Startdate);

                param = cmd.Parameters.Add("@CurrentMonth", SqlDbType.Date);
                param.Value = BasicConverter.NullableDateToDbValue(CurrentMonth);

                param = cmd.Parameters.Add("@FiscalYearId", SqlDbType.BigInt);
                param.Value = BasicConverter.NullableLongToDbValue(fiscalYearId);

                param = cmd.Parameters.Add("@PageSize", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageSize);

                param = cmd.Parameters.Add("@PageNumber", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageNumber);

                param = cmd.Parameters.Add("@SortField", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(sortField));

                param = cmd.Parameters.Add("@SortDirection", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(orderByDirection));

                List<IDataHelper> lstPurchaseOrders = new List<IDataHelper>();
                ResultInfo objResultInfo = new ResultInfo();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        PurchaseOrder objPurchaseOrder = new PurchaseOrder();
                        objPurchaseOrder = _Bind(reader);
                        lstPurchaseOrders.Add(objPurchaseOrder);
                    }
                    objResultInfo.Items = lstPurchaseOrders;
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

        public static POSummaryResultInfo GetAllForAdhocPOSummary(string strSearchText, long? officeId, long? fiscalYearId, DateTime? asOfdate, DateTime? dtStartDate, int? iPageSize, int? iPageNumber, NPSCommon.Enums.SortFields.PurchaseOrderSortField sortField, NPSCommon.Enums.OrderByDirection orderByDirection)
        {
            return new SafeDBExecute<POSummaryResultInfo>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_PURCHASEORDER_GetAllForAdhocPOSummary";

                param = cmd.Parameters.Add("@SearchText", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strSearchText);

                param = cmd.Parameters.Add("@OfficeId", SqlDbType.BigInt);
                param.Value = BasicConverter.NullableLongToDbValue(officeId);

                param = cmd.Parameters.Add("@AsOfDate", SqlDbType.Date);
                param.Value = BasicConverter.NullableDateToDbValue(asOfdate);

                param = cmd.Parameters.Add("@StartDate", SqlDbType.Date);
                param.Value = BasicConverter.NullableDateToDbValue(dtStartDate);

                param = cmd.Parameters.Add("@FiscalYearId", SqlDbType.BigInt);
                param.Value = BasicConverter.NullableLongToDbValue(fiscalYearId);

                List<IDataHelper> lstPurchaseOrders = new List<IDataHelper>();
                POSummaryResultInfo objResultInfo = new POSummaryResultInfo();
                try
                {
                    reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        objResultInfo.POExpended = BasicConverter.DbToDoubleValue(reader["POExpended"]);
                        objResultInfo.POObligated = BasicConverter.DbToDoubleValue(reader["POObligated"]);
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
        public static ResultInfo GetAllForAdhocPOYearly(string strSearchText, string officeIds, long? fiscalYearId, DateTime? asOfdate, DateTime? Startdate, int? iPageSize, int? iPageNumber, NPSCommon.Enums.SortFields.PurchaseOrderSortField sortField, NPSCommon.Enums.OrderByDirection orderByDirection)
        {
            return new SafeDBExecute<ResultInfo>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_PURCHASEORDER_GetAllForAdhocPOYearly";

                param = cmd.Parameters.Add("@SearchText", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strSearchText);

                param = cmd.Parameters.Add("@OfficeIds", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(officeIds);

                param = cmd.Parameters.Add("@AsOfDate", SqlDbType.Date);
                param.Value = BasicConverter.NullableDateToDbValue(asOfdate);

                param = cmd.Parameters.Add("@StartDate", SqlDbType.Date);
                param.Value = BasicConverter.NullableDateToDbValue(Startdate);

                param = cmd.Parameters.Add("@FiscalYearId", SqlDbType.BigInt);
                param.Value = BasicConverter.NullableLongToDbValue(fiscalYearId);

                param = cmd.Parameters.Add("@PageSize", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageSize);

                param = cmd.Parameters.Add("@PageNumber", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageNumber);

                param = cmd.Parameters.Add("@SortField", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(sortField));

                param = cmd.Parameters.Add("@SortDirection", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(orderByDirection));

                List<IDataHelper> lstPurchaseOrders = new List<IDataHelper>();
                ResultInfo objResultInfo = new ResultInfo();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        PurchaseOrder objPurchaseOrder = new PurchaseOrder();
                        objPurchaseOrder = _Bind(reader);
                        lstPurchaseOrders.Add(objPurchaseOrder);
                    }
                    objResultInfo.Items = lstPurchaseOrders;
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

        public static PurchaseOrder GetByPurchaseID(long lPurchaseID)
        {
            return new SafeDBExecute<PurchaseOrder>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_PURCHASEORDER_GetbyPurchaseId";

                param = cmd.Parameters.Add("@PurchaseId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(lPurchaseID);

                PurchaseOrder objPurchaseOrder = null;
                try
                {
                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        objPurchaseOrder = _Bind(reader);
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        DBContext.CloseReader(reader);
                    }
                }

                return objPurchaseOrder;

            }).DoExecute(GetDbConnectionString());
        }


        private static PurchaseOrder _Bind(SqlDataReader reader)
        {
            PurchaseOrder objPurchaseOrder = new PurchaseOrder();
            objPurchaseOrder.BudgetID = BasicConverter.DbToLongValue(reader["BudgetID"]);
            objPurchaseOrder.Budget = Budget.Bind(reader);
            objPurchaseOrder.CreatedDate = BasicConverter.DbToDateValue(reader["CreatedDate"]);
            objPurchaseOrder.DateOfTransaction = BasicConverter.DbToDateValue(reader["DateOfTransaction"]);
            objPurchaseOrder.FiscalYearID = BasicConverter.DbToLongValue(reader["FiscalYearID"]);
            objPurchaseOrder.FiscalYear = FiscalYear.Bind(reader);
            objPurchaseOrder.ImportID = BasicConverter.DbToLongValue(reader["ImportID"]);
            objPurchaseOrder.PurchaseOrderImport = PurchaseOrderImport.Bind(reader);
            objPurchaseOrder.IsDeleted = BasicConverter.DbToBoolValue(reader["IsDeleted"]);
            objPurchaseOrder.OBJCode = BasicConverter.DbToStringValue(reader["OBJCode"]);
            objPurchaseOrder.OfficeID = BasicConverter.DbToLongValue(reader["OfficeID"]);
            objPurchaseOrder.Office = Office.Bind(reader);
            objPurchaseOrder.PurchaseOrderDescription = PurchaseOrderDescription.Bind(reader);
            objPurchaseOrder.POAdjAmtSum = BasicConverter.DbToDoubleValue(reader["POAdjAmtSum"]);
            objPurchaseOrder.POAmtSum = BasicConverter.DbToDoubleValue(reader["POAmtSum"]);
            objPurchaseOrder.POBalSum = BasicConverter.DbToDoubleValue(reader["POBalSum"]);
            objPurchaseOrder.PONumber = BasicConverter.DbToStringValue(reader["PONumber"]);
            objPurchaseOrder.PurchaseOrderID = BasicConverter.DbToLongValue(reader["PurchaseOrderID"]);
            objPurchaseOrder.UpdatedDate = BasicConverter.DbToNullableDateValue(reader["UpdatedDate"]);
            objPurchaseOrder.VendorName = BasicConverter.DbToStringValue(reader["VendorName"]);
            objPurchaseOrder.VoucherAmtSum = BasicConverter.DbToDoubleValue(reader["VoucherAmtSum"]);
            objPurchaseOrder.AlternateOfficeID = BasicConverter.DbToLongValue(reader["AlternateOfficeID"]);
            objPurchaseOrder.ExpenditureSubCategoryId = BasicConverter.DbToLongValue(reader["ExpenditureSubCategoryId"]);
            objPurchaseOrder.IsTrainingExpense = BasicConverter.DbToBoolValue(reader["IsTrainingExpense"]);
            objPurchaseOrder.ExpenditureSubCategory = ExpenditureSubCategory.Bind(reader);
            return objPurchaseOrder;
        }

        private static string GetDbConnectionString()
        {
            return PRIFACT.DCCouncil.NPS.Core.NPSCommon.AppSettings.DbConnectionString;
        }


    }
}
