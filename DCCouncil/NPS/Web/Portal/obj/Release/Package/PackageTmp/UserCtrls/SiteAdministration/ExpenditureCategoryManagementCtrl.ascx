<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExpenditureCategoryManagementCtrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.SiteAdministration.ExpenditureCategoryManagementCtrl" %>
<%@ Register Src="~/UserCtrls/SiteAdministration/ExpenditureCategoryManagement/ExpenditureCategoryManagementItemCtrl.ascx" TagName="ExpenditureCategoryManagementItemCtrl" TagPrefix="uc1" %>
<%@ Register Src="~/UserCtrls/SiteAdministration/ExpenditureCategoryManagement/ExpenditureCategoryAttributeManagementCtrl.ascx" TagName="ExpenditureCategoryAttributeCtrl" TagPrefix="uc2" %>
<%@ Register Src="~/UserCtrls/Common/PagerCtrl.ascx" TagName="PagerCtrl" TagPrefix="uc2" %>
<%@ Register Src="~/UserCtrls/Common/PopupCtrl.ascx" TagName="PopupCtrl" TagPrefix="uc3" %>
<h1 class="page-header">Expenditure Category Management</h1>
<div class="popup-parent">
    <div class="popup-trigger">
        <div class="search-query">
            <%--<asp:TextBox ID="txtSearch" runat="server" AutoPostBack="true" OnTextChanged="txtSearch_TextChanged"></asp:TextBox>--%>
            <div class="search-text-loading">
                <asp:TextBox ID="txtSearch" autocomplete="off" runat="server"></asp:TextBox><div class="ajax-loader"></div>
            </div>
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
            <%--   <Triggers>
                <asp:AsyncPostBackTrigger ControlID="txtSearch" />
            </Triggers>--%>
            <ContentTemplate>
                <div class="create-new-button-container">
                    <a id="lnkCreate" runat="server" onserverclick="lnkCreate_ServerClick" visible="false">
                        <asp:Literal ID="litCreateLink" runat="server"></asp:Literal></a>
                </div>
                <asp:Button ID="bttn" Style="display: none" Text="Submit" OnClick="txtSearch_TextChanged" runat="server" />
                <asp:HiddenField ID="hfSortField" runat="server" Value="Name" />
                <asp:HiddenField ID="hfOrderByDirection" runat="server" Value="Descending" />
                <asp:Repeater ID="rptrResult" runat="server" OnItemDataBound="rptrResult_ItemDataBound" OnItemCommand="rptrResult_ItemCommand">
                    <HeaderTemplate>
                        <table cellspacing="0" cellpadding="0" class="grid">
                            <tr>                                
                                <th>
                                    <asp:LinkButton ID="lBtnCategoryName" runat="server" CommandName="Name" Text="Category Name"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnCategoryCode" runat="server" CommandName="Code" Text="Category Code"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnIsFixed" runat="server" CommandName="IsFixed" Text="Is Fixed?"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnIsStaffLevel" runat="server" CommandName="IsStaffLevel" Text="Is Staff Level?"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="IsVendorStaff" Text="Is Vendor Staff?"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandName="IsMonthly" Text="Is Monthly?"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="LinkButton3" runat="server" CommandName="IsSystemDefined" Text="Is System Defined?"></asp:LinkButton>
                                </th>
                                 <th>
                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandName="AppendMonth" Text="Append Month?"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnIsActive" runat="server" CommandName="IsActive" Text="Is Active?"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnCreatedDate" runat="server" CommandName="CreatedDate" Text="Created Date"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnUpdatedDate" runat="server" CommandName="UpdatedDate" Text="Updated Date"></asp:LinkButton>
                                </th>
                                <th>&nbsp;</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Literal ID="litCategoryName" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litCategoryCode" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litIsFixed" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litIsStaffLevel" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litIsVendorStaff" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litIsMonthly" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litIsSystemDefined" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litAppendMonth" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litIsActive" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litCreatedDate" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litUpdatedDate" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <div class="submenu-container">
                                    <div class="submenu-heading" onclick="actionClick(this);">
                                        <a href="javascript:;">Action<em></em></a>
                                    </div>
                                    <div class="small-submenu">
                                        <em></em>
                                        <a id="lnkEdit" runat="server" href="javascript:;" onserverclick="lnkEdit_ServerClick" visible="false">Edit</a>
                                        <a id="lnkEditAttributes" runat="server" href="javascript:;" onserverclick="lnkEditAttributes_ServerClick">Edit Attributes</a>
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
                <uc2:PagerCtrl ID="PagerCtrl1" runat="server" PageSize="10" OnBindMainRepeater="PagerCtrl1_BindMainRepeater" />
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
                <uc1:ExpenditureCategoryManagementItemCtrl ID="ExpenditureCategoryManagementItemCtrl1" runat="server" OnSubmitClick="ExpenditureCategoryManagementItemCtrl1_SubmitClick" OnCancelClick="ExpenditureCategoryManagementItemCtrl1_CancelClick" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="popup-control">
        <asp:UpdatePanel ID="upItemPopup2" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="lnkCreate" EventName="ServerClick" />
            </Triggers>
            <ContentTemplate>
                <uc2:ExpenditureCategoryAttributeCtrl ID="ExpenditureCategoryAttributeCtrl1" runat="server" OnSubmitClick="ExpenditureCategoryAttributeCtrl1_SubmitClick" OnCancelClick="ExpenditureCategoryAttributeCtrl1_CancelClick" />
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
        $("#<%=txtSearch.ClientID%>").trigger('change');
    });
</script>
