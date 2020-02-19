using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.SortFields;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.UserCtrlModes;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Common;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using PRIFACT.PRIFACTBase.MiscHelpers;
using PRIFACT.PRIFACTBase.UrlHelpers;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.SiteAdministration
{
    public partial class BudgetManagementCtrl : System.Web.UI.UserControl
    {
        #region Private Properties
        //Properties for sorting in the repeater
        private BudgetSortFields SortField
        {
            get
            {
                return EnumHelper.ParseEnum<BudgetSortFields>(hfSortField.Value);
            }
            set
            {
                hfSortField.Value = value.ToString();
            }
        }

        private OrderByDirection OrderByDirection
        {
            get
            {
                return EnumHelper.ParseEnum<OrderByDirection>(hfOrderByDirection.Value);
            }
            set
            {
                hfOrderByDirection.Value = value.ToString();
            }
        }

        #endregion

        public Office Office
        {
            get;
            set;
        }
        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            _SetUI();
            //The data needs to be binded here since the viewstate is disabled and we need the posted values to determine the data to be fetched.
            _BindData(txtSearch.Text.Trim(), PagerCtrl1.PageSize, PagerCtrl1.CurrentPage, SortField, OrderByDirection);

            txtSearch.Attributes.Add("onchange", string.Format("javascript:doButtonClick('{0}');", bttn.ClientID));
        }

        private void _SetUI()
        {
            txtSearch.Attributes.Add("placeholder", "Search");
            litOfficeName.Text = Office.Name;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            //Setting the success message if present in Immediate Session
            _SetMessage();
        }
        #endregion
        #region Search Header Events
        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //No need to do anything here since the data is bound on PageLoad            
        }

        protected void lnkCreate_ServerClick(object sender, EventArgs e)
        {
            _ShowItemPopup(BudgetManagementItemCtrlMode.Create, null,"Create");
        }
        protected void lnkCreate_AddFundsServerClick(object sender, EventArgs e)
        {
            _ShowItemPopup(BudgetManagementItemCtrlMode.AddFunds, null, "AddFunds");
        }
        protected void lnkCreate_DeductFundsServerClick(object sender, EventArgs e)
        {
            _ShowItemPopup(BudgetManagementItemCtrlMode.DeductFunds, null, "DeductFunds");
        }
        #endregion

        #region Repeater Events
        protected void lnkEdit_ServerClick(object sender, EventArgs e)
        {
            HtmlAnchor lnkEdit = (HtmlAnchor)sender;
            long lBudgetId = Convert.ToInt64(lnkEdit.Attributes["BudgetId"]);
            Budget objBudget = Budget.GetByBudgetId(lBudgetId);
            if (objBudget.IsDefault)
                _ShowItemPopup(BudgetManagementItemCtrlMode.Edit, objBudget, string.Empty);
            else
            {
                _ShowItemPopup(BudgetManagementItemCtrlMode.AddFundsEdit, objBudget, string.Empty);
            }
        }

        protected void lnkDelete_ServerClick(object sender, EventArgs e)
        {
            HtmlAnchor lnkDelete = (HtmlAnchor)sender;
            PopupCtrl1.SetProperties(Convert.ToInt64(lnkDelete.Attributes["BudgetId"]).ToString(), true, "Delete", "Cancel", "Confirm Delete", "Are you sure you want to Delete the selected Budget?", "DeleteBudget");
            PopupCtrl1.Show();
        }

        protected void rptrResult_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Literal litName = e.Item.FindControl("litName") as Literal;
                Literal litAmount = e.Item.FindControl("litAmount") as Literal;
                Literal litBudgetType = e.Item.FindControl("litBudgetType") as Literal;
                Literal litTrainingExpense = e.Item.FindControl("litTrainingExpense") as Literal;
                Literal litCreatedDate = e.Item.FindControl("litCreatedDate") as Literal;
                Literal litUpdatedDate = e.Item.FindControl("litUpdatedDate") as Literal;

                Budget objBudget = e.Item.DataItem as Budget;

                litName.Text = objBudget.Name;
                litAmount.Text = UIHelper.GetAmountInDefaultFormat(objBudget.Amount);
                litBudgetType.Text = objBudget.IsDefault ? "Default" : "Reprogramming";
                litTrainingExpense.Text = objBudget.IsTrainingExpense ? "Yes" : "No";
                litCreatedDate.Text = UIHelper.GetDateTimeInDefaultFormat(objBudget.CreatedDate);
                litUpdatedDate.Text = UIHelper.GetDateTimeInDefaultFormat(objBudget.UpdatedDate);

                HtmlAnchor lnkEdit = e.Item.FindControl("lnkEdit") as HtmlAnchor;
                HtmlAnchor lnkDelete = e.Item.FindControl("lnkDelete") as HtmlAnchor;

                lnkEdit.Attributes.Add("BudgetId", objBudget.BudgetID.ToString());
                lnkDelete.Attributes.Add("BudgetId", objBudget.BudgetID.ToString());
                if (objBudget.IsDefault)
                    lnkDelete.Visible = false;
            }
        }
        #endregion

        #region Item Popup Events
        //Once the data is updated inside the child item popup, the main management control needs to be refreshed
        protected void BudgetManagementItemCtrl1_SubmitClick(object sender, EventArgs e)
        {
            //Things to be done after Submit is clicked in the child item popup
            _ResetAll();
        }

        protected void BudgetManagementItemCtrl1_CancelClick(object sender, EventArgs e)
        {
            //Things to be done after Cance is clicked in the child item popup
            _CloseItemPopup();
        }
        #endregion

        #region Paging and Sorting Events
        //Binding the data based on page number
        protected void PagerCtrl1_BindMainRepeater(object sender, EventArgs e)
        {
            _BindData(txtSearch.Text.Trim(), PagerCtrl1.PageSize, PagerCtrl1.CurrentPage, SortField, OrderByDirection);
        }

        //Sorting done here
        protected void rptrResult_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            BudgetSortFields selectedSortField = EnumHelper.ParseEnum<BudgetSortFields>(e.CommandName);
            if (selectedSortField == SortField)
            {
                _SwitchOrderByDirection();
                _BindData(txtSearch.Text.Trim(), PagerCtrl1.PageSize, PagerCtrl1.CurrentPage, SortField, OrderByDirection);
            }
            else
            {
                SortField = selectedSortField;
                OrderByDirection = Core.NPSCommon.Enums.OrderByDirection.Ascending;
                _BindData(txtSearch.Text.Trim(), PagerCtrl1.PageSize, PagerCtrl1.CurrentPage, SortField, OrderByDirection);
            }
        }
        #endregion

        #region Confirmation Popup Events
        protected void PopupCtrl1_OkayButtonClick(object sender, EventArgs e)
        {
            PopupCtrl popUpCtrl = (PopupCtrl)sender;
            long lBudgetId = Convert.ToInt64(popUpCtrl.IdToProcess);
            if (popUpCtrl.Mode.Equals("DeleteBudget"))
            {
                Budget objBudget = Budget.GetByBudgetId(lBudgetId);
                int expenseCount = 0;
                User objUser = NPSRequestContext.GetContext().LoggedInUser;
                List<Expenditure> lstExpenditures = Expenditure.GetAll(String.Empty, String.Empty, objUser.LastFiscalYearSelected.FiscalYearID, String.Empty, null, -1, null, ExpenditureSortFields.IndexCode, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items.ConvertAll(q => (Expenditure)q);
                if (lstExpenditures.Count > 0)
                {
                    foreach (Expenditure objExpenditure in lstExpenditures)
                    {
                        if (objExpenditure.BudgetID == objBudget.BudgetID)
                            expenseCount++;
                    }
                    if (expenseCount > 0)
                    {
                        UIHelper.SetErrorMessage("Unable to delete budget as there are expenditures under it.");
                    }
                    else
                    {
                        objBudget.IsDeleted = true;
                        objBudget.Update();
                        UIHelper.SetSuccessMessage("Budget deleted successfully");
                    }
                }
                else
                {
                    objBudget.IsDeleted = true;
                    objBudget.Update();
                    UIHelper.SetSuccessMessage("Budget deleted successfully");
                }
            }
            _ResetAll();
        }

        protected void PopupCtrl1_CancelButtonClick(object sender, EventArgs e)
        {
            //Raised when cancel is clicked on the confirmation popup
            PopupCtrl1.Hide();
        }
        #endregion

        #region Private Methods

        //Binding the data to repeater initially
        private void _InitializeRepeater()
        {
            _BindData(txtSearch.Text.Trim(), PagerCtrl1.PageSize, 1, SortField, OrderByDirection);
        }

        //Bind the data to the repeater based on the page selected and sort order
        private void _BindData(string strSearchText, int? iPageSize, int? iPageNumber, BudgetSortFields sortField, OrderByDirection orderByDirection)
        {

            ResultInfo objResultInfo = null;
            User objUser = NPSRequestContext.GetContext().LoggedInUser;
            objResultInfo = Budget.GetAll(strSearchText, Office.OfficeID, objUser.LastFiscalYearSelected.FiscalYearID, iPageSize, iPageNumber, sortField, orderByDirection);

            if (objResultInfo != null)
            {
                if (objResultInfo.Items.Count > 0)
                {
                    PagerCtrl1.Visible = true;
                    rptrResult.Visible = true;
                    litNoResults.Visible = false;

                    //Initilizing the pager control
                    PagerCtrl1.SetPager(objResultInfo.RowCount);
                    rptrResult.DataSource = objResultInfo.Items;
                    rptrResult.DataBind();
                    lnkAddFunds.Visible = true;
                    lnkDeductFunds.Visible = true;
                    lnkCreateBudget.Visible = false;
                    
                }
                else
                {
                    PagerCtrl1.Visible = false;
                    litNoResults.Visible = true;
                    litNoResults.Text = "No results found.";
                    rptrResult.Visible = false;
                    lnkCreateBudget.Visible = true;
                    lnkAddFunds.Visible = false;
                    lnkDeductFunds.Visible = false;
                    
                }
                //Binding the data

            }
            else
            {
                PagerCtrl1.Visible = false;
                rptrResult.Visible = false;
                lnkCreateBudget.Visible = true;
                lnkAddFunds.Visible = false;
                lnkDeductFunds.Visible = false;
                    
            }
        }

        //Reset all the fields and rebind data
        private void _ResetAll()
        {
            txtSearch.Text = string.Empty;
            _InitializeRepeater();
            upSearchResults.Update();
            _CloseItemPopup();
            PopupCtrl1.Hide();
        }

        //Changint the direction of sort
        private void _SwitchOrderByDirection()
        {
            if (OrderByDirection == Core.NPSCommon.Enums.OrderByDirection.Ascending)
            {
                OrderByDirection = Core.NPSCommon.Enums.OrderByDirection.Descending;
            }
            else
            {
                OrderByDirection = Core.NPSCommon.Enums.OrderByDirection.Ascending;
            }
        }

        private void _SetMessage()
        {
            string successMessage = UIHelper.GetSuccessMessage();
            string errorMessage = UIHelper.GetErrorMessage();
            if (!String.IsNullOrEmpty(successMessage))
            {
                litSuccessMessage.Visible = true;
                litSuccessMessage.Text = successMessage;
            }
            else if (!String.IsNullOrEmpty(errorMessage))
            {
                litErrorMessage.Visible = true;
                litErrorMessage.Text = errorMessage;
            }
            else
            {
                litErrorMessage.Visible = false;
                litSuccessMessage.Visible = false;
            }
            upMessage.Update();
        }
        #region Show and Hide Popup
        private void _ShowItemPopup(BudgetManagementItemCtrlMode budgetManagementItemCtrlMode, Budget objBudget,string strCreateMode)
        {
            PopupCtrl1.Visible = false;
            BudgetManagementItemCtrl1.Mode = budgetManagementItemCtrlMode;
            BudgetManagementItemCtrl1.InitializeFields(objBudget,strCreateMode);
            upItemPopup.Update();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowPopup", "ShowPopup('" + BudgetManagementItemCtrl1.ItemPopupClientID + "');", true);
        }
        private void _CloseItemPopup()
        {

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClosePopup", "ClosePopup('" + BudgetManagementItemCtrl1.ItemPopupClientID + "');", true);
        }
        #endregion
        #endregion

    }
}