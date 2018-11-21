<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Guest.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.Pages.Login" %>
<%@ Register src="~/UserCtrls/LoginCtrl.ascx" tagname="LoginCtrl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <uc1:LoginCtrl ID="LoginCtrl1" runat="server" />

</asp:Content>
