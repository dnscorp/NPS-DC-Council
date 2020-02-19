<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExpenditureItemCtrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Common.ExpenditureItemCtrl" %>
<div id="divItemPopup" runat="server">
    <asp:HiddenField ID="hfMode" runat="server" />
    <asp:HiddenField ID="hfExpenditureId" runat="server" />
    <asp:HiddenField ID="hfCategoryCode" runat="server" />
    <asp:HiddenField ID="hfExpenditureSubCategoryCode" runat="server" />
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
                <%--<asp:ComboBox runat="server" ID="ddlOffices" AutoPostBack="true" DropDownStyle="Simple"  AutoCompleteMode="SuggestAppend" CaseSensitive="false" RenderMode="Block"  OnSelectedIndexChanged="ddlOffices_SelectedIndexChanged"></asp:ComboBox>--%>
                <asp:Literal ID="litOffice" runat="server"></asp:Literal>
                <asp:DropDownList ID="ddlOffices" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlOffices_SelectedIndexChanged"></asp:DropDownList>
            </td>
            <td>
                <asp:CustomValidator CssClass="error" ID="cvalOffice" runat="server" Display="Dynamic" ValidationGroup="ValGroup1" OnServerValidate="cvalOffice_ServerValidate"></asp:CustomValidator>
            </td>
        </tr>
        <%--<tr id="trBudget" runat="server">
            <th>Budget
            </th>
            <td>
                <asp:DropDownList ID="ddlBudgets" runat="server"></asp:DropDownList>
            </td>
            <td>
                <asp:CustomValidator CssClass="error" ID="cvalBudget" runat="server" Display="Dynamic" ValidationGroup="ValGroup1" OnServerValidate="cvalBudget_ServerValidate"></asp:CustomValidator>
            </td>
        </tr>--%>
        <tr id="trDateOfTransaction" runat="server">
            <th>
                <span class="required">*</span><asp:Literal ID="litDateOfTransactionFieldText" runat="server"></asp:Literal>
            </th>
            <td>
                <asp:TextBox ID="txtDateOfTransaction" runat="server"></asp:TextBox>
                <asp:CalendarExtender
                    ID="CalendarExtender1" 
                    TargetControlID="txtDateOfTransaction"
                    runat="server" />
                <asp:DropDownList ID="ddlMonths" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlMonths_SelectedIndexChanged"></asp:DropDownList>
                <asp:Literal ID="litMonth" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:CustomValidator CssClass="error" ID="cvalDateOfTransaction" OnServerValidate="cvalDateOfTransaction_ServerValidate" runat="server" Display="Dynamic" ValidationGroup="ValGroup1"></asp:CustomValidator>
            </td>
        </tr>
        <asp:PlaceHolder ID="phIsFixed" runat="server">            
            <tr runat="server" id="trVendorName">
                <th>
                    <span class="required">*</span><asp:Literal ID="litVendorNameFieldText" runat="server"></asp:Literal>
                </th>
                <td>
                    <asp:DropDownList ID="ddlStaffs" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStaffs_SelectedIndexChanged"></asp:DropDownList>
                    <asp:TextBox ID="txtVendorName" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:CustomValidator CssClass="error" ID="cvalVendorName" runat="server" Display="Dynamic" ValidationGroup="ValGroup1" OnServerValidate="cvalVendorName_ServerValidate"></asp:CustomValidator>
                </td>
            </tr>
            <tr runat="server" id="trIsVendor" visible="false">
                <th>Vendor</th>
                <td><asp:CheckBox ID="chkIsVendor" runat="server" /></td>
                <td></td>
            </tr>
            <tr>
                <th>Description
                </th>
                <td>
                    <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
                </td>
                <td>&nbsp;
                </td>
            </tr>

            <tr>
                <th><span class="required">*</span>OBJ
                </th>
                <td>
                    <asp:TextBox ID="txtOBJCode" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:CustomValidator CssClass="error" ID="cvalOBJCode" runat="server" Display="Dynamic" ValidationGroup="ValGroup1" OnServerValidate="cvalOBJCode_ServerValidate"></asp:CustomValidator>
                </td>
            </tr>
        </asp:PlaceHolder>

        
        <asp:PlaceHolder ID="phStaffLevel" runat="server" Visible="false">
            <tr class="sub-form-border">
                <th><strong>Staffwise Amount</strong></th>
                <td colspan="2"></td>
            </tr>
            <asp:Repeater ID="rptrStaffLevelAmount" runat="server" OnItemDataBound="rptrStaffLevelAmount_ItemDataBound">
                <HeaderTemplate>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <th>
                            <asp:HiddenField ID="hfStaffID" runat="server" />
                            <asp:Literal ID="litStaffName" runat="server"></asp:Literal>
                        </th>
                        <td>
                            <asp:TextBox ID="txtStaffLevelAmount" runat="server" OnTextChanged="txtStaffLevelAmount_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:CustomValidator CssClass="error" ID="cvalStaffLevelAmount" runat="server" Display="Dynamic" ValidationGroup="ValGroupStaffLevelAmount" OnServerValidate="cvalStaffLevelAmount_ServerValidate"></asp:CustomValidator>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <tr class="sub-form-border-b">
                        <td colspan="2"></td>
                    </tr>
                </FooterTemplate>

            </asp:Repeater>

        </asp:PlaceHolder>
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
        <tr runat="server" id="trTrainingExpense">
            <th>Training Related Expense
            </th>
            <td>
                <asp:CheckBox ID="chkTrainingExpense" runat="server" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <th><span class="required">*</span>Expenditure Sub-Category</th>
            <td>
                <asp:DropDownList ID="ddlExpenditureSubCategory" runat="server"></asp:DropDownList>
            </td>
            <td>
                <asp:CustomValidator CssClass="error" ID="cvalExpenditureSubCategory" runat="server" Display="Dynamic" ValidationGroup="ValGroup1" OnServerValidate="cvalExpenditureSubCategory_ServerValidate"></asp:CustomValidator>
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
<script>
    function StaffLevelAmountOnLostFocus(txtAmountClientId) {
        $("#" + txtAmountClientId).trigger('change');
    }
</script>




