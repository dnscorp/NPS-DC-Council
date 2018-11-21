<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExpendituresCtrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Common.ExpendituresCtrl" %>
<%@ Register Src="~/UserCtrls/Common/PagerCtrl.ascx" TagName="PagerCtrl" TagPrefix="uc2" %>
<%@ Register Src="~/UserCtrls/Common/PopupCtrl.ascx" TagName="PopupCtrl" TagPrefix="uc3" %>
<%@ Register Src="~/UserCtrls/Common/ExpenditureItemCtrl.ascx" TagName="ExpenditureItemCtrl" TagPrefix="uc1" %>
<h1 class="page-header">
    <asp:Literal ID="litPageHeading" runat="server"></asp:Literal>
    <a href="~/Pages/Members/TelephoneTransactions/Import.aspx?Code=TC" class="importpurchaseorder btn" runat="server" id="lnkImportTelephoneTransactions">Import Telephone Transactions</a>
</h1>
<%--<asp:UpdatePanel ID="upSearchBar" runat="server" UpdateMode="Conditional">
    <ContentTemplate>--%>
<div class="popup-parent">
    <div class="popup-trigger">
        <div class="search-query">
            <asp:PlaceHolder ID="phSearchBar" runat="server">
                <div class="search-text-loading">
                    <asp:TextBox ID="txtSearch" autocomplete="off" runat="server"></asp:TextBox><div class="ajax-loader"></div>
                </div>
                <%-- <asp:TextBox ID="txtSearch" runat="server" AutoPostBack="true" OnTextChanged="txtSearch_TextChanged"></asp:TextBox>--%>
                <asp:DropDownList ID="ddlOfficeFilter" runat="server" AutoPostBack="true"></asp:DropDownList>
                <a href="javascript:;" class="btn" id="lnkReset" runat="server">Reset</a>
            </asp:PlaceHolder>
        </div>
        <%--    </ContentTemplate>
</asp:UpdatePanel>--%>

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
                    <a id="lnkCreate" runat="server" onserverclick="lnkCreate_ServerClick">
                        <asp:Literal ID="litCreateLink" runat="server"></asp:Literal></a>
                </div>
                <asp:Button ID="bttn" Style="display: none" Text="Submit" OnClick="txtSearch_TextChanged" runat="server" />
                <asp:HiddenField ID="hfSortField" runat="server" Value="DateOfTransaction" />
                <asp:HiddenField ID="hfOrderByDirection" runat="server" Value="Descending" />
                <asp:Repeater ID="rptrResult" runat="server" OnItemDataBound="rptrResult_ItemDataBound" OnItemCommand="rptrResult_ItemCommand">
                    <HeaderTemplate>
                        <table cellspacing="0" cellpadding="0" class="grid">
                            <tr>
                                <th>
                                    <div class="create-new-button"></div>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnDateOfTransaction" runat="server" CommandName="DateOfTransaction"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnOfficeName" runat="server" CommandName="VendorName" Text="Office Name"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnVendorName" runat="server" CommandName="VendorName"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnDescription" runat="server" CommandName="Description" Text="Description"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnObjCode" runat="server" CommandName="OBJCode" Text="OBJ"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnIndex" runat="server" CommandName="IndexCode" Text="Index"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnPCA" runat="server" CommandName="PCA" Text="PCA"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnAmount" runat="server" CommandName="Amount" Text="Amount"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnDateOfTransaction2" runat="server" CommandName="DateOfTransaction"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnComments" runat="server" CommandName="Comments" Text="Comments"></asp:LinkButton>
                                </th>
                                <th>&nbsp;</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td></td>
                            <td>
                                <asp:Literal ID="litDateOfTransaction" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litOfficeName" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litVendorName" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litDescription" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litObjCode" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litIndex" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litPCA" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litAmount" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litDateOfTransaction2" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litComments" runat="server"></asp:Literal>
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
                <uc1:ExpenditureItemCtrl ID="ExpenditureItemCtrl1" runat="server" OnSubmitClick="ExpenditureItemCtrl1_SubmitClick" OnCancelClick="ExpenditureItemCtrl1_CancelClick" />
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

