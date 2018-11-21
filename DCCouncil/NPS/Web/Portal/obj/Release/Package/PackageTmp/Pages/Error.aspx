<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Guest.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.Pages.Error" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="login-ctrl">

        <div style="font-size: 12px;">
            <h3>An error occured on the site.</h3>
            <p><b>Error ID = <%= Request["id"] %></b></p>
            <p>
                Please contact administrator and refer the Error ID.
            </p>
            <p>Click <a href="<%= PRIFACT.DCCouncil.NPS.Core.NPSCommon.NPSUrls.Dashboard %>">here</a> to go to Dashboard.</p>
        </div>
    </div>
</asp:Content>
