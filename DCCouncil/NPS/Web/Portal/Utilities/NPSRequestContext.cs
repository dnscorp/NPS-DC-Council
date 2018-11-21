using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PRIFACT.DCCouncil.NPS.Core.NPSApi.SessionHelper;
using PRIFACT.DCCouncil.NPS.Core.NPSDataHelper;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.Utilities
{
    public class NPSRequestContext
    {
        private DateTime _dtStartTime;

        public User LoggedInUser
        {
            get;
            set;
        }

        public FiscalYear FiscalYearSelected
        {
            get;
            set;
        }

        private NPSRequestContext()
        {
            _dtStartTime = DateTime.Now;
        }

        public static NPSRequestContext GetContext()
        {
            if (System.Web.HttpContext.Current == null)
            {
                throw new System.Exception("This call is only valid during an HTTP request");
            }

            if (System.Web.HttpContext.Current.Items["NPSContext"] == null)
            {
                var newContext = new NPSRequestContext();
                newContext._DoPopulate();
                System.Web.HttpContext.Current.Items["NPSContext"] = newContext;
            }

            return System.Web.HttpContext.Current.Items["NPSContext"] as NPSRequestContext;
        }
        private void _DoPopulate()
        {
            if (PRIFACT.DCCouncil.NPS.Core.NPSApi.SessionHelper.SessionManager.GetSessionData().LoggedInUser != null)
            {
                LoggedInUser = PRIFACT.DCCouncil.NPS.Core.NPSApi.SessionHelper.SessionManager.GetSessionData().LoggedInUser as User;
                FiscalYearSelected = LoggedInUser.LastFiscalYearSelected;                
            }
        }

        internal static void RefreshContext()
        {
            var newContext = new NPSRequestContext();
            newContext._DoPopulate();
            System.Web.HttpContext.Current.Items["NPSContext"] = newContext;
        }
    }
}