using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PRIFACT.PRIFACTBase.UrlHelpers
{
    public static class UrlUtility
    {
        /// <summary>
        /// For http://www.google.co.in/search.aspx?hl=en&q=asp.net+datetime+format+list&meta=
        /// returns http://www.google.co.in/search.aspx?hl=en&q=asp.net+datetime+format+list&meta=
        /// </summary>
        public static string FullCurrentPageUrlWithQV
        {
            get
            {
                if (_GetQueryStrings().Length > 0)
                    return GetHttpPrefix() + System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + PortSuffix + RelativeCurrentPageUrl + "?" + _GetQueryStrings();
                else
                    return GetHttpPrefix() + System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + PortSuffix + RelativeCurrentPageUrl;
            }
        }

        public static string GetHttpPrefix()
        {
            if (System.Web.HttpContext.Current.Request.IsSecureConnection)
                return "https://";

            return "http://";
        }

        private static string _GetQueryStrings()
        {
            string urlRequest = string.Empty;
            if (System.Web.HttpContext.Current.Request.ServerVariables["QUERY_STRING"] != null && System.Web.HttpContext.Current.Request.ServerVariables["QUERY_STRING"].Length > 0)
            {
                urlRequest = System.Web.HttpContext.Current.Request.ServerVariables["QUERY_STRING"];
            }
            return urlRequest;
        }

        private static string _key_for_storing_relative_pageUrl_on_context = "PRIFACT_URL_UTILITY_RELATIVE";

        /// <summary>
        /// For http://www.google.co.in/search.aspx?hl=en&q=asp.net+datetime+format+list&meta=
        /// returns /search.aspx
        /// </summary>
        public static string RelativeCurrentPageUrl
        {
            get
            {
                if (System.Web.HttpContext.Current.Items.Contains(_key_for_storing_relative_pageUrl_on_context))
                {
                    return System.Web.HttpContext.Current.Items[_key_for_storing_relative_pageUrl_on_context].ToString();
                }
                System.Web.HttpContext.Current.Items[_key_for_storing_relative_pageUrl_on_context] = _DoGetRelativeCurrentPageUrl();
                return System.Web.HttpContext.Current.Items[_key_for_storing_relative_pageUrl_on_context].ToString();
            }
        }

        private static string _DoGetRelativeCurrentPageUrl()
        {
            if (System.Web.HttpContext.Current.Items.Contains("Layouts"))
            {
                //System.Uri contextUri = System.Web.HttpContext.Current.Items["Microsoft.SharePoint.Administrtion.ContextUri"] as System.Uri;
                //string pathAndQuery = System.Web.HttpContext.Current.Request.RawUrl;
                string pathAndQuery = System.Web.HttpContext.Current.Items["HTTP_VTI_SCRIPT_NAME"].ToString();

                int endOfUrl = pathAndQuery.IndexOf("?");
                if (endOfUrl > 0)
                {
                    return pathAndQuery.ToString().Substring(0, endOfUrl);
                }
                return pathAndQuery.ToString();
            }
            return System.Web.HttpContext.Current.Request.ServerVariables["URL"];
        }


        public static string PortSuffix
        {
            get
            {
                if (System.Web.HttpContext.Current.Request.IsSecureConnection)
                    return (System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT"] == "443") ? ("") : (":" + System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT"]);
                else
                    return (System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT"] == "80") ? ("") : (":" + System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT"]);
            }

        }

        public static string SetStringOnQv(string qv)
        {
            if (String.IsNullOrEmpty(qv)) return string.Empty;
            string _iQV = PRIFACTBase.EncryptionHelpers.BlowFish.EncryptString(qv);
            return Encode(_iQV);
        }

        private static string Encode(string str)
        {
            byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(str);
            return HttpUtility.UrlEncode(Convert.ToBase64String(encbuff));
        }

        public static string GetStringFromQv(string qv)
        {
            if (String.IsNullOrEmpty(qv)) return string.Empty;
            string _strQV = Decode(qv);
            string _decryptedMsg = PRIFACTBase.EncryptionHelpers.BlowFish.DecryptString(_strQV);
            return _decryptedMsg;
        }

        public static string Decode(string str)
        {
            str = HttpUtility.UrlDecode(str);
            byte[] decbuff = Convert.FromBase64String(str);
            return System.Text.Encoding.UTF8.GetString(decbuff);
        }
    }

}
