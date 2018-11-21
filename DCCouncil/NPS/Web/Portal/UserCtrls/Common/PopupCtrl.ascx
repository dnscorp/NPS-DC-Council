<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PopupCtrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Common.PopupCtrl" %>
<div id="confirmationForm">
    <asp:HiddenField ID="hfHeadingText" runat="server" />
    <asp:HiddenField ID="hfMessage" runat="server" />
    <asp:HiddenField ID="hfIdToProcess" runat="server" />
    <asp:HiddenField ID="hfMode" runat="server" />
    <asp:HiddenField ID="hfOkayButtonText" runat="server" />
    <asp:HiddenField ID="hfCancelButtonText" runat="server" />
    <asp:HiddenField ID="hfShowOkayButton" runat="server" />
    <div id="popupConfirmation" class="popup">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <div class="popup-top">
                        <div class="popup-top-right">
                            <div class="popup-top-mid">
                            </div>
                        </div>
                    </div>
                    <div class="popup-mid">
                        <div class="popup-mid-container">
                            <div class="popup-head">
                                <span class="heading">
                                    <asp:Literal ID="litHeading" runat="server"></asp:Literal></span> <a class="popup-close-btn"
                                        onclick="CloseConfirmationBox();"></a>
                            </div>
                            <div class="popup-content-details">
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td>
                                            <asp:Literal ID="litConfirmationMessage" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr>

                                        <td class="button-group">
                                            <a class="button" href="javascript:;" runat="server" id="lnkOkay" onserverclick="OnLnkOkayClick">
                                                <asp:Literal ID="litOkayButtonText" runat="server"></asp:Literal></a> <a href="javascript:;" id="lnkCancel" class="button"
                                                    runat="server" onserverclick="lnkCancel_ServerClick">
                                                    <asp:Literal ID="litCancelButtonText" runat="server"></asp:Literal></a>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                        </div>
                    </div>
                    <div class="popup-bottom">
                        <div class="popup-bottom-right">
                            <div class="popup-bottom-mid">
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</div>
