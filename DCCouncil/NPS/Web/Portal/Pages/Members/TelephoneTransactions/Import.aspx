﻿<%@ Page Language="C#"  Title="Import Telephone Transactions"  MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Import.aspx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.Pages.Members.TelephoneTransactions.Import" %>
<%@ Register Src="~/UserCtrls/Members/TelephoneTransaction/ImportCtrl.ascx" tagname="ImportCtrl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <uc1:ImportCtrl ID="ImportCtrl1" runat="server" />
    
</asp:Content>

