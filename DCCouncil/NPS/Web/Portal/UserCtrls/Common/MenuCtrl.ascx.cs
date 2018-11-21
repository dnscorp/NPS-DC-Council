using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
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