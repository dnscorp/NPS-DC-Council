<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DashboardV2.aspx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.Pages.DashboardV2" %>
<%@ Register src="~/UserCtrls/DashboardV2Ctrl.ascx" tagname="DashboardCtrl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <uc1:DashboardCtrl ID="DashboardCtrl1" runat="server" />
    
</asp:Content>
