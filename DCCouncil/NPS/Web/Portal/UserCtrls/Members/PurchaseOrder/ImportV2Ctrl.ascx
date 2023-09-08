<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportV2Ctrl.ascx.cs" Inherits="PRIFACT.DCCouncil.NPS.Web.Portal.UserCtrls.Members.PurchaseOrder.ImportV2Ctrl" %>
<asp:HiddenField ID="hfGuid" runat="server" />
<asp:HiddenField runat="server" ID="CurrentStepNumber" ClientIDMode="Static" />
<style>
    .table-container {
            width: 100%; /* Full-screen width */
            overflow-x: auto; /* Enable horizontal scrolling */            
            margin-bottom: 20px;
        }

    .po-aging-container, .po-closeout-container {
            width: 100%; /* Set the desired width for the container */
            overflow-x: auto; /* Enable horizontal scrolling */
        }

    #tblAging, #tblCloseout {
        border-collapse: collapse;
        /*width: 100%;*/
    }

    #tblAging th, #tblAging td, #tblCloseout th, #tblCloseout td {
        border: 1px solid #ccc;
        padding: 8px;
        text-align: left;
        font-size: 14px; /* Adjust the font size as needed */
    }

    #tblAging th, #tblCloseout th {
        background-color: #f2f2f2; /* Background color for column headers */
    }

    /* Optional: Add hover effect to rows */
    #tblAging tbody tr:hover, #tblCloseout tbody tr:hover {
        background-color: #e0e0e0;
    }
    .stepButtonsAlignCenter{
        text-align:center;
        margin-bottom:20px;
        margin-top:20px;
    }
    .stepButtonsAlignLeft{
        text-align:left;
        margin-bottom:20px;
        margin-top:20px;
    }
    .alignLeft {
        float: left;
        /* You can add additional styles as needed */
    }
    .mb20{
        margin-bottom:20px;
    }
    .mt20{
        margin-top:20px;
    }

    #tblAging tr:nth-child(even), #tblCloseout tr:nth-child(even){
    background-color: #f9f9f9;
}
        
</style>
<asp:Wizard CssClass="wizard" ID="Wizard1" runat="server" DisplayCancelButton="true" DisplaySideBar="false" OnNextButtonClick="Wizard1_NextButtonClick" OnCancelButtonClick="Wizard1_CancelButtonClick" OnPreviousButtonClick="Wizard1_PreviousButtonClick" OnActiveStepChanged="Wizard1_ActiveStepChanged">
    <FinishNavigationTemplate>
        <div class="wiz-button stepButtonsAlignCenter">
            <asp:Button ID="StepPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious" Text="Previous" Visible="False" />
            <asp:Button ID="Finish" runat="server" CommandName="MoveComplete" Text="Finish" OnClick="Finish_Click" />
        </div>
    </FinishNavigationTemplate>
    <StartNavigationTemplate>
        <div class="stepButtonsAlignCenter">
            <asp:Button ID="StepNextButton" runat="server" ClientIDMode="Static" CommandName="MoveNext" Text="Next" />
            <asp:Button ID="StepCancelButton" runat="server" ClientIDMode="Static" Text="Cancel" OnClick="Wizard1_CancelButtonClick" />
        </div>
    </StartNavigationTemplate>
    <StepNavigationTemplate>
        <div id="nextButton" class="stepButtonsAlignCenter">
            <asp:Button ID="StepPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious" Text="Previous" />
            <asp:Button ID="StepNextButton" runat="server" ClientIDMode="Static" CommandName="MoveNext" Text="Next" />
        </div>        
    </StepNavigationTemplate>    
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
            <div class="table-container">
                <h2>Preview</h2>
                <h3>PO Aging Balance</h3>
                <table id="tblAging" border="1">
                    <thead>
                        <tr>
                            <asp:Literal ID="ltrlAgingHeader" runat="server"></asp:Literal>                        
                        </tr>
                    </thead>
                    <tbody id="tableAgingBody">
                        <!-- Your table data goes here -->
                    </tbody>
                </table>
            </div>
            
            <div class="table-container">
                <h3>PO Closeout Balance</h3>
                <table id="tblCloseout" border="1">
                    <thead>
                        <tr>
                            <asp:Literal ID="ltrlCloseoutHeader" runat="server"></asp:Literal>                        
                        </tr>
                    </thead>
                    <tbody id="tblCloseoutBody">
                        <!-- Your table data goes here -->
                    </tbody>
                </table>
                <asp:HiddenField ID="hdnAgingTotalColumns" Value="0" ClientIDMode="Static" runat="server" />
                <asp:HiddenField ID="hdnCloseoutTotalColumns" Value="0" ClientIDMode="Static" runat="server" />                
            </div>
        </asp:WizardStep>
        <asp:WizardStep ID="WizardStep3" runat="server" Title="Step 3">
            <div class="wizard-step2-item inner">
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
            <div class="wizard-item inner">
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
<div>
    <strong>Note:</strong> Per requirement, "Pay As You Go" Purchase Orders are ignored from the import file. They are identified using the Fund code in the PO file. If the fund code for a PO starts with "3030...", it will not be imported.
</div>
<script>
        var dict = {};
    var closeoutDict = {};
    var currentStep;

        $(document).ready(function () {

            // Initialize an array to store the selected columns
            var selectedColumns = [];

            // Access the data stored in dataArray
            if (typeof agingDataArray !== 'undefined') {
                for (var i = 1; i <= $("#hdnAgingTotalColumns").val(); i++) {
                    var columnId = "#hAgingcol" + i;
                    var dropdownId = "#colAging" + i + "Dropdown";
                    var columnSelect = $(dropdownId);
                    //console.log(dropdownId);
                    // Populate each dropdown with column names
                    columnSelect.append($("<option>").text("---Select---").val(""));
                    for (var j = 0; j < agingColumnNames.length; j++) {
                        var columnName = agingColumnNames[j];
                        //var columnNameWithoutSpace = agingColumnNames[j].replace(/ /g, "");
                        var option = $("<option>").text(columnName).val(columnName);
                        columnSelect.append(option);
                    }

                    //Code to set the selected items based on the column header
                    $(dropdownId + " option").each(function () {
                        if ($(this).val().trim().toLowerCase() === $(columnId).text().trim().toLowerCase()) {
                            $(this).attr('selected', 'selected');
                            var headerText = $(columnId).text();
                            var dropdownSelectedValue = $(this).text();
                            dict[headerText] = dropdownSelectedValue;
                        }
                    });

                    populateTable();

                    // Add event handler for dropdown change
                    columnSelect.on('change', function () {
                        var selectedColumn = $(this).val();

                        //console.log("Count:",$('select option:selected[value='+selectedColumn+']').length);

                        var headerColumnId = $(this).attr('id').replace("colAging", "").replace("Dropdown", "");
                        var headerColumn = "#hAgingcol" + headerColumnId;
                        var headerText = $(headerColumn).text();

                        // Check if the column is not already in selectedColumns before pushing it
                        if (selectedColumn && selectedColumns.indexOf(selectedColumn) === -1) {
                            selectedColumns.push(selectedColumn);
                            dict[headerText] = $(this).find("option:selected").text();
                        }

                        populateTable(selectedColumn); // Populate the table based on selected columns
                    });

                }
                //console.log(dict);
            }

            // Function to populate the table based on the selected column
            function populateTable() {
                var tableBody = $("#tableAgingBody");
                tableBody.empty(); // Clear existing table data

                // Loop through dataArray and create rows based on selected columns
                for (var i = 0; i < agingDataArray.length; i++) {
                    var row = $("<tr>");                    
                    for (var dtc = 1; dtc <= $("#hdnAgingTotalColumns").val(); dtc++) {
                        selectedColname = $("#colAging" + dtc + "Dropdown :selected").text()
                        //if (selectedColname === "Expenditure Invoice Amount")
                        //    selectedColname = "Expenditure(Invoice Amount)";
                        var cellValue = agingDataArray[i][selectedColname] || ""; // Use data from dataArray or blank if not available
                        var cell = $("<td>").text(cellValue);
                        row.append(cell);
                    }

                    tableBody.append(row);
                    highlightAgingEmptyColumns();
                }
            }

            if (typeof closeoutDataArray !== 'undefined') {
                for (var i = 1; i <= $("#hdnCloseoutTotalColumns").val(); i++) {
                    var columnId = "#hCloseoutcol" + i;
                    var dropdownId = "#colCloseout" + i + "Dropdown";
                    var columnSelect = $(dropdownId);

                    // Populate each dropdown with column names
                    columnSelect.append($("<option>").text("---Select---").val(""));
                    for (var j = 0; j < closeoutColumnNames.length; j++) {
                        var columnName = closeoutColumnNames[j];
                        //var columnNameWithoutSpace = agingColumnNames[j].replace(/ /g, "");
                        var option = $("<option>").text(columnName).val(columnName);
                        columnSelect.append(option);
                    }

                    //Code to set the selected items based on the column header
                    $(dropdownId + " option").each(function () {
                        if ($(this).val().trim().toLowerCase() === $(columnId).text().trim().toLowerCase()) {
                            $(this).attr('selected', 'selected');
                            var headerText = $(columnId).text();
                            var dropdownSelectedValue = $(this).text();
                            closeoutDict[headerText] = dropdownSelectedValue;
                        }
                    });

                    populateCloseoutTable();

                    // Add event handler for dropdown change
                    columnSelect.on('change', function () {
                        var selectedColumn = $(this).val();

                        //console.log("Count:",$('select option:selected[value='+selectedColumn+']').length);

                        var headerColumnId = $(this).attr('id').replace("colCloseout", "").replace("Dropdown", "");
                        var headerColumn = "#hCloseoutcol" + headerColumnId;
                        var headerText = $(headerColumn).text();

                        // Check if the column is not already in selectedColumns before pushing it
                        if (selectedColumn && selectedColumns.indexOf(selectedColumn) === -1) {
                            selectedColumns.push(selectedColumn);
                            closeoutDict[headerText] = $(this).find("option:selected").text();
                        }

                        populateCloseoutTable(selectedColumn); // Populate the table based on selected columns
                    });

                }
                //console.log(dict);
            }

            // Function to populate the table based on the selected column
            function populateCloseoutTable() {
                var tableBody = $("#tblCloseoutBody");
                tableBody.empty(); // Clear existing table data

                // Loop through dataArray and create rows based on selected columns
                for (var i = 0; i < closeoutDataArray.length; i++) {
                    var row = $("<tr>");

                    for (var dtc = 1; dtc <= $("#hdnCloseoutTotalColumns").val(); dtc++) {
                        selectedColname = $("#colCloseout" + dtc + "Dropdown :selected").text()
                        //if (selectedColname === "Expenditure Invoice Amount")
                        //    selectedColname = "Expenditure(Invoice Amount)";
                        var cellValue = closeoutDataArray[i][selectedColname] || ""; // Use data from dataArray or blank if not available
                        var cell = $("<td>").text(cellValue);
                        row.append(cell);
                    }

                    tableBody.append(row);
                    highlightCloseoutEmptyColumns();
                }
            }

            currentStep = parseInt($("#CurrentStepNumber").val());
            console.log('Current Step Number: ' + currentStep);
            
            if (currentStep === 2) {
                $("#nextButton").removeClass("stepButtonsAlignCenter").addClass("stepButtonsAlignLeft");
            }
            else if (currentStep === 3)
            {
                $("#StepNextButton").val('Import');
                //$("#nextButton").attr("text","Import");
                /*$("#nextButton").attr('prop', 'Import');*/
            }
            else {
                $("#nextButton").removeClass("stepButtonsAlignLeft").addClass("stepButtonsAlignCenter");
            }

            function highlightAgingEmptyColumns() {
                //Highlight columns with red if dropdown is empty
                var firstRowDropdowns = $("#tblAging thead tr:eq(0) .dropdown");
                
                // Loop through each dropdown in the first row
                firstRowDropdowns.each(function (index) {
                    // Check if the dropdown is empty
                    if ($(this).val() === "") {
                        // If empty, highlight the corresponding column in all rows
                        $("#tblAging tbody tr").each(function () {
                            $(this).find("td:eq(" + index + ")").css("background-color", "#feeceb");
                        });
                    }
                });
            }

            function highlightCloseoutEmptyColumns() {
                //Highlight columns with red if dropdown is empty
                var firstRowDropdowns = $("#tblCloseout thead tr:eq(0) .dropdown");

                // Loop through each dropdown in the first row
                firstRowDropdowns.each(function (index) {
                    // Check if the dropdown is empty
                    if ($(this).val() === "") {
                        // If empty, highlight the corresponding column in all rows
                        $("#tblCloseout tbody tr").each(function () {
                            $(this).find("td:eq(" + index + ")").css("background-color", "#feeceb");
                        });
                    }
                });
            }
        }

    );

    $('#StepNextButton').click(function () {
        //console.log("Click:",currentStep);
        if(currentStep===2)            
            return importExcelData();
    });
        

        function importExcelData() {

            //$('select option:selected[value=""]').parent();

            for (var i = 1; i <= $("#hdnAgingTotalColumns").val(); i++) {
                var dropdownId = "#colAging" + i + "Dropdown";
                var columnSelect = $(dropdownId);

                var headerColumn = "#hAgingcol" + i;
                var headerText = $(headerColumn).text();

                if (columnSelect.val() === "") {

                    alert("Please select '" + headerText + "'.")
                    columnSelect.focus();
                    return false;

                    //var msgHeader = $(dropdownId + " option[value=" + headerText + "]").text();

                    //if (msgHeader && msgHeader != "")
                    //    alert("Please select '" + msgHeader + "'.")
                    //else
                    //    alert("Please select '" + headerText + "'.")

                    //columnSelect.focus();
                    //return false;
                }
            }

            for (var i = 1; i <= $("#hdnCloseoutTotalColumns").val(); i++) {
                var dropdownId = "#colCloseout" + i + "Dropdown";
                var columnSelect = $(dropdownId);

                var headerColumn = "#hCloseoutcol" + i;
                var headerText = $(headerColumn).text();

                if (columnSelect.val() === "") {
                    alert("Please select '" + headerText + "'.")
                    columnSelect.focus();
                    return false;

                    //var msgHeader = $(dropdownId + " option[value=" + headerText + "]").text();

                    //if (msgHeader && msgHeader != "")
                    //    alert("Please select '" + msgHeader + "'.")
                    //else
                    //    alert("Please select '" + headerText + "'.")

                    //columnSelect.focus();
                    //return false;
                }
            }

            //console.log(dict);
            let jsonAgingData = JSON.stringify(dict);
            let jsonCloseoutData = JSON.stringify(closeoutDict);
            let data = { agingData: jsonAgingData, closeoutData: jsonCloseoutData };

            $.ajax({
                type: "POST",
                url: "ImportV2.aspx/OnSubmit",
                data: JSON.stringify(data),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                    return false;
                },
                success: function (result) {                    
                    return true;
                }
            });
        }

</script> 
