using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums;
using PRIFACT.PRIFACTBase.SQLHelpers;

namespace PRIFACT.DCCouncil.NPS.Core.NPSDataHelper
{
    public class PurchaseOrderImportSummary
    {
        public long PurchaseOrderImportSummaryID
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

        public PurchaseOrderImportSummaryStatus ImportStatus
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



        public static void Insert(Guid importGuid)
        {
            new SafeDBExecute<bool>(delegate(DBContext dbContext)
            {

                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_PURCHASEORDERIMPORTSUMMARY_Insert";

                param = cmd.Parameters.Add("@ImportGUID", SqlDbType.UniqueIdentifier);
                param.Value = BasicConverter.GuidToDbValue(importGuid);

                cmd.ExecuteNonQuery();

                return true;
            }).DoExecute(GetDbConnectionString());
        }

        private static string GetDbConnectionString()
        {
            return AppSettings.DbConnectionString;
        }

        public static List<PurchaseOrderImportSummary> GetAllByGuid(Guid importGuid)
        {
            return new SafeDBExecute<List<PurchaseOrderImportSummary>>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_PURCHASEORDERIMPORTSUMMARY_GetAllByGuid";

                param = cmd.Parameters.Add("@ImportGUID", SqlDbType.UniqueIdentifier);
                param.Value = BasicConverter.GuidToDbValue(importGuid);

                List<PurchaseOrderImportSummary> lstPurchaseOrderImportSummaries = new List<PurchaseOrderImportSummary>();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        PurchaseOrderImportSummary objHelper = _Bind(reader); ;
                        lstPurchaseOrderImportSummaries.Add(objHelper);
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        DBContext.CloseReader(reader);
                    }
                }
                return lstPurchaseOrderImportSummaries;

            }).DoExecute(GetDbConnectionString());
        }

        private static PurchaseOrderImportSummary _Bind(SqlDataReader reader)
        {
            PurchaseOrderImportSummary objSummary = new PurchaseOrderImportSummary();
            objSummary.BudgetID = BasicConverter.DbToLongValue(reader["BudgetID"]);
            objSummary.Budget = Budget.Bind(reader);
            objSummary.CreatedDate = BasicConverter.DbToDateValue(reader["CreatedDate"]);
            objSummary.DateOfTransaction = BasicConverter.DbToDateValue(reader["DateOfTransaction"]);
            objSummary.FiscalYearID = BasicConverter.DbToLongValue(reader["FiscalYearID"]);
            objSummary.FiscalYear = FiscalYear.Bind(reader);
            objSummary.ImportID = BasicConverter.DbToLongValue(reader["ImportID"]);
            objSummary.PurchaseOrderImport = PurchaseOrderImport.Bind(reader);
            objSummary.ImportStatus = (PurchaseOrderImportSummaryStatus)BasicConverter.DbToIntValue(reader["ImportStatus"]);
            objSummary.IsDeleted = BasicConverter.DbToBoolValue(reader["IsDeleted"]);
            objSummary.OBJCode = BasicConverter.DbToStringValue(reader["OBJCode"]);
            objSummary.OfficeID = BasicConverter.DbToLongValue(reader["OfficeID"]);
            objSummary.Office = Office.Bind(reader);
            objSummary.POAdjAmtSum = BasicConverter.DbToDoubleValue(reader["POAdjAmtSum"]);
            objSummary.POAmtSum = BasicConverter.DbToDoubleValue(reader["POAmtSum"]);
            objSummary.POBalSum = BasicConverter.DbToDoubleValue(reader["POBalSum"]);
            objSummary.PONumber = BasicConverter.DbToStringValue(reader["PONumber"]);
            objSummary.PurchaseOrderImportSummaryID = BasicConverter.DbToLongValue(reader["PurchaseOrderImportSummaryID"]);
            objSummary.UpdatedDate = BasicConverter.DbToNullableDateValue(reader["UpdatedDate"]);
            objSummary.VendorName = BasicConverter.DbToStringValue(reader["VendorName"]);
            objSummary.VoucherAmtSum = BasicConverter.DbToDoubleValue(reader["VoucherAmtSum"]);
            return objSummary;
        }

        public static void Process(Guid importGuid,string strSelectedXml)
        {
            new SafeDBExecute<bool>(delegate(DBContext dbContext)
            {
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_PURCHASEORDERIMPORTSUMMARY_Process";

                param = cmd.Parameters.Add("@ImportGUID", SqlDbType.UniqueIdentifier);
                param.Value = BasicConverter.GuidToDbValue(importGuid);

                param = cmd.Parameters.Add("@SelectedSummaryIds", SqlDbType.Xml);
                param.Value = BasicConverter.StringToDbValue(strSelectedXml);

                cmd.ExecuteNonQuery();

                return true;
            }).DoExecute(GetDbConnectionString());
        }
    }
}
