<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExpenditureCategoryManagementItemCtrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.SiteAdministration.ExpenditureCategoryManagement.ExpenditureCategoryManagementItemCtrl" %>
<div id="divItemPopup" runat="server">
    <asp:HiddenField ID="hfMode" runat="server" />
    <asp:HiddenField ID="hfCategoryId" runat="server" />
    <table cellpadding="0" cellspacing="0" class="form">
        <tr>
            <th><span class="required">*</span>Category Name</th>
            <td>
                <asp:TextBox ID="txtCategoryName" runat="server"></asp:TextBox></td>
            <td>
                <asp:CustomValidator CssClass="error" ID="cvalCategoryName" runat="server" OnServerValidate="cvalCategoryName_ServerValidate" Display="Dynamic" ValidationGroup="ValGroup1"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <th><span class="required">*</span>Category Code</th>
            <td>
                <asp:TextBox ID="txtCategoryCode" runat="server"></asp:TextBox></td>
            <td>
                <asp:CustomValidator CssClass="error" ID="cvalCategoryCode" runat="server" OnServerValidate="cvalCategoryCode_ServerValidate" Display="Dynamic" ValidationGroup="ValGroup1"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <th>Is Fixed?</th>
            <td>
                <asp:RadioButtonList ID="rblIsFixed" RepeatColumns="2" runat="server">
                    <asp:ListItem Value="True" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="False" Text="No"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <th>Is Staff Level?</th>
            <td>
                <asp:RadioButtonList ID="rblIsStaffLevel" RepeatColumns="2" runat="server">
                    <asp:ListItem Value="True" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="False" Text="No"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <th>Is the Vendor Staff itself?</th>
            <td>
                <asp:RadioButtonList ID="rblIsVendorStaff" RepeatColumns="2" runat="server">
                    <asp:ListItem Value="True" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="False" Text="No"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td>
            </td>
        </tr>    
         <tr>
            <th>Are the Expenditures to be entered on a Monthly basis?</th>
            <td>
                <asp:RadioButtonList ID="rblIsMonthly" RepeatColumns="2" runat="server">
                    <asp:ListItem Value="True" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="False" Text="No"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td>
            </td>
        </tr>        
        <tr>
            <th>Is Active?</th>
            <td>
                <asp:RadioButtonList ID="rblIsActive" RepeatColumns="2" runat="server">
                    <asp:ListItem Value="True" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="False" Text="No"></asp:ListItem>
                </asp:RadioButtonList>
            <td>
            </td>
        </tr>
        <tr>
            <th>Should the Month name be appended in the Type Column of NPS Transaction Sheet?</th>
            <td>
                <asp:RadioButtonList ID="rblAppendMonth" RepeatColumns="2" runat="server">
                    <asp:ListItem Value="True" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="False" Text="No"></asp:ListItem>
                </asp:RadioButtonList>
            <td>
            </td>
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



