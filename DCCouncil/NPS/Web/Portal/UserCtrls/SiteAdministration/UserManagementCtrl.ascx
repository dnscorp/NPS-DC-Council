﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserManagementCtrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.SiteAdministration.UserManagementCtrl" EnableViewState="false" %>
<%@ Register Src="~/UserCtrls/SiteAdministration/UserManagement/UserManagementItemCtrl.ascx" TagName="UserManagementItemCtrl" TagPrefix="uc1" %>
<%@ Register Src="~/UserCtrls/Common/PagerCtrl.ascx" TagName="PagerCtrl" TagPrefix="uc2" %>
<%@ Register Src="~/UserCtrls/Common/PopupCtrl.ascx" TagName="PopupCtrl" TagPrefix="uc3" %>
<script type="text/javascript">
    function ChangeUrl(newUrl) {
        window.location.href = newUrl;
    }
</script>
<h1 class="page-header">User Account Management</h1>

<div class="popup-parent">
    <div class="popup-trigger">
        <div class="search-query">
            <div class="search-text-loading">
                <asp:TextBox ID="txtSearch" autocomplete="off" runat="server"></asp:TextBox><div class="ajax-loader"></div>
            </div>
            <%--<asp:TextBox ID="txtSearch" runat="server" AutoPostBack="true" OnTextChanged="txtSearch_TextChanged"></asp:TextBox>--%>
            <a href="javascript:;" class="btn" id="lnkReset" runat="server">Reset</a>
        </div>
        <asp:UpdatePanel ID="upMessage" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="sucess-msg">
                    <asp:Literal ID="litSuccessMessage" runat="server"></asp:Literal>
                </div>
                <div class="error-msg">
                    <asp:Literal ID="litErrorMessage" runat="server" Visible="false"></asp:Literal>
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
                <asp:HiddenField ID="hfSortField" runat="server" Value="UserName" />
                <asp:HiddenField ID="hfOrderByDirection" runat="server" Value="Descending" />
                <asp:Repeater ID="rptrResult" runat="server" OnItemDataBound="rptrResult_ItemDataBound" OnItemCommand="rptrResult_ItemCommand">
                    <HeaderTemplate>
                        <table cellspacing="0" cellpadding="0" class="grid">
                            <tr>
                                <th>
                                    <div class="create-new-button"></div>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnUserName" runat="server" CommandName="Username" Text="Username"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnFirstName" runat="server" CommandName="FirstName" Text="First Name"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnLastName" runat="server" CommandName="LastName" Text="Last Name"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lBtnIsActive" runat="server" CommandName="IsActive" Text="IsActive"></asp:LinkButton></th>
                                <th>
                                    <asp:LinkButton ID="lBtnCreatedDate" runat="server" CommandName="CreatedDate" Text="Created Date"></asp:LinkButton></th>
                                <th>
                                    <asp:LinkButton ID="lBtnUpdatedDate" runat="server" CommandName="UpdatedDate" Text="Updated Date"></asp:LinkButton></th>
                                <th>&nbsp;</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td></td>
                            <td>
                                <asp:Literal ID="litUsername" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litFirstname" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litLastname" runat="server"></asp:Literal>
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
                <uc1:UserManagementItemCtrl ID="UserManagementItemCtrl1" runat="server" OnSubmitClick="UserManagementItemCtrl1_SubmitClick" OnCancelClick="UserManagementItemCtrl1_CancelClick" />
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
