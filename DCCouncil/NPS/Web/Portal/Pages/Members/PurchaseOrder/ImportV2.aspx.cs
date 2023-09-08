using Newtonsoft.Json;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.Pages.Members.PurchaseOrder
{
    public partial class ImportV2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string OnSubmit(string agingData, string closeoutData)
        {
            string truncateQuery = "TRUNCATE TABLE POAgingBalanceExcelData_V2;TRUNCATE TABLE POCloseoutBalanceExcelData_V2;";
            string insertQuery = truncateQuery + " INSERT INTO POAgingBalanceExcelData_V2 (";
            string selectQuery = " SELECT ";
            var keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(agingData);
            foreach (var pair in keyValuePairs)
            {
                insertQuery += "[" + pair.Key + "],";

                //if(pair.Value=="Expenditure Invoice Amount")
                //    selectQuery += "[" + pair.Value.Replace(" Invoice Amount", "(Invoice Amount)") + "],";
                //else
                selectQuery += "[" + pair.Value + "],";
            }

            insertQuery = insertQuery.TrimEnd(',', ' ') + ")";
            selectQuery = selectQuery.TrimEnd(',', ' ');

            selectQuery += " FROM tempAging;";

            insertQuery += selectQuery;// + ")";

            string insertQueryCO = " INSERT INTO POCloseoutBalanceExcelData_V2 (";
            string selectQueryCO = " SELECT ";
            var keyValuePairsCO = JsonConvert.DeserializeObject<Dictionary<string, string>>(closeoutData);
            foreach (var pair in keyValuePairsCO)
            {
                insertQueryCO += "[" + pair.Key + "],";

                //if(pair.Value=="Expenditure Invoice Amount")
                //    selectQuery += "[" + pair.Value.Replace(" Invoice Amount", "(Invoice Amount)") + "],";
                //else
                selectQueryCO += "[" + pair.Value + "],";
            }

            insertQueryCO = insertQueryCO.TrimEnd(',', ' ') + ")";
            selectQueryCO = selectQueryCO.TrimEnd(',', ' ');

            selectQueryCO += " FROM tempCloseout;";

            insertQueryCO += selectQueryCO;

            insertQuery += insertQueryCO;

            //Include the import Stored Procedure too
            var fiscalYear = NPSRequestContext.GetContext().FiscalYearSelected;
            insertQuery += ";EXEC Proc_ImportPOExcel_Mapping_ToTempTable_V2 " + fiscalYear.FiscalYearID.ToString();

            SqlConnection con = new SqlConnection(AppSettings.DbConnectionString);
            con.Open();
            using (SqlCommand cmd = new SqlCommand(insertQuery, con))
            {
                cmd.ExecuteNonQuery();
            }
            con.Close();
            con.Dispose();

            return "";
        }
    }
}