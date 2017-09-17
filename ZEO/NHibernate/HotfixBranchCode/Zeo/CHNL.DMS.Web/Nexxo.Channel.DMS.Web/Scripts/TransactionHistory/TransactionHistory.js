
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
        colNames: ["Date / Time", "Teller", "Session ID", "Trnx #", "Location", "Trnx Type", "TrnxStatus", "Trnx Details", "Total Amount($)"],

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
        loadComplete: function () {
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
            $confirm.load(url);
            showSpinner();
        }
    });

    $confirm.dialog("open");
}

function add_api_call_to_queue(qname, baseUrl, type, currTime, splitData) {
    $(document).queue(qname, function () {
        var sendUrl = baseUrl + type + currTime + splitData;
        $.ajax({
            type: "GET",
            url: sendUrl,
            dataType: "jsonp",
            cache: false,
            success: function (data, textStatus, jqXHR) {
                // activate the next ajax call when this one finishes
            	$(document).dequeue(qname);
				// spinner should hide after the last transaction receipt packet been sent
                if (type == "E" && canHideSpinner) {
                	hideSpinner();
                }
            },
            error: function () {
                hideSpinner();
            },
            timeout: 30000
        });
    });
}

function jsonSplitPrint(baseUrl, receipts) {
    var now = new Date();
    var splitSize = 1000;
    var imageData = "";
    var currTime = now.getFullYear() + '' + ("0" + (now.getMonth() + 1)).slice(-2) + '' + ("0" + (now.getDate())).slice(-2) + ("0" + now.getHours()).slice(-2) + ("0" + now.getMinutes()).slice(-2) + ("0" + now.getSeconds()).slice(-2) + ("00" + now.getMilliseconds()).slice(-3);
    for (var r = 0; r < receipts.length; r++) {
        imageData = receipts[r] + "\\";
        var imageDataLen = imageData.length;
        var splits = Math.ceil(imageDataLen / splitSize);
        for (var i = 0; i < splits; i++) {
            var endSplit = i * splitSize + splitSize;
            if (endSplit > imageDataLen)
                endSplit = imageDataLen;
            var splitData = imageData.substring(i * splitSize, endSplit);
            splitData = splitData.replace("#", " ");
            add_api_call_to_queue(currTime, baseUrl, "A", currTime, splitData);
        }
    }
    add_api_call_to_queue(currTime, baseUrl, "E", currTime, '');
    $(document).dequeue(currTime);
}

function PrintReceipt(baseURL, printParams) {
    var receipts = new Array();
    receipts[0] = printParams;
    jsonSplitPrint(baseURL + "PrintDocStream?printparams=", receipts);
    return;
}

function RePrintReceipt(btnprint, transactionId, transactionType, isSummaryReceiptRequired) {
    showSpinner();
    GetReceiptDataAndPrint(btnprint, transactionId, transactionType, false);
	// Adding some time delay using set timer as summary receipt is printing before actual transaction receipt
    if (isSummaryReceiptRequired) {
    	setTimeout(function () {
    		canHideSpinner = true;
    		showSpinner();
    		GetReceiptDataAndPrint(btnprint, transactionId, transactionType, isSummaryReceiptRequired);
    	}, 30000);
    }   
}

function PrintTrxAndSummaryReceipt(btnprint, transactionId, transactionType, isSummaryReceiptRequired) {
	RePrintReceipt(btnprint, transactionId, transactionType);
	if (isSummaryReceiptRequired)
		RePrintReceipt(btnprint, transactionId, transactionType, isSummaryReceiptRequired);
}

function GetReceiptDataAndPrint(btnprint, transactionId, transactionType, isSummaryReceiptRequired) {
    $.ajax({
        url: TransactionHistoryReceiptDataURL,
        data: { transactionId: transactionId, dt: (new Date()).getTime(), transactiontype: transactionType, isSummaryReceiptRequired: isSummaryReceiptRequired, isReprint: 'true' },
        type: 'POST',
        datatype: 'json',
        success: function (jsonData) {
            if (jsonData.success) {
                var printParams = jsonData.data;
                for (var receiptIndex = 0; receiptIndex < printParams.length; receiptIndex++) {
                    for (var copyIndex = 0; copyIndex < printParams[receiptIndex].NumberOfCopies; copyIndex++) {
                        if (printParams[receiptIndex].PrintData !="")
                            PrintReceipt(NPSbaseURL, printParams[receiptIndex].PrintData);
                        else {
                            hideSpinner();
                            showExceptionPopupMsg("Receipt template missing");
                        }
                    }
                }
            }
            else {
                hideSpinner();
                showExceptionPopupMsg(jsonData.data);
            }
        },
        complete: function () {
        },
        error: function (err) {
            hideSpinner();
            showExceptionPopupMsg(err);
        }
    });
}

function ClosePopup() {
    $('#divPoupClose').dialog('destroy').remove();
    $('#divPrepaid').dialog('destroy').remove();
}
