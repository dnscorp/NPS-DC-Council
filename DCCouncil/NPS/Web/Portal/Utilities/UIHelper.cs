using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.Utilities
{
    public class UIHelper
    {
        public static void SetSuccessMessage(string strMessage)
        {
            System.Web.HttpContext.Current.Session["IMMEDIATE_EXPIRE_SUCCESS_SESSION_VALUE"] = strMessage;
        }

        public static void SetErrorMessage(string strMessage)
        {
            System.Web.HttpContext.Current.Session["IMMEDIATE_EXPIRE_ERROR_SESSION_VALUE"] = strMessage;
        }

        public static string GetSuccessMessage()
        {
            string str = System.Web.HttpContext.Current.Session["IMMEDIATE_EXPIRE_SUCCESS_SESSION_VALUE"] as string;
            System.Web.HttpContext.Current.Session["IMMEDIATE_EXPIRE_SUCCESS_SESSION_VALUE"] = null;
            return str;
        }

        public static string GetErrorMessage()
        {
            string str = System.Web.HttpContext.Current.Session["IMMEDIATE_EXPIRE_ERROR_SESSION_VALUE"] as string;
            System.Web.HttpContext.Current.Session["IMMEDIATE_EXPIRE_ERROR_SESSION_VALUE"] = null;
            return str;
        }

        public static string GetDateTimeInDefaultFormat(DateTime? dtSpecifiedTime)
        {
            if (dtSpecifiedTime.HasValue)
            {
                return dtSpecifiedTime.Value.ToString(AppSettings.DateFormat);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetAmountInDefaultFormat(double dblAmount)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.CurrencySymbol = AppSettings.DefaultCurrencySymbol;
            return String.Format(nfi, "{0:C}", dblAmount);
        }
        
    }
}