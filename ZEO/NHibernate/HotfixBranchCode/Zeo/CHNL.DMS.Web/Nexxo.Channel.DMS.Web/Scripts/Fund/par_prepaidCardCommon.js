$(document).ready(function () {
	if (hasGprCard) {
		$("#CardNumber").attr("readonly", "readonly");
	}

	$('#Name').keydown(function (event) {
		if (event.keyCode == 8) {
			event.preventDefault();
			return;
		}
	});

	$("#CardNumber").keydown(function (event) {
		var cardNumberCount = $("#CardNumber").val().replace(/ /g, '').length;
		Validate(event);
		if(cardNumberCount == 16 ) {
			// Allow: backspace, delete, tab, left arrow and right arrow
			if(event.keyCode != 8 && event.keyCode != 9 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 46)
				return false;
		}		
	});

    $("#AccountNumber").keypress(function (e) {
        var enterKey = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        var regexNumbers = /^[\d]*$/;
        if (regexNumbers.test(enterKey)) {
            return true;
        }
        e.preventDefault();
        return false;
	});

	function Validate(event){
		if (!hasGprCard) {
			// Allow: backspace, delete, tab, escape, and enter
			if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
			// Allow: Ctrl+A
			((event.keyCode == 65 || event.keyCode == 86) && event.ctrlKey === true) ||
			// Allow: home, end, left, right
			(event.keyCode >= 35 && event.keyCode <= 39)) {
				// let it happen, don't do anything
				return;
			} else {
				// Ensure that it is a number and stop the keypress
				if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
					event.preventDefault();
				}
			}
		} else {
			if (event.keyCode == 8) {
				event.preventDefault();
				return;
			}
		}	
	}

	$('#CardNumber').focus(function () {
		if (!hasGprCard) {
			var cardValue = $('#CardNumber').val().replace(/ /g, '');
			$('#CardNumber').val(cardValue);
		}
	});

	//Card Number text box field tabs out or focus out event handling
	$('#CardNumber').blur(function (e) {
		if (!hasGprCard) {
			var cardNumber = $('#CardNumber').val();
			//Remove the spaces between the input characters & get the length
			var cardNumberCount = $("#CardNumber").val().replace(/ /g, '').length;
				
            //Check for the input length
			if (cardNumberCount == 16) {
				var formattedCardNumber = "";
				//Format them into the mask format "xxxx xxxx xxxx xxxx"
				cardNumber = $("#CardNumber").val().replace(/ /g, '');
				for (var i = 4; i <= 16; i += 4) {
					for (var j = i - 4; j < i; j++) {
						formattedCardNumber = formattedCardNumber + cardNumber.charAt(j);
					}
					formattedCardNumber = formattedCardNumber + " ";
				}
				$('#divErrorCardNumber').hide();
				formattedCardNumber = $.trim(formattedCardNumber);
				$('#CardNumber').val(formattedCardNumber);
			}				
		}
	});

});
