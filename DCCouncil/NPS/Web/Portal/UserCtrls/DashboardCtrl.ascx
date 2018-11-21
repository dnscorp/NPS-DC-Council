<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DashboardCtrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.DashboardCtrl" %>
<%@ Register Src="~/UserCtrls/Common/PagerCtrl.ascx" TagName="PagerCtrl" TagPrefix="uc2" %>

<div class="dashboard-ctrl">
    <div class="dashboard-ctrl-links">
        <ul class="thumbnails">
            <li class="span3">
                <a href="<%= PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.Expenditures + "?Code=PCE"%>">
                    <div class="thumbnail">
                        <img alt="" src="/images/petty.jpg">
                        <p>Petty Cash Expenditures</p>
                    </div>
                </a>
            </li>
            <li class="span3">
                <a href="<%= PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.PurchaseOrders%>">
                    <div class="thumbnail">
                        <img alt="" src="/images/po.jpg">
                        <p>Purchase Orders</p>
                    </div>
                </a>
            </li>
            <li class="span3">
                <a href="<%= PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.Expenditures + "?Code=PC"%>">
                    <div class="thumbnail">
                        <img alt="" src="/images/postage.jpg">
                        <p>Postage</p>
                    </div>
                </a>
            </li>
            <li class="span3">
                <a href="<%= PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.Expenditures + "?Code=DV"%>">
                    <div class="thumbnail">
                        <img alt="" src="/images/voucher.jpg">
                        <p>Direct Vouchers</p>
                    </div>
                </a>
            </li>
            <li class="span3">
                <a href="<%= PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.Expenditures + "?Code=PCard"%>">
                    <div class="thumbnail">
                        <img alt="" src="/images/pcard.jpg">
                        <p>P-Card Transactions</p>
                    </div>
                </a>
            </li>
            <li class="span3">
                <a href="<%= PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.Expenditures + "?Code=TC"%>">
                    <div class="thumbnail">
                        <img alt="" src="/images/cell.jpg">
                        <p>Cell Phone Charges</p>
                    </div>
                </a>
            </li>
            <li class="span3">
                <a href="<%= PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.NPSReports %>">
                    <div class="thumbnail">
                        <img alt="" src="/images/reports.jpg">
                        <p>Reports</p>
                    </div>
                </a>
            </li>
            <li class="span3">
                <a href="<%= PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.SiteSettings%>">
                    <div class="thumbnail ">
                        <img alt="" src="/images/settings.jpg">
                        <p>Settings</p>
                    </div>
                </a>
            </li>
        </ul>
    </div>

    
    <h1 class="page-header">Non-Personal Spending</h1>
    <div class="popup-parent">
        <div class="popup-trigger">
            <div class="search-query">
                <%--  <asp:TextBox ID="txtSearch" runat="server" AutoPostBack="true" OnTextChanged="txtSearch_TextChanged"></asp:TextBox>--%>
                <div class="search-text-loading">
                    <asp:TextBox ID="txtSearch" autocomplete="off" runat="server"></asp:TextBox><div class="ajax-loader"></div>
                </div>
                <a href="javascript:;" class="btn" id="lnkReset" runat="server">Reset</a>
            </div>
            <asp:UpdatePanel ID="upSearchResults" runat="server" UpdateMode="Conditional">
                <%--<Triggers>
                <asp:AsyncPostBackTrigger ControlID="txtSearch" />
            </Triggers>--%>
                <ContentTemplate>
                    <div id="spanInfo" runat="server">
                    </div>
                    <asp:Button ID="bttn" Style="display: none" Text="Submit" OnClick="txtSearch_TextChanged" runat="server" />
                    <asp:HiddenField ID="hfSortField" runat="server" Value="OfficeName" />
                    <asp:HiddenField ID="hfOrderByDirection" runat="server" Value="Ascending" />
                    <asp:Repeater ID="rptrResult" runat="server" OnItemDataBound="rptrResult_ItemDataBound" OnItemCommand="rptrResult_ItemCommand">
                        <HeaderTemplate>
                            <table cellspacing="0" cellpadding="0" class="grid">
                                <tr>
                                    <th>
                                        <asp:LinkButton ID="lBtnOfficeName" runat="server" CommandName="OfficeName" Text="Office"></asp:LinkButton>
                                    </th>
                                    <th>
                                        <asp:LinkButton ID="lBtnBudget" runat="server" CommandName="TotalBudgetAmount" Text="Budget"></asp:LinkButton>
                                    </th>
                                    <th>
                                        <asp:LinkButton ID="lBtnExpenditure" runat="server" CommandName="TotalExpenditureAmount" Text="Spent"></asp:LinkButton>
                                    </th>
                                    <th>
                                        <asp:LinkButton ID="lBtnBurnRate" runat="server" CommandName="BurnRate" Text="Burn Rate"></asp:LinkButton>
                                    </th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Literal ID="litOfficeName" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="litBudgetAmount" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="litExpenditureAmount" runat="server"></asp:Literal>
                                </td>
                                <td style="width: 160px;">
                                    <div class="perc-bar-conatiner">
                                        <div class="perc-label">
                                            <asp:Literal ID="litBurnRate1" runat="server"></asp:Literal>
                                        </div>
                                        <div class="perc-bar" runat="server" id="divPercBar">
                                            <div class="perc-label perc-label2">
                                                <asp:Literal ID="litBurnRate2" runat="server"></asp:Literal>
                                            </div>
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
                    <uc2:PagerCtrl ID="PagerCtrl1" runat="server" PageSize="25" OnBindMainRepeater="PagerCtrl1_BindMainRepeater" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
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
