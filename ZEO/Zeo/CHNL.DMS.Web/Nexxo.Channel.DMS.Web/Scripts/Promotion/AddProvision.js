$(document).ready(function () {

    $("#MaxAmount").rules("add", {
        greaterthanmin: { dependingproperty: "MinAmount", validateminvalue: "false" },
        messages: {           
            greaterthanmin: '_NexxoTranslate(MaxAmountShouldBeGreaterThanMin)'
        }
    });
    $("#MinAmount").rules("add", {
        greaterthanmin: { dependingproperty: "MaxAmount", validateminvalue: "true" },
        messages: {
            greaterthanmin: '_NexxoTranslate(MaxAmountShouldBeLessThanMin)'
        }
    });

    if (disable_Check_Type.toLowerCase() === 'true') {
        $("#CheckType").attr('disabled', 'disabled');
    }

    DisableOrEnableTrainStop();

    PopulateLocationsMutliSelect();

    AppendDollor('MinAmount', 'MinAmountWithCurrency', '$ ', ' USD');
    AppendDollor('MaxAmount', 'MaxAmountWithCurrency', '$ ', ' USD');
    checkDiscutValue();

    provisionGrid();

    $("#MinAmount").on('blur', function () {
        AppendDollor('MinAmount', 'MinAmountWithCurrency', '$ ', ' USD');
    });

    $("#MaxAmount").on('blur', function () {
        AppendDollor('MaxAmount', 'MaxAmountWithCurrency', '$ ', ' USD');
    });

    $("#MinAmountWithCurrency").focus(function () {
        FocusAppendTextBox('MinAmount', 'MinAmountWithCurrency');
    });

    $("#MaxAmountWithCurrency").focus(function () {
        FocusAppendTextBox('MaxAmount', 'MaxAmountWithCurrency');
    });

    $("#MinAmount").keypress(function (e) {
        ValidateText(e);
    });

    $("#MaxAmount").keypress(function (e) {
        ValidateText(e);
    });

    $("#Value").keypress(function (e) {
        ValidateText(e);
    });

    $("#DiscountValueWithSymbol").focus(function () {
        FocusAppendTextBox('Value', 'DiscountValueWithSymbol');
    });

    $("#Value").on('blur', function () {
        checkDiscutValue();
    });

    $("#discountType").change(function () {
        checkDiscutValue();
    });

    GetSelectedCheckTypes();

    GetSelectedLocations();

    GetSelectedGroups();

    $('.promostatelocations').multiselect({
        placeholder: 'Select',
        search: true,
        searchOptions: {
            'default': 'Search'
        },
        minHeight: 170,
        maxHeight: 200,
        maxWidth: 280,
        minWidth: 168,
        selectAll: true,
        showOptGroups: true,
        selectGroup: false,
        onControlClose: function (element) {
            GetSelectedLocations();
        },
        onSelectAll: function (element) {
            GetSelectedLocations();
        }
    });

    $('.promochecktypes').multiselect({
        placeholder: 'Select',
        search: true,
        searchOptions: {
            'default': 'Search'
        },
        selectAll: true,
        minHeight: 170,
        maxHeight: 170,
        maxWidth: 170,
        minWidth: 168,
        onControlClose: function (element) {
            GetSelectedCheckTypes();
        },
        onSelectAll: function (element) {
            GetSelectedCheckTypes();
        }
    });


    $('.promoGroupNames').multiselect({
        placeholder: 'Select',
        search: true,
        searchOptions: {
            'default': 'Search'
        },
        selectAll: true,
        minHeight: 170,
        maxHeight: 170,
        maxWidth: 170,
        minWidth: 168,
        onControlClose: function (element) {
            GetSelectedGroups();
        },
        onSelectAll: function (element) {
            GetSelectedGroups();
        }
    });
    
    $('.promochecktypes').multiselect('loadOptions', selectedCheckTypes);

    $('.promolocations').multiselect('loadOptions', selectedLocations);

    $('.promoGroupNames').multiselect('loadOptions', selectedGroups);

    $("#provisiondelete").live('click', function (e) {
        var dataHref = this.getAttribute('data-href');
        ShowPopUp(dataHref, "SYSTEM MESSAGE", 400, 125);
    });
});

function checkDiscutValue() {
    var disType = $("#discountType").val();
    if (disType == "1") {
        var value = parseInt($("#Value").val());
        if (value > 100)
            $("#Value").val(100);
        AppendDollor('Value', 'DiscountValueWithSymbol', '', ' %');
    }
    else if (disType != "") {
        AppendDollor('Value', 'DiscountValueWithSymbol', '$ ', ' USD');
    }
    else {
        AppendDollor('Value', 'DiscountValueWithSymbol', '', '');
    }
}

function DeleteProvision(id) {
    showSpinner();
    $.ajax({
        type: "GET",
        url: ProvisionDelete_Url + '?provisionId=' + id,
        dataType: "json",
        contentType: "application/json; charset=UTF-8",
        data: {},
        success: function (data) {
            if (data.success) {
                RemovePopUp();
                DisableOrEnableTrainStop(data.disableTrainStop);
                $("#jqProvision").trigger("reloadGrid");
            }
            hideSpinner();
        },
        error: function () {
            showExceptionPopupMsg(defaultErrorMessage);
            hideSpinner();
        }
    });
}

function GetSelectedCheckTypes() {

    var checkTypes = $('select[multiple].promochecktypes').val();
    if (checkTypes !== null)
        $("#SelectedCheckTypes").val(checkTypes.join(","));
    else
        $("#SelectedCheckTypes").val('');
}

function GetSelectedLocations() {
    var locations = $('select[multiple].promostatelocations').val();
    if (locations !== null)
        $("#SelectedLocations").val(locations.join(","));
    else
        $("#SelectedLocations").val('');
}

function GetSelectedGroups() {

    var groups = $('select[multiple].promoGroupNames').val();
    if (groups !== null)
        $("#SelectedGroupNames").val(groups.join(","));
    else
        $("#SelectedGroupNames").val('');
}

function DisableOrEnableTrainStop(shouldDisable) {

    if (disableTrainStop === 'True' || shouldDisable === 'True' || disableTrainStop === true || shouldDisable === true) {
        $('#provisionSummaryButton').addClass('DisableButtons').attr('disabled','disabled');
        $('#provisionSummaryTrainStop').addClass('isDisabled');
    }
    else {
        $('#provisionSummaryButton').removeClass('DisableButtons').attr('disabled', false);
        $('#provisionSummaryTrainStop').removeClass('isDisabled');
    }    
}

function SaveProvision() {
    $("#provisionForm").submit();
}

function ShowPageLeavePopup(isQualifier) {
    var discount = $('#Value').val();
    if (discount == "")
    {
        window.location.href = ProvisionSummaryRedirectUrl + '?isQualifier=' + isQualifier;
    }
    else {
        var isFormValid = $('#provisionForm').validate().checkForm();
        if(isFormValid)
            ShowPopUpdataMinHeight(PageLeavePopup_Url, "Confirmation", 516, 200, isQualifier);
    }
    
}

function PopulateLocationsMutliSelect() {
    var locations = selectedLocations;
    var options;
    $.each(locations, function (index, e) {
        if (e != null) {
        var isAlreadyExists = $('.promostatelocations optgroup[label="' + e.Name + '"]').html() != null;
        var selected = e.Selected === true ? 'selected' : '';
        if (isAlreadyExists){
            $('.promostatelocations optgroup[label="' + e.Name + '"]').append('<option value="' + e.Id + '"' + selected + '>' + e.Code + '</option>');
        }
        else {
            options = '<optgroup label="' + e.Name + '">' +
                    '<option value="' + e.Id + '"' + selected + '>' + e.Code + '</option>' +
                   '</optgroup>'
            $('.promostatelocations').append(options);
        }
        }
    });
}
