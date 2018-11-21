using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.SortFields;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Common;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using PRIFACT.PRIFACTBase.MiscHelpers;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.SiteAdministration
{
    public partial class VendorManagementCtrl : System.Web.UI.UserControl
    {
        #region Private Properties
        //Properties for sorting in the repeater
        private VendorSortFields SortField
        {
            get
            {
                return EnumHelper.ParseEnum<VendorSortFields>(hfSortField.Value);
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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _BindData(txtSearch.Text.Trim(), null, PagerCtrl1.PageSize, PagerCtrl1.CurrentPage, SortField, OrderByDirection);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //The data needs to be binded here since the viewstate is disabled and we need the posted values to determine the data to be fetched.
            _SetUI();
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

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            _BindData(txtSearch.Text.Trim(), null, PagerCtrl1.PageSize, PagerCtrl1.CurrentPage, SortField, OrderByDirection);
            //No need to do anything here since the data is bound on PageLoad            
        }

        protected void rptrResult_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Literal litOfficeName = e.Item.FindControl("litOfficeName") as Literal;
                Literal litName = e.Item.FindControl("litName") as Literal;
                CheckBox chkIsRolledUp = e.Item.FindControl("chkIsRolledUp") as CheckBox;
                Literal litCreatedDate = e.Item.FindControl("litCreatedDate") as Literal;
                Literal litUpdatedDate = e.Item.FindControl("litUpdatedDate") as Literal;
                HtmlAnchor lnkDelete = e.Item.FindControl("lnkDelete") as HtmlAnchor;

                Vendor objVendor = e.Item.DataItem as Vendor;
                litOfficeName.Text = objVendor.Office.Name;
                litName.Text = objVendor.Name;
                chkIsRolledUp.Checked = objVendor.IsRolledUp;
                chkIsRolledUp.Attributes.Add("VendorID", objVendor.VendorID.ToString());
                litCreatedDate.Text = UIHelper.GetDateTimeInDefaultFormat(objVendor.CreatedDate);
                litUpdatedDate.Text = UIHelper.GetDateTimeInDefaultFormat(objVendor.UpdatedDate);
                lnkDelete.Attributes.Add("VendorID", objVendor.VendorID.ToString());
            }
        }
        protected void chkIsRolledUp_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkIsRolledUp = (CheckBox)sender;
            long lVendorId = Convert.ToInt64(chkIsRolledUp.Attributes["VendorID"]);
            Vendor objVendor = Vendor.GetByID(lVendorId);
            objVendor.IsRolledUp = chkIsRolledUp.Checked;
            objVendor.Update();
            _InitializeRepeater();
            UIHelper.SetSuccessMessage("Vendor updated successfully");
        }
        //Binding the data based on page number
        protected void PagerCtrl1_BindMainRepeater(object sender, EventArgs e)
        {
            _BindData(txtSearch.Text.Trim(), null, PagerCtrl1.PageSize, PagerCtrl1.CurrentPage, SortField, OrderByDirection);
        }

        //Sorting done here
        protected void rptrResult_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            VendorSortFields selectedSortField = EnumHelper.ParseEnum<VendorSortFields>(e.CommandName);
            if (selectedSortField == SortField)
            {
                _SwitchOrderByDirection();
                _BindData(txtSearch.Text.Trim(), null, PagerCtrl1.PageSize, PagerCtrl1.CurrentPage, SortField, OrderByDirection);
            }
            else
            {
                SortField = selectedSortField;
                OrderByDirection = Core.NPSCommon.Enums.OrderByDirection.Ascending;
                _BindData(txtSearch.Text.Trim(), null, PagerCtrl1.PageSize, PagerCtrl1.CurrentPage, SortField, OrderByDirection);
            }
        }

        //Binding the data to repeater initially
        private void _InitializeRepeater()
        {
            PagerCtrl1.CurrentPageIndex = 0;
            _BindData(txtSearch.Text.Trim(), null, PagerCtrl1.PageSize, 1, SortField, OrderByDirection);
        }

        //Bind the data to the repeater based on the page selected and sort order
        private void _BindData(string strSearchText, long? lOfficeId, int? iPageSize, int? iPageNumber, VendorSortFields sortField, OrderByDirection orderByDirection)
        {
            ResultInfo objResultInfo = null;

            objResultInfo = Vendor.GetAll(strSearchText, null, iPageSize, iPageNumber, sortField, orderByDirection);

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
        protected void lnkDelete_ServerClick(object sender, EventArgs e)
        {
            HtmlAnchor lnkDelete = (HtmlAnchor)sender;
            PopupCtrl1.SetProperties(Convert.ToInt64(lnkDelete.Attributes["VendorID"]).ToString(), true, "Delete", "Cancel", "Confirm Delete", "Are you sure you want to Delete the selected Vendor?", "DeleteVendor");
            PopupCtrl1.Show();
        }
        #region Confirmation Popup Events
        protected void PopupCtrl1_OkayButtonClick(object sender, EventArgs e)
        {
            PopupCtrl popUpCtrl = (PopupCtrl)sender;
            long lVendorID = Convert.ToInt64(popUpCtrl.IdToProcess);
            if (popUpCtrl.Mode.Equals("DeleteVendor"))
            {
                Vendor objVendor = Vendor.GetByID(lVendorID);
                objVendor.IsDeleted = true;
                objVendor.Update();
                UIHelper.SetSuccessMessage("Vendor deleted successfully");
            }
            _ResetAll();
        }

        protected void PopupCtrl1_CancelButtonClick(object sender, EventArgs e)
        {
            //Raised when cancel is clicked on the confirmation popup
            PopupCtrl1.Hide();
        }
        #endregion
    }
}