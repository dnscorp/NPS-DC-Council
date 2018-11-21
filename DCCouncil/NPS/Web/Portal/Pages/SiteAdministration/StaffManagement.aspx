<%@ Page Title="Staff Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StaffManagement.aspx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.Pages.SiteAdministration.StaffManagement" %>
<%@ Register src="~/UserCtrls/SiteAdministration/StaffManagementCtrl.ascx" tagname="StaffManagementCtrl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <uc1:StaffManagementCtrl ID="StaffManagementCtrl1" runat="server" />

</asp:Content>
