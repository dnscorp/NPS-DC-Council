using System;
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
    public partial class ExpenditureCategoryManagementCtrl : System.Web.UI.UserControl
    {
        #region Private Properties
        //Properties for sorting in the repeater
        private ExpenditureCategorySortFields SortField
        {
            get
            {
                return EnumHelper.ParseEnum<ExpenditureCategorySortFields>(hfSortField.Value);
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
            _ShowItemPopup(ExpenditureCategoryManagementItemCtrlMode.Create, null);
        }
        #endregion

        #region Repeater Events

        protected void lnkEdit_ServerClick(object sender, EventArgs e)
        {
            HtmlAnchor lnkEdit = (HtmlAnchor)sender;
            long lExpenditureCategoryId = Convert.ToInt64(lnkEdit.Attributes["ExpenditureCategoryId"]);
            ExpenditureCategory objExpenditureCategory = ExpenditureCategory.GetByID(lExpenditureCategoryId);
            _ShowItemPopup(ExpenditureCategoryManagementItemCtrlMode.Edit, objExpenditureCategory);
        }

        protected void lnkEditAttributes_ServerClick(object sender, EventArgs e)
        {
            HtmlAnchor lnkEditAttributes = (HtmlAnchor)sender;
            long lExpenditureCategoryId = Convert.ToInt64(lnkEditAttributes.Attributes["ExpenditureCategoryId"]);
            ExpenditureCategory objExpenditureCategory = ExpenditureCategory.GetByID(lExpenditureCategoryId);
            _ShowItemPopup(ExpenditureCategoryManagementItemCtrlMode.EditAttributes, objExpenditureCategory);
        }

        protected void lnkDelete_ServerClick(object sender, EventArgs e)
        {
            HtmlAnchor lnkDelete = (HtmlAnchor)sender;
            PopupCtrl1.SetProperties(lnkDelete.Attributes["ExpenditureCategoryId"], true, "Delete", "Cancel", "Confirm Delete", "Are you sure you want to Delete the selected Category?", "DeleteCategory");
            PopupCtrl1.Show();
        }

        protected void rptrResult_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Literal litCategoryName = e.Item.FindControl("litCategoryName") as Literal;
                Literal litCategoryCode = e.Item.FindControl("litCategoryCode") as Literal;
                Literal litIsFixed = e.Item.FindControl("litIsFixed") as Literal;
                Literal litIsStaffLevel = e.Item.FindControl("litIsStaffLevel") as Literal;
                Literal litIsVendorStaff = e.Item.FindControl("litIsVendorStaff") as Literal;
                Literal litIsMonthly = e.Item.FindControl("litIsMonthly") as Literal;
                Literal litIsSystemDefined = e.Item.FindControl("litIsSystemDefined") as Literal;
                Literal litAppendMonth = e.Item.FindControl("litAppendMonth") as Literal;                
                Literal litIsActive = e.Item.FindControl("litIsActive") as Literal;
                Literal litCreatedDate = e.Item.FindControl("litCreatedDate") as Literal;
                Literal litUpdatedDate = e.Item.FindControl("litUpdatedDate") as Literal;


                HtmlAnchor lnkEdit = e.Item.FindControl("lnkEdit") as HtmlAnchor;
                HtmlAnchor lnkEditAttributes = e.Item.FindControl("lnkEditAttributes") as HtmlAnchor;
                HtmlAnchor lnkDelete = e.Item.FindControl("lnkDelete") as HtmlAnchor;

                ExpenditureCategory objExpenditureCategory = e.Item.DataItem as ExpenditureCategory;
                litCategoryName.Text = objExpenditureCategory.Name;
                litCategoryCode.Text = objExpenditureCategory.Code;
                litIsFixed.Text = (objExpenditureCategory.IsFixed == true) ? "Yes" : "No";
                litIsStaffLevel.Text = (objExpenditureCategory.IsStaffLevel == true) ? "Yes" : "No";
                litIsVendorStaff.Text = (objExpenditureCategory.IsVendorStaff == true) ? "Yes" : "No";
                litIsMonthly.Text = (objExpenditureCategory.IsMonthly == true) ? "Yes" : "No";
                litIsSystemDefined.Text = (objExpenditureCategory.IsSystemDefined == true) ? "Yes" : "No";
                litAppendMonth.Text = (objExpenditureCategory.AppendMonth == true) ? "Yes" : "No";                
                litIsActive.Text = (objExpenditureCategory.IsActive == true) ? "Yes" : "No";
                litCreatedDate.Text = UIHelper.GetDateTimeInDefaultFormat(objExpenditureCategory.CreatedDate);
                litUpdatedDate.Text = UIHelper.GetDateTimeInDefaultFormat(objExpenditureCategory.UpdatedDate);

                lnkEdit.Attributes.Add("ExpenditureCategoryId", objExpenditureCategory.ExpenditureCategoryID.ToString());
                lnkEditAttributes.Attributes.Add("ExpenditureCategoryId", objExpenditureCategory.ExpenditureCategoryID.ToString());
                lnkDelete.Attributes.Add("ExpenditureCategoryId", objExpenditureCategory.ExpenditureCategoryID.ToString());

                if (objExpenditureCategory.IsSystemDefined)
                {
                    lnkDelete.Visible = false;
                }
            }
        }
        #endregion

        #region Confirmation Popup Events
        protected void PopupCtrl1_OkayButtonClick(object sender, EventArgs e)
        {
            PopupCtrl popUpCtrl = (PopupCtrl)sender;
            long lExpenditureCategoryId = Convert.ToInt64(popUpCtrl.IdToProcess);
            if (popUpCtrl.Mode.Equals("DeleteCategory"))
            {
                ExpenditureCategory objCategory = ExpenditureCategory.GetByID(lExpenditureCategoryId);
                objCategory.IsDeleted = true;
                objCategory.Update();
                UIHelper.SetSuccessMessage("Category deleted successfully");
            }
            _ResetAll();
        }

        protected void PopupCtrl1_CancelButtonClick(object sender, EventArgs e)
        {
            //Raised when cancel is clicked on the confirmation popup
            _CloseItemPopup();
        }

        #endregion

        #region Paging and Sorting Events
        ////Binding the data based on page number
        protected void PagerCtrl1_BindMainRepeater(object sender, EventArgs e)
        {
            _BindData(txtSearch.Text.Trim(), PagerCtrl1.PageSize, PagerCtrl1.CurrentPage, SortField, OrderByDirection);
        }

        ////Sorting done here
        protected void rptrResult_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ExpenditureCategorySortFields selectedSortField = EnumHelper.ParseEnum<ExpenditureCategorySortFields>(e.CommandName);
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
        protected void ExpenditureCategoryManagementItemCtrl1_SubmitClick(object sender, EventArgs e)
        {
            //Things to be done after Submit is clicked in the child item popup
            _ResetAll();
        }

        protected void ExpenditureCategoryManagementItemCtrl1_CancelClick(object sender, EventArgs e)
        {
            //Things to be done after Cance is clicked in the child item popup
            _CloseItemPopup();
        }

        //Once the data is updated inside the child item popup, the main management control needs to be refreshed
        protected void ExpenditureCategoryAttributeCtrl1_SubmitClick(object sender, EventArgs e)
        {
            //Things to be done after Submit is clicked in the child item popup
            _ResetAll();
        }

        protected void ExpenditureCategoryAttributeCtrl1_CancelClick(object sender, EventArgs e)
        {
            //Things to be done after Cance is clicked in the child item popup
            _CloseItemPopup();
        }
        #endregion

        #region Private Methods

        ////Binding the data to repeater initially
        private void _InitializeRepeater()
        {
            //PagerCtrl1.CurrentPageIndex = 0;
            _BindData(txtSearch.Text.Trim(), PagerCtrl1.PageSize, 1, SortField, OrderByDirection);
        }

        // Bind the data to the repeater based on the page selected and sort order
        private void _BindData(string strSearchText, int? iPageSize, int? iPageNumber, ExpenditureCategorySortFields sortField, OrderByDirection orderByDirection)
        {
            ResultInfo objResultInfo = null;

            objResultInfo = ExpenditureCategory.GetAll(strSearchText,iPageSize, iPageNumber, sortField, orderByDirection);

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
            _CloseItemPopup();
            PopupCtrl1.Hide();
            txtSearch.Text = string.Empty;
            _InitializeRepeater();
            upSearchResults.Update();
            
        }

        //Changint the direction of sort
        private void _SwitchOrderByDirection()
        {
            if (OrderByDirection == Core.NPSCommon.Enums.OrderByDirection.Ascending)
            {
                OrderByDirection = Core.NPSCommon.Enums.OrderByDirection.Descending;
            }
            else if (OrderByDirection == Core.NPSCommon.Enums.OrderByDirection.Descending)
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
        #region Show and Hide Popup
        private void _ShowItemPopup(ExpenditureCategoryManagementItemCtrlMode ExpenditureCategoryCtrlMode, ExpenditureCategory category)
        {
            if (ExpenditureCategoryCtrlMode == ExpenditureCategoryManagementItemCtrlMode.EditAttributes)
            {
                ExpenditureCategoryAttributeCtrl1.InitializeFields(category);
                ExpenditureCategoryAttributeCtrl1.Mode = ExpenditureCategoryAttributeManagementCtrlMode.Edit;
                upItemPopup2.Update();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowPopup", "ShowPopup('" + ExpenditureCategoryAttributeCtrl1.ItemPopupClientID + "');", true);
            }
            else if (ExpenditureCategoryCtrlMode == ExpenditureCategoryManagementItemCtrlMode.Edit || ExpenditureCategoryCtrlMode == ExpenditureCategoryManagementItemCtrlMode.Create)
            {
                PopupCtrl1.Visible = false;
                ExpenditureCategoryManagementItemCtrl1.Mode = ExpenditureCategoryCtrlMode;
                ExpenditureCategoryManagementItemCtrl1.InitializeFields(category);
                upItemPopup.Update();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowPopup", "ShowPopup('" + ExpenditureCategoryManagementItemCtrl1.ItemPopupClientID + "');", true);
            }
        }
        private void _CloseItemPopup()
        {
            ExpenditureCategoryAttributeCtrl1.Mode = ExpenditureCategoryAttributeManagementCtrlMode.NotSet;
            ExpenditureCategoryManagementItemCtrl1.Mode = ExpenditureCategoryManagementItemCtrlMode.NotSet;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClosePopup", "ClosePopup('" + ExpenditureCategoryAttributeCtrl1.ItemPopupClientID + "');", true);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClosePopup", "ClosePopup('" + ExpenditureCategoryManagementItemCtrl1.ItemPopupClientID + "');", true);
        }


        #endregion
        #endregion

    }
}