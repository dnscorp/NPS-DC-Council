using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIFACT.DCCouncil.NPS.Core.NPSReportLibrary
{
    public static class POImporter_V2Helper
    {
        public static void ImportPOToTemp(string excelFileNameWithFullPath, string AgingBalanceSheetName, string CloseOutBalanceSheetName, string firstRowFirstColumnHeaderName, string SQLConnectionString, int @FiscalYearId)
        {
            SqlConnection con;
            SqlTransaction transaction = null;
            SqlCommand cmd;
            try
            {
                DataTable dt_POAgingBalance, dt_POCloseoutBalance;
                dt_POAgingBalance = getDataFromExcelSheet(excelFileNameWithFullPath, AgingBalanceSheetName, firstRowFirstColumnHeaderName, 21);
                dt_POCloseoutBalance = getDataFromExcelSheet(excelFileNameWithFullPath, CloseOutBalanceSheetName, firstRowFirstColumnHeaderName, 8);

                con = new SqlConnection(SQLConnectionString);
                con.Open();
                transaction = con.BeginTransaction();

                cmd = new SqlCommand("Proc_ImportPOExcel_ToTempTable_V2", con, transaction);
                cmd.CommandType = CommandType.StoredProcedure;

                //pass the datatables to stored procedure as parameter
                cmd.Parameters.AddWithValue("@tblPOAgingBalance", dt_POAgingBalance);
                cmd.Parameters.AddWithValue("@tblPOCloseoutBalance", dt_POCloseoutBalance);

                cmd.Parameters.AddWithValue("@FiscalYearId", @FiscalYearId);


                //execute stored procedure
                cmd.ExecuteNonQuery();

                transaction.Commit();
                //close connection
                con.Close();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        private static DataTable getDataFromExcelSheet(string ExcelFile, string sheetName, string firstRowFirstColumnHeaderName, int ColumnsToKeep)
        {
            string connectionString = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = \"" + ExcelFile + "\"; Extended Properties = \"Excel 8.0;HDR=No\";";
            DataTable dt_ExcelSheetData = new DataTable();

            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    string query = String.Format("select * from [{0}{1}]", sheetName,"A5:Z");
                    OleDbDataAdapter objDA = new System.Data.OleDb.OleDbDataAdapter(query, conn);

                    //objDA.FillSchema(dt_ExcelSheetData, SchemaType.Source);
                    //dt_ExcelSheetData.Columns[1].DataType = typeof(string);
                    objDA.Fill(dt_ExcelSheetData);                    

                    //Removing Unwanted Rows
                    //we look up to 100 rows
                    int rowIndexToSearch = 0;
                    while (rowIndexToSearch < 100)
                    {
                        if (dt_ExcelSheetData.Rows[rowIndexToSearch][0].ToString().Trim() == firstRowFirstColumnHeaderName) break;
                        else dt_ExcelSheetData.Rows[rowIndexToSearch].Delete();
                        rowIndexToSearch++;
                    }


                    //keeping only the necessary columns and removing the rest
                    for (int i = dt_ExcelSheetData.Columns.Count - 1; i >= ColumnsToKeep; i--)
                    {
                        dt_ExcelSheetData.Columns.RemoveAt(i);
                    }
                                                           
                    //Committing the changes
                    dt_ExcelSheetData.AcceptChanges();

                    for (int i = 0; i < dt_ExcelSheetData.Columns.Count; i++)
                    {
                        dt_ExcelSheetData.Columns[i].ColumnName = dt_ExcelSheetData.Rows[0][i].ToString();
                    }

                    dt_ExcelSheetData.Rows[0].Delete();
                    dt_ExcelSheetData.AcceptChanges();

                }

                return dt_ExcelSheetData;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
