using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIFACT.DCCouncil.NPS.Core.NPSApi.SessionHelper
{
    public static class SessionManager
    {
        public static void Abandon()
        {
            System.Web.HttpContext.Current.Session[sessionDataKey] = null;
            System.Web.HttpContext.Current.Session.Abandon();
        }

        public static void Clear()
        {
            System.Web.HttpContext.Current.Session.Clear();
        }


        private const string sessionDataKey = "NPS_SESSION_DATA";

        public static NPSSessionData GetSessionData()
        {
            if (System.Web.HttpContext.Current.Session[sessionDataKey] == null)
            {
                System.Web.HttpContext.Current.Session[sessionDataKey] = new NPSSessionData(Guid.NewGuid());
            }
            return System.Web.HttpContext.Current.Session[sessionDataKey] as NPSSessionData;
        }
    }
}
