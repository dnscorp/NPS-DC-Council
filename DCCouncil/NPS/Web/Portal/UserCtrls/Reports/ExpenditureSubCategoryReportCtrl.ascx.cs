using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Helpers;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Reports
{
    public partial class ExpenditureSubCategoryReportCtrl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hfFiscalYearId.Value = NPSRequestContext.GetContext().FiscalYearSelected.FiscalYearID.ToString();
            litSelectedFiscalYear.Text = NPSRequestContext.GetContext().FiscalYearSelected.Name;
            if (!IsPostBack)
            {
                _LoadOffices();
                _LoadExpenditureSubCategories();
            }
            _SetUI();
        }

        protected void lnkSubmit_ServerClick(object sender, EventArgs e)
        {
            Page.Validate("ValGroup1");
            if (!Page.IsValid)
                return;

            DateTime? dtStartDate = null;

            List<long> lstOfficeIds = new List<long>();
            List<long> lstExpenditureSubCategoryIds = new List<long>();

            if (radBetweenTwoDates.Checked)
                dtStartDate = Convert.ToDateTime(txtStartDate.Text);

            foreach (ListItem item in chkOfficeList.Items)
            {
                if (item.Selected)
                {
                    lstOfficeIds.Add(Convert.ToInt64(item.Value));
                }
            }
            foreach (ListItem item in chkExpenditureSubCategoriesList.Items)
            {
                if (item.Selected)
                {
                    lstExpenditureSubCategoryIds.Add(Convert.ToInt64(item.Value));
                }
            }

            List<ExpenditureSubCategorySummaryHelper> lstExpenditures = Expenditure.GetAllExpenditureSubCategoryReport(Office.GenerateXml(lstOfficeIds), Convert.ToInt64(hfFiscalYearId.Value), ExpenditureSubCategory.GenerateXml(lstExpenditureSubCategoryIds), DateTime.ParseExact(txtAsOfDate.Text, CalendarExtender1.Format, null));
            FiscalYear objFiscalYear = FiscalYear.GetByFiscalYearID(Convert.ToInt64(hfFiscalYearId.Value));
            Guid downloadGuid = ExpenditureSubCategoryReportHelper.GenerateExpenditureSubCategoryReport(objFiscalYear, lstOfficeIds, lstExpenditureSubCategoryIds, DateTime.ParseExact(txtAsOfDate.Text, CalendarExtender1.Format, null), lstExpenditures, dtStartDate);
            Response.Redirect("~/Pages/Download.aspx?Type=ExpenditureSubCategoryReport&Id=" + downloadGuid.ToString() + "&FY=" + NPSRequestContext.GetContext().FiscalYearSelected.Year.ToString() + "&AsOfDate=" + txtAsOfDate.Text.Trim());
        }

        private void _SetUI()
        {
            CalendarExtender1.Format = AppSettings.DateFormat;
        }
        private void _LoadOffices()
        {
            List<IDataHelper> lstOffices = Office.GetAll(string.Empty, Convert.ToInt64(hfFiscalYearId.Value), -1, 0, 0, 0).Items;
            if (lstOffices != null && lstOffices.Count > 0)
            {
                chkOfficeList.DataSource = lstOffices;
                chkOfficeList.DataTextField = "Name";
                chkOfficeList.DataValueField = "OfficeID";
                chkOfficeList.DataBind();
            }
            else
            {
                litErrorMessage.Text = "No Offices assigned in the system";
                tblExpenditureSubCategoryReport.Visible = false;
                return;
            }
        }

        private void _LoadExpenditureSubCategories()
        {
            List<IDataHelper> lstExpenditures = ExpenditureSubCategory.GetAllExpenditureSubCategories().Items;
            if (lstExpenditures != null && lstExpenditures.Count > 0)
            {
                chkExpenditureSubCategoriesList.DataSource = lstExpenditures;
                chkExpenditureSubCategoriesList.DataTextField = "Name";
                chkExpenditureSubCategoriesList.DataValueField = "ExpenditureSubCategoryID";
                chkExpenditureSubCategoriesList.DataBind();
            }
            else
            {
                litErrorMessage.Text = "No Expenditure sub-categories assigned in the system.Report cannot be generated";
                tblExpenditureSubCategoryReport.Visible = false;
                return;
            }
        }

        protected void cvalOffices_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            List<long> lstOfficeIds = new List<long>();
            if (chkOfficeList.SelectedItem == null)
            {
                cv.ErrorMessage = "Please select at least one office.";
                args.IsValid = false;
                return;
            }



        }
        protected void cvalExpenditureSubCategory_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            List<long> lstExpenditureSubCategoryIds = new List<long>();
            if (chkExpenditureSubCategoriesList.SelectedItem == null)
            {
                cv.ErrorMessage = "Please select at least one expenditure sub category for the report generation.";
                args.IsValid = false;
                return;
            }


        }
        protected void cvalStartDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (radBetweenTwoDates.Checked)
            {
                CustomValidator cv = (CustomValidator)source;
                if (string.IsNullOrEmpty(txtStartDate.Text.Trim()))
                {
                    cv.ErrorMessage = "Start date should not be empty";
                    args.IsValid = false;
                    return;
                }
                DateTime dtDate = new DateTime();
                if (!DateTime.TryParse(txtStartDate.Text.Trim(), out dtDate))
                {
                    cv.ErrorMessage = "Please enter a valid date for start date";
                    args.IsValid = false;
                    return;
                }

                if (Convert.ToDateTime(txtStartDate.Text) > NPSRequestContext.GetContext().FiscalYearSelected.EndDate)
                {
                    cv.ErrorMessage = "Please select date within the current Fiscal Year " + NPSRequestContext.GetContext().FiscalYearSelected.StartDate.ToShortDateString() + "-" + NPSRequestContext.GetContext().FiscalYearSelected.EndDate.ToShortDateString();
                    args.IsValid = false;
                    return;
                }

                if (Convert.ToDateTime(txtStartDate.Text) < NPSRequestContext.GetContext().FiscalYearSelected.StartDate)
                {
                    cv.ErrorMessage = "Selected date range doesn't lie within the current Fiscal Year " + NPSRequestContext.GetContext().FiscalYearSelected.StartDate.ToShortDateString() + "-" + NPSRequestContext.GetContext().FiscalYearSelected.EndDate.ToShortDateString();
                    args.IsValid = false;
                    return;
                }
            }
        }
        protected void cvalAsOfDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            if (string.IsNullOrEmpty(txtAsOfDate.Text.Trim()))
            {
                cv.ErrorMessage = "End date should not be empty";
                args.IsValid = false;
                return;
            }
            DateTime dtDate = new DateTime();
            if (!DateTime.TryParse(txtAsOfDate.Text.Trim(), out dtDate))
            {
                cv.ErrorMessage = "Please enter a valid date for end date";
                args.IsValid = false;
                return;
            }
            if (radBetweenTwoDates.Checked)
            {
                if (Convert.ToDateTime(txtAsOfDate.Text) > NPSRequestContext.GetContext().FiscalYearSelected.EndDate)
                {
                    cv.ErrorMessage = "Selected date doesn't lie within the current Fiscal Year " + NPSRequestContext.GetContext().FiscalYearSelected.StartDate.ToShortDateString() + "-" + NPSRequestContext.GetContext().FiscalYearSelected.EndDate.ToShortDateString();
                    args.IsValid = false;
                    return;
                }

                if (string.IsNullOrEmpty(txtStartDate.Text.Trim()))
                {
                    args.IsValid = false;
                    return;
                }

                if (Convert.ToDateTime(txtAsOfDate.Text) < Convert.ToDateTime(txtStartDate.Text))
                {
                    cv.ErrorMessage = "Selected date range doesn't lie within the current Fiscal Year " + NPSRequestContext.GetContext().FiscalYearSelected.StartDate.ToShortDateString() + "-" + NPSRequestContext.GetContext().FiscalYearSelected.EndDate.ToShortDateString();
                    args.IsValid = false;
                    return;
                }

            }
            else
            {
                DateTime dtEndDate = NPSRequestContext.GetContext().FiscalYearSelected.EndDate;
                DateTime dtStartDate = NPSRequestContext.GetContext().FiscalYearSelected.StartDate;
                if (Convert.ToDateTime(txtAsOfDate.Text) > dtEndDate.Date || Convert.ToDateTime(txtAsOfDate.Text) < dtStartDate.Date)
                {
                    cv.ErrorMessage = "Selected date doesn't lie within the current Fiscal Year " + NPSRequestContext.GetContext().FiscalYearSelected.StartDate.ToShortDateString() + "-" + NPSRequestContext.GetContext().FiscalYearSelected.EndDate.ToShortDateString();
                    args.IsValid = false;
                    return;
                }
            }
        }

        protected void radToAsOfDate_CheckedChanged(object sender, EventArgs e)
        {
            if (radToAsOfDate.Checked)
            {
                trStartDate.Visible = false;

            }
            else
            {
                trStartDate.Visible = true;
            }
        }
    }
}