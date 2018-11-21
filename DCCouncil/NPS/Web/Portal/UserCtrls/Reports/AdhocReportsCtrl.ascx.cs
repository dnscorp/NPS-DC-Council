using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary;
using System.Globalization;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Reports
{
    public partial class AdhocReportsCtrl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hfFiscalYearId.Value = NPSRequestContext.GetContext().FiscalYearSelected.FiscalYearID.ToString();
            litSelectedFiscalYear.Text = NPSRequestContext.GetContext().FiscalYearSelected.Name;
            if (!IsPostBack)
            {
                _LoadOffices();
                _LoadExpenditureCategories();
            }
            _SetUI();
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
                tblAdhocReport.Visible = false;
                return;
            }
        }
        private void _LoadExpenditureCategories()
        {
            List<IDataHelper> lstExpenditures = ExpenditureCategory.GetAll(string.Empty, -1, 0, 0, 0).Items;
            if (lstExpenditures != null && lstExpenditures.Count > 0)
            {
                chkExpenditureCategoriesList.DataSource = lstExpenditures;
                chkExpenditureCategoriesList.DataTextField = "Name";
                chkExpenditureCategoriesList.DataValueField = "ExpenditureCategoryID";
                chkExpenditureCategoriesList.DataBind();

                ListItem li = new ListItem();
                li.Text = "Purchase Orders (PO)";
                li.Value = "16";
                chkExpenditureCategoriesList.Items.Add(li);
                
            }
            else
            {
                litErrorMessage.Text = "No Expenditure categories assigned in the system.Report cannot be generated";
                tblAdhocReport.Visible = false;
                return;
            }

            
        }


        //protected void chkAllExpenditureCategories_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (chkAllExpenditureCategories.Checked)
        //    {
        //        foreach (ListItem item in chkExpenditureCategoriesList.Items)
        //        {
        //            item.Selected = true;
        //        }
        //        chkExpenditureCategoriesList.Enabled = true;
        //    }
        //    else
        //    {
        //        foreach (ListItem item in chkExpenditureCategoriesList.Items)
        //        {
        //            item.Selected = false;

        //        }
        //        chkExpenditureCategoriesList.Enabled = true;
        //    }
        //}
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
        protected void cvalExpenditureCategory_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            List<long> lstExpenditureCategoryIds = new List<long>();
            if (chkExpenditureCategoriesList.SelectedItem == null)
            {
                cv.ErrorMessage = "Please select at least one expenditure category for the report generation.";
                args.IsValid = false;
                return;
            }


        }
        protected void cvalReportType_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;

            if (!chkYearWise.Checked && !chkMonthWise.Checked && !chkCustomReport.Checked)
            {
                cv.ErrorMessage = "Please select atleast one report to be generated.";
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
                    cv.ErrorMessage = "Please select date within current Fiscal Year " + NPSRequestContext.GetContext().FiscalYearSelected.StartDate.ToShortDateString() + "-" + NPSRequestContext.GetContext().FiscalYearSelected.EndDate.ToShortDateString();
                    args.IsValid = false;
                    return;
                }
                if (Convert.ToDateTime(txtStartDate.Text) < NPSRequestContext.GetContext().FiscalYearSelected.StartDate)
                {
                    cv.ErrorMessage = "Selected date range doesn't lie within current Fiscal Year " + NPSRequestContext.GetContext().FiscalYearSelected.StartDate.ToShortDateString() + "-" + NPSRequestContext.GetContext().FiscalYearSelected.EndDate.ToShortDateString(); 
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
                    cv.ErrorMessage = "Selected date doesn't lie within current Fiscal Year " + NPSRequestContext.GetContext().FiscalYearSelected.StartDate.ToShortDateString() + "-" + NPSRequestContext.GetContext().FiscalYearSelected.EndDate.ToShortDateString();
                    args.IsValid = false;
                    return;
                }

                if (Convert.ToDateTime(txtAsOfDate.Text) < Convert.ToDateTime(txtStartDate.Text))
                {
                    cv.ErrorMessage = "Selected date range doesn't lie within current Fiscal Year " + NPSRequestContext.GetContext().FiscalYearSelected.StartDate.ToShortDateString() + "-" + NPSRequestContext.GetContext().FiscalYearSelected.EndDate.ToShortDateString();
                    args.IsValid = false;
                    return;
                }

            }
            else
            {
                DateTime dtEndDate = NPSRequestContext.GetContext().FiscalYearSelected.EndDate;
                if (Convert.ToDateTime(txtAsOfDate.Text) > dtEndDate.Date)
                {
                    cv.ErrorMessage = "Selected date doesn't lie within current Fiscal Year" + NPSRequestContext.GetContext().FiscalYearSelected.StartDate.ToShortDateString() + " " + NPSRequestContext.GetContext().FiscalYearSelected.EndDate.ToShortDateString();
                    args.IsValid = false;
                    return;
                }
            }
        }
        protected void lnkSubmit_ServerClick(object sender, EventArgs e)
        {
            Page.Validate("ValGroup1");
            if (!Page.IsValid)
                return;
            bool blnIsYearWise = false;
            bool blnIsMonthWise = false;
            bool blnCustomReportSheet = false;
            DateTime? dtStartDate = null;

            List<long> lstOfficeIds = new List<long>();
            List<long> lstExpenditureCategoryIds = new List<long>();

            if (chkMonthWise.Checked)
                blnIsMonthWise = true;
            if (chkYearWise.Checked)
                blnIsYearWise = true;
            if (chkCustomReport.Checked)
                blnCustomReportSheet = true;

            if (radBetweenTwoDates.Checked)
                dtStartDate = Convert.ToDateTime(txtStartDate.Text);



            foreach (ListItem item in chkOfficeList.Items)
            {
                if (item.Selected)
                {
                    lstOfficeIds.Add(Convert.ToInt64(item.Value));
                }
            }
            foreach (ListItem item in chkExpenditureCategoriesList.Items)
            {
                if (item.Selected)
                {
                    lstExpenditureCategoryIds.Add(Convert.ToInt64(item.Value));
                }
            }
            

            List<IDataHelper> lstExpenditures = Expenditure.GetAll(string.Empty, Office.GenerateXml(lstOfficeIds), Convert.ToInt64(hfFiscalYearId.Value), ExpenditureCategory.GenerateXml(lstExpenditureCategoryIds), DateTime.ParseExact(txtAsOfDate.Text, CalendarExtender1.Format, null), -1, 0, 0, 0).Items;
            FiscalYear objFiscalYear = FiscalYear.GetByFiscalYearID(Convert.ToInt64(hfFiscalYearId.Value));
            Guid downloadGuid = AdHocReportHelper.GenerateAdHocReport(objFiscalYear, lstOfficeIds, lstExpenditureCategoryIds, DateTime.ParseExact(txtAsOfDate.Text, CalendarExtender1.Format, null), lstExpenditures, blnIsYearWise, blnIsMonthWise, blnCustomReportSheet, dtStartDate);
            Response.Redirect("~/Pages/Download.aspx?Type=AdHocReport&Id=" + downloadGuid.ToString() + "&FY=" + NPSRequestContext.GetContext().FiscalYearSelected.Year.ToString() + "&AsOfDate=" + txtAsOfDate.Text.Trim());

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