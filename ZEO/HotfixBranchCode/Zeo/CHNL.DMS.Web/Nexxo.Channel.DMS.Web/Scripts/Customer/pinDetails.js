$(document).ready(function () {
	$('#Pin').attr('maxLength', '4');
	$('#ReEnter').attr('maxLength', '4');

	$("#PinNextSubmit").click(function (event) {
		var channelPartnerName = $('#ChannelPartnerName').val().toUpperCase();
		if (channelPartnerName != "CARVER") {
			var pinLength = $("#Pin").val().replace(/ /g, '').length;
			var reEnterPinLength = $("#ReEnter").val().replace(/ /g, '').length;

			if (pinLength == 0) {
				pinValidation($('#divErrorPinNumber'), pinLength)
				$('#Pin').focus();
				event.preventDefault();
				return false;
			}

			if (reEnterPinLength == 0) {
				pinValidation($('#divErrorReEnterPin'), reEnterPinLength)
				event.preventDefault();
				return false;
			}
		}
	});

	$('#Pin').blur(function (e) {
		var pinLength = $("#Pin").val().replace(/ /g, '').length;
		pinValidation($('#divErrorPinNumber'), pinLength)
	});

	$('#ReEnter').blur(function (e) {
		var pinLength = $("#ReEnter").val().replace(/ /g, '').length;
		pinValidation($('#divErrorReEnterPin'), pinLength)
	});

	function pinValidation(pinErrorMessageControl, length) {
		if (length == 0 && pinErrorMessageControl.prev('div').find('span').first('span').text() == "") {
			pinErrorMessageControl.show();
		}
		else {
			pinErrorMessageControl.hide();
		}
	}
});
