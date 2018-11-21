using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.SortFields;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.UserCtrlModes;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Common;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using PRIFACT.PRIFACTBase.MiscHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.Pages.Members
{
    public partial class RecurringTransactions : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            Page.Title = "Recurring Transactions";
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}