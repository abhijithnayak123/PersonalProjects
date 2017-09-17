$(document).ready(function () {
	if ($('#CountryOfBirth > option').length > 0 && $('#CountryOfBirth').val() == "")
		$("#CountryOfBirth").val("US");
	$("#divUserSessionId").css("display", "");
	var formId = $('form').attr('id');

	switch (formId) {
		case "frmPersonalInfo":
			PersonalInformationValidator();
			break;
		case "frmIdentificationInformation":
			IdentificationInformationValidator();
			break;
		case "frmEmploymentDetails":
			EmployeeDetailsValidator();
			break;
		case "frmPinDetails":
			PinDetailsValidator();
			break;
		case "frmProfileSummary":
			ProfileSummaryValidator();
			break;

	}
	AlterTCFRule(formId);
	if (formId != "frmProfileSummary")
		LoadMandatorySymbol(formId);


	$("select#Profession").change(function () {
		checkOccupation();
	});

});

function addProfessionRules() {
	$("#Profession").rules("add", {
		required: true,
		messages: {
			required: '_NexxoTranslate(ProfessionRequired)',
		}
	});

	$('#EmployerName').keypress(function (e) {
		ValidateKey(e, regexAlphabetsAndNumericOnly);

	});

	$("#EmployerName").rules("add", {
		maxlength: 40,
		regex: regexAlphabetsAndNumericOnly,
		messages: {
			maxlength: '_NexxoTranslate(EmployeeNameMaxlength)',
			regex: '_NexxoTranslate(EmployeeNameRequired)'
		}
	});
	$("#EmployerName").attr("maxlength", 40);
}

function addLegalCodeRules() {
	$("#LegalCode").rules("add", {
		required: true,
		messages: {
			required: '_NexxoTranslate(LegalCodeRequired)',
		}
	});
}

function addPrimaryCountryCitizenShipRules() {
	$("#PrimaryCountryCitizenShip").rules("add", {
		required: true,
		messages: {
			required: '_NexxoTranslate(PrimaryCountryCitizenShipRequired)',
		}
	});
}
function removeMotherMaidenRules() {
	$("#MotherMaidenName").rules("remove", "required");
}

function addDescriptionRules() {

	$("#OccupationDescription").rules("add", {
		required: true,
		regex: regexDescription,
		messages: {
			required: '_NexxoTranslate(OccupationDescriptionRequired)',
			regex: '_NexxoTranslate(EmploymentOccupationDescRegularExpression)'
		}
	});

}

function checkOccupation() {
	var occupationName = $("#Profession option:selected").text();

	if (occupationName == 'OTHER' || occupationName == 'UNEMPLOYED' || occupationName == 'RETIRED') {
		$(".description").show();
	}
	else {
		//TODO : change this code. Added because IE has some issues while clearing the textbox before hide.
		setTimeout(function () {
			$("#OccupationDescription").val('');
		}, 500);

		$(".description").hide();
	}

	addDescriptionRules();
}

function AlterTCFRule(formId) {

	switch (formId) {
		case "frmIdentificationInformation":
			addLegalCodeRules();
			addPrimaryCountryCitizenShipRules();
			removeMotherMaidenRules();
			break;
		case "frmPersonalInfo":
			$('#SSN').rules("remove", "required");
			break;
		case "frmEmploymentDetails":
			addProfessionRules();
			checkOccupation();
			break;
		case "frmProfileSummary":
			removeMotherMaidenRules();
			break;
	}
}

