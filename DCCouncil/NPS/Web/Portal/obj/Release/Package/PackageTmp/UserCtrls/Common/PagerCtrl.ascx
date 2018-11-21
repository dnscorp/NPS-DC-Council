<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PagerCtrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Common.PagerCtrl" %>
<asp:HiddenField ID="hfCurrentPage" runat="server" Value="0" />
<asp:HiddenField ID="hfPageCount" runat="server" />
<div id="divPager" class="grid-pager" runat="server">
    <div class="grid-pager-current-page">
        <asp:Label ID="lblCurrentPage" runat="server"></asp:Label></div>
    <table class="grid-pager-ctrls">
        <tr>
            <td>
                <asp:Button ID="cmdFirst" runat="server" Text="<< First" OnClick="cmdFirst_Click">
                </asp:Button>&nbsp;
            </td>
            <td>
                <asp:Button ID="cmdPrev" runat="server" Text="< Prev" OnClick="cmdPrev_Click"></asp:Button>&nbsp;
            </td>
            <td class="grid-page-number">
                <asp:Repeater ID="rptPages" runat="server" OnItemCommand="rptPages_ItemCommand">
                    <HeaderTemplate>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr >
                                <td>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="btnPage" CommandName="Page" CommandArgument="<%# Container.DataItem %>"
                            runat="server"><%# Container.DataItem %>
                        </asp:LinkButton>&nbsp;
                    </ItemTemplate>
                    <FooterTemplate>
                        </td> </tr> </table>
                    </FooterTemplate>
                </asp:Repeater>
            </td>
            <td>
               <asp:Button ID="cmdNext" runat="server" Text="Next >" OnClick="cmdNext_Click">
                </asp:Button>
            </td>
            <td>
               <asp:Button ID="cmdLast" runat="server" Text="Last >>" OnClick="cmdLast_Click">
                </asp:Button>
            </td>
        </tr>
    </table>
</div>