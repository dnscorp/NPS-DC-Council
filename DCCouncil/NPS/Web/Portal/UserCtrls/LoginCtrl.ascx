<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginCtrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.LoginCtrl" %>

<div class="login-ctrl">
    <h3>Login</h3>
     <p>
        <asp:Label ID="lblMsg" ForeColor="red" runat="server" />
    </p>
  
    <table cellpadding="0" cllspacing="0" class="form">
        <tr class="error-form">
            <td></td>
            <td>

                <asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                    ControlToValidate="txtUsername"
                    Display="Dynamic"
                    ErrorMessage="Username cannot be empty."
                    runat="server" CssClass="error" />
            </td>

        </tr>
        <tr>
            <th>Username</th>
            <td>
                <asp:TextBox ID="txtUsername" runat="server" /></td>

        </tr>
        <tr class="error-form">
            <th></th>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                    ControlToValidate="txtPassword"
                    ErrorMessage="Password cannot be empty."
                    runat="server" CssClass="error" />
            </td>
        </tr>
        <tr>
            <th>Password</th>
            <td>
                <asp:TextBox ID="txtPassword" TextMode="Password"
                    runat="server" />
            </td>

        </tr>
        <%--<tr>
        <td>Remember me?</td>
        <td>
            <asp:CheckBox ID="chkPersist" runat="server" /></td>
    </tr>--%>
        <tr>
            <th></th>
            <td>
                 <asp:Button ID="btnSubmit" OnClick="OnBtnSubmitClick" Text="Login"
        runat="server" />
   
            </td>

        </tr>
    </table>
   
</div>
