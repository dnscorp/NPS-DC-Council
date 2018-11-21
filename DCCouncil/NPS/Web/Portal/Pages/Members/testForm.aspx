<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="testForm.aspx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.Pages.Members.testForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3 class="popup-heading">
        <asp:Literal ID="litHeading" runat="server"></asp:Literal></h3>
    <table cellpadding="0" cellspacing="0" class="form">
        <tr>
            <th>Fiscal Year selected
            </th>
            <td>
                <asp:Literal ID="litFiscalYearSelected" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <th><span class="required">*</span>Office
            </th>
            <td>
                <asp:Literal ID="litOffice" runat="server"></asp:Literal>
                <asp:DropDownList ID="ddlOffices" runat="server"></asp:DropDownList>
            </td>
            <td>
                <asp:CustomValidator CssClass="error" ID="cvalOffice" runat="server" Display="Dynamic" ValidationGroup="ValGroup1" OnServerValidate="cvalOffice_ServerValidate"></asp:CustomValidator>
            </td>
        </tr>
        </table>

    </asp:TextBox><asp:Button ID="Button1" OnClick="Button1_Click" runat="server" Text="Button" />
</asp:Content>
