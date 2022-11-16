using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.ViewModels;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls
{
    public partial class DashboardV2Ctrl : System.Web.UI.UserControl
    {
        #region Private Properties
        //Properties for sorting in the repeater
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
            //Council was using a different PO excel file till 2022. This has changed in 2023.
            if (FiscalYearSelected != null && FiscalYearSelected.Year >= 2023)
            {
                litPurchaseOrderLink.Text = "<a href=" + PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.PurchaseOrdersV2 + "><div class='thumbnail'><img alt='' src='/images/po.jpg'><p>Purchase Orders</p></div></a>";
            }
            else
            {
                litPurchaseOrderLink.Text = "<a href=" + PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.PurchaseOrders + "><div class='thumbnail'><img alt='' src='/images/po.jpg'><p>Purchase Orders</p></div></a>";
            }
                
            _BindData(rdoOptions.SelectedValue);            
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

                var objExpenditureSummary = e.Item.DataItem as DashboardViewModel;
                litOfficeName.Text = objExpenditureSummary.OfficeName;
                litBudgetAmount.Text = UIHelper.GetAmountInDefaultFormat(objExpenditureSummary.BudgetAmount);
                litExpenditureAmount.Text = UIHelper.GetAmountInDefaultFormat(objExpenditureSummary.TotalExpense);
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
            _BindData(rdoOptions.SelectedValue);
        }

        //Sorting done here
        protected void rptrResult_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
                _BindData(rdoOptions.SelectedValue);
        }
        #endregion

        #region Private Methods

        //Binding the data to repeater initially
        private void _InitializeRepeater()
        {
            //PagerCtrl1.CurrentPageIndex = 0;
            _BindData(rdoOptions.SelectedValue);
        }

        //Bind the data to the repeater based on the page selected and sort order
        private void _BindData(string reportType)
        {
            var objResultInfo = DashboardViewModel.GetAllByReportType(FiscalYearSelected.FiscalYearID, reportType);

            if (objResultInfo != null)
            {
                if (objResultInfo.Count > 0)
                {
                    //PagerCtrl1.Visible = true;
                    rptrResult.Visible = true;
                    litNoResults.Visible = false;

                    //Initilizing the pager control
                    //PagerCtrl1.SetPager(objResultInfo.Count);

                }
                else
                {
                    //PagerCtrl1.Visible = false;
                    litNoResults.Visible = true;
                    litNoResults.Text = "No results found.";
                }


                //Binding the data
                rptrResult.DataSource = objResultInfo;
                rptrResult.DataBind();
            }
        }        
        #endregion

        protected void rdoOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            _InitializeRepeater();
        }
    }
}