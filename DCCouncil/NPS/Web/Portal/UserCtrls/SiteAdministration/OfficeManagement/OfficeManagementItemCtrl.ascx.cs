using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.UserCtrlModes;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.SiteAdministration.OfficeManagement
{
    public partial class OfficeManagementItemCtrl : System.Web.UI.UserControl
    {
        #region Public Events
        public event EventHandler SubmitClick;
        public event EventHandler CancelClick;
        #endregion

        #region Public Properties
        public OfficeManagementItemCtrlMode Mode
        {
            get
            {
                if (string.IsNullOrEmpty(hfMode.Value))
                {
                    return OfficeManagementItemCtrlMode.NotSet;
                }
                else
                {
                    return (OfficeManagementItemCtrlMode)Convert.ToInt32(hfMode.Value);
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
        public void InitializeFields(Office objOffice)
        {
            if (objOffice != null)
            {
                hfOfficeId.Value = objOffice.OfficeID.ToString();
                txtOfficeName.Text = objOffice.Name;
                txtActiveFrom.Text = objOffice.ActiveFrom.ToShortDateString();
                if(objOffice.ActiveTo.HasValue)
                    txtActiveTo.Text = objOffice.ActiveTo.Value.ToShortDateString();
                txtIndexCode.Text = objOffice.IndexCode;
                txtIndexTitle.Text = objOffice.IndexTitle;
                txtPCA.Text = objOffice.PCA;
                txtPCATitle.Text = objOffice.PCATitle;
                txtCompCode.Text = objOffice.CompCode;

            }
            else
            {
                txtOfficeName.Text = string.Empty;
                txtActiveFrom.Text = string.Empty;
                txtActiveTo.Text = string.Empty;
                txtIndexCode.Text = string.Empty;
                txtIndexTitle.Text = string.Empty;
                txtPCA.Text = string.Empty;
                txtPCATitle.Text = string.Empty;
                txtCompCode.Text = string.Empty;
            }
        }
        #endregion

        #region Private Methods
        private void _SetUI()
        {
            if (Mode == OfficeManagementItemCtrlMode.Create)
            {
                _ShowPopup();
                lnkSubmit.InnerText = "Create";
            }
            if (Mode == OfficeManagementItemCtrlMode.Edit)
            {
                _ShowPopup();
                lnkSubmit.InnerText = "Update";
            }
            if (Mode == OfficeManagementItemCtrlMode.NotSet)
            {
                _HidePopup();
            }
            CalendarExtender1.Format = AppSettings.DateFormat;
            CalendarExtender2.Format = AppSettings.DateFormat;
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

            if (Mode == OfficeManagementItemCtrlMode.Create)
            {
                //Creating the user
                if ((String.IsNullOrEmpty(txtActiveTo.Text.Trim())))
                {
                    Office.Create(txtOfficeName.Text.Trim(), DateTime.ParseExact(txtActiveFrom.Text.ToString(), CalendarExtender1.Format, null), null, txtPCA.Text.Trim(), txtPCATitle.Text.Trim(), txtIndexCode.Text.Trim(), txtIndexTitle.Text.Trim(), false,txtCompCode.Text.Trim());
                }
                else
                {
                    Office.Create(txtOfficeName.Text.Trim(), DateTime.ParseExact(txtActiveFrom.Text.ToString(), CalendarExtender1.Format, null), DateTime.ParseExact(txtActiveTo.Text.ToString(), CalendarExtender2.Format, null), txtPCA.Text.Trim(), txtPCATitle.Text.Trim(), txtIndexCode.Text.Trim(), txtIndexTitle.Text.Trim(), false,txtCompCode.Text.Trim());
                }
                UIHelper.SetSuccessMessage("Office added successfully");
            }
            if (Mode == OfficeManagementItemCtrlMode.Edit)
            {
                //Updating the user
                long lUserId = Convert.ToInt64(hfOfficeId.Value);
                Office objOffice = Office.GetByOfficeID(lUserId);
                objOffice.Name = txtOfficeName.Text.Trim();
                objOffice.ActiveFrom = Convert.ToDateTime(txtActiveFrom.Text.Trim());
                if (!String.IsNullOrEmpty(txtActiveTo.Text.Trim()))
                {
                    objOffice.ActiveTo = Convert.ToDateTime(txtActiveTo.Text.Trim());
                }
                else
                {
                    objOffice.ActiveTo = null;
                }
                objOffice.IndexCode = txtIndexCode.Text.Trim();
                objOffice.IndexTitle = txtIndexTitle.Text.Trim();
                objOffice.PCA = txtPCA.Text.Trim();
                objOffice.PCATitle = txtPCATitle.Text.Trim();
                objOffice.CompCode = txtCompCode.Text.Trim();
                objOffice.Update();
                UIHelper.SetSuccessMessage("Office updated successfully");
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
            Mode = OfficeManagementItemCtrlMode.NotSet;
            _HidePopup();

            //Raising the CancelClick event
            if (CancelClick != null)
            {
                CancelClick(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Custom Validator Validation Events

        protected void cvalOfficeName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            if (string.IsNullOrEmpty(txtOfficeName.Text.Trim()))
            {
                cv.ErrorMessage = "Office Name should not be empty";
                args.IsValid = false;
                return;
            }
        }

        protected void cvalActiveTo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            if (!string.IsNullOrEmpty(txtActiveTo.Text))
            {
                DateTime dtActiveFromDate = new DateTime();
                DateTime dtActiveToDate = new DateTime();
                if (!DateTime.TryParse(txtActiveTo.Text.Trim(), out dtActiveFromDate))
                {
                    cv.ErrorMessage = "Please enter a valid date for Active To";
                    args.IsValid = false;
                    return;
                }
                if (!string.IsNullOrEmpty(txtActiveFrom.Text.Trim()) && !string.IsNullOrEmpty(txtActiveTo.Text.Trim()))
                {
                    if (DateTime.TryParse(txtActiveFrom.Text.Trim(), out dtActiveFromDate) && DateTime.TryParse(txtActiveTo.Text.Trim(), out dtActiveToDate))
                    {
                        if (dtActiveFromDate > dtActiveToDate)
                        {
                            cv.ErrorMessage = "Active From Date should be before Active To Date.";
                            args.IsValid = false;
                            return;
                        }
                    }
                }
                if (Mode == OfficeManagementItemCtrlMode.Edit)
                {
                    Office objOffice = Office.GetByOfficeID(Convert.ToInt64(hfOfficeId.Value));
                    List<Expenditure> expenditures = Expenditure.GetAll(String.Empty, String.Format("<officeids><officeid>{0}</officeid></officeids>", objOffice.OfficeID.ToString()), null, String.Empty, null, -1, null, Core.NPSCommon.Enums.SortFields.ExpenditureSortFields.DateOfTransaction, Core.NPSCommon.Enums.OrderByDirection.Descending).Items.ConvertAll(q => (Expenditure)q);
                    if (expenditures.Count > 0)
                    {
                        DateTime maxDate = expenditures[0].DateOfTransaction;
                        if (DateTime.Compare(Convert.ToDateTime(txtActiveTo.Text.Trim()), maxDate) < 0)
                        {
                            cv.ErrorMessage = String.Format("Active To Date should be after the Latest Date Of Transaction ({0}) for this office", maxDate.Date.ToShortDateString());
                            args.IsValid = false;
                            return;
                        }
                    }
                }
            }
        }
        protected void cvalActiveFrom_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            if (string.IsNullOrEmpty(txtActiveFrom.Text.Trim()))
            {
                cv.ErrorMessage = "Active From should not be empty";
                args.IsValid = false;
                return;
            }
            DateTime dtDate = new DateTime();
            if (!DateTime.TryParse(txtActiveFrom.Text.Trim(), out dtDate))
            {
                cv.ErrorMessage = "Please enter a valid date for Active From";
                args.IsValid = false;
                return;
            }
            if (Mode == OfficeManagementItemCtrlMode.Edit)
            {
                Office objOffice = Office.GetByOfficeID(Convert.ToInt64(hfOfficeId.Value));
                List<Expenditure> expenditures = Expenditure.GetAll(String.Empty, String.Format("<officeids><officeid>{0}</officeid></officeids>", objOffice.OfficeID.ToString()), null, String.Empty, null, -1, null, Core.NPSCommon.Enums.SortFields.ExpenditureSortFields.DateOfTransaction, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items.ConvertAll(q => (Expenditure)q);
                if (expenditures.Count > 0)
                {
                    DateTime minDate = expenditures[0].DateOfTransaction;
                    if (DateTime.Compare(Convert.ToDateTime(txtActiveFrom.Text.Trim()), minDate) > 0)
                    {
                        cv.ErrorMessage = String.Format("Active From Date should be before the Oldest Date Of Transaction ({0}) for this office", minDate.Date.ToShortDateString());
                        args.IsValid = false;
                        return;
                    }
                }
            }
        }
        protected void cvalPCA_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            if (string.IsNullOrEmpty(txtPCA.Text.Trim()))
            {
                cv.ErrorMessage = "PCA should not be empty";
                args.IsValid = false;
                return;
            }

        }
        protected void cvalIndexCode_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            if (string.IsNullOrEmpty(txtIndexCode.Text.Trim()))
            {
                cv.ErrorMessage = "Index Code should not be empty";
                args.IsValid = false;
                return;
            }

        }

        protected void cvalCompCode_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            if (string.IsNullOrEmpty(txtCompCode.Text.Trim()))
            {
                cv.ErrorMessage = "Comp Code should not be empty";
                args.IsValid = false;
                return;
            }

        }

        #endregion
    }
}