<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VendorManagement.aspx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.Pages.SiteAdministration.VendorManagement" %>
<%@ Register src="~/UserCtrls/SiteAdministration/VendorManagementCtrl.ascx" tagname="VendorManagementCtrl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:VendorManagementCtrl ID="VendorManagementCtrl1" runat="server" />
</asp:Content>
