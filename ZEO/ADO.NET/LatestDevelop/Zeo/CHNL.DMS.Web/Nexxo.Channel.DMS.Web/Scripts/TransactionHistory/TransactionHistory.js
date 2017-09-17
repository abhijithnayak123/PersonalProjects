
	// to keep the spinner running till it prints both trasaction and summary receipts
var canHideSpinner = true;

function LoadTransactionHistoryGrid(jqDataUrl) {
    $("#layout_right_pane").css({ "display": "none" });

    var rowsToColor = [];
    var flag = 0;
    // Set up the jquery grid
    jQuery("#jqTable1").jqGrid({
        // Ajax related configurations
        url: jqDataUrl,
        datatype: "json",
        mtype: "POST",
        // Specify the column names
        colNames: ["Date / Time", "Teller", "Session ID", "Trxn #", "Location", "Trxn Type", "TrxnStatus", "Trxn Details", "Total Amount($)"],

        // Configure the columns
        colModel: [
            { name: "TransactionDate", index: "TransactionDate", width: 210, align: "left" },
            { name: "Teller", index: "Teller", width: 180, align: "left" },
            { name: "SessionId", index: "SessionId", width: 130, align: "center" },
            { name: "TransactionId", index: "TransactionId", width: 130, align: "center",
                cellattr: function (rowId, tv, rawObject, cm, rdata) {
                	return ' onclick="ShowTransactionDetailsPopup(' + '\'' + rowId.toString() + '\'' + ',' + (new Date()).getTime() + '' + ',\'' + rawObject[5].toString() + '\'' + ',\'' + rawObject[6].toString() + '\'' + ',\'' + rawObject[2].toString() + '\'' + ')" style="text-decoration:underline;color:#0000ff;cursor:pointer" ';
                }
            },
            { name: "Location", index: "Location", width: 150, align: "left" },
            { name: "TransactionType", index: "TransactionType", width: 180, align: "left" },
			{ name: "TransactionStatus", index: "TransactionStatus", align: "left", hidden: true },
            { name: "TransactionDetail", index: "TransactionDetails", width: 200, cellattr: function (rowId, tv, rawObject, cm, rdata) { return 'style="white-space: normal;text-align:left !important;"' } },
            { name: "TotalAmount", index: "TotalAmount", width: 170, align: "right" }
            ],

        // Grid total width and height
        cmTemplate: { sortable: false },
        width: 940,
        height: 320,

        // Paging
        toppager: false,
        pager: jQuery("#jqTablePager"),
        rowNum: 8,
        // rowList: [8, 10, 20],
        viewrecords: true, // Specify if "total number of records" is displayed

        // Grid caption
        caption: "",
        loadComplete: function (data) {
        	if (data.success == false) {
        		showExceptionPopupMsg(data.data);
        	}
            $("tr.jqgrow:even").css("background", "#ebf0ee");
        }
    }).navGrid("#jqTablePager",
            { refresh: false, search: false, add: false, edit: false, del: false },
                {}, // settings for edit
                {}, // settings for add
                {}, // settings for delete
                {sopt: ["cn"]} // Search options. Some options can be set on column level
         );
}
$("#JQgridColumnTextWrap").css({"text-align":"left !important"});
function ShowTransactionDetailsPopup(trxId, dt, type, status, custSessionId) {
	if (type == 'Check Processing') {
    	type = 'check';
    }
    if (type == 'Cash Out' || type == 'Cash In') {
        type = 'cash';
    }
    var $confirm;

    if (type == 'SendMoney') {
        custSessionId = '';//3170 custSessionId is not require for SendMoney transactions
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

            var url = TransactionPopupURL + '?transactionId=' + trxId + '&dt=' + dt + '&transactionType=' + type + '&transactionStatus=' + status + '&isAgentSession=false&CustSessionId=' + custSessionId;//Added custSessionId to get transaction details in AgentTransaction History AL-2012//AL-3170 'isAgentSession' should be false to get Modify & Refund buttons visible in SendMoney transaction details.
            $confirm.load(url,
				function (responseText, textStatus, XMLHttpRequest) {
					var data = parseData(responseText);
					if (data && data.success == false) {
						ClosePopup();
						showExceptionPopupMsg(data.data);
					}
				});
        }
    });

    $confirm.dialog("open");
}

function ClosePopup() {
    $('#divPoupClose').dialog('destroy').remove();
    $('#divPrepaid').dialog('destroy').remove();
}
