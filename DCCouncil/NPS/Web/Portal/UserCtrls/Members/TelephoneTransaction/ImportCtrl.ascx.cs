using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualBasic.FileIO;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.DCCouncil.NPS.Web.Portal.Helpers;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using PRIFACT.PRIFACTBase.MiscHelpers;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Members.TelephoneTransaction
{
    public partial class ImportCtrl : System.Web.UI.UserControl
    {
        public string ExpenditureCategoryCode
        {
            get
            {
                if (Request["Code"] != null)
                {
                    return Request["Code"];
                }
                else
                {
                    throw new Exception("Expenditure Category Code not specified");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                divResult.Visible = false;
            }
        }

        protected void lnkSubmitButton_Click(object sender, EventArgs e)
        {
            Page.Validate("ValGroupFile");
            if (!Page.IsValid)
                return;
            string strFileName = fuImportFile.FileName;
            string defaultsavepath = AppSettings.DefaultCSVFileSavePath;

            System.Guid newGuid = System.Guid.NewGuid();
            string fullFileName = defaultsavepath + newGuid + strFileName;
            fuImportFile.PostedFile.SaveAs(fullFileName);

            List<TelephoneTransactionSheetImportHelper> lstPhoneTransactionSheetHelper = _GeneratePhoneTransactionReportList(fullFileName);
            List<TelephoneTransactionSheetImportHelper> lstPhoneTransactionSheetImport = lstPhoneTransactionSheetHelper.Where(item => item.ImportStatusBeforeImport == TelephoneChargesStatusBeforeImport.Valid).ToList();

            string strTelephoneTransactionXml = _GenerateXml(lstPhoneTransactionSheetImport);
            hfGuid.Value = Guid.NewGuid().ToString();

            TelephoneTransactionImportItems.Insert(new Guid(hfGuid.Value), strTelephoneTransactionXml, 0);
            TelephoneTransactionImportItems.Process(new Guid(hfGuid.Value), NPSRequestContext.GetContext().FiscalYearSelected.FiscalYearID, AppSettings.CommentTextForImport);


            List<TelephoneTransactionImportItems> lstTransactionItems = TelephoneTransactionImportItems.GetAllByGuid(new Guid(hfGuid.Value), null);
            if (lstTransactionItems.Count > 0)
            {
                divUpload.Visible = false;
                divResult.Visible = true;
            }
            List<TelephoneTransactionImportItems> lstImportSucces = lstTransactionItems.Where(item => item.ImportStatus == TelephoneChargesStatusAfterImport.Success).ToList();
            if (lstImportSucces.Count > 0)
            {
                divRecordsImported.Visible = true;
                repTelephoneTransactionstoImport.DataSource = lstImportSucces;
                repTelephoneTransactionstoImport.DataBind();
                litImportSuccessCount.Text = lstImportSucces.Count.ToString();
            }

            List<TelephoneTransactionImportItems> lstImportFaliure = lstTransactionItems.Where(item => item.ImportStatus != TelephoneChargesStatusAfterImport.Success).ToList();
            if (lstImportFaliure.Count > 0)
            {
                repTelephoneTransactionstoImportInvalid.DataSource = lstImportFaliure;
                repTelephoneTransactionstoImportInvalid.DataBind();
            }

            List<TelephoneTransactionSheetImportHelper> lstPhoneTransactionSheetNonImported = lstPhoneTransactionSheetHelper.Where(item => item.ImportStatusBeforeImport != TelephoneChargesStatusBeforeImport.Valid).ToList();
            if (lstPhoneTransactionSheetNonImported.Count > 0)
            {
                repNonImportedItemList.DataSource = lstPhoneTransactionSheetNonImported;
                repNonImportedItemList.DataBind();
            }

            if (lstImportFaliure.Count > 0 || lstPhoneTransactionSheetNonImported.Count > 0)
            {
                divUpload.Visible = false;
                divResult.Visible = true;
                divRecordsNotImported.Visible = true;
                litImportFaliureCount.Text = (lstImportFaliure.Count + lstPhoneTransactionSheetNonImported.Count).ToString();
            }

        }



        private List<TelephoneTransactionSheetImportHelper> _GeneratePhoneTransactionReportList(string strFileName)
        {
            string[] source = File.ReadAllLines(strFileName);
            List<TelephoneTransactionSheetImportHelper> lstPhoneTransactionSheetHelper = new List<TelephoneTransactionSheetImportHelper>();
            if (source.Count() > 0)
            {
                foreach (string line in source)
                {
                    var parser = new TextFieldParser(new StringReader(line));
                    parser.HasFieldsEnclosedInQuotes = true;
                    parser.SetDelimiters(",");
                    string[] parts = parser.ReadFields();

                    //string[] parts = line.Split(new string[] { "\",\"" }, StringSplitOptions.None);
                    if (parts.Length == 18)
                    {
                        //parts[0] = parts[0].Remove(0, 1);
                        if (parts[0].ToString().ToLower() == "foundation account" || parts[1].ToString().ToLower() == "account number" || string.IsNullOrEmpty(parts[5].ToString()))
                            continue;
                        else
                        {
                            if (parts[8].Contains("("))
                                parts[8] = parts[8].Replace("(", "-").Replace(")", "");

                            if (parts[8].Contains("$"))
                                parts[8] = parts[8].Replace("$", "");

                            var culture = System.Globalization.CultureInfo.CurrentCulture;
                            Int64 result;

                            // DateTime dtDate = new DateTime(Convert.ToInt32(parts[4].Substring(4)), Convert.ToInt32(parts[4].Substring(2, 2)), Convert.ToInt32(parts[4].Substring(0, 2)));
                            //string strDate = parts[4].Substring(2, 2) + "/" + parts[4].Substring(0, 2) + "/" + parts[4].Substring(4);

                            //New Format YYYYMMDD - Added by Vivek on Sep 16, 2014
                            string strDate = parts[5]; //parts[5].Substring(4, 2) + "/" + parts[5].Substring(6, 2) + "/" + parts[5].Substring(0, 4);

                            var marketCycleEndDate = new DateTime();
                            if (DateTime.TryParse(strDate, out marketCycleEndDate))
                                strDate = marketCycleEndDate.ToString("MM/dd/yyyy");

                            TelephoneTransactionSheetImportHelper objTelephoneTransactionHelper = new TelephoneTransactionSheetImportHelper();
                            objTelephoneTransactionHelper.FoundationAccount = parts[0];
                            objTelephoneTransactionHelper.BillingAccount = parts[1];
                            objTelephoneTransactionHelper.WirelessNumber = parts[2];
                            objTelephoneTransactionHelper.Username = parts[7];

                            objTelephoneTransactionHelper.TotalUsage = parts[13];
                            objTelephoneTransactionHelper.NumberOfEvents = "";
                            objTelephoneTransactionHelper.MOUUsage = "";

                            string strFormat = AppSettings.TelephoneChargesImportDateFormat;
                            DateTime dateTime = new DateTime();
                            if (DateTime.TryParseExact(strDate, strFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                            {
                                objTelephoneTransactionHelper.MarketCycleEndDate = Convert.ToDateTime(dateTime.ToString());

                                Double number;
                                if (Double.TryParse(parts[8], out number))
                                {
                                    objTelephoneTransactionHelper.TotalCurrentCharges = Convert.ToDouble(parts[8]);
                                    if (Int64.TryParse(parts[2], out result))
                                    {
                                        objTelephoneTransactionHelper.ImportStatusBeforeImport = TelephoneChargesStatusBeforeImport.Valid;
                                        objTelephoneTransactionHelper.WirelessNumber = parts[2];
                                    }
                                    else
                                    {
                                        objTelephoneTransactionHelper.ImportStatusBeforeImport = TelephoneChargesStatusBeforeImport.InvalidWirelessNumber;
                                        objTelephoneTransactionHelper.WirelessNumber = parts[2];
                                        objTelephoneTransactionHelper.MarketCycleEndDateFieldValue = parts[5];
                                        objTelephoneTransactionHelper.TotalCurrentChargesFieldValue = parts[8];
                                    }

                                }
                                else
                                {
                                    objTelephoneTransactionHelper.MarketCycleEndDateFieldValue = parts[5];
                                    objTelephoneTransactionHelper.WirelessNumber = parts[2];
                                    objTelephoneTransactionHelper.ImportStatusBeforeImport = TelephoneChargesStatusBeforeImport.InvalidTotalCurrentCharges;
                                }
                                lstPhoneTransactionSheetHelper.Add(objTelephoneTransactionHelper);

                            }
                            else
                            {
                                objTelephoneTransactionHelper.MarketCycleEndDateFieldValue = parts[5];
                                objTelephoneTransactionHelper.TotalCurrentChargesFieldValue = parts[8];
                                objTelephoneTransactionHelper.WirelessNumber = parts[2];
                                objTelephoneTransactionHelper.ImportStatusBeforeImport = TelephoneChargesStatusBeforeImport.InvalidTransactionDate;
                                lstPhoneTransactionSheetHelper.Add(objTelephoneTransactionHelper);
                            }

                        }
                    }
                    else if (parts.Length == 21)
                    {
                        //parts[0] = parts[0].Remove(0, 1);
                        if (parts[0].ToString().ToLower() == "foundation account" || parts[1].ToString().ToLower() == "account number" || string.IsNullOrEmpty(parts[5].ToString()))
                            continue;
                        else
                        {
                            if (parts[11].Contains("("))
                                parts[11] = parts[11].Replace("(", "-").Replace(")", "");

                            if (parts[11].Contains("$"))
                                parts[11] = parts[11].Replace("$", "");

                            var culture = System.Globalization.CultureInfo.CurrentCulture;
                            Int64 result;

                            // DateTime dtDate = new DateTime(Convert.ToInt32(parts[4].Substring(4)), Convert.ToInt32(parts[4].Substring(2, 2)), Convert.ToInt32(parts[4].Substring(0, 2)));
                            //string strDate = parts[4].Substring(2, 2) + "/" + parts[4].Substring(0, 2) + "/" + parts[4].Substring(4);

                            //New Format YYYYMMDD - Added by Vivek on Sep 16, 2014
                            string strDate = parts[5]; //parts[5].Substring(4, 2) + "/" + parts[5].Substring(6, 2) + "/" + parts[5].Substring(0, 4);

                            var marketCycleEndDate = new DateTime();
                            if (DateTime.TryParse(strDate, out marketCycleEndDate))
                                strDate = marketCycleEndDate.ToString("MM/dd/yyyy");

                            TelephoneTransactionSheetImportHelper objTelephoneTransactionHelper = new TelephoneTransactionSheetImportHelper();
                            objTelephoneTransactionHelper.FoundationAccount = parts[0];
                            objTelephoneTransactionHelper.BillingAccount = parts[1];
                            objTelephoneTransactionHelper.WirelessNumber = parts[2];
                            objTelephoneTransactionHelper.Username = parts[7];

                            objTelephoneTransactionHelper.TotalUsage = parts[16];
                            objTelephoneTransactionHelper.NumberOfEvents = "";
                            objTelephoneTransactionHelper.MOUUsage = "";

                            string strFormat = AppSettings.TelephoneChargesImportDateFormat;
                            DateTime dateTime = new DateTime();
                            if (DateTime.TryParseExact(strDate, strFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                            {
                                objTelephoneTransactionHelper.MarketCycleEndDate = Convert.ToDateTime(dateTime.ToString());

                                Double number;
                                if (Double.TryParse(parts[11], out number))
                                {
                                    objTelephoneTransactionHelper.TotalCurrentCharges = Convert.ToDouble(parts[11]);
                                    if (Int64.TryParse(parts[2], out result))
                                    {
                                        objTelephoneTransactionHelper.ImportStatusBeforeImport = TelephoneChargesStatusBeforeImport.Valid;
                                        objTelephoneTransactionHelper.WirelessNumber = parts[2];
                                    }
                                    else
                                    {
                                        objTelephoneTransactionHelper.ImportStatusBeforeImport = TelephoneChargesStatusBeforeImport.InvalidWirelessNumber;
                                        objTelephoneTransactionHelper.WirelessNumber = parts[2];
                                        objTelephoneTransactionHelper.MarketCycleEndDateFieldValue = parts[5];
                                        objTelephoneTransactionHelper.TotalCurrentChargesFieldValue = parts[11];
                                    }

                                }
                                else
                                {
                                    objTelephoneTransactionHelper.MarketCycleEndDateFieldValue = parts[5];
                                    objTelephoneTransactionHelper.WirelessNumber = parts[2];
                                    objTelephoneTransactionHelper.ImportStatusBeforeImport = TelephoneChargesStatusBeforeImport.InvalidTotalCurrentCharges;
                                }
                                lstPhoneTransactionSheetHelper.Add(objTelephoneTransactionHelper);

                            }
                            else
                            {
                                objTelephoneTransactionHelper.MarketCycleEndDateFieldValue = parts[5];
                                objTelephoneTransactionHelper.TotalCurrentChargesFieldValue = parts[11];
                                objTelephoneTransactionHelper.WirelessNumber = parts[2];
                                objTelephoneTransactionHelper.ImportStatusBeforeImport = TelephoneChargesStatusBeforeImport.InvalidTransactionDate;
                                lstPhoneTransactionSheetHelper.Add(objTelephoneTransactionHelper);
                            }

                        }
                    }
                }
            }
            return lstPhoneTransactionSheetHelper;
        }

        private string _GenerateXml(List<TelephoneTransactionSheetImportHelper> lstPhoneTransactionSheetHelper)
        {
            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("TelephoneTransactions");
            xml.AppendChild(root);


            foreach (TelephoneTransactionSheetImportHelper item in lstPhoneTransactionSheetHelper)
            {
                XmlElement root1 = xml.CreateElement("TelephoneTransaction");

                XmlElement child = xml.CreateElement("FoundationAccount");
                child.InnerText = item.FoundationAccount;
                root1.AppendChild(child);

                child = xml.CreateElement("BillingAccount");
                child.InnerText = item.BillingAccount;
                root1.AppendChild(child);

                child = xml.CreateElement("WirelessNumber");
                child.InnerText = item.WirelessNumber;
                root1.AppendChild(child);

                child = xml.CreateElement("UserName");
                child.InnerText = item.Username;
                root1.AppendChild(child);

                child = xml.CreateElement("MarketCycleEndDate");
                child.InnerText = item.MarketCycleEndDate.ToString();
                root1.AppendChild(child);

                child = xml.CreateElement("TotalKBUsage");
                child.InnerText = item.TotalUsage.ToString();
                root1.AppendChild(child);

                child = xml.CreateElement("TotalNumberofEvents");
                child.InnerText = item.NumberOfEvents.ToString();
                root1.AppendChild(child);

                child = xml.CreateElement("TotalMOUUsage");
                child.InnerText = item.MOUUsage.ToString();
                root1.AppendChild(child);

                child = xml.CreateElement("TotalCurrentCharges");
                child.InnerText = item.TotalCurrentCharges.ToString();
                root1.AppendChild(child);
                root.AppendChild(root1);
            }

            string strXml = xml.OuterXml;
            return strXml;
        }
        protected void lnkCancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Members/Expenditures?Code=TC");
        }

        protected void lnkBackButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Members/Expenditures?Code=TC");
        }

        protected void cvalSelectFile_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            if (!fuImportFile.HasFile)
            {
                args.IsValid = false;
                cv.ErrorMessage = "Please select a Telephone transaction Import file.";
            }
        }
        protected void cvalFileType_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            if (fuImportFile.HasFile)
            {
                string fileExt;
                fileExt = fuImportFile.FileName.Substring(fuImportFile.FileName.LastIndexOf('.') + 1).ToUpper();
                if (!fileExt.ToLower().Equals("csv"))
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

        protected void repTelephoneTransactionstoImport_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal litFoundationAccount = e.Item.FindControl("litFoundationAccount") as Literal;
                Literal litBillingAccount = e.Item.FindControl("litBillingAccount") as Literal;
                Literal litImportStatus = e.Item.FindControl("litImportStatus") as Literal;
                Literal litWirelessNumber = e.Item.FindControl("litWirelessNumber") as Literal;
                Literal litTransactionDate = e.Item.FindControl("litTransactionDate") as Literal;
                Literal litTotalUsage = e.Item.FindControl("litTotalUsage") as Literal;
                Literal litTotalEvents = e.Item.FindControl("litTotalEvents") as Literal;
                Literal litTotalMouUsage = e.Item.FindControl("litTotalMouUsage") as Literal;
                Literal litTotalCurrentCharges = e.Item.FindControl("litTotalCurrentCharges") as Literal;
                Literal litUserName = e.Item.FindControl("litUserName") as Literal;

                TelephoneTransactionImportItems item = e.Item.DataItem as TelephoneTransactionImportItems;
                litFoundationAccount.Text = item.TelephoneTransactionSheetImport.FoundationAccount;
                litBillingAccount.Text = item.TelephoneTransactionSheetImport.BillingAccount;
                litWirelessNumber.Text = item.TelephoneTransactionSheetImport.WirelessNumber;
                litTransactionDate.Text = item.TelephoneTransactionSheetImport.MarketCycleEndDate.ToShortDateString();
                litUserName.Text = item.TelephoneTransactionSheetImport.Username;
                litTotalUsage.Text = item.TelephoneTransactionSheetImport.TotalUsage.ToString();
                litTotalEvents.Text = item.TelephoneTransactionSheetImport.NumberOfEvents.ToString();
                litTotalMouUsage.Text = item.TelephoneTransactionSheetImport.MOUUsage.ToString();
                litTotalCurrentCharges.Text = item.TelephoneTransactionSheetImport.TotalCurrentCharges.ToString();


                if (item.ImportStatus == TelephoneChargesStatusAfterImport.Success)
                    litImportStatus.Text = "Imported succssfully";
                else if (item.ImportStatus == TelephoneChargesStatusAfterImport.NotImported)
                    litImportStatus.Text = "Not Imported";
                else if (item.ImportStatus == TelephoneChargesStatusAfterImport.UserDoesNotExist)
                    litImportStatus.Text = "User does not exist";
                else if (item.ImportStatus == TelephoneChargesStatusAfterImport.MoreThanOneUserExists)
                    litImportStatus.Text = "More than one user with same name exist";
                else if (item.ImportStatus == TelephoneChargesStatusAfterImport.DateOutsideCurrentFiscalYear)
                    litImportStatus.Text = "Transaction date outside current fiscal year";
                else if (item.ImportStatus == TelephoneChargesStatusAfterImport.BudgetNotSpecified)
                    litImportStatus.Text = "Budget not set for the fiscal year";
                else if (item.ImportStatus == TelephoneChargesStatusAfterImport.ImportedDateNotMonthEnd)
                    litImportStatus.Text = "Invalid date, should be the last day of the month";
            }

        }

        protected void repNonImportedItemList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal litFoundationAccount = e.Item.FindControl("litFoundationAccount") as Literal;
                Literal litBillingAccount = e.Item.FindControl("litBillingAccount") as Literal;
                Literal litImportStatus = e.Item.FindControl("litImportStatus") as Literal;
                Literal litWirelessNumber = e.Item.FindControl("litWirelessNumber") as Literal;
                Literal litTransactionDate = e.Item.FindControl("litTransactionDate") as Literal;
                Literal litTotalUsage = e.Item.FindControl("litTotalUsage") as Literal;
                Literal litTotalEvents = e.Item.FindControl("litTotalEvents") as Literal;
                Literal litTotalMouUsage = e.Item.FindControl("litTotalMouUsage") as Literal;
                Literal litTotalCurrentCharges = e.Item.FindControl("litTotalCurrentCharges") as Literal;
                Literal litUserName = e.Item.FindControl("litUserName") as Literal;

                TelephoneTransactionSheetImportHelper item = e.Item.DataItem as TelephoneTransactionSheetImportHelper;
                litFoundationAccount.Text = item.FoundationAccount;
                litBillingAccount.Text = item.BillingAccount;
                litWirelessNumber.Text = item.WirelessNumber;
                litTransactionDate.Text = item.MarketCycleEndDateFieldValue;
                litUserName.Text = item.Username;
                litTotalUsage.Text = item.TotalUsage;
                litTotalEvents.Text = item.NumberOfEvents;
                litTotalMouUsage.Text = item.MOUUsage;
                litTotalCurrentCharges.Text = item.TotalCurrentChargesFieldValue;

                if (item.ImportStatusBeforeImport == TelephoneChargesStatusBeforeImport.Valid)
                    litImportStatus.Text = "Imported succssfully";
                if (item.ImportStatusBeforeImport == TelephoneChargesStatusBeforeImport.InvalidTotalCurrentCharges)
                    litImportStatus.Text = "Invalid total current charges";
                if (item.ImportStatusBeforeImport == TelephoneChargesStatusBeforeImport.InvalidTransactionDate)
                    litImportStatus.Text = "Invalid market cycle end date";
                if (item.ImportStatusBeforeImport == TelephoneChargesStatusBeforeImport.InvalidWirelessNumber)
                    litImportStatus.Text = "Invalid wireless number";


            }


        }
    }
}