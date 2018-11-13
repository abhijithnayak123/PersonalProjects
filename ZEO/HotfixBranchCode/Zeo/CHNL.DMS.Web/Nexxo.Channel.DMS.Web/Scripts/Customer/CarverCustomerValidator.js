
$(document).ready(function () {
	if ($('#CountryOfBirth > option').length > 0 && $('#CountryOfBirth').val() == "")
		$("#CountryOfBirth").val("");
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
	AlterCarverRule(formId);
	$('#GovtIDType').change(function () {
		if ($('#GovtIDType').val() != "") {
			$('#GovernmentId').rules('add', {
				required: true,
				messages: {
					required: '_NexxoTranslate(IdentificationIDNumberRequired)'
				}
			});
			$("[for=GovernmentId]").empty();
			$("[for=GovernmentId]").html('ID Number');
			$("[for=GovernmentId]").append('<span class="mandatory" id="GovtIDRequired" style="display: inline;">*</span>');
			$('#GovernmentId').removeAttr('disabled');
			$('#IDIssuedDate').removeAttr('disabled', '');
		}
		else {
			$('#GovtIDType').rules("remove", "required");
			$('#GovtIdIssueState').rules("remove", "required");
			$('#GovernmentId').rules("remove", "required");
			$('#IDIssuedDate').rules("remove", "required");
			$('#GovernmentId').rules("remove", "required");
			$('#stateRequired').hide();
			$("[for=GovernmentId]").empty();
			$("[for=GovernmentId]").html('ID Number')
			$('#GovernmentId').val('');
			$('#IDIssuedDate').val('');
			$('#IDExpireDate').val('');

		}

	});
	$('#Country').change(function () {
		if ($('#Country').val() == "") {
			$('#GovtIDType').rules("remove", "required");
			$('#GovtIdIssueState').rules("remove", "required");
			$('#GovernmentId').rules("remove", "required");
			$('#IDIssuedDate').rules("remove", "required");
			$('#GovernmentId').rules("remove", "required");
			$("[for=GovernmentId]").empty();
			$("[for=GovernmentId]").html('ID Number');
			$('#stateRequired').hide();
			$("#GovtIdIssueState").val($("#GovtIdIssueState option:first").val());
			$('#IdissueRequired').hide();
			$('#IdExpireRequired').hide();
			$("[for=GovtIDType]").empty();
			$("[for=GovtIDType]").html('Government ID Type');
			$('#GovernmentId').val('');
			$('#IDIssuedDate').val('');
			$('#IDExpireDate').val('');
			$('#GovernmentId').attr('disabled', 'disabled');
			$('#IDIssuedDate').attr('disabled', 'disabled');
			$('#IDExpireDate').attr('disabled', 'disabled');
		}

	});
	if (formId != "frmProfileSummary")
		LoadMandatorySymbol(formId);

});

function AlterCarverRule(formId) {

	switch (formId) {
		case "frmIdentificationInformation":
			$('#MotherMaidenName').rules("remove", "required");
			$('#CountryOfBirth').rules("remove", "required");
			$('#Country').rules("remove", "required");
			if ($('#GovtIDType').val() == "") {
				$('#GovtIDType').rules("remove", "required");
				$('#GovtIdIssueState').rules("remove", "required");
				$('#GovernmentId').rules("remove", "required");
				$('#IDIssuedDate').rules("remove", "required");
				$('#stateRequired').hide();
				$('#IdissueRequired').hide();
				$('#IdExpireRequired').hide();
				$('#GovernmentId').attr('disabled', 'disabled');
				$('#IDIssuedDate').attr('disabled', 'disabled');
			}
			$('#DateOfBirth').rules("remove", "required");
			break;
		case "frmProfileSummary":
			$('#MotherMaidenName').rules("remove", "required");
			$('#DateOfBirth').rules("remove", "required");
			$('#Country').rules("remove", "required");
			$('#CountryOfBirth').rules("remove", "required");
			$('#GovtIDType').rules("remove", "required");
			$('#GovernmentId').rules("remove", "required");
			$("#Gender").rules("remove", "required");
			break;
		case "frmPersonalInfo":
			$('#SSN').rules("remove", "required");
			$('input[name="Gender"]').rules("remove", "required");
			break;
		case "frmEmploymentDetails":
			CarverEmployervalidation();
			break;
		case "frmPinDetails":
			$('#Pin').rules("remove", "required");
			$('#ReEnter').rules("remove", "required");
			break;

	}
}
function CarverEmployervalidation() {

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
