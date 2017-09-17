var clickedOnDelFavButton = false;
$(document).ready(function () {
    frequentBanner();

	$('select#PickupCountry').change(function () {
		populateState();
	});

	if ($('#FrequentReceivers_SelectedReceiverId').val() > 0) {
		$('#addreceiver').css('display', 'none');
		$('#editreceiver').css('display', 'block');
	}
	
	if ($('#DestinationAmount').val()) {
	    $('#Amount').prop("disabled", true);
	}

	if ($('#Amount').val()) {
	    var amountArray = $('#Amount').val().split(' ');
	    if (amountArray.length == 3 && !isNaN(amountArray[1])) {
	        $('#hdnAmount').val(amountArray[1]);
	    }

	    $('#DestinationAmount').prop("disabled", true);
	}

	$('#DestinationAmount').blur(function () {
	    var destinationControl = $(this);
	    var destinationAmount = $.trim(destinationControl.val());
	    if (destinationAmount) {
	        if (isNaN(destinationAmount)) {
	            destinationControl.val('').focus();
	            return;
	        }
	        $('#Amount').prop("disabled", true);
	        $('#continueButton').prop("disabled", false);
	        $('#CouponPromoCode').focus();
	    } else {
	        $('#continueButton').prop("disabled", true);	       
	        $('#Amount').prop("disabled", false);
	    }
	});

	$("#continueButton").click(function (e) {
		validateAmount(e);
	});

	$('#Amount').focus(function () {
		$('#dvAmountError').text("").removeClass("field-validation-error");
	});

	$('#DestinationAmount').focus(function () {
		$('#dvDestAmountError').text("").removeClass("field-validation-error");
	});

	$('#Amount').blur(function () {
		var amount = $.trim($(this).val());
	    if (amount) {
			if (isNaN(amount)) {
				$(this).val('');
				$('#hdnAmount').val('');
				$('#Amount').focus();
				return false;
			}
			$('#hdnAmount').val(amount);
			$(this).val('$ ' + amount + ' USD');
			$('#DestinationAmount').prop("disabled", true);
			$('#continueButton').prop("disabled", false);
			$('#CouponPromoCode').focus();
	    } else {
			$('#continueButton').prop("disabled", true);
			$('#DestinationAmount').prop("disabled", false);
			$('#hdnAmount').val('');
		}
	});

	$('#Amount').focus(function () {
	    var amount = $('#hdnAmount').val();
	    $(this).val(amount);	
	});	

	$('#LastName').autocomplete({
		source: receiverAutoComplete_URL,
		focus: function () {
			// prevent value inserted on focus
			return false;
		},
		select: function (event, ui) {
			$('#LastName').val(ui.item.label);
			var fullName = ui.item.label;
			var array = fullName.split(/[\s,]+/);
			$('#LastName').val(array[1]);
			$('#hdnReceiverFullName').val(fullName);
			// remove the current input
			array.pop();
			// add the selected item
			array.push(ui.item.value);
			// add placeholder to get the comma-and-space at the end
			array.push("");
			return false;
		}
	});

	$('#LastName').blur(function () {
		if ($(this).val() !== '') {
			showSpinner();
			$.ajax({
				type: "GET",
				url: receiverSearchByFullName_URL + '?fullName=' + $('#hdnReceiverFullName').val(),
				dataType: "json",
				contentType: "application/json; charset=UTF-8",
				data: {},
				success: function (receiver) {
					if (receiver && receiver !== 0) {
						$('#divheader')[0].innerHTML = 'Send Money to Receiver';
						populateReceiverDetails(receiver);
						$('#addreceiver').css('display', 'none');
						$('#editreceiver').css('display', 'block');
						hideSpinner();
					}
				},
				error: function (data) {
					hideSpinner();
				},
				complete: function () {
					hideSpinner();
				}
			});
		}
	});

	$("#addreceiver").click(function (e) {
		e.preventDefault();
		submitForm();
	});
	if (!$('#Amount').val()  && !$('#DestinationAmount').val())  {
		$('#continueButton').attr("disabled", "disabled");
	}
});

function populateState(selectedState) {
	showSpinner();
	var selectedCountryCode = $("#PickupCountry").val();
	$.ajax({
		url: MoneyTransfer_States_URL,
		data: { countryCode: selectedCountryCode },
		type: 'GET',
		datatype: 'json',
		success: function (states) {
			statesDropDown = $('select#PickupState');
			bindDropdownList(statesDropDown, states);
			$('select#PickupState').val(selectedState);
			hideSpinner();
		},
		error: function () {
		    showExceptionPopupMsg('Error processing JSON Call for retreiving states');
			hideSpinner();
		}
	});
}

function selectReceiver(receiverId) {
    if (clickedOnDelFavButton == false) {
        if (receiverId) {
            $('#ReceiverID').val(receiverId);
            $('#divheader')[0].innerHTML = 'Send Money to Receiver';
            populateReceiverDetails(receiverId);
        }
    }
    else {
        clickedOnDelFavButton = false;
    }
}

function btnFavReceiverDeleteClick(control) {
    var receiverId = control.id.split("_")[1];
    var $confirmation = $("<div id='divDeletefavReceiverDialog'></div>");
    $confirmation.empty();
    $confirmation.dialog({
        autoOpen: false,
        title: "Zeo",
        width: 480,
        draggable: false,
        modal: true,
        resizable: false,
        closeOnEscape: false,
        minHeight: 125,
        scroll: false,
        cache: false,
        open: function (event, ui) {
            $confirmation.load(DelFavReceiverMsgPopup_URL, { id: receiverId }, function () {
                $('#btnYes').focus();
            });

        }
    });
    $confirmation.dialog("open")
    clickedOnDelFavButton = true;
}

function DeleteFavReceiver(receiverId) {
    $('#divDeletefavReceiverDialog').dialog('destroy').remove();
    showSpinner();
    $.ajax({
        type: "POST",
        url: DeleteFavoriteReceiver_URL,
        contentType: "application/json; charset=UTF-8",
        data: "{receiverId: '" + receiverId + "'}",
        processData: true,
        success: function (data) {
            if (data.data) {
                $('#frequentReceiver').html();
                showExceptionPopupMsg(data.data);
            }
            else {
                $('#frequentReceiver').html(data);
            }
            frequentReceiverGallery();
            hideSpinner();
        },
        error: function (data) {
            if (data != '') {
                $('#frequentReceiver').html();
                showExceptionPopupMsg(data.data);
            }
            frequentReceiverGallery();
            hideSpinner();
        }
    });
}

function clearAmount() {
    $('#Amount').val('').removeAttr("disabled");
    $('#DestinationAmount').val('').removeAttr("disabled");
    $('#CouponPromoCode').val('');
    $('#hdnDestinationAmount').val('');
	$('#hdnAmount').val('');
}

function populateReceiverDetails(receiverId) {
	clearAmount();
	showSpinner();

	$.ajax({
		type: "GET",
		url: populateReceiverDetailsURL + '?receiverId=' + receiverId,
		dataType: "json",
		contentType: "application/json; charset=UTF-8",
		data: {},
		success: function (data) {
			var fqRLogo = imgFreqRecURL;
			var fqRLogo_selected = imgSelectedFreqRecURL;
			$('a[id^=fqRcvr]').css("background-color", "#006937");
			$('a[id^=fqRcvr]').css("color", "#ffffff");
			$('a[id^=fqRcvr]').find('#fqRcrLogo').attr("src", imgFreqRecURL);

			var selectedReceiverId = '#fqRcvr_';

			$('#FirstName').val(data.FirstName);
			$('#MiddleName').val(data.MiddleName);
			$('#LastName').val(data.LastName);
			$('#SecondLastName').val(data.SecondLastName);
			$('#PickupCountry').val(data.PickupCountry);
			$('#PickupState').val(data.PickupState);
			$('#editReceiver').css("background-color", "##808080");
			$('#editReceiver').attr("disabled", true);
			$('#FrequentReceivers_SelectedReceiverId').val(receiverId);
			$('#Amount').focus();
			populateState(data.PickupState);
			selectedReceiverId += receiverId;

			if ($(selectedReceiverId).length) {
				// Highlight the selected receiver by changing the styling/image
				$(selectedReceiverId).css("background-color", "#fdbe57");
				$(selectedReceiverId).css("color", "#000000");
				$(selectedReceiverId).css("font-size", "12px");
				$(selectedReceiverId).find("#fqRcrLogo").attr("src", fqRLogo_selected);

				// if the selected receiver is outside the visible area, move the 
				// trolley to the left or right to bring the display image into view

				// Declare variables to hold the values 
				var totalImages = jQuery("#gallery > li").length,
					imageWidth = jQuery("#gallery > li:first").outerWidth(true),
					totalWidth = imageWidth * totalImages,
					visibleImages = Math.round(jQuery("#gallery-wrap").width() / imageWidth),
					visibleWidth = visibleImages * imageWidth,
					stopPosition = (visibleWidth - totalWidth);

				// if the selected receiver( from the auto-complete) is present in the list of favorite receivers
				if (jQuery(selectedReceiverId).position().left != undefined && jQuery("#gallery") != undefined) {
					// if the the selected receiver is the left-most & beyond the visible area, then move the trolley to the right.
					if (jQuery("#gallery").position().left < 0 && jQuery(selectedReceiverId).position().left == 0 && !jQuery("#gallery").is(":animated")) {
						jQuery("#gallery").animate({ left: "+=" + imageWidth + "px" });
					}

					// if the selected receiver is the right-most & beyond the visible area, then move the trolley to the left.
					if (jQuery("#gallery").position().left > stopPosition && jQuery(selectedReceiverId).position().left == visibleWidth && !jQuery("#gallery").is(":animated")) {
						jQuery("#gallery").animate({ left: "-=" + imageWidth + "px" });
					}
				}
			}
			$('#editreceiver').prop('href', receivereEdit_URL + receiverId);
			hideSpinner();
		},
		error: function (data) {
			if (data.status == "404")
			    showExceptionPopupMsg("Could not find receiver " + selectedReceiver);
			hideSpinner();
		},
		complete: function () {
			$('#addreceiver').css('display', 'none');
			$('#editreceiver').css('display', 'block');
		}
	});
}

function submitForm() {
	var firstName = $('#FirstName').val();
	var lastName = $('#LastName').val();	
	var country = $('#PickupCountry').val();
	if (firstName === '' && lastName === '' && country === '') {
		$('#FirstName').rules('remove');
		$('#LastName').rules('remove');
		$('#PickupCountry').rules('remove');
	}
	$('#sendMoneyForm').submit();
}

function validateAmount(e) {
	var amount = $('#hdnAmount').val();
	var destinationAmount = $('#DestinationAmount').val();

    $('#dvAmountError').text("").removeClass("field-validation-error");
	//$('#dvDestinationAmountError').text("").removeClass("field-validation-error");

    if (amount > 99999.99) {
    	$('#sendMoneyForm').validate();
    	$('#dvAmountError').append("You can only send money up to 100000").addClass("field-validation-error");
    	e.preventDefault();
    	return;
    }

    if (destinationAmount > 999999999.99) {
    	$('#sendMoneyDetailForm').validate();
    	$('#dvDestAmountError').append("You can only send money up to 1000000000").addClass("field-validation-error");
    	e.preventDefault();
    }
    else
    {
    	$('#dvAmountError').hide();
    	$('#sendMoneyForm').submit();
	}
}

function frequentReceiverGallery() {
    if (jQuery("#gallery").length) {
        var totalImages = jQuery("#gallery > li").length,
        imageWidth = jQuery("#gallery > li:first").outerWidth(true),
        totalWidth = imageWidth * totalImages,
        visibleImages = Math.round(jQuery("#gallery-wrap").width() / imageWidth),
        visibleWidth = visibleImages * imageWidth,
        stopPosition = (visibleWidth - totalWidth);
        jQuery("#gallery").width(totalWidth);

        jQuery("#gallery-prev").click(function () {
            if (jQuery("#gallery").position().left < 0 && !jQuery("#gallery").is(":animated")) {
                jQuery("#gallery").animate({ left: "+=" + 3 * imageWidth + "px" });
            }
            return false;
        });

        jQuery("#gallery-next").click(function () {
            if (jQuery("#gallery").position().left > stopPosition && !jQuery("#gallery").is(":animated")) {
                jQuery("#gallery").animate({ left: "-=" + 3 * imageWidth + "px" });
            }
            return false;
        });
    }
}
