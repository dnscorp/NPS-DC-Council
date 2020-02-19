using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIFACT.DCCouncil.NPS.Core.NPSCommon
{
    public static class NPSUrls
    {
        public const string Dashboard = "/Pages/Dashboard";
        public const string Login = "/Pages/Login";
        public const string Error = "/Pages/Error";

        //Members        
        public const string PurchaseOrders = "/Pages/Members/PurchaseOrders";
        public const string Expenditures = "/Pages/Members/Expenditures";
        public const string RecurringTransactions = "/Pages/Members/RecurringTransactions";
        
        //Reports
        public const string AdhocReports = "/Pages/Reports/AdhocReports";
        public const string NPSReports = "/Pages/Reports/NPSReports";
        public const string ExpenditureCategoryReports = "/Pages/Reports/ExpenditureSubCategoryReport";

        //SiteAdministration
        public const string SiteSettings = "/Pages/SiteAdministration/SiteSettings";
        public const string UserManagement = "/Pages/SiteAdministration/UserManagement";

        public const string OfficeManagement = "/Pages/SiteAdministration/OfficeManagement";
        public const string FiscalYearManagement = "/Pages/SiteAdministration/FiscalYearManagement";
        public const string StaffManagement = "/Pages/SiteAdministration/StaffManagement";
        public const string BudgetManagement = "/Pages/SiteAdministration/BudgetManagement";
        public const string VendorManagement = "/Pages/SiteAdministration/VendorManagement";
        public const string ExpenditureCategoryManagement = "/Pages/SiteAdministration/ExpenditureCategoryManagement";


    }
}
