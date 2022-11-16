<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportV2Ctrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Members.PurchaseOrder.ImportCtrlV2" %>
<asp:HiddenField ID="hfGuid" runat="server" />
<asp:Wizard CssClass="wizard" ID="Wizard1" runat="server" DisplayCancelButton="true" DisplaySideBar="false" OnNextButtonClick="Wizard1_NextButtonClick" OnCancelButtonClick="Wizard1_CancelButtonClick" OnPreviousButtonClick="Wizard1_PreviousButtonClick">
    <FinishNavigationTemplate>
        <div class="wiz-button">
            <asp:Button ID="StepPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious" Text="Previous" Visible="False" />
            <asp:Button ID="Finish" runat="server" CommandName="MoveComplete" Text="Finish" OnClick="Finish_Click" />
        </div>
    </FinishNavigationTemplate>
    <WizardSteps>
        <asp:WizardStep ID="WizardStep1" runat="server" Title="Select File">

            <div class="error">
                <asp:CustomValidator ID="cvalSelectFile" runat="server" OnServerValidate="cvalSelectFile_ServerValidate" ValidationGroup="ValGroupFile"></asp:CustomValidator><br />
                <asp:CustomValidator ID="cvalFileType" runat="server" OnServerValidate="cvalFileType_ServerValidate" ValidationGroup="ValGroupFile"></asp:CustomValidator><br />
                <asp:CustomValidator ID="cvalFileSize" runat="server" OnServerValidate="cvalFileSize_ServerValidate" ValidationGroup="ValGroupFile"></asp:CustomValidator>
                <asp:CustomValidator ID="cvalIsPOImportFile" runat="server" OnServerValidate="cvalIsPOImportFile_ServerValidate" ValidationGroup="ValGroupFile"></asp:CustomValidator><br />
            </div>

            <div class="wizard-item">
                <h3>Upload the Purchase Order report file to import:</h3>
                <asp:FileUpload ID="fuImportFile" runat="server" />
            </div>
        </asp:WizardStep>
        <asp:WizardStep ID="WizardStep2" runat="server" Title="Step 2">
            <div class="wizard-step2-item">
                <h3>Preview</h3>                
                <h5>You are about to import
                <asp:Literal ID="litSelectedCount" runat="server"></asp:Literal>
                    Purchase orders into the system. Click Next to continue..</h5>
                <asp:Repeater ID="rptrPurchaseOrdersToImport" runat="server" OnItemDataBound="rptrPurchaseOrdersToImport_ItemDataBound">
                    <HeaderTemplate>
                        <table cellpadding="0" cellspacing="0" class="grid">
                            <tr>
                                <%--<th>
                                    <asp:CheckBox ID="chkSelectAll" runat="server" OnCheckedChanged="chkSelectAll_CheckedChanged" AutoPostBack="true" /></th>--%>
                                
                                <th>Office Name</th>
                                <th>Vendor Name</th>
                                <th>PO Number</th>
                                <th>PO Line Number</th>
                                <th>PO Description</th>
                                <th>Expended Amount</th>
                                <th>Accounting Date</th>
                                <th>PO Balance</th>                                
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <%--<td>
                                <asp:CheckBox ID="chkSelect" runat="server" /></td>--%>
                            <td>
                                <asp:HiddenField ID="hfPurchaseOrdersV2Id" runat="server" />
                                <asp:Literal ID="litOfficeName" runat="server"></asp:Literal>
                            </td>                            
                            <td>
                                <asp:Literal ID="litVendorName" runat="server"></asp:Literal>
                            </td>
                            <td>                                
                                <asp:Literal ID="litPONumber" runat="server"></asp:Literal>
                            </td> 
                            <td>                                
                                <asp:Literal ID="litPOLineNumber" runat="server"></asp:Literal>
                            </td> 
                            <td>                                
                                <asp:Literal ID="litPODescription" runat="server"></asp:Literal>
                            </td> 
                            <td>
                                <asp:Literal ID="litExpendedAmount" runat="server"></asp:Literal>
                            </td>                            
                            <td>
                                <asp:Literal ID="litAccountingDate" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litPOBalance" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>

            </div>
        </asp:WizardStep>
        <asp:WizardStep ID="WizardStep4" runat="server" Title="Step 4">
            <div class="wizard-item">
                <h3>Summary of the Purchase Order Import</h3>
                <asp:Repeater ID="rptrPurchaseOrderImportSummary" runat="server" OnItemDataBound="rptrPurchaseOrderImportSummary_ItemDataBound">
                    <HeaderTemplate>
                        <table cellpadding="0" cellspacing="0" class="grid">
                            <tr>
                                <th>Office Name</th>
                                <th>Vendor Name</th>
                                <th>PO Number</th>
                                <th>PO Line Number</th>
                                <th>PO Description</th>
                                <th>Expended Amount</th>
                                <th>Accounting Date</th>
                                <th>PO Balance</th>  
                                <%--<th></th>--%>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:HiddenField ID="hfPurchaseOrdersV2Id" runat="server" />
                                <asp:Literal ID="litOfficeName" runat="server"></asp:Literal>
                            </td>                            
                            <td>
                                <asp:Literal ID="litVendorName" runat="server"></asp:Literal>
                            </td>
                            <td>                                
                                <asp:Literal ID="litPONumber" runat="server"></asp:Literal>
                            </td> 
                            <td>                                
                                <asp:Literal ID="litPOLineNumber" runat="server"></asp:Literal>
                            </td> 
                            <td>                                
                                <asp:Literal ID="litPODescription" runat="server"></asp:Literal>
                            </td> 
                            <td>
                                <asp:Literal ID="litExpendedAmount" runat="server"></asp:Literal>
                            </td>                            
                            <td>
                                <asp:Literal ID="litAccountingDate" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litPOBalance" runat="server"></asp:Literal>
                            </td>
                            <%--<td>
                                <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                            </td>--%>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </asp:WizardStep>
    </WizardSteps>
</asp:Wizard>


