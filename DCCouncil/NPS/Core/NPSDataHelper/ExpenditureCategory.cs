using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.PRIFACTBase.SQLHelpers;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.SortFields;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums;
using System.Xml;

namespace PRIFACT.DCCouncil.NPS.Core.NPSDataHelper
{
    public class ExpenditureCategory : IDataHelper
    {
        public long ExpenditureCategoryID
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public bool IsStaffLevel
        {
            get;
            set;
        }

        public bool IsFixed
        {
            get;
            set;
        }

        public bool IsMonthly
        {
            get;
            set;
        }

        public bool IsVendorStaff
        {
            get;
            set;
        }

        public bool IsVendorStaffAndOther
        {
            get;
            set;
        }

        public int NPSSummarySortOrder { get; set; }

        public bool IsSystemDefined
        {
            get;
            set;
        }

        public bool AppendMonth
        {
            get;
            set;
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

        private bool _IsAttributesLoaded;
        private List<ExpenditureCategoryAttribute> _Attributes;
        public List<ExpenditureCategoryAttribute> Attributes()
        {
            if (_IsAttributesLoaded)
            {
                return _Attributes;
            }
            else
            {
                _Attributes = ExpenditureCategoryAttribute.GetByCategoryId(this.ExpenditureCategoryID);
                _IsAttributesLoaded = true;
                return _Attributes;
            }
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

        public string GetAttribute(string strAttributeName)
        {
            return this.Attributes().Single(q => q.ExpenditureCategoryAttributeLookup.Name == strAttributeName).AttributeValue;
        }

        public static ResultInfo GetAll(string strSearchText, int? iPageSize, int? iPageNumber, ExpenditureCategorySortFields sortField, OrderByDirection orderByDirection)
        {
            return new SafeDBExecute<ResultInfo>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_EXPENDITURECATEGORY_GetAll";

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

                List<IDataHelper> lstExpenditureCategories = new List<IDataHelper>();
                ResultInfo objResultInfo = new ResultInfo();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ExpenditureCategory objExpenditureCategory = new ExpenditureCategory();
                        objExpenditureCategory = _Bind(reader);
                        lstExpenditureCategories.Add(objExpenditureCategory);
                    }
                    objResultInfo.Items = lstExpenditureCategories;
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

        public static ExpenditureCategory GetByCode(string strCode)
        {
            return new SafeDBExecute<ExpenditureCategory>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_EXPENDITURECATEGORY_GetByCode";

                param = cmd.Parameters.Add("@Code", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strCode);

                ExpenditureCategory objExpenseCategory = null;
                try
                {
                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        objExpenseCategory = _Bind(reader);
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        DBContext.CloseReader(reader);
                    }
                }
                return objExpenseCategory;
            }
            ).DoExecute(GetDbConnectionString());
        }

        public static ExpenditureCategory GetByID(long lExpenditureCategoryId)
        {
            return new SafeDBExecute<ExpenditureCategory>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_EXPENDITURECATEGORY_GetById";

                param = cmd.Parameters.Add("@ExpenditureCategoryId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(lExpenditureCategoryId);

                ExpenditureCategory objExpenseCategory = null;
                try
                {
                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        objExpenseCategory = _Bind(reader);
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        DBContext.CloseReader(reader);
                    }
                }
                return objExpenseCategory;
            }
            ).DoExecute(GetDbConnectionString());
        }

        public static void Create(string strName, string strCode, bool blnIsStaffLevel, bool blnIsFixed, bool blnIsActive, bool blnIsDeleted,bool blnIsVendorStaff, bool blnIsMonthly, bool blnAppendMonth,bool blnIsSystemDefined)
        {
            new SafeDBExecute<bool>(delegate(DBContext dbContext)
            {
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_EXPENDITURECATEGORY_Create";

                param = cmd.Parameters.Add("@Name", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strName);

                param = cmd.Parameters.Add("@Code", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strCode);

                param = cmd.Parameters.Add("@IsFixed", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(blnIsFixed);

                param = cmd.Parameters.Add("@IsStaffLevel", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(blnIsStaffLevel);

                param = cmd.Parameters.Add("@IsActive", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(blnIsActive);

                param = cmd.Parameters.Add("@IsVendorStaff", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(blnIsVendorStaff);

                param = cmd.Parameters.Add("@IsMonthly", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(blnIsMonthly);

                param = cmd.Parameters.Add("@AppendMonth", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(blnAppendMonth);

                param = cmd.Parameters.Add("@IsSystemDefined", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(blnIsSystemDefined);

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
                cmd.CommandText = "Proc_EXPENDITURECATEGORY_Update";

                param = cmd.Parameters.Add("@ExpenditureCategoryID", SqlDbType.NVarChar);
                param.Value = BasicConverter.DoubleToDbValue(this.ExpenditureCategoryID);

                param = cmd.Parameters.Add("@Name", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(this.Name);

                param = cmd.Parameters.Add("@Code", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(this.Code);

                param = cmd.Parameters.Add("@IsStaffLevel", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(this.IsStaffLevel);

                param = cmd.Parameters.Add("@IsFixed", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(this.IsFixed);

                param = cmd.Parameters.Add("@IsActive", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(this.IsActive);

                param = cmd.Parameters.Add("@IsVendorStaff", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(this.IsVendorStaff);

                param = cmd.Parameters.Add("@IsMonthly", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(this.IsMonthly);

                param = cmd.Parameters.Add("@AppendMonth", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(this.AppendMonth);

                param = cmd.Parameters.Add("@IsSystemDefined", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(this.IsSystemDefined);

                param = cmd.Parameters.Add("@IsDeleted", SqlDbType.Bit);
                param.Value = BasicConverter.BoolToDbValue(this.IsDeleted);


                cmd.ExecuteNonQuery();

                return true;
            }).DoExecute(GetDbConnectionString());


        }


        public void UpdateAttribute(long expenditureCategoryAttributeLookupID, String attributeValue)
        {
            new SafeDBExecute<bool>(delegate(DBContext dbContext)
            {
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_EXPENDITURECATEGORY_UpdateAttribute";

                param = cmd.Parameters.Add("@ExpenditureCategoryID", SqlDbType.NVarChar);
                param.Value = BasicConverter.LongToDbValue(this.ExpenditureCategoryID);

                param = cmd.Parameters.Add("@ExpenditureCategoryAttributeLookupID", SqlDbType.NVarChar);
                param.Value = BasicConverter.LongToDbValue(expenditureCategoryAttributeLookupID);

                param = cmd.Parameters.Add("@AttributeValue", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(attributeValue);

                cmd.ExecuteNonQuery();

                return true;
            }).DoExecute(GetDbConnectionString());
        }



        private static ExpenditureCategory _Bind(SqlDataReader reader)
        {
            ExpenditureCategory objExpenditureCategory = new ExpenditureCategory();
            objExpenditureCategory.Code = BasicConverter.DbToStringValue(reader["Code"]);
            objExpenditureCategory.CreatedDate = BasicConverter.DbToDateValue(reader["CreatedDate"]);
            objExpenditureCategory.ExpenditureCategoryID = BasicConverter.DbToLongValue(reader["ExpenditureCategoryID"]);
            objExpenditureCategory.IsActive = BasicConverter.DbToBoolValue(reader["IsActive"]);
            objExpenditureCategory.IsStaffLevel = BasicConverter.DbToBoolValue(reader["IsStaffLevel"]);
            objExpenditureCategory.IsFixed = BasicConverter.DbToBoolValue(reader["IsFixed"]);
            objExpenditureCategory.IsMonthly = BasicConverter.DbToBoolValue(reader["IsMonthly"]);
            objExpenditureCategory.IsVendorStaff = BasicConverter.DbToBoolValue(reader["IsVendorStaff"]);
            objExpenditureCategory.IsSystemDefined = BasicConverter.DbToBoolValue(reader["IsSystemDefined"]);
            objExpenditureCategory.AppendMonth = BasicConverter.DbToBoolValue(reader["AppendMonth"]);
            objExpenditureCategory.IsVendorStaffAndOther = BasicConverter.DbToBoolValue(reader["IsVendorStaffAndOther"]);
            objExpenditureCategory.Name = BasicConverter.DbToStringValue(reader["Name"]);
            objExpenditureCategory.UpdatedDate = BasicConverter.DbToNullableDateValue(reader["UpdatedDate"]);
            objExpenditureCategory.NPSSummarySortOrder = BasicConverter.DbToIntValue(reader["NPSSummarySortOrder"]);
            return objExpenditureCategory;
        }

        private static string GetDbConnectionString()
        {
            return PRIFACT.DCCouncil.NPS.Core.NPSCommon.AppSettings.DbConnectionString;
        }

        internal static ExpenditureCategory Bind(SqlDataReader reader)
        {
            ExpenditureCategory objExpenditureCategory = new ExpenditureCategory();
            objExpenditureCategory.Code = BasicConverter.DbToStringValue(reader["ExpenditureCategoryCode"]);
            objExpenditureCategory.CreatedDate = BasicConverter.DbToDateValue(reader["ExpenditureCategoryCreatedDate"]);
            objExpenditureCategory.ExpenditureCategoryID = BasicConverter.DbToLongValue(reader["ExpenditureCategoryID"]);
            objExpenditureCategory.IsActive = BasicConverter.DbToBoolValue(reader["ExpenditureCategoryIsActive"]);
            objExpenditureCategory.IsDeleted = BasicConverter.DbToBoolValue(reader["ExpenditureCategoryIsDeleted"]);
            objExpenditureCategory.IsStaffLevel = BasicConverter.DbToBoolValue(reader["ExpenditureCategoryIsStaffLevel"]);
            objExpenditureCategory.IsMonthly = BasicConverter.DbToBoolValue(reader["ExpenditureCategoryIsMonthly"]);
            objExpenditureCategory.IsFixed = BasicConverter.DbToBoolValue(reader["ExpenditureCategoryIsFixed"]);
            objExpenditureCategory.IsVendorStaff = BasicConverter.DbToBoolValue(reader["ExpenditureCategoryIsVendorStaff"]);
            objExpenditureCategory.IsSystemDefined = BasicConverter.DbToBoolValue(reader["ExpenditureCategoryIsSystemDefined"]);
            objExpenditureCategory.IsVendorStaffAndOther = BasicConverter.DbToBoolValue(reader["ExpenditureCategoryIsVendorStaffAndOther"]);
            objExpenditureCategory.Name = BasicConverter.DbToStringValue(reader["ExpenditureCategoryName"]);
            objExpenditureCategory.AppendMonth = BasicConverter.DbToBoolValue(reader["ExpenditureCategoryAppendMonth"]);
            objExpenditureCategory.UpdatedDate = BasicConverter.DbToNullableDateValue(reader["ExpenditureCategoryUpdatedDate"]);
            objExpenditureCategory.NPSSummarySortOrder = BasicConverter.DbToIntValue(reader["NPSSummarySortOrder"]);
            return objExpenditureCategory;
        }

        public static string GenerateXml(List<long> lstExpenditureCategoryIds)
        {
            string strXml = string.Empty;
            if (lstExpenditureCategoryIds != null && lstExpenditureCategoryIds.Count > 0)
            {
                XmlDocument xDoc = new XmlDocument();
                XmlNode xRootNode = xDoc.CreateNode(XmlNodeType.Element, "expenditurecategoryids", string.Empty);
                foreach (long objItem in lstExpenditureCategoryIds)
                {
                    XmlNode itemNode = _GetItemNode(xDoc, objItem);
                    xRootNode.AppendChild(itemNode);
                }
                xDoc.AppendChild(xRootNode);
                strXml = xDoc.OuterXml;
            }
            return strXml;
        }

        private static XmlNode _GetItemNode(XmlDocument xDoc, long objItem)
        {
            XmlNode xChildNode = xDoc.CreateNode(XmlNodeType.Element, "expenditurecategoryid", string.Empty);
            xChildNode.InnerText = objItem.ToString();
            return xChildNode;
        }
    }
}
