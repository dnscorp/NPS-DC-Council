﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrderItemV2Ctrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Members.PurchaseOrderItemV2Ctrl" %>
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
            <th>Accounting Date
            </th>
            <td>                
                <asp:Literal ID="litAccountingDate" runat="server"></asp:Literal>
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
            <th>PO Number
            </th>
            <td>
                <asp:Literal ID="litPONumber" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <th>PO Line Number
            </th>
            <td>
                <asp:Literal ID="litPOLineNumber" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <th>PO Description
            </th>
            <td>
                <asp:Literal ID="litPOLineDescription" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <th>Expended Amount
            </th>
            <td>
                <asp:Literal ID="litExpendedAmount" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <th>PO Balance
            </th>
            <td>
                <asp:Literal ID="litPOBalance" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <th style="vertical-align:top">Description</th>
            <td>
                <asp:TextBox Width="100%" ID="txtDescription" runat="server"></asp:TextBox><br />
                <span style="font-size:12px;">Note: Updating this PO description will update the PO description for all the line items (if present) for this PO Number</span>
            </td>
        </tr>
         <tr>
            <th style="vertical-align:top">Associate a different Office</th>
            <td>
                <asp:DropDownList ID="ddlAlternateOffice" runat="server"></asp:DropDownList><br />
                <span style="font-size:12px;">Note: Selecting a different office from this dropdown will update the office association for all the line items (if present) for this PO Number</span>
            </td>
        </tr>
        <tr>
            <th><span class="required">*</span>Expenditure Sub-Category</th>
            <td>
                <asp:DropDownList ID="ddlExpenditureSubCategory" runat="server"></asp:DropDownList><br />
                <asp:CustomValidator CssClass="error" ID="cvalExpenditureSubCategory" runat="server" Display="Dynamic" ValidationGroup="ValGroup1" OnServerValidate="cvalExpenditureSubCategory_ServerValidate"></asp:CustomValidator>
            </td>
        </tr>
        <tr runat="server" id="trTrainingExpense">
            <th>Training Related Expense
            </th>
            <td>
                <asp:CheckBox ID="chkTrainingExpense" runat="server" />
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
