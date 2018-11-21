<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StaffMangementItemCtrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.SiteAdministration.StaffManagement.StaffMangementItemCtrl" %>
<div id="divItemPopup" runat="server">
    <asp:HiddenField ID="hfMode" runat="server" />
    <asp:HiddenField ID="hfStaffId" runat="server" />
    <asp:HiddenField ID="hfOfficeId" runat="server" />
    <table cellpadding="0" cellspacing="0" class="form">
        <tr>
            <th><span class="required">*</span>First Name</th>
            <td>
                <asp:TextBox ID="txtStaffFirstName" runat="server"></asp:TextBox></td>
            <td>
                <asp:CustomValidator CssClass="error" ID="cvalStaffFirstName" runat="server" OnServerValidate="cvalStaffFirstName_ServerValidate" Display="Dynamic" ValidationGroup="ValGroup1"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <th><span class="required">*</span>Last Name</th>
            <td>
                <asp:TextBox ID="txtStaffLastName" runat="server"></asp:TextBox></td>
            <td>
                <asp:CustomValidator CssClass="error" ID="cvalStaffLastName" runat="server" OnServerValidate="cvalStaffLastName_ServerValidate" Display="Dynamic" ValidationGroup="ValGroup1"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <th>Has Staff Level Expenditures?</th>
            <td>
                <asp:Checkbox ID="chkHasStaffLevelExpenditures" runat="server"></asp:Checkbox></td>
            <td>
            </td>
        </tr>
         <tr>
            <th><span class="required">*</span>Wireless Number</th>
            <td>
                <asp:TextBox ID="txtWirelessNumber" runat="server"></asp:TextBox></td>
            <td>
                <asp:CustomValidator CssClass="error" ID="cvalWirelessNumber" runat="server" OnServerValidate="cvalWirelessNumber_ServerValidate" Display="Dynamic" ValidationGroup="ValGroup1"></asp:CustomValidator>
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
                <asp:CustomValidator CssClass="error" ID="cvalActiveTo" runat="server" OnServerValidate="cvalActiveTo_ServerValidate" Display="Dynamic" ValidationGroup="ValGroup1"></asp:CustomValidator></td>
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
