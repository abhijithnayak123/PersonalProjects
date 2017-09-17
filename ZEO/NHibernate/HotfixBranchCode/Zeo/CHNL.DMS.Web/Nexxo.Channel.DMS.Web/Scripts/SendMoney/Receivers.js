$(document).ready(function () {
	function fillDeliveryOptions() {
		var selectedCountryCode = $('select#PickUpCountry').val();
		var selectedStateName = $('select#PickUpState option:selected').text();
		var selectedCityName = $('select#PickUpCity option:selected').text();
		//var DeliveryOptions_URL = '@Url.Action("WUDeliveryOptions", "SendMoney")';

		//Fill Delivery Options based on Delivery Method Selection
		var deliveryMethodDropDown = $('select#DeliveryMethod option:selected');
		var deliveryOptionsDropDown = $('select#DeliveryOptions');

		var selectedSVCCode = deliveryMethodDropDown.val();

		if ($('select#PickUpState option:selected').val() == "") {
			selectedStateName = "";
		}
		if ($('select#PickUpCity option:selected').val() == "") {
			selectedCityName = "";
		}
		showSpinner();
		$.ajax({
			url: DeliveryOptions_URL,
			data: { countryCode: selectedCountryCode, state: selectedStateName, city: selectedCityName, svcCode: selectedSVCCode }, //parameters go here in object literal form 
			type: 'GET',
			datatype: 'json',
			success: function (wuOptions) {
				if (handleException(wuOptions)) {
					hideSpinner();
					return;
				}
				deliveryOptionsDropDown.empty();
				var items = '';
				$.each(wuOptions, function (i, deliveryOption) {
					if (deliveryOption.Text != undefined) {
						items += '<option value="' + deliveryOption.Value + '">' + deliveryOption.Text + '</option>';
					}
					else {
						items = '<option value="">Select</option>';
					}
				});
				deliveryOptionsDropDown.html(items);
				hideSpinner();
			},
			error: function () {
				showExceptionPopupMsg(defaultErrorMessage);
				hideSpinner();
			}
		});

	}




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

	$('select#PickUpCity').change(function () {
		//fill Delivery Method DDL
		fillDeliveryMethods();
	});

	function fillDeliveryMethods() {
		// Make ajax call & fill the DeliveryMethod DDL
		var selectedCountryCode = $('select#PickUpCountry').val();
		//var countryCurrencyCode = $("#CurrencyType").val();
		var selectedStateCode = $('select#PickUpState option:selected').val();
		var selectedStateName = $('select#PickUpState option:selected').text();
		var selectedCityName = $('select#PickUpCity option:selected').text();
		//var DeliveryMethods_URL = '@Url.Action("WUDeliveryMethods", "SendMoney")';
		var pickUpDeliveryOptions = $('select#DeliveryOptions');
		if ($('select#PickUpState option:selected').val() == "") {
			selectedStateName = "";
		}
		if ($('select#PickUpCity option:selected').val() == "") {
			selectedCityName = "";
		}
		showSpinner();
		$.ajax({
			url: DeliveryMethods_URL,
			data: { countryCode: selectedCountryCode, state: selectedStateName, stateCode: selectedStateCode, city: selectedCityName }, //parameters go here in object literal form 
			type: 'GET',
			datatype: 'json',
			success: function (wuMethods) {
				if (handleException(wuMethods)) {
					hideSpinner();
					return;
				}
				deliveryMethodDropDown = $('select#DeliveryMethod');
				deliveryMethodDropDown.empty();
				var items = '';
				$.each(wuMethods, function (	i, deliveryMethod) {
					if (deliveryMethod.Text != undefined) {
						items += '<option value="' + deliveryMethod.Value + '">' + deliveryMethod.Text + '</option>';
					}
					else {
						items = '<option value="">Select</option>';
					}
				});
				$('select#DeliveryMethod').html(items);
				hideSpinner();
			},
			error: function () {
				showExceptionPopupMsg(defaultErrorMessage);
				hideSpinner();
			}
		});
		pickUpDeliveryOptions.empty();
		pickUpDeliveryOptions.html('<option value="">Select</option>');
	}

	//Save Receiver Button Click
	$("#saveReceiverSubmit").click(function (event) {

		var selectedPickUpState = $('select#PickUpState');
		var selectedPickUpCity = $('select#PickUpCity');
		var pickUpStateRequiredStar = $('#pickUpStateRequiredStar');
		var pickUpCityRequiredStar = $('#pickUpCityRequiredStar');
		var divRequiredPickUpState = $('#divRequiredPickUpState');
		var divRequiredPickUpCity = $('#divRequiredPickUpCity');

		if (pickUpStateRequiredStar.css('display') == 'inline' && pickUpCityRequiredStar.css('display') == 'inline') {
			if (selectedPickUpState.val() == "" && selectedPickUpCity.val() == "") {
				divRequiredPickUpState.show();
				divRequiredPickUpCity.show();
				event.preventDefault();
				return false;
			}
		}
		if (pickUpCityRequiredStar.css('display') == 'inline') {

			if (selectedPickUpCity.val() == "") {
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


	//Check for new or existing receiver 
	var receiverId = $('#ReceiverId').val();
	if (receiverId == "0") {
		//Add
		$('#divAddReceiverMenuHeader').show();
		$('#divEditReceiverMenuHeader').hide();
		if ($('select#PickUpCountry').val() != "") {
			selectPickUpStateAndCity();
		}
	} //Add
	else {
		//Edit
		$('#divAddReceiverMenuHeader').hide();
		$('#divEditReceiverMenuHeader').show();

		selectPickUpStateAndCity();

	} //Edit

	//Pickup country item selected
	$('select#PickUpCountry').change(function () {
		var selectedCountryCode = $(this).val();
		var citiesDropDown = $('select#PickUpCity');

		var pickUpStateRequired = $('#pickUpStateRequiredStar');
		var pickUpCityRequiredStar = $('#pickUpCityRequiredStar');
		var divRequiredPickUpState = $('#divRequiredPickUpState');
		var divRequiredPickUpCity = $('#divRequiredPickUpCity');    

		var pickUpDeliveryMethod = $('select#DeliveryMethod');
		var pickUpDeliveryOptions = $('select#DeliveryOptions');
		resetValidation()

		if (selectedCountryCode.toString() == 'US' || selectedCountryCode.toString() == 'MX' || selectedCountryCode.toString() == 'CA') {

			pickUpStateRequired.show();

			$('select#PickUpState').prop("disabled", false);
			var pickupStateDropDown = $('select#PickUpState');
			pickupStateDropDown.empty();
			pickupStateDropDown.html('<option value="">Select</option>');

			if (selectedCountryCode.toString() != 'MX') {
				$('select#PickUpCity').prop("disabled", true);
				citiesDropDown.empty();
				divRequiredPickUpCity.prop("disabled", true);
				divRequiredPickUpCity.empty();
				citiesDropDown.html('<option value="">Not Applicable</option>');				
				pickUpCityRequiredStar.hide();
			}
			else {
			    pickUpCityRequiredStar.show();
			    divRequiredPickUpCity.show();

				$('select#PickUpCity').prop("disabled", false);
				citiesDropDown.empty();
				citiesDropDown.html('<option value="">Select</option>');
			}
			showSpinner();
			$.ajax({
				url: WUStates_URL,
				data: { countryCode: selectedCountryCode }, //parameters go here in object literal form 
				type: 'GET',
				datatype: 'json',
				success: function (wuStates) {
					if (handleException(wuStates)) {
						hideSpinner();
						return;
					}
					statesDropDown = $('select#PickUpState');
					statesDropDown.empty();
					var items = '';
					$.each(wuStates, function (i, state) {
						if (state.Text != undefined) {
							if (state.Text != "CurrencyCode") {
								items += '<option value="' + state.Value + '">' + state.Text + '</option>';
							}
						}
						else {
							items = '<option value="">Select</option>';
						}

					});
					$('select#PickUpState').html(items);
					//fill Delivery Method DDL
					fillDeliveryMethods();
					hideSpinner();
				},
				error: function () {
					showExceptionPopupMsg(defaultErrorMessage);
					hideSpinner();
				}
			});
		}
		else {
			pickUpStateRequired.hide();
			pickUpCityRequiredStar.hide();

			divRequiredPickUpState.hide();
			divRequiredPickUpCity.hide();

			var pickupStateDropDown = $('select#PickUpState');
			var citiesDropDown = $('select#PickUpCity');

			if (selectedCountryCode.toString() == "") {
				$('select#PickUpState').prop("disabled", false);
				$('select#PickUpCity').prop("disabled", false);

				pickupStateDropDown.empty();
				pickupStateDropDown.html('<option value="">Select</option>');

				citiesDropDown.empty();
				citiesDropDown.html('<option value="">Select</option>');

				pickUpDeliveryMethod.empty();
				pickUpDeliveryMethod.html('<option value="">Select</option>');

				pickUpDeliveryOptions.empty();
				pickUpDeliveryOptions.html('<option value="">Select</option>');
			}
			else {
				$('select#PickUpState').prop("disabled", true);
				$('select#PickUpCity').prop("disabled", true);

				pickupStateDropDown.empty();
				pickupStateDropDown.html('<option value="">Not Applicable</option>');

				citiesDropDown.empty();
				citiesDropDown.html('<option value="">Not Applicable</option>');

				//fill Delivery Method DDL
				fillDeliveryMethods();
			} //Others Country		
		}
		if ($('select#PickUpState').prop('disabled') == true) {
			$('div#PickUpState').hide();
		}
		else {
			$('div#PickUpState').show();
		}
	});

	//Pickup city item selected
	$('select#PickUpCity').change(function () {
		$('#divRequiredPickUpCity').attr('style', 'display:none;')
	});

	//Pickup state item selected
	$('select#PickUpState').change(function () {
		var selectedstateCode = $(this).val();
		var selectedCountryCode = $('select#PickUpCountry');

		var pickUpStateRequired = $('#pickUpStateRequiredStar');
		var pickUpCityRequired = $('#pickUpCityRequiredStar');
		var divRequiredPickUpState = $('#divRequiredPickUpState');
		var divRequiredPickUpCity = $('#divRequiredPickUpCity');

		var pickUpDeliveryMethod = $('select#DeliveryMethod');
		var pickUpDeliveryOptions = $('select#DeliveryOptions');

		if (selectedCountryCode.val().toString() == 'US' || selectedCountryCode.val().toString() == 'MX' || selectedCountryCode.val().toString() == 'CA') {

			if (selectedCountryCode.val().toString() == 'MX') {

				if (selectedstateCode == "") {
					pickUpStateRequired.show();
					//divRequiredPickUpState.hide();
					$('#divRequiredPickUpState').attr('style', 'display:none;')
					pickUpCityRequired.show();
					//divRequiredPickUpCity.hide();
					$('#divRequiredPickUpCity').attr('style', 'display:none;')
					pickUpDeliveryOptions.empty();
					pickUpDeliveryOptions.html('<option value="">Select</option>');
				}
				else {
				    $('#divRequiredPickUpState').attr('style', 'display:none;')
				}

			} //MX
			else {
				if (selectedstateCode == "") {
					pickUpStateRequired.show();
					//divRequiredPickUpState.hide();
					$('#divRequiredPickUpState').attr('style', 'display:none;')
					pickUpCityRequired.hide();
					pickUpCityRequiredStar.hide();
					//divRequiredPickUpCity.hide();
					$('#divRequiredPickUpCity').attr('style', 'display:none;')
					pickUpDeliveryOptions.empty();
					pickUpDeliveryOptions.html('<option value="">Select</option>');
				}
				else {
					$('#divRequiredPickUpState').attr('style', 'display:none;')
					$('#divRequiredPickUpCity').attr('style', 'display:none;')
				}

			} //US & CA
		}
		else {
			if (selectedstateCode == "") {
				pickUpStateRequired.hide();
				pickUpCityRequired.hide();

				//				divRequiredPickUpState.hide();
				//				divRequiredPickUpCity.hide();
				$('#divRequiredPickUpState').attr('style', 'display:none;')
				$('#divRequiredPickUpCity').attr('style', 'display:none;')
				pickUpDeliveryOptions.empty();
				pickUpDeliveryOptions.html('<option value="">Select</option>');
			}
		}

		if (selectedCountryCode.val().toString() == 'MX') {
			showSpinner();
			$.ajax({
				url: WUCities_URL,
				data: { stateCode: selectedstateCode }, //parameters go here in object literal form 
				type: 'GET',
				datatype: 'json',
				success: function (wuCities) {
					if (handleException(wuCities)) {
						hideSpinner();
						return;
					}
					citiesDropDown = $('select#PickUpCity');
					citiesDropDown.empty();
					var items = '';
					$.each(wuCities, function (i, state) {
						items += '<option value="' + state.Value + '">' + state.Text + '</option>';
					});
					$('select#PickUpCity').html(items);
					//fill Delivery Method DDL
					fillDeliveryMethods();
					hideSpinner();
				},
				error: function () {
					showExceptionPopupMsg(defaultErrorMessage);
					hideSpinner();
				}
			});
		}
	});

    //Regular expression validation expression
	var regexName = /^[a-zA-Z\- ']*$/;
	var regexCity = /^[a-zA-Z ]*$/;
	var regexStateProvince = /^[a-zA-Z0-9-_#:. ]*$/;
	var regexZipCode = /^[a-zA-Z0-9]*$/;
	var regexZipCodeValidation = /^[a-zA-Z0-9]{3,10}/;
	var regexPhone = /^\d*$/;
	var regexPhoneValidation = /^\d{10,15}$/;

	$('#FirstName').rules("add", {
	    required: true,
	    regex: regexName,
	    messages: {
	        required: 'Please enter First Name.',
	        regex: 'Please enter a valid First Name.'
	    }
	});

	$('#FirstName').keypress(function (e) {
	    ValidateKey(e, regexName);
	});

	$('#LastName').rules("add", {
	    required: true,
	    regex: regexName,
	    messages: {
	        required: 'Please enter Last Name.',
	        regex: 'Please enter a valid Last Name.'
	    }
	});

	$('#LastName').keypress(function (e) {
	    ValidateKey(e, regexName);
	});

	$('#SecondLastName').rules("add", {
	    regex: regexName,
	    messages: {
	        regex: 'Please enter a valid Second Last Name.'
	    }
	});

	$('#SecondLastName').keypress(function (e) {
	    ValidateKey(e, regexName);
	});

	$('#City').rules("add", {
	    regex: regexCity,
	    messages: {
	        regex: 'Please enter a valid City.'
	    }
	});

	$('#City').keypress(function (e) {
	    ValidateKey(e, regexCity);
	});

	$('#ZipCode').rules("add", {
	    regex: regexZipCodeValidation,
	    messages: {
	        regex: 'Please enter a valid Zip Code.'
	    }
	});

	$('#ZipCode').keypress(function (e) {
	    ValidateKey(e, regexZipCode);
	});

	$('#Phone').rules("add", {
	    regex: regexPhoneValidation,
	    messages: {
	        regex: 'Please enter a valid Phone Number.'
	    }
	});

	$('#Phone').keypress(function (e) {
	    ValidateKey(e, regexPhone);
	});

	$("#Address").keydown(function (event) {
		var kCode = event.keyCode || event.charCode; // for cross browser check
		if (kCode == 16 || kCode == 192 || (kCode == 49 && event.shiftKey)
            || (kCode == 50 && event.shiftKey) || (kCode == 52 && event.shiftKey)
            || (kCode == 53 && event.shiftKey) || (kCode == 54 && event.shiftKey)
            || (kCode == 55 && event.shiftKey) || (kCode == 56 && event.shiftKey)
            || (kCode == 58) || (kCode == 61)
            || (kCode == 219 && event.shiftKey) || (kCode == 221 && event.shiftKey)
            || (kCode == 221 && event.shiftKey) || (kCode == 188 && event.shiftKey)
            || (kCode == 190 && event.shiftKey) || (kCode == 191 && event.shiftKey)
            || (kCode == 220 && event.shiftKey) || kCode == 222
            || (kCode == 59 && event.shiftKey == false) || (kCode == 187)
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

	$('#PickUpState').rules('add', {
	    required: true,
	    messages: {
	        required: 'Please select Pickup State.'
	    }
	});

	$('#PickUpCity').rules('add', {
	    required: true,
	    messages: {
	        required: 'Please select Pickup City.'
	    }
	});
	function selectPickUpStateAndCity() {

		var selectedPickUpCountry = $('select#PickUpCountry');
		var selectedPickUpState = $('select#PickUpState');
		var selectedPickUpCity = $('select#PickUpCity'); 
		var pickUpStateRequiredStar = $('#pickUpStateRequiredStar');
		var pickUpCityRequiredStar = $('#pickUpCityRequiredStar');

		if (selectedPickUpCountry.length != 0) {
			if (selectedPickUpCountry.val().toString() == 'US' || selectedPickUpCountry.val().toString() == 'MX' || selectedPickUpCountry.val().toString() == 'CA') {

				pickUpStateRequiredStar.show();
				selectedPickUpState.prop("disabled", false);
				if (selectedPickUpCountry.val().toString() != 'MX') {
				    pickUpCityRequiredStar.hide();
					selectedPickUpCity.prop("disabled", true);
					selectedPickUpCity.empty();
					selectedPickUpCity.html('<option value="">Not Applicable</option>');
				}
				else {
				    pickUpCityRequiredStar.show();                    
				    selectedPickUpCity.prop("disabled", false);
				    selectedPickUpCity.show();
				}
			}
			else {

				//Others Country
				pickUpStateRequiredStar.hide();
				pickUpCityRequiredStar.hide();

				selectedPickUpState.prop("disabled", true);
				selectedPickUpState.empty();
				selectedPickUpState.html('<option value="">Not Applicable</option>');

				selectedPickUpCity.prop("disabled", true);
				selectedPickUpCity.empty();
				selectedPickUpCity.html('<option value="">Not Applicable</option>');
			}
		}

	}

	ContactDetailsRequired();
	$('select#DeliveryOptions').change(function () {
		ContactDetailsRequired();
	});
});

function ValidateKey(e, regex) {
    var enterKey = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (regex.test(enterKey)) {
        return true;
    }
    e.preventDefault();
    return false;
}

function ContactDetailsRequired() {
	var deliveryOptionsSelectedText = $('#DeliveryOptions option:selected').text();
	if (deliveryOptionsSelectedText.toLowerCase() == 'phone notification') {
		$('span[name="ContactDetailRequiredStar"]').show();
	} else {
		$('span[name="ContactDetailRequiredStar"]').hide();
	}
}
function resetValidation() {
    //Removes validation from input-fields
    $('.input-validation-error').addClass('field-validation-valid').removeClass('field-validation-valid');
    //Removes validation message after input-fields
    $('.field-validation-error').addClass('field-validation-valid').removeClass('field-validation-error');
    //Removes validation summary 
    $('.validation-summary-errors').addClass('validation-summary-valid').removeClass('validation-summary-errors');
}
