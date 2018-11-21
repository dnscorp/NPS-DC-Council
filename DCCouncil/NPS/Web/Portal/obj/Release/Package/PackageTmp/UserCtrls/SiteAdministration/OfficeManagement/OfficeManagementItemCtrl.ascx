<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OfficeManagementItemCtrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.SiteAdministration.OfficeManagement.OfficeManagementItemCtrl" %>
<div id="divItemPopup" runat="server">
    <asp:HiddenField ID="hfMode" runat="server" />
    <asp:HiddenField ID="hfOfficeId" runat="server" />
    <table cellpadding="0" cellspacing="0" class="form">
        <tr>
            <th><span class="required">*</span>Office Name</th>
            <td>
                <asp:TextBox ID="txtOfficeName" runat="server"></asp:TextBox></td>
            <td>
                <asp:CustomValidator CssClass="error" ID="cvalOfficeName" runat="server" OnServerValidate="cvalOfficeName_ServerValidate" Display="Dynamic" ValidationGroup="ValGroup1"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <th><span class="required">*</span>Active From</th>
            <td>
                <asp:TextBox ID="txtActiveFrom" runat="server"></asp:TextBox>
                <asp:CalendarExtender
                    ID="CalendarExtender1" 
                    TargetControlID="txtActiveFrom"
                    runat="server" />
            </td>
            <td>
                <asp:CustomValidator CssClass="error" ID="cvalActiveFrom" runat="server" OnServerValidate="cvalActiveFrom_ServerValidate" Display="Dynamic" ValidationGroup="ValGroup1"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <th>Active To</th>
            <td>
                <asp:TextBox ID="txtActiveTo" runat="server"></asp:TextBox>
                <asp:CalendarExtender
                    ID="CalendarExtender2" 
                    TargetControlID="txtActiveTo"
                    runat="server" />
            </td>
            <td>
                <asp:CustomValidator CssClass="error" ID="cvalActiveTo" runat="server" OnServerValidate="cvalActiveTo_ServerValidate" Display="Dynamic" ValidationGroup="ValGroup1"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <th><span class="required">*</span>PCA</th>
            <td>
                <asp:TextBox ID="txtPCA" runat="server"></asp:TextBox></td>
            <td>
                <asp:CustomValidator CssClass="error" ID="cvalPCA" runat="server" OnServerValidate="cvalPCA_ServerValidate" Display="Dynamic" ValidationGroup="ValGroup1"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <th>PCA Title</th>
            <td>
                <asp:TextBox ID="txtPCATitle" runat="server"></asp:TextBox></td>
            <td></td>
        </tr>
        <tr>
            <th><span class="required">*</span>Index Code</th>
            <td>
                <asp:TextBox ID="txtIndexCode" runat="server"></asp:TextBox></td>
            <td>
                <asp:CustomValidator CssClass="error" ID="cvalIndexCode" runat="server" OnServerValidate="cvalIndexCode_ServerValidate" Display="Dynamic" ValidationGroup="ValGroup1"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <th>Index Title</th>
            <td>
                <asp:TextBox ID="txtIndexTitle" runat="server"></asp:TextBox></td>
            <td></td>
        </tr>
        <tr>
            <th>Comp Code</th>
            <td>
                <asp:TextBox ID="txtCompCode" runat="server"></asp:TextBox></td>
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

