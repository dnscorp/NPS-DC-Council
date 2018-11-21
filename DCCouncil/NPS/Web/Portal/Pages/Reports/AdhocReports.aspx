<%@ Page Title="AdHoc Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdhocReports.aspx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.Pages.Reports.AdhocReports" %>
<%@ Register src="~/UserCtrls/Reports/AdhocReportsCtrl.ascx" tagname="AdhocReportsCtrl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <uc1:AdhocReportsCtrl ID="AdhocReportsCtrl1" runat="server" />

</asp:Content>
