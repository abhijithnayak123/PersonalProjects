//US1641 CheckFranking Changes
function FrankCheck(baseURL, frankdata, transactionId) {
	DisableCheckFrankButtons();
	showSpinner();
	frankdata = JSON.parse(DecodeHTML(frankdata));

	$.ajax({
		data: "{}",
		dataType: "jsonp",
		type: "GET",
		url: baseURL + "/FrankCheck?frankparams=" + frankdata,
		contentType: "application/json; charset=UTF-8",
		processData: true,
		cache: false,
		success: function (data) {
			if (data.ErrorNo != undefined) {
			    if (!showExceptionPopupMsg(data.ErrorDetails)) {
					var btn = $(this);
					btn.attr('disabled', false);
					EnableCheckFrankButtons();
				}
			}
			else {
				currentCheckCount = currentCheckCount + 1;
				$('#divFrankPopup').dialog('destroy').remove();
				PrintStatus = true;
				checkCount();
				UpdateTransaction(transactionId);
			}
		},
		error: function (data, errmessage, errdetails) {
			if (data.ErrNo != undefined) {
				$("#loading").fadeOut();
				if (!showExceptionPopupMsg(data.ErrorDetails)) {
					var btn = $(this);
					btn.attr('disabled', false);
					EnableCheckFrankButtons();
				}
			}
			else {
			    if (!showExceptionPopupMsg('Please Contact System Administrator')) {
					var btn = $(this);
					btn.attr('disabled', false);
					EnableCheckFrankButtons();
				}
			}
			hideSpinner();
		},
		complete: function () {
			hideSpinner();
		},
		timeout: 10000
	});
}
function PrintEvent(TransactionID) {
	FrankCheck(NPSbaseURL, frankdata, TransactionID)
	var PrintStatusInterval = setInterval(function () {
		if (PrintStatus == true) {
			$('#divFrankPopup').dialog('destroy').remove();
			PrintStatus = false;
			clearInterval(PrintStatusInterval);
			showCheckFrankingPopup(chkslno);
		}
	}, 0);
}
function DisableCheckFrankButtons() {
	$('#btnPrint').addClass('DisableButtons');
	$('#btnPrint').attr('disabled', 'disabled');
	$('#btnCancel').addClass('DisableButtons');
	$('#btnCancel').attr('disabled', 'disabled');
}
function EnableCheckFrankButtons() {
	$('#btnPrint').removeAttr('disabled');
	$('#btnPrint').removeClass('DisableButtons');
	$('#btnCancel').removeAttr('disabled');
	$('#btnCancel').removeClass('DisableButtons');
}
function CancelCheckFrankingYes() {
	currentCheckCount = currentCheckCount + 1;
	$('#divcancelFrankPopup').dialog('destroy').remove();
	$('#divFrankPopup').dialog('destroy').remove();
	checkCount();
	showCheckFrankingPopup(chkslno);
}
function CancelCheckFrankingNo() {
	$('#divcancelFrankPopup').dialog('destroy').remove();
}

function UpdateTransaction(transactionId) {
	$.ajax({
		data: "{}",
		dataType: "json",
		type: "POST",
		data: { transactionId: transactionId },
		url: checkFrankingUpdateURL,
		success: function (data) {
		},
		error: function (data) {
		}
	});
}

function DecodeHTML(str) {
		return str.replace(/&lt;/g, '<').replace(/&gt;/g, '>');	
}

function checkCount() {
	if (currentCheckCount == checkCountNo) {
		$('.checkFrankDiv').hide();
		$('#ReceiptsList').show();

		//Author : Abhijith
		//Description : Added a call to post flush after doing shopping cart checkout process.
		//Receipts and Post Flush will call simultaneously and if any errors in Post Flush, pop up is 
		//displayed with error messages but receipt processing will be continued.	
		//Starts Here
		postFlush();
		//Ends Here

		PrintReceipts(receiptArray);
	}
}

function postFlush() {
	$.ajax({
		type: "GET",
		url: PostFlushURL,
		datatype: "json",
		cache: false,
		error: function (request) {
			AddErrorRow(currentReceipt, request.responseText, true, currentSubReceipt);
		},
		success: function (response) {
			if (response.success && response.ErrorMsg != "")
				showExceptionPopupMsg(response.ErrorMsg);
		},
		timeout: 10000
	});
}
