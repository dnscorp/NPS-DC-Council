<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BudgetManagementItemCtrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.SiteAdministration.BudgetManangement.BudgetManagementItemCtrl" %>
<div id="divItemPopup" runat="server">
    <asp:HiddenField ID="hfMode" runat="server" />
    <asp:HiddenField ID="hfBudgetId" runat="server" />
    <asp:HiddenField ID="hfOfficeId" runat="server" />
    <asp:HiddenField ID="hfFiscalYearId" runat="server" />
    <table cellpadding="0" cellspacing="0" class="form">
        <tr>
            <th><asp:Literal ID="litBudgetHeader" runat="server"></asp:Literal></th>
            <td colspan="2">&nbsp;</td>
        </tr>
        
        <tr>
            <th><span class="required">*</span>Name</th>
            <td>
                <asp:TextBox ID="txtBudgetName" runat="server"></asp:TextBox></td>
            <td>
                <asp:CustomValidator ID="cvalBudgetName" CssClass="error" runat="server" OnServerValidate="cvalBudgetName_ServerValidate" Display="Dynamic" ValidationGroup="ValGroup1"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <th><span class="required">*</span><asp:Literal ID="litBudgetAmount" runat="server"></asp:Literal></th>
            <td>
                <asp:TextBox ID="txtAmount" runat="server"></asp:TextBox></td>
            <td>
                <asp:CustomValidator CssClass="error" ID="cvalAmount" runat="server" OnServerValidate="cvalAmount_ServerValidate" Display="Dynamic" ValidationGroup="ValGroup1"></asp:CustomValidator>
            </td>
        </tr>
        <%--<tr>
            <th>Is Default Budget:</th>
            <td>
                <asp:CheckBox ID="chkIsDefault" runat="server" />
            <td>
                <asp:CustomValidator CssClass="error" ID="cvalIsDefault" runat="server" OnServerValidate="cvalIsDefault_ServerValidate" Display="Dynamic" ValidationGroup="ValGroup1"></asp:CustomValidator>
            </td>
        </tr>--%>
        <tr class="form-button">
            <th></th>
            <td colspan="2">
                <a href="javascript:;" class="btn btn-primary" runat="server" id="lnkSubmit" onserverclick="lnkSubmit_ServerClick" onclick="this.style.visibility='hidden'"></a>
                <a href="javascript:;" class="btn" id="lnkCancel" runat="server" onserverclick="lnkCancel_ServerClick">Cancel</a>
            </td>
        </tr>
    </table>
</div>