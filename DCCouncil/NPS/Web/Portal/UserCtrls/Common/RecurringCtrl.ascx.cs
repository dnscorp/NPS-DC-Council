using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.SortFields;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.UserCtrlModes;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Common;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using PRIFACT.PRIFACTBase.MiscHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls
{
    public partial class RecurringCtrl : System.Web.UI.UserControl
    {

        #region Private Properties
        //Properties for sorting in the repeater
        private RecurringSortFields SortField
        {
            get
            {
                return EnumHelper.ParseEnum<RecurringSortFields>(hfSortField.Value);
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
        #region Public Properties

        public FiscalYear FiscalYearSelected
        {
            get
            {
                return NPSRequestContext.GetContext().FiscalYearSelected;
            }
        }

        #endregion
        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hfSortField.Value = RecurringSortFields.VendorName.ToString();
                hfOrderByDirection.Value = OrderByDirection.Ascending.ToString();
            }
            //The data needs to be binded here since the viewstate is disabled and we need the posted values to determine the data to be fetched.
            _BindData(txtSearch.Text.Trim(), PagerCtrl1.PageSize, PagerCtrl1.CurrentPage, SortField, OrderByDirection);
            //RecurringItemCtrl1.ExpenditureCategory = ExpenditureCategory;
            txtSearch.Attributes.Add("onchange", string.Format("javascript:doButtonClick('{0}');", bttn.ClientID));
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            //Setting the success message if present in Immediate Session
            _SetMessage();
            _SetUI();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _LoadOffices();
        }

        private void _SetUI()
        {
            txtSearch.Attributes.Add("Placeholder", "Search");
            litCreateLink.Text = "Create/Update";//ExpenditureCategory.GetAttribute("CreateNewLinkText");
            litPageHeading.Text = "Recurring Transactions";
        }

        #endregion

        #region Search Header Events
        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //No need to do anything here since the data is bound on PageLoad            
        }

        protected void lnkCreate_ServerClick(object sender, EventArgs e)
        {
            _ShowItemPopup(RecurringItemCtrlMode.Create, null);
        }

        #endregion
        #region Repeater Events
        protected void lnkEdit_ServerClick(object sender, EventArgs e)
        {
            HtmlAnchor lnkEdit = (HtmlAnchor)sender;
            long lRecurringId = Convert.ToInt64(lnkEdit.Attributes["RecurringId"]);
            RecurringTransactions objRecurring = RecurringTransactions.GetByRecurringID(lRecurringId);
            _ShowItemPopup(RecurringItemCtrlMode.Edit, objRecurring);
        }

        protected void lnkDelete_ServerClick(object sender, EventArgs e)
        {
            HtmlAnchor lnkDelete = (HtmlAnchor)sender;
            PopupCtrl1.SetProperties(Convert.ToInt64(lnkDelete.Attributes["RecurringId"]).ToString(), true, "Delete", "Cancel", "Confirm Delete", "Are you sure you want to Delete the selected recurring?", "DeleteRecurring");
            PopupCtrl1.Show();
        }

        protected void rptrResult_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                LinkButton lBtnVendorName = e.Item.FindControl("lBtnVendorName") as LinkButton;
                LinkButton lBtnDescription = e.Item.FindControl("lBtnDescription") as LinkButton;
                LinkButton lBtnOfficeName = e.Item.FindControl("lBtnOfficeName") as LinkButton;

                lBtnOfficeName.Visible = true;
                lBtnVendorName.Visible = true;
                lBtnDescription.Visible = true;
            }

            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Literal litVendorName = e.Item.FindControl("litVendorName") as Literal;
                Literal litCategory = e.Item.FindControl("litCategory") as Literal;
                Literal litDescription = e.Item.FindControl("litDescription") as Literal;
                Literal litAmount = e.Item.FindControl("litAmount") as Literal;
                Literal litComments = e.Item.FindControl("litComments") as Literal;
                Literal litOfficeName = e.Item.FindControl("litOfficeName") as Literal;

                RecurringTransactions objRecurring = e.Item.DataItem as RecurringTransactions;
                litVendorName.Text = objRecurring.VendorName;
                litCategory.Text = objRecurring.RecurringCategory;
                litDescription.Text = objRecurring.Description;
                litAmount.Text = UIHelper.GetAmountInDefaultFormat(objRecurring.Amount);
                litComments.Text = objRecurring.Comments;
                HtmlAnchor lnkEdit = e.Item.FindControl("lnkEdit") as HtmlAnchor;
                HtmlAnchor lnkDelete = e.Item.FindControl("lnkDelete") as HtmlAnchor;
                lnkEdit.Attributes.Add("RecurringID", objRecurring.RecurringID.ToString());
                lnkDelete.Attributes.Add("RecurringID", objRecurring.RecurringID.ToString());

                litOfficeName.Text = objRecurring.Office.Name;
                litOfficeName.Visible = true;
                litVendorName.Visible = true;
                litDescription.Visible = true;
            }
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
            RecurringSortFields selectedSortField = EnumHelper.ParseEnum<RecurringSortFields>(e.CommandName);
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
            long lRecurringId = Convert.ToInt64(popUpCtrl.IdToProcess);
            if (popUpCtrl.Mode.Equals("DeleteRecurring"))
            {
                RecurringTransactions.Delete(lRecurringId);
                UIHelper.SetSuccessMessage("Recurring Transaction deleted successfully");
            }
            _ResetAll();
        }

        protected void PopupCtrl1_CancelButtonClick(object sender, EventArgs e)
        {
            //Raised when cancel is clicked on the confirmation popup
            PopupCtrl1.Hide();
        }
        #endregion

        #region Item Popup Events
        //TODO
        //Once the data is updated inside the child item popup, the main management control needs to be refreshed
        protected void RecurringItemCtrl1_SubmitClick(object sender, EventArgs e)
        {
            //Things to be done after Submit is clicked in the child item popup
            _ResetAll();
        }

        protected void RecurringItemCtrl1_CancelClick(object sender, EventArgs e)
        {
            //Things to be done after Cance is clicked in the child item popup
            _CloseItemPopup();
        }
        #endregion

        

        #region Private Methods

        //Binding the data to repeater initially
        private void _InitializeRepeater()
        {
            PagerCtrl1.CurrentPageIndex = 0;
            _BindData(txtSearch.Text.Trim(), PagerCtrl1.PageSize, 1, SortField, OrderByDirection);
        }

        //Bind the data to the repeater based on the page selected and sort order
        private void _BindData(string strSearchText, int? iPageSize, int? iPageNumber, RecurringSortFields sortField, OrderByDirection orderByDirection)
        {
            ResultInfo objResultInfo = null;
            string RecurringCategory = "";
            if (ddlOfficeFilter.SelectedValue != "0")
            {
                objResultInfo = RecurringTransactions.GetAll(strSearchText, Convert.ToInt64(ddlOfficeFilter.SelectedValue), FiscalYearSelected.FiscalYearID, RecurringCategory, null, iPageSize, iPageNumber, sortField, orderByDirection);
            }
            else
                objResultInfo = RecurringTransactions.GetAll(strSearchText, null, FiscalYearSelected.FiscalYearID, RecurringCategory, null, iPageSize, iPageNumber, sortField, orderByDirection);

            if (objResultInfo != null)
            {
                if (objResultInfo.Items.Count > 0)
                {
                    PagerCtrl1.Visible = true;
                    //rptrResult.Visible = true;
                    litNoResults.Visible = false;
                    //Initilizing the pager control
                    PagerCtrl1.SetPager(objResultInfo.RowCount);
                }
                else
                {
                    PagerCtrl1.Visible = false;
                    litNoResults.Visible = true;
                    litNoResults.Text = "No results found.";
                }
                //Binding the data
                rptrResult.DataSource = objResultInfo.Items;
                rptrResult.DataBind();
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
            string message = UIHelper.GetSuccessMessage();
            if (!String.IsNullOrEmpty(message))
            {
                litSuccessMessage.Visible = true;
                litSuccessMessage.Text = message;
            }
            else
            {
                litSuccessMessage.Visible = false;
            }
            upMessage.Update();
        }

        private void _LoadOffices()
        {
            ddlOfficeFilter.Items.Clear();
            List<Office> lstOffices = Office.GetAll(string.Empty, NPSRequestContext.GetContext().FiscalYearSelected.FiscalYearID, -1, null, OfficeSortFields.Name, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items.ConvertAll(q => (Office)q);

            foreach (Office objOffice in lstOffices)
            {
                ListItem lstItem = new ListItem(objOffice.Name, objOffice.OfficeID.ToString());
                ddlOfficeFilter.Items.Add(lstItem);
            }
            ListItem lstItemAll = new ListItem("All", "0");
            ddlOfficeFilter.Items.Insert(0, lstItemAll);
        }

        #region Show and Hide Popup
        private void _ShowItemPopup(RecurringItemCtrlMode recurringItemCtrlMode, RecurringTransactions objRecurring)
        {
            phSearchBar.Visible = false;
            //upSearchBar.Update();
            PopupCtrl1.Visible = false;
            RecurringItemCtrl1.Mode = recurringItemCtrlMode;
            RecurringItemCtrl1.InitializeFields(objRecurring);
            upItemPopup.Update();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowPopup", "ShowPopup('" + RecurringItemCtrl1.ItemPopupClientID + "');", true);
        }
        private void _CloseItemPopup()
        {
            phSearchBar.Visible = true;
            //upSearchBar.Update();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClosePopup", "ClosePopup('" + RecurringItemCtrl1.ItemPopupClientID + "');", true);
        }
        #endregion

        #endregion
    }
}