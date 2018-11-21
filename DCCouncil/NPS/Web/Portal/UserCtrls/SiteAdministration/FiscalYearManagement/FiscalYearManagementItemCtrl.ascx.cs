using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.UserCtrlModes;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.SiteAdministration.FiscalYearManagement
{
    public partial class FiscalYearManagementItemCtrl : System.Web.UI.UserControl
    {
        #region Public Events
        public event EventHandler SubmitClick;
        public event EventHandler CancelClick;
        #endregion

        #region Public Properties
        public FiscalYearManagementItemCtrlMode Mode
        {
            get
            {
                if (string.IsNullOrEmpty(hfMode.Value))
                {
                    return FiscalYearManagementItemCtrlMode.NotSet;
                }
                else
                {
                    return (FiscalYearManagementItemCtrlMode)Convert.ToInt32(hfMode.Value);
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
        public void InitializeFields(FiscalYear objFiscalYear)
        {
            if (objFiscalYear != null)
            {
                hfFiscalYearId.Value = objFiscalYear.FiscalYearID.ToString();
                txtName.Text = objFiscalYear.Name;
                txtYear.Text = objFiscalYear.Year.ToString();
                litStartDate.Text = UIHelper.GetDateTimeInDefaultFormat(objFiscalYear.StartDate);
                litEndDate.Text = UIHelper.GetDateTimeInDefaultFormat(objFiscalYear.EndDate);
                phDates.Visible = true;
            }
            else
            {
                hfFiscalYearId.Value = string.Empty;
                txtName.Text = string.Empty;
                txtYear.Text = string.Empty;
                litStartDate.Text = string.Empty;
                litEndDate.Text = string.Empty;
                phDates.Visible = false;
            }
        }
        #endregion

        #region Private Methods
        private void _SetUI()
        {
            if (Mode == FiscalYearManagementItemCtrlMode.Create)
            {
                _ShowPopup();
                lnkSubmit.InnerText = "Create";
            }
            if (Mode == FiscalYearManagementItemCtrlMode.Edit)
            {
                _ShowPopup();
                lnkSubmit.InnerText = "Update";
            }
            if (Mode == FiscalYearManagementItemCtrlMode.NotSet)
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

            if (Mode == FiscalYearManagementItemCtrlMode.Create)
            {
                //Creating the user
                DateTime dtStartDate = new DateTime(Convert.ToInt32(txtYear.Text.Trim()) - 1, AppSettings.FiscalYearStartMonth, AppSettings.FiscalYearStartDay);
                DateTime dtEndDate = dtStartDate.AddMonths(AppSettings.FiscalYearMonthDuration).AddDays(-1);
                FiscalYear.Create(txtName.Text.Trim(), Convert.ToInt32(txtYear.Text), dtStartDate, dtEndDate);
                UIHelper.SetSuccessMessage("Fiscal Year created successfully");
            }
            if (Mode == FiscalYearManagementItemCtrlMode.Edit)
            {
                if (Expenditure.GetAll(String.Empty, String.Empty, Convert.ToInt64(hfFiscalYearId.Value), String.Empty, null, -1, null, Core.NPSCommon.Enums.SortFields.ExpenditureSortFields.IndexCode, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items.Count > 0)
                {
                    UIHelper.SetErrorMessage("Unable to edit Fiscal Year as there are Expenditures under it.");
                }
                else
                {
                    long lFiscalYearId = Convert.ToInt64(hfFiscalYearId.Value);
                    FiscalYear objFiscalYear = FiscalYear.GetByFiscalYearID(lFiscalYearId);
                    objFiscalYear.Name = txtName.Text.Trim();
                    objFiscalYear.Year = Convert.ToInt32(txtYear.Text.Trim());
                    DateTime dtStartDate = new DateTime(Convert.ToInt32(txtYear.Text.Trim()), AppSettings.FiscalYearStartMonth, AppSettings.FiscalYearStartDay);
                    DateTime dtEndDate = dtStartDate.AddMonths(AppSettings.FiscalYearMonthDuration).AddDays(-1);
                    objFiscalYear.StartDate = dtStartDate;
                    objFiscalYear.EndDate = dtEndDate;
                    objFiscalYear.Update();
                    UIHelper.SetSuccessMessage("Fiscal Year updated successfully");
                }
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
            Mode = FiscalYearManagementItemCtrlMode.NotSet;
            _HidePopup();

            //Raising the CancelClick event
            if (CancelClick != null)
            {
                CancelClick(this, EventArgs.Empty);
            }
        }
        #endregion

        protected void cvalName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            //Validate whether the username already exists
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                cv.ErrorMessage = "Name should not be empty";
                args.IsValid = false;
                return;
            }
        }

        protected void cvalYear_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            int iYear = 0;
            bool blnValid = int.TryParse(txtYear.Text.Trim(), out iYear);
            if (!blnValid)
            {
                cv.ErrorMessage = "Year should be a valid integer.";
                args.IsValid = false;
                return;
            }

            iYear = Convert.ToInt32(txtYear.Text.Trim());
            if (!(iYear >= DateTime.MinValue.Year && iYear <= DateTime.MaxValue.Year))
            {
                cv.ErrorMessage = "Year should be a between " + DateTime.MinValue.Year + " and " + DateTime.MaxValue.Year + ".";
                args.IsValid = false;
                return;
            }

            
             
                List<IDataHelper> lstFiscalYears = FiscalYear.GetAll(-1, null, Core.NPSCommon.Enums.SortFields.FiscalYearSortFields.Year, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items;
                if (lstFiscalYears.Count > 0)
                {
                    if (Mode == FiscalYearManagementItemCtrlMode.Create)
                    {
                        if (lstFiscalYears.ConvertAll(q => (FiscalYear)q).Where(q => q.Year == iYear).Count() > 0)
                        {
                            cv.ErrorMessage = "Entered year already exists.";
                            args.IsValid = false;
                            return;
                        }
                    }

                    if (Mode == FiscalYearManagementItemCtrlMode.Edit)
                    {
                        if (lstFiscalYears.ConvertAll(q => (FiscalYear)q).Where(q => q.Year == iYear && q.FiscalYearID != Convert.ToInt64(hfFiscalYearId.Value)).Count() > 0)
                        {
                            cv.ErrorMessage = "Entered year already exists.";
                            args.IsValid = false;
                            return;
                        }
                    }
                    
                }

        }

    }
}