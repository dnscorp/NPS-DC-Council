<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrderItemCtrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Members.PurchaseOrderItemCtrl" %>
<div id="divItemPopup" runat="server">
    <asp:HiddenField ID="hfMode" runat="server" />
    <asp:HiddenField ID="hfPurchaseOrderId" runat="server" />
    <asp:HiddenField ID="hfFiscalYearId" runat="server" />
    <asp:Literal ID="litHeading" runat="server"></asp:Literal>
    <table cellpadding="0" cellspacing="0" class="form">
        <tr>
            <th>Office
            </th>
            <td>
             <%--   <asp:ComboBox runat="server" ID="ddlOffices" AutoPostBack="true" DropDownStyle="DropDownList" AutoCompleteMode="SuggestAppend" CaseSensitive="false" RenderMode="Inline" OnSelectedIndexChanged="ddlOffices_SelectedIndexChanged"></asp:ComboBox>--%>
                <asp:Literal ID="litOffice" runat="server"></asp:Literal>                 
            </td>
        </tr>
       <%-- <tr id="trBudget" runat="server">
            <th><span class="required">*</span> Budget
            </th>
            <td>
                <asp:DropDownList ID="ddlBudgets" runat="server"></asp:DropDownList>
                           <asp:CustomValidator CssClass="error" ID="cvalBudget" runat="server" Display="Dynamic" ValidationGroup="ValGroup1" OnServerValidate="cvalBudget_ServerValidate"></asp:CustomValidator>
            </td>
        </tr>--%>
        <tr>
            <th>Date Of Transaction
            </th>
            <td>                
                <asp:Literal ID="litDateOfTransaction" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <th>Vendor Name
            </th>
            <td>
                <asp:Literal ID="litVendorName" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <th>OBJ Code
            </th>
            <td>
                <asp:Literal ID="litOBJCode" runat="server"></asp:Literal>
            </td>

        </tr>
        <tr>
            <th>PO Number
            </th>
            <td>
                <asp:Literal ID="litPONumber" runat="server"></asp:Literal>
            </td>

        </tr>

        <tr>
            <th>PO AmountSum
            </th>
            <td>
                <asp:Literal ID="litPOAmountSum" runat="server"></asp:Literal>
            </td>

        </tr>
        <tr>
            <th>POAdj AmountSum
            </th>
            <td>
                <asp:Literal ID="litPOAdjAmountSum" runat="server"></asp:Literal>
            </td>

        </tr>

        <tr>
            <th>Voucher Amount Sum
            </th>
            <td>
                <asp:Literal ID="litVoucherAmountSum" runat="server"></asp:Literal>
            </td>

        </tr>
        <tr>
            <th>PO Balance Sum
            </th>
            <td>
                <asp:Literal ID="litPOBalSum" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <th>Description</th>
            <td>
                <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <th>Associate a different Office</th>
            <td>
                <asp:DropDownList ID="ddlAlternateOffice" runat="server"></asp:DropDownList>
            </td>
        </tr>


        <tr class="form-button">
            <th></th>
            <td >
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
