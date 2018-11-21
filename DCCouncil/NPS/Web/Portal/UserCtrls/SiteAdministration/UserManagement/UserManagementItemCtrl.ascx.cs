using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.UserCtrlModes;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using System.Text.RegularExpressions;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.SiteAdministration.UserManagement
{
    public partial class UserManagementItemCtrl : System.Web.UI.UserControl
    {
        #region Public Events
        public event EventHandler SubmitClick;
        public event EventHandler CancelClick;
        #endregion

        #region Public Properties
        public UserManagementItemCtrlMode Mode
        {
            get
            {
                if (string.IsNullOrEmpty(hfMode.Value))
                {
                    return UserManagementItemCtrlMode.NotSet;
                }
                else
                {
                    return (UserManagementItemCtrlMode)Convert.ToInt32(hfMode.Value);
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
        public void InitializeFields(User objUser,bool bchkChangePassword)
        {
            if (objUser != null)
            {
                hfUserId.Value = objUser.UserID.ToString();
                txtFirstName.Text = objUser.UserProfile.FirstName;
                txtLastName.Text = objUser.UserProfile.LastName;
                txtUsername.Text = objUser.Username;
                phPassword.Visible = false;
                chkChangePassword.Checked = bchkChangePassword;

            }
            else
            {
                txtConfirmPassword.Text = string.Empty;
                txtFirstName.Text = string.Empty;
                txtLastName.Text = string.Empty;
                txtPassword.Text = string.Empty;
                txtUsername.Text = string.Empty;
            }
        }
        #endregion

        #region Private Methods
        private void _SetUI()
        {
            if (Mode == UserManagementItemCtrlMode.Create)
            {
                _ShowPopup();
                lnkSubmit.InnerText = "Create";
                trChangePassword.Visible = false;
                phPassword.Visible = true;
            }
            if (Mode == UserManagementItemCtrlMode.Edit)
            {
                _ShowPopup();
                lnkSubmit.InnerText = "Update";
                trChangePassword.Visible = true;
               
            }
            if (Mode == UserManagementItemCtrlMode.NotSet)
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

            if (Mode == UserManagementItemCtrlMode.Create)
            {
                //Creating the user
                User.Create(txtUsername.Text.Trim(), PRIFACT.PRIFACTBase.PasswordHelpers.HashPassword.HashMD5(txtPassword.Text.Trim()), true, false, txtFirstName.Text.Trim(), txtLastName.Text.Trim());
                UIHelper.SetSuccessMessage("User created successfully");
            }
            if (Mode == UserManagementItemCtrlMode.Edit)
            {
                //Updating the user
                long lUserId = Convert.ToInt64(hfUserId.Value);
                User objUser = User.GetByUserID(lUserId);
                objUser.Username = txtUsername.Text.Trim();
                if (chkChangePassword.Checked == true)
                {
                    if (!string.IsNullOrEmpty(txtPassword.Text.Trim()))
                        objUser.PasswordHash = PRIFACT.PRIFACTBase.PasswordHelpers.HashPassword.HashMD5(txtPassword.Text.Trim());
                }
                objUser.UserProfile.FirstName = txtFirstName.Text.Trim();
                objUser.UserProfile.LastName = txtLastName.Text.Trim();
                objUser.Update();
                UIHelper.SetSuccessMessage("User updated successfully");
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
            Mode = UserManagementItemCtrlMode.NotSet;
            _HidePopup();

            //Raising the CancelClick event
            if (CancelClick != null)
            {
                CancelClick(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Custom Validator Validation Events
        protected void cvalFirstName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            if (string.IsNullOrEmpty(txtFirstName.Text.Trim()))
            {
                cv.ErrorMessage = "First Name should not be empty";
                args.IsValid = false;
                return;
            }
        }

        protected void cvalPassword_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (Mode == UserManagementItemCtrlMode.Create || (Mode == UserManagementItemCtrlMode.Edit && chkChangePassword.Checked == true))
            {
                CustomValidator cv = (CustomValidator)source;
                if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
                {
                    cv.ErrorMessage = "Password should not be empty";
                    args.IsValid = false;
                    return;
                }
                String regEx = AppSettings.PasswordComplexityRegEx;
                Match match = Regex.Match(txtPassword.Text, regEx, RegexOptions.IgnoreCase);
                if (!match.Success)
                {
                    args.IsValid = false;
                    cv.ErrorMessage = AppSettings.PasswordComplexityErrorMessage;
                    return;
                }
            }
        }

        protected void cvalUsername_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            //Validate whether the username already exists
            if (string.IsNullOrEmpty(txtUsername.Text.Trim()))
            {
                cv.ErrorMessage = "Username should not be empty";
                args.IsValid = false;
                return;
            }
            if (Mode == UserManagementItemCtrlMode.Create)
            {
                if (User.GetByUserName(txtUsername.Text.Trim()) != null)
                {
                    cv.ErrorMessage = "Username already exist.";
                    args.IsValid = false;
                    return;
                }
            }
            if (Mode == UserManagementItemCtrlMode.Edit)
            {
                if ((User.GetByUserID(Convert.ToInt64(hfUserId.Value)).Username != txtUsername.Text) && (User.GetByUserName(txtUsername.Text.Trim()) != null))
                {
                    cv.ErrorMessage = "Username already exist.";
                    args.IsValid = false;
                    return;
                }
            }
        }

        protected void cvalConfirmPassword_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (Mode == UserManagementItemCtrlMode.Create || (Mode == UserManagementItemCtrlMode.Edit && chkChangePassword.Checked == true))
            {
                CustomValidator cv = (CustomValidator)source;
                if (!txtPassword.Text.Trim().Equals(txtConfirmPassword.Text.Trim()))
                {
                    cv.ErrorMessage = "Password and Confirm password should match";
                    args.IsValid = false;
                    return;
                }
            }
        }
        #endregion

        protected void chkChangePassword_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkChangePassword = (CheckBox)sender;
            if (chkChangePassword.Checked == true)
            {
                phPassword.Visible = true;
            }
            else
            {
                phPassword.Visible = false;
            }
        }

    }
}