using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.UserCtrlModes;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.SiteAdministration.ExpenditureCategoryManagement
{
    public partial class ExpenditureCategoryAttributeManagementCtrl : System.Web.UI.UserControl
    {
        #region Public Events
        public event EventHandler SubmitClick;
        public event EventHandler CancelClick;
        #endregion

        #region Public Properties
        public ExpenditureCategoryAttributeManagementCtrlMode Mode
        {
            get
            {
                if (string.IsNullOrEmpty(hfMode.Value))
                {
                    return ExpenditureCategoryAttributeManagementCtrlMode.NotSet;
                }
                else
                {
                    return (ExpenditureCategoryAttributeManagementCtrlMode)Convert.ToInt32(hfMode.Value);
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
        public void InitializeFields(ExpenditureCategory objCategory)
        {
            if (objCategory != null)
            {
                hfCategoryId.Value = objCategory.ExpenditureCategoryID.ToString();
                rptrCategoryAttribute.DataSource = ExpenditureCategoryAttribute.GetByCategoryId(objCategory.ExpenditureCategoryID);
                rptrCategoryAttribute.DataBind();
            }
        }
        #endregion

        #region Private Methods
        private void _SetUI()
        {
            if (Mode == ExpenditureCategoryAttributeManagementCtrlMode.Edit)
            {
                _ShowPopup();
                lnkSubmit.InnerText = "Update";
            }
            if (Mode == ExpenditureCategoryAttributeManagementCtrlMode.NotSet)
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

            if (Mode == ExpenditureCategoryAttributeManagementCtrlMode.Edit)
            {
                //Updating the Category Attributes
                long expenditureCategoryId = Convert.ToInt64(hfCategoryId.Value);
                ExpenditureCategory objExpenditureCategory = ExpenditureCategory.GetByID(expenditureCategoryId);
                List<ExpenditureCategoryAttribute> listExpenditureCategoryAttribute = objExpenditureCategory.Attributes();

                foreach (RepeaterItem item in rptrCategoryAttribute.Items)
                {
                    if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
                    {
                        TextBox txtAttributeValue = item.FindControl("txtAttributeValue") as TextBox;
                        HiddenField hfExpenditureCategoryAttributeLookupId = item.FindControl("hfExpenditureCategoryAttributeLookupId") as HiddenField;
                        long expenditureCategoryAttributeLookUpID = Convert.ToInt64(hfExpenditureCategoryAttributeLookupId.Value);
                        objExpenditureCategory.UpdateAttribute(expenditureCategoryAttributeLookUpID, txtAttributeValue.Text);
                    }
                }
                UIHelper.SetSuccessMessage("Category Attributes Updated successfully");
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
            Mode = ExpenditureCategoryAttributeManagementCtrlMode.NotSet;
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
            foreach (RepeaterItem item in rptrCategoryAttribute.Items)
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


        protected void rptrCategoryAttribute_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Label lblAttributeName = e.Item.FindControl("lblAttributeName") as Label;
                TextBox txtAttributeValue = e.Item.FindControl("txtAttributeValue") as TextBox;
                HiddenField hfExpenditureCategoryAttributeLookupId = e.Item.FindControl("hfExpenditureCategoryAttributeLookupId") as HiddenField;
                ExpenditureCategoryAttribute objExpenditureCategoryAttribute = e.Item.DataItem as ExpenditureCategoryAttribute;
                hfExpenditureCategoryAttributeLookupId.Value = objExpenditureCategoryAttribute.ExpenditureCategoryAttributeLookupID.ToString();
                lblAttributeName.Text = objExpenditureCategoryAttribute.ExpenditureCategoryAttributeLookup.Description;
                txtAttributeValue.Text = objExpenditureCategoryAttribute.AttributeValue;
            }
        }

    }
}