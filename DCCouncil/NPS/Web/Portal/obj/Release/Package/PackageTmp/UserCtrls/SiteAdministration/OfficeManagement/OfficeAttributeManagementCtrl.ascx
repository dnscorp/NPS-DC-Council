<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OfficeAttributeManagementCtrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.SiteAdministration.OfficeManagement.OfficeAttributeManagementCtrl" %>
<div id="divItemPopup" runat="server">
    <asp:HiddenField ID="hfMode" runat="server" />
    <asp:HiddenField ID="hfOfficeId" runat="server" />
    <table cellpadding="0" cellspacing="0" class="form">
        <asp:Repeater ID="rptrOfficeAttribute" OnItemDataBound="rptrOfficeAttribute_ItemDataBound" runat="server">
            <ItemTemplate>
                <tr>

                    <th><span class="required">*</span>
                        <asp:Label ID="lblAttributeName" runat="server"></asp:Label><asp:HiddenField ID="hfofficeAttributeLookUpID" runat="server" />
                    </th>
                    <td>
                        <asp:TextBox ID="txtAttributeValue" runat="server"></asp:TextBox></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <tr>
            <td colspan="2">
                <div class="error-msg">
                    <asp:Literal ID="litErrorMessage" runat="server" Visible="false"></asp:Literal>
                </div>
                <asp:CustomValidator ID="CustomValidator1" ValidationGroup="ValGroup2" OnServerValidate="txtAttributeValueVal_ServerValidate" runat="server"></asp:CustomValidator>
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
