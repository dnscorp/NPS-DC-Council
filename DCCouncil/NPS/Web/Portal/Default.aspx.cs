using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;

namespace PRIFACT.DCCouncil.NPS.Web.Portal
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (NPSRequestContext.GetContext().LoggedInUser == null)
            {
                //Response.Redirect(NPSUrls.Login+"?ReturnUrl=Dashboard");
                Response.Redirect(NPSUrls.Login);
            }
            else
            {
                var fiscalYear = NPSRequestContext.GetContext().FiscalYearSelected;
                if (fiscalYear != null && fiscalYear.Year >= 2023)
                    Response.Redirect(NPSUrls.DashboardV2);
                else
                    Response.Redirect(NPSUrls.Dashboard);                
            }
        }
    }
}