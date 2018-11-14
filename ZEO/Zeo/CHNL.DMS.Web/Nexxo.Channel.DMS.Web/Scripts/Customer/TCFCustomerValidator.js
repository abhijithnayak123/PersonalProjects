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

	$("#summarySubmit").click(function () {
	    //checking the SSN and throwing the error pop up to enter the SSN when United States is selected for Primary/Secondary Citizenship and/or Country of Birth.
	    var ssn = $("#SSN").val();
	    var countryOfBirth = $("#CountryOfBirth").val();
	    var secondaryCountryCitizenShip = $("#SecondaryCountryCitizenShip").val();
	    var primaryCountryCitizenShip = $("#PrimaryCountryCitizenShip").val();

	    if ((ssn == "" || ssn == null || ssn == undefined) && (countryOfBirth.toLowerCase() == "united states" || secondaryCountryCitizenShip.toLowerCase() == "united states" || primaryCountryCitizenShip.toLowerCase() == "united states")) {
	        //showing error pop
	        //showExceptionPopupMsg("Please enter SSN/ITIN to proceed further.");
	        //getMessage(CustomerExceptions.SSN_Required_Exception, null);

	        var msg = getExceptionMessage(CustomerExceptions.SSN_Required_Exception);
	        showSSNRequiredPopup(SSNExceptionMsgPopupURL, 'SSN Required', 505, 220, msg, 'Continue');
	    }
	    else {
	        $("#frmProfileSummary").submit();
	    }
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

