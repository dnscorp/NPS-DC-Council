<%@ Page Title="Purchase Orders" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PurchaseOrders.aspx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.Pages.Members.PurchaseOrders" %>
<%@ Register src="~/UserCtrls/Members/PurchaseOrdersCtrl.ascx" tagname="PurchaseOrdersCtrl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <uc1:PurchaseOrdersCtrl ID="PurchaseOrdersCtrl1" runat="server" />

</asp:Content>
