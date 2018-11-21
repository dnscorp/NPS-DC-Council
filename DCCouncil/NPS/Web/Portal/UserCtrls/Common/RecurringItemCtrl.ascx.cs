using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.UserCtrlModes;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Common
{
    public partial class RecurringItemCtrl : System.Web.UI.UserControl
    {
        #region Public Events
        public event EventHandler SubmitClick;
        public event EventHandler CancelClick;
        #endregion

        #region Public Properties
        public RecurringItemCtrlMode Mode
        {
            get
            {
                if (string.IsNullOrEmpty(hfMode.Value))
                {
                    return RecurringItemCtrlMode.NotSet;
                }
                else
                {
                    return (RecurringItemCtrlMode)Convert.ToInt32(hfMode.Value);
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

        private static string GetDbConnectionString()
        {
            return PRIFACT.DCCouncil.NPS.Core.NPSCommon.AppSettings.DbConnectionString;
        }


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
        }

        private void _SetUI()
        {
            if (Mode == RecurringItemCtrlMode.Create)
            {
                _ShowPopup();
                lnkSubmit.InnerText = "Create";
                litOffice.Visible = false;
                ddlOffices.Visible = true;
                litHeading.Text = "Create";
            }
            if (Mode == RecurringItemCtrlMode.Edit)
            {
                _ShowPopup();
                lnkSubmit.InnerText = "Update";
                litOffice.Visible = false;
                ddlOffices.Visible = false;
                litHeading.Text = "Update";
                
            }
            if (Mode == RecurringItemCtrlMode.NotSet)
            {
                _HidePopup();
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
        protected void cvalVendorName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            if (string.IsNullOrEmpty(txtVendorName.Text.Trim()))
            {
                cv.ErrorMessage = "Vendor name should not be empty";
                args.IsValid = false;
                return;
            }
        }
        protected void cvalDescription_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
            {
                cv.ErrorMessage = "Description should not be empty";
                args.IsValid = false;
                return;
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

        protected void lnkSubmit_ServerClick(object sender, EventArgs e)
        {
            //Validation
            Page.Validate("ValGroup1");
            
            if (!Page.IsValid)
            {
                _ShowPopup();
                return;
            }

            if (Mode == RecurringItemCtrlMode.Create)
            {
                //TODO Change to date from control
                long officeId = Convert.ToInt64(ddlOffices.SelectedValue);
                Office objOffice = Office.GetByOfficeID(officeId);
                string strDescription = string.Empty;
                string strVendorName = string.Empty;
                string strRecurringCategory = string.Empty;

                strDescription = txtDescription.Text.Trim();
                strVendorName = txtVendorName.Text.Trim();
                strRecurringCategory = ddlCategory.SelectedValue;

                List<IDataHelper> lstBudgets = objOffice.Budgets(NPSRequestContext.GetContext().FiscalYearSelected.FiscalYearID);
                if (lstBudgets != null && lstBudgets.Count > 0)
                {
                    List<Budget> lstBudgetsConverted = lstBudgets.ConvertAll(q => (Budget)q);
                    Budget objBudget = lstBudgetsConverted.Single(q => q.IsDefault);
                    RecurringTransactions.Create(strRecurringCategory, strVendorName, strDescription, Convert.ToDouble(txtAmount.Text.Trim().Replace(",", "")), Convert.ToInt64(ddlOffices.SelectedValue), txtComments.Text.Trim(), Convert.ToInt64(hfFiscalYearId.Value), objBudget.BudgetID, false);
                    UIHelper.SetSuccessMessage("Recurring Transaction created successfully");
                }
                else
                {
                    throw new Exception("Budget not set for the Office");
                }
            }
            if (Mode == RecurringItemCtrlMode.Edit)
            {
                long lhfRecurringID = Convert.ToInt64(hfRecurringID.Value);
                string strDescription = string.Empty;
                string strVendorName = string.Empty;
                string strRecurringCategory = string.Empty;

                long officeId = Convert.ToInt64(ddlOffices.SelectedValue);
                strDescription = txtDescription.Text.Trim();
                strVendorName = txtVendorName.Text.Trim();
                strRecurringCategory = ddlCategory.SelectedValue;

                Office objOffice = Office.GetByOfficeID(officeId);
                List<IDataHelper> lstBudgets = objOffice.Budgets(NPSRequestContext.GetContext().FiscalYearSelected.FiscalYearID);

                if (lstBudgets != null && lstBudgets.Count > 0)
                {
                    List<Budget> lstBudgetsConverted = lstBudgets.ConvertAll(q => (Budget)q);
                    Budget objBudget = lstBudgetsConverted.Single(q => q.IsDefault);
                    RecurringTransactions.Update(lhfRecurringID, strRecurringCategory, strVendorName, strDescription, Convert.ToDouble(txtAmount.Text.Trim().Replace(",", "")), Convert.ToInt64(ddlOffices.SelectedValue), txtComments.Text.Trim(), Convert.ToInt64(hfFiscalYearId.Value), objBudget.BudgetID, false);
                    UIHelper.SetSuccessMessage("Recurring Transaction updated successfully");
                }
            }
            //Raising the SubmitClick event
            if (SubmitClick != null)
            {
                SubmitClick(this, EventArgs.Empty);
            }

            //Hiding the popup
            Mode = RecurringItemCtrlMode.NotSet;
        }

        protected void lnkCancel_ServerClick(object sender, EventArgs e)
        {
            //Hiding the popup
            Mode = RecurringItemCtrlMode.NotSet;
            _HidePopup();

            //Raising the CancelClick event
            if (CancelClick != null)
            {
                CancelClick(this, EventArgs.Empty);
            }
        }

        #region Public Setter and Getter Methods
        public void InitializeFields(RecurringTransactions objRecurring)
        {
            if (objRecurring != null)
            {
                //Set the fields
                hfRecurringID.Value = objRecurring.RecurringID.ToString();
                hfFiscalYearId.Value = objRecurring.FiscalYear.FiscalYearID.ToString();
                litFiscalYearSelected.Text = objRecurring.FiscalYear.Name;

                txtVendorName.Text = objRecurring.VendorName;
                ddlCategory.SelectedValue = objRecurring.RecurringCategory;
                txtDescription.Text = objRecurring.Description;
                ddlOffices.Visible = true;
                ddlOffices.SelectedValue = objRecurring.Office.OfficeID.ToString();
                litOffice.Text = objRecurring.Office.Name;
                txtAmount.Text = objRecurring.Amount.ToString();
                txtComments.Text = objRecurring.Comments;
            }
            else
            {
                //Initialise the fields
                hfRecurringID.Value = string.Empty;
                hfFiscalYearId.Value = NPSRequestContext.GetContext().FiscalYearSelected.FiscalYearID.ToString();
                litFiscalYearSelected.Text = NPSRequestContext.GetContext().FiscalYearSelected.Name;
                
                txtVendorName.Text = string.Empty;
                txtDescription.Text = string.Empty;
                ddlOffices.SelectedValue = "-1";
                txtAmount.Text = string.Empty;
                txtComments.Text = string.Empty;
            }

        }

        private void _HidePopup()
        {
            divItemPopup.Attributes.Add("style", "display: none");
        }

        private void _ShowPopup()
        {
            divItemPopup.Attributes.Add("style", "display:block");
        }

        #endregion

    }
}