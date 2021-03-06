﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.SortFields;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.UserCtrlModes;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Common;
using PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.SiteAdministration.OfficeManagement;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using PRIFACT.PRIFACTBase.MiscHelpers;
using PRIFACT.PRIFACTBase.UrlHelpers;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.SiteAdministration
{
    public partial class OfficeManagementCtrl : System.Web.UI.UserControl
    {
        #region Private Properties
        //Properties for sorting in the repeater
        private OfficeSortFields SortField
        {
            get
            {
                return EnumHelper.ParseEnum<OfficeSortFields>(hfSortField.Value);
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

        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            //The data needs to be binded here since the viewstate is disabled and we need the posted values to determine the data to be fetched.
            _SetUI();
            _BindData(txtSearch.Text.Trim(), PagerCtrl1.PageSize, PagerCtrl1.CurrentPage, SortField, OrderByDirection);
            txtSearch.Attributes.Add("onchange", string.Format("javascript:doButtonClick('{0}');", bttn.ClientID));
        }

        private void _SetUI()
        {
            txtSearch.Attributes.Add("placeholder", "Search");
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
            _ShowItemPopup(OfficeManagementItemCtrlMode.Create, null);
        }
        #endregion

        #region Repeater Events

        protected void lnkEdit_ServerClick(object sender, EventArgs e)
        {
            HtmlAnchor lnkEdit = (HtmlAnchor)sender;
            long lOfficeId = Convert.ToInt64(lnkEdit.Attributes["OfficeId"]);
            Office objOffice = Office.GetByOfficeID(lOfficeId);
            _ShowItemPopup(OfficeManagementItemCtrlMode.Edit, objOffice);
        }

        protected void lnkEditAttributes_ServerClick(object sender, EventArgs e)
        {
            HtmlAnchor lnkEditAttributes = (HtmlAnchor)sender;
            long lOfficeId = Convert.ToInt64(lnkEditAttributes.Attributes["OfficeId"]);
            Office objOffice = Office.GetByOfficeID(lOfficeId);
            _ShowItemPopup(OfficeManagementItemCtrlMode.EditAttributes, objOffice);
        }

        protected void lnkDelete_ServerClick(object sender, EventArgs e)
        {
            HtmlAnchor lnkDelete = (HtmlAnchor)sender;
            PopupCtrl1.SetProperties(Convert.ToInt64(lnkDelete.Attributes["OfficeId"]).ToString(), true, "Delete", "Cancel", "Confirm Delete", "Are you sure you want to Delete the selected Office?", "DeleteOffice");
            PopupCtrl1.Show();
        }

        protected void rptrResult_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Literal litOfficeName = e.Item.FindControl("litOfficeName") as Literal;
                Literal litActiveFrom = e.Item.FindControl("litActiveFrom") as Literal;
                Literal litActiveTo = e.Item.FindControl("litActiveTo") as Literal;
                Literal litPCA = e.Item.FindControl("litPCA") as Literal;
                Literal litPCATitle = e.Item.FindControl("litPCATitle") as Literal;
                Literal litIndexCode = e.Item.FindControl("litIndexCode") as Literal;
                Literal litIndexTitle = e.Item.FindControl("litIndexTitle") as Literal;
                Literal litCreatedDate = e.Item.FindControl("litCreatedDate") as Literal;
                Literal litUpdatedDate = e.Item.FindControl("litUpdatedDate") as Literal;
                Literal litCompCode = e.Item.FindControl("litCompCode") as Literal;

                HtmlAnchor lnkEdit = e.Item.FindControl("lnkEdit") as HtmlAnchor;
                HtmlAnchor lnkEditAttributes = e.Item.FindControl("lnkEditAttributes") as HtmlAnchor;
                HtmlAnchor lnkDelete = e.Item.FindControl("lnkDelete") as HtmlAnchor;
                HtmlAnchor lnkAddStaff = e.Item.FindControl("lnkAddStaff") as HtmlAnchor;
                HtmlAnchor lnkBudgetManagement = e.Item.FindControl("lnkBudgetManagement") as HtmlAnchor;                
                
                Office objOffice = e.Item.DataItem as Office;
                litOfficeName.Text = objOffice.Name;
                litActiveFrom.Text = objOffice.ActiveFrom.ToShortDateString();
                if (objOffice.ActiveTo.HasValue)
                {
                    litActiveTo.Text = objOffice.ActiveTo.Value.ToShortDateString();
                }
                litCompCode.Text = objOffice.CompCode;
                litPCA.Text = objOffice.PCA;
                litPCATitle.Text = objOffice.PCATitle;
                litIndexCode.Text = objOffice.IndexCode;
                litIndexTitle.Text = objOffice.IndexTitle;
                litCreatedDate.Text = UIHelper.GetDateTimeInDefaultFormat(objOffice.CreatedDate);
                litUpdatedDate.Text = UIHelper.GetDateTimeInDefaultFormat(objOffice.UpdatedDate);

                lnkEdit.Attributes.Add("OfficeId", objOffice.OfficeID.ToString());
                lnkEditAttributes.Attributes.Add("OfficeId", objOffice.OfficeID.ToString());
                lnkDelete.Attributes.Add("OfficeId", objOffice.OfficeID.ToString());
                lnkAddStaff.HRef = NPSUrls.StaffManagement + "?OfficeId=" + UrlUtility.SetStringOnQv(objOffice.OfficeID.ToString());
                lnkBudgetManagement.HRef = NPSUrls.BudgetManagement + "?OfficeId=" + UrlUtility.SetStringOnQv(objOffice.OfficeID.ToString());

            }
        }
        #endregion

        #region Confirmation Popup Events
        protected void PopupCtrl1_OkayButtonClick(object sender, EventArgs e)
        {
            PopupCtrl popUpCtrl = (PopupCtrl)sender;
            long lOfficeID = Convert.ToInt64(popUpCtrl.IdToProcess);
            if (popUpCtrl.Mode.Equals("DeleteOffice"))
            {
                Office objOffice = Office.GetByOfficeID(lOfficeID);

                //Check if office have any staff
                if (Staff.GetAllByOfficeId(objOffice.OfficeID, false, -1, null, StaffSortFields.FirstName, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items.Count > 0)
                {
                    UIHelper.SetErrorMessage("Unable to delete office as there are Staffs under it.");
                }

                //Check if Office have any expenditure entered
                else if (Expenditure.GetAll(String.Empty, String.Format("<officeids><officeid>{0}</officeid></officeids>", objOffice.OfficeID.ToString()), null, String.Empty, null, -1, null, ExpenditureSortFields.IndexCode, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items.Count > 0)
                {
                    UIHelper.SetErrorMessage("Unable to delete office as there are Expenditures added under it.");
                } 
                
                //Check if there are any purchase orders
                else if(PurchaseOrder.GetAll(String.Empty, objOffice.OfficeID, null, null, -1 , null, PurchaseOrderSortField.OfficeName, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items.Count > 0)
                {
                    UIHelper.SetErrorMessage("Unable to delete office as there are Purchase Orders under it.");
                }

                //Else delete Office
                else
                {
                    objOffice.IsDeleted = true;
                    objOffice.Update();
                    UIHelper.SetSuccessMessage("Office deleted successfully");
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

        #region Paging and Sorting Events
        //Binding the data based on page number
        protected void PagerCtrl1_BindMainRepeater(object sender, EventArgs e)
        {
            _BindData(txtSearch.Text.Trim(), PagerCtrl1.PageSize, PagerCtrl1.CurrentPage, SortField, OrderByDirection);
        }

        //Sorting done here
        protected void rptrResult_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            OfficeSortFields selectedSortField = EnumHelper.ParseEnum<OfficeSortFields>(e.CommandName);
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

        #region Item Popup Events
        //Once the data is updated inside the child item popup, the main management control needs to be refreshed
        protected void OfficeManagementItemCtrl1_SubmitClick(object sender, EventArgs e)
        {
            //Things to be done after Submit is clicked in the child item popup
            _ResetAll();
        }

        protected void OfficeManagementItemCtrl1_CancelClick(object sender, EventArgs e)
        {
            //Things to be done after Cancel is clicked in the child item popup
            _CloseItemPopup();
        }

        protected void OfficeAttributeManagementCtrl1_SubmitClick(object sender, EventArgs e)
        {
            //Things to be done after Submit is clicked in the child item popup
            _ResetAll();
        }

        protected void OfficeAttributeManagementCtrl1_CancelClick(object sender, EventArgs e)
        {
            //Things to be done after Cancel is clicked in the child item popup
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
        private void _BindData(string strSearchText, int? iPageSize, int? iPageNumber, OfficeSortFields sortField, OrderByDirection orderByDirection)
        {
            ResultInfo objResultInfo = null;

            objResultInfo = Office.GetAll(strSearchText,NPSRequestContext.GetContext().FiscalYearSelected.FiscalYearID,iPageSize, iPageNumber, sortField, orderByDirection);

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
        private void _ShowItemPopup(OfficeManagementItemCtrlMode OfficeManagementCtrlMode, Office objOffice)
        {

            if (OfficeManagementCtrlMode == OfficeManagementItemCtrlMode.EditAttributes)
            {
                PopupCtrl1.Visible = false;
                OfficeAttributeManagementCtrl1.Mode = OfficeAttributeManagementCtrlMode.Edit;
                OfficeAttributeManagementCtrl1.InitializeFields(objOffice);
                upItemPopup2.Update();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowPopup", "ShowPopup('" + OfficeAttributeManagementCtrl1.ItemPopupClientID + "');", true);
            }
            else if (OfficeManagementCtrlMode == OfficeManagementItemCtrlMode.Edit || OfficeManagementCtrlMode == OfficeManagementItemCtrlMode.Create)
            {
                PopupCtrl1.Visible = false;
                OfficeManagementItemCtrl1.Mode = OfficeManagementCtrlMode;
                OfficeManagementItemCtrl1.InitializeFields(objOffice);
                upItemPopup.Update();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowPopup", "ShowPopup('" + OfficeManagementItemCtrl1.ItemPopupClientID + "');", true);
            }
        }
        private void _CloseItemPopup()
        {
            OfficeManagementItemCtrl1.Mode = OfficeManagementItemCtrlMode.NotSet;
            OfficeAttributeManagementCtrl1.Mode = OfficeAttributeManagementCtrlMode.NotSet;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClosePopup", "ClosePopup('" + OfficeAttributeManagementCtrl1.ItemPopupClientID + "');", true);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClosePopup", "ClosePopup('" + OfficeManagementItemCtrl1.ItemPopupClientID + "');", true);
        }
        #endregion
        #endregion




    }
}