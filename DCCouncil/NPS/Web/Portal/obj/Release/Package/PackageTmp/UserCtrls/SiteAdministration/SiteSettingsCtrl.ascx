<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SiteSettingsCtrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.SiteAdministration.SiteSettingsCtrl" %>
<h1 class="page-header">Site Settings</h1>
<div class="dashboard-ctrl">
    <div class="dashboard-ctrl-links">
        <ul class="thumbnails">
            <li class="span3">
                <a href="<%= PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.UserManagement%>">
                    <div class="thumbnail">
                        <img alt="" src="/images/settings.jpg">
                        <p>User Account Management</p>
                    </div>
                </a>
            </li>
            <li class="span3">
                <a href="<%= PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.OfficeManagement%>">
                    <div class="thumbnail">
                        <img alt="" src="/images/settings.jpg">
                        <p>Office Management</p>
                    </div>
                </a>
            </li>
            <li class="span3">
                <a href="<%= PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.FiscalYearManagement%>">
                    <div class="thumbnail">
                        <img alt="" src="/images/settings.jpg">
                        <p>Fiscal Year Management</p>
                    </div>
                </a>
            </li>
            <li class="span3">
                <a href="<%= PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.ExpenditureCategoryManagement%>">
                    <div class="thumbnail">
                        <img alt="" src="/images/settings.jpg">
                        <p>Expenditure Category Management</p>
                    </div>
                </a>
            </li>
            <li class="span3">
                <a href="<%= PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.VendorManagement%>">
                    <div class="thumbnail">
                        <img alt="" src="/images/settings.jpg">
                        <p>Vendor Management</p>
                    </div>
                </a>
            </li>
        </ul>
    </div>
</div>
