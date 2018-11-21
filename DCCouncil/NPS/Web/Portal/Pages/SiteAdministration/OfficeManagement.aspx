<%@ Page Title="Office Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OfficeManagement.aspx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.Pages.SiteAdministration.OfficeManagement" %>
<%@ Register src="~/UserCtrls/SiteAdministration/OfficeManagementCtrl.ascx" tagname="OfficeManagementCtrl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        
    <uc1:OfficeManagementCtrl ID="OfficeManagementCtrl1" runat="server" />
        
</asp:Content>
