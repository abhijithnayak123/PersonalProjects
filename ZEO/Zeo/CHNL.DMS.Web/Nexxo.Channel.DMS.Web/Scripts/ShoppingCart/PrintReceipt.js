function add_api_call_to_queue(qname, baseUrl, type, currTime, splitData) {
	$(document).queue(qname, function () {
        var sendUrl = baseUrl + type + currTime + splitData;
		$.ajax({
			type: "GET",
			url: sendUrl,
			dataType: "jsonp",
			cache: false,
			success: function (data, textStatus, jqXHR) {
			    if (data.ErrorNo != undefined) {
			        hideSpinner();
			        showExceptionPopupMsg(data.ErrorDetails);
			        return;
			    }
				// activate the next ajax call when this one finishes
				$(document).dequeue(qname);
				if (type == "E")
					hideSpinner();
			},
			error: function (data, errmessage, errdetails) {
				hideSpinner();
				getMessage(EpsonException.Printing_Receipt_Error);
			},
			timeout: 30000
		});
	});
}

function jsonSplitPrint(baseUrl, receipts, str2) {
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

function PrintReceiptMultiple(baseURL, receiptArray) {
	jsonSplitPrint(baseURL + "PrintDocStream?printparams=", receiptArray, "");
}

function PrintReceipt(baseURL, printParams) {
	showSpinner();
	printCompleted = false;
	var receipts = new Array();
	receipts[0] = printParams;
	PrintReceiptMultiple(baseURL, receipts);
	return;
}

function PrintTrxAndSummaryReceipt(btnprint, transactionId, transactionType, isSummaryReceiptRequired) {
	RePrintReceipt(btnprint, transactionId, transactionType);
	if (isSummaryReceiptRequired)
		RePrintReceipt(btnprint, transactionId, transactionType, isSummaryReceiptRequired);
}

function RePrintReceipt(btnprint, transactionId, transactionType, isSummaryReceiptRequired) {
	btnprint.attr('disabled', true);
	$.ajax({
		url: TransactionHistoryReceiptDataURL,
		data: { transactionId: transactionId, dt: (new Date()).getTime(), transactiontype: transactionType, isSummaryReceiptRequired: isSummaryReceiptRequired, isReprint: 'true' },
		type: 'POST',
		datatype: 'json',
		success: function (jsonData) {
			if (jsonData.success) {
				var printParams = jsonData.data;
				for (var r = 0; r < printParams.length; r++) {
					PrintReceipt(NPSbaseURL, printParams[r].PrintData);
				}
			}
			else {
				showExceptionPopupMsg(jsonData.data);
			}
		},
		complete: function () {
			btnprint.attr('disabled', false);
		},
		error: function (err) {
			showExceptionPopupMsg(err);
		}
	});
}

function PrintReport(baseURL, printParams) {
	showSpinner();
	$.ajax({
		data: "{}",
		dataType: "jsonp",
		type: "GET",
		cache: false,
		url: baseURL + "PrintCashDrawerReport?printparams=" + JSON.stringify(printParams),
		contentType: "application/json; charset=UTF-8",
		processData: true,
		success: function (data) {
			hideSpinner();
			if (data.ErrorNo != undefined) {
				getMessage(EpsonException.Printing_Receipt_Error);
				return;
		}
		},
		error: function (data, errmessage, errdetails) {
			EnableButtons();
			hideSpinner();
			if (data.ErrorNo != undefined) {
				getMessage(EpsonException.Printing_Receipt_Error);
				return;
			}
			else {
				getMessage(EpsonException.Print_Service_Connectivity);
				return;
			}
		},
		timeout: 10000
	});
}

function CallActionMethods(url, customerSessionId, checkId) {
	$.ajax({
		url: url,
		data: { customerSessionId: customerSessionId, checkId: checkId }, //parameters go here in object literal form 
		type: 'GET',
		datatype: 'json',
		success: function (data) {
		},
		error: function (data) { }
	});
}