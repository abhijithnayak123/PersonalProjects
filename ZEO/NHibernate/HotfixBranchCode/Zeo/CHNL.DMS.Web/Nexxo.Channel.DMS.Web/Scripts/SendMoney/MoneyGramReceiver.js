$(document).ready(function () {	
	if($("input[name='IsReceiverHasPhotoId']:checked").val() == "True") {
		$(".testquestion").addClass("hide");
		$(".testquestion").removeClass("show");
		$("#TestQuestion, #TestAnswer").val('');
	}
	else {
		$(".testquestion").addClass("show");
		$(".testquestion").removeClass("hide");
	}

	$("#IsReceiverHasPhotoIdYes, #IsReceiverHasPhotoIdNo").change(function () {
		if (this.id == "IsReceiverHasPhotoIdYes") {
			$(".testquestion").addClass("hide");
			$(".testquestion").removeClass("show");
			$("#TestQuestion, #TestAnswer").val('');
		}
		else {
			$(".testquestion").addClass("show");
			$(".testquestion").removeClass("hide");
		}

	});
	$("#saveReceiverSubmit").click(function (event) {
		var selectedPickUpState = $('select#PickUpState');
		var pickUpStateRequiredStar = $('#pickUpStateRequiredStar');
		var divRequiredPickUpState = $('#divRequiredPickUpState');

		if (pickUpStateRequiredStar.css('display') == 'inline' && pickUpCityRequiredStar.css('display') == 'inline') {
			if (selectedPickUpState.val() == "" && selectedPickUpCity.val() == "") {
				divRequiredPickUpState.show();
				divRequiredPickUpCity.show();
				event.preventDefault();
				return false;
			}
		}
		if (pickUpStateRequiredStar.css('display') == 'inline') {
			if (selectedPickUpState.val() == "") {
				divRequiredPickUpState.show();
				event.preventDefault();
				return false;
			}
		}
	});

	$('select#PickUpCountry').change(function () {
		var selectedCountryCode = $(this).val();

		var pickUpStateRequired = $('#pickUpStateRequiredStar');
		var divRequiredPickUpState = $('#divRequiredPickUpState');

		var pickUpDeliveryMethod = $('select#DeliveryMethod');

		pickUpStateRequired.show();

		$('select#PickUpState').prop("disabled", false);
		var pickupStateDropDown = $('select#PickUpState');
		pickupStateDropDown.empty();
		pickupStateDropDown.html('<option value="">Select</option>');

		showSpinner();
		$.ajax({
		    url: MoneyTransfer_States_URL,
			data: { countryCode: selectedCountryCode }, //parameters go here in object literal form 
			type: 'GET',
			datatype: 'json',
			success: function (wuStates) {
				if (handleException(wuStates)) {
					return;
				}
				statesDropDown = $('select#PickUpState');
				bindDropdownList(statesDropDown, wuStates);
				hideSpinner();
			},
			error: function () {
				showExceptionPopupMsg(defaultErrorMessage);
				hideSpinner();
			}
		});
		if ($('select#PickUpState').prop('disabled') == true) {
			$('div#PickUpState').hide();
		}
		else {
			$('div#PickUpState').show();
		}
	});

	$("#Address").keydown(function (event) {
		var kCode = event.keyCode || event.charCode; // for cross browser check
		if (kCode == 16 || kCode == 192 || (kCode == 49 && event.shiftKey)
            || (kCode == 50 && event.shiftKey) || (kCode == 52 && event.shiftKey)
            || (kCode == 53 && event.shiftKey) || (kCode == 54 && event.shiftKey)
            || (kCode == 55 && event.shiftKey) || (kCode == 56 && event.shiftKey)
            || (kCode == 58 && event.shiftKey) || (kCode == 61)
            || (kCode == 219 && event.shiftKey) || (kCode == 221 && event.shiftKey)
            || (kCode == 221 && event.shiftKey) || (kCode == 188 && event.shiftKey)
            || (kCode == 190 && event.shiftKey) || (kCode == 191 && event.shiftKey)
            || (kCode == 220 && event.shiftKey) || kCode == 222
            || (kCode == 59 && event.shiftKey == false)
            ) {
			event.preventDefault();
		}
	});

	$("#StateProvince").keydown(function (event) {
		var kCode = event.keyCode || event.charCode; // for cross browser check
		if (kCode == 16 || kCode == 192 || kCode == 49 || kCode == 50 || kCode == 52 || (kCode == 51 && event.shiftKey == false)
                    || kCode == 53 || kCode == 54 || kCode == 48
                    || kCode == 55 || kCode == 56 || kCode == 57
                    || kCode == 58 || kCode == 61
                    || (kCode == 219) || (kCode == 221 && event.shiftKey)
                    || (kCode == 221) || (kCode == 188)
                    || (kCode == 190 && event.shiftKey) || (kCode == 191)
                    || (kCode == 220) || kCode == 222
                    || (kCode == 59 && event.shiftKey == false)
                    ) {
			event.preventDefault();
		}
	});


});       // document.ready end