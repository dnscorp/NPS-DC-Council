using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon.Enums.SortFields;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Helpers;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using PRIFACT.PRIFACTBase.MiscHelpers;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls
{
    public partial class DashboardCtrl : System.Web.UI.UserControl
    {
        #region Private Properties
        //Properties for sorting in the repeater
        private ExpenditureSummarySortField SortField
        {
            get
            {
                return EnumHelper.ParseEnum<ExpenditureSummarySortField>(hfSortField.Value);
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
        public FiscalYear FiscalYearSelected
        {
            get
            {
                return NPSRequestContext.GetContext().FiscalYearSelected;
            }
        }
        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            //The data needs to be binded here since the viewstate is disabled and we need the posted values to determine the data to be fetched.
            _SetUI();

            var fiscalYear = NPSRequestContext.GetContext().FiscalYearSelected;
            if (fiscalYear != null)
            {
                if (fiscalYear.Year >= 2023) //Council was using a different PO excel file till 2022. This has changed in 2023.
                {
                    litPurchaseOrderLink.Text = "<a href=" + PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.PurchaseOrdersV2 + "><div class='thumbnail'><img alt='' src='/images/po.jpg'><p>Purchase Orders</p></div></a>";
                }
                else
                {
                    litPurchaseOrderLink.Text = "<a href=" + PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.PurchaseOrders + "><div class='thumbnail'><img alt='' src='/images/po.jpg'><p>Purchase Orders</p></div></a>";
                }
            }
                
            _BindData(txtSearch.Text.Trim(), PagerCtrl1.PageSize, PagerCtrl1.CurrentPage, SortField, OrderByDirection, rdoOptions.SelectedValue);
            txtSearch.Attributes.Add("onchange", string.Format("javascript:doButtonClick('{0}');", bttn.ClientID));
        }

        private void _SetUI()
        {
            txtSearch.Attributes.Add("placeholder", "Search");
        }
        #endregion

        #region Search Header Events
        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //No need to do anything here since the data is bound on PageLoad            
        }
        #endregion

        protected void rptrResult_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Literal litOfficeName = e.Item.FindControl("litOfficeName") as Literal;
                Literal litBudgetAmount = e.Item.FindControl("litBudgetAmount") as Literal;
                Literal litExpenditureAmount = e.Item.FindControl("litExpenditureAmount") as Literal;
                Literal litBurnRate1 = e.Item.FindControl("litBurnRate1") as Literal;
                Literal litBurnRate2 = e.Item.FindControl("litBurnRate2") as Literal;
                HtmlGenericControl divPercBar = e.Item.FindControl("divPercBar") as HtmlGenericControl;

                ExpenditureSummary objExpenditureSummary = e.Item.DataItem as ExpenditureSummary;
                litOfficeName.Text = objExpenditureSummary.Office.Name;
                litBudgetAmount.Text = UIHelper.GetAmountInDefaultFormat(objExpenditureSummary.TotalBudgetAmount);
                litExpenditureAmount.Text = UIHelper.GetAmountInDefaultFormat(objExpenditureSummary.TotalExpenditureAmount);
                double dblBurnRate = Math.Round(objExpenditureSummary.BurnRate, MidpointRounding.AwayFromZero);
                litBurnRate1.Text = dblBurnRate.ToString() + "%";
                litBurnRate2.Text = dblBurnRate.ToString() + "%";
                divPercBar.Attributes.Add("style", "width:" + dblBurnRate.ToString() + "%");
            }
        }

        #region Paging and Sorting Events
        //Binding the data based on page number
        protected void PagerCtrl1_BindMainRepeater(object sender, EventArgs e)
        {
            _BindData(txtSearch.Text.Trim(), PagerCtrl1.PageSize, PagerCtrl1.CurrentPage, SortField, OrderByDirection, rdoOptions.SelectedValue);
        }

        //Sorting done here
        protected void rptrResult_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ExpenditureSummarySortField selectedSortField = EnumHelper.ParseEnum<ExpenditureSummarySortField>(e.CommandName);
            if (selectedSortField == SortField)
            {
                _SwitchOrderByDirection();
                _BindData(txtSearch.Text.Trim(), PagerCtrl1.PageSize, PagerCtrl1.CurrentPage, SortField, OrderByDirection, rdoOptions.SelectedValue);
            }
            else
            {
                SortField = selectedSortField;
                OrderByDirection = Core.NPSCommon.Enums.OrderByDirection.Ascending;
                _BindData(txtSearch.Text.Trim(), PagerCtrl1.PageSize, PagerCtrl1.CurrentPage, SortField, OrderByDirection, rdoOptions.SelectedValue);
            }
        }
        #endregion

        #region Private Methods

        //Binding the data to repeater initially
        private void _InitializeRepeater()
        {
            PagerCtrl1.CurrentPageIndex = 0;
            //if (rdoOptions.SelectedValue == "0")
            //    ltlSearchHeader.Text = "Non-Personal Spending";
            //else if (rdoOptions.SelectedValue == "1")
            //    ltlSearchHeader.Text = "Training Expense";
            //else if (rdoOptions.SelectedValue == "2")
            //    ltlSearchHeader.Text = "Non-Personal & Training Spending";

            _BindData(txtSearch.Text.Trim(), PagerCtrl1.PageSize, 1, SortField, OrderByDirection,rdoOptions.SelectedValue);
        }

        //Bind the data to the repeater based on the page selected and sort order
        private void _BindData(string strSearchText, int? iPageSize, int? iPageNumber, ExpenditureSummarySortField sortField, OrderByDirection orderByDirection,string filter)
        {
            ResultInfo objResultInfo = null;
            objResultInfo = ExpenditureSummary.SearchByFiscalYearId(strSearchText, FiscalYearSelected.FiscalYearID, iPageSize, iPageNumber, sortField, orderByDirection,filter);

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
        #endregion

        protected void rdoOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            _InitializeRepeater();
        }
    }
}