using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Common
{
    public partial class HeaderCtrl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void OnLnkSignoutClick(object sender, EventArgs e)
        {
            AuthenticationHelper.Signout();
            Response.Redirect("~/Pages/Login");
        }
    }
}