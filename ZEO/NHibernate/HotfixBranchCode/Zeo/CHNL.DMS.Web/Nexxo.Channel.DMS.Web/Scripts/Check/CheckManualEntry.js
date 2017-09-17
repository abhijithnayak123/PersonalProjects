$(function () {
	$('#RoutingNumber').focus();
	var regexNumbersOnly = /^([0-9]*)$/;
	$("#RoutingNumber").rules("add", {
		required: true,
		maxlength: 9,
		regex: regexNumbersOnly,
		messages: {
			required: RoutingNumberRequired,
			regex: RoutingNumberRegx,
			maxlength: RoutingNumberMaxlength
		}
	});

	$("#ConfirmRoutingNumber").rules("add", {
		regex: regexNumbersOnly,
		maxlength: 9,
		equalTo: "#RoutingNumber",
		messages: {
			regex: RoutingNumberRegx,
			equalTo: RoutingNumberCompare,
			maxlength: RoutingNumberMaxlength
		}
	});
	$("#AccountNumber").rules("add", {
		required: true,
		maxlength: 32,
		regex: regexNumbersOnly,
		messages: {
			required: AccountNumberRequired,
			regex: AccountNumberRegx,
			maxlength: AccountNumberMaxlength
		}
	});

	$("#ConfirmAccountNumber").rules("add", {
		regex: regexNumbersOnly,
		maxlength: 32,
		equalTo: "#AccountNumber",
		messages: {
			regex: AccountNumberRegx,
			equalTo: AccountNumberCompare,
			maxlength: AccountNumberMaxlength
		}
	});
	$("#CheckNumber").rules("add", {
		required: true,
		maxlength: 15,
		regex: regexNumbersOnly,
		messages: {
			required: CheckNumberRequired,
			regex: CheckNumberRegex,
			maxlength: CheckNumberMaxlength
		}
	});

	$('#RoutingNumber').blur(function () {
		var rountingnumber = this.value;
		if (rountingnumber && rountingnumber.length == 9) {
			$('#RoutingNumber').prop('type', 'password');
		}
		if (rountingnumber == '') {
			$('#ConfirmRoutingNumber').val('');
		}
		$('#ConfirmRoutingNumber').focus();
	});

	$('#RoutingNumber').keydown(function () {
		if ($('#RoutingNumber').val() == "") {
			$('#RoutingNumber').val("");
			$('#RoutingNumber').prop('type', 'text');
		}
	});

	$('#ConfirmRoutingNumber').blur(function () {
		var confirmRoutingNumber = this.value;
		var routingNumber = $('#RoutingNumber').val();
		if (confirmRoutingNumber != routingNumber && confirmRoutingNumber) {
			$('#RoutingNumber').val('');
			$('#ConfirmRoutingNumber').val('');
			$('#RoutingNumber').focus();
		}
	});

	$('#AccountNumber').blur(function () {
		var accountnumber = this.value;
		if (accountnumber && accountnumber.length >= 12 || accountnumber.length <= 32) {
			$('#AccountNumber').prop('type', 'password');
		}
	});

	$('#AccountNumber').keydown(function () {
		if ($('#AccountNumber').val() == "") {
			$('#AccountNumber').val('');
			$('#AccountNumber').prop('type', 'text');
		}
	});

	$('#ConfirmAccountNumber').blur(function () {
		var confirmAccountNumber = this.value;
		var AccountNumber = $('#AccountNumber').val();
		if (confirmAccountNumber != AccountNumber) {
			$('#AccountNumber').val('');
			$('#ConfirmAccountNumber').val('');
			$('#AccountNumber').focus();
		}
	});
});