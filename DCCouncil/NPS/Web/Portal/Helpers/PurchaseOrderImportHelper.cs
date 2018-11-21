using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml;
using ClosedXML.Excel;
using PRIFACT.PRIFACTBase.ClosedXmlHelpers;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.Helpers
{
    public class PurchaseOrderImportHelper
    {
        public string Agy { get; set; }
        public string AppropFund { get; set; }
        public string AgyFund { get; set; }
        public string AgyFundTitle { get; set; }
        public string CompSourceGroup { get; set; }
        public string OrgCode1 { get; set; }
        public string PONumber { get; set; }
        public string RefSfx { get; set; }
        public string VoucherDOcVP { get; set; }
        public string VendorName { get; set; }
        public string POAdjustment { get; set; }
        public double POADJAMT { get; set; }
        public double POAMT { get; set; }
        public double VOUCHERAMT { get; set; }
        public double POBAL { get; set; }
        public string InvoiceNo { get; set; }
        public string CheckNo { get; set; }
        public DateTime? PaymentDate { get; set; }
        public double FISCAL_YEAR { get; set; }
        public DateTime? EffDate { get; set; }
        public string ProjectNo { get; set; }
        public string ProjectPh { get; set; }
        public string ProjectTitle { get; set; }
        public DateTime? LastProcDate { get; set; }
        public double AppropYear { get; set; }
        public string CompGLAcct { get; set; }
        public string TCode { get; set; }
        public string RefDoc { get; set; }
        public string AgyGLAcct { get; set; }
        public string IndexCode { get; set; }
        public string IndexCodeTitle { get; set; }
        public string PCA { get; set; }
        public string PCATitle { get; set; }
        public string CompObject { get; set; }
        public string PrimaryDocKey { get; set; }
        public string PrimaryDocSfx { get; set; }


        internal static Dictionary<string, PropertyInfo> GetPropertyInfos()
        {
            Dictionary<string, PropertyInfo> dictPropertyInfo = new Dictionary<string, PropertyInfo>();
            PurchaseOrderImportHelper obj = new PurchaseOrderImportHelper();
            PropertyInfo[] arrayPropertyInfos = obj.GetType().GetProperties();
            foreach (PropertyInfo propInfo in arrayPropertyInfos)
            {
                dictPropertyInfo.Add(propInfo.Name, propInfo);
            }
            return dictPropertyInfo;
        }

        public static List<PurchaseOrderImportHelper> GetData(IXLWorksheet workSheet)
        {
            StringBuilder sb = new StringBuilder();            
            int jValue = workSheet.RangeUsed().ColumnsUsed().Count();
            int iValue = workSheet.RangeUsed().RowsUsed().Count();
            IXLRange excelRange = workSheet.RangeUsed();
            object[,] valueArray = new object[iValue, jValue];            
            Int32 rowIndex = 0;
            foreach (var row in excelRange.RowsUsed())
            {
                object[] rowData = new object[jValue];
                Int32 i = 0;
                row.Cells().ForEach(c => valueArray[rowIndex,i++] = c.Value);
                rowIndex++;
            }
            //  get data in cell
            List<PurchaseOrderImportHelper> lstItems = new List<PurchaseOrderImportHelper>();
            Dictionary<string, PropertyInfo> dictPropertyInfo = PurchaseOrderImportHelper.GetPropertyInfos();
            Dictionary<int, string> dictColumnNames = new Dictionary<int, string>();
            for (int i = 0; i < jValue; i++)
            {
                string strColumnName = _GetColumnName(valueArray, i);
                dictColumnNames.Add(i, strColumnName);
            }
            for (int i = 1; i < iValue; i++)
            {
                PurchaseOrderImportHelper objItem = new PurchaseOrderImportHelper();
                for (int j = 0; j < jValue; j++)
                {
                    object objValue = valueArray[i, j];                    
                    if (objValue != null)
                    {
                        if (objValue.ToString().Equals(""))
                        {
                            objValue = null;
                        }
                        dictPropertyInfo[dictColumnNames[j]].SetValue(objItem, objValue);
                    }
                }
                lstItems.Add(objItem);
            }
            return lstItems;
        }

        private static string _GetColumnName(object[,] valueArray, int colIndex)
        {
            string strColumnName = valueArray[0, colIndex].ToString();            
            strColumnName = strColumnName.Replace('.', ' ');
            strColumnName = strColumnName.Replace('(', ' ');
            strColumnName = strColumnName.Replace(')', ' ');
            strColumnName = strColumnName.Replace(" ", "");
            return strColumnName;
        }

        internal static string GenerateXml(ExcelGenerator objExcelGenerator, string strSheetName)
        {
            string strXml = string.Empty;
            Dictionary<string, PropertyInfo> dictPropertyInfos = GetPropertyInfos();
            List<PurchaseOrderImportHelper> lstPurchaseOrderImportHelpers = PurchaseOrderImportHelper.GetData(objExcelGenerator.WorkBook.Worksheet(strSheetName));
            if (lstPurchaseOrderImportHelpers != null && lstPurchaseOrderImportHelpers.Count > 0)
            {
                XmlDocument xDoc = new XmlDocument();
                XmlNode xRootNode = xDoc.CreateNode(XmlNodeType.Element, "purchaseorderimport", string.Empty);
                foreach (PurchaseOrderImportHelper objItem in lstPurchaseOrderImportHelpers)
                {
                    XmlNode itemNode = _GetItemNode(xDoc, objItem, dictPropertyInfos);
                    xRootNode.AppendChild(itemNode);
                }
                xDoc.AppendChild(xRootNode);
                strXml = xDoc.OuterXml;
            }
            return strXml;
        }

        private static XmlNode _GetItemNode(XmlDocument xDoc, PurchaseOrderImportHelper objItem, Dictionary<string, PropertyInfo> dictPropertyInfos)
        {
            XmlNode xRootNode = xDoc.CreateNode(XmlNodeType.Element, "purchaseorderimportitem", string.Empty);            
            foreach (KeyValuePair<string, PropertyInfo> keyValuePair in dictPropertyInfos)
            {
                XmlNode xChildNode = xDoc.CreateNode(XmlNodeType.Element, keyValuePair.Key, string.Empty);
                dynamic dynValue = keyValuePair.Value.GetValue(objItem);
                if (dynValue != null)
                {
                    xChildNode.InnerText = dynValue.ToString();
                }
                xRootNode.AppendChild(xChildNode);
            }
            return xRootNode;
        }
    }
}