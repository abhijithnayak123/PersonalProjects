$(document).ready(function () {

	//Making the saved tab as active.
	settabActive();
	
	$("input[type=text], input[type=password], input.SaveCredential").attr('disabled', true);

	$(".EditCredential").click(function (e) {
		var id = this.id.replace("Edit", "");
		var formName = "#form" + id + "Credential";
		$(formName).find("input").attr('disabled', false);
	});


	//AL-436 on keypress validating the key
	if ($("#txtCertegySiteId").length > 0) {
		$("#txtCertegySiteId").keypress(function (e) {
			ValidateKey(e, regexNumbersOnly);
		});
	}

	$(".SaveCredential").click(function (e) {
		var id = this.id.replace("Save", "");
		var formName = "#form" + id + "Credential";
		$(formName).validate();
		if ($(formName).valid())
			showWarningPopup(id);
	});

	if ($("#errorMGIAgentId").length > 0) {
		$("#errorMGIAgentId").keypress(function (e) {
			ValidateKey(e, regexNumbersOnly);
		});
	}
	if ($("#errorPosId").length > 0) {
		$("#errorPosId").keypress(function (e) {
			ValidateKey(e, regexNumbersOnly);
		});
	}

	if ($("#errorMGIAgentId").length > 0) {
		$("#errorMGIAgentId").rules("add", {
			required: true,
			minlength: 8,
			messages: {
				required: MGIAgentIdRequired,
				minlength: MGIAgentIdStringLength
			}
		});
	}

	if ($("#errorPosId").length > 0) {
		$("#errorPosId").rules("add", {
			required: true,
			min: 1,
			max:99,
			messages: {
				required: MGIPosIdRequired,
				min: MGIPosIdMinValue,
			}
		});
	}

//AL-436 Add validation rules for Certegy Site Id.
if ($("#txtCertegySiteId").length > 0) {
	$("#txtCertegySiteId").rules("add", {
		required: true,
		minlength: 16,
		messages: {
			required: CertergySiteIdRequired,
			minlength: CertegySiteIdStringLength
		}
	});
}
});


function settabActive()
{
	var index = localStorage.getItem("activeProcessorName");
	if (index != null) {
		$('#tabs').tabs({ active: index });
	}
	else {
		$("#tabs").tabs();
	}
}

function showWarningPopup(processorName) {
	var $confirm = $("<div id='divWarning'></div>");
	$confirm.empty();
	$confirm.dialog({
		autoOpen: false,
		title: 'Message',
		width: 400,
		draggable: false,
		resizable: false,
		closeOnEscape: false,
		dialogClass: 'no-close',
		modal: true,
		height: 150,
		cache: false,
		open: function (event, ui) {
			$confirm.load(WarningURL + '?Name=' + processorName);
		}
	});

	$confirm.dialog("open");
	return false;
}

function postCredentials() {
	var name = $('#hdnName')[0].value;

	var processorname = $("#tabs").tabs("option", "active");

	localStorage.setItem("activeProcessorName", processorname);

	var formName = "#form" + name + "Credential";
	$(formName).submit();
}
