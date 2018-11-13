var isSuccess = false;

function PrintPDSReceipt() {
        $.ajax({
        url: DoddfrankDisclosureReceiptDataURL,
        data: { transactionId: transactionId, dt: (new Date()).getTime() },
        type: 'POST',
        datatype: 'json',
        success: function (jsonData) {

        	if (jsonData.success) {
        		isSuccess = jsonData.success;

                var printParams = jsonData.data;
                for (var receiptIndex = 0; receiptIndex < printParams.length; receiptIndex++) {
                    for (var copyIndex = 0; copyIndex < printParams[receiptIndex].NumberOfCopies; copyIndex++) {
                        if (printParams[receiptIndex].PrintData != "")
                        	PrintDoddFrankReceipt(NPSbaseURL, printParams[receiptIndex].PrintData);
                        else {
                            hideSpinner();
                            showDoddFrankPopupMsg("MGiAlloy" + "|" + "0.0" + "|" + "Receipt template missing" + "|" + $("#DoddFrankPrintError").val());
                        }
                    }
                }
            } else {
				hideSpinner();
            	showDoddFrankPopupMsg("MGiAlloy" + "|" + "0.0" + "|" + "Receipt template missing" + "|" + $("#DoddFrankPrintError").val());
            	
            }
        },
        complete: function () {
        	if (isSuccess && ($("#divRetry").is(":visible"))) //If the Dodd Frank receipt not found then don't destroy the retry popup. 
        		$('#divRetry').dialog('destroy').remove();
        },
        error: function (err) {
			hideSpinner();
            showExceptionPopupMsg(err);

        }
    });
}

function PrintDoddFrankReceipt(baseURL, printParams) {
    showSpinner();
    printCompleted = false;
    var receipts = new Array();
    receipts[0] = printParams;
    PrintDoddFrankReceiptMultiple(baseURL, receipts);
    return;
}

function PDSPrintWarningPopup() {
	showCartAbandonmentConfirm = true;

	$("#dlgCancelMT").dialog('destroy').remove();
	var $confirmation = $("<div id='dlgCancelMT'></div>");
    $confirmation.empty();
    $confirmation.dialog({
        autoOpen: false,
        title: "MGiAlloy",
        width: 400,
        draggable: false,
        modal: true,
        minHeight: 150,
        resizable: false,
        closeOnEscape: false,
        open: function (event, ui) {
        	var url = cancelMTTransactionURL + '?id=' + transactionId + '&screenName=SendMoneyConfirm';
        	$confirmation.load(url);
        }
    });
    $confirmation.dialog("open");
}


function add_api_call_to_queue_doddFrank(qname, baseUrl, type, currTime, splitData) {
    $(document).queue(qname, function () {
        var sendUrl = baseUrl + type + currTime + splitData;
        $.ajax({
            type: 'GET',
			cache:'false',
            url: sendUrl,
            dataType: 'jsonp',
            success: function (data, textStatus, jqXHR) {
                if (data.ErrorNo != undefined) {
                	hideSpinner();

                	if (!($("#divRetry").is(":visible")))
                		showDoddFrankPopupMsg("MGiAlloy" + "|" + "0.0" + "|" + data.ErrorDetails + "|" + $("#DoddFrankPrintError").val());

                    return;
                }
                // activate the next ajax call when this one finishes
                $(document).dequeue(qname);
                if (type == "E")
                    hideSpinner();
            },
            error: function (data, errmessage, errdetails) {
            	hideSpinner();

				//'E' - means printing of receipt is came to an end. So if printing of receipt is done do not show the pop up.
            	if (type != "E")
            	{
            		if (!($("#divRetry").is(":visible"))) //MGI & RedStone.
            			showDoddFrankPopupMsg("MGiAlloy" + "|" + "0.0" + "|" + "NPS Service is not running" + "|" + $("#DoddFrankPrintError").val());
            	}
            },
			timeout:'10000'
        });
    });
}

//Author : Abhijith
//Bug : AL-2014
//Description : Adding a exception pop up if Dodd frank receipt not printed.
//Starts Here
function showDoddFrankPopupMsg(exceptionmsg) {
	showCartAbandonmentConfirm = true;

	var $confirmation = $("<div id='divRetry'></div>");
	$confirmation.empty();

	$confirmation.dialog({
		autoOpen: false,
		title: "SYSTEM MESSAGE",
		width: 505,
		draggable: false,
		modal: true,
		resizable: false,
		closeOnEscape: false,
		minHeight: 225,
		scroll: false,
		cache: false,
		open: function (event, ui) {
			$confirmation.load(DoddFrankExceptionMsgPopupURL, { dt: (new Date()).getTime(), msg: exceptionmsg }, function () {
				$('#btnOk').focus();
			});

		}
	});
	$confirmation.dialog("open");
	return false;
}
//Ends Here

function jsonSplitPrintDoddFrankPrint(baseUrl, receipts, str2) {
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
            add_api_call_to_queue_doddFrank(currTime, baseUrl, "A", currTime, splitData);
        }
    }
    add_api_call_to_queue_doddFrank(currTime, baseUrl, "E", currTime, '');
    $(document).dequeue(currTime);
}

function PrintDoddFrankReceiptMultiple(baseURL, receiptArray) {
	jsonSplitPrintDoddFrankPrint(baseURL + "PrintDocStream?printparams=", receiptArray, "");
}


