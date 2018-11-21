using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using System.Globalization;

namespace PRIFACT.DCCouncil.NPS.Core.NPSApi.HttpModules
{
    public class NPSHttpModule : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
            
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(BeginRequest);
            context.PreRequestHandlerExecute += new EventHandler(PreRequestHandlerExecute);
        }

        #endregion

        protected virtual void BeginRequest(object sender, EventArgs e)
        {
            CultureInfo cInfo = new CultureInfo(CultureInfo.CurrentCulture.Name);

            cInfo.DateTimeFormat.ShortDatePattern = AppSettings.DateFormat;
            cInfo.DateTimeFormat.DateSeparator = AppSettings.DateSeparator;

            System.Threading.Thread.CurrentThread.CurrentUICulture = cInfo;
            System.Threading.Thread.CurrentThread.CurrentCulture = cInfo;
        }

        protected virtual void PreRequestHandlerExecute(object sender, EventArgs args)
        {
            if ((HttpContext.Current.CurrentHandler as Page) == null)
                return;

            if (AppSettings.IsSSLEnabled)
            {
                if (!System.Web.HttpContext.Current.Request.IsSecureConnection)
                {
                    string url = PRIFACTBase.UrlHelpers.UrlUtility.FullCurrentPageUrlWithQV;
                    string redirectUrl = "https://" + url.Substring("http://".Length);
                    System.Web.HttpContext.Current.Response.Redirect(redirectUrl);
                    return;
                }
            }

        }
    }
}
