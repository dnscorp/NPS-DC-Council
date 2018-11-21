<%@ Page Title="Budget Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BudgetManagement.aspx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.Pages.SiteAdministration.BudgetManagement" %>
<%@ Register src="~/UserCtrls/SiteAdministration/BudgetManagementCtrl.ascx" tagname="BudgetManagementCtrl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <uc1:BudgetManagementCtrl ID="BudgetManagementCtrl1" runat="server" />

</asp:Content>
