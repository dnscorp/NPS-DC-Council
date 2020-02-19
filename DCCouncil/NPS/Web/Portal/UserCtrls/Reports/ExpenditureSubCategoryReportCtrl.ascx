<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExpenditureSubCategoryReportCtrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Reports.ExpenditureSubCategoryReportCtrl" %>
<h1 class="page-header">Generate Expenditure Sub-Category Report</h1>
<div>
    <asp:HiddenField ID="hfFiscalYearId" runat="server" />
</div>
<h3 class="popup-heading">Selected Fiscal Year:<asp:Literal ID="litSelectedFiscalYear" runat="server"></asp:Literal></h3>
<div class="error-msg">
    <asp:Literal ID="litErrorMessage" runat="server"></asp:Literal>
</div>

<table id="tblExpenditureSubCategoryReport" runat="server" cellspacing="0" cellpadding="0" class="form">
    <tr>
        <th class="vtop">Offices:</th>
        <td>
            <div class="select-all">

                <input type="checkbox" id="chkAllOffice" onclick="SelectAllCheckboxes1(this, 'select-all-Offices', 'labOfficeStatus');" />
                <label for="chkAllOffice" id="labOfficeStatus">Select All</label>
            </div>
            <div id="select-all-Offices">
                <asp:CheckBoxList CssClass="checkbox-grid" ID="chkOfficeList" runat="server" RepeatColumns="3"></asp:CheckBoxList>

                <asp:CustomValidator CssClass="error" ID="cvalOffices" runat="server" OnServerValidate="cvalOffices_ServerValidate" Display="Dynamic" ValidationGroup="ValGroup1"></asp:CustomValidator>
            </div>
        </td>

    </tr>
    <tr>
        <th class="vtop">Expenditure Sub-Categories:</th>
        <td>
            <div class="select-all">

                <input type="checkbox" id="chkAllExpSubCategories" onclick="SelectAllCheckboxes1(this, 'select-all-ExpSubCategories', 'labExpSubCategoryStatus');" />
                <label for="chkAllExpSubCategories" id="labExpSubCategoryStatus">Select All</label>
            </div>
            <div id="select-all-ExpSubCategories">
                <asp:CheckBoxList CssClass="checkbox-grid" ID="chkExpenditureSubCategoriesList" runat="server" RepeatColumns="3">
                </asp:CheckBoxList>
                
                <asp:CustomValidator CssClass="error" ID="cvalExpenditureSubCategory" runat="server" OnServerValidate="cvalExpenditureSubCategory_ServerValidate" Display="Dynamic" ValidationGroup="ValGroup1"></asp:CustomValidator>
            </div>
        </td>

    </tr>
    <tr>
        <th><strong>Report Date Range:</strong></th>
        <td>
            <asp:RadioButton ID="radToAsOfDate" runat="server" GroupName="Date" Text="YTD" AutoPostBack="true" OnCheckedChanged="radToAsOfDate_CheckedChanged" Checked ="true"/>
            <asp:RadioButton ID="radBetweenTwoDates" runat="server" GroupName="Date" Text="Custom"  AutoPostBack="true" OnCheckedChanged="radToAsOfDate_CheckedChanged" />
        </td>

    </tr>
    <tr id="trStartDate" runat="server" visible="false">
        <th><strong>Start Date:</strong></th>
        <td>
            <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
            <asp:CalendarExtender
                ID="CalendarExtender2"
                TargetControlID="txtStartDate"
                runat="server" />
            <asp:CustomValidator CssClass="error" ID="cvalStartDate" runat="server" OnServerValidate="cvalStartDate_ServerValidate" Display="Dynamic" ValidationGroup="ValGroup1"></asp:CustomValidator>
        </td>

    </tr>
    <tr>
        <th><strong>End Date:</strong></th>
        <td>
            <asp:TextBox ID="txtAsOfDate" runat="server"></asp:TextBox>
            <asp:CalendarExtender
                ID="CalendarExtender1"
                TargetControlID="txtAsOfDate"
                runat="server" />
            <asp:CustomValidator CssClass="error" ID="cvalAsOfDate" runat="server" OnServerValidate="cvalAsOfDate_ServerValidate" Display="Dynamic" ValidationGroup="ValGroup1"></asp:CustomValidator>
        </td>

    </tr>
    <tr class="form-button">
        <td></td>
        <td>
            <a href="javascript:;" class="btn btn-primary" runat="server" id="lnkSubmit" onserverclick="lnkSubmit_ServerClick">Generate Report</a>
            <a href="javascript:;" class="btn" id="lnkCancel" runat="server">Cancel</a></td>
    </tr>
</table>

<script type="text/javascript">
    $(document).ready(function () {
        $('#select-all-Offices').find("input[type='checkbox']").click(function () {
            $('#labOfficeStatus').html('Select All');
            $('#chkAllOffice').attr("checked", false);
        });

        $('#select-all-ExpSubCategories').find("input[type='checkbox']").click(function () {
            $('#labExpSubCategoryStatus').html('Select All');
            $('#chkAllExpSubCategories').attr("checked", false);
        });

        $(".trStartDate").hide();

        $('input[type=radio]').change(function () {
            var isChecked = $(this).prop('checked');
            var isShow = $(this).hasClass('xshow');
            $(".trStartDate").toggle(isChecked && isShow);
        });

        setTimeout(function () {
            $('.error').fadeOut('fast');
        }, 5000); // <-- time in milliseconds
    });

    function SelectAllCheckboxes1(CheckBox, div, labOfficeStatus) {
        if ($('#' + labOfficeStatus).html() == 'Select All') {
            $('#' + div).find("input:checkbox").each(function () {
                this.checked = true;
            });
            $('#' + labOfficeStatus).text('Unselect All');
        }
        else {
            $('#' + div).find("input:checkbox").each(function () {
                this.checked = false;
            });
            $('#' + labOfficeStatus).text("Select All");
        }
    }

    

</script>