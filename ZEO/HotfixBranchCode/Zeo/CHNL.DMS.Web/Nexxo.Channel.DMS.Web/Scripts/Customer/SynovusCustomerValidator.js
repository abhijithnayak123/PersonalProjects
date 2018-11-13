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
	AlterSynovusRule(formId);
	if (formId != "frmProfileSummary")
		LoadMandatorySymbol(formId);
});

function AlterSynovusRule(formId) {

	switch (formId) {
		case "frmPersonalInfo":
			$('input[name="Gender"]').rules("remove", "required");
			break;
		case "frmProfileSummary":
			$("#Gender").rules("remove", "required");
			break;
		case "frmEmploymentDetails":
			SynovusEmployervalidation();
			break;

	}
}
function SynovusEmployervalidation() {

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

