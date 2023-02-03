using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Helpers;
using PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary;
using PRIFACT.DCCouncil.NPS.Web.Portal.Helpers;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using PRIFACT.PRIFACTBase.ClosedXmlHelpers;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Members.PurchaseOrder
{
    public partial class ImportCtrlV2 : System.Web.UI.UserControl
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

                POImporter_V2Helper.ImportPOToTemp(FileName, AppSettings.AgingBalanceSheetName, AppSettings.CloseoutBalanceSheetName,AppSettings.FirstRowFirstColumnHeaderName,AppSettings.DbConnectionString,11);
                _BindPurchaseOrdersToImport(new Guid(hfGuid.Value));
                List<PurchaseOrders_Temp_V2> lstPurchaseOrderImportSummary = PurchaseOrders_Temp_V2.GetAll();
                int selectedCount = lstPurchaseOrderImportSummary.Count;
                litSelectedCount.Text = selectedCount.ToString();

                //byte[] fileBytes = _GetFileBuffer(FileName);
                //using (ExcelGenerator objExcelGenerator = new ExcelGenerator(FileName))
                //{
                //    string strXml = PurchaseOrderImportHelper.GenerateXml(objExcelGenerator, AppSettings.PurchaseOrderImportSheet3Name);
                //    PurchaseOrderImport.Insert(new Guid(hfGuid.Value), fuImportFile.FileName, strXml, fileBytes);
                //    PurchaseOrderImportSummary.Insert(new Guid(hfGuid.Value));
                //    _BindPurchaseOrdersToImport(new Guid(hfGuid.Value));
                //    List<PurchaseOrderImportSummary> lstPurchaseOrderImportSummary = PurchaseOrderImportSummary.GetAllByGuid(new Guid(hfGuid.Value));
                //    int selectedCount = lstPurchaseOrderImportSummary.Count;
                //    litSelectedCount.Text = selectedCount.ToString();
                //}
            }            
            if (e.CurrentStepIndex == 1)
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
    }
}