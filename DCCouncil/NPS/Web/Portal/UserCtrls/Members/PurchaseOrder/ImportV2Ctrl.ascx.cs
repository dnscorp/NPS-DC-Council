using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Helpers;
using PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary;
using PRIFACT.DCCouncil.NPS.Web.Portal.Helpers;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using PRIFACT.PRIFACTBase.ClosedXmlHelpers;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Members.PurchaseOrder
{
    public partial class ImportV2Ctrl : System.Web.UI.UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            if (!string.IsNullOrEmpty(hfGuid.Value))
            {
                _BindPurchaseOrdersToImport(new Guid(hfGuid.Value));
            } 
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set the current step number as a JavaScript variable
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CurrentStep",
                    "var currentStep = " + Wizard1.ActiveStepIndex + ";", true);

                //if (Wizard1.ActiveStepIndex == 3)
                //    Wizard1.StepNextButtonText = "Import";
            }
            
        }

        protected void cvalFileType_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            if (fuImportFile.HasFile)
            {
                string fileExt;
                fileExt = fuImportFile.FileName.Substring(fuImportFile.FileName.LastIndexOf('.') + 1).ToUpper();
                if (!fileExt.ToLower().Equals("xls") && !fileExt.ToLower().Equals("xlsx"))
                {
                    args.IsValid = false;
                    cv.ErrorMessage = "Invalid file format.";
                }
            }
        }

        protected void cvalFileSize_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            if (fuImportFile.HasFile)
            {
                int maxFileSizeAllowed = AppSettings.MaxUploadFileSize * 1024;
                if (fuImportFile.PostedFile.ContentLength > maxFileSizeAllowed)
                {
                    args.IsValid = false;
                    cv.ErrorMessage = "File size cannot be greater than " + AppSettings.MaxUploadFileSize.ToString() + "KB.";
                }
            }
        }

        private void loadColumnHeaders(string sheetName)
        {
            string tableName;
            string headerColumnName;
            string dropdownName;
            if (sheetName.ToLower() == "aging")
            {
                tableName = "POAgingBalanceExcelData_V2";
                headerColumnName = "hAgingcol";
                dropdownName = "colAging";
            }
            else
            {
                tableName = "POCloseoutBalanceExcelData_V2";
                headerColumnName = "hCloseoutcol";
                dropdownName = "colCloseout";
            }

            var sqlDatatable = new DataTable();
            using (SqlConnection conn = new SqlConnection(AppSettings.DbConnectionString))
            {
                conn.Open();
                string query = "SELECT * FROM " + tableName;
                SqlDataAdapter dataadapter = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();
                dataadapter.Fill(ds);
                sqlDatatable = ds.Tables[0];
            }

            if (sqlDatatable.Columns.Count > 0)
            {
                string header = string.Empty;
                int counter = 1;
                foreach (var item in sqlDatatable.Columns)
                {
                    string spanId = headerColumnName + counter.ToString();
                    string dropdownId = dropdownName + counter.ToString() + "Dropdown";
                    header += "<th><span id='" + spanId + "'>" + item.ToString() + "</span><br /><select class='dropdown' id='" + dropdownId + "'></select></th>";
                    counter++;
                }

                if (sheetName == "Aging")
                {
                    ltrlAgingHeader.Text = header;
                    hdnAgingTotalColumns.Value = sqlDatatable.Columns.Count.ToString();// counter.ToString();
                }
                else
                {
                    ltrlCloseoutHeader.Text = header;
                    hdnCloseoutTotalColumns.Value = sqlDatatable.Columns.Count.ToString();//counter.ToString();
                }
            }
        }
        private void DeleteSqlTable(SqlConnection connection, string tableName)
        {
            string deleteTableQuery = "DROP TABLE " + tableName;

            using (SqlCommand cmd = new SqlCommand(deleteTableQuery, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
        private bool TableExists(SqlConnection connection, string tableName)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '" + tableName + "'", connection))
            {
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
        private void CreateSqlTable(SqlConnection connection, string tableName, List<string> columnNames)
        {
            string createTableQuery = "CREATE TABLE " + tableName + " (";

            foreach (var columnName in columnNames)
            {
                createTableQuery += "[" + columnName + "] NVARCHAR(MAX), "; // Assuming all columns are NVARCHAR(MAX)
            }
            createTableQuery = createTableQuery.TrimEnd(',', ' ') + ")";

            using (SqlCommand cmd = new SqlCommand(createTableQuery, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private void CreateAndInsertDataIntoSqlTable(SqlConnection connection, string tableName, ISheet sheet)
        {
            bool headerFound = false;
            List<string> columnNames = new List<string>();

            for (int i = 0; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row != null && row.Cells.Count > 0)
                {
                    if (!headerFound)
                    {
                        foreach (var colHeader in row.Cells)
                        {
                            // Check if the first cell in the current row contains "PO Number"
                            if (row.GetCell(colHeader.ColumnIndex) != null && row.GetCell(colHeader.ColumnIndex).ToString().Trim() == "PO Number")
                            {
                                headerFound = true;
                                // Parse the header row to get column names
                                foreach (var cell in row.Cells)
                                {
                                    columnNames.Add(cell.ToString());
                                }

                                // Delete the table if it exists
                                if (TableExists(connection, tableName))
                                {
                                    DeleteSqlTable(connection, tableName);
                                }
                                CreateSqlTable(connection, tableName, columnNames);
                            }

                            if (headerFound) break;
                        }

                        continue;
                    }

                    // Check if the first cell in the current row is empty or null to indicate the end of data
                    if (row.GetCell(0) == null || string.IsNullOrWhiteSpace(row.GetCell(0).ToString()))
                    {
                        break;
                    }

                    // Create the table

                    // Insert data into the table
                    string insertQuery = "INSERT INTO " + tableName + " VALUES (";
                    foreach (var cell in row.Cells)
                    {
                        if (cell.CellType.ToString().ToLower() == "numeric")
                            insertQuery += "'" + cell.NumericCellValue.ToString().Replace("'", "''") + "', "; // Assuming all columns are NVARCHAR(MAX)
                        else
                            insertQuery += "'" + cell.ToString().Replace("'", "''") + "', "; // Assuming all columns are NVARCHAR(MAX)
                    }
                    insertQuery = insertQuery.TrimEnd(',', ' ') + ")";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private void loadData(SqlConnection connection, string sheetName)
        {
            string tableName = sheetName == "Aging" ? "tempAging" : "tempCloseout";

            string query = "SELECT top 10 * FROM " + tableName;
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);


            // Fetch column names from the DataTable
            List<string> columnNames = new List<string>();
            foreach (DataColumn column in dt.Columns)
            {
                //if(column.ColumnName.Contains("("))
                //{
                //    columnNames.Add(column.ColumnName.Replace("("," ").Replace(")",""));
                //    //dt.Columns[column.ColumnName].ColumnName = column.ColumnName.Replace("(", " ").Replace(")", "");
                //}
                //else
                columnNames.Add(column.ColumnName);
            }

            // Convert DataTable to JSON and send it to the client-side
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(dt);

            // Convert the column names list to JSON
            string columnNamesJson = Newtonsoft.Json.JsonConvert.SerializeObject(columnNames);


            // Register the script to store the data in a JavaScript variable
            string script = string.Empty;
            string columnNamesScript = string.Empty;
            if (sheetName.ToLower() == "aging")
            {
                script = "<script type='text/javascript'>var agingDataArray = " + json + "; </script>";
                columnNamesScript = "<script type='text/javascript'>var agingColumnNames = " + columnNamesJson + ";</script>";
            }
            else
            {
                script = "<script type='text/javascript'>var closeoutDataArray = " + json + "; </script>";
                columnNamesScript = "<script type='text/javascript'>var closeoutColumnNames = " + columnNamesJson + ";</script>";
            }

            Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), script);
            Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), columnNamesScript);

        }

        private void CreateTempTableAndImportExcelData(HttpPostedFile uploadedFile)
        {
            // Create a workbook and load the uploaded Excel file
            HSSFWorkbook workbook;
            using (Stream stream = uploadedFile.InputStream)
            {
                workbook = new HSSFWorkbook(stream); // For .xls files
            }

            // Assuming the first sheet in the workbook is the one you want to display
            //NPOI.SS.UserModel.ISheet sheetAging = workbook.GetSheetAt(0);
            NPOI.SS.UserModel.ISheet sheetAging = workbook.GetSheet("PO Aging Balance");
            NPOI.SS.UserModel.ISheet sheetCloseout = workbook.GetSheet("PO Closeout Balance");

            SqlConnection con = new SqlConnection(AppSettings.DbConnectionString);
            con.Open();
            CreateAndInsertDataIntoSqlTable(con, "tempAging", sheetAging);
            loadData(con, "Aging");

            CreateAndInsertDataIntoSqlTable(con, "tempCloseout", sheetCloseout);
            loadData(con, "Closeout");
            //Now query top 50 from the temp table and store in UI array

            con.Close();
        }
                
        protected void Wizard1_NextButtonClick(object sender, WizardNavigationEventArgs e)
        {
            if (e.CurrentStepIndex == 0)
            {
                hfGuid.Value = Guid.NewGuid().ToString();
                Page.Validate("ValGroupFile");
                if (!Page.IsValid)
                {
                    e.Cancel = true;
                    return;
                }

            //Mapping and Preview Block            
                CreateTempTableAndImportExcelData(fuImportFile.PostedFile);
                loadColumnHeaders("Aging");
                loadColumnHeaders("Closeout");

                //POImporter_V2Helper.ImportPOToTemp(FileName, AppSettings.AgingBalanceSheetName, AppSettings.CloseoutBalanceSheetName,AppSettings.FirstRowFirstColumnHeaderName,AppSettings.DbConnectionString,11);
                //_BindPurchaseOrdersToImport(new Guid(hfGuid.Value));
                //List<PurchaseOrders_Temp_V2> lstPurchaseOrderImportSummary = PurchaseOrders_Temp_V2.GetAll();
                //int selectedCount = lstPurchaseOrderImportSummary.Count;
                //litSelectedCount.Text = selectedCount.ToString();

                ////byte[] fileBytes = _GetFileBuffer(FileName);
                ////using (ExcelGenerator objExcelGenerator = new ExcelGenerator(FileName))
                ////{
                ////    string strXml = PurchaseOrderImportHelper.GenerateXml(objExcelGenerator, AppSettings.PurchaseOrderImportSheet3Name);
                ////    PurchaseOrderImport.Insert(new Guid(hfGuid.Value), fuImportFile.FileName, strXml, fileBytes);
                ////    PurchaseOrderImportSummary.Insert(new Guid(hfGuid.Value));
                ////    _BindPurchaseOrdersToImport(new Guid(hfGuid.Value));
                ////    List<PurchaseOrderImportSummary> lstPurchaseOrderImportSummary = PurchaseOrderImportSummary.GetAllByGuid(new Guid(hfGuid.Value));
                ////    int selectedCount = lstPurchaseOrderImportSummary.Count;
                ////    litSelectedCount.Text = selectedCount.ToString();
                ////}
            }
            if (e.CurrentStepIndex == 1)
            {
                _BindPurchaseOrdersToImport(new Guid(hfGuid.Value));
                List<PurchaseOrders_Temp_V2> lstPurchaseOrderImportSummary = PurchaseOrders_Temp_V2.GetAll();
                int selectedCount = lstPurchaseOrderImportSummary.Count;
                litSelectedCount.Text = selectedCount.ToString();
            }

            if (e.CurrentStepIndex == 2)
            {
                var fiscalYear = NPSRequestContext.GetContext().FiscalYearSelected;
                PurchaseOrders_V2.Process(fiscalYear.FiscalYearID);
                _BindSummary(new Guid(hfGuid.Value));
            }
        }

        //private byte[] _GetFileBuffer(string newStrSaveFilePath)
        //{
        //    byte[] buffer;
        //    using (FileStream fs = File.OpenRead(newStrSaveFilePath))
        //    {
        //        int length = (int)fs.Length;
        //        using (BinaryReader br = new BinaryReader(fs))
        //        {
        //            buffer = br.ReadBytes(length);
        //        }
        //    }
        //    return buffer;
        //}

        private void _BindSummary(Guid guid)
        {
            var fiscalYear = NPSRequestContext.GetContext().FiscalYearSelected;
            List<PurchaseOrders_V2> lstPurchaseOrderImportSummary = PurchaseOrders_V2.GetAll(fiscalYear.FiscalYearID);
            rptrPurchaseOrderImportSummary.DataSource = lstPurchaseOrderImportSummary;
            rptrPurchaseOrderImportSummary.DataBind();
        }

        //private string _GenerateSelectedXml()
        //{
        //    XmlDocument xDoc = new XmlDocument();
        //    XmlNode xRootNode = xDoc.CreateNode(XmlNodeType.Element, "purchaseorderimportsummaries", string.Empty);
        //    List<PurchaseOrderImportSummary> lstPurchaseOrderImportSummary = PurchaseOrderImportSummary.GetAllByGuid(new Guid(hfGuid.Value));
        //    foreach (PurchaseOrderImportSummary objItem in lstPurchaseOrderImportSummary)
        //    {
        //        XmlNode itemNode = _GetItemNode(xDoc, objItem);
        //        xRootNode.AppendChild(itemNode);
        //    }
        //    xDoc.AppendChild(xRootNode);
        //    return xDoc.OuterXml;
        //}

        //private XmlNode _GetItemNode(XmlDocument xDoc, PurchaseOrderImportSummary objItem)
        //{
        //    long lPurchaseOrderImportSummeryId = objItem.PurchaseOrderImportSummaryID;
        //    XmlNode xRootNode = xDoc.CreateNode(XmlNodeType.Element, "purchaseorderimportsummaryid", string.Empty);
        //    xRootNode.InnerText = lPurchaseOrderImportSummeryId.ToString();
        //    return xRootNode;
        //}

        private void _BindPurchaseOrdersToImport(Guid guid)
        {
            List<PurchaseOrders_Temp_V2> lstPurchaseOrderImportSummary = PurchaseOrders_Temp_V2.GetAll();
            rptrPurchaseOrdersToImport.DataSource = lstPurchaseOrderImportSummary;
            rptrPurchaseOrdersToImport.DataBind();
        }
        protected void Wizard1_CancelButtonClick(object sender, EventArgs e)
        {
            Response.Redirect(NPSUrls.PurchaseOrdersV2);
        }

        protected void Wizard1_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
        {
            if (e.CurrentStepIndex==2)
            {
                loadColumnHeaders("Aging");
                loadColumnHeaders("Closeout");
                using (SqlConnection conn = new SqlConnection(AppSettings.DbConnectionString))
                {
                    conn.Open();
                    loadData(conn, "Aging");
                    loadData(conn, "Closeout");
                }
                
            }
        }

        protected void cvalSelectFile_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            if (!fuImportFile.HasFile)
            {
                args.IsValid = false;
                cv.ErrorMessage = "Please select a Purchase Order Import file.";
            }
        }

        protected void cvalIsPOImportFile_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            if (Page.IsValid && fuImportFile.HasFile)
            {
                _SaveFileToTempPath(FileName, fuImportFile.PostedFile);
                //bool blnValidFile = _ValidatePOImportFile(FileName);
                //if (!blnValidFile)
                //{
                //    args.IsValid = false;
                //    cv.ErrorMessage = "Please select a valid Purchase Order Import file.";
                //}
            }
        }

        //private bool _ValidatePOImportFile(string strFilePath)
        //{
        //    using (ExcelGenerator objExcelGenerator = new ExcelGenerator())
        //    {
        //        try
        //        {
        //            //objExcelGenerator.WorkBook.Worksheets.get_Item(AppSettings.PurchaseOrderImportSheet3Name);
        //        }
        //        catch (Exception e)
        //        {
        //            return false;
        //        }
        //        return true;
        //    }
        //}
        private string FileName
        {
            get
            {
                return AppSettings.ExcelImportTempLocationPath + hfGuid.Value + "Import.xls";
            }
        }

        private void _SaveFileToTempPath(string strFilePath, HttpPostedFile httpPostedFile)
        {
            fuImportFile.SaveAs(strFilePath);
        }

        protected void rptrPurchaseOrdersToImport_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Literal litOfficeName = e.Item.FindControl("litOfficeName") as Literal;
                Literal litVendorName = e.Item.FindControl("litVendorName") as Literal;
                Literal litPONumber = e.Item.FindControl("litPONumber") as Literal;
                Literal litPOLineNumber = e.Item.FindControl("litPOLineNumber") as Literal;
                Literal litPODescription = e.Item.FindControl("litPODescription") as Literal;
                Literal litExpendedAmount = e.Item.FindControl("litExpendedAmount") as Literal;
                Literal litAccountingDate = e.Item.FindControl("litAccountingDate") as Literal;
                Literal litPOBalance = e.Item.FindControl("litPOBalance") as Literal;
                
                HiddenField hfPurchaseOrdersV2Id = e.Item.FindControl("hfPurchaseOrdersV2Id") as HiddenField;

                PurchaseOrders_Temp_V2 objSummary = e.Item.DataItem as PurchaseOrders_Temp_V2;
                litOfficeName.Text = objSummary.OfficeName;
                litPONumber.Text = objSummary.PONumber;
                litVendorName.Text = objSummary.VendorName;
                litPOLineNumber.Text = objSummary.POLineNumber.ToString();
                litPODescription.Text = objSummary.POLineItemDescription;
                litExpendedAmount.Text = UIHelper.GetAmountInDefaultFormat(objSummary.ExpendedAmount);
                litAccountingDate.Text = objSummary.AccountingDate.ToShortDateString();
                litPOBalance.Text = UIHelper.GetAmountInDefaultFormat(objSummary.POBalance);

                hfPurchaseOrdersV2Id.Value = objSummary.PurchaseOrdersV2Id.ToString();

            }
        }

        //protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        //{
        //    CheckBox chkSelectAll = sender as CheckBox;
        //    foreach (RepeaterItem item in rptrPurchaseOrdersToImport.Items)
        //    {
        //        CheckBox chkSelect = item.FindControl("chkSelect") as CheckBox;
        //        if (chkSelectAll.Checked)
        //        {
        //            chkSelect.Checked = true;
        //        }
        //        else
        //        {
        //            chkSelect.Checked = false;
        //        }
        //    }
        //}

        protected void rptrPurchaseOrderImportSummary_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Literal litOfficeName = e.Item.FindControl("litOfficeName") as Literal;
                Literal litVendorName = e.Item.FindControl("litVendorName") as Literal;
                Literal litPONumber = e.Item.FindControl("litPONumber") as Literal;
                Literal litPOLineNumber = e.Item.FindControl("litPOLineNumber") as Literal;
                Literal litPODescription = e.Item.FindControl("litPODescription") as Literal;
                Literal litExpendedAmount = e.Item.FindControl("litExpendedAmount") as Literal;
                Literal litAccountingDate = e.Item.FindControl("litAccountingDate") as Literal;
                Literal litPOBalance = e.Item.FindControl("litPOBalance") as Literal;

                HiddenField hfPurchaseOrdersV2Id = e.Item.FindControl("hfPurchaseOrdersV2Id") as HiddenField;

                PurchaseOrders_V2 objSummary = e.Item.DataItem as PurchaseOrders_V2;
                litOfficeName.Text = objSummary.OfficeName;
                litPONumber.Text = objSummary.PONumber;
                litVendorName.Text = objSummary.VendorName;
                litPOLineNumber.Text = objSummary.POLineNumber.ToString();
                litPODescription.Text = objSummary.POLineItemDescription;
                litExpendedAmount.Text = UIHelper.GetAmountInDefaultFormat(objSummary.ExpendedAmount);
                litAccountingDate.Text = objSummary.AccountingDate.ToShortDateString();
                litPOBalance.Text = UIHelper.GetAmountInDefaultFormat(objSummary.POBalance);

                hfPurchaseOrdersV2Id.Value = objSummary.PurchaseOrdersV2Id.ToString();

                //Literal litPONumber = e.Item.FindControl("litPONumber") as Literal;
                //Literal litVendorName = e.Item.FindControl("litVendorName") as Literal;
                //Literal litObjCode = e.Item.FindControl("litObjCode") as Literal;
                //Literal litFiscalYear = e.Item.FindControl("litFiscalYear") as Literal;
                //Literal litSumOfPOAmt = e.Item.FindControl("litSumOfPOAmt") as Literal;
                //Literal litSumOfPOAdjAmt = e.Item.FindControl("litSumOfPOAdjAmt") as Literal;
                //Literal litSumOfVoucherAmt = e.Item.FindControl("litSumOfVoucherAmt") as Literal;
                //Literal litSumOfPOBal = e.Item.FindControl("litSumOfPOBal") as Literal;
                //Literal litOfficeName = e.Item.FindControl("litOfficeName") as Literal;
                //HiddenField hfPurchaseOrderImportSummeryId = e.Item.FindControl("hfPurchaseOrderImportSummeryId") as HiddenField;
                //Literal litStatus = e.Item.FindControl("litStatus") as Literal;

                //PurchaseOrderImportSummary objSummary = e.Item.DataItem as PurchaseOrderImportSummary;
                //litPONumber.Text = objSummary.PONumber;
                //litVendorName.Text = objSummary.VendorName;
                //litObjCode.Text = objSummary.OBJCode;
                //litFiscalYear.Text = objSummary.FiscalYear.Name.ToString();
                //litSumOfPOAdjAmt.Text = UIHelper.GetAmountInDefaultFormat(objSummary.POAdjAmtSum);
                //litSumOfPOAmt.Text = UIHelper.GetAmountInDefaultFormat(objSummary.POAmtSum);
                //litSumOfPOBal.Text = UIHelper.GetAmountInDefaultFormat(objSummary.POBalSum);
                //litSumOfVoucherAmt.Text = UIHelper.GetAmountInDefaultFormat(objSummary.VoucherAmtSum);
                //litOfficeName.Text = objSummary.Office.Name;
                //hfPurchaseOrderImportSummeryId.Value = objSummary.PurchaseOrderImportSummaryID.ToString();

                //switch (objSummary.ImportStatus)
                //{
                //    case Core.NPSCommon.Enums.PurchaseOrderImportSummaryStatus.NotImported:
                //        litStatus.Text = "Not Imported";
                //        break;

                //    case Core.NPSCommon.Enums.PurchaseOrderImportSummaryStatus.Imported:
                //        litStatus.Text = "Imported";
                //        break;

                //    case Core.NPSCommon.Enums.PurchaseOrderImportSummaryStatus.AlreadyExist:
                //        litStatus.Text = "Replaced";
                //        break;
                //}
            }
        }

        protected void Finish_Click(object sender, EventArgs e)
        {
            Response.Redirect(NPSUrls.PurchaseOrdersV2);
        }

        protected void Wizard1_ActiveStepChanged(object sender, EventArgs e)
        {
            // Update the HiddenField with the current step number
            CurrentStepNumber.Value = (Wizard1.ActiveStepIndex + 1).ToString();
            if (Wizard1.ActiveStepIndex==2)                
                Wizard1.StepNextButtonText = "Import";
        }
    }
}