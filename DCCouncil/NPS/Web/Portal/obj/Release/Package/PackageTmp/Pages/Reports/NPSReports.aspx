<%@ Page Title="NPS Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NPSReports.aspx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.Pages.Reports.NPSReports" %>
<%@ Register src="~/UserCtrls/Reports/NPSReportsCtrl.ascx" tagname="NPSReportsCtrl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <uc1:NPSReportsCtrl ID="NPSReportsCtrl1" runat="server" />

</asp:Content>
