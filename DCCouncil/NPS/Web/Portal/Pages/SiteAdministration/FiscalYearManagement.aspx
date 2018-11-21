<%@ Page Title="Fiscal Year Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FiscalYearManagement.aspx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.Pages.SiteAdministration.FiscalYearManagement" %>
<%@ Register src="~/UserCtrls/SiteAdministration/FiscalYearManagementCtrl.ascx" tagname="FiscalYearManagementCtrl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <uc1:FiscalYearManagementCtrl ID="FiscalYearManagementCtrl1" runat="server" />

</asp:Content>
