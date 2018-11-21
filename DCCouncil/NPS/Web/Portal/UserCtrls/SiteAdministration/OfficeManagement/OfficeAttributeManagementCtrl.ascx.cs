using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.UserCtrlModes;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.SiteAdministration.OfficeManagement
{
    public partial class OfficeAttributeManagementCtrl : System.Web.UI.UserControl
    {
        #region Public Events
        public event EventHandler SubmitClick;
        public event EventHandler CancelClick;
        #endregion

        #region Public Properties
        public OfficeAttributeManagementCtrlMode Mode
        {
            get
            {
                if (string.IsNullOrEmpty(hfMode.Value))
                {
                    return OfficeAttributeManagementCtrlMode.NotSet;
                }
                else
                {
                    return (OfficeAttributeManagementCtrlMode)Convert.ToInt32(hfMode.Value);
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
        #endregion

        #region Public Setter and Getter Methods
        public void InitializeFields(Office objOffice)
        {
            if (objOffice != null)
            {
                hfOfficeId.Value = objOffice.OfficeID.ToString();
                rptrOfficeAttribute.DataSource = Office.GetByOfficeID(objOffice.OfficeID).Attributes();
                rptrOfficeAttribute.DataBind();
            }
        }
        #endregion

        #region Private Methods
        private void _SetUI()
        {
            if (Mode == OfficeAttributeManagementCtrlMode.Edit)
            {
                _ShowPopup();
                lnkSubmit.InnerText = "Update";
            }
            if (Mode == OfficeAttributeManagementCtrlMode.NotSet)
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
            Page.Validate("ValGroup2");
            if (!Page.IsValid)
            {
                _ShowPopup();
                return;
            }

            if (Mode == OfficeAttributeManagementCtrlMode.Edit)
            {
                //Updating the attributes

                long officeID = Convert.ToInt64(hfOfficeId.Value);
                Office objOffice = Office.GetByOfficeID(officeID);
                List<OfficeAttribute> listOfficeAttributes = objOffice.Attributes();

                foreach(RepeaterItem item in rptrOfficeAttribute.Items)
                {
                    if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
                   {
                        TextBox txtAttributeValue = item.FindControl("txtAttributeValue") as TextBox;
                        HiddenField hfofficeAttributeLookUpID = item.FindControl("hfofficeAttributeLookUpID") as HiddenField;
                        long officeAttributeLookUpID = Convert.ToInt64(hfofficeAttributeLookUpID.Value);
                        objOffice.UpdateAttribute(officeAttributeLookUpID, txtAttributeValue.Text);
                   }
                }
                UIHelper.SetSuccessMessage("Category updated successfully");
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
            Mode = OfficeAttributeManagementCtrlMode.NotSet;
            _HidePopup();

            //Raising the CancelClick event
            if (CancelClick != null)
            {
                CancelClick(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Custom Validator Validation Events

        protected void txtAttributeValueVal_ServerValidate(object source, ServerValidateEventArgs args)
        {
            foreach (RepeaterItem item in rptrOfficeAttribute.Items)
            {
                if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
                {
                    TextBox txtAttributeValue = item.FindControl("txtAttributeValue") as TextBox;
                    if (String.IsNullOrEmpty(txtAttributeValue.Text.Trim()))
                    {
                        args.IsValid = false;
                        litErrorMessage.Visible = true;
                        litErrorMessage.Text = "All fields are required";
                    }
                }
            }

        }

        #endregion

        protected void rptrOfficeAttribute_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Label lblAttributeName = e.Item.FindControl("lblAttributeName") as Label;
                TextBox txtAttributeValue = e.Item.FindControl("txtAttributeValue") as TextBox;
                HiddenField hfofficeAttributeLookUpID = e.Item.FindControl("hfofficeAttributeLookUpID") as HiddenField;

                OfficeAttribute objOfficeAttribute = e.Item.DataItem as OfficeAttribute;

                hfofficeAttributeLookUpID.Value = objOfficeAttribute.OfficeAttributeLookupID.ToString();
                lblAttributeName.Text = objOfficeAttribute.OfficeAttributeLookup.Description;
                txtAttributeValue.Text = objOfficeAttribute.AttributeValue;
            }
        }
    }
}