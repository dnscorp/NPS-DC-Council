<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NPSReportsCtrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Reports.NPSReportsCtrl" %>
<%@ Register Src="~/UserCtrls/Common/PagerCtrl.ascx" TagName="PagerCtrl" TagPrefix="uc2" %>
<%@ Register Src="~/UserCtrls/Common/PopupCtrl.ascx" TagName="PopupCtrl" TagPrefix="uc3" %>
<h1 class="page-header">Generate Non-Personal Spending Report</h1>
<%--<div class="search-query">
    <asp:TextBox ID="txtSearch" runat="server" AutoPostBack="true" OnTextChanged="txtSearch_TextChanged"></asp:TextBox>
    <a href="javascript:;" id="lnkReset" runat="server">Reset</a>
</div>--%>
<div class="popup-parent">
    <div class="popup-trigger">

        <div class="box">
            <table cellpadding="0" cellspacing="0" class="form">
                <tr>
                    <th>Fiscal Year selected
                    </th>
                    <td>
                        <asp:Literal ID="litSelectedFiscalYear" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <th>As of Date : 
                    </th>
                    <td>
                        <asp:TextBox ID="txtAsOfDate" autocomplete="off" runat="server"></asp:TextBox>
                        <asp:CalendarExtender
                            ID="CalendarExtender1"
                            TargetControlID="txtAsOfDate"
                            runat="server" />

                        <asp:CustomValidator CssClass="error" ID="cvalAsOfDate" runat="server" OnServerValidate="cvalAsOfDate_ServerValidate" Display="Dynamic" ValidationGroup="ValGroup1"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <th>Filters
                    </th>
                    <td>
                        <div class="dns-radio-buttons-wrapper">
                            <asp:RadioButtonList ID="rdoFilters" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server">
                                <asp:ListItem Selected="True" Value="0">Include Training</asp:ListItem>
                                <asp:ListItem Value="1">Exclude Training</asp:ListItem>
                                <asp:ListItem Value="2">Training Only</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </td>
                </tr>

            </table>
        </div>
        <div class="error-msg">
            <asp:Literal ID="litReportStatus" runat="server" Visible="false"></asp:Literal>
        </div>
        <asp:HiddenField ID="hfSortField" runat="server" Value="Name" />
        <asp:HiddenField ID="hfOrderByDirection" runat="server" Value="Ascending" />

        <asp:Repeater ID="rptrResult" runat="server" OnItemDataBound="rptrResult_ItemDataBound" OnItemCommand="rptrResult_ItemCommand">
            <HeaderTemplate>
                <table cellspacing="0" cellpadding="0" class="grid">
                    <tr>
                        <th>
                            <asp:LinkButton ID="lBtnOfficeName" runat="server" CommandName="Name" Text="Office Name"></asp:LinkButton>
                        </th>
                        <th>
                            <asp:LinkButton ID="lBtnActiveFrom" runat="server" CommandName="ActiveFrom" Text="Active From"></asp:LinkButton>
                        </th>
                        <th>
                            <asp:LinkButton ID="lBtnActiveTo" runat="server" CommandName="ActiveTo" Text="Active Till"></asp:LinkButton>
                        </th>
                        <th>
                            <asp:LinkButton ID="lBtnPCA" runat="server" CommandName="PCA" Text="PCA"></asp:LinkButton></th>
                        <th>
                            <asp:LinkButton ID="lBtnPCATitle" runat="server" CommandName="PCATitle" Text="PCA Title"></asp:LinkButton></th>
                        <th>
                            <asp:LinkButton ID="lBtnIndexCode" runat="server" CommandName="IndexCode" Text="Index Code"></asp:LinkButton></th>
                        <th>
                            <asp:LinkButton ID="lBtnIndexTitle" runat="server" CommandName="IndexTitle" Text="Index Title"></asp:LinkButton></th>
                        <th>
                            <asp:LinkButton ID="lBtnCreatedDate" runat="server" CommandName="CreatedDate" Text="Created Date"></asp:LinkButton></th>
                        <th>
                            <asp:LinkButton ID="lBtnUpdatedDate" runat="server" CommandName="UpdatedDate" Text="Updated Date"></asp:LinkButton></th>
                        <th>&nbsp;</th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:Literal ID="litOfficeName" runat="server"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal ID="litActiveFrom" runat="server"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal ID="litActiveTo" runat="server"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal ID="litPCA" runat="server"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal ID="litPCATitle" runat="server"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal ID="litIndexCode" runat="server"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal ID="litIndexTitle" runat="server"></asp:Literal>
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
                                <a href="javascript:;" runat="server" id="lnkGenerateNPSReport" onserverclick="lnkGenerateNPSReport_ServerClick">Generate NPS Report</a>
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
        <%--   <uc2:PagerCtrl ID="PagerCtrl1" runat="server" PageSize="10" OnBindMainRepeater="PagerCtrl1_BindMainRepeater" />--%>

        <%--<script>
            $("#<%=txtSearch.ClientID%>").keyup(function (event) {
                this.onchange();
            });

            $("#<%=lnkReset.ClientID%>").click(function (event) {
                $("#<%=txtSearch.ClientID%>").val("");
                $("#<%=txtSearch.ClientID%>").trigger('change');
            });
        </script>--%>
    </div>
</div>
