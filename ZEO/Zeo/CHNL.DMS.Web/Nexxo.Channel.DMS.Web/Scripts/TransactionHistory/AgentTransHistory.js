$(document).ready(function () {

    $("#pickstartdate").datepicker({
        inline: true,
        dateFormat: 'mm/dd/yy',
        showOtherMonths: true,
        dayNamesMin: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
        minDate: -180,
        maxDate: 0,
        changeMonth: true,
        onClose: function () {
            if (this.value != undefined && this.value != '') {
                resetEndDatePicker(this.value);
                $("#pickenddate").attr("disabled", false);
            }
            else {
                $("#pickenddate").attr("disabled", "disabled");
            }
        }
    });

    if ($('#pickenddate').val() != undefined || $('#pickenddate').val() != '')
    {
        resetEndDatePicker($('#pickstartdate').val());
    }

    function resetEndDatePicker(startDate) {
        var endDate = $('#pickenddate').val().toString();
        $('#pickenddate').datepicker('destroy');

        if (endDate != undefined && endDate != '')
        {
            var startPickerDate = new Date(startDate.toString());
            var endPickerDate = new Date(endDate);

            if (startPickerDate > endPickerDate){
                $('#pickenddate').val(startDate);
            }
        }

        $("#pickenddate").datepicker({
            inline: true,
            showOtherMonths: true,
            dayNamesMin: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
            minDate: startDate,
            maxDate: 0,
            changeMonth: true
        });
    }

    $("#divUserSessionId").css("display", "");
    $('#TransactionType').focus();
    if (isAgentTeller == 'True') {
        $('#Agent').attr('disabled', true);
    }
    if ($('#TransactionID').val() == "0") {
        $('#TransactionID').val('');
    }
    else if ($('#TransactionID').val() != '') {
        $('#TransactionType').prop("disabled", true);
        $('#Agent').attr("disabled", true);
    }


    $('#TransactionID').change(function () {
        if ($('#TransactionID').val() != '') {
            $('#TransactionType').prop("disabled", true);
            $('#Agent').attr("disabled", true);
        }
        else {
            $('#TransactionType').prop("disabled", false);
            $('#Agent').prop("disabled", false);
        }
    });

    $('#agentDailySummaryReport').click(function () {
        var $confirm = $("<div id='divagentDailySummaryReport'></div>");
        $confirm.empty();
        $confirm.dialog({
            autoOpen: false,
            title: 'Agent Summary Report',
            width: 700,
            draggable: false,
            resizable: false,
            modal: true,
            height: 700,
            open: function (event, ui) {
                var url = agentSummaryReport;
                $confirm.load(url);
            }
        });
        $confirm.dialog("open");

    });

    var rowsToColor = [];
    var flag = 0;
    var jqDataUrl = "GetAgentTransHistory/CustTransHistory";
    // Set up the jquery grid
    jQuery("#jqTable1").jqGrid({
        // Ajax related configurations
        url: jqDataUrl,
        datatype: "json",
        mtype: "POST",
        // Specify the column names
        colNames: ["Date", "Teller", "Session ID", "Trxn #", "Customer Name", "Trxn Status ", "Trxn Type", "Trxn Details", "Total Amount($)"],

        // Configure the columns
        colModel: [
            { name: "TransactionDate", index: "TransactionDate", width: 150, align: "center", fixed: true },
            { name: "Teller", index: "Teller", width: 150, align: "left", fixed: true },
            { name: "SessionId", index: "SessionId", width: 115, align: "center", fixed: true },
            { name: "TransactionId", index: "TransactionId", width: 115, fixed: true, align: "center",
                cellattr: function (rowId, tv, rawObject, cm, rdata) {
                    return ' onclick="ShowAgentTransactionDetailsPopup(' + '\'' + rowId.toString() + '\'' + ',' + (new Date()).getTime() + '' + ',\'' + rawObject[6].toString() + '\'' + ',\'' + rawObject[5].toString()+ '\'' + ',\'' + rawObject[2].toString()  + '\'' + ')" style="text-decoration:underline;color:#0000ff;cursor:pointer" ';
                }
            },
            { name: "CustomerName", index: "CustomerName", width: 150, align: "left", fixed: true },
		    { name: "TransactionStatus", index: "TransactionStatus", align: "left", fixed: true },
            { name: "TransactionType", index: "TransactionType", align: "left", fixed: true },
            { name: "TransactionDetail", index: "TransactionDetails", width: 180, fixed: true, cellattr: function (rowId, tv, rawObject, cm, rdata) { return 'style="white-space: normal;text-align:left !important;"' } },
			{ name: "TotalAmount", index: "TotalAmount", align: "right", fixed: true }
        ],

        // Grid total width and height
        cmTemplate: { sortable: false },
        width: 940,
        height: 220,

        // Paging
        toppager: false,
        pager: jQuery("#jqTablePager"),
        rowNum: 8,
        // rowList: [8, 10, 20],
        viewrecords: true, // Specify if "total number of records" is displayed

        // Default sorting
        // sortname: "FName",
        // sortorder: "asc",

        // Grid caption
        caption: "",
        loadComplete: function (data) {
        	if (data.success == false) {
        		showExceptionPopupMsg(data.data);
        	}
            $("tr.jqgrow:even").css("background", "#ebf0ee");
            $("#trancnt").text("Transaction Count = " + $('#jqTable1').getGridParam('records'));
        }
    }).navGrid("#jqTablePager",
            { refresh: false, search: false, add: false, edit: false, del: false },
                {}, // settings for edit
                {}, // settings for add
                {}, // settings for delete
                {sopt: ["cn"]} // Search options. Some options can be set on column level
         );
});

function LinkFormatter(cellvalue, options, rowObject) {
    return '&action = ' + cellvalue;
}

function ShowAgentTransactionDetailsPopup(trxId, dt, type, status, custSessionId) {
    if (type == 'Check Processing') {
        type = 'check';
    }
    if (type == 'Cash Out' || type == 'Cash In') {
        type = 'cash';
    }
    var $confirm;

    //if (type == 'Prepaid-Load' || type == 'Prepaid-Withraw' || type == 'Prepaid-Activate' || type == 'Money Order' || type == 'Receive Money' || type == 'check' || type == 'Bill Pay') {
    if (type == 'SendMoney') {
        $confirm = $('<div class="divPopupLong" id="divPoupClose"></div>');
    } else {
        $confirm = $('<div class="divPopupSmall" id="divPoupClose"></div>');
    }
    $confirm.empty();
    $confirm.dialog({
        autoOpen: false,
        height: 'auto',
        title: "Transaction Details",
        width: 640,
        draggable: false,
        resizable: false,
        closeOnEscape: false,
        modal: true,
        position: 'top',
        overflow: 'hidden',
        zIndex: 400,
        open: function (event, ui) {
            var url = TransactionPopupURL + '?transactionId=' + trxId + '&dt=' + dt + '&transactionType=' + type + '&transactionStatus=' + status + '&isAgentSession=true&CustSessionId=' +custSessionId;
            $confirm.load(url, function (responseText, textStatus, XMLHttpRequest) {
            	var data = parseData(responseText);
            	if (data && data.success == false) {
            		RemovePopUp();
            		showExceptionPopupMsg(data.data);
            	}
            });
        }
    });
    $confirm.dialog("open");
    
}

function RemovePopUp() {
	$('#divPoupClose').dialog('destroy').remove();
}

function parseData(data) {
	try {
		var obj = JSON.parse(data);
		return obj;
	} catch (ex) {
		return null;
	}
}

function printdiv(printPage) {
    //var newStr = document.all.item(printPage).innerHTML;
    //var oldStr = document.body.innerHTML;
    
   // document.body.innerHTML = newStr;
    $('#divagentDailySummaryReport').print();

   // document.body.innerHTML = oldStr;
}

function closediv() {
   $('#divagentDailySummaryReport').dialog('destroy').remove();
}

function previewPDF() {
    var $confirm = $("<div id='divPreviewPDF'></div>");
    $confirm.empty();
    $confirm.dialog({
        autoOpen: false,
        title: 'Agent Summary Report',
        width: 900,
        draggable: false,
        resizable: false,
        modal: true,
        height: 700,
        open: function (event, ui) {
            _spinner();
            var url = agentSummaryPDFPreviewPopUp;
            $confirm.load(url, 
                function (responseText, textStatus, XMLHttpRequest) {
                    //Making ajax call to create PDF file and to show spinner in browser
                    $.ajax({
                        url: createPdf_Url,
                            data: { },
                            type: 'GET',
                            datatype: 'json',
                            success: function (result) {
                                //once pdf file generated displaying in PDF preview in Object tag
                                var object = "<object id=\"objectID\" data=" + agentSummaryPrintPDF_URL + " type=\"application/pdf\" width=\"100%\" height=\"100%\" standby=\"Loading....\" title=\"Loading..\">";
                                object += "</object>";
                                $("#pdfReviewdiv").html(object);
                                hideSpinner();
                            },
                            error: function () {
                                showExceptionPopupMsg(defaultErrorMessage);
                                hideSpinner();
                            }
                        });
            });
        }
    });
    $confirm.dialog("open");

    $('#divPreviewPDF').parent().attr("id", "custom_Title");
}