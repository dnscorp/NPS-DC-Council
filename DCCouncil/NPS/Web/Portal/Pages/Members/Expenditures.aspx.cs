using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.Pages.Members
{
    public partial class Expenditures : System.Web.UI.Page
    {
        public string ExpenditureCategoryCode
        {
            get
            {
                if (Request["Code"] != null)
                {
                    return Request["Code"];
                }
                else
                {
                    throw new Exception("Expenditure Category Code not specified");
                }
            }
        }
        protected override void OnInit(EventArgs e)
        {
            ExpenditureCategory expenditureCategory = ExpenditureCategory.GetByCode(ExpenditureCategoryCode);
            ExpendituresCtrl1.ExpenditureCategory = expenditureCategory;
            Page.Title = expenditureCategory.Name;
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}