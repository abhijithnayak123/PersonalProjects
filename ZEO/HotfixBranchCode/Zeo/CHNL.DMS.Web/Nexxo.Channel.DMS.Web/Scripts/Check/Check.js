function DisableControls() {
	var submitbutton = $(this).find('input[type="submit"]');
	var button = $(this).find('input[type="button"]');
	setTimeout(function () {
		$('#layout_left_pane_shadow').removeClass('displaynoneleftpanel');
		$('#layout_left_pane_shadow').addClass('displayblockleftpanel');
		button.addClass('DisableButtons');
		submitbutton.addClass('DisableButtons');
		$('.anc_link_btn').addClass('DisableButtons');
		submitbutton.attr('disabled', 'disabled');
		button.attr('disabled', 'disabled');
		$('.anc_link_btn').attr('disabled', 'disabled');
		$(".nav_item").each(function () {
			$(this).addClass("DisablePanels");
			$(this).attr('disabled', 'disabled');
		});
		$(".nav_name").each(function () {
			$(this).attr('disabled', 'disabled');
		});
		$('#btnfrankCheck').removeAttr('disabled');
		$('#btnfrankCheck').removeClass('DisableButtons');
		$('#btnYes').removeAttr('disabled');
		$('#btnYes').removeClass('DisableButtons');
		$('#divFrankPopup').find('input').removeClass('DisableButtons');
		$('#divFrankPopup').find('input').removeAttr('Disabled');
	}, 0);
}

function DisableButtonsScanCheck() {
	//to disable buttons and panel on scan check 
	if (window.location.pathname)
		if (window.location.pathname.indexOf("/CashaCheck/ScanACheck") >= 0) {
			$('#btnNext').addClass('DisableButtons');
			DisableControls();
		}
}

function ScanCheck(baseURL, uniqueId) {
	DisableButtonsScanCheck();
	if (window.location.pathname != undefined)
		if (window.location.pathname.indexOf("/CashaCheck/ScanACheck") >= 0) {
			var submitbutton = $(this).find('input[type="submit"]');
			var button = $(this).find('input[type="button"]');
			setTimeout(function () {
				button.addClass('DisableInputButtons');
				submitbutton.addClass('DisableSubmitButtons');
				button.addClass('OpaqueViewCart');
				submitbutton.addClass('OpaqueViewCart');
				$('.anc_link_btn').addClass('DisableHyperLinks');
				$('.anc_link_btn').addClass('OpaqueViewCart');
				submitbutton.attr('disabled', 'disabled');
				button.attr('disabled', 'disabled');
				$('#btnNext').addClass('OpaqueViewCart');
				$('#layout_left_pane_shadow').show();
				$('.anc_link_btn').attr('disabled', 'disabled');
				$(".nav_item").each(function () {
					$(this).addClass("DisablePanels");
					$(this).attr('disabled', 'disabled');
				});
				$(".nav_name").each(function () {
					$(this).attr('disabled', 'disabled');
				});
			}, 0);
		}
	var scanFileType = "tiff";
	var imageSrc = "data:image/" + scanFileType + ";base64,";
	var scanParams = { ScanFileType: scanFileType, UniqueId: uniqueId };
	showSpinner();
	$.ajax({
		data: "{}",
		dataType: "jsonp",
		type: "GET",
		url: baseURL + "ScanCheck?scanparams=" + JSON.stringify(scanParams),
		contentType: "application/json; charset=UTF-8",
		processData: true,
		success: function (data) {
			if (data.ErrorNo != undefined) {

				//alert("Scanning of check could not be completed and scanner returned error message, " + data.ErrorDetails);
				EnableButtons();
				hideSpinner();
				showExceptionPopupMsg($('#CheckScanError').val() + ', ' + data.ErrorDetails);
				return;
			}
			$("#CheckFrontImage").val(data.ScanCheckResult.Scan_FrontImageJPG);
			$("#CheckBackImage").val(data.ScanCheckResult.Scan_BackImageJPG);
			$("#CheckFrontImage_TIFF").val(data.ScanCheckResult.Scan_FrontImageTIFF);
			$("#CheckBackImage_TIFF").val(data.ScanCheckResult.Scan_BackImageTIFF);
			$("#MICRCode").val(data.ScanCheckResult.Micr);
			$("#UniqueId").val("@uniqueId");
			$("#NpsId").val(data.ScanCheckResult.UniqueId);
			$("#RoutingNumber").val(data.ScanCheckResult.MicrTransitNumber);
			$("#CheckNumber").val(data.ScanCheckResult.MicrAuxillatyOnUSField.substring(0, 10));
			$("#AccountNumber").val(data.ScanCheckResult.MicrAccountNumber);
			$("#MicrError").val(data.ScanCheckResult.MicrError);

			if (data.ScanCheckResult.MicrError == 1) {
				$('#MicrErrorMessage').val($('#BadMicrError').val());
			}
			$("#Send").click();
			hideSpinner();
		},
		error: function (data, errmessage, errdetails) {
			EnableButtons();
			hideSpinner();
			if (data.ErrNo != undefined) {
				showExceptionPopupMsg($('#CheckScanError').val() + ', ' + data.ErrorDetails);
				return;
			}
			else {
				showExceptionPopupMsg($('#CheckScanError').val());
				return;
			}
		},
		timeout: 10000 //AL-1669 Changes for IE11 issue.
	});
}

function ScanCheckMICR(baseURL, uniqueId) {
	showSpinner();
	$.ajax({
		data: "{}",
		dataType: "jsonp",
		type: "GET",
		url: baseURL + "ScanCheck",
		contentType: "application/json; charset=UTF-8",
		processData: true,
		success: function (data) {
			if (data.ErrorNo != undefined) {
				EnableButtons();
				hideSpinner();
				showExceptionPopupMsg($('#CheckScanError').val() + ', ' + data.ErrorDetails);
				return;
			}
			$("#RoutingNumber").val(data.ScanCheckResult.MicrTransitNumber);
			$("#CheckNumber").val(data.ScanCheckResult.MicrAuxillatyOnUSField.substring(0, 10));
			$("#AccountNumber").val(data.ScanCheckResult.MicrAccountNumber);			
			$("#NpsId").val(data.ScanCheckResult.UniqueId);
			$('#MICRCode').val(data.ScanCheckResult.Micr);
			$("#MicrError").val(data.ScanCheckResult.MicrError);
			if (data.ScanCheckResult.MicrError == 1) {
				$('#MicrErrorMessage').val($('#BadMicrError').val());
			}
			$("#Send").click();
			hideSpinner();
		},
		error: function (data, errmessage, errdetails) {
			EnableButtons();
			hideSpinner();
			if (data.ErrNo != undefined) {
				showExceptionPopupMsg($('#CheckScanError').val() + ', ' + data.ErrorDetails);
				return;
			}
			else {
				showExceptionPopupMsg($('#CheckScanError').val());
				return;
			}
		},
		timeout: 10000
	});
}

function ShowCheckFrontImage(frontImageData)
{
    var frontImageSrc = "data:image/jpeg;base64," + frontImageData;
    $("#CheckFrontImage").attr("src", frontImageSrc);
}

function ShowCheckBackImage(backImageData)
{
    var backImageSrc = "data:image/jpeg;base64," + backImageData;
    $("#CheckBackImage").attr("src", backImageSrc);
}

function ClosePopup() {
	$('#divPoupClose').dialog('destroy').remove();
	$('#divPrepaid').dialog('destroy').remove();
}

function EnableButtons() {
	if (window.location.pathname)
		if (window.location.pathname.indexOf("ShoppingCart/ShoppingCartCheckout") >= 0 || window.location.pathname.indexOf("/CashaCheck/ScanACheck") >= 0) {
			var submitbutton = $(this).find('input[type="submit"]');
			var button = $(this).find('input[type="button"]');
			$('#layout_left_pane_shadow').addClass('displaynoneleftpanel');
			$('#layout_left_pane_shadow').removeClass('displayblockleftpanel');
			button.removeClass('DisableButtons');
			submitbutton.removeClass('DisableButtons');
			$('#btnNext').removeClass('DisableButtons');
			$('#btnNext').css('opacity', '1');
			$('.anc_link_btn').removeClass('DisableButtons');
			submitbutton.removeAttr('disabled');
			button.removeAttr('disabled');
			$('.anc_link_btn').removeAttr('disabled');
			$(".nav_item").each(function () {
				$(this).removeClass("DisablePanels");
				$(this).removeAttr('disabled');
			});
			$(".nav_name").each(function () {
				$(this).removeAttr('disabled', 'disabled');
			});
		}
}

function CheckDeclinedReceiptPrint()
{
	if (isDeclinedCheck == "True") {
		$.ajax({
			url: checkDeclinedReceiptDataURL,
			data: { transactionId: checkDeclinedTransactionId, dt: (new Date()).getTime() },
			type: 'POST',
			datatype: 'json',
			success: function (jsonData) {
				if (jsonData.success) {
					var printParams = jsonData.data;
					for (var receiptIndex = 0; receiptIndex < printParams.length; receiptIndex++) {
						for (var copyIndex = 0; copyIndex < printParams[receiptIndex].NumberOfCopies; copyIndex++) {
							if (printParams[receiptIndex].PrintData != "")
								PrintReceipt(NPSbaseURL, printParams[receiptIndex].PrintData);
							else {
								hideSpinner();
								showExceptionPopupMsg("Receipt template missing");
							}
						}
					}
				} else {
					showExceptionPopupMsg(jsonData.data);
				}
			},
			complete: function () {
			},
			error: function (err) {
				showExceptionPopupMsg(err);
			}
		});
	}

}

function feeChangePopUp(oldFee, newFee, checkId) {

    var $confirm = $("<div id='removeCheckPopup'></div>");
    $confirm.empty();
    $confirm.dialog({
        autoOpen: false,
        title: "MGiAlloy",
        width: 380,
        draggable: false,
        modal: true,
        resizable: false,
        closeOnEscape: false,
        minHeight: 150,
        open: function (event, ui) {
            $confirm.load(FeeChangeOnCheckTypeChange_URL, { baseFee: oldFee, netFee: newFee, checkId: checkId });
        }
    });
    $confirm.dialog("open");
}

