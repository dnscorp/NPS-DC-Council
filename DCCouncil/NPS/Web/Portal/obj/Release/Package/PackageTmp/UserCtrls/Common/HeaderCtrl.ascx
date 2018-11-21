<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HeaderCtrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Common.HeaderCtrl" %>

<%@ Register Src="~/UserCtrls/Common/FiscalYearSelectorCtrl.ascx" TagName="FiscalYearSelectorCtrl" TagPrefix="uc1" %>

<header>
    <div class="header">
        <div class="inner">
            <div class="DC-Council">
                <p>
                    Non-Personnel Expenditure Management System
                </p>
            </div>
            <div class="user-details">
                Welcome <%= PRIFACT.DCCouncil.NPS.Web.Portal.Utilities.NPSRequestContext.GetContext().LoggedInUser.UserProfile.FullName %> | <a href="javascript:;" runat="server" id="lnkSignout" onserverclick="OnLnkSignoutClick">Signout</a>
            </div>
            <div class="fiscal">
                <uc1:FiscalYearSelectorCtrl ID="FiscalYearSelectorCtrl1" runat="server" />
            </div>
        </div>
    </div>
</header>
