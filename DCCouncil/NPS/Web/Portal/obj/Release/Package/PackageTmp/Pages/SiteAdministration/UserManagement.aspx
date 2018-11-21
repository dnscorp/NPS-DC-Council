<%@ Page Title="User Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserManagement.aspx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.Pages.SiteAdministration.UserManagement" %>

<%@ Register Src="~/UserCtrls/SiteAdministration/UserManagementCtrl.ascx" TagName="UserManagementCtrl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <uc1:UserManagementCtrl ID="UserManagementCtrl1" runat="server" />

</asp:Content>
