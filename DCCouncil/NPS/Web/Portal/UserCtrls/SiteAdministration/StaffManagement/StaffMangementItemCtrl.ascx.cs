using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.UserCtrlModes;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using PRIFACT.PRIFACTBase.UrlHelpers;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.SiteAdministration.StaffManagement
{
    public partial class StaffMangementItemCtrl : System.Web.UI.UserControl
    {

        #region Public Events
        public event EventHandler SubmitClick;
        public event EventHandler CancelClick;
        #endregion

        #region Public Properties
        public StaffManagementItemCtrlMode Mode
        {
            get
            {
                if (string.IsNullOrEmpty(hfMode.Value))
                {
                    return StaffManagementItemCtrlMode.NotSet;
                }
                else
                {
                    return (StaffManagementItemCtrlMode)Convert.ToInt32(hfMode.Value);
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
            hfOfficeId.Value = UrlUtility.GetStringFromQv(Request["OfficeId"]);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            _SetUI();
        }
        #endregion

        #region Public Setter and Getter Methods
        public void InitializeFields(Staff objStaff)
        {
            if (objStaff != null)
            {
                hfStaffId.Value = objStaff.StaffId.ToString();
                txtStaffFirstName.Text = objStaff.FirstName;
                txtStaffLastName.Text = objStaff.LastName;
                chkHasStaffLevelExpenditures.Checked = objStaff.HasStaffLevelExpenditures;
                txtActiveFrom.Text = objStaff.ActiveFrom.ToShortDateString();
                txtWirelessNumber.Text = objStaff.WirelessNumber;
                if (objStaff.ActiveTo.HasValue)
                    txtActiveTo.Text = objStaff.ActiveTo.Value.ToShortDateString();
            }
            else
            {
                txtStaffFirstName.Text = string.Empty;
                txtStaffLastName.Text = string.Empty;
                chkHasStaffLevelExpenditures.Checked = false;
                txtActiveFrom.Text = string.Empty;
                txtActiveTo.Text = string.Empty;
                txtWirelessNumber.Text = string.Empty;
            }
        }
        #endregion

        #region Private Methods
        private void _SetUI()
        {
            if (Mode == StaffManagementItemCtrlMode.Create)
            {
                _ShowPopup();
                lnkSubmit.InnerText = "Create";
            }
            if (Mode == StaffManagementItemCtrlMode.Edit)
            {
                _ShowPopup();
                lnkSubmit.InnerText = "Update";
            }
            if (Mode == StaffManagementItemCtrlMode.NotSet)
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

            if (Mode == StaffManagementItemCtrlMode.Create)
            {
                //Creating the user
                if ((String.IsNullOrEmpty(txtActiveTo.Text.Trim())))
                {
                    Staff.Create(txtStaffFirstName.Text.Trim(), txtStaffLastName.Text.Trim(), chkHasStaffLevelExpenditures.Checked, Convert.ToInt64(hfOfficeId.Value), DateTime.ParseExact(txtActiveFrom.Text, CalendarExtender1.Format, null), null, false, txtWirelessNumber.Text.Trim());
                }
                else
                {
                    Staff.Create(txtStaffFirstName.Text.Trim(), txtStaffLastName.Text.Trim(), chkHasStaffLevelExpenditures.Checked, Convert.ToInt64(hfOfficeId.Value), DateTime.ParseExact(txtActiveFrom.Text, CalendarExtender1.Format, null), DateTime.ParseExact(txtActiveTo.Text.Trim(), CalendarExtender2.Format, null), false, txtWirelessNumber.Text.Trim());
                }
                UIHelper.SetSuccessMessage("Staff created successfully");
            }
            if (Mode == StaffManagementItemCtrlMode.Edit)
            {
                //Updating the user
                long lngStaffId = Convert.ToInt64(hfStaffId.Value);
                Staff objStaff = Staff.GetByStaffID(lngStaffId);
                objStaff.FirstName = txtStaffFirstName.Text.Trim();
                objStaff.LastName = txtStaffLastName.Text.Trim();
                objStaff.WirelessNumber = txtWirelessNumber.Text.Trim();
                objStaff.HasStaffLevelExpenditures = chkHasStaffLevelExpenditures.Checked;
                objStaff.ActiveFrom = DateTime.ParseExact(txtActiveFrom.Text.Trim(), CalendarExtender1.Format, null);
                if (!String.IsNullOrEmpty(txtActiveTo.Text.Trim()))
                    objStaff.ActiveTo = DateTime.ParseExact(txtActiveTo.Text.Trim(), CalendarExtender2.Format, null);
                else objStaff.ActiveTo = null;
                objStaff.Update();
                UIHelper.SetSuccessMessage("Staff updated successfully");
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
            Mode = StaffManagementItemCtrlMode.NotSet;
            _HidePopup();

            //Raising the CancelClick event
            if (CancelClick != null)
            {
                CancelClick(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Custom Validator Validation Events

        protected void cvalStaffFirstName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            if (string.IsNullOrEmpty(txtStaffFirstName.Text.Trim()))
            {
                cv.ErrorMessage = "First Name should not be empty";
                args.IsValid = false;
                return;
            }
        }

        protected void cvalStaffLastName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            if (string.IsNullOrEmpty(txtStaffLastName.Text.Trim()))
            {
                cv.ErrorMessage = "Last Name should not be empty";
                args.IsValid = false;
                return;
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
            Office objOffice = Office.GetByOfficeID(Convert.ToInt64(hfOfficeId.Value));
            if (DateTime.Compare(Convert.ToDateTime(txtActiveFrom.Text.Trim()), objOffice.ActiveFrom) < 0)
            {
                cv.ErrorMessage = String.Format("Active From Date of Staff should be after the Active From Date of Office ({0})", objOffice.ActiveFrom.Date.ToShortDateString());
                args.IsValid = false;
                return;
            }
        }
        protected void cvalWirelessNumber_ServerValidate(object source, ServerValidateEventArgs e)
        {
            CustomValidator cv = (CustomValidator)source;
            if (!string.IsNullOrEmpty(txtWirelessNumber.Text.Trim()))
            {
                Int64 result;
                if (!Int64.TryParse(txtWirelessNumber.Text.Trim(), out result))
                {
                    cv.ErrorMessage = "Please enter valid wireless number";
                    e.IsValid = false;
                    return;
                }

                List<IDataHelper> lstStaffs = Staff.GetAllStaffs().Items;
                List<Staff> lstOtherStaffs = new List<Staff>();
                if (Mode == StaffManagementItemCtrlMode.Create)
                {
                    lstOtherStaffs = lstStaffs.ConvertAll(q => (Staff)q);
                }
                if (Mode == StaffManagementItemCtrlMode.Edit)
                {
                    long lSelectedStaffId = Convert.ToInt64(hfStaffId.Value);
                    lstOtherStaffs = lstStaffs.ConvertAll(q => (Staff)q).Where(q => q.StaffId != lSelectedStaffId).ToList();
                }
                foreach (Staff objStaff in lstOtherStaffs)
                    if (objStaff.WirelessNumber == txtWirelessNumber.Text.Trim())
                    {
                        cv.ErrorMessage = "This number is already assigned to another user with username " + objStaff.FirstName + " " + objStaff.LastName;
                        e.IsValid = false;
                        return;
                    }
            }

        }
        protected void cvalActiveTo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            if (!string.IsNullOrEmpty(txtActiveTo.Text.Trim()))
            {
                DateTime dtActiveFromDate = new DateTime();
                DateTime dtActiveToDate = new DateTime();
                if (!DateTime.TryParse(txtActiveTo.Text.Trim(), out dtActiveFromDate))
                {
                    cv.ErrorMessage = "Please enter a valid date for Active To";
                    args.IsValid = false;
                    return;
                }
                if (!string.IsNullOrEmpty(txtActiveFrom.Text.Trim()))
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
                Office objOffice = Office.GetByOfficeID(Convert.ToInt64(hfOfficeId.Value));
                if (objOffice.ActiveTo.HasValue)
                {
                    if (DateTime.Compare(Convert.ToDateTime(txtActiveTo.Text.Trim()), objOffice.ActiveTo.Value) > 0)
                    {
                        cv.ErrorMessage = String.Format("Active To Date of Staff should be before the Active To Date of Office ({0})", objOffice.ActiveTo.Value.ToShortDateString());
                        args.IsValid = false;
                        return;
                    }
                }
            }
        }

        #endregion

    }
}