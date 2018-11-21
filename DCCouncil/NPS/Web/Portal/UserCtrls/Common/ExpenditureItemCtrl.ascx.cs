using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.UserCtrlModes;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using System.Globalization;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.DCCouncil.NPS.Web.Portal.Pages.Members;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Common
{
    public partial class ExpenditureItemCtrl : System.Web.UI.UserControl
    {
        #region Public Events
        public event EventHandler SubmitClick;
        public event EventHandler CancelClick;
        #endregion

        #region Public Properties
        public ExpenditureItemCtrlMode Mode
        {
            get
            {
                if (string.IsNullOrEmpty(hfMode.Value))
                {
                    return ExpenditureItemCtrlMode.NotSet;
                }
                else
                {
                    return (ExpenditureItemCtrlMode)Convert.ToInt32(hfMode.Value);
                }
            }
            set
            {
                hfMode.Value = Convert.ToInt32(value).ToString();
            }
        }

        public string ItemPopupClientID
        {
            get
            {
                return divItemPopup.ClientID;
            }
        }

        public ExpenditureCategory ExpenditureCategory
        {
            get;
            set;
        }

        #endregion

        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            _SetUI();
        }

        protected override void OnInit(EventArgs e)
        {
            //_LoadBudgets(null, null);
            _LoadOffices();
            base.OnInit(e);
        }

        #endregion

        #region Public Setter and Getter Methods
        public void InitializeFields(Expenditure objExpenditure)
        {
            if (objExpenditure != null)
            {
                //Set the fields
                hfExpenditureId.Value = objExpenditure.ExpenditureID.ToString();
                hfFiscalYearId.Value = objExpenditure.FiscalYear.FiscalYearID.ToString();
                litFiscalYearSelected.Text = objExpenditure.FiscalYear.Name;
                litDateOfTransactionFieldText.Text = objExpenditure.ExpenditureCategory.GetAttribute("DateOfTransactionFieldText");
                hfCategoryCode.Value = ExpenditureCategory.Code;
                if (ExpenditureCategory.IsVendorStaff)
                {
                    txtVendorName.Visible = false;
                    _LoadStaffForVendor(objExpenditure, objExpenditure.Office.OfficeID);                    
                }
                else
                {
                    txtVendorName.Visible = true;
                    ddlStaffs.Visible = false;
                }
                litVendorNameFieldText.Text = ExpenditureCategory.GetAttribute("VendorNameFieldText");
                if (ExpenditureCategory.IsMonthly)
                {
                    _LoadMonths();
                    ListItem selectedListItem = ddlMonths.Items.FindByText(objExpenditure.DateOfTransaction.ToString("MMM yyyy"));
                    if (selectedListItem != null)
                        selectedListItem.Selected = true;
                    trDateOfTransaction.Visible = true;
                    txtDateOfTransaction.Visible = false;
                    ddlMonths.Visible = true;
                }
                else
                {
                    txtDateOfTransaction.Visible = true;
                    ddlMonths.Visible = false;
                }
                if (!objExpenditure.ExpenditureCategory.IsFixed)
                {
                    ddlOffices.SelectedValue = objExpenditure.OfficeID.ToString();
                    litOffice.Visible = false;

                }
                else
                {
                    ddlMonths.Visible = false;
                    litMonth.Text = objExpenditure.DateOfTransaction.ToString("MMM yyyy");
                    ddlOffices.Visible = false;
                    litOffice.Visible = true;
                    litOffice.Text = objExpenditure.Office.Name;

                }
                txtDateOfTransaction.Text = UIHelper.GetDateTimeInDefaultFormat(objExpenditure.DateOfTransaction);
                txtVendorName.Text = objExpenditure.VendorName;
                txtDescription.Text = objExpenditure.Description;
                txtOBJCode.Text = objExpenditure.OBJCode;
                ddlOffices.SelectedValue = objExpenditure.Office.OfficeID.ToString();
                litOffice.Text = objExpenditure.Office.Name;
                //_LoadBudgets(objExpenditure.Office.OfficeID, objExpenditure.FiscalYear.FiscalYearID);
                //ddlBudgets.SelectedValue = objExpenditure.Budget.BudgetID.ToString();
                txtAmount.Text = objExpenditure.Amount.ToString();
                txtComments.Text = objExpenditure.Comments;
                chkIsVendor.Checked = false;
                if (ExpenditureCategory.IsStaffLevel)
                {
                    _BindStaffs(objExpenditure.Office.OfficeID, objExpenditure.StaffLevelExpenditures());
                }
                else
                {
                    phStaffLevel.Visible = false;
                }
            }
            else
            {
                //Initialise the fields
                hfExpenditureId.Value = string.Empty;
                hfFiscalYearId.Value = NPSRequestContext.GetContext().FiscalYearSelected.FiscalYearID.ToString();
                litFiscalYearSelected.Text = NPSRequestContext.GetContext().FiscalYearSelected.Name;
                litDateOfTransactionFieldText.Text = ExpenditureCategory.GetAttribute("DateOfTransactionFieldText");
                hfCategoryCode.Value = ExpenditureCategory.Code;
                if (ExpenditureCategory.IsVendorStaff)
                {
                    txtVendorName.Visible = false;
                    ddlStaffs.Visible = true;                    
                    trVendorName.Visible = false;                    
                }
                else
                {
                    trVendorName.Visible = true;                    
                    txtVendorName.Visible = true;
                    ddlStaffs.Visible = false;
                }
                litVendorNameFieldText.Text = ExpenditureCategory.GetAttribute("VendorNameFieldText");
                if (ExpenditureCategory.IsMonthly)
                {
                    trDateOfTransaction.Visible = false;
                    ddlMonths.Visible = true;
                    txtDateOfTransaction.Visible = false;
                }
                else
                {
                    ddlMonths.Visible = false;
                    txtDateOfTransaction.Visible = true;
                }
                txtDateOfTransaction.Text = string.Empty;
                txtVendorName.Text = string.Empty;
                txtDescription.Text = string.Empty;
                txtOBJCode.Text = string.Empty;
                ddlOffices.SelectedValue = "-1";
                //ddlBudgets.SelectedValue = "-1";
                txtAmount.Text = string.Empty;
                txtComments.Text = string.Empty;
                phStaffLevel.Visible = false;

            }

            //Hiding the placeholders based on the Expenditure Category
            if (ExpenditureCategory.IsFixed)
            {
                phIsFixed.Visible = false;
            }
            else
            {
                phIsFixed.Visible = true;
            }

            if (ExpenditureCategory.IsStaffLevel)
            {
                txtAmount.Enabled = false;
            }
            else
            {
                txtAmount.Enabled = true;
            }
        }

        private void _LoadStaffForVendor(Expenditure objExpenditure, long lOfficeId)
        {
            long lnOfficeId = 0;
            if (objExpenditure != null)
            {
                lnOfficeId = objExpenditure.Office.OfficeID;
            }
            else
            {
                lnOfficeId = lOfficeId;
            }
            List<Staff> lstStaff = _GetStaffsForOffice(lOfficeId);
            ddlStaffs.Items.Clear();
            foreach (Staff staff in lstStaff)
            {
                ListItem listItemStaff = new ListItem();
                listItemStaff.Text = staff.FirstName + " " + staff.LastName;
                listItemStaff.Value = staff.StaffId.ToString();
                ddlStaffs.Items.Add(listItemStaff);
            }
            ListItem liSelectStaff = new ListItem("Select Staff", "-1");
            ddlStaffs.Items.Insert(0, liSelectStaff);
            if (ExpenditureCategory.IsVendorStaffAndOther)
            {
                ddlStaffs.Items.Add(new ListItem("Other", "-2"));
            }
            if (objExpenditure != null)
            {
                List<StaffLevelExpenditure> lstStaffLevelExpenditures = objExpenditure.StaffLevelExpenditures();
                long lStaffId = -1;
                if (lstStaffLevelExpenditures != null && lstStaffLevelExpenditures.Count > 0)
                {
                    lStaffId = lstStaffLevelExpenditures[0].StaffID;
                    ddlStaffs.SelectedValue = lStaffId.ToString();
                }
                else
                {
                    if (!ExpenditureCategory.IsVendorStaffAndOther)
                    {
                        ddlStaffs.Items.Add(new ListItem("Other", "-2"));
                    }
                    ddlStaffs.SelectedValue = "-2";
                    txtVendorName.Visible = true;
                    txtVendorName.Text = objExpenditure.VendorName;
                    Vendor objVendor = Vendor.GetByNameAndOffice(objExpenditure.VendorName, objExpenditure.OfficeID);
                    if (objVendor != null)
                    {   
                        chkIsVendor.Checked = true;
                    }
                    trIsVendor.Visible = true;
                }
            }
            else
            {
                ddlStaffs.SelectedIndex = 0;
            }
            ddlStaffs.Visible = true;
        }

        private List<Staff> _GetStaffsForOffice(long lofficeId)
        {
            Office objOffice = Office.GetByOfficeID(lofficeId);
            return Staff.GetAllByOfficeId(lofficeId, false, -1, 0, Core.NPSCommon.Enums.SortFields.StaffSortFields.FirstName, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items.ConvertAll(q => (Staff)q);
        }

        private void _LoadMonths()
        {
            User objUser = NPSRequestContext.GetContext().LoggedInUser;
            FiscalYear objFiscalYear = objUser.LastFiscalYearSelected;
            DateTime start = objFiscalYear.StartDate;
            DateTime end = objFiscalYear.EndDate;
            ddlMonths.Items.Clear();
            while (start < end)
            {
                ListItem listItemMonth = new ListItem();
                listItemMonth.Text = start.ToString("MMM yyyy");
                listItemMonth.Value = new DateTime(start.Year, start.Month, DateTime.DaysInMonth(start.Year, start.Month)).ToShortDateString();

                ddlMonths.Items.Add(listItemMonth);
                start = start.AddMonths(1);
            }
            ListItem item = new ListItem("Select Month", "-1");
            ddlMonths.Items.Insert(0, item);
            ddlMonths.SelectedIndex = -1;
        }

        private void _BindStaffs(long lOfficeId, List<StaffLevelExpenditure> lstStaffLevelExpenditures)
        {
            if (ExpenditureCategory.IsStaffLevel)
            {
                if (!(lstStaffLevelExpenditures != null && lstStaffLevelExpenditures.Count > 0))
                {
                    ResultInfo objResultInfo = Staff.GetAllByOfficeId(lOfficeId, true, -1, null, Core.NPSCommon.Enums.SortFields.StaffSortFields.FirstName, Core.NPSCommon.Enums.OrderByDirection.Ascending);
                    List<Staff> lstStaff = objResultInfo.Items.ConvertAll(q => (Staff)q);
                    lstStaffLevelExpenditures = new List<StaffLevelExpenditure>();
                    foreach (Staff objStaff in lstStaff)
                    {
                        StaffLevelExpenditure objStaffLevelExpenditure = new StaffLevelExpenditure();
                        objStaffLevelExpenditure.Staff = objStaff;
                        lstStaffLevelExpenditures.Add(objStaffLevelExpenditure);
                    }
                }

                rptrStaffLevelAmount.DataSource = lstStaffLevelExpenditures;
                rptrStaffLevelAmount.DataBind();
                phStaffLevel.Visible = true;
            }
        }
        #endregion

        #region Private Methods

        private void _LoadOffices()
        {
            long lFiscalYearId = NPSRequestContext.GetContext().FiscalYearSelected.FiscalYearID;
            List<IDataHelper> lstOffices = Office.GetAll(string.Empty, lFiscalYearId, -1, null, Core.NPSCommon.Enums.SortFields.OfficeSortFields.Name, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items;
            ddlOffices.Items.Clear();
            ddlOffices.DataSource = lstOffices;
            ddlOffices.DataTextField = "Name";
            ddlOffices.DataValueField = "OfficeID";
            ddlOffices.DataBind();

            ListItem item = new ListItem("Select Office", "-1");
            ddlOffices.Items.Insert(0, item);
            ddlOffices.SelectedIndex = 0;
            //trBudget.Visible = false;
            phStaffLevel.Visible = false;
        }

        private void _SetUI()
        {
            if (Mode == ExpenditureItemCtrlMode.Create)
            {
                _ShowPopup();
                lnkSubmit.InnerText = "Create";
                litOffice.Visible = false;
                ddlOffices.Visible = true;
                litHeading.Text = "Create";
            }
            if (Mode == ExpenditureItemCtrlMode.Edit)
            {
                _ShowPopup();
                lnkSubmit.InnerText = "Update";
                litOffice.Visible = false;
                ddlOffices.Visible = false;
                litHeading.Text = "Update";
                if (ExpenditureCategory.IsFixed)
                {
                    litOffice.Visible = true;
                }
                else
                {
                    ddlOffices.Visible = true;
                }
                if (ExpenditureCategory.IsMonthly && !String.IsNullOrEmpty(hfOfficeName.Value))
                {
                    litOffice.Text = hfOfficeName.Value;
                }
            }
            if (Mode == ExpenditureItemCtrlMode.NotSet)
            {
                _HidePopup();
            }
            CalendarExtender1.Format = AppSettings.DateFormat;
        }

        private void _HidePopup()
        {
            divItemPopup.Attributes.Add("style", "display: none");
        }

        private void _ShowPopup()
        {
            divItemPopup.Attributes.Add("style", "display:block");
        }

        //private void _LoadBudgets(long? lOfficeId, long? lFiscalYearId)
        //{
        //    ddlBudgets.Items.Clear();
        //    if (lOfficeId.HasValue)
        //    {
        //        List<IDataHelper> lstBudgets = Budget.GetAll(string.Empty,lOfficeId.Value, lFiscalYearId.Value, -1, null, Core.NPSCommon.Enums.SortFields.BudgetSortFields.Name, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items;
        //        if (lstBudgets.Count > 0)
        //        {
        //            ddlBudgets.DataSource = lstBudgets;
        //            ddlBudgets.DataTextField = "Name";
        //            ddlBudgets.DataValueField = "BudgetID";
        //            ddlBudgets.DataBind();
        //        }

        //        ListItem item = new ListItem("Select Budget", "-1");
        //        ddlBudgets.Items.Insert(0, item);
        //        ddlBudgets.SelectedIndex = 0;
        //    }
        //    else
        //    {
        //        ListItem item = new ListItem("Select Budget", "-1");
        //        ddlBudgets.Items.Insert(0, item);
        //        ddlBudgets.SelectedIndex = 0;
        //    }
        //    trBudget.Visible = true;
        //}
        #endregion

        #region Submit and Cancel Button Clicks
        protected void lnkSubmit_ServerClick(object sender, EventArgs e)
        {
            //Validation
            Page.Validate("ValGroup1");
            if (ExpenditureCategory.IsStaffLevel)
            {
                Page.Validate("ValGroupStaffLevelAmount");
            }
            if (!Page.IsValid)
            {
                _ShowPopup();
                return;
            }

            if (Mode == ExpenditureItemCtrlMode.Create)
            {
                //TODO Change to date from control
                long officeId = Convert.ToInt64(ddlOffices.SelectedValue);
                Office objOffice = Office.GetByOfficeID(officeId);
                string strDescription = string.Empty;
                string strOBJCode = string.Empty;
                string strVendorName = string.Empty;
                string strStaffLevelExpenditureXml = string.Empty;
                if (ExpenditureCategory.IsFixed)
                {
                    strDescription = ExpenditureCategory.GetAttribute("FixedDescription");
                    strOBJCode = ExpenditureCategory.GetAttribute("FixedOBJCode");
                    strVendorName = ExpenditureCategory.GetAttribute("FixedVendorName"); ;
                }
                else
                {
                    strDescription = txtDescription.Text.Trim();
                    strOBJCode = txtOBJCode.Text.Trim();
                    if (ExpenditureCategory.IsVendorStaff)
                    {
                        if (ExpenditureCategory.IsVendorStaffAndOther && ddlStaffs.SelectedValue == "-2")
                        {
                            if (!String.IsNullOrEmpty(txtVendorName.Text.Trim()))
                            {
                                strVendorName = txtVendorName.Text.Trim();
                                if (chkIsVendor.Checked)
                                {
                                    //DV
                                    Vendor.Create(strVendorName, strDescription, officeId, Convert.ToInt64(hfFiscalYearId.Value), false, false);
                                }                                
                            }
                        }
                        else
                        {
                            //Petty Cash
                            strVendorName = ddlStaffs.SelectedItem.Text;
                            strStaffLevelExpenditureXml = _GetStaffLevelExpenditures(Convert.ToInt64(ddlStaffs.SelectedValue), Convert.ToDouble(txtAmount.Text.Trim()));
                        }
                    }
                    else
                    {
                        //P-Card
                        strVendorName = txtVendorName.Text.Trim();
                        Vendor.Create(strVendorName, strDescription, officeId, Convert.ToInt64(hfFiscalYearId.Value), false, false);
                    }
                }
                if (ExpenditureCategory.IsStaffLevel)
                {
                    strStaffLevelExpenditureXml = _GetStaffLevelExpenditures(null, null);
                }
                DateTime dateOfTransaction = new DateTime();
                if (ExpenditureCategory.IsMonthly)
                {
                    dateOfTransaction = Convert.ToDateTime(ddlMonths.SelectedValue);
                }
                else
                {
                    dateOfTransaction = Convert.ToDateTime(txtDateOfTransaction.Text);
                }
                List<IDataHelper> lstBudgets = objOffice.Budgets(NPSRequestContext.GetContext().FiscalYearSelected.FiscalYearID);
                if (lstBudgets != null && lstBudgets.Count > 0)
                {
                    List<Budget> lstBudgetsConverted = lstBudgets.ConvertAll(q => (Budget)q);
                    Budget objBudget = lstBudgetsConverted.Single(q => q.IsDefault);
                    Expenditure.Create(ExpenditureCategory.ExpenditureCategoryID, strVendorName, strDescription, strOBJCode, dateOfTransaction, Convert.ToDouble(txtAmount.Text.Trim().Replace(",", "")), Convert.ToInt64(ddlOffices.SelectedValue), txtComments.Text.Trim(), Convert.ToInt64(hfFiscalYearId.Value), objBudget.BudgetID, false, strStaffLevelExpenditureXml);
                    UIHelper.SetSuccessMessage("Expenditure created successfully");
                }
                else
                {
                    throw new Exception("Budget not set for the Office");
                }
            }
            if (Mode == ExpenditureItemCtrlMode.Edit)
            {
                long lExpenditureId = Convert.ToInt64(hfExpenditureId.Value);
                Expenditure objExpenditure = Expenditure.GetByExpenditureID(lExpenditureId);
                string strStaffLevelExpenditureXml = string.Empty;

                objExpenditure.Amount = Convert.ToDouble(txtAmount.Text.Trim());
                //objExpenditure.Budget = Budget.GetByBudgetId(Convert.ToInt64(ddlBudgets.SelectedValue));
                objExpenditure.Comments = txtComments.Text.Trim();
                if (ExpenditureCategory.IsMonthly)
                {
                    objExpenditure.DateOfTransaction = Convert.ToDateTime(ddlMonths.SelectedValue);
                }
                else
                {
                    objExpenditure.DateOfTransaction = Convert.ToDateTime(txtDateOfTransaction.Text);
                }
                //objEpenditure.DateOfTransaction = DateTime.ParseExact(txtDateOfTransaction.Text, CalendarExtender1.Format, null);
                if (!ExpenditureCategory.IsFixed)
                {
                    objExpenditure.Description = txtDescription.Text.Trim();
                    objExpenditure.OBJCode = txtOBJCode.Text.Trim();
                    if (ExpenditureCategory.IsVendorStaff)
                    {
                        objExpenditure.VendorName = ddlStaffs.SelectedItem.Text;
                        if (ExpenditureCategory.IsVendorStaffAndOther && ddlStaffs.SelectedValue == "-2")
                        {
                            if (!String.IsNullOrEmpty(txtVendorName.Text.Trim()))
                            {
                                objExpenditure.VendorName = txtVendorName.Text.Trim();
                                Vendor.Create(txtVendorName.Text.Trim(), txtDescription.Text.Trim(), Convert.ToInt64(ddlOffices.SelectedValue), Convert.ToInt64(hfFiscalYearId.Value), false, false);
                            }
                        }
                        else
                        {
                            strStaffLevelExpenditureXml = _GetStaffLevelExpenditures(Convert.ToInt64(ddlStaffs.SelectedValue), Convert.ToDouble(txtAmount.Text.Trim()));
                        }
                    }
                    else
                    {
                        objExpenditure.VendorName = txtVendorName.Text.Trim();
                        Vendor.Create(txtVendorName.Text.Trim(), txtDescription.Text.Trim(), Convert.ToInt64(ddlOffices.SelectedValue), Convert.ToInt64(hfFiscalYearId.Value), false, false);

                    }
                }

                //     objEpenditure.Office = Office.GetByOfficeID(Convert.ToInt64(ddlOffices.SelectedValue));                               
                if (ExpenditureCategory.IsStaffLevel)
                {
                    strStaffLevelExpenditureXml = _GetStaffLevelExpenditures(null, null);
                }
                objExpenditure.Office.OfficeID = Convert.ToInt64(ddlOffices.SelectedValue);
                long officeId = Convert.ToInt64(ddlOffices.SelectedValue);
                Office objOffice = Office.GetByOfficeID(officeId);
                List<IDataHelper> lstBudgets = objOffice.Budgets(NPSRequestContext.GetContext().FiscalYearSelected.FiscalYearID);

                if (lstBudgets != null && lstBudgets.Count > 0)
                {
                    List<Budget> lstBudgetsConverted = lstBudgets.ConvertAll(q => (Budget)q);
                    Budget objBudget = lstBudgetsConverted.Single(q => q.IsDefault);
                    objExpenditure.Budget.BudgetID = objBudget.BudgetID;
                    objExpenditure.Update(strStaffLevelExpenditureXml);
                    UIHelper.SetSuccessMessage("Expenditure updated successfully");
                }
            }
            //Raising the SubmitClick event
            if (SubmitClick != null)
            {
                SubmitClick(this, EventArgs.Empty);
            }

            //Hiding the popup
            Mode = ExpenditureItemCtrlMode.NotSet;

        }

        private string _GetStaffLevelExpenditures(long? staffId, double? dblAmount)
        {
            XmlDocument xDoc = new XmlDocument();
            XmlNode xRootNode = null;
            XmlNode xChildNode = null;
            List<XmlNode> lstNodes = new List<XmlNode>();
            if (staffId.HasValue)
            {
                xRootNode = xDoc.CreateNode(XmlNodeType.Element, "stafflevelexpenditure", string.Empty);
                xChildNode = xDoc.CreateNode(XmlNodeType.Element, "staffid", string.Empty);
                xChildNode.InnerText = staffId.Value.ToString();
                xRootNode.AppendChild(xChildNode);
                xChildNode = xDoc.CreateNode(XmlNodeType.Element, "amount", string.Empty);
                xChildNode.InnerText = dblAmount.ToString().Trim().Replace(",", "");
                xRootNode.AppendChild(xChildNode);
                lstNodes.Add(xRootNode);
            }
            else
            {
                foreach (RepeaterItem objItem in rptrStaffLevelAmount.Items)
                {
                    HiddenField hfStaffID = objItem.FindControl("hfStaffID") as HiddenField;
                    TextBox txtStaffLevelAmount = objItem.FindControl("txtStaffLevelAmount") as TextBox;
                    if (!string.IsNullOrEmpty(txtStaffLevelAmount.Text.Trim()))
                    {
                        xRootNode = xDoc.CreateNode(XmlNodeType.Element, "stafflevelexpenditure", string.Empty);
                        xChildNode = xDoc.CreateNode(XmlNodeType.Element, "staffid", string.Empty);
                        xChildNode.InnerText = hfStaffID.Value;
                        xRootNode.AppendChild(xChildNode);
                        dblAmount = Convert.ToDouble(txtStaffLevelAmount.Text.Trim());
                        xChildNode = xDoc.CreateNode(XmlNodeType.Element, "amount", string.Empty);
                        xChildNode.InnerText = dblAmount.ToString().Trim().Replace(",", "");
                        xRootNode.AppendChild(xChildNode);
                        lstNodes.Add(xRootNode);
                    }
                }
            }

            xRootNode = xDoc.CreateNode(XmlNodeType.Element, "stafflevelexpenditures", string.Empty);
            foreach (XmlNode childNode in lstNodes)
            {
                xRootNode.AppendChild(childNode);
            }
            xDoc.AppendChild(xRootNode);
            return xDoc.OuterXml;
        }
        protected void lnkCancel_ServerClick(object sender, EventArgs e)
        {
            //Hiding the popup
            Mode = ExpenditureItemCtrlMode.NotSet;
            _HidePopup();

            //Raising the CancelClick event
            if (CancelClick != null)
            {
                CancelClick(this, EventArgs.Empty);
            }
        }
        #endregion

        protected void cvalVendorName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!ExpenditureCategory.IsFixed)
            {
                if (ExpenditureCategory.IsVendorStaff)
                {
                    CustomValidator cv = (CustomValidator)source;
                    if (ddlStaffs.SelectedValue == "-1")
                    {
                        cv.ErrorMessage = ExpenditureCategory.GetAttribute("VendorNameFieldText") + " should not be empty";
                        args.IsValid = false;
                        return;
                    }
                }
                else
                {
                    CustomValidator cv = (CustomValidator)source;
                    if (string.IsNullOrEmpty(txtVendorName.Text.Trim()))
                    {
                        cv.ErrorMessage = ExpenditureCategory.GetAttribute("VendorNameFieldText") + " should not be empty";
                        args.IsValid = false;
                        return;
                    }
                }
            }
        }

        protected void cvalOBJCode_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!ExpenditureCategory.IsFixed)
            {
                CustomValidator cv = (CustomValidator)source;
                if (string.IsNullOrEmpty(txtOBJCode.Text.Trim()))
                {
                    cv.ErrorMessage = "OBJ should not be empty";
                    args.IsValid = false;
                    return;
                }
            }
        }

        protected void cvalAmount_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            if (string.IsNullOrEmpty(txtAmount.Text.Trim()))
            {
                cv.ErrorMessage = "Amount should not be empty";
                args.IsValid = false;
                return;
            }
            double dblAmount = 0;
            if (!Double.TryParse(txtAmount.Text.Trim().Replace(",", ""), out dblAmount))
            {
                cv.ErrorMessage = "Enter a valid amount";
                args.IsValid = false;
                return;
            }
        }

        protected void cvalOffice_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            if (ddlOffices.SelectedIndex == 0 || ddlOffices.SelectedIndex == -1)
            {
                cv.ErrorMessage = "Select an Office";
                args.IsValid = false;
                return;
            }
            else
            {
                Office objOffice = Office.GetByOfficeID(Convert.ToInt64(ddlOffices.SelectedValue));
                List<IDataHelper> lstBudgets = objOffice.Budgets(NPSRequestContext.GetContext().FiscalYearSelected.FiscalYearID);
                if (!(lstBudgets != null && lstBudgets.Count > 0))
                {
                    cv.ErrorMessage = "Cannot create the expenditure since the budget is not set for the selected office";
                    args.IsValid = false;
                    return;
                }
            }
        }

        //protected void cvalBudget_ServerValidate(object source, ServerValidateEventArgs args)
        //{
        //    CustomValidator cv = (CustomValidator)source;
        //    if (ddlBudgets.SelectedIndex == 0 || ddlBudgets.SelectedIndex == -1)
        //    {
        //        cv.ErrorMessage = "Select a Budget";
        //        args.IsValid = false;
        //        return;
        //    }
        //}

        protected void ddlOffices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOffices.SelectedIndex != -1)
            {
                if (ddlOffices.SelectedValue == "-1")
                {
                    //trBudget.Visible = false;
                    phStaffLevel.Visible = false;
                    if (ExpenditureCategory.IsVendorStaff)                        
                        trVendorName.Visible = false;
                }
                else
                {
                    //_LoadBudgets(Convert.ToInt64(ddlOffices.SelectedValue), Convert.ToInt64(hfFiscalYearId.Value));
                    if (ExpenditureCategory.IsStaffLevel)
                    {
                        _BindStaffs(Office.GetByOfficeID(Convert.ToInt64(ddlOffices.SelectedValue)).OfficeID, null);
                    }
                    if (ExpenditureCategory.IsVendorStaff)
                    {
                        _LoadStaffForVendor(null, Convert.ToInt64(ddlOffices.SelectedValue));
                        trVendorName.Visible = true;                        
                    }                    
                    if (ExpenditureCategory.IsMonthly)
                    {
                        _LoadMonths();
                        trDateOfTransaction.Visible = true;
                        txtDateOfTransaction.Visible = false;
                        ddlMonths.Visible = true;
                    }
                }
            }
        }


        protected void rptrStaffLevelAmount_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Literal litStaffName = e.Item.FindControl("litStaffName") as Literal;
                HiddenField hfStaffID = e.Item.FindControl("hfStaffID") as HiddenField;

                StaffLevelExpenditure objStaffLevelExpenditure = e.Item.DataItem as StaffLevelExpenditure;
                litStaffName.Text = objStaffLevelExpenditure.Staff.FirstName + " " + objStaffLevelExpenditure.Staff.LastName;
                hfStaffID.Value = objStaffLevelExpenditure.Staff.StaffId.ToString();

                TextBox txtStaffLevelAmount = e.Item.FindControl("txtStaffLevelAmount") as TextBox;
                txtStaffLevelAmount.Text = objStaffLevelExpenditure.Amount.ToString();
            }
        }

        protected void cvalStaffLevelAmount_ServerValidate(object source, ServerValidateEventArgs args)
        {
            // Cast the sender object to a CustomValidator
            CustomValidator vldCustomValidator = (CustomValidator)source;
            // Find the RepeaterItem, this is the parent object of the CustomValidator
            RepeaterItem rptItem = (RepeaterItem)vldCustomValidator.Parent;
            // Now find the control in the RepeaterItem
            TextBox txtStaffLevelAmount = (TextBox)rptItem.FindControl("txtStaffLevelAmount");
            // something to return
            if (!string.IsNullOrEmpty(txtStaffLevelAmount.Text.Trim()))
            {
                double dblAmount = 0;
                if (!Double.TryParse(txtStaffLevelAmount.Text.Trim().Replace(",", ""), out dblAmount))
                {
                    args.IsValid = false;
                    vldCustomValidator.ErrorMessage = "Invalid amount";
                }
            }
        }

        protected void txtStaffLevelAmount_TextChanged(object sender, EventArgs e)
        {
            Page.Validate("ValGroupStaffLevelAmount");
            if (!Page.IsValid)
            {
                _ShowPopup();
                return;
            }

            _SetTotalAmount();
        }

        private void _SetTotalAmount()
        {
            double dblTotal = 0;
            foreach (RepeaterItem rptrItem in rptrStaffLevelAmount.Items)
            {
                TextBox txtStaffLevelAmount = (TextBox)rptrItem.FindControl("txtStaffLevelAmount");
                if (!string.IsNullOrEmpty(txtStaffLevelAmount.Text.Trim()))
                {
                    double dblAmount = 0;
                    if (Double.TryParse(txtStaffLevelAmount.Text.Trim(), out dblAmount))
                    {
                        dblTotal += dblAmount;
                    }
                }
            }
            txtAmount.Text = dblTotal.ToString();
        }

        protected void cvalUniqueId_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }

        protected void cvalDateOfTransaction_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            DateTime dtDate = new DateTime();
            if (ExpenditureCategory.IsMonthly)
            {
                if (!DateTime.TryParse(ddlMonths.SelectedValue.Trim(), out dtDate))
                {
                    cv.ErrorMessage = "Please select the Month for this " + ExpenditureCategory.Name;
                    args.IsValid = false;
                    return;
                }
                FiscalYear objFiscalYear = FiscalYear.GetByFiscalYearID(Convert.ToInt64(hfFiscalYearId.Value));
                if ((DateTime.Compare(Convert.ToDateTime(ddlMonths.SelectedValue.Trim()), objFiscalYear.StartDate) < 0) || (DateTime.Compare(Convert.ToDateTime(ddlMonths.SelectedValue.Trim()), objFiscalYear.EndDate) > 0))
                {
                    cv.ErrorMessage = String.Format("Month should be between Fiscal Year Date Range ({0} - {1})", objFiscalYear.StartDate.ToShortDateString(), objFiscalYear.EndDate.ToShortDateString());
                    args.IsValid = false;
                    return;
                }

                if (ddlOffices.SelectedValue != null)
                {
                    long lOfficeId = Convert.ToInt64(ddlOffices.SelectedValue);
                    Office objOffice = Office.GetByOfficeID(lOfficeId);
                    if (DateTime.Compare(Convert.ToDateTime(ddlMonths.SelectedValue), objOffice.ActiveFrom) < 0)
                    {
                        cv.ErrorMessage = String.Format("Date Of Transaction should be after active office date {0}", objOffice.ActiveFrom.ToShortDateString());
                        args.IsValid = false;
                        return;
                    }
                    if (objOffice.ActiveTo.HasValue == true)
                    {
                        if (DateTime.Compare(Convert.ToDateTime(ddlMonths.SelectedValue), objOffice.ActiveTo.Value) > 0)
                        {
                            cv.ErrorMessage = String.Format("Date Of Transaction should be with in office active date range ({0} - {1})", objOffice.ActiveFrom.ToShortDateString(), objOffice.ActiveTo.Value.ToShortDateString());
                            args.IsValid = false;
                            return;
                        }
                    }
                }


            }
            else
            {
                if (!DateTime.TryParse(txtDateOfTransaction.Text.Trim(), out dtDate))
                {
                    cv.ErrorMessage = "Please enter a valid Date Of Transaction";
                    args.IsValid = false;
                    return;
                }
                FiscalYear objFiscalYear = FiscalYear.GetByFiscalYearID(Convert.ToInt64(hfFiscalYearId.Value));
                if ((DateTime.Compare(Convert.ToDateTime(txtDateOfTransaction.Text.Trim()), objFiscalYear.StartDate) < 0) || (DateTime.Compare(Convert.ToDateTime(txtDateOfTransaction.Text.Trim()), objFiscalYear.EndDate) > 0))
                {
                    cv.ErrorMessage = String.Format("Date Of Transaction should be in Fiscal Year Date Range ({0} - {1})", objFiscalYear.StartDate.ToShortDateString(), objFiscalYear.EndDate.ToShortDateString());
                    args.IsValid = false;
                    return;
                }

                if (ddlOffices.SelectedValue != null)
                {
                    long lOfficeId = Convert.ToInt64(ddlOffices.SelectedValue);
                    Office objOffice = Office.GetByOfficeID(lOfficeId);
                    if (DateTime.Compare(Convert.ToDateTime(txtDateOfTransaction.Text.Trim()), objOffice.ActiveFrom) < 0)
                    {
                        cv.ErrorMessage = String.Format("Date Of Transaction should be after Active Office Date {0}", objOffice.ActiveFrom.ToShortDateString());
                        args.IsValid = false;
                        return;
                    }
                    if (objOffice.ActiveTo.HasValue == true)
                    {
                        if (DateTime.Compare(Convert.ToDateTime(txtDateOfTransaction.Text.Trim()), objOffice.ActiveTo.Value) > 0)
                        {
                            cv.ErrorMessage = String.Format("Date Of Transaction should be with in office active date range ({0} - {1})", objOffice.ActiveFrom.ToShortDateString(), objOffice.ActiveTo.Value.ToShortDateString());
                            args.IsValid = false;
                            return;
                        }
                    }
                }

            }

        }

        protected void ddlMonths_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMonths.SelectedValue != "-1")
            {
                List<Expenditure> lstExpenditure = Expenditure.GetAll(
                    string.Empty,
                    String.Format("<officeids><officeid>{0}</officeid></officeids>",
                    ddlOffices.SelectedValue),
                    NPSRequestContext.GetContext().FiscalYearSelected.FiscalYearID,
                    String.Format("<expenditurecategoryids><expenditurecategoryid>{0}</expenditurecategoryid></expenditurecategoryids>",
                    ExpenditureCategory.ExpenditureCategoryID),
                    null,
                    -1,
                    null,
                    Core.NPSCommon.Enums.SortFields.ExpenditureSortFields.Amount,
                    Core.NPSCommon.Enums.OrderByDirection.Ascending
                    ).Items.ConvertAll(q => (Expenditure)q);

                List<Expenditure> lstExpenditureFiltered = new List<Expenditure>();
             
               

                foreach (Expenditure objExp in lstExpenditure)
                {
                    if (objExp.DateOfTransaction.Date == Convert.ToDateTime(ddlMonths.SelectedValue).Date)
                    {
                        lstExpenditureFiltered.Add(objExp);
                    }
                }

                if (lstExpenditureFiltered.Count == 1)
                {
                    Expenditure objExpenditure = lstExpenditureFiltered[0];
                    List<StaffLevelExpenditure> lstStaffLevelExpenditures = StaffLevelExpenditure.GetAllByExpenditureId(objExpenditure.ExpenditureID);
                    _BindStaffs(Office.GetByOfficeID(Convert.ToInt64(ddlOffices.SelectedValue)).OfficeID, lstStaffLevelExpenditures);
                    txtAmount.Text = objExpenditure.Amount.ToString();
                    txtComments.Text = objExpenditure.Comments;
                    Mode = ExpenditureItemCtrlMode.Edit;
                    hfExpenditureId.Value = objExpenditure.ExpenditureID.ToString();
                    hfOfficeName.Value = Office.GetByOfficeID(Convert.ToInt64(ddlOffices.SelectedValue)).Name;


                }
                else
                {
                    Mode = ExpenditureItemCtrlMode.Create;
                    _BindStaffs(Office.GetByOfficeID(Convert.ToInt64(ddlOffices.SelectedValue)).OfficeID,null);
                    txtAmount.Text = "";
                    txtComments.Text = "";
                    hfOfficeName.Value = "";
                    hfExpenditureId.Value = "";
                }
            }
            else
            {
                Mode = ExpenditureItemCtrlMode.Create;
                txtAmount.Text = "";
                txtComments.Text = "";
                hfOfficeName.Value = "";
                hfExpenditureId.Value = "";
            }

        }

        protected void ddlStaffs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStaffs.SelectedValue == "-2")
            {
                txtVendorName.Visible = true;
                trIsVendor.Visible = true;
            }
            else
            {
                txtVendorName.Visible = false;
                trIsVendor.Visible = false;
            }
        }

    }
}