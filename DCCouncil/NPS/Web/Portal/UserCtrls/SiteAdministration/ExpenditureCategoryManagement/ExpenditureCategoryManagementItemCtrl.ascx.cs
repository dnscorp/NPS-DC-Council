using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.UserCtrlModes;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.SiteAdministration.ExpenditureCategoryManagement
{
    public partial class ExpenditureCategoryManagementItemCtrl : System.Web.UI.UserControl
    {
        #region Public Events
        public event EventHandler SubmitClick;
        public event EventHandler CancelClick;
        #endregion

        #region Public Properties
        public ExpenditureCategoryManagementItemCtrlMode Mode
        {
            get
            {
                if (string.IsNullOrEmpty(hfMode.Value))
                {
                    return ExpenditureCategoryManagementItemCtrlMode.NotSet;
                }
                else
                {
                    return (ExpenditureCategoryManagementItemCtrlMode)Convert.ToInt32(hfMode.Value);
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
        public void InitializeFields(ExpenditureCategory objExpenditureCategory)
        {
            if (objExpenditureCategory != null)
            {
                hfCategoryId.Value = objExpenditureCategory.ExpenditureCategoryID.ToString();
                txtCategoryName.Text = objExpenditureCategory.Name;
                txtCategoryCode.Text = objExpenditureCategory.Code;
                rblIsFixed.SelectedValue = objExpenditureCategory.IsFixed.ToString();
                rblIsStaffLevel.SelectedValue = objExpenditureCategory.IsStaffLevel.ToString();                
                rblIsActive.SelectedValue = objExpenditureCategory.IsActive.ToString();
                rblAppendMonth.SelectedValue = objExpenditureCategory.AppendMonth.ToString();
                rblIsVendorStaff.SelectedValue = objExpenditureCategory.IsVendorStaff.ToString();
                rblIsMonthly.SelectedValue = objExpenditureCategory.IsMonthly.ToString();
            }
            else
            {
                txtCategoryName.Text = string.Empty;
            }
        }
        #endregion

        #region Private Methods
        private void _SetUI()
        {
            if (Mode == ExpenditureCategoryManagementItemCtrlMode.Create)
            {
                _ShowPopup();
                lnkSubmit.InnerText = "Create";
            }
            if (Mode == ExpenditureCategoryManagementItemCtrlMode.Edit)
            {
                _ShowPopup();
                lnkSubmit.InnerText = "Update";
            }
            if (Mode == ExpenditureCategoryManagementItemCtrlMode.NotSet)
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

            if (Mode == ExpenditureCategoryManagementItemCtrlMode.Create)
            {
                //Creating the ExpenditureCategory
                ExpenditureCategory.Create(txtCategoryName.Text.Trim(), txtCategoryCode.Text.Trim(), Convert.ToBoolean(rblIsStaffLevel.SelectedValue), Convert.ToBoolean(rblIsFixed.SelectedValue), Convert.ToBoolean(rblIsActive.SelectedValue), false, Convert.ToBoolean(rblIsVendorStaff.SelectedValue), Convert.ToBoolean(rblIsMonthly.SelectedValue), Convert.ToBoolean(rblAppendMonth.SelectedValue),false);
                UIHelper.SetSuccessMessage("Category added successfully");
            }
            if (Mode == ExpenditureCategoryManagementItemCtrlMode.Edit)
            {
                //Updating the user
                long categoryId = Convert.ToInt64(hfCategoryId.Value);
                ExpenditureCategory objExpenditureCategory = ExpenditureCategory.GetByID(categoryId);
                objExpenditureCategory.Name = txtCategoryName.Text.Trim();
                objExpenditureCategory.Code = txtCategoryCode.Text.Trim();
                objExpenditureCategory.IsFixed = Convert.ToBoolean(rblIsFixed.SelectedValue);
                objExpenditureCategory.IsStaffLevel = Convert.ToBoolean(rblIsStaffLevel.SelectedValue);                
                objExpenditureCategory.IsActive = Convert.ToBoolean(rblIsActive.SelectedValue);
                objExpenditureCategory.IsVendorStaff = Convert.ToBoolean(rblIsVendorStaff.SelectedValue);
                objExpenditureCategory.IsMonthly = Convert.ToBoolean(rblIsMonthly.SelectedValue);
                objExpenditureCategory.AppendMonth = Convert.ToBoolean(rblAppendMonth.SelectedValue);
                objExpenditureCategory.UpdatedDate = DateTime.Now;
                objExpenditureCategory.Update();
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
            Mode = ExpenditureCategoryManagementItemCtrlMode.NotSet;
            _HidePopup();

            //Raising the CancelClick event
            if (CancelClick != null)
            {
                CancelClick(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Custom Validator Validation Events

        protected void cvalCategoryName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            if (string.IsNullOrEmpty(txtCategoryName.Text.Trim()))
            {
                cv.ErrorMessage = "Category Name should not be empty";
                args.IsValid = false;
                return;
            }
        }

        protected void cvalCategoryCode_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            if (string.IsNullOrEmpty(txtCategoryCode.Text.Trim()))
            {
                cv.ErrorMessage = "Category Code should not be empty";
                args.IsValid = false;
                return;
            }
        }

        #endregion
    }
}