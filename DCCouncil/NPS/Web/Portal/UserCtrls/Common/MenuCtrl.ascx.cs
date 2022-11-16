using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Common
{
    public partial class MenuCtrl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //MasterPage mstr = this.Parent.Page.Master as MasterPage;
            //DropDownList ddlFiscalYears = (DropDownList)mstr.FindControl("ctl00$HeaderCtrl1$FiscalYearSelectorCtrl1$ddlFiscalYears");
            var fiscalYear = NPSRequestContext.GetContext().FiscalYearSelected;
            if (fiscalYear!=null)
            {
                if (fiscalYear.Year>=2023) //Council was using a different PO excel file till 2022. This has changed in 2023.
                {
                    litPurchaseOrderLink.Text = "<a href=" + NPSUrls.PurchaseOrdersV2 + ">Purchase Orders</a>";
                }
                else
                {
                    litPurchaseOrderLink.Text = "<a href=" + NPSUrls.PurchaseOrders + ">Purchase Orders</a>";
                }
            }
            _BindExpenditureCategories();
        }

        private void _BindExpenditureCategories()
        {
            List<IDataHelper> lstExpenditureCategories = ExpenditureCategory.GetAll(string.Empty,-1,null,Core.NPSCommon.Enums.SortFields.ExpenditureCategorySortFields.Name,Core.NPSCommon.Enums.OrderByDirection.Ascending).Items;
            rptrExpenditureCategories.DataSource = lstExpenditureCategories;
            rptrExpenditureCategories.DataBind();
        }

        protected void rptrExpenditureCategories_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                HtmlAnchor lnkExpenditure = e.Item.FindControl("lnkExpenditure") as HtmlAnchor;
                ExpenditureCategory objItem = e.Item.DataItem as ExpenditureCategory;
                lnkExpenditure.HRef = NPSUrls.Expenditures + "?Code=" + objItem.Code;
                lnkExpenditure.InnerText = objItem.Name;
            }
        }
    }
}