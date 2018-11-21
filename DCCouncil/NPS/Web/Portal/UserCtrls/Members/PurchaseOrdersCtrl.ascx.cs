using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.SortFields;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.UserCtrlModes;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Web.Portal.Pages.Members;
using PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Common;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using PRIFACT.PRIFACTBase.MiscHelpers;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Members
{
    public partial class    PurchaseOrdersCtrl : System.Web.UI.UserControl
    {
        #region Private Properties
        //Properties for sorting in the repeater
        private PurchaseOrderSortField SortField
        {
            get
            {
                return EnumHelper.ParseEnum<PurchaseOrderSortField>(hfSortField.Value);
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

        #region Public Properties

        public PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.PurchaseOrder PurchaseOrder
        {
            get;
            set;
        }

        public ExpenditureCategory ExpenditureCategory
        {
            get;
            set;
        }

        public FiscalYear FiscalYearSelected
        {
            get
            {
                return NPSRequestContext.GetContext().FiscalYearSelected;
            }
        }

        #endregion

        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            //The data needs to be binded here since the viewstate is disabled and we need the posted values to determine the data to be fetched.
            _BindData(txtSearch.Text.Trim(), PagerCtrl1.PageSize, PagerCtrl1.CurrentPage, SortField, OrderByDirection);
            PurchaseOrderItemCtrl1.PurchaseOrder = PurchaseOrder;
            txtSearch.Attributes.Add("onchange", string.Format("javascript:doButtonClick('{0}');", bttn.ClientID));
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            //Setting the success message if present in Immediate Session
            _SetMessage();
            
            _SetUI();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _LoadOffices();
        }

        private void _SetUI()
        {
            litCreateLink.Text = "Create";
            litPageHeading.Text = "Purchase Orders";
            txtSearch.Attributes.Add("Placeholder", "Search");
        }

        #endregion

        #region Search Header Events
        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //No need to do anything here since the data is bound on PageLoad            
        }

        protected void lnkCreate_ServerClick(object sender, EventArgs e)
        {
            _ShowItemPopup(PurchaseOrderItemCtrlMode.Create, null);
        }

        #endregion

        #region Repeater Events
        protected void lnkEdit_ServerClick(object sender, EventArgs e)
        {
            HtmlAnchor lnkEdit = (HtmlAnchor)sender;
            long lPurchaseOrderId = Convert.ToInt64(lnkEdit.Attributes["PurchaseOrderId"]);
            PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.PurchaseOrder objPurchaseOrder = PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.PurchaseOrder.GetByPurchaseID(lPurchaseOrderId);
            _ShowItemPopup(PurchaseOrderItemCtrlMode.Edit, objPurchaseOrder);
        }

        protected void lnkDelete_ServerClick(object sender, EventArgs e)
        {
            HtmlAnchor lnkDelete = (HtmlAnchor)sender;
            PopupCtrl1.SetProperties(Convert.ToInt64(lnkDelete.Attributes["PurchaseOrderId"]).ToString(), true, "Delete", "Cancel", "Confirm Delete", "Are you sure you want to Delete the selected purchaseorder?", "DeletePurchaseorder");
            PopupCtrl1.Show();
        }

        protected void rptrResult_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                LinkButton lBtnDateOfTransaction = e.Item.FindControl("lBtnDateOfTransaction") as LinkButton;
                LinkButton lBtnVendorName = e.Item.FindControl("lBtnVendorName") as LinkButton;
                LinkButton lBtnUniquId = e.Item.FindControl("lBtnUniquId") as LinkButton;
                //lBtnDateOfTransaction.Text = ExpenditureCategory.GetAttribute("DateOfTransactionHeader");
                //lBtnVendorName.Text = ExpenditureCategory.GetAttribute("VendorNameHeader");
                
              
            }
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Literal litDateOfTransaction = e.Item.FindControl("litDateOfTransaction") as Literal;
                Literal litVendorName = e.Item.FindControl("litVendorName") as Literal;
                Literal litObjCode = e.Item.FindControl("litObjCode") as Literal;
                Literal litPONumber = e.Item.FindControl("litPONumber") as Literal;
                Literal litPOAmtSum = e.Item.FindControl("litPOAmtSum") as Literal;
                Literal litPOAdjAmtSum = e.Item.FindControl("litPOAdjAmtSum") as Literal;
                Literal litVoucherAmtSum = e.Item.FindControl("litVoucherAmtSum") as Literal;
                Literal litPOBalSum = e.Item.FindControl("litPOBalSum") as Literal;
                Literal litName = e.Item.FindControl("litName") as Literal;
                Literal litYear = e.Item.FindControl("litYear") as Literal;
                Literal litDescription = e.Item.FindControl("litDescription") as Literal;

                PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.PurchaseOrder objPurchaseOrder = e.Item.DataItem as PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.PurchaseOrder;
                litDateOfTransaction.Text = UIHelper.GetDateTimeInDefaultFormat(objPurchaseOrder.DateOfTransaction);
                litVendorName.Text = objPurchaseOrder.VendorName;
                litObjCode.Text = objPurchaseOrder.OBJCode;
                litPONumber.Text = objPurchaseOrder.PONumber;
                litPOAmtSum.Text = UIHelper.GetAmountInDefaultFormat(objPurchaseOrder.POAmtSum);
                litPOAdjAmtSum.Text = UIHelper.GetAmountInDefaultFormat(objPurchaseOrder.POAdjAmtSum);
                litVoucherAmtSum.Text = UIHelper.GetAmountInDefaultFormat(objPurchaseOrder.VoucherAmtSum);
                litPOBalSum.Text = UIHelper.GetAmountInDefaultFormat(objPurchaseOrder.POBalSum);
                litName.Text = objPurchaseOrder.Office.Name;
                litYear.Text = objPurchaseOrder.FiscalYear.Year.ToString();
                litDescription.Text = objPurchaseOrder.PurchaseOrderDescription.DescriptionText;
                              
                HtmlAnchor lnkEdit = e.Item.FindControl("lnkEdit") as HtmlAnchor;
                HtmlAnchor lnkDelete = e.Item.FindControl("lnkDelete") as HtmlAnchor;
                lnkEdit.Attributes.Add("PurchaseOrderId", objPurchaseOrder.PurchaseOrderID.ToString());
                lnkDelete.Attributes.Add("PurchaseOrderId", objPurchaseOrder.PurchaseOrderID.ToString());
            }
        }
        #endregion

        #region Paging and Sorting Events
        //Binding the data based on page number
        protected void PagerCtrl1_BindMainRepeater(object sender, EventArgs e)
        {
            _BindData(txtSearch.Text.Trim(), PagerCtrl1.PageSize, PagerCtrl1.CurrentPage, SortField, OrderByDirection);
        }

        //Sorting done here
        protected void rptrResult_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            PurchaseOrderSortField selectedSortField = EnumHelper.ParseEnum<PurchaseOrderSortField>(e.CommandName);
            if (selectedSortField == SortField)
            {
                _SwitchOrderByDirection();
                _BindData(txtSearch.Text.Trim(), PagerCtrl1.PageSize, PagerCtrl1.CurrentPage, SortField, OrderByDirection);
            }
            else
            {
                SortField = selectedSortField;
                OrderByDirection = Core.NPSCommon.Enums.OrderByDirection.Ascending;
                _BindData(txtSearch.Text.Trim(), PagerCtrl1.PageSize, PagerCtrl1.CurrentPage, SortField, OrderByDirection);
            }
        }
        #endregion

        #region Confirmation Popup Events
        protected void PopupCtrl1_OkayButtonClick(object sender, EventArgs e)
        {
            PopupCtrl popUpCtrl = (PopupCtrl)sender;
            long lPurchaseOrderId = Convert.ToInt64(popUpCtrl.IdToProcess);
            if (popUpCtrl.Mode.Equals("DeletePurchaseorder"))
            {
                PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.PurchaseOrder objPurchaseOrder = PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.PurchaseOrder.GetByPurchaseID(lPurchaseOrderId);
                objPurchaseOrder.IsDeleted = true;
                objPurchaseOrder.Update();
                UIHelper.SetSuccessMessage("Purchaseorder deleted successfully");
            }
            _ResetAll();
        }

        protected void PopupCtrl1_CancelButtonClick(object sender, EventArgs e)
        {
            //Raised when cancel is clicked on the confirmation popup
            PopupCtrl1.Hide();
        }
        #endregion

        #region Item Popup Events
        //TODO
        //Once the data is updated inside the child item popup, the main management control needs to be refreshed
        protected void PurchaseOrderItemCtrl1_SubmitClick(object sender, EventArgs e)
        {
            //Things to be done after Submit is clicked in the child item popup
            _ResetAll();
        }

        protected void PurchaseOrderItemCtrl1_CancelClick(object sender, EventArgs e)
        {
            //Things to be done after Cance is clicked in the child item popup
            _CloseItemPopup();
        }
        #endregion

        #region Private Methods

        //Binding the data to repeater initially
        private void _InitializeRepeater()
        {
            PagerCtrl1.CurrentPageIndex = 0;
            _BindData(txtSearch.Text.Trim(), PagerCtrl1.PageSize, 1, SortField, OrderByDirection);
        }

        private void _LoadOffices()
        {
            ddlOfficeFilter.Items.Clear();
            List<Office> lstOffices = Office.GetAll(string.Empty, NPSRequestContext.GetContext().FiscalYearSelected.FiscalYearID, -1, null, OfficeSortFields.Name, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items.ConvertAll(q => (Office)q);

            foreach (Office objOffice in lstOffices)
            {
                ListItem lstItem = new ListItem(objOffice.Name, objOffice.OfficeID.ToString());
                ddlOfficeFilter.Items.Add(lstItem);
            }
            ListItem lstItemAll = new ListItem("All", "0");
            ddlOfficeFilter.Items.Insert(0, lstItemAll);
        }

        //Bind the data to the repeater based on the page selected and sort order
        private void _BindData(string strSearchText, int? iPageSize, int? iPageNumber, PurchaseOrderSortField sortField, OrderByDirection orderByDirection)
        {
            ResultInfo objResultInfo = null;

            if (ddlOfficeFilter.SelectedValue != "0")
            {
                objResultInfo = PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.PurchaseOrder.GetAll(strSearchText, Convert.ToInt64(ddlOfficeFilter.SelectedValue), FiscalYearSelected.FiscalYearID, null, iPageSize, iPageNumber, sortField, orderByDirection);
            }
            else
                objResultInfo = PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.PurchaseOrder.GetAll(strSearchText, null, FiscalYearSelected.FiscalYearID, null, iPageSize, iPageNumber, sortField, orderByDirection);

            if (objResultInfo != null)
            {
                if (objResultInfo.Items.Count > 0)
                {
                    PagerCtrl1.Visible = true;
                    rptrResult.Visible = true;
                    litNoResults.Visible = false;

                    //Initilizing the pager control
                    PagerCtrl1.SetPager(objResultInfo.RowCount);

                    
                }
                else
                {
                    PagerCtrl1.Visible = false;
                    litNoResults.Visible = true;
                    litNoResults.Text = "No results found.";
                }
                //Binding the data
                rptrResult.DataSource = objResultInfo.Items;
                rptrResult.DataBind();
            }
        }
        //Reset all the fields and rebind data
        private void _ResetAll()
        {
            txtSearch.Text = string.Empty;
            _InitializeRepeater();
            upSearchResults.Update();
            _CloseItemPopup();
            PopupCtrl1.Hide();
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

        private void _SetMessage()
        {
            string message = UIHelper.GetSuccessMessage();
            if (!String.IsNullOrEmpty(message))
            {
                litSuccessMessage.Visible = true;
                litSuccessMessage.Text = message;
            }
            else
            {
                litSuccessMessage.Visible = false;
            }
            upMessage.Update();
        }
        #region Show and Hide Popup
        private void _ShowItemPopup(PurchaseOrderItemCtrlMode purchaseOrderItemCtrlMode, PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.PurchaseOrder objPurchaseOrder)
        {
            PopupCtrl1.Visible = false;
            PurchaseOrderItemCtrl1.Mode = purchaseOrderItemCtrlMode;
            PurchaseOrderItemCtrl1.InitializeFields(objPurchaseOrder);
            upItemPopup.Update();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowPopup", "ShowPopup('" + PurchaseOrderItemCtrl1.ItemPopupClientID + "');", true);
        }
        private void _CloseItemPopup()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ClosePopup", "ClosePopup('" + PurchaseOrderItemCtrl1.ItemPopupClientID + "');", true);
        }
        #endregion
        #endregion    
    }
}