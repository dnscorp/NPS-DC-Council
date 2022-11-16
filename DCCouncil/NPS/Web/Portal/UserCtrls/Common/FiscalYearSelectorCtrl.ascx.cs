using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PRIFACT.DCCouncil.NPS.Core.NPSApi.SessionHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.SortFields;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using PRIFACT.PRIFACTBase.UrlHelpers;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Common
{
    public partial class FiscalYearSelectorCtrl : System.Web.UI.UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            _LoadFiscalYears();
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlFiscalYears.SelectedValue = NPSRequestContext.GetContext().LoggedInUser.LastFiscalYearSelected.FiscalYearID.ToString();
            }
        }
        private void _LoadFiscalYears()
        {
            List<IDataHelper> lstFiscalYears = FiscalYear.GetAll(-1, null, FiscalYearSortFields.Year, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items;
            ddlFiscalYears.DataSource = lstFiscalYears;
            ddlFiscalYears.DataTextField = "Name";
            ddlFiscalYears.DataValueField = "FiscalYearID";
            ddlFiscalYears.DataBind();
        }

        protected void ddlFiscalYears_SelectedIndexChanged(object sender, EventArgs e)
        {
            long lFiscalYearId = Convert.ToInt64(ddlFiscalYears.SelectedValue);
            FiscalYear objFiscalYear = FiscalYear.GetByFiscalYearID(lFiscalYearId);
            User objUser = NPSRequestContext.GetContext().LoggedInUser;
            objUser.LastFiscalYearSelected = objFiscalYear;
            objUser.Update();
            SessionManager.GetSessionData().LoggedInUser = objUser;
            NPSRequestContext.RefreshContext();
            var url = UrlUtility.FullCurrentPageUrlWithQV;
            if(url.Contains("Dashboard"))
            {
                var fiscalYear = NPSRequestContext.GetContext().FiscalYearSelected;
                if (fiscalYear != null && fiscalYear.Year >= 2023)
                    Response.Redirect(NPSUrls.DashboardV2);
                else
                    Response.Redirect(NPSUrls.Dashboard);
            }
            else
                Response.Redirect(UrlUtility.FullCurrentPageUrlWithQV);
        }
    }
}