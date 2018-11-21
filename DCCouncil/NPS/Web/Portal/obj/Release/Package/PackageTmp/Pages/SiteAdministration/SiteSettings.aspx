<%@ Page Title="Site Settings" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SiteSettings.aspx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.Pages.SiteAdministration.SiteSettings" %>
<%@ Register src="~/UserCtrls/SiteAdministration/SiteSettingsCtrl.ascx" tagname="SiteSettingsCtrl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:SiteSettingsCtrl ID="SiteSettingsCtrl1" runat="server" />
</asp:Content>
