<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RecurringItemCtrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Common.RecurringItemCtrl" %>
<div id="divItemPopup" runat="server">
    <asp:HiddenField ID="hfMode" runat="server" />
    <asp:HiddenField ID="hfRecurringID" runat="server" />
    <asp:HiddenField ID="hfFiscalYearId" runat="server" />
    <asp:HiddenField ID="hfOfficeName" runat="server" />

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
        <tr>
            <th>Category</th>
            <td>
                <asp:DropDownList ID="ddlCategory" runat="server">
                    <asp:ListItem Value="PCard">PCard</asp:ListItem>
                    <asp:ListItem Value="Phone">Phone</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr runat="server" id="trVendorName">
            <th>
                <span class="required">*</span>Vendor Name
            </th>
            <td>
                <asp:TextBox ID="txtVendorName" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:CustomValidator CssClass="error" ID="cvalVendorName" runat="server" Display="Dynamic" ValidationGroup="ValGroup1" OnServerValidate="cvalVendorName_ServerValidate"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <th><span class="required">*</span>Description</th>
            <td>
                <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
            </td>
            <td><asp:CustomValidator CssClass="error" ID="cvalDescription" runat="server" Display="Dynamic" ValidationGroup="ValGroup1" OnServerValidate="cvalDescription_ServerValidate"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <th><span class="required">*</span>Amount
            </th>
            <td>
                <asp:TextBox ID="txtAmount" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:CustomValidator ID="cvalAmount" CssClass="error" runat="server" Display="Dynamic" ValidationGroup="ValGroup1" OnServerValidate="cvalAmount_ServerValidate"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <th>Comments
            </th>
            <td>
                <asp:TextBox TextMode="multiline" Rows="5"  ID="txtComments" runat="server"></asp:TextBox>
            </td>
            <td>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2"></td>
        </tr>
        <tr class="form-button">
            <th></th>
            <td colspan="2">
                <a href="javascript:;" class="btn btn-primary" runat="server" id="lnkSubmit" onserverclick="lnkSubmit_ServerClick" onclick="this.style.visibility='hidden'"></a>
                <a href="javascript:;" class="btn" id="lnkCancel" runat="server" onserverclick="lnkCancel_ServerClick">Cancel</a>
            </td>
        </tr>
    </table>
</div>