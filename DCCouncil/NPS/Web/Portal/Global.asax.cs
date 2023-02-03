using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Routing;
using System.Web.Security;
using Elmah;
using PRIFACT.DCCouncil.NPS.Core.NPSCommon;
using PRIFACT.DCCouncil.NPS.Web.Portal.App_Start;
using PRIFACT.DCCouncil.NPS.Web.Portal.Utilities;

namespace PRIFACT.DCCouncil.NPS.Web.Portal
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup            
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void ErrorLog_Logged(object sender, ErrorLoggedEventArgs args)
        {
            var config = WebConfigurationManager.OpenWebConfiguration("~");
            var customErrorsSection = (CustomErrorsSection)config.GetSection("system.web/customErrors");

            if (customErrorsSection != null)
            {
                switch (customErrorsSection.Mode)
                {
                    case CustomErrorsMode.Off:
                        break;
                    case CustomErrorsMode.On:
                        FriendlyErrorTransfer(args.Entry.Id, customErrorsSection.DefaultRedirect);
                        break;
                    case CustomErrorsMode.RemoteOnly:
                        if (!HttpContext.Current.Request.IsLocal)
                            FriendlyErrorTransfer(args.Entry.Id, customErrorsSection.DefaultRedirect);
                        break;
                    default:
                        break;
                }
            }
        }
        void FriendlyErrorTransfer(string emlahId, string url)
        {            
            Response.Redirect(String.Format("{0}?id={1}", NPSUrls.Error, Server.UrlEncode(emlahId)));

        }
    }
}
