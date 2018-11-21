<%@ Page Title="Expenditure Category Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ExpenditureCategoryManagement.aspx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.Pages.SiteAdministration.ExpenditureCategoryManagement" %>
<%@ Register src="~/UserCtrls/SiteAdministration/ExpenditureCategoryManagementCtrl.ascx" tagname="ExpenditureCategoryManagementCtrl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <uc1:ExpenditureCategoryManagementCtrl ID="ExpenditureCategoryManagementCtrl1" runat="server" />
</asp:Content>
