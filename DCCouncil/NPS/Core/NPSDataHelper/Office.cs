using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.SortFields;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.PRIFACTBase.SQLHelpers;

namespace PRIFACT.DCCouncil.NPS.Core.NPSDataHelper
{
    [Serializable]
    public class Office : IDataHelper
    {
        public long OfficeID
        {
            get;
            set;
        }

        public string Name
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

        public string PCA
        {
            get;
            set;
        }

        public string PCATitle
        {
            get;
            set;
        }

        public string IndexCode
        {
            get;
            set;
        }

        public string IndexTitle
        {
            get;
            set;
        }
        public string CompCode
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

        private bool _IsBudgetsLoaded;
        private List<IDataHelper> _Budgets = null;

        public List<IDataHelper> Budgets(long? lFiscalYearId)
        {
            if (_IsBudgetsLoaded)
            {
                return _Budgets;
            }
            else
            {
                _IsBudgetsLoaded = true;
                _Budgets = Budget.GetAll(string.Empty, this.OfficeID, lFiscalYearId, -1, null, BudgetSortFields.IsDefault, OrderByDirection.Descending).Items;
                return _Budgets;
            }
        }

        private bool _IsAttributesLoaded = false;
        private List<OfficeAttribute> _Attributes = null;
        public List<OfficeAttribute> Attributes()
        {
            if (_IsAttributesLoaded)
            {
                return _Attributes;
            }
            else
            {
                _IsAttributesLoaded = true;
                _Attributes = OfficeAttribute.GetAllByOfficeId(this.OfficeID);
                return _Attributes;
            }
        }
        public string GetAttribute(string strAttributeName)
        {
            return this.Attributes().Single(q => q.OfficeAttributeLookup.Name == strAttributeName).AttributeValue;
        }

        private static string GetDbConnectionString()
        {
            return AppSettings.DbConnectionString;
        }

        public static void Create(string strOfficeName, DateTime dtActiveFrom, DateTime? dtActiveTo, string strPCA, string strPCATitle, string strIndexCode, string IndexTitle, bool blnIsDeleted, string strCompCode)
        {
            new SafeDBExecute<bool>(delegate(DBContext dbContext)
            {

                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_OFFICE_Create";

                param = cmd.Parameters.Add("@Name", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strOfficeName);

                param = cmd.Parameters.Add("@ActiveFrom", SqlDbType.DateTime);
                param.Value = BasicConverter.DateToDbValue(dtActiveFrom);

                param = cmd.Parameters.Add("@ActiveTo", SqlDbType.DateTime);
                param.Value = BasicConverter.NullableDateToDbValue(dtActiveTo);

                param = cmd.Parameters.Add("@PCA", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strPCA);

                param = cmd.Parameters.Add("@PCATitle", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strPCATitle);

                param = cmd.Parameters.Add("@IndexCode", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strIndexCode);

                param = cmd.Parameters.Add("@IndexTitle", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(IndexTitle);

                param = cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strCompCode);

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
                    cmd.CommandText = "Proc_OFFICE_Update";

                    param = cmd.Parameters.Add("@OfficeID", SqlDbType.NVarChar);
                    param.Value = BasicConverter.DoubleToDbValue(this.OfficeID);

                    param = cmd.Parameters.Add("@Name", SqlDbType.NVarChar);
                    param.Value = BasicConverter.StringToDbValue(this.Name);

                    param = cmd.Parameters.Add("@ActiveFrom", SqlDbType.DateTime);
                    param.Value = BasicConverter.DateToDbValue(this.ActiveFrom);

                    param = cmd.Parameters.Add("@ActiveTo", SqlDbType.DateTime);
                    param.Value = BasicConverter.NullableDateToDbValue(this.ActiveTo);

                    param = cmd.Parameters.Add("@PCA", SqlDbType.NVarChar);
                    param.Value = BasicConverter.StringToDbValue(this.PCA);

                    param = cmd.Parameters.Add("@PCATitle", SqlDbType.NVarChar);
                    param.Value = BasicConverter.StringToDbValue(this.PCATitle);

                    param = cmd.Parameters.Add("@IndexCode", SqlDbType.NVarChar);
                    param.Value = BasicConverter.StringToDbValue(this.IndexCode);

                    param = cmd.Parameters.Add("@IndexTitle", SqlDbType.NVarChar);
                    param.Value = BasicConverter.StringToDbValue(this.IndexTitle);

                    param = cmd.Parameters.Add("@IsDeleted", SqlDbType.Bit);
                    param.Value = BasicConverter.BoolToDbValue(this.IsDeleted);

                    param = cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar);
                    param.Value = BasicConverter.StringToDbValue(this.CompCode);

                    cmd.ExecuteNonQuery();

                    return true;
                }).DoExecute(GetDbConnectionString());
        }

        public void UpdateAttribute(long officeAttributeLookupID, String attributeValue)
        {
            new SafeDBExecute<bool>(delegate(DBContext dbContext)
            {
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_OFFICE_UpdateAttribute";

                param = cmd.Parameters.Add("@OfficeId", SqlDbType.NVarChar);
                param.Value = BasicConverter.LongToDbValue(this.OfficeID);

                param = cmd.Parameters.Add("@AttributeLookupId", SqlDbType.NVarChar);
                param.Value = BasicConverter.LongToDbValue(officeAttributeLookupID);

                param = cmd.Parameters.Add("@AttributeValue", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(attributeValue);

                cmd.ExecuteNonQuery();

                return true;
            }).DoExecute(GetDbConnectionString());
        }

        public static ResultInfo GetAll(string strSearchText, long? lFiscalYearId, int? iPageSize, int? iPageNumber, OfficeSortFields sortField, OrderByDirection orderByDirection)
        {
            return new SafeDBExecute<ResultInfo>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_OFFICE_GetAll";

                param = cmd.Parameters.Add("@SearchText", SqlDbType.NVarChar);
                param.Value = BasicConverter.StringToDbValue(strSearchText);

                param = cmd.Parameters.Add("@FiscalYearID", SqlDbType.BigInt);
                param.Value = BasicConverter.NullableLongToDbValue(lFiscalYearId);

                param = cmd.Parameters.Add("@PageSize", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageSize);

                param = cmd.Parameters.Add("@PageNumber", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(iPageNumber);

                param = cmd.Parameters.Add("@SortField", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(sortField));

                param = cmd.Parameters.Add("@SortDirection", SqlDbType.Int);
                param.Value = BasicConverter.NullableIntToDbValue(Convert.ToInt32(orderByDirection));

                List<IDataHelper> lstOffices = new List<IDataHelper>();
                ResultInfo objResultInfo = new ResultInfo();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Office objOffice = new Office();
                        objOffice = _Bind(reader);
                        lstOffices.Add(objOffice);
                    }
                    objResultInfo.Items = lstOffices;
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

        public static Office GetByOfficeID(long lOfficeId)
        {
            return new SafeDBExecute<Office>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_OFFICE_GetByOfficeId";

                param = cmd.Parameters.Add("@OfficeId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(lOfficeId);

                Office objOffice = null;
                try
                {
                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        objOffice = _Bind(reader);
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        DBContext.CloseReader(reader);
                    }
                }
                return objOffice;
            }
            ).DoExecute(GetDbConnectionString());
        }

        internal static Office _Bind(SqlDataReader reader)
        {
            Office objOffice = new Office();
            objOffice.OfficeID = BasicConverter.DbToIntValue(reader["OfficeID"]);
            objOffice.Name = BasicConverter.DbToStringValue(reader["Name"]);
            objOffice.ActiveFrom = BasicConverter.DbToDateValue(reader["ActiveFrom"]);
            objOffice.ActiveTo = BasicConverter.DbToNullableDateValue(reader["ActiveTo"]);
            objOffice.PCA = BasicConverter.DbToStringValue(reader["PCA"]);
            objOffice.PCATitle = BasicConverter.DbToStringValue(reader["PCATitle"]);
            objOffice.IndexCode = BasicConverter.DbToStringValue(reader["IndexCode"]);
            objOffice.IndexTitle = BasicConverter.DbToStringValue(reader["IndexTitle"]);
            //objOffice.IsDeleted = BasicConverter.DbToBoolValue(reader["IsDeleted"]);
            objOffice.CreatedDate = BasicConverter.DbToDateValue(reader["CreatedDate"]);
            objOffice.UpdatedDate = BasicConverter.DbToNullableDateValue(reader["UpdatedDate"]);
            objOffice.CompCode = BasicConverter.DbToStringValue(reader["CompCode"]);

            return objOffice;
        }

        internal static Office Bind(SqlDataReader reader)
        {
            Office objOffice = new Office();
            objOffice.OfficeID = BasicConverter.DbToLongValue(reader["OfficeID"]);
            objOffice.Name = BasicConverter.DbToStringValue(reader["OfficeName"]);
            objOffice.ActiveFrom = BasicConverter.DbToDateValue(reader["OfficeActiveFrom"]);
            objOffice.ActiveTo = BasicConverter.DbToDateValue(reader["OfficeActiveTo"]);
            objOffice.PCA = BasicConverter.DbToStringValue(reader["OfficePCA"]);
            objOffice.PCATitle = BasicConverter.DbToStringValue(reader["OfficePCATitle"]);
            objOffice.IndexCode = BasicConverter.DbToStringValue(reader["OfficeIndexCode"]);
            objOffice.IndexTitle = BasicConverter.DbToStringValue(reader["OfficeIndexTitle"]);
            objOffice.IsDeleted = BasicConverter.DbToBoolValue(reader["OfficeIsDeleted"]);
            objOffice.CreatedDate = BasicConverter.DbToDateValue(reader["OfficeCreatedDate"]);
            objOffice.UpdatedDate = BasicConverter.DbToNullableDateValue(reader["OfficeUpdatedDate"]);
            objOffice.CompCode = BasicConverter.DbToStringValue(reader["OfficeCompCode"]);
            return objOffice;
        }

        public static string GenerateXml(List<long> lstOfficeIds)
        {
            string strXml = string.Empty;
            if (lstOfficeIds != null && lstOfficeIds.Count > 0)
            {
                XmlDocument xDoc = new XmlDocument();
                XmlNode xRootNode = xDoc.CreateNode(XmlNodeType.Element, "officeids", string.Empty);
                foreach (long objItem in lstOfficeIds)
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
            XmlNode xChildNode = xDoc.CreateNode(XmlNodeType.Element, "officeid", string.Empty);
            xChildNode.InnerText = objItem.ToString();
            return xChildNode;
        }
    }
}
