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
    AlterRedstoneRule(formId);
    if (formId != "frmProfileSummary")
        LoadMandatorySymbol(formId);
});



function AlterRedstoneRule(formId) {

    switch (formId) {
        case "frmPersonalInfo":
            $('#SSN').rules("remove", "required");
            $('input[name="Gender"]').rules("remove", "required");
           // $("#SSN").focus();
            break;
        case "frmIdentificationInformation":
            $("#MotherMaidenName").rules("remove", "required");

            break;
        case "frmProfileSummary":
            $('#SSN').rules("remove", "required");
            $("#Gender").rules("remove", "required");
            $("#MotherMaidenName").rules("remove", "required");
            break;
    }
}
