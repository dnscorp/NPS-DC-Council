using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.SortFields;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.UserCtrlModes;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using PRIFACT.PRIFACTBase.UrlHelpers;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.SiteAdministration.BudgetManangement
{
    public partial class BudgetManagementItemCtrl : System.Web.UI.UserControl
    {
        #region Public Events
        public event EventHandler SubmitClick;
        public event EventHandler CancelClick;
        #endregion

        #region Public Properties
        public BudgetManagementItemCtrlMode Mode
        {
            get
            {
                if (string.IsNullOrEmpty(hfMode.Value))
                {
                    return BudgetManagementItemCtrlMode.NotSet;
                }
                else
                {
                    return (BudgetManagementItemCtrlMode)Convert.ToInt32(hfMode.Value);
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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            hfOfficeId.Value = UrlUtility.GetStringFromQv(Request["OfficeId"]);

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            _SetUI();
        }
        #endregion

        #region Public Setter and Getter Methods
        public void InitializeFields(Budget objBudget, string strMode)
        {
            if (objBudget != null)
            {
                hfBudgetId.Value = objBudget.BudgetID.ToString();
                txtBudgetName.Text = objBudget.Name;
                //txtAmount.Text = objBudget.Amount.ToString();
                //chkIsDefault.Checked = objBudget.IsDefault;
                hfFiscalYearId.Value = objBudget.FiscalYear.FiscalYearID.ToString();
                chkTrainingExpense.Checked = objBudget.IsTrainingExpense;
                if (objBudget.IsDefault)
                {
                    litBudgetHeader.Text = "Update Default Budget";
                    txtAmount.Text = objBudget.Amount.ToString();
                }
                else
                {
                    if (objBudget.IsDeduct)
                    {
                        litBudgetHeader.Text = "Update Reprogramming(Less)";
                        litBudgetAmount.Text = "Amount to be deducted";
                        txtAmount.Text = (objBudget.Amount * -1).ToString();
                    }
                    else
                    {
                        litBudgetHeader.Text = "Update Reprogramming(Add)";
                        txtAmount.Text = objBudget.Amount.ToString();
                        litBudgetAmount.Text = "Amount to be added";
                    }
                }

            }
            else
            {
                txtBudgetName.Text = string.Empty;
                txtAmount.Text = string.Empty;
                //chkIsDefault.Checked = false;
                User objUser = NPSRequestContext.GetContext().LoggedInUser;
                hfFiscalYearId.Value = objUser.LastFiscalYearSelected.FiscalYearID.ToString();
                if (strMode.ToLower() == "create")
                {
                    litBudgetHeader.Text = "Add Default Budget";
                }
                else if (strMode.ToLower() == "addfunds")
                {
                    litBudgetHeader.Text = "Add funds to default budget";
                }

                else if (strMode.ToLower() == "deductfunds")
                {
                    litBudgetHeader.Text = "Deduct funds from default budget";
                }

            }
        }
        #endregion

        #region Private Methods

        private void _SetUI()
        {
            if (Mode == BudgetManagementItemCtrlMode.Create)
            {
                _ShowPopup();
                lnkSubmit.InnerText = "Create";
                litBudgetAmount.Text = "Amount";
            }
            if (Mode == BudgetManagementItemCtrlMode.Edit)
            {
                _ShowPopup();
                lnkSubmit.InnerText = "Update";
                litBudgetAmount.Text = "Amount";
            }
            if (Mode == BudgetManagementItemCtrlMode.DeductFunds)
            {
                _ShowPopup();
                lnkSubmit.InnerText = "Create";
                litBudgetAmount.Text = "Amount to be deducted";
            }
            if (Mode == BudgetManagementItemCtrlMode.AddFunds)
            {
                _ShowPopup();
                lnkSubmit.InnerText = "Create";
                litBudgetAmount.Text = "Amount to be added ";
            }
            if (Mode == BudgetManagementItemCtrlMode.AddFundsEdit)
            {
                _ShowPopup();
                lnkSubmit.InnerText = "Update";
                
            }
            if (Mode == BudgetManagementItemCtrlMode.NotSet)
            {
                _HidePopup();
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

        #region Submit and Cancel Button Clicks
        protected void lnkSubmit_ServerClick(object sender, EventArgs e)
        {
            //Validation
            Page.Validate("ValGroup1");
            if (!Page.IsValid)
            {
                _ShowPopup();
                return;
            }

            if (Mode == BudgetManagementItemCtrlMode.Create)
            {
                FiscalYear objFiscalYear = FiscalYear.GetByFiscalYearID(Convert.ToInt64(hfFiscalYearId.Value));
                Office objOffice = Office.GetByOfficeID(Convert.ToInt64(hfOfficeId.Value));
                int isDefaultCount = 0;
                foreach (Budget budget in Budget.GetAll(String.Empty, objOffice.OfficeID, objFiscalYear.FiscalYearID, -1, null, BudgetSortFields.FiscalYear, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items)
                {
                    if (budget.IsDefault)
                        isDefaultCount++;
                }
                //if (chkIsDefault.Checked)
                //{
                //if (isDefaultCount < 1)
                //{
                Budget.Create(txtBudgetName.Text.Trim(), Convert.ToDouble(txtAmount.Text.Replace(",", "")), isDefaultCount < 1 ? true : false, Convert.ToInt64(hfFiscalYearId.Value), Convert.ToInt64(hfOfficeId.Value), false, false,chkTrainingExpense.Checked);
                UIHelper.SetSuccessMessage("Budget created successfully");
                //}
                //}
                //else
                //{
                //    Budget.Create(txtBudgetName.Text.Trim(), Convert.ToDouble(txtAmount.Text.Replace(",", "")), false, Convert.ToInt64(hfFiscalYearId.Value), Convert.ToInt64(hfOfficeId.Value), false);
                //    UIHelper.SetSuccessMessage("Budget created successfully");
                //}
            }

            if (Mode == BudgetManagementItemCtrlMode.AddFunds)
            {
                FiscalYear objFiscalYear = FiscalYear.GetByFiscalYearID(Convert.ToInt64(hfFiscalYearId.Value));
                Office objOffice = Office.GetByOfficeID(Convert.ToInt64(hfOfficeId.Value));

                Budget.Create(txtBudgetName.Text.Trim(), Convert.ToDouble(txtAmount.Text.Replace(",", "")), false, Convert.ToInt64(hfFiscalYearId.Value), Convert.ToInt64(hfOfficeId.Value), false, false,chkTrainingExpense.Checked);
                UIHelper.SetSuccessMessage("Reprogramming budget added  successfully");

            }
            if (Mode == BudgetManagementItemCtrlMode.DeductFunds)
            {
                FiscalYear objFiscalYear = FiscalYear.GetByFiscalYearID(Convert.ToInt64(hfFiscalYearId.Value));
                Office objOffice = Office.GetByOfficeID(Convert.ToInt64(hfOfficeId.Value));
                Double DeductedAmount = Convert.ToDouble(txtAmount.Text.Replace(",", ""));
                DeductedAmount = DeductedAmount * -1;

                Budget.Create(txtBudgetName.Text.Trim(), DeductedAmount, false, Convert.ToInt64(hfFiscalYearId.Value), Convert.ToInt64(hfOfficeId.Value), false, true,chkTrainingExpense.Checked);
                UIHelper.SetSuccessMessage("Reprogramming budget(Deduction) added successfully");

            }

            if (Mode == BudgetManagementItemCtrlMode.AddFundsEdit)
            {
                long lBudgetId = Convert.ToInt64(hfBudgetId.Value);
                Budget objBudget = Budget.GetByBudgetId(lBudgetId);
                objBudget.Name = txtBudgetName.Text.Trim();
                Double dblAmount = Convert.ToDouble(txtAmount.Text);
                if (objBudget.IsDeduct)
                { dblAmount = dblAmount * -1; }
                objBudget.Amount = dblAmount;
                objBudget.FiscalYearID = Convert.ToInt64(hfFiscalYearId.Value);
                objBudget.IsTrainingExpense = chkTrainingExpense.Checked;
                objBudget.Update();
                UIHelper.SetSuccessMessage("Budget updated successfully");

            }

            if (Mode == BudgetManagementItemCtrlMode.Edit)
            {
                //Updating the user
                long lBudgetId = Convert.ToInt64(hfBudgetId.Value);
                Budget objBudget = Budget.GetByBudgetId(lBudgetId);
                objBudget.Name = txtBudgetName.Text.Trim();
                objBudget.Amount = Convert.ToDouble(txtAmount.Text.Trim());
                objBudget.FiscalYearID = Convert.ToInt64(hfFiscalYearId.Value);
                objBudget.IsTrainingExpense = chkTrainingExpense.Checked;
                objBudget.Update();
                UIHelper.SetSuccessMessage("Budget updated successfully");
            }

            //Raising the SubmitClick event
            if (SubmitClick != null)
            {
                SubmitClick(this, EventArgs.Empty);
            }

        }

        protected void lnkCancel_ServerClick(object sender, EventArgs e)
        {
            //Hiding the popup
            Mode = BudgetManagementItemCtrlMode.NotSet;
            _HidePopup();

            //Raising the CancelClick event
            if (CancelClick != null)
            {
                CancelClick(this, EventArgs.Empty);
            }
        }
        #endregion
        protected void cvalBudgetName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            if (string.IsNullOrEmpty(txtBudgetName.Text.Trim()))
            {
                cv.ErrorMessage = "Budget Name should not be empty";
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
            if (!Double.TryParse((txtAmount.Text.Trim().Replace(",", "")), out dblAmount))
            {
                cv.ErrorMessage = "Enter a valid amount";
                args.IsValid = false;
                return;
            }
        }

        //protected void cvalIsDefault_ServerValidate(object source, ServerValidateEventArgs args)
        //{
        //    CustomValidator cv = (CustomValidator)source;
        //    if (chkIsDefault.Checked)
        //    {
        //        FiscalYear objFiscalYear = FiscalYear.GetByFiscalYearID(Convert.ToInt64(hfFiscalYearId.Value));
        //        Office objOffice = Office.GetByOfficeID(Convert.ToInt64(hfOfficeId.Value));
        //        int isDefaultCount = 0;

        //        foreach (Budget budget in Budget.GetAll(String.Empty, objOffice.OfficeID, objFiscalYear.FiscalYearID, -1, null, BudgetSortFields.FiscalYear, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items)
        //        {
        //            if (Mode == BudgetManagementItemCtrlMode.Create)
        //            {
        //                if (budget.IsDefault)
        //                    isDefaultCount++;
        //            }
        //            else if (Mode == BudgetManagementItemCtrlMode.Edit)
        //            {
        //                if (budget.IsDefault && budget.BudgetID != Convert.ToInt64(hfBudgetId.Value))
        //                    isDefaultCount++;
        //            }
        //        }

        //        if (isDefaultCount >= 1)
        //        {
        //            cv.ErrorMessage = "Cannot Set as Default Budget as there is already a default Budget for this Fiscal Year.";
        //            args.IsValid = false;
        //            return;
        //        }

        //    }
        //}
    }
}