$(document).ready(function () {
	var DestinationAmount = $("#DestinationAmount").val();
	var TransferAmount = $("#TransferAmount").val();
	var prohibited = sendMoneyDetTestQuestion;

	if (typeof invalidPromoCode != 'undefined') {
		if (invalidPromoCode == 'T3006') {
			$('#CouponPromoCode').val('');
			$("#btnSubmit").attr("disabled", "disabled").addClass("disableText");
		}
	}


	$('#CouponPromoCode, #TransferAmount').on('change', function () {
		if ($('#CouponPromoCode').val())
		{
			var couponCode = $('#CouponPromoCode').val().toUpperCase();
			$('#CouponPromoCode').val(couponCode);

			var amount = $('#TransferAmount').val();
			var type = 3;
			validatePromocode(type, amount, couponCode, 'CouponPromoCode', 301)
		}
	});

	$("#MessageFeeWithCurrency").val('$ ' + $("#MessageFee").val() + ' USD');
	$('select#DeliveryMethod').change(function () {
		var deliveryMethod = $(this).val();
		if (deliveryMethod != "") {
			fillDeliveryOptions();
		}
		else {
			var deliveryOptionDropDown = $('select#DeliveryOptions');
			deliveryOptionDropDown.empty();
			deliveryOptionDropDown.html('<option value="">Select</option>');
		}
	});

	var deliveryOptionsDropDown = $('select#DeliveryOptions');
	if (!deliveryOptionsDropDown.html()) {
		var items = '<option value="">Not Applicable</option>';
		deliveryOptionsDropDown.html(items);
	}

	$("#PersonalMessage").keypress(function (event) {
		if (!((event.keyCode > 47 && event.keyCode < 58) || (event.keyCode > 96 && event.keyCode < 123) || (event.keyCode > 64 && event.keyCode < 91) || (event.keyCode == 46) || (event.keyCode == 44) || (event.keyCode == 32) || (event.keyCode == 8))) {
			return false;
		}
	});

	$("#TransferAmount").change(function () {
		if (DestinationAmount > 0) {
			$("#DestinationAmount").val('');
			$("#DestinationAmountWithCurrency1").val('');
		}
		disableAndEnableTQ();
	});

	$("#DestinationAmount").change(function () {
		if (TransferAmount > 0) {
			$("#TransferAmount").val('');
			$("#TransferAmountWithCurrency").val('');
			$("#Amount").val('');
		}
	});

	var testQuestion = $('#TestQuestion');
	var testAnswer = $('#TestAnswer');
	var prohibited = sendMoneyDetTestQuestion;
	var transferAmount = $("#TransferAmount").val();
	disableAndEnableTQ();

	$('#TransferAmount').keypress(function (event) {
		if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || ((event.keyCode == 65 || event.keyCode == 86) && event.ctrlKey === true)) {
			return;
		}
		else {
			if (event.keyCode < 48 || event.keyCode > 57) {
				event.preventDefault();
			}
		}
	});

	$('#DestinationAmount').keypress(function (event) {
		if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || ((event.keyCode == 65 || event.keyCode == 86) && event.ctrlKey === true)) {
			return;
		}
		else {
			if (event.keyCode < 48 || event.keyCode > 57) {
				event.preventDefault();
			}
		}
	});


	$("#CurrencyType").change(function () {
		$("#btnSubmit").attr("disabled", "disabled").addClass("disableText");
	});

	$("#TransferAmount").keyup(function () {
		$("#btnSubmit").attr("disabled", "disabled").addClass("disableText");
	});

	$("#DeliveryMethod").change(function () {
		$("#btnSubmit").attr("disabled", "disabled").addClass("disableText");
	});

	$("#CouponPromoCode").keyup(function () {
		$("#btnSubmit").attr("disabled", "disabled").addClass("disableText");
	});

	$("#DestinationAmount").keyup(function () {
		$("#btnSubmit").attr("disabled", "disabled").addClass("disableText");
	});

	$("#btnUpdate").click(function () {
		$("#btnSubmit").attr("disabled", false).removeClass("disableText");
	});

	if (sendMoneyDetIsAmountValid == 'False')
		$("#btnSubmit").attr("disabled", 'disabled').removeClass("disableText");

	var destval = $('#DestinationAmountWithCurrency1').val();
	if (destval == 'Not Applicable') {
	    $('#DestinationAmountWithCurrency1').attr('disabled', 'disabled').addClass("disable_txt");
	}
    //AL-6293 starts
	$('#PersonalMessage').on('paste', function () {
	    setTimeout(function () {
	        regex = /[`~!@#$%^&*()_|+\-=?;:'"<>\{\}\[\]\\\/]/gi;
	        sanitizeTextOnPaste('#PersonalMessage', regex);
	    }, 0);
	});//Ends

	$("#PersonalMessage").keyup(function () {

		$("#btnSubmit").attr("disabled", "disabled").addClass("disableText");
	});

	$("#DeliveryOptions").change(function () {
		$("#btnSubmit").attr("disabled", "disabled").addClass("disableText");
	});

	$('#DestinationAmountWithCurrency1').focus(function () {
		$('#destAmountSymbols').hide();
		$('#destAmount').show();
		$('#DestinationAmount').focus();
	});

	$('#DestinationAmount').blur(function () {
		$('#destAmountSymbols').show();
		$('#destAmount').hide();
		var cur = $('#CurrencyType').val();
		if (cur == null)
			cur = '';
		if ($('#DestinationAmount').val() != '') {
			var amountModified = $('#DestinationAmount').val() + ' ' + cur;
			$('#DestinationAmountWithCurrency1').val(amountModified)
		}
		else {
			$('#DestinationAmountWithCurrency1').val('');
		}
	});

	$('#TransferAmountWithCurrency').focus(function () {
		$('#transamtwithSymbols').hide();
		$('#transamt').show();
		$('#TransferAmount').focus();
		//$('#TransferAmount').val('');
	});

	$('#TransferAmountWithCurrency').siblings('.clearlink').mousedown(function () {
	    $('#TransferAmountWithCurrency').val('');
	    $('#TransferAmount').val('').focus();
	    updateTransferAmount();
	});
	$('#TransferAmount').siblings('.clearlink').mousedown(function () {
	    $('#TransferAmountWithCurrency').val('');
	    $('#TransferAmount').val('').focus();
	    updateTransferAmount();
	});

	$('#TransferAmount').blur(function () {
	    updateTransferAmount();
	});
	function updateTransferAmount() {
	    $('#transamtwithSymbols').show();
	    $('#transamt').hide();
	    var amount = $('#TransferAmount').val();
	    if (amount != "" && amount != "0") {
	        var amountModified = "$ " + amount + " USD";
	        $('#TransferAmountWithCurrency').val(amountModified);
	        $("#btnUpdate").attr("disabled", false).removeClass("disableText");
	    }
	    else {
	        amount = "0";
	        $('#TransferAmountWithCurrency').val(amount);
	        $("#btnUpdate").attr("disabled", "disabled").addClass("disableText");
	        $("#btnSubmit").attr("disabled", "disabled").addClass("disableText");
	    }
	}
	if (lpmtHasError == 'true') {
		showGprTrxPopup();
	}

	function showGprTrxPopup() {
		var $confirmation = $("<div id='lpmtMessage'></div>");
		$confirmation.empty();
		$confirmation.dialog({
			autoOpen: false,
			title: "Message",
			width: 373,
			draggable: false,
			modal: true,
			minHeight: 150,
			resizable: false,
			closeOnEscape: false,
			open: function (event, ui) {
				$confirmation.load(lpmtDialog);
			}
		});
		$confirmation.dialog("open");
	}

	$('#CouponPromoCode').focus(); //To hide the clearlink('X') on load

    $("#btnSubmit").click(function (e) {
        if (!isFormPost) {
            e.preventDefault();
            var current = $(this);

            var amount = 0;
            var destinationAmount = 0;

            var recipientCurrency = $('#CurrencyType').val();
            if ($('#DestinationAmount') && $('#DestinationAmount').val() != 0) {
                destinationAmount = $('#DestinationAmount').val();
            }

            if ($('#TransferAmount').val() && $('#TransferAmount').val() != 0) {
                amount = $('#TransferAmount').val();
            }

            var recipientCountryCode = $('#CountryCode').val();
            if ((destinationAmount != 0 || amount != 0) && (recipientCountryCode != null && recipientCountryCode != '')) {
                $.ajax({
                    url: fraudLimitCheck_URL,
                    data: { recipientCountryCode: recipientCountryCode, recipientCountryCurrency: recipientCurrency, amount: amount, destinationAmount: destinationAmount },
                    type: 'GET',
                    datatype: 'json',
                    success: function (result) {
                        if (result != null) {
                            if (result.data != undefined) {
                                showExceptionPopupMsg(result.data);
                            }
                            else if (result && result.limitCheckRequired) {
                                $('#IsFraudCheck').val(true);
                                ShowPopUp(fraudWarningPopUp_URL, 'Customer Fraud Warning', 400, 150);
                            }
                            else {
                                isFormPost = true;
                                $('#IsFraudCheck').val(false);
                                $('#ConsumerFraudPromptQuestion').val(false);
                                current.trigger("click");
                            }
                        }
                    },
                    error: function (err) {
                        showExceptionPopupMsg(err);
                    }
                });
            }
        }

    });

}); // end of document.ready

$("#CurrencyType").change(function () {
	$("#btnSubmit").attr("disabled", "disabled").addClass("disableText");
});

function disableAndEnableTQ() {
	var testQuestion = $('#TestQuestion');
	var testAnswer = $('#TestAnswer');
	var prohibited = sendMoneyDetTestQuestion;
	var transferAmount = $("#TransferAmount").val();
	if (transferAmount == "") {
		transferAmount = "0";
		$("#TransferAmount").val(transferAmount);
	}

	var country = sendMoneyDetCountry;

	$('#testQuestionmandatory').show();
	$('#testAnswermandatory').show();
	$('#TestQuestion').attr("disabled", false).removeClass("disable_txt");
	$('#TestAnswer').attr("disabled", false).removeClass("disable_txt");

	if (country == 'US' && transferAmount >= 300) {
		var tqEle = $('#TestQuestion').parent();
		var tqAns = $('#TestAnswer').parent();
		tqEle.next().hide();
		tqAns.next().hide();
		$("#TestQuestion").val('');
		$("#TestAnswer").val('');
		$('#testQuestionmandatory').hide();
		$('#testAnswermandatory').hide();
		$('#TestQuestion').attr("disabled", true).addClass("disable_txt");
		$('#TestAnswer').attr("disabled", true).addClass("disable_txt");
	}
	else if (prohibited == 'P') {
		$('#testQuestionmandatory').hide();
		$('#testAnswermandatory').hide();
		$('#TestQuestion').attr("disabled", true).addClass("disable_txt");
		$('#TestAnswer').attr("disabled", true).addClass("disable_txt");

	}
	else if (prohibited == 'Y') {
		var tqEle = $('#TestQuestion').parent();
		var tqAns = $('#TestAnswer').parent();
		tqEle.next().show();
		tqAns.next().show();
		$('#testQuestionmandatory').show();
		$('#testAnswermandatory').show();
		$('#TestQuestion').attr("disabled", false).removeClass("disable_txt");
		$('#TestAnswer').attr("disabled", false).removeClass("disable_txt");
	}
	else if (prohibited == 'N') {
		$('#testQuestionmandatory').hide();
		$('#testAnswermandatory').hide();
		$('#TestQuestion').attr("disabled", false).removeClass("disable_txt");
		$('#TestAnswer').attr("disabled", false).removeClass("disable_txt");
	}
	else if (prohibited == '') {
		$('#testQuestionmandatory').hide();
		$('#testAnswermandatory').hide();
	}
}

function fillDeliveryOptions() {
	var selectedCountryCode = $('#Country').val();
	var selectedStateName = $('#StateProvince').text();
	var selectedCityName = $('#City').text();
	var DeliveryOptions_URL = sendMoneyDetWUDeliveryOptions;

	//Fill Delivery Options based on Delivery Method Selection
	var deliveryMethodDropDown = $('select#DeliveryMethod option:selected');
	var deliveryOptionsDropDown = $('select#DeliveryOptions');

	var selectedSVCCode = deliveryMethodDropDown.val();

	if ($('#StateProvince').val() == "") {
		selectedStateName = "";
	}
	if ($('#City').val() == "") {
		selectedCityName = "";
	}
	showSpinner();
	$.ajax({
		url: DeliveryOptions_URL,
		data: { countryCode: selectedCountryCode, state: selectedStateName, city: selectedCityName, svcCode: selectedSVCCode }, //parameters go here in object literal form 
		type: 'GET',
		datatype: 'json',
		success: function (wuOptions) {
			if (handleException(wuOptions)) return;
			deliveryOptionsDropDown.empty();
			var items = '';
			if (wuOptions.length > 0) {
				$.each(wuOptions, function (i, deliveryOption) {
					if (deliveryOption.Text != undefined) {
						items += '<option value="' + deliveryOption.Value + '">' + deliveryOption.Text + '</option>';
					}
					else {
						items = '<option value="">Not Applicable</option>';
					}
				});
			}
			else {
				items = '<option value="">Not Applicable</option>';
			}

			deliveryOptionsDropDown.html(items);
			hideSpinner();
		},
		error: function () {
			showExceptionPopupMsg(defaultErrorMessage);
			hideSpinner();
		}
	});
}

function setConsumerPrompt(consumerFraudPromptQuestion) {
    isFormPost = true;
    hideSpinner();
    $('#ConsumerFraudPromptQuestion').val(consumerFraudPromptQuestion);
    $("#btnSubmit").trigger("click");
}

function showTelemarketingError(messageKey) {
    messageKey = messageKey.replace(/&#39;/g, "'");
    messageKey = messageKey.replace("\\u0027", "'");
    debugger;
    var id = $("#TransactionId").val();
    var $confirm = $("<div id='divPopUp'></div>");
    $confirm.empty();
    $confirm.dialog({
        autoOpen: false,
        title: "SYSTEM MESSAGE",
        width: 505,
        draggable: false,
        resizable: false,
        closeOnEscape: false,
        modal: true,
        minheight: 176,
        cache: false,
        open: function (event, ui) {
            $confirm.load(CancelFraudTransaction_URL, { "id": id, "message": messageKey },
                function (responseText, textStatus, XMLHttpRequest) {
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




