<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdhocReportsCtrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Reports.AdhocReportsCtrl" %>

<h1 class="page-header">Generate Adhoc Report</h1>
<div>
    <asp:HiddenField ID="hfFiscalYearId" runat="server" />
</div>
<h3 class="popup-heading">Selected Fiscal Year:<asp:Literal ID="litSelectedFiscalYear" runat="server"></asp:Literal></h3>
<div class="error-msg">
    <asp:Literal ID="litErrorMessage" runat="server"></asp:Literal>
</div>
<%--<asp:UpdatePanel ID="upSearchResults" runat="server" UpdateMode="Conditional">
    <ContentTemplate>--%>

<table id="tblAdhocReport" runat="server" cellspacing="0" cellpadding="0" class="form">
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
        <th class="vtop">Expenditure Categories:</th>
        <td>
            <div class="select-all">

                <input type="checkbox" id="chkAllExpCategories" onclick="SelectAllCheckboxes1(this, 'select-all-ExpCategories', 'labExpCategoryStatus');" />
                <label for="chkAllExpCategories" id="labExpCategoryStatus">Select All</label>
                <%-- <asp:CheckBox ID="chkAllExpenditureCategories" runat="server" AutoPostBack="true" OnCheckedChanged="chkAllExpenditureCategories_CheckedChanged" />--%>
            </div>
            <div id="select-all-ExpCategories">
                <asp:CheckBoxList CssClass="checkbox-grid" ID="chkExpenditureCategoriesList" runat="server" RepeatColumns="3">
                </asp:CheckBoxList>
                
                <asp:CustomValidator CssClass="error" ID="cvalExpenditureCategory" runat="server" OnServerValidate="cvalExpenditureCategory_ServerValidate" Display="Dynamic" ValidationGroup="ValGroup1"></asp:CustomValidator>
            </div>
        </td>

    </tr>
    <tr>
        <th class="vtop">Reports to be generated:</th>
        <td>
            <div class="select-all">

                <input type="checkbox" id="chkAllReportTypes" onclick="SelectAllCheckboxes1(this, 'select-all-Reporttypes', 'labReportTypesStatus');" />
                <label for="chkAllReportTypes" id="labReportTypesStatus">Select All</label>
                <%-- <asp:CheckBox ID="chkAllExpenditureCategories" runat="server" AutoPostBack="true" OnCheckedChanged="chkAllExpenditureCategories_CheckedChanged" />--%>
            </div>
            <div id="select-all-Reporttypes">
                  <input type="checkbox"    id="chkYearWise" runat="server" />:Detailed Transaction Report<br />
                  <input type="checkbox"   id="chkMonthWise" runat="server" />:Monthwise Transaction Report<br />
                  <input type="checkbox"   id="chkCustomReport" runat="server" />:Summary Report<br />
             <asp:CustomValidator CssClass="error" ID="cvalReportType" runat="server" OnServerValidate="cvalReportType_ServerValidate" Display="Dynamic" ValidationGroup="ValGroup1"></asp:CustomValidator>
            </div>
        </td>

    </tr>
    <tr>
        <th><strong>Report Date Range:</strong></th>
        <td>
            <%--<div class="select-all-Offices">

              <input type="radio" class="xhide" ID="radToAsOfDate" runat="server" GroupName="Date"  />: From Fiscal Year Start
              <input type="radio" class="xshow" ID="radBetweenTwoDates" runat="server" GroupName="Date" />:Between Two Different Dates
            </div>--%>

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
<%--    </ContentTemplate>
</asp:UpdatePanel>--%>
<script type="text/javascript">
    $(document).ready(function () {
        $('#select-all-Offices').find("input[type='checkbox']").click(function () {
            $('#labOfficeStatus').html('Select All');
            $('#chkAllOffice').attr("checked", false);
        });

        $('#select-all-ExpCategories').find("input[type='checkbox']").click(function () {
            $('#labExpCategoryStatus').html('Select All');
            $('#chkAllExpCategories').attr("checked", false);
        });

        $('#select-all-Reporttypes').find("input[type='checkbox']").click(function () {
            $('#labReportTypesStatus').html('Select All');
            $('#chkReportList').attr("checked", false);
        });
        
      
         $(".trStartDate").hide();

        $('input[type=radio]').change(function () {
            var isChecked = $(this).prop('checked');
            var isShow = $(this).hasClass('xshow');
            $(".trStartDate").toggle(isChecked && isShow);
        });




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

