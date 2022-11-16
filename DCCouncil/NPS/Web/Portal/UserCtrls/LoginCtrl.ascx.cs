using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using PRIFACT.DCCouncil.NPS.Core.NPSApi.SessionHelper;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls
{
    public partial class LoginCtrl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();
        }

        protected void OnBtnSubmitClick(object sender, EventArgs e)
        {
            User objUser = User.Validate(txtUsername.Text.Trim(), PRIFACT.PRIFACTBase.PasswordHelpers.HashPassword.HashMD5(txtPassword.Text.Trim()));
            if (objUser == null)
            {
                lblMsg.Text = "Invalid Username or password";
                return;
            }            
            PRIFACT.DCCouncil.NPS.Core.NPSApi.SessionHelper.SessionManager.GetSessionData().LoggedInUser = objUser;            
            if (Request.QueryString["ReturnUrl"] != null)
            {
                FormsAuthentication.RedirectFromLoginPage(this.txtUsername.Text.Trim(), false);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(this.txtUsername.Text.Trim(), false);
                var fiscalYear = NPSRequestContext.GetContext().FiscalYearSelected;
                if(fiscalYear!=null && fiscalYear.Year >= 2023)
                    Response.Redirect(NPSUrls.DashboardV2);
                else
                    Response.Redirect(NPSUrls.Dashboard);
            }
        }
    }
}