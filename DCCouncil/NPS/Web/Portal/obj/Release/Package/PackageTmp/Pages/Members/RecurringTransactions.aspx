<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RecurringTransactions.aspx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.Pages.Members.RecurringTransactions" %>
<%@ Register src="~/UserCtrls/Common/RecurringCtrl.ascx" tagname="RecurringCtrl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:RecurringCtrl ID="RecurringCtrl1" runat="server" />
</asp:Content>
