function PrintMoneyOrder(baseURL, checkPrintdata) {
	$('#btnPrint').addClass('DisableButtons');
	$('#btnPrint').attr('disabled', 'disabled');
	showSpinner();	
	var postUrl = baseURL + "PrintCheckStream?printparams=";
	var now = new Date();

	var splitSize = 1000;
	var currTime = now.getFullYear() + '' + ("0" + (now.getMonth() + 1)).slice(-2) + '' + ("0" + (now.getDate())).slice(-2) + ("0" + now.getHours()).slice(-2) + ("0" + now.getMinutes()).slice(-2) + ("0" + now.getSeconds()).slice(-2) + ("00" + now.getMilliseconds()).slice(-3);
	var imageDataLen = checkPrintdata.length;
	var splits = Math.ceil(imageDataLen / splitSize);
	for (var i = 0; i < splits; i++) {
		var endSplit = i * splitSize + splitSize;
		if (endSplit > imageDataLen)
			endSplit = imageDataLen;
		var splitData = checkPrintdata.substring(i * splitSize, endSplit);
		add_api_call_to_queue_moprint(currTime, postUrl, "A", currTime, splitData);
	}
	add_api_call_to_queue_moprint(currTime, postUrl, "E", currTime, '');
	$(document).dequeue(currTime);

}
function add_api_call_to_queue_moprint(qname, baseUrl, type, currTime, splitData) {
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
					$('#btnPrint').removeAttr('disabled');
					$('#btnPrint').removeClass('DisableButtons');
					getMessage(EpsonException.MoneyOrder_Not_Printed);
					return;
				}
				// activate the next ajax call when this one finishes
				$(document).dequeue(qname);
				if (type == "E") {					
					MoneyOrderPrintConfirmationPopup(
					$("#CheckNumber").val(),
					data.PrintCheckStreamResult.Scan_FrontImageTIFF,
					data.PrintCheckStreamResult.Scan_BackImageTIFF,
					$("#AccountNumber").val(),
					$("#RoutingNumber").val(),
					$("#MICR").val(),
					$("#NpsId").val(),
					$("#Amount").val()
					);
				}
			},
			error: function (data, errmessage, errdetails) {
				hideSpinner();
				if (data.ErrorNo != undefined) {
					$('#btnPrint').removeAttr('disabled');
					$('#btnPrint').removeClass('DisableButtons');
					getMessage(EpsonException.MoneyOrder_Not_Printed);
					return;
				}
				else {
					$('#btnPrint').removeAttr('disabled');
					$('#btnPrint').removeClass('DisableButtons');
					getMessage(EpsonException.Service_Connectivity);
					return;
				}
			},
			timeout: 30000
		});

	});
}

function ScanMoneyOrder(baseURL, moneyOrderId) {
	showSpinner();
	var scanFileType = "jpg";
	var imageSrc = "data:image/" + scanFileType + ";base64,";
	var scanParams = { ScanFileType: scanFileType, moneyOrderId: moneyOrderId };
	$.ajax({
		data: "{}",
		dataType: "jsonp",
		type: "GET",
		url: baseURL + "ScanCheck?scanparams=" + JSON.stringify(scanParams),
		contentType: "application/json; charset=UTF-8",
		processData: true,
		success: function (data) {
			if (data.ErrorNo != undefined) {
				hideSpinner();
				getMessage(EpsonException.MoneyOrder_Scan_Error);
				return;
			}
			var resultObject = data.ScanCheckResult.Scan_FrontImage;
			$("#FrontImage").val(resultObject);
			$("#CheckNumber").val(data.ScanCheckResult.MicrAuxillatyOnUSField.substring(0, 10));
			if (data.ScanCheckResult.MicrAuxillatyOnUSField == "") {
				getMessage(EpsonException.MoneyOrder_Not_Scaned);
			}
			$("#UniqueId").val("@uniqueId");
			$("#NpsId").val(data.ScanCheckResult.UniqueId);
			$("#AccountNumber").val(data.ScanCheckResult.MicrAccountNumber);
			$("#RoutingNumber").val(data.ScanCheckResult.MicrTransitNumber);
			$("#MICR").val(data.ScanCheckResult.Micr);
           MoneyOrderPrintPopup(data.ScanCheckResult.MicrAuxillatyOnUSField.substring(0, 10), data.ScanCheckResult.Scan_FrontImageTIFF, data.ScanCheckResult.Scan_BackImageTIFF, data.ScanCheckResult.UniqueId, data.ScanCheckResult.MicrAccountNumber, data.ScanCheckResult.MicrTransitNumber, data.ScanCheckResult.Micr)
		},
		error: function (data, errmessage, errdetails) {
			hideSpinner();
			if (data.ErrNo != undefined) {
				getMessage(EpsonException.MoneyOrder_Scan_Error);
				return;
			}
			else {
				getMessage(EpsonException.MoneyOrder_Scan_Error);
				return;
			}
		},
		timeout: 10000 //AL-1669 Changes for IE11 issue.
	});
}

function ShowMoneyOrderImage(frontImageData)
{
    var frontImageSrc = "data:image/jpeg;base64," + frontImageData;
    $("#MoneyOrderImage").attr("src", frontImageSrc);
}

function SetMoneyOrderImages(frontImageData) {
	var frontImageSrc = "data:image/jpeg;base64," + frontImageData;	
	$("#MoneyOrderImage").attr("src", frontImageSrc);
}

function ProcessMoneyOrderPopup() {
	jQuery.ajaxSetup({ cache: false });
	var $ScanMoneyOrder = $("<div id='ScanMoneyOrderPopup'></div>");
	$ScanMoneyOrder.empty();
	$ScanMoneyOrder.dialog({
		autoOpen: false,
		title: "Zeo - Scan Money Order Check",
		width: 630,
		draggable: false,
		modal: true,
		minHeight: 260,
		resizable: false,
		closeOnEscape: false,
		open: function (event, ui) {
		    $ScanMoneyOrder.load(MoneyOrderScanPopupURL, function () {
				$('#btnScan').focus();
			});
		},
		error: function (err) {
			showExceptionPopupMsg(err);
		}
	});
	$ScanMoneyOrder.dialog("open");
}
function MoneyOrderScanConfirmPopup() {
	jQuery.ajaxSetup({ cache: false });
	hideSpinner();
	var moneyOrderImage = "moneyOrderFrontImage=" + "" + "&moneyOrderCheckNumber=" + $("#CheckNumber").val() + "&moneyOrderAccountNumber=" + $("#AccountNumber").val() +  "&moneyOrderRoutingNumber=" + $("#RoutingNumber").val() +"&micr=" + $("#MICR").val()+"&npsId=" + $("NpsId").val();
	$('#ScanMoneyOrderPopup').dialog('destroy').remove();
	var $MoneyOrderConfirm = $("<div id='MoneyOrderConfirmPopup'></div>");
	$MoneyOrderConfirm.dialog({
		autoOpen: false,
		title: "Zeo - Scan Money Order Check Confirm",
		width: 650,
		draggable: false,
		modal: true,
		minHeight: 600,
		resizable: false,
		closeOnEscape: false,
		open: function (event, ui) {
			var url = MoneyOrderScanConfirmPopupURL + "?" + moneyOrderImage;
			$MoneyOrderConfirm.load(url, function () {
				$('#btnNext').focus();
			});
		},
		error: function (err) {
			showExceptionPopupMsg(err);
		}
	});
	$MoneyOrderConfirm.dialog("open");
}

function MoneyOrderPrintPopup(checkNumber, frontImage, backImage, npsId, accountNumber, routingNumber, micr) {
	
	jQuery.ajaxSetup({ cache: false });
	if (checkNumber == '') {
		$('#divCheckNumberValidation').show();
		return false;
	}
	var MoneyOrder = 
	{
		"CheckNumber": checkNumber,
		"FrontImage": frontImage,
		"BackImage": backImage,
		"NpsId": npsId,
		"AccountNumber": accountNumber,
		"RoutingNumber": routingNumber,
		"MICR": micr
	}

	//$('#ScanMoneyOrderPopup').dialog('destroy').remove();
	var MOCheckNumber = checkNumber;
	$('#divOk').dialog('destroy').remove();
	$('#MoneyOrderConfirmPopup').dialog('destroy').remove();

	var $PrintMoneyOrder = $("<div id='MoneyOrderPrintPopup'></div>");
	$PrintMoneyOrder.empty();
	$PrintMoneyOrder.dialog({
		autoOpen: false,
		title: "Zeo - Print Money Order Check",
		width: 630,
		draggable: false,
		modal: true,
		minHeight: 260,
		resizable: false,
		closeOnEscape: false,
		open: function (event, ui) {
		    $PrintMoneyOrder.load(MoneyOrderPrintPopupURL, MoneyOrder, function (response, status) {
		        if (status == "success") {
		            try{
		                var jsonObj = JSON.parse(response);//jsonresult parsed here, AL-722 changes
		                var responsestatus = jsonObj.success;
		                var errorMsg = jsonObj.data;
		                if (responsestatus == false) 
		                {
		                    $('#MoneyOrderPrintPopup').dialog('destroy').remove();
		                    hideSpinner();
		                    showExceptionPopupMsg(errorMsg); 
		                }
		            }
                    catch(e)
		            {
		                $('#ScanMoneyOrderPopup').dialog('destroy').remove();
		            }
		        }
		        $('#btnPrint').focus();

			});
		},
		error: function (err) {
			showExceptionPopupMsg(err);
		}
	});
	$PrintMoneyOrder.dialog("open");
}

function MoneyOrderPrintConfirmationPopup(checkNumber, frontImage, backImage, accountNumber, routingNumber, micr,npsId,amount) {	
	var MoneyOrder =
	{
		"CheckNumber": checkNumber,
		"FrontImage": frontImage,
		"BackImage": backImage,
		"AccountNumber": accountNumber,
		"RoutingNumber": routingNumber,
		"MICR": micr,
		"NpsId": npsId,
		"Amount":amount
	}
	$('#divOk').dialog('destroy').remove();
	$('#MoneyOrderPrintPopup').dialog('destroy').remove();
	hideSpinner();
	var $confirm = $("<div id='PrintConfirm'></div>");
	$confirm.empty();
	$confirm.dialog({
		autoOpen: true,
		title: "Confirm Money Order Check",
		width: 630,
		draggable: false,
		modal: true,
		minHeight: 260,
		resizable: false, 
		closeOnEscape: false,
		open: function (event, ui) {
		    $confirm.load(MoneyOrderPrintConfirmPopupURL, MoneyOrder, function (response, status) {
		        if (status == "success") {
		            try {
		                var jsonObj = JSON.parse(response);
		                var responsestatus = jsonObj.success;
		                var errorMsg = jsonObj.data;
		                if (responsestatus == false) {
		                    $('#PrintConfirm').dialog('destroy').remove();
		                    hideSpinner();
		                    showExceptionPopupMsg(errorMsg);
		                }
		            }
		            catch (e) {
		                $('#btnPrint').focus();
		            }
		        }
		        $('#btnPrint').focus();
			});
		}
	});
	$('#btnMoConfirm').focus(); 		
}
function ReplaceMoneyOrder() {
	ReplaceMoneyOrderDialogue();
}
function ReplaceMoneyOrderDialogue() {
	jQuery.ajaxSetup({ cache: false });
	var $replaceDialog = $("<div id='dlgReplaceMoneyOrder'></div>");
	$replaceDialog.empty();
	$replaceDialog.dialog({
		autoOpen: false,
		title: "Void Check",
		width: 400,
		draggable: false,
		modal: true,
		minHeight: 150,
		resizable: false,
		closeOnEscape: false,
		open: function (event, ui) {
			$replaceDialog.load(replaceMoneyOrderURL,function () {
				$('#btnMOReplace').focus();
			});
		}
	});
	$replaceDialog.dialog("open");
}
function ReScanMO() {
	$('#MoneyOrderConfirmPopup').dialog('destroy').remove();
	ProcessMoneyOrderPopup();
}
function ReplaceMO() {
	$('#dlgReplaceMoneyOrder').dialog('destroy').remove();
	$('#PrintConfirm').dialog('destroy').remove();
	$('#MoneyOrderPrintPopup').dialog('destroy').remove();
	ProcessMoneyOrderPopup();
}
function MOConfirm() {
	$('#PrintConfirm').dialog('destroy').remove();
	$('#MoneyOrderPrintPopup').dialog('destroy').remove();
	$('#btnSubmit').trigger('click');
}
