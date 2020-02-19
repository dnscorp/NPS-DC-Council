using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.UserCtrlModes;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.SortFields;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Members
{
    public partial class PurchaseOrderItemCtrl : System.Web.UI.UserControl
    {
        #region Public Events
        public event EventHandler SubmitClick;
        public event EventHandler CancelClick;
        #endregion

        #region Public Properties
        public PurchaseOrderItemCtrlMode Mode
        {
            get
            {
                if (string.IsNullOrEmpty(hfMode.Value))
                {
                    return PurchaseOrderItemCtrlMode.NotSet;
                }
                else
                {
                    return (PurchaseOrderItemCtrlMode)Convert.ToInt32(hfMode.Value);
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
        public PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.PurchaseOrder PurchaseOrder
        {
            get;
            set;
        }
        public ExpenditureSubCategory ExpenditureSubCategory
        {
            get;
            set;
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

        protected override void OnInit(EventArgs e)
        {
            //_LoadBudgets(null, null);
            _LoadOffices();
            _LoadExpenditureSubCategory();
            base.OnInit(e);
        }

        #endregion

        private void _LoadExpenditureSubCategory()
        {
            ddlExpenditureSubCategory.Items.Clear();
            List<ExpenditureSubCategory> lstExpenditureSubCategories = ExpenditureSubCategory.GetAllExpenditureSubCategories().Items.ConvertAll(q => (ExpenditureSubCategory)q);

            foreach (ExpenditureSubCategory expenditureSubCategory in lstExpenditureSubCategories)
            {
                ListItem lstItem = new ListItem(expenditureSubCategory.Name, expenditureSubCategory.ExpenditureSubCategoryID.ToString());
                ddlExpenditureSubCategory.Items.Add(lstItem);
            }
            ListItem lstItemAll = new ListItem("--Select--", "0");
            ddlExpenditureSubCategory.Items.Insert(0, lstItemAll);
        }

        protected void cvalExpenditureSubCategory_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            if (ddlExpenditureSubCategory.SelectedValue == "0")
            {
                cv.ErrorMessage = "Expenditure sub-category should not be empty";
                args.IsValid = false;
                return;
            }
        }

        #region Public Setter and Getter Methods
        public void InitializeFields(PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.PurchaseOrder objPurchaseOrder)
        {
            if (objPurchaseOrder != null)
            {
                //Set the fields
                hfPurchaseOrderId.Value = objPurchaseOrder.PurchaseOrderID.ToString();
                hfFiscalYearId.Value = objPurchaseOrder.FiscalYear.FiscalYearID.ToString();


                litDateOfTransaction.Text = UIHelper.GetDateTimeInDefaultFormat(objPurchaseOrder.DateOfTransaction);
                litVendorName.Text = objPurchaseOrder.VendorName;
                litPONumber.Text = objPurchaseOrder.PONumber;
                litPOAmountSum.Text = UIHelper.GetAmountInDefaultFormat(objPurchaseOrder.POAmtSum);
                litPOAdjAmountSum.Text = UIHelper.GetAmountInDefaultFormat(objPurchaseOrder.POAdjAmtSum);
                litVoucherAmountSum.Text = UIHelper.GetAmountInDefaultFormat(objPurchaseOrder.VoucherAmtSum);
                litPOBalSum.Text = UIHelper.GetAmountInDefaultFormat(objPurchaseOrder.POBalSum);
                litOBJCode.Text = objPurchaseOrder.OBJCode;
                litOffice.Text = objPurchaseOrder.Office.Name;
                txtDescription.Text = objPurchaseOrder.PurchaseOrderDescription.DescriptionText;
                //_LoadBudgets(objPurchaseOrder.Office.OfficeID, objPurchaseOrder.FiscalYear.FiscalYearID);
                //ddlBudgets.SelectedValue = objPurchaseOrder.Budget.BudgetID.ToString();
                _LoadOffices_EditPO();
                ddlAlternateOffice.SelectedValue = objPurchaseOrder.AlternateOfficeID.ToString();
                //ddlAlternateOffice.SelectedValue = "47";
                ddlExpenditureSubCategory.SelectedValue = objPurchaseOrder.ExpenditureSubCategoryId.ToString();
                chkTrainingExpense.Checked = objPurchaseOrder.IsTrainingExpense;
            }
            else
            {
                //Initialise the fields
                hfPurchaseOrderId.Value = string.Empty;
                User objUser = NPSRequestContext.GetContext().LoggedInUser;
                hfFiscalYearId.Value = objUser.LastFiscalYearSelected.FiscalYearID.ToString();
                
                //txtDateOfTransaction.Text = string.Empty;
                //txtVendorName.Text = string.Empty;
                //txtPONumber.Text = string.Empty;
                //txtPOAmountSum.Text = string.Empty;
                //txtPOAdjAmountSum.Text = string.Empty;
                //txtVoucherAmountSum.Text = string.Empty;
                //txtPOBalSum.Text = string.Empty;

                //txtOBJCode.Text = string.Empty;
                //ddlOffices.SelectedValue = "-1";
                //ddlBudgets.SelectedValue = "-1";



            }


        }


        #endregion
        #region Private Methods

        private void _LoadOffices()
        {
            long lFiscalYearId = NPSRequestContext.GetContext().FiscalYearSelected.FiscalYearID;
            List<IDataHelper> lstOffices = Office.GetAll(string.Empty, lFiscalYearId, -1, null, Core.NPSCommon.Enums.SortFields.OfficeSortFields.Name, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items;
            //ddlOffices.Items.Clear();
            //ddlOffices.DataSource = lstOffices;
            //ddlOffices.DataTextField = "Name";
            //ddlOffices.DataValueField = "OfficeID";
            //ddlOffices.DataBind();

            ListItem item = new ListItem("Select Office", "-1");
            //ddlOffices.Items.Insert(0, item);
            ////ddlOffices.SelectedIndex = 0;
            //trBudget.Visible = false;
        }

        private void _SetUI()
        {
            if (Mode == PurchaseOrderItemCtrlMode.Create)
            {
                _ShowPopup();
                lnkSubmit.InnerText = "Create";
                litOffice.Visible = false;
                //ddlOffices.Visible = true;
            }
            if (Mode == PurchaseOrderItemCtrlMode.Edit)
            {
                _ShowPopup();
                lnkSubmit.InnerText = "Update";
                litOffice.Visible = true;
                //ddlOffices.Visible = false;
            }
            if (Mode == PurchaseOrderItemCtrlMode.NotSet)
            {
                _HidePopup();
            }
            //CalendarExtender1.Format = AppSettings.DateFormat;
        }

        private void _HidePopup()
        {
            divItemPopup.Attributes.Add("style", "display: none");
        }

        private void _ShowPopup()
        {
            divItemPopup.Attributes.Add("style", "display:block");
        }

        //private void _LoadBudgets(long? lOfficeId, long? lFiscalYearId)
        //{
        //    ddlBudgets.Items.Clear();
        //    if (lOfficeId.HasValue)
        //    {
        //        List<IDataHelper> lstBudgets = Budget.GetAll(string.Empty, lOfficeId.Value, lFiscalYearId.Value, -1, null, Core.NPSCommon.Enums.SortFields.BudgetSortFields.Name, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items;
        //        if (lstBudgets.Count > 0)
        //        {
        //            ddlBudgets.DataSource = lstBudgets;
        //            ddlBudgets.DataTextField = "Name";
        //            ddlBudgets.DataValueField = "BudgetID";
        //            ddlBudgets.DataBind();
        //        }

        //        ListItem item = new ListItem("Select Budget", "-1");
        //        ddlBudgets.Items.Insert(0, item);
        //        ddlBudgets.SelectedIndex = 0;
        //    }
        //    else
        //    {
        //        ListItem item = new ListItem("Select Budget", "-1");
        //        ddlBudgets.Items.Insert(0, item);
        //        ddlBudgets.SelectedIndex = 0;
        //    }
        //    trBudget.Visible = true;
        //}
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

            if (Mode == PurchaseOrderItemCtrlMode.Create)
            {
                //long officeId = Convert.ToInt64(ddlOffices.SelectedValue);
                //long lBudgetId = Convert.ToInt64(ddlBudgets.SelectedValue);
                //bool bAdd = true;
                //foreach (PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.PurchaseOrder objPurchaseOrder in PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.PurchaseOrder.GetAll(String.Empty, officeId, null, null, -1, null, Core.NPSCommon.Enums.SortFields.PurchaseOrderSortField.OfficeName, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items)
                //    if (objPurchaseOrder.PONumber == txtPONumber.Text)
                //    {
                //        bAdd = false;
                //        break;
                //    }
                //if (bAdd == true)
                //{
                //    PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.PurchaseOrder.Create(txtVendorName.Text.Trim(), txtOBJCode.Text.Trim(), Convert.ToDateTime(txtDateOfTransaction.Text.Trim()), txtPONumber.Text.Trim(), Convert.ToSingle(txtPOAmountSum.Text.Trim()), Convert.ToSingle(txtPOAdjAmountSum.Text.Trim()), Convert.ToSingle(txtVoucherAmountSum.Text.Trim()), Convert.ToSingle(txtPOBalSum.Text.Trim()), officeId, lBudgetId, null, Convert.ToDouble(hfFiscalYearId.Value), false);
                //    UIHelper.SetSuccessMessage("Purchase order added successfully");
                //}
            }
            if (Mode == PurchaseOrderItemCtrlMode.Edit)
            {
                long lPurchaseOrderId = Convert.ToInt64(hfPurchaseOrderId.Value);
                PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.PurchaseOrder objPurchaseOrder = PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.PurchaseOrder.GetByPurchaseID(lPurchaseOrderId);              
                objPurchaseOrder.PurchaseOrderDescription.DescriptionText = txtDescription.Text.Trim();
                objPurchaseOrder.ExpenditureSubCategoryId = long.Parse(ddlExpenditureSubCategory.SelectedValue);
                objPurchaseOrder.AlternateOfficeID =  Convert.ToInt64(ddlAlternateOffice.Text);
                objPurchaseOrder.IsTrainingExpense = chkTrainingExpense.Checked;
                objPurchaseOrder.Update();
                UIHelper.SetSuccessMessage("Expenditure updated successfully");
            }
            //Raising the SubmitClick event
            if (SubmitClick != null)
            {
                SubmitClick(this, EventArgs.Empty);
            }

            //Hiding the popup
            Mode = PurchaseOrderItemCtrlMode.NotSet;

        }

        protected void ddlOffices_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlOffices.SelectedIndex != -1)
            //{
            //    if(ddlOffices.SelectedValue == "-1")
            //        trBudget.Visible = false;
            //    else
            //    _LoadBudgets(Convert.ToInt64(ddlOffices.SelectedValue), Convert.ToInt64(hfFiscalYearId.Value));
            //    //_BindStaffs(Convert.ToInt64(ddlOffices.SelectedValue), null);
            //}
        }
        protected void lnkCancel_ServerClick(object sender, EventArgs e)
        {
            //Hiding the popup
            Mode = PurchaseOrderItemCtrlMode.NotSet;
            _HidePopup();

            //Raising the CancelClick event
            if (CancelClick != null)
            {
                CancelClick(this, EventArgs.Empty);
            }
        }
        #endregion

        //protected void cvalOffice_ServerValidate(object source, ServerValidateEventArgs args)
        //{
        //    CustomValidator cv = (CustomValidator)source;
        //    if (ddlOffices.SelectedIndex == 0 || ddlOffices.SelectedIndex == -1)
        //    {
        //        cv.ErrorMessage = "Select an Office";
        //        args.IsValid = false;
        //        return;
        //    }
        //}
        //protected void cvalVendorName_ServerValidate(object sender, ServerValidateEventArgs args)
        //{
        //    CustomValidator cv = (CustomValidator)sender;
        //    if (string.IsNullOrEmpty(txtDateOfTransaction.Text.Trim()))
        //    {
        //        cv.ErrorMessage = "Vendor name should not be empty";
        //        args.IsValid = false;
        //        return;
        //    }
        //}
        //protected void cvalOBJCode_ServerValidate(object sender, ServerValidateEventArgs args)
        //{
        //    CustomValidator cv = (CustomValidator)sender;
        //    if (string.IsNullOrEmpty(txtOBJCode.Text.Trim()))
        //    {
        //        cv.ErrorMessage = "OBJCode should not be empty";
        //        args.IsValid = false;
        //        return;
        //    }
        //}
        //protected void cvalPONumber_ServerValidate(object sender, ServerValidateEventArgs args)
        //{
        //    CustomValidator cv = (CustomValidator)sender;
        //    if (string.IsNullOrEmpty(txtPONumber.Text.Trim()))
        //    {
        //        cv.ErrorMessage = "PONumber should not be empty";
        //        args.IsValid = false;
        //        return;
        //    }
        //    long officeId = Convert.ToInt64(ddlOffices.SelectedValue);
        //    if (Mode == PurchaseOrderItemCtrlMode.Create)
        //    {
        //        foreach (PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.PurchaseOrder objPurchaseOrder in PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.PurchaseOrder.GetAll(String.Empty, officeId, null, null, -1, null, Core.NPSCommon.Enums.SortFields.PurchaseOrderSortField.OfficeName, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items)
        //        {
        //            if (objPurchaseOrder.PONumber == txtPONumber.Text)
        //            {
        //                cv.ErrorMessage = "PONumber already exists";
        //                args.IsValid = false;
        //                return;
        //            }
        //        }
        //    }
        //    else if (Mode == PurchaseOrderItemCtrlMode.Create)
        //    {
        //        foreach (PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.PurchaseOrder objPurchaseOrder in PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.PurchaseOrder.GetAll(String.Empty, officeId, null, null, -1, null, Core.NPSCommon.Enums.SortFields.PurchaseOrderSortField.OfficeName, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items)
        //        {
        //            if (objPurchaseOrder.PONumber == txtPONumber.Text && objPurchaseOrder.PurchaseOrderID != PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.PurchaseOrder.GetByPurchaseID(Convert.ToInt64(hfPurchaseOrderId)).PurchaseOrderID)
        //            {
        //                cv.ErrorMessage = "PONumber already exists";
        //                args.IsValid = false;
        //                return;
        //            }
        //        }
        //    }
        //}

        //protected void cvalPOAmountSum_ServerValidate(object sender, ServerValidateEventArgs args)
        //{
        //    CustomValidator cv = (CustomValidator)sender;
        //    if (string.IsNullOrEmpty(txtPOAmountSum.Text.Trim()))
        //    {
        //        cv.ErrorMessage = "PO amountsum should not be empty";
        //        args.IsValid = false;
        //        return;
        //    }
        //}
        //protected void cvalPOAdjAmountSum_ServerValidate(object sender, ServerValidateEventArgs args)
        //{
        //    CustomValidator cv = (CustomValidator)sender;
        //    if (string.IsNullOrEmpty(txtPOAdjAmountSum.Text.Trim()))
        //    {
        //        cv.ErrorMessage = "PO adj amountsum should not be empty";
        //        args.IsValid = false;
        //        return;
        //    }
        //}
        //protected void cvalVoucherAmountSum_ServerValidate(object sender, ServerValidateEventArgs args)
        //{
        //    CustomValidator cv = (CustomValidator)sender;
        //    if (string.IsNullOrEmpty(txtVoucherAmountSum.Text.Trim()))
        //    {
        //        cv.ErrorMessage = "Voucher amountsum should not be empty";
        //        args.IsValid = false;
        //        return;
        //    }
        //}
        //protected void cvalPOBalanceSum_ServerValidate(object sender, ServerValidateEventArgs args)
        //{
        //    CustomValidator cv = (CustomValidator)sender;
        //    if (string.IsNullOrEmpty(txtPOBalSum.Text.Trim()))
        //    {
        //        cv.ErrorMessage = "Voucher amountsum should not be empty";
        //        args.IsValid = false;
        //        return;
        //    }
        //}
        //protected void cvalDateOfTransaction_ServerValidate(object source, ServerValidateEventArgs args)
        //{
        //    CustomValidator cv = (CustomValidator)source;
        //    if (string.IsNullOrEmpty(txtDateOfTransaction.Text.Trim()))
        //    {
        //        cv.ErrorMessage = "Date of Transaction should not be empty";
        //        args.IsValid = false;
        //        return;
        //    }
        //    DateTime dtDate = new DateTime();
        //    if (!DateTime.TryParse(txtDateOfTransaction.Text.Trim(), out dtDate))
        //    {
        //        cv.ErrorMessage = "Please enter a valid date for Date of Transaction";
        //        args.IsValid = false;
        //        return;
        //    }
        //}
        //protected void cvalBudget_ServerValidate(object source, ServerValidateEventArgs args)
        //{
        //    CustomValidator cv = (CustomValidator)source;
        //    if (ddlBudgets.SelectedIndex == 0 || ddlBudgets.SelectedIndex == -1)
        //    {
        //        cv.ErrorMessage = "Select a Budget";
        //        args.IsValid = false;
        //        return;
        //    }
        //}
        private void _LoadOffices_EditPO()
        {
            ddlAlternateOffice.Items.Clear();
            List<Office> lstOffices = Office.GetAll(string.Empty, NPSRequestContext.GetContext().FiscalYearSelected.FiscalYearID, -1, null, OfficeSortFields.Name, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items.ConvertAll(q => (Office)q);

            foreach (Office objOffice in lstOffices)
            {
                ListItem lstItem = new ListItem(objOffice.Name, objOffice.OfficeID.ToString());
                ddlAlternateOffice.Items.Add(lstItem);
            }
            ListItem lstItemAll = new ListItem("--Select--", "0");
            ddlAlternateOffice.Items.Insert(0, lstItemAll);
        }

    }
}