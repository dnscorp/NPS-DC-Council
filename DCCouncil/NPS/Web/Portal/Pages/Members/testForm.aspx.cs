using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper.Interfaces;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using PRIFACT.PRIFACTBase.SQLHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.Pages.Members
{
    public partial class testForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _LoadOffices();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //INSERT BLOCK
            new SafeDBExecute<bool>(delegate(DBContext dbContext)
            {
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_test_Create";

                //param = cmd.Parameters.Add("@ExpenditureCategoryId", SqlDbType.BigInt);
                //param.Value = BasicConverter.LongToDbValue(expenditureCategoryId);

                //param = cmd.Parameters.Add("@VendorName", SqlDbType.NVarChar);
                //param.Value = BasicConverter.StringToDbValue(strVendorName);

                //param = cmd.Parameters.Add("@DateOfTransaction", SqlDbType.DateTime);
                //param.Value = BasicConverter.DateToDbValue(dtDateOfTransaction);

                //param = cmd.Parameters.Add("@Amount", SqlDbType.Float);
                //param.Value = dblAmount;

                //param = cmd.Parameters.Add("@IsDeleted", SqlDbType.Bit);
                //param.Value = BasicConverter.BoolToDbValue(blnIsDeleted);

                //param = cmd.Parameters.Add("@StaffLevelExpenditures", SqlDbType.Xml);
                //param.Value = BasicConverter.StringToDbValue(strStaffLevelExpenditureXml);

                cmd.ExecuteNonQuery();

                return true;
            }
               ).DoExecute(GetDbConnectionString());

            //UPDATE BLOCK
            new SafeDBExecute<bool>(delegate(DBContext dbContext)
            {
                SqlParameter param = null;
                SqlCommand cmd = dbContext.ContextSqlCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Proc_test_Update";

                //param = cmd.Parameters.Add("@ExpenditureId", SqlDbType.BigInt);
                //param.Value = BasicConverter.LongToDbValue(this.ExpenditureID);
                
                //param = cmd.Parameters.Add("@VendorName", SqlDbType.NVarChar);
                //param.Value = BasicConverter.StringToDbValue(this.VendorName);

                //param = cmd.Parameters.Add("@DateOfTransaction", SqlDbType.DateTime);
                //param.Value = BasicConverter.DateToDbValue(this.DateOfTransaction);

                //param = cmd.Parameters.Add("@Amount", SqlDbType.Float);
                //param.Value = this.Amount;

                //param = cmd.Parameters.Add("@IsDeleted", SqlDbType.Bit);
                //param.Value = BasicConverter.BoolToDbValue(this.IsDeleted);

                //param = cmd.Parameters.Add("@StaffLevelExpenditures", SqlDbType.Xml);
                //param.Value = BasicConverter.StringToDbValue(strStaffLevelExpendituresXml);

                cmd.ExecuteNonQuery();

                return true;
            }
               ).DoExecute(GetDbConnectionString());
        }

        private static string GetDbConnectionString()
        {
            return PRIFACT.DCCouncil.NPS.Core.NPSCommon.AppSettings.DbConnectionString;
        }

        
        private void _LoadOffices()
        {
            long lFiscalYearId = NPSRequestContext.GetContext().FiscalYearSelected.FiscalYearID;
            List<IDataHelper> lstOffices = Office.GetAll(string.Empty, lFiscalYearId, -1, null, Core.NPSCommon.Enums.SortFields.OfficeSortFields.Name, Core.NPSCommon.Enums.OrderByDirection.Ascending).Items;
            ddlOffices.Items.Clear();
            ddlOffices.DataSource = lstOffices;
            ddlOffices.DataTextField = "Name";
            ddlOffices.DataValueField = "OfficeID";
            ddlOffices.DataBind();

            ListItem item = new ListItem("Select Office", "-1");
            ddlOffices.Items.Insert(0, item);
            ddlOffices.SelectedIndex = 0;
        }

        protected void cvalOffice_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)source;
            if (ddlOffices.SelectedIndex == 0 || ddlOffices.SelectedIndex == -1)
            {
                cv.ErrorMessage = "Select an Office";
                args.IsValid = false;
                return;
            }
            else
            {
                Office objOffice = Office.GetByOfficeID(Convert.ToInt64(ddlOffices.SelectedValue));
                List<IDataHelper> lstBudgets = objOffice.Budgets(NPSRequestContext.GetContext().FiscalYearSelected.FiscalYearID);
                if (!(lstBudgets != null && lstBudgets.Count > 0))
                {
                    cv.ErrorMessage = "Cannot create the expenditure since the budget is not set for the selected office";
                    args.IsValid = false;
                    return;
                }
            }
        }
    }
}