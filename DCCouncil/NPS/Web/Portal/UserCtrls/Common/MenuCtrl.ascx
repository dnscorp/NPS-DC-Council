<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuCtrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Common.MenuCtrl" %>
<div class="menu">
    <div class="inner">
        <ul>
            <li><a href="<%= PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.Dashboard %>">Dashboard</a></li>
            <li>
                <a href="javascript:;">NPS Categories</a>
                <ul>
                    <asp:Repeater ID="rptrExpenditureCategories" runat="server" OnItemDataBound="rptrExpenditureCategories_ItemDataBound">
                        <ItemTemplate>
                            <li><a runat="server" id="lnkExpenditure"></a></li>
                        </ItemTemplate>
                    </asp:Repeater>
                    <% %>
                    <li><a href="<%= PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.RecurringTransactions%>">Recurring Transactions</a></li>
                    <li><asp:Literal ID="litPurchaseOrderLink" runat="server"></asp:Literal></li>
                    <%--<li><a href="<%= PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.PurchaseOrders%>">Purchase Orders</a></li>--%>
                    <%--<li><a href="~/Pages/Members/PurchaseOrder/ImportV2" runat="server">Import Purchase Order</a></li>--%>
                </ul>
            </li>
            <li>
                <a href="javascript:;">Reports</a>
                <ul>
                 
                    <li><a href="<%= PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.NPSReports%>">NPS Reports</a></li>
                   <li><a href="<%= PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.AdhocReports%>">AdHoc Reports</a></li>
                    <li><a href="<%= PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.ExpenditureCategoryReports%>">Expenditure Category Reports</a></li>
                </ul>
            </li>
            <li>
                <a href="javascript:;">Site Administration</a>
                <ul>
                    <%--<li><a href="<%= PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.SiteSettings%>">Site Settings</a></li>--%>
                    <li><a href="<%= PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.UserManagement%>">User Management</a></li>
                    <li><a href="<%= PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.OfficeManagement%>">Office Management</a></li>
                    <li><a href="<%= PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.FiscalYearManagement%>">Fiscal Year Management</a></li>
                    <li><a href="<%= PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.ExpenditureCategoryManagement%>">Expenditure Category Management</a></li>
                    <li><a href="<%= PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.VendorManagement%>">Vendor Management</a></li>
                </ul>
            </li>
        </ul>
    </div>
</div>
