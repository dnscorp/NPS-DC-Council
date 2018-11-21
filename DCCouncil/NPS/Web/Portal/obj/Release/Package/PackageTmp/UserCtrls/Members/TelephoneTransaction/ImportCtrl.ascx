<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportCtrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Members.TelephoneTransaction.ImportCtrl" %>
<asp:HiddenField ID="hfGuid" runat="server" />
<div>
    <div id="divUpload" runat="server">
        <table cellspacing="0" cellpadding="0" style="border-collapse: collapse;" class="wizard">
            <tbody>
                <tr style="height: 100%;">
                    <td>
                        <div class="error">
                            <asp:CustomValidator ID="cvalSelectFile" runat="server" OnServerValidate="cvalSelectFile_ServerValidate" ValidationGroup="ValGroupFile"></asp:CustomValidator><br />
                            <asp:CustomValidator ID="cvalFileType" runat="server" OnServerValidate="cvalFileType_ServerValidate" ValidationGroup="ValGroupFile"></asp:CustomValidator><br />
                            <asp:CustomValidator ID="cvalFileSize" runat="server" OnServerValidate="cvalFileSize_ServerValidate" ValidationGroup="ValGroupFile"></asp:CustomValidator>

                        </div>
                        <div class="wizard-item">
                            <h3>Upload the Telephone transaction report file to import</h3>
                            <asp:FileUpload ID="fuImportFile" runat="server" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <table cellspacing="5" cellpadding="5">
                            <tbody>
                                <tr>
                                    <td align="right"><a class="btn" id="lnkSubmitButton" runat="server" onserverclick="lnkSubmitButton_Click">Submit</a></td>
                                    <td align="right"><a class="btn" id="lnkCancelButton" runat="server" onserverclick="lnkCancelButton_Click">Cancel</a></td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <div class="result" id="divResult" runat="server" visible="false">
        <div>
            <h3>Summary of the Telephone Records Import</h3>
        </div>
        <div id="divRecordsImported" runat="server" visible="false">
            <h3 class="import-header">Records imported (<asp:Literal ID="litImportSuccessCount" runat="server"></asp:Literal>)</h3>
            <asp:Repeater ID="repTelephoneTransactionstoImport" runat="server" OnItemDataBound="repTelephoneTransactionstoImport_ItemDataBound">
                <HeaderTemplate>
                    <table cellpadding="0" cellspacing="0" class="grid">
                        <tr>
                            <th>Foundation Account</th>
                            <th>Billing Account</th>
                            <th>Wireless Number</th>
                            <th>UserName</th>
                            <th>Market Cycle End Date</th>
                            <th>Total Usage</th>
                            <th>Total Events</th>
                            <th>Total MOU Usage</th>
                            <th>Total Current Charges</th>
                            <th>Status</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Literal ID="litFoundationAccount" runat="server"></asp:Literal></td>
                        <td>
                            <asp:Literal ID="litBillingAccount" runat="server"></asp:Literal></td>
                        <td>
                            <asp:Literal ID="litWirelessNumber" runat="server"></asp:Literal></td>
                        <td>
                            <asp:Literal ID="litUserName" runat="server"></asp:Literal></td>
                        <td>
                            <asp:Literal ID="litTransactionDate" runat="server"></asp:Literal></td>
                        <td>
                            <asp:Literal ID="litTotalUsage" runat="server"></asp:Literal></td>
                        <td>
                            <asp:Literal ID="litTotalEvents" runat="server"></asp:Literal></td>
                        <td>
                            <asp:Literal ID="litTotalMouUsage" runat="server"></asp:Literal></td>
                        <td>
                            <asp:Literal ID="litTotalCurrentCharges" runat="server"></asp:Literal></td>
                        <td>
                            <asp:Literal ID="litImportStatus" runat="server"></asp:Literal></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <div id="divRecordsNotImported" runat="server" visible="false">
            <h3 class="import-header">Records not imported (<asp:Literal ID="litImportFaliureCount" runat="server"></asp:Literal>)</h3>
            <table cellpadding="0" cellspacing="0" class="grid">
                <tr>
                    <th>Foundation Account</th>
                    <th>Billing Account</th>
                    <th>Wireless Number</th>
                    <th>UserName</th>
                    <th>Market Cycle End Date</th>
                    <th>Total Usage</th>
                    <th>Total Events</th>
                    <th>Total MOU Usage</th>
                    <th>Total Current Charges</th>
                    <th>Status</th>
                </tr>
                <asp:Repeater ID="repTelephoneTransactionstoImportInvalid" runat="server" OnItemDataBound="repTelephoneTransactionstoImport_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Literal ID="litFoundationAccount" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litBillingAccount" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litWirelessNumber" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litUserName" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litTransactionDate" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litTotalUsage" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litTotalEvents" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litTotalMouUsage" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litTotalCurrentCharges" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litImportStatus" runat="server"></asp:Literal></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Repeater ID="repNonImportedItemList" runat="server" OnItemDataBound="repNonImportedItemList_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Literal ID="litFoundationAccount" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litBillingAccount" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litWirelessNumber" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litUserName" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litTransactionDate" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litTotalUsage" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litTotalEvents" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litTotalMouUsage" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litTotalCurrentCharges" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litImportStatus" runat="server"></asp:Literal></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>        
        <div class="back-button">
            <a class="btn" id="lnkBack" runat="server" onserverclick="lnkBackButton_Click">Back to Telephone Charges</a>
        </div>
    </div>
</div>
