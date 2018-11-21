<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Expenditures.aspx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.Pages.Members.Expenditures" %>
<%@ Register src="~/UserCtrls/Common/ExpendituresCtrl.ascx" tagname="ExpendituresCtrl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <uc1:ExpendituresCtrl ID="ExpendituresCtrl1" runat="server" />

</asp:Content>
