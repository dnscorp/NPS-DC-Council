using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.PRIFACTBase.UrlHelpers;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.Pages.SiteAdministration
{
    public partial class BudgetManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            long lOfficeId = Convert.ToInt64(UrlUtility.GetStringFromQv(Request["OfficeId"]));
            BudgetManagementCtrl1.Office = Office.GetByOfficeID(lOfficeId);
        }
    }
}