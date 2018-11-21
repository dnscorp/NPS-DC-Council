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
using PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Common;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using PRIFACT.PRIFACTBase.MiscHelpers;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.SiteAdministration
{
    public partial class StaffManagementCtrl : System.Web.UI.UserControl
    {

        #region Private Properties
        //Properties for sorting in the repeater
        private StaffSortFields SortField
        {
            get
            {
                return EnumHelper.ParseEnum<StaffSortFields>(hfSortField.Value);
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
            _ShowItemPopup(StaffManagementItemCtrlMode.Create, null);
        }
        #endregion

        #region Repeater Events
        protected void lnkEdit_ServerClick(object sender, EventArgs e)
        {
            HtmlAnchor lnkEdit = (HtmlAnchor)sender;
            long lStaffId = Convert.ToInt64(lnkEdit.Attributes["StaffId"]);
            Staff objStaff = Staff.GetByStaffID(lStaffId);
            _ShowItemPopup(StaffManagementItemCtrlMode.Edit, objStaff);
        }

        protected void lnkDelete_ServerClick(object sender, EventArgs e)
        {
            HtmlAnchor lnkDelete = (HtmlAnchor)sender;
            PopupCtrl1.SetProperties(Convert.ToInt64(lnkDelete.Attributes["StaffId"]).ToString(), true, "Delete", "Cancel", "Confirm Delete", "Are you sure you want to Delete the selected Staff?", "DeleteUser");
            PopupCtrl1.Show();
        }

        protected void rptrResult_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {

                Literal litFirstname = e.Item.FindControl("litFirstname") as Literal;
                Literal litLastname = e.Item.FindControl("litLastname") as Literal;
                Literal litOfficename = e.Item.FindControl("litOfficename") as Literal;
                Literal litHasStaffLevelExpenditures = e.Item.FindControl("litHasStaffLevelExpenditures") as Literal;
                Literal litCreatedDate = e.Item.FindControl("litCreatedDate") as Literal;
                Literal litUpdatedDate = e.Item.FindControl("litUpdatedDate") as Literal;
                Literal litActiveFrom = e.Item.FindControl("litActiveFrom") as Literal;
                Literal litActiveTo = e.Item.FindControl("litActiveTo") as Literal;
                Literal litWirelessNumber = e.Item.FindControl("litWirelessNumber") as Literal;

                HtmlAnchor lnkEdit = e.Item.FindControl("lnkEdit") as HtmlAnchor;
                HtmlAnchor lnkDelete = e.Item.FindControl("lnkDelete") as HtmlAnchor;

                Staff objStaff = e.Item.DataItem as Staff;

                litFirstname.Text = objStaff.FirstName;
                litLastname.Text = objStaff.LastName;
                litOfficename.Text = objStaff.Office.Name;
                if (objStaff.HasStaffLevelExpenditures == true)
                    litHasStaffLevelExpenditures.Text = "Yes";
                else if (objStaff.HasStaffLevelExpenditures == false)
                    litHasStaffLevelExpenditures.Text = "No";
                litWirelessNumber.Text = objStaff.WirelessNumber;
                litActiveFrom.Text = UIHelper.GetDateTimeInDefaultFormat(objStaff.ActiveFrom);
                if(objStaff.ActiveTo.HasValue)
                    litActiveTo.Text = UIHelper.GetDateTimeInDefaultFormat(objStaff.ActiveTo.Value);
                litCreatedDate.Text = UIHelper.GetDateTimeInDefaultFormat(objStaff.CreatedDate);
                litUpdatedDate.Text = UIHelper.GetDateTimeInDefaultFormat(objStaff.UpdatedDate);
                lnkEdit.Attributes.Add("StaffId", objStaff.StaffId.ToString());
                lnkDelete.Attributes.Add("StaffId", objStaff.StaffId.ToString());
            }
        }
        #endregion

        #region Item Popup Events
        //Once the data is updated inside the child item popup, the main management control needs to be refreshed
        protected void StaffManagementItemCtrl1_SubmitClick(object sender, EventArgs e)
        {
            //Things to be done after Submit is clicked in the child item popup
            _ResetAll();
        }

        protected void StaffManagementItemCtrl1_CancelClick(object sender, EventArgs e)
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
            StaffSortFields selectedSortField = EnumHelper.ParseEnum<StaffSortFields>(e.CommandName);
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
            long lStaffId = Convert.ToInt64(popUpCtrl.IdToProcess);
            if (popUpCtrl.Mode.Equals("DeleteUser"))
            {
                Staff objStaff = Staff.GetByStaffID(lStaffId);
                if (StaffLevelExpenditure.GetAllByStaffId(objStaff.StaffId).Count > 0)
                {
                    UIHelper.SetErrorMessage("Unable to delete Staff as there are Staff Level Expenditures under this Staff");
                }
                else
                {
                    objStaff.IsDeleted = true;
                    objStaff.Update();
                    UIHelper.SetSuccessMessage("Staff deleted successfully");
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
            _BindData( txtSearch.Text.Trim(), PagerCtrl1.PageSize, 1, SortField, OrderByDirection);
        }

        //Bind the data to the repeater based on the page selected and sort order
        private void _BindData(string strSearchText, int? iPageSize, int? iPageNumber, StaffSortFields sortField, OrderByDirection orderByDirection)
        {
            ResultInfo objResultInfo = null;
            if (string.IsNullOrEmpty(strSearchText))
            {
                objResultInfo = Staff.GetAllByOfficeId(Office.OfficeID, false, iPageSize, iPageNumber, sortField, orderByDirection);
            }
            else
            {
                objResultInfo = Staff.Search(strSearchText,Office.OfficeID, iPageSize, iPageNumber, sortField, orderByDirection);
            }
            if (objResultInfo != null)
            {
                if (objResultInfo.Items.Count > 0)
                {
                    PagerCtrl1.Visible = true;
                    rptrResult.Visible = true;
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
        private void _ShowItemPopup(StaffManagementItemCtrlMode staffManagementItemCtrlMode, Staff objStaff)
        {
            PopupCtrl1.Visible = false;
            StaffManagementItemCtrl1.Mode = staffManagementItemCtrlMode;
            StaffManagementItemCtrl1.InitializeFields(objStaff);
            upItemPopup.Update();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowPopup", "ShowPopup('" + StaffManagementItemCtrl1.ItemPopupClientID + "');", true);
        }
        private void _CloseItemPopup()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClosePopup", "ClosePopup('" + StaffManagementItemCtrl1.ItemPopupClientID + "');", true);
        }
        #endregion
        #endregion
    }
}