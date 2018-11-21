using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace PRIFACT.DCCouncil.NPS.Web.Portal.Utilities
{
    public class AuthenticationHelper
    {
        public static void Signout()
        {
            FormsAuthentication.SignOut();            
        }
    }
}