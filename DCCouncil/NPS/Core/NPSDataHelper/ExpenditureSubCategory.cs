using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.PRIFACTBase.SQLHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PRIFACT.DCCouncil.NPS.Core.NPSDataHelper
{
    public class ExpenditureSubCategory : IDataHelper
    {
        public long ExpenditureSubCategoryID
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
        public string ExportColumnName
        {
            get;
            set;
        }
        public bool IsActive
        {
            get;
            set;
        }

        public static ResultInfo GetAllExpenditureSubCategories()
        {
            return new SafeDBExecute<ResultInfo>(delegate (DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[Proc_ExpenditureSubCategory_GetAll]";

                List<IDataHelper> lstExpenditureSubCategory = new List<IDataHelper>();
                ResultInfo objResultInfo = new ResultInfo();
                try
                {
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ExpenditureSubCategory objExpenditureSubCategory = new ExpenditureSubCategory();
                        objExpenditureSubCategory = _Bind(reader);
                        lstExpenditureSubCategory.Add(objExpenditureSubCategory);
                    }
                    objResultInfo.Items = lstExpenditureSubCategory;
                    reader.NextResult();

                }
                finally
                {
                    if (reader != null)
                    {
                        DBContext.CloseReader(reader);
                    }
                }
                return objResultInfo;

            }

                ).DoExecute(GetDbConnectionString());

        }
        public static ExpenditureSubCategory GetByID(long lExpenditureSubCategoryId)
        {
            return new SafeDBExecute<ExpenditureSubCategory>(delegate (DBContext dbContext)
            {
                SqlDataReader reader = null;
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_EXPENDITURESUBCATEGORY_GetById";

                param = cmd.Parameters.Add("@ExpenditureSubCategoryId", SqlDbType.BigInt);
                param.Value = BasicConverter.LongToDbValue(lExpenditureSubCategoryId);

                ExpenditureSubCategory objExpenseCategory = null;
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
        private static ExpenditureSubCategory _Bind(SqlDataReader reader)
        {
            ExpenditureSubCategory objExpenditureSubCategory = new ExpenditureSubCategory();
            objExpenditureSubCategory.Code = BasicConverter.DbToStringValue(reader["Code"]);
            objExpenditureSubCategory.ExpenditureSubCategoryID = BasicConverter.DbToLongValue(reader["ExpenditureSubCategoryID"]);
            objExpenditureSubCategory.IsActive = BasicConverter.DbToBoolValue(reader["IsActive"]);
            objExpenditureSubCategory.Name = BasicConverter.DbToStringValue(reader["Name"]);
            objExpenditureSubCategory.ExportColumnName = BasicConverter.DbToStringValue(reader["ExportColumnName"]);
            return objExpenditureSubCategory;
        }

        private static string GetDbConnectionString()
        {
            return PRIFACT.DCCouncil.NPS.Core.NPSCommon.AppSettings.DbConnectionString;
        }

        internal static ExpenditureSubCategory Bind(SqlDataReader reader)
        {
            ExpenditureSubCategory objExpenditureSubCategory = new ExpenditureSubCategory();
            objExpenditureSubCategory.Code = BasicConverter.DbToStringValue(reader["ExpenditureSubCategoryCode"]);
            objExpenditureSubCategory.ExpenditureSubCategoryID = BasicConverter.DbToLongValue(reader["ExpenditureSubCategoryID"]);
            objExpenditureSubCategory.IsActive = BasicConverter.DbToBoolValue(reader["ExpenditureSubCategoryIsActive"]);
            objExpenditureSubCategory.Name = BasicConverter.DbToStringValue(reader["ExpenditureSubCategoryName"]);
            objExpenditureSubCategory.ExportColumnName = BasicConverter.DbToStringValue(reader["ExpenditureSubCategoryExportColumnName"]);
            return objExpenditureSubCategory;
        }
        public static string GenerateXml(List<long> lstExpenditureSubCategoryIds)
        {
            string strXml = string.Empty;
            if (lstExpenditureSubCategoryIds != null && lstExpenditureSubCategoryIds.Count > 0)
            {
                XmlDocument xDoc = new XmlDocument();
                XmlNode xRootNode = xDoc.CreateNode(XmlNodeType.Element, "expendituresubcategoryids", string.Empty);
                foreach (long objItem in lstExpenditureSubCategoryIds)
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
            XmlNode xChildNode = xDoc.CreateNode(XmlNodeType.Element, "expendituresubcategoryid", string.Empty);
            xChildNode.InnerText = objItem.ToString();
            return xChildNode;
        }

    }
}
