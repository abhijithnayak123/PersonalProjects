$(document).ready(function () {
	if ($('#CountryOfBirth > option').length > 0 && $('#CountryOfBirth').val() == "")
		$("#CountryOfBirth").val("US");
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

    if (formId != "frmProfileSummary")
        LoadMandatorySymbol(formId);
});
