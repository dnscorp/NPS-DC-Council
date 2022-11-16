<%@ Page Title="Purchase Orders" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PurchaseOrdersV2.aspx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.Pages.Members.PurchaseOrdersV2" %>
<%@ Register src="~/UserCtrls/Members/PurchaseOrdersV2Ctrl.ascx" tagname="PurchaseOrdersCtrl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <uc1:PurchaseOrdersCtrl ID="PurchaseOrdersCtrl1" runat="server" />

</asp:Content>
