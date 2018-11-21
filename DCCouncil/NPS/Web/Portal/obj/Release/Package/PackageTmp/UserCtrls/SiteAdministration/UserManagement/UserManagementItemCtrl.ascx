<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserManagementItemCtrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.SiteAdministration.UserManagement.UserManagementItemCtrl" %>
<div id="divItemPopup" runat="server">
    <asp:HiddenField ID="hfMode" runat="server" />
    <asp:HiddenField ID="hfUserId" runat="server" />
    <table cellpadding="0" cellspacing="0" class="form">
        <tr>
            <th><span class="required">*</span> Username</th>
            <td>
                <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox></td>
            <td>
                <asp:CustomValidator CssClass="error" ID="cvalUsername" runat="server" OnServerValidate="cvalUsername_ServerValidate" Display="Dynamic" ValidationGroup="ValGroup1"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <th><span class="required">*</span>First Name</th>
            <td>
                <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox></td>
            <td>
                <asp:CustomValidator CssClass="error" ID="cvalFirstName" runat="server" OnServerValidate="cvalFirstName_ServerValidate" Display="Dynamic" ValidationGroup="ValGroup1"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <th>Last Name</th>
            <td>
                <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox></td>
            <td></td>
        </tr>
        <tr id="trChangePassword" runat="server">
            <th></th>
            <td>
                <asp:CheckBox ID="chkChangePassword" Text="Change Password ?" runat="server"   AutoPostBack="true" OnCheckedChanged="chkChangePassword_CheckedChanged"/>
            </td>
            <td></td>
        </tr>
        <asp:PlaceHolder ID="phPassword" runat="server" Visible="false">
            <tr>
                <th><span class="required">*</span>Password</th>
                <td>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox></td>
                <td>
                    <asp:CustomValidator CssClass="error" ID="cvalPassword" runat="server" Display="Dynamic" OnServerValidate="cvalPassword_ServerValidate" ValidationGroup="ValGroup1"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <th><span class="required">*</span>Confirm Password</th>
                <td>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password"></asp:TextBox></td>
                <td>
                    <asp:CustomValidator CssClass="error" ID="cvalConfirmPassword" runat="server" Display="Dynamic" OnServerValidate="cvalConfirmPassword_ServerValidate" ValidationGroup="ValGroup1"></asp:CustomValidator>
                </td>
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

