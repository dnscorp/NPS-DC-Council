﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrdersCtrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Members.PurchaseOrdersCtrl" %>
<%@ Register Src="~/UserCtrls/Members/PurchaseOrderItemCtrl.ascx" TagName="PurchaseOrderItemCtrl" TagPrefix="uc1" %>
<%@ Register Src="~/UserCtrls/Common/PagerCtrl.ascx" TagName="PagerCtrl" TagPrefix="uc2" %>
<%@ Register Src="~/UserCtrls/Common/PopupCtrl.ascx" TagName="PopupCtrl" TagPrefix="uc3" %>
<h1 class="page-header">
    <asp:Literal ID="litPageHeading" runat="server"></asp:Literal>
    <a href="~/Pages/Members/PurchaseOrder/Import" class="importpurchaseorder btn" runat="server" id="lnkImportPurchaseOrders">Import Purchase Order</a>
</h1>
<div class="popup-parent">
    <div class="popup-trigger">
        <div class="search-query">
            <%--<asp:TextBox ID="txtSearch" runat="server" AutoPostBack="true" onchange="" OnTextChanged="txtSearch_TextChanged"></asp:TextBox>--%>

            <div class="search-text-loading">
                <asp:TextBox ID="txtSearch" autocomplete="off" runat="server"></asp:TextBox><div class="ajax-loader"></div>
            </div>
            <asp:DropDownList ID="ddlOfficeFilter" runat="server" AutoPostBack="true"></asp:DropDownList>
            <a href="javascript:;" class="btn" id="lnkReset" runat="server">Reset</a>
        </div>
        <asp:UpdatePanel ID="upMessage" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="sucess-msg">
                    <asp:Literal ID="litSuccessMessage" runat="server"></asp:Literal>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="upSearchResults" runat="server" UpdateMode="Conditional">
            <%--<Triggers>
                <asp:AsyncPostBackTrigger ControlID="txtSearch" />
            </Triggers>--%>
            <ContentTemplate>
                <div class="create-new-button-container">
                    <a id="lnkCreate" runat="server" onserverclick="lnkCreate_ServerClick" visible="false">
                        <asp:Literal ID="litCreateLink" runat="server"></asp:Literal></a>
                </div>
                <asp:Button ID="bttn" Style="display: none" Text="Submit" OnClick="txtSearch_TextChanged" runat="server" />
                <asp:HiddenField ID="hfSortField" runat="server" Value="DateOfTransaction" />
                <asp:HiddenField ID="hfOrderByDirection" runat="server" Value="Descending" />
                <asp:Repeater ID="rptrResult" runat="server" OnItemDataBound="rptrResult_ItemDataBound" OnItemCommand="rptrResult_ItemCommand">
                    <HeaderTemplate>
                        <table cellspacing="0" cellpadding="0" class="grid">
                            <tr>
                                <%--<th>
                                    <div class="create-new-button"></div>
                                </th>--%>
                                <th>
                                    <asp:LinkButton ID="lBtnDateOfTransaction" runat="server" CommandName="DateOfTransaction" Text="Date Of Transaction"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnVendorName" runat="server" CommandName="VendorName" Text="Vendor Name"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnObjCode" runat="server" CommandName="OBJCode" Text="OBJ"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnPONumber" runat="server" CommandName="PONumber" Text="PO Number"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnDescription" runat="server" CommandName="Description" Text="Description"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnPOAmtSum" runat="server" CommandName="POAmtSum" Text="POAmtSum"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnPOAdjAmtSum" runat="server" CommandName="POAdjAmtSum" Text="POAdjAmtSum"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnVoucherAmtSum" runat="server" CommandName="VoucherAmtSum" Text="VoucherAmtSum"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnPOBalSum" runat="server" CommandName="POBalSum" Text="POBalSum"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnName" runat="server" CommandName="OfficeName" Text="Office"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnYear" runat="server" CommandName="Year" Text="Year"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnExpSubCategory" runat="server" CommandName="ExpSubCategory" Text="Exp. Sub-Category"></asp:LinkButton>
                                </th>
                                <th>&nbsp;</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <%--<td></td>--%>
                            <td>
                                <asp:Literal ID="litDateOfTransaction" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litVendorName" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litObjCode" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litPONumber" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litDescription" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litPOAmtSum" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litPOAdjAmtSum" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litVoucherAmtSum" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litPOBalSum" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litName" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litYear" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litExpSubCategory" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <div class="submenu-container">
                                    <div class="submenu-heading" onclick="actionClick(this);">
                                        <a href="javascript:;">Action <em></em></a>
                                    </div>
                                    <div class="small-submenu">
                                        <em></em>
                                        <a id="lnkEdit" runat="server" href="javascript:;" onserverclick="lnkEdit_ServerClick">Edit</a>
                                        <a id="lnkDelete" runat="server" href="javascript:;" onserverclick="lnkDelete_ServerClick">Delete</a>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                <div class="no-record">
                    <asp:Literal ID="litNoResults" runat="server"></asp:Literal>
                </div>
                <uc2:PagerCtrl ID="PagerCtrl1" runat="server" PageSize="100" OnBindMainRepeater="PagerCtrl1_BindMainRepeater" />
                <uc3:PopupCtrl ID="PopupCtrl1" runat="server" OnOkayButtonClick="PopupCtrl1_OkayButtonClick" Visible="false" OnCancelButtonClick="PopupCtrl1_CancelButtonClick" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="ctrldimmer"></div>
    </div>
    <div class="popup-control">
        <asp:UpdatePanel ID="upItemPopup" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="lnkCreate" EventName="ServerClick" />
            </Triggers>
            <ContentTemplate>
                <uc1:PurchaseOrderItemCtrl ID="PurchaseOrderItemCtrl1" runat="server" OnSubmitClick="PurchaseOrderItemCtrl1_SubmitClick" OnCancelClick="PurchaseOrderItemCtrl1_CancelClick" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
<script>
    $("#<%=txtSearch.ClientID%>").keyup(function (event) {
        this.onchange();
    });

    $("#<%=lnkReset.ClientID%>").click(function (event) {
        $("#<%=txtSearch.ClientID%>").val("");
        $("#<%=ddlOfficeFilter.ClientID%>").val("0");
        $("#<%=txtSearch.ClientID%>").trigger('change');
    });
</script>




