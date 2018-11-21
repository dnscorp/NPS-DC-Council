<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FiscalYearManagementItemCtrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.SiteAdministration.FiscalYearManagement.FiscalYearManagementItemCtrl" %>
<div id="divItemPopup" runat="server">
    <asp:HiddenField ID="hfMode" runat="server" />
    <asp:HiddenField ID="hfFiscalYearId" runat="server" />
    <table cellpadding="0" cellspacing="0" class="form">
        <tr>
            <th><span class="required">*</span>Name</th>
            <td>
                <asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
            <td>
                <asp:CustomValidator CssClass="error" ID="cvalName" runat="server" OnServerValidate="cvalName_ServerValidate" Display="Dynamic" ValidationGroup="ValGroup1"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <th>Year</th>
            <td>
                <asp:TextBox ID="txtYear" runat="server"></asp:TextBox>
                <%--<asp:Literal ID="litYear" runat="server"></asp:Literal>--%>
            <td>
                <asp:CustomValidator CssClass="error" ID="cvalYear" runat="server" OnServerValidate="cvalYear_ServerValidate" Display="Dynamic" ValidationGroup="ValGroup1"></asp:CustomValidator>
            </td>
        </tr>
        <asp:PlaceHolder ID="phDates" runat="server">
            <tr>
                <th>Start Date</th>
                <td>
                    <asp:Literal ID="litStartDate" runat="server"></asp:Literal>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <th>End Date</th>
                <td>
                    <asp:Literal ID="litEndDate" runat="server"></asp:Literal>
                <td>&nbsp;</td>
            </tr>
        </asp:PlaceHolder>
        <tr class="form-button">
            <th></th>
            <td colspan="2">
                <a href="javascript:;" class="btn btn-primary" runat="server" id="lnkSubmit" onserverclick="lnkSubmit_ServerClick" onclick="this.style.visibility='hidden'"></a>
                <a href="javascript:;" class="btn" id="lnkCancel" runat="server" onserverclick="lnkCancel_ServerClick">Cancel</a>
            </td>
        </tr>
    </table>
</div>
