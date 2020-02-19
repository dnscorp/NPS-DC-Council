<%@ Page Title="Expenditure Sub-Category Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ExpenditureSubCategoryReport.aspx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.Pages.Reports.ExpenditureSubCategoryReport" %>
<%@ Register src="~/UserCtrls/Reports/ExpenditureSubCategoryReportCtrl.ascx" tagname="ExpenditureSubCategoryReportCtrl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:ExpenditureSubCategoryReportCtrl ID="ExpenditureSubCategoryReportCtrl1" runat="server" />
</asp:Content>
