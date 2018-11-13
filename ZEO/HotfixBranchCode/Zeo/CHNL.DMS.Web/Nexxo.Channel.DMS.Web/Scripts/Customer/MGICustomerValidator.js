$(document).ready(function () {

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
    AlterMGIRule(formId);
    if (formId != "frmProfileSummary")
        LoadMandatorySymbol(formId);
});

function AlterMGIRule(formId) {

    switch (formId) {
        case "frmPersonalInfo":
            $('#SSN').rules("remove", "required");
            break;
        case "frmProfileSummary":
            $('#SSN').rules("remove", "required");
            break;
    	case "frmEmploymentDetails":
    		MGIEmployervalidation();
    		break;

    }
}

function MGIEmployervalidation() {

	$("#EmployerName").rules("add", {
		maxlength: 100,
		regex: regexAlphabetsOnly,
		messages: {
			maxlength: '_NexxoTranslate(EmployeeNameMaxlength)',
			regex: '_NexxoTranslate(EmployeeNameRequired)'
		}
	});
	$("#EmployerName").attr("maxlength", 100);

	$('#EmployerName').keypress(function (e) {

		ValidateKey(e, regexAlphabetsOnly);

	});
}


