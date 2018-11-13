$(document).ready(function () {
	SetMandatoryForIdType();

	//$('#ClientID').attr('disabled', true);

	$('#Country').change(function () {
		stateValidation();
		var selectedProvinceId = $(this).val();
		$.ajax({
			url: base_url_GovtIdType,
			data: { CountryId: selectedProvinceId }, //parameters go here in object literal form 
			type: 'GET',
			datatype: 'json',
			success: function (GovtIdTypeIds) {
				if (handleException(GovtIdTypeIds)) return;
				govtidselect = $('#GovtIDType');
				govtidselect.empty();
				var items = ''; //'<option>Select</option>';
				$.each(GovtIdTypeIds, function (i, state) {
					items += '<option value="' + state.Value + '">' + state.Text + '</option>';
					// state.Value cannot contain ' character. We are OK because state.Value = cnt++;
				});
				$('#GovtIDType').html(items);
			},
			error: function () { showExceptionPopupMsg('Error processing JSON Call for Government ID Type'); }
		});
	});
	$('#GovtIDType').change(function () {
		var selectedProvinceId = unescape($(this).val());
		var selectedCountryId = $('#Country').val();

		stateValidation();
		$.ajax({
			url: base_url_GetStates,
			data: { CountryId: selectedCountryId, GovtIdType: selectedProvinceId }, //parameters go here in object literal form 
			type: 'GET',
			datatype: 'json',
			success: function (data) {
				if (handleException(data.states)) return;
				govtidselect = $('#GovtIdIssueState');
				govtidselect.empty();
				var items = ''; //'<option>Select</option>';
				$.each(data.states, function (i, state) {
					if (state.Selected == true) {
						items += "<option value='" + state.Value + "' selected='" + state.Selected + "'>" + state.Text + "</option>";
					} else {
						items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
					}

					// state.Value cannot contain ' character. We are OK because state.Value = cnt++;
				});
				if (data.isNullItemExist)
				    IdIssueStateMandatory(false);
				else
					IdIssueStateMandatory(true);

				$('#GovtIdIssueState').html(items);
				getIdValidations();

				SetMandatoryForIdType();
			},
			error: function () { showExceptionPopupMsg('Error processing JSON Call for Government ID Type'); }
		});
	});
	$('#GovtIdIssueState').change(function () {
		getIdValidations();

	});

	function stateValidation() {
		var govtIdType = unescape($('#GovtIDType').val());
		var countryId = $('#Country').val();
		if ((countryId.toUpperCase() == 'UNITED STATES' && govtIdType.toUpperCase() == "DRIVER'S LICENSE") || (countryId.toUpperCase() == 'UNITED STATES' && govtIdType.toUpperCase() == "U.S. STATE IDENTITY CARD") || (countryId.toUpperCase() == 'MEXICO' && govtIdType.toUpperCase() == 'LICENCIA DE CONDUCIR')) {
			$('#IsStateMandatory').show();
			$('span[data-valmsg-for="GovtIdIssueState"]').show();
		}
		else {
			$('#IsStateMandatory').hide();
			$('span[data-valmsg-for="GovtIdIssueState"]').hide();
			$('select[id="GovtIdIssueState"]').removeClass('input-validation-error');
		}
	}

	$(document).ready(function () {
		getIdValidations();
	});
	function getIdValidations() {
		var selectedCountry = $('#Country').val();
		var selectedIdType = unescape($('#GovtIDType').val());
		var selectedState = $('#GovtIdIssueState').val();

		selectedIdType = selectedIdType.toUpperCase() == 'SELECT' ? '' : selectedIdType;
		selectedState = selectedState.toUpperCase() == 'SELECT' ? '' : selectedState;

		var hasExpirationDate = false;
		var idValidationString;
		$.ajax({
			url: base_url_GetIdRequirements,
			data: { CountryId: selectedCountry, IdType: selectedIdType, State: selectedState }, //parameters go here in object literal form 
			type: 'GET',
			datatype: 'json',
			success: function (idrequirement) {
				if (handleException(idrequirement)) return;
				if (idrequirement != null) {
					hasExpirationDate = idrequirement.HasExpirationDate;
					idValidationString = idrequirement.ValidationString;
				}
				if (hasExpirationDate) {				    
				    if (selectedIdType.toUpperCase() == 'U.S. STATE IDENTITY CARD' && selectedState.toUpperCase() == 'ARIZONA' ||
                        selectedIdType.toUpperCase() == 'U.S. STATE IDENTITY CARD' && selectedState.toUpperCase() == 'TEXAS' ||
                        selectedIdType.toUpperCase() == 'U.S. STATE IDENTITY CARD' && selectedState.toUpperCase() == 'TENNESSEE' ||
                        selectedIdType.toUpperCase() == 'U.S. STATE IDENTITY CARD' && selectedState.toUpperCase() == 'WYOMING')
				    {
				        $('#IdExpireRequired').hide();
				        $('#IDExpireDate').attr('disabled', false);
				    }
				    else {
				        $('#IdExpireRequired').show();				        
				        $('#IDExpireDate').attr('disabled', false);
				        $('span[data-valmsg-for="IDExpireDate"]').show();
				    }
				    
				}
				else {
					$('#IDExpireDate').attr('disabled', true);
					$('#IdExpireRequired').hide();
					$('span[data-valmsg-for="IDExpireDate"]').hide();
				}
			},
			error: function () { showExceptionPopupMsg('Error processing JSON Call for Government ID Type'); }
		});
	}
})

function IdIssueStateMandatory(required) {
	if (required)
		$('#stateRequired').show();
	else
		$('#stateRequired').hide();
}
function SetMandatoryForIdType() {

	var selectedGovtTypeId = $('#GovtIDType').val().toLowerCase();

	if (selectedGovtTypeId == 'driver\'s license' || selectedGovtTypeId == 'new york city id' || selectedGovtTypeId == 'employment authorization card (ead)' || selectedGovtTypeId == 'green card / permanent resident card')
		$('#IdissueRequired').show();
	else
		$('#IdissueRequired').hide();

	if (selectedGovtTypeId == 'licencia de conducir')
		$('#IdExpireRequired').hide();
	else
		$('#IdExpireRequired').show();

	if (selectedGovtTypeId == 'employment authorization card (ead)' || selectedGovtTypeId == 'green card / permanent resident card' || selectedGovtTypeId == 'passport' || selectedGovtTypeId == 'military id' || selectedGovtTypeId == 'matricula consular' || selectedGovtTypeId == 'instituto federal electoral' || selectedGovtTypeId == "")
		$('#stateRequired').hide();
	else
		$('#stateRequired').show();
}
