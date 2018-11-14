$(document).ready(function () {
	$('.validate').click(function (e) {
		e.preventDefault();
		var ssn = $(this).hasClass('SSN'); //based on Product and Processors
		var swb = $(this).hasClass('SWB'); //based on Product and Processors
		Validate(e, ssn, swb);
	});
});

function Validate(e, ssn, swb) {
	if (ssn) {
		ssnValidation(e, function (status) {
			if (status) {
				SWBValidation(e, swb);
			}
		});
	}
	else {
		SWBValidation(e, swb);
	}
}


function ssnValidation(e, ssnresult) {
	$.ajax({
		url: SSNValidationUrl,
		data: {},
		type: 'GET',
		datatype: 'json',
		success: function (jsonData) {
			if (jsonData) {
				ssnresult(false);
				showSSNValidationWarningPopup();
			}
			else {
				ssnresult(true);
			}
		},
		error: function (err) {
			showExceptionPopupMsg(err.data);
		}
	});
}

function showSSNValidationWarningPopup() {
	showCartAbandonmentConfirm = true;

	var $confirmation = $("<div id='dlgSSNValidationWarning'></div>");
	$confirmation.empty();
	$confirmation.dialog({
		autoOpen: false,
		title: "ZEO",
		width: 400,
		draggable: false,
		modal: true,
		minHeight: 150,
		resizable: false,
		closeOnEscape: false,
		open: function (event, ui) {
			$confirmation.load(SSNValidationWarningUrl);
		}
	});
	$confirmation.dialog("open");
}

function SWBValidation(e, swb) {
	if (swb) {
		CashierLocationState(e);
	}
	else {
		RedirectToUrl(e.target.href);
	}
}

function CashierLocationState(e) {
	$.ajax({
		type: "POST",
		url: Product_URL,
		dataType: 'json',
		contentType: 'application/json; charset=utf-8',
		success: function (result) {
		    if (handleException(result))
		        return;
			if (result.isStateAvailable == true) {
				var CashierAgree = $('#hdnCashierAgree').val();
				if (CashierAgree == '' || CashierAgree == 'false') {
					SWBAgreeURL = e.target.href;
					NotifyPopUp(result.agentFirstName, result.agentLastName);
				} else {
					RedirectToUrl(e.target.href);
				}
			} else {
				RedirectToUrl(e.target.href);
			}
		},
		error: function (err) {
			showExceptionPopupMsg(err.data);
		}
	});
}

function NotifyPopUp(agentFirstName, agentLastName) {
	showCartAbandonmentConfirm = true;

	var $confirm = $("<div id='divCashierValidate'></div>");
	$confirm.empty();
	$confirm.dialog({
		autoOpen: false,
		title: "ZEO",
		width: 480,
		draggable: false,
		modal: true,
		resizable: false,
		closeOnEscape: false,
		minHeight: 150,
		open: function (event, ui) {
			$confirm.load(CashierValidateUrl, { agentFirstName: agentFirstName, agentLastName: agentLastName });
		}
	});
	$confirm.dialog("open");
}

function destroyValidatePopup() {
	$('#divCashierValidate').dialog('destroy').remove();
}

function Redirect() {
	RedirectToUrl(SWBAgreeURL);
}
