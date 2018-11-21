using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Common
{
    public partial class PagerCtrl : System.Web.UI.UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //rptPages.ItemCommand +=
               //new RepeaterCommandEventHandler(rptPages_ItemCommand);            
        }
        protected void rptPages_ItemCommand(object source,
                            RepeaterCommandEventArgs e)
        {
            CurrentPageIndex = e.Item.ItemIndex;
            if (BindMainRepeater != null)
                BindMainRepeater(this, e);
        }
        public int CurrentPage
        {
            get
            {
                return CurrentPageIndex + 1;
            }            
        }
        public int CurrentPageIndex
        {
            get
            {
                // look for current page in ViewState
                string strCurrentPage = hfCurrentPage.Value;
                if (string.IsNullOrEmpty(strCurrentPage))
                    return 0;   // default to showing the first page
                else
                    return Convert.ToInt32(strCurrentPage);
            }

            set
            {
                hfCurrentPage.Value = value.ToString();
            }
        }

        protected int PageCount
        {
            get
            {
                // look for current page count in ViewState
                string strPageCount = hfPageCount.Value;
                if (string.IsNullOrEmpty(strPageCount))
                    return 1;   // default to showing the first page
                else
                    return Convert.ToInt32(strPageCount);                
            }

            set
            {
                hfPageCount.Value = value.ToString();
            }
        }
        #region Event Handlers
        public event EventHandler BindMainRepeater;
        #endregion
        public int PageSize
        {
            get;
            set;
        }
        private List<int> _GeneratePagedData(int count)
        {
            List<int> lstItems = new List<int>();
            for (int i = 0; i < count; i++)
            {
                lstItems.Add(i);
            }
            return lstItems;
        }
        public void SetPager(int count)
        {
            PagedDataSource pagedData = new PagedDataSource();            
            pagedData.AllowPaging = true;
            pagedData.PageSize = PageSize;
            pagedData.DataSource = _GeneratePagedData(count);
            pagedData.CurrentPageIndex = CurrentPageIndex;


            lblCurrentPage.Text = "<b>Page:</b> " + (CurrentPageIndex + 1).ToString() + " of " + pagedData.PageCount.ToString();

            PageCount = pagedData.PageCount;

            // Disable Prev/Next First/Last buttons if necessary
            cmdPrev.Enabled = !pagedData.IsFirstPage;
            cmdFirst.Enabled = !pagedData.IsFirstPage;
            cmdNext.Enabled = !pagedData.IsLastPage;
            cmdLast.Enabled = !pagedData.IsLastPage;

            

            // Wire up the page numbers
            if (pagedData.PageCount > 1)
            {
                rptPages.Visible = true;
                ArrayList pages = new ArrayList();
                for (int i = 0; i < pagedData.PageCount; i++)
                    if (i == CurrentPageIndex)
                    {
                        pages.Add("<b>" + (i + 1).ToString() + "</b>");
                    }
                    else
                    {
                        pages.Add((i + 1).ToString());
                    }
                rptPages.DataSource = pages;
                rptPages.DataBind();
            }
            else
            {
                divPager.Attributes.Add("style", "display:none");
            }

        }
       
        protected void cmdPrev_Click(object sender, System.EventArgs e)
        {
            // Set viewstate variable to the previous page
            CurrentPageIndex -= 1;

            // Reload control
            if (BindMainRepeater != null)
                BindMainRepeater(this, e);
        }

        protected void cmdNext_Click(object sender, System.EventArgs e)
        {
            // Set viewstate variable to the next page
            CurrentPageIndex += 1;

            // Reload control
            if (BindMainRepeater != null)
                BindMainRepeater(this, e);
        }

        protected void cmdFirst_Click(object sender, System.EventArgs e)
        {
            // Set viewstate variable to the first page
            CurrentPageIndex = 0;

            // Reload control
            if (BindMainRepeater != null)
                BindMainRepeater(this, e);
        }
        protected void cmdLast_Click(object sender, System.EventArgs e)
        {
            // Set viewstate variable to the last page
            CurrentPageIndex = PageCount - 1;

            // Reload control
            if (BindMainRepeater != null)
                BindMainRepeater(this, e);
        }
        protected bool CurrentPageHighlight(int currPage)
        {
            return currPage == CurrentPageIndex ? true : false;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}