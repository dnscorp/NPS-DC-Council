using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using System.IO;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.SortFields;
using PRIFACT.PRIFACTBase.MiscHelpers;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums;
using System.Web.UI.HtmlControls;
using PRIFACT.PRIFACTBase.UrlHelpers;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Reports
{
    public partial class NPSReportsCtrl : System.Web.UI.UserControl
    {
        #region Private Properties
        //Properties for sorting in the repeater
        private OfficeSortFields SortField
        {
            get
            {
                return EnumHelper.ParseEnum<OfficeSortFields>(hfSortField.Value);
            }
            set
            {
                hfSortField.Value = value.ToString();
            }
        }

        private OrderByDirection OrderByDirection
        {
            get
            {
                return EnumHelper.ParseEnum<OrderByDirection>(hfOrderByDirection.Value);
            }
            set
            {
                hfOrderByDirection.Value = value.ToString();
            }
        }

        #endregion

        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            _SetUI();
            //The data needs to be binded here since the viewstate is disabled and we need the posted values to determine the data to be fetched.
            _BindData(string.Empty, -1, null, SortField, OrderByDirection);
        }

        #endregion
        #region Search Header Events
        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //No need to do anything here since the data is bound on PageLoad            
        }
        #endregion

        #region Repeater Events
        protected void lnkGenerateNPSReport_ServerClick(object sender, EventArgs e)
        {
            Page.Validate("ValGroup1");
            if (!Page.IsValid)
            {
                return;
            }
            HtmlAnchor lnkGenerateNPSReport = sender as HtmlAnchor;
            long lOfficeId = Convert.ToInt64(lnkGenerateNPSReport.Attributes["OfficeId"]);

            //The Fiscal year selected by the user.
            FiscalYear objFiscalYear = NPSRequestContext.GetContext().FiscalYearSelected;

            //Office which was selected
            Office objOffice = Office.GetByOfficeID(lOfficeId);

            //Setting values for Recurring Transaction
            RecurringTransaction.GetAsOfDateTime = Convert.ToDateTime(txtAsOfDate.Text.Trim());
            RecurringTransaction.FiscalYear = NPSRequestContext.GetContext().FiscalYearSelected.Year;

            //Get the list of budgets for the selected office and the selected fiscal year.
            List<IDataHelper> lstBudgets = objOffice.Budgets(objFiscalYear.FiscalYearID);
            if (lstBudgets != null && lstBudgets.Count > 0)
            {
                litReportStatus.Visible = false;
                Guid downloadGuid = ReportHelper.GenerateNPSReport(objOffice, objFiscalYear, Convert.ToDateTime(txtAsOfDate.Text.Trim()));                
                Response.Redirect("~/Pages/Download.aspx?Type=NPSReport&Id=" + downloadGuid.ToString() + "&FY=" + NPSRequestContext.GetContext().FiscalYearSelected.Year.ToString() + "&OfficeId=" + lOfficeId.ToString() + "&AsOfDate=" + txtAsOfDate.Text.Trim());
            }
            else
            {
                litReportStatus.Visible = true;
                litReportStatus.Text = "Report cannot be generated, since no budget is assigned to the office-"+ objOffice.Name;
            }
        }
        protected void rptrResult_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Literal litOfficeName = e.Item.FindControl("litOfficeName") as Literal;
                Literal litActiveFrom = e.Item.FindControl("litActiveFrom") as Literal;
                Literal litActiveTo = e.Item.FindControl("litActiveTo") as Literal;
                Literal litPCA = e.Item.FindControl("litPCA") as Literal;
                Literal litPCATitle = e.Item.FindControl("litPCATitle") as Literal;
                Literal litIndexCode = e.Item.FindControl("litIndexCode") as Literal;
                Literal litIndexTitle = e.Item.FindControl("litIndexTitle") as Literal;
                Literal litCreatedDate = e.Item.FindControl("litCreatedDate") as Literal;
                Literal litUpdatedDate = e.Item.FindControl("litUpdatedDate") as Literal;

                HtmlAnchor lnkGenerateNPSReport = e.Item.FindControl("lnkGenerateNPSReport") as HtmlAnchor;

                Office objOffice = e.Item.DataItem as Office;
                litOfficeName.Text = objOffice.Name;
                litActiveFrom.Text = objOffice.ActiveFrom.ToShortDateString();
                if (objOffice.ActiveTo.HasValue)
                {
                    litActiveTo.Text = objOffice.ActiveTo.Value.ToShortDateString();
                }
                litPCA.Text = objOffice.PCA;
                litPCATitle.Text = objOffice.PCATitle;
                litIndexCode.Text = objOffice.IndexCode;
                litIndexTitle.Text = objOffice.IndexTitle;
                litCreatedDate.Text = UIHelper.GetDateTimeInDefaultFormat(objOffice.CreatedDate);
                litUpdatedDate.Text = UIHelper.GetDateTimeInDefaultFormat(objOffice.UpdatedDate);
                lnkGenerateNPSReport.Attributes.Add("OfficeId", objOffice.OfficeID.ToString());

            }
        }
        #endregion
        #region Paging and Sorting Events
        //Binding the data based on page number
        protected void PagerCtrl1_BindMainRepeater(object sender, EventArgs e)
        {
            _BindData(string.Empty, -1, null, SortField, OrderByDirection);
        }

        //Sorting done here
        protected void rptrResult_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            OfficeSortFields selectedSortField = EnumHelper.ParseEnum<OfficeSortFields>(e.CommandName);
            if (selectedSortField == SortField)
            {
                _SwitchOrderByDirection();
                _BindData(string.Empty, -1, null, SortField, OrderByDirection);
            }
            else
            {
                SortField = selectedSortField;
                OrderByDirection = Core.NPSCommon.Enums.OrderByDirection.Ascending;
                _BindData(string.Empty, -1, null, SortField, OrderByDirection);
            }
        }
        #endregion

        #region Private Methods

        //Binding the data to repeater initially
        private void _InitializeRepeater()
        {
            //PagerCtrl1.CurrentPageIndex = 0;
            _BindData(string.Empty, -1, null, SortField, OrderByDirection);
        }

        //Bind the data to the repeater based on the page selected and sort order
        private void _BindData(string strSearchText, int? iPageSize, int? iPageNumber, OfficeSortFields sortField, OrderByDirection orderByDirection)
        {
            ResultInfo objResultInfo = null;
            long lFiscalYearId = NPSRequestContext.GetContext().FiscalYearSelected.FiscalYearID;
            objResultInfo = Office.GetAll(strSearchText, lFiscalYearId, iPageSize, iPageNumber, sortField, orderByDirection);
            if (objResultInfo != null)
            {
                if (objResultInfo.Items.Count > 0)
                {
                    //PagerCtrl1.Visible = true;
                    rptrResult.Visible = true;
                    litNoResults.Visible = false;

                    //Initilizing the pager control
                    //PagerCtrl1.SetPager(objResultInfo.RowCount);


                }
                else
                {
                    //PagerCtrl1.Visible = false;

                    litNoResults.Visible = true;
                    litNoResults.Text = "No offices found.";
                }
                //Binding the data
                rptrResult.DataSource = objResultInfo.Items;
                rptrResult.DataBind();
            }
        }
        //Reset all the fields and rebind data
        private void _ResetAll()
        {
            //txtSearch.Text = string.Empty;
            _InitializeRepeater();
            //upSearchResults.Update();
        }

        //Changint the direction of sort
        private void _SwitchOrderByDirection()
        {
            if (OrderByDirection == Core.NPSCommon.Enums.OrderByDirection.Ascending)
            {
                OrderByDirection = Core.NPSCommon.Enums.OrderByDirection.Descending;
            }
            else
            {
                OrderByDirection = Core.NPSCommon.Enums.OrderByDirection.Ascending;
            }
        }

        private void _SetUI()
        {
            litSelectedFiscalYear.Text = NPSRequestContext.GetContext().FiscalYearSelected.Name;
            CalendarExtender1.Format = AppSettings.DateFormat;
            //txtSearch.Attributes.Add("placeholder", "Search");
        }
        #endregion

        protected void cvalAsOfDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            if (string.IsNullOrEmpty(txtAsOfDate.Text))
            {
                cv.ErrorMessage = "As of date should not be empty";
                args.IsValid = false;
                return;
            }
            DateTime dtDate = new DateTime();
            if (!DateTime.TryParse(txtAsOfDate.Text.Trim(), out dtDate))
            {
                cv.ErrorMessage = "Please enter a valid date for As of date";
                args.IsValid = false;
                return;
            }
            if (Convert.ToDateTime(txtAsOfDate.Text) < NPSRequestContext.GetContext().FiscalYearSelected.StartDate)
            {
                cv.ErrorMessage = "Entered date outside current fiscal year,Selected Fiscal Year starts from" + NPSRequestContext.GetContext().FiscalYearSelected.StartDate.ToShortDateString() + " to " + NPSRequestContext.GetContext().FiscalYearSelected.EndDate.ToShortDateString();
                args.IsValid = false;
                return;
            }
            if (Convert.ToDateTime(txtAsOfDate.Text) > NPSRequestContext.GetContext().FiscalYearSelected.EndDate)
            {
                cv.ErrorMessage = "Entered date outside current fiscal year,Selected Fiscal Year starts from" + NPSRequestContext.GetContext().FiscalYearSelected.StartDate.ToShortDateString() + " to " + NPSRequestContext.GetContext().FiscalYearSelected.EndDate.ToShortDateString();
                args.IsValid = false;
                return;
            }

        }
    }
}