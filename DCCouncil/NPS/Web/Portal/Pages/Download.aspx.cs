using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.Pages
{
    public partial class Download : System.Web.UI.Page
    {
        public string Type
        {
            get
            {
                if (Request["Type"] != null)
                {
                    return Request["Type"];
                }
                else
                {
                    return string.Empty;
                }
            }
         
        }

        public string Id
        {
            get
            {
                if (Request["Id"] != null)
                {
                    return Request["Id"];
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string FiscalYear
        {
            get 
            {
                if (Request["FY"] != null)
                {
                    return Request["FY"];
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string OfficeId
        {
            get
            {
                if (Request["OfficeId"] != null)
                {
                    return Request["OfficeId"];
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string AsOfDate
        {
            get 
            {
                if (Request["AsOfDate"] != null)
                {
                    return Request["AsOfDate"];
                }
                else
                {
                    return string.Empty;
                }
            
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Type.Equals("NPSReport"))
            {
                string newStrSaveFilePath = AppSettings.ExcelTempLocationPath + Id + ".xlsx";
                byte[] buffer;
                buffer = _GetFileBuffer(newStrSaveFilePath);
                Office objOffice = Office.GetByOfficeID(Convert.ToInt64(OfficeId));
                
                DateTime dtAsOfDate = Convert.ToDateTime(AsOfDate);
                string strFileName = AppSettings.ExcelSheetPrefix + FiscalYear + "_" + objOffice.Name + "_" + dtAsOfDate .Month+"_"+dtAsOfDate.Day+"_"+dtAsOfDate.Year+".xlsx";
                _DownloadFile(buffer, strFileName);
            }
            if (Type.Equals("AdHocReport"))
            {
                string newStrSaveFilePath = AppSettings.ExcelTempLocationPath + Id + ".xlsx";
                byte[] buffer;
                buffer = _GetFileBuffer(newStrSaveFilePath);
              
                DateTime dtAsOfDate = Convert.ToDateTime(AsOfDate);
                string strFileName = AppSettings.ExcelAdhocSheetPrefix + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.ToString("HH") +DateTime.Now.ToString("mm")+ ".xlsx";
                _DownloadFile(buffer, strFileName);
            }
            if (Type.Equals("ExpenditureSubCategoryReport"))
            {
                string newStrSaveFilePath = AppSettings.ExcelTempLocationPath + Id + ".xlsx";
                byte[] buffer;
                buffer = _GetFileBuffer(newStrSaveFilePath);

                DateTime dtAsOfDate = Convert.ToDateTime(AsOfDate);
                string strFileName = AppSettings.ExcelExpenditureSheetPrefix + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.ToString("HH") + DateTime.Now.ToString("mm") + ".xlsx";
                _DownloadFile(buffer, strFileName);
            }
        }

        private byte[] _GetFileBuffer(string newStrSaveFilePath)
        {
            byte[] buffer;
            using (FileStream fs = File.OpenRead(newStrSaveFilePath))
            {
                int length = (int)fs.Length;             
                using (BinaryReader br = new BinaryReader(fs))
                {
                    buffer = br.ReadBytes(length);
                }
            }
            return buffer;
        }

        private void _DownloadFile(byte[] buffer, string strFileName)
        {
            System.Web.HttpContext.Current.Response.AddHeader("Content-disposition", "attachment; filename=" + strFileName);
            System.Web.HttpContext.Current.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; 
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.OutputStream.Write(buffer, 0, buffer.Length);
            System.Web.HttpContext.Current.Response.OutputStream.Flush();
            System.Web.HttpContext.Current.Response.OutputStream.Close();
            System.Web.HttpContext.Current.Response.End();
        }
    }
}