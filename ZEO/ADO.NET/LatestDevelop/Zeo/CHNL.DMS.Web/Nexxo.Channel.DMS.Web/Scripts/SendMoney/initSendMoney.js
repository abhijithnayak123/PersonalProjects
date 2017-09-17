
var clickedOnDelFavButton = false;
$(document).ready(function () {
    frequentReceiverGallery();
    $('#stateRequiredStar').hide();
    $('#cityRequiredStar').hide();
    $('#PersonalMessage').placeholder();

    $("#DeliveryOptions").change(function () {

        var deliveryOptionsSelectedVal = $('#DeliveryOptions option:selected').val();

        if (deliveryOptionsSelectedVal == '002') {
            var selectedReceiverId = $('#ReceiverID').val();
            $.ajax({
                url: CheckPNotificationDetails,
                data: { ReceiverID: selectedReceiverId },
                type: 'GET',
                datatype: 'json',
                success: function (data) {
                        if (handleException(data))
                            return;
                    if (data) {
                    		getMessage(MoneyTransferException.MoneyTransfer_deliveryOptions);
                        event.preventDefault();
                        $('#continueButton').prop("disabled", true);
                        $('#continueButton').css("background-color", "##808080");
                    }
                },
                error: function () {

                }
            });

        }
        else if (($('#Amount').val() != '' || $('#DestinationAmount').val() != '') && selectedReceiverId != 0 && $('#ActOnMyBehalf').val() == "2") {
            $('#continueButton').prop("disabled", false);
            $('#continueButton').css("background-color", "#006937");
        }
    });

    $("#continueButton").click(function (event) {

        if ($('#ActOnMyBehalf').val() == "0") {
    		getMessage(MoneyTransferException.MoneyTransfer_ActOnMyBehalf);
            event.preventDefault();
        }
    });
    $("#ActOnMyBehalf").change(function () {

        var selectedReceiverId = $('#ReceiverID').val();

        if (this.value == '1') {
        	var message = getExceptionMessage(MoneyTransferException.MoneyTransfer_ActOnMyBehalf_Yes);
            showExceptionPopupMsgForActOnMyBehalf(message);
            $('#continueButton').prop("disabled", true);
            $('#continueButton').css("background-color", "##808080");

        }

        else if (this.value == '0') {
        	getMessage(MoneyTransferException.MoneyTransfer_ActOnMyBehalf);

            if (($('#Amount').val() == '' || $('#DestinationAmount').val() == '') && selectedReceiverId == 0 && $('#ActOnMyBehalf').val() != "2") {
                $('#continueButton').prop("disabled", true);
                $('#continueButton').css('background', '#8e8e8e');
            }
            else if (($('#Amount').val() != '' || $('#DestinationAmount').val() != '') && selectedReceiverId != 0 && $('#ActOnMyBehalf').val() == "2" && $('#DeliveryOptions option:selected').val() != '002') {
                $('#continueButton').prop("disabled", false);
                $('#continueButton').css("background-color", "#006937");
            }

        }

        else {

            if (($('#Amount').val() == '' || $('#DestinationAmount').val() == '') && selectedReceiverId == 0 && $('#ActOnMyBehalf').val() != "2") {
                $('#continueButton').prop("disabled", true);
                $('#continueButton').css('background', '#8e8e8e');
            }
            else
                if (($('#Amount').val() != '' || $('#DestinationAmount').val() != '') && selectedReceiverId != 0 && $('#ActOnMyBehalf').val() == "2" && $('#DeliveryOptions option:selected').val() != '002') {
                    $('#continueButton').prop("disabled", false);
                    $('#continueButton').css("background-color", "#006937");
                }
        }

    });

    $("#addreceiver").click(function () {
        SubmitForm();
    });

    $("#PersonalMessage").placeholder();

    $("#FirstName", "#LastName", "#SecondLastName").blur(function () {
        if (this.value != '')
            disableClick();
    });
    $("#Country", "#StateProvince", "#City", "#DeliveryMethod", "#DeliveryOptions").change(function () {
        if (this.value != '')
            disableClick();
    });

    $("#PersonalMessage").keypress(function (event) {
        if (!((event.keyCode > 47 && event.keyCode < 58) || (event.keyCode > 96 && event.keyCode < 123) || (event.keyCode > 64 && event.keyCode < 91) || (event.keyCode > 95 && event.keyCode < 106) || (event.keyCode == 46) || (event.keyCode == 44) || (event.keyCode == 32) || (event.keyCode == 8))) {
            return false;
        }
    });
    //AL-6293 starts
    $('#PersonalMessage').on('paste', function () {
        setTimeout(function () {
            regex = /[`~!@#$%^&*()_|+\-=?;:'"<>\{\}\[\]\\\/]/gi;
            sanitizeTextOnPaste('#PersonalMessage', regex);
        }, 0);
    });//ends

});

function SelectReceiver(receiverId) {
    if (clickedOnDelFavButton == false) {
        if (receiverId != null) {
            resetValidation();
            $('#ReceiverID').val(receiverId);
            $('#divheader')[0].innerHTML = 'Send Money to Receiver';
            fillReceiverDetailsFromFrequentReceiver(receiverId);
            $('#DestinationAmount').attr("disabled", false);
            $('#Amount').attr("disabled", false);
        }
    }
    else {
        clickedOnDelFavButton = false;
    }
}

function btnFavReceiverDeleteClick(control) {
    var receiverId = control.id.split("_")[1];
    var $confirmation = $("<div id='divDeletefavReceiverDialog'></div>");
    $confirmation.empty();
    $confirmation.dialog({
        autoOpen: false,
        title: "Zeo",
        width: 485,
        draggable: false,
        modal: true,
        resizable: false,
        closeOnEscape: false,
        minHeight: 125,
        scroll: false,
        cache: false,
        open: function (event, ui) {
            $confirmation.load(DelFavReceiverMsgPopup_URL, { id: receiverId }, function () {
                $('#btnYes').focus();
            });

        }
    });
    $confirmation.dialog("open")
    clickedOnDelFavButton = true;
}

function DeleteFavReceiver(receiverId) {
    $('#divDeletefavReceiverDialog').dialog('destroy').remove();
    showSpinner();
    $.ajax({
        type: "POST",
        url: DeleteFavoriteReceiver_URL,
        contentType: "application/json; charset=UTF-8",
        data: "{receiverId: '" + receiverId + "'}",
        processData: true,
        success: function (data) {
            if (data.data) {
                $('#frequentReceiver').html();
                showExceptionPopupMsg(data.data);
            }
            else {
                $('#frequentReceiver').html(data);
            }
            frequentReceiverGallery();
            hideSpinner();
        },
        error: function (data) {
            if (data != '') {
                $('#frequentReceiver').html();
                showExceptionPopupMsg(data.data);
            }
            frequentReceiverGallery();
            hideSpinner();
        }
    });
}

function disableClick() {
    $("#addreceiver").attr("onclick", "return false");
}

function SubmitForm() {
    var firstName = $('#FirstName').val();
    var lastName = $('#LastName').val();
    var deliveryMethod = $('#DeliveryMethod').val();
    var country = $('#Country').val();
    if (firstName == '') {
        $('#FirstName').rules('remove');        
    }
    if (lastName == '') {
        $('#LastName').rules('remove');
    }
    if (deliveryMethod == '')
    {
        $('#DeliveryMethod').rules('remove');
    }
    if (country == '') {
        $('#Country').rules('remove');
    }
    $("#sendMoneyForm").submit();
}

function selectPickUpStateAndCity() {
    var selectedPickUpCountry = $('select#Country');
    var selectedPickUpState = $('select#StateProvince');
    var selectedPickUpCity = $('select#City');
    var pickUpStateRequiredStar = $('#pickUpStateRequiredStar');
    var pickUpCityRequiredStar = $('#pickUpCityRequiredStar');
    var selectedCountryCode = selectedPickUpCountry.val().toUpperCase();

    if (selectedPickUpCountry.length != 0) {

        //$('#StateProvinceCode').val('');
        if (selectedCountryCode) {
            setCountryCurrencyCode();
            if (selectedCountryCode == 'US' || selectedCountryCode == 'MX' || selectedCountryCode == 'CA') {
                pickUpStateRequiredStar.show();
                $('select#StateProvince').rules("add", {
                    required: true,
                    messages: {
                        required: 'Please select State / Province.',
                    }
                });
                selectedPickUpState.prop("disabled", false);
                if (selectedCountryCode == 'MX') {
                    pickUpCityRequiredStar.show();
                    $('select#City').rules("add", {
                        required: true,
                        messages: {
                            required: 'Please select Pickup City.',
                        }
                    });

                }
                else {
                    setDropDownNotApplicable(selectedPickUpCity);
                }
            }
            else {
                pickUpStateRequiredStar.hide();
                pickUpCityRequiredStar.hide();

                setDropDownNotApplicable(selectedPickUpState);
                setDropDownNotApplicable(selectedPickUpCity);
            }
        }
    }
}

function setCountryCurrencyCode() {
    showSpinner();
    var countryselectedCountryCode = $('#Country').val();
    $.ajax({
        type: "GET",
        url: CurrencyCodeURL + '?countryCode=' + countryselectedCountryCode,
        dataType: "json",
        contentType: "application/json; charset=UTF-8",
        data: {},
        success: function (data) {
            if (handleException(data)) {
                hideSpinner();
                return;
            }
            $("#CurrencyType").val(data.toString());
            hideSpinner();
        },
        error: function () {
            showExceptionPopupMsg(defaultErrorMessage);
            hideSpinner();
        }
    });
}

function fillStateDropDown() {
    var selectedCountryCode = $("#Country").val();
    if (selectedCountryCode == 'US' || selectedCountryCode == 'MX' || selectedCountryCode == 'CA') {
        showSpinner();
        $.ajax({
            url: WUStates_URL,
            data: { countryCode: selectedCountryCode }, //parameters go here in object literal form 
            type: 'GET',
            datatype: 'json',
            success: function (wuStates) {
                if (handleException(wuStates)) {
                    hideSpinner();
                    return;
                }
                statesDropDown = $('select#StateProvince');
                statesDropDown.attr("disabled", false);
                $('#pickUpStateRequiredStar').show();
                bindDropdownList(statesDropDown, wuStates);
                var stateSelectedValue = $('#StateProvinceCode').val();
                $('select#StateProvince').val(stateSelectedValue);
                hideSpinner();
            },
            error: function () {
                showExceptionPopupMsg(defaultErrorMessage);
                hideSpinner();
            }
        });
    }
}

function fillCityDropDown() {
    var selectedCountryCode = $('#Country').val();
    var selectedstateCode = $('#StateProvinceCode').val();
    if (selectedCountryCode == 'MX') {
        $('#pickUpCityRequiredStar').show();
        showSpinner();
        $.ajax({
            url: WUCities_URL,
            data: { stateCode: selectedstateCode }, //parameters go here in object literal form 
            type: 'GET',
            datatype: 'json',
            success: function (wuCities) {
                if (handleException(wuCities)) {
                    hideSpinner();
                    return;
                }
                citiesDropDown = $('select#City');
                citiesDropDown.attr("disabled", false);
                bindDropdownList(citiesDropDown, wuCities);
                var selectedPickUpCity = $('#CityID').val();
                $('select#City').val(selectedPickUpCity);
                hideSpinner();
            },
            error: function () {
                showExceptionPopupMsg(defaultErrorMessage);
                hideSpinner();
            }
        });
    }
}

function setCountryCurrencyCode() {
    var selectedCountryCode = $("#Country").val();
    if (selectedCountryCode != "") {
        showSpinner();
        $.ajax({
            type: "GET",
            url: CurrencyCodeURL + '?countryCode=' + selectedCountryCode,
            dataType: "json",
            contentType: "application/json; charset=UTF-8",
            data: {},
            success: function (data) {
                if (handleException(data)) {
                    hideSpinner();
                    return;
                }
                $("#CurrencyType").val(data.toString());
                $('#CountryCode').val(selectedCountryCode);
                hideSpinner();
            },
            error: function () {
                showExceptionPopupMsg(defaultErrorMessage);
                hideSpinner();
            }
        });
    }
}

function changeCountryDropDown() {
    var selectedCountryCode = $("#Country").val().toUpperCase();
    var citiesDropDown = $('select#City');
    var pickupStateDropDown = $('select#StateProvince');
    var pickUpStateRequired = $('#pickUpStateRequiredStar');
    var pickUpCityRequired = $('#pickUpCityRequiredStar');
    var divRequiredPickUpState = $('#divRequiredPickUpState');
    var divRequiredPickUpCity = $('#divRequiredPickUpCity');
    var pickUpDeliveryMethod = $('select#DeliveryMethod');
    var pickUpDeliveryOptions = $('select#DeliveryOptions');
    $('#StateProvinceCode').val('');
    $('#CityID').val('');
    $('#CityName').val('');
    if (selectedCountryCode) {
        setCountryCurrencyCode();
        if (selectedCountryCode == 'US' || selectedCountryCode == 'MX' || selectedCountryCode == 'CA') {
            pickUpStateRequired.show();
            $('select#StateProvince').rules("add", {
                required: true,
                messages: {
                    required: 'Please select State / Province.',
                }
            });
        }
        if (selectedCountryCode.toString() == 'US' || selectedCountryCode.toString() == 'MX' || selectedCountryCode.toString() == 'CA') {

            pickUpStateRequired.show();
            fillStateDropDown();
            if (selectedCountryCode == 'MX') {
                pickUpCityRequired.show();
                setDropDownSelect(citiesDropDown);
            }
            else {
                pickUpCityRequired.hide();
                setDropDownNotApplicable(citiesDropDown);
            }
        }
        else {
            pickUpStateRequired.hide();
            pickUpCityRequired.hide();

            divRequiredPickUpState.hide();
            divRequiredPickUpCity.hide();
            setDropDownNotApplicable(pickupStateDropDown);
            setDropDownNotApplicable(citiesDropDown);

            //fill Delivery Method DDL
            setDropDownSelect(pickUpDeliveryOptions);
        }
        fillDeliveryMethodDropDown();
    }
    else {
        pickUpStateRequired.hide();
        setDropDownSelect(pickupStateDropDown);
        setDropDownSelect(citiesDropDown);
        setDropDownSelect(pickUpDeliveryMethod);
        setDropDownSelect(pickUpDeliveryOptions);
    }
}

function changeStateProvinceDropDown() {
    var selectedCountryCode = $('select#Country').val().toUpperCase();
    var selectedStateCode = $('select#StateProvince').val().toUpperCase();
    var pickUpCityRequired = $('#pickUpCityRequiredStar');
    var citiesDropDown = $('select#City');
    $('#StateProvinceCode').val(selectedStateCode);

    fillCityDropDown();
    if (selectedCountryCode.toString() == 'US' || selectedCountryCode.toString() == 'MX' || selectedCountryCode.toString() == 'CA') {

        $('#pickUpStateRequiredStar').show();
        $('select#City').rules("add", {
            required: true,
            messages: {
                required: 'Please select Pickup City.',
            }
        });
    }
    var selectedPickUpCity = $('#CityID').val();
    $('select#City').val(selectedPickUpCity);
    if (selectedCountryCode != 'MX') {
        pickUpCityRequired.hide();
        setDropDownNotApplicable(citiesDropDown);
        fillDeliveryMethodDropDown();
    }
}

//*** may we can remove this one
function fillActOnMyBehalf() {

    // Make ajax call & fill the ActOnMyBehalf DDL
    var selectedActOnMyBehalf = $('select#ActOnMyBehalf').val();
    var selectedActOnMyBehalfName = $('select#StateProvince option:selected').text();
    var ActOnMyBehalf_URL = initGetValuesForActOnMyBehalf;
    showSpinner();
    $.ajax({
        url: ActOnMyBehalf_URL,
        data: {},
        type: 'GET',
        datatype: 'json',
        success: function (ActOnMyBehalf) {
            if (handleException(ActOnMyBehalf)) {
                hideSpinner();
                return;
            }
            ActOnMyBehalfDropDown = $('select#ActOnMyBehalf');
            bindDropdownList(ActOnMyBehalfDropDown, ActOnMyBehalf);
            hideSpinner();
        },
        error: function () {
            showExceptionPopupMsg(defaultErrorMessage);
            hideSpinner();
        }
    });
}

function fillDeliveryMethodDropDown() {
    var selectedCountryCode = $('select#Country').val();
    var countryCurrencyCode = $("#CurrencyType").val();
    var selectedStateCode = $('select#StateProvince option:selected').val();
    var selectedStateName = $('select#StateProvince option:selected').text();
    var selectedCityCode = $('select#City option:selected').val();
    var selectedCityName = $('select#City option:selected').text();
    var DeliveryMethods_URL = initSendMoneyWUDeliveryMethods;

    if (selectedStateCode == "") {
        selectedStateName = "";
    }
    if (selectedCityCode == "") {
        selectedCityName = "";
    }
    showSpinner();
    $.ajax({
        url: DeliveryMethods_URL,
        data: { countryCode: selectedCountryCode, state: selectedStateName, stateCode: selectedStateCode, city: selectedCityName }, //parameters go here in object literal form 
        type: 'GET',
        datatype: 'json',
        success: function (wuMethods) {
            if (handleException(wuMethods)) {
                hideSpinner();
                return;
            }
            deliveryMethodDropDown = $('select#DeliveryMethod');
            bindDropdownList(deliveryMethodDropDown, wuMethods);
            hideSpinner();
        },
        error: function () {
            showExceptionPopupMsg(defaultErrorMessage);
            hideSpinner();
        }
    });
}

function fillDeliveryOptionDropDown() {
    var selectedCountryCode = $('select#Country').val();
    var selectedStateName = $('select#StateProvince option:selected').text();
    var selectedCityName = $('select#City option:selected').text();
    var DeliveryOptions_URL = initSendMoneyWUDeliveryOptions;

    //Fill Delivery Options based on Delivery Method Selection
    var deliveryMethodDropDown = $('select#DeliveryMethod option:selected');
    var deliveryOptionsDropDown = $('select#DeliveryOptions');

    var selectedSVCCode = deliveryMethodDropDown.val();

    if ($('select#StateProvince option:selected').val() == "") {
        selectedStateName = "";
    }
    if ($('select#City option:selected').val() == "") {
        selectedCityName = "";
    }
    showSpinner();
    $.ajax({
        url: DeliveryOptions_URL,
        data: { countryCode: selectedCountryCode, state: selectedStateName, city: selectedCityName, svcCode: selectedSVCCode }, //parameters go here in object literal form 
        type: 'GET',
        datatype: 'json',
        success: function (wuOptions) {
            if (handleException(wuOptions)) {
                hideSpinner();
                return;
            }
            bindDropdownList(deliveryOptionsDropDown, wuOptions);
            hideSpinner();
        },
        error: function () {
            showExceptionPopupMsg(defaultErrorMessage);
            hideSpinner();
        }
    });
}

function setLastNameAutoComplete() {

    $('#LastName').autocomplete({
        source: initSendMoneyAutoComplete,
        focus: function () {
            // prevent value inserted on focus
            return false;
        },
        select: function (event, ui) {
            $('#LastName').val(ui.item.label);
            var lastfirstname = ui.item.label;
            var array = lastfirstname.split(/[\s,]+/);
            $('#LastName').val(array[1]);
            $('#LastNameFirstName').val(lastfirstname);
            //var terms = split(this.value);
            // remove the current input
            array.pop();
            // add the selected item
            array.push(ui.item.value);
            // add placeholder to get the comma-and-space at the end
            array.push("");
            //this.value = array.join(", ");
            return false;
        }
    });
}

function DestinationAmountWithCurrency() {
    var text = $("#destAmtmsg").val().length;
    var amtlength = $("#DestinationAmount").val().length;
    if (amtlength > 0) {
        $("#destAmtmsg").show();
        $("#destinationAmountErrormsg").css("display", "none");
    }
}

function fillReceiverDetailsUsingAutoComplete() {
    var ReceiverIdURL = initSendMoneyReceiverByFullName;
    if ($('#LastName').val() != '') {
        showSpinner();
        $.ajax({
            type: "GET",
            url: ReceiverIdURL + '?fullName=' + $('#LastNameFirstName').val(),
            dataType: "json",
            contentType: "application/json; charset=UTF-8",
            data: {},
            success: function (data) {
                if (handleException(data))
                    return true;
                if (data != null && data != 0) {
                    $('#ReceiverID').val(data);
                    $('#divheader')[0].innerHTML = 'Send Money to Receiver';
                    fillReceiverDetailsFromFrequentReceiver(data);
                    $('#addreceiver').css('display', 'none');
                    $('#editreceiver').css('display', 'block');
                }
                hideSpinner();
            }
        });
    }
}

$(document).ready(function () {

    //Regular expression validation expression
    var regexName = /^[a-zA-Z\- ']*$/;

    var selectedCountry = $('select#Country').val();
    if (selectedCountry != 'US' && selectedCountry != 'MX' && selectedCountry != 'CA' && selectedCountry != "") {

        var stateDropDown = $('select#StateProvince');
        var citiesDropDown = $('select#City');
        setDropDownNotApplicable(stateDropDown);
        setDropDownNotApplicable(citiesDropDown);
    }

    $('select#Country').change(function () {
        $('select#StateProvince').rules('remove');
        $('select#City').rules('remove');

        changeCountryDropDown();
    });

    $('select#StateProvince').change(function () {
        $('select#City').rules('remove');
        changeStateProvinceDropDown();
    });

    $('select#City').change(function () {
        var selectedCityName = $('select#City option:selected').text();
        $('#CityName').val(selectedCityName);
        fillDeliveryMethodDropDown();
    });

    $('select#DeliveryMethod').change(function () {
        var deliveryMethod = $(this).val();
        if (deliveryMethod != "") {
            fillDeliveryOptionDropDown();
        }
        else {
            var deliveryOptionDropDown = $('select#DeliveryOptions');
            setDropDownSelect(deliveryOptionDropDown);
        }
    });

    var receiverId = $('#ReceiverID').val();
    if (initSendMoneyReceiverID != '0') {
        $('#addreceiver').css('display', 'none');
        $('#editreceiver').css('display', 'block');
        $('#divheader')[0].innerHTML = 'Send Money to Receiver';
        $('#continueButton').prop("disabled", true);
        $('#continueButton').css('background', '#006937');
        $('#continueButton').prop("disabled", false);
        selectPickUpStateAndCity();
    }
    else {
        $('#continueButton').prop("disabled", true);
        $('#continueButton').css('background', '#8e8e8e');
        $('#continueButton').css('color', '#ffffff');
    }
    //We have disable the text box 
    //setLastNameAutoComplete();

    $('#DestinationAmountWithCurrency').focus(function () {
        $('#destAmountSymbols').hide();
        $('#destAmount').show();

        //Commented code because "DestinationAmount" is hidden. So it throws error in IE 8.
        var browsername = navigator.userAgent;

        var msie = browsername.indexOf("MSIE")
        var version = parseInt(browsername.substring(msie + 5, browsername.indexOf(".", msie)));

        if (browsername.indexOf("MSIE") != -1 && version != 8)
            $('#DestinationAmount').focus();

    });

    $('#DestinationAmount').blur(function () {
        var selectedReceiverId = $('#ReceiverID').val();

        // Added this condition for User Story # US1956. - Start.

        if ($('#DestinationAmount').val() == '') {
            $('#continueButton').prop("disabled", true);
            $('#continueButton').css('background', '#8e8e8e');
        }
        if ($('#DestinationAmount').val() == '' && selectedReceiverId == 0) {
            $('#continueButton').prop("disabled", true);
            $('#continueButton').css('background', '#8e8e8e');
        }
        else if ($('#DestinationAmount').val() != '' && selectedReceiverId != 0 && $('#DeliveryOptions option:selected').val() != '002') {
            $('#continueButton').prop("disabled", false);
            $('#continueButton').css("background-color", "#006937");
        }

        // Added this condition for User Story # US1956. - Complete.
        $('#destAmountSymbols').show();
        $('#destAmount').hide();
        var cur = $('#CurrencyType').val();
        if ($('#DestinationAmount').val() != '') {
            $('#Amount').val('');
            $('#AmountWithCurrency').val('');
            var amountModified = $('#DestinationAmount').val() + ' ' + $('#CurrencyType').val();
            $('#DestinationAmountWithCurrency').val(amountModified);
            DestinationAmountWithCurrency();
        }
        else
            $('#DestinationAmountWithCurrency').val('');
    });

    $('#AmountWithCurrency').focus(function () {
        $('#transamtwithSymbols').hide();
        $('#transamt').show();

        //Commented code because "Amount" is hidden. So it throws error in IE 8.
        var browsername = navigator.userAgent;

        var msie = browsername.indexOf("MSIE")
        var version = parseInt(browsername.substring(msie + 5, browsername.indexOf(".", msie)));

        if (browsername.indexOf("MSIE") != -1 && version != 8)
            $('#Amount').focus();

    });

    $('#Amount').blur(function () {
        var selectedReceiverId = $('#ReceiverID').val();

        $('#transamtwithSymbols').show();
        $('#transamt').hide();
        var amount = $('#Amount').val();
        if (amount != '') {
            $('#DestinationAmount').val('');
            $('#DestinationAmountWithCurrency').val('');
            var amountModified = "$ " + amount + " USD";
            $('#AmountWithCurrency').val(amountModified);
            if (selectedReceiverId != 0 && $('#DeliveryOptions option:selected').val() != '002') {
                $('#continueButton').prop("disabled", false);
                $('#continueButton').css("background-color", "#006937");
            }
        }
        else {
            $('#AmountWithCurrency').val('');
        }
    });

    var sourceCurrencyAmount = function () {
        $('#DestinationAmount').attr("disabled", true);
        $('#DestinationAmountWithCurrency').attr("disabled", true);
        var amount = $.trim($('#Amount')[0].value);
        if (amount == '') {
            $('#DestinationAmountWithCurrency').attr("disabled", false);
            $('#DestinationAmount').attr("disabled", false);
        }
    };

    var destinationCurrencyAmount = function () {
        $('#Amount').attr("disabled", true);
        $('#AmountWithCurrency').attr("disabled", true);

        var amount = $.trim($('#DestinationAmount')[0].value);
        if (amount == '') {
            $('#AmountWithCurrency').attr("disabled", false);
            $('#Amount').attr("disabled", false);

        }
    };
    $('#Amount').on('keyup', sourceCurrencyAmount);

    $('#DestinationAmount').on('keyup', destinationCurrencyAmount);

    $('#AmountWithCurrency').siblings('.clearlink').mousedown(function () {
        $('#AmountWithCurrency').val('');
        $('#Amount').val('').focus();
        sourceCurrencyAmount();
    });
    $('#Amount').siblings('.clearlink').mousedown(function () {
        $('#AmountWithCurrency').val('');
        $('#Amount').val('').focus();
        sourceCurrencyAmount();
    });

    $('#DestinationAmountWithCurrency').siblings('.clearlink').mousedown(function () {
        $('#DestinationAmountWithCurrency').val('');
        $('#DestinationAmount').val('').focus();
        destinationCurrencyAmount();    
    });

    $('#DestinationAmount').siblings('.clearlink').mousedown(function () {
        $('#DestinationAmountWithCurrency').val('');
        $('#DestinationAmount').val('').focus();
        destinationCurrencyAmount();
    });
    //We have disable the input text box
    //$('#LastName').blur(function () {
    //    fillReceiverDetailsUsingAutoComplete();
    //});

    $('#Amount').blur(function () {
        var deliveryMethodValue = $('select#DeliveryMethod').val();
        var selectedPickUpCountry = $('select#Country').val();
        if (deliveryMethodValue == "" && selectedPickUpCountry != "")
            fillDeliveryMethodDropDown();
        if (initSendMoneyErrorMessage !== '') {
            $('#divErrorMsg').hide();
        }
        else {
            $('#divErrorMsg').show();
            if (!($("#Amount").val() == '' || $("#Amount").val() == NaN && $("#DestinationAmount").val() == '' || $("#DestinationAmount").val() == NaN)) {
                $('#destinationAmountErrormsg').hide();
            }
            else {
                $('#destinationAmountErrormsg').show();
            }
        }
    });

    $('#Amount').keypress(function (event) {
        if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || ((event.keyCode == 65 || event.keyCode == 86) && event.ctrlKey === true)) {
            return;
        }
        else {
            if (event.keyCode < 48 || event.keyCode > 57) {
                event.preventDefault();
            }
        }
    });

    $('#DestinationAmount').keypress(function (event) {
        if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || ((event.keyCode == 65 || event.keyCode == 86) && event.ctrlKey === true)) {
            return;
        }
        else {
            if (event.keyCode < 48 || event.keyCode > 57) {
                event.preventDefault();
            }
        }
    });

    $('#CouponPromoCode').blur(function () {
        $('#CouponPromoCode').val(convertLowerToCaps($('#CouponPromoCode').val()));
    });

    $('#FirstName').keypress(function (e) {
        ValidateKey(e, regexName);
    });

    $('#LastName').keypress(function (e) {
        ValidateKey(e, regexName);
    });

    $('#SecondLastName').keypress(function (e) {
        ValidateKey(e, regexName);
    });
}

);

function ValidateKey(e, regex) {
    var enterKey = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (regex.test(enterKey)) {
        return true;
    }
    e.preventDefault();
    return false;
}

function importPastReceiver() {
    var productName = "receiver";
    showSpinner();
    $("#loadingspinner").text("Importing Favorite Receivers....");
    $.ajax({
        type: "POST",
        url: initSendMoneyImportPastReceiver,
        contentType: "application/json; charset=UTF-8",
        data: "{productName: '" + productName + "'}",
        processData: true,
        success: function (data) {
            if (data != '') {
                $('#frequentReceiver').html(data);
                frequentReceiverGallery();
            }
            hideSpinner();
        },
        error: function () {
            frequentReceiverGallery();
            hideSpinner();
        }
    });
}

// This method will create a Gallery for Imported Receivers.
function frequentReceiverGallery() {
    if (jQuery("#gallery").length) {
        var totalImages = jQuery("#gallery > li").length,
        imageWidth = jQuery("#gallery > li:first").outerWidth(true),
        totalWidth = imageWidth * totalImages,
        visibleImages = Math.round(jQuery("#gallery-wrap").width() / imageWidth),
        visibleWidth = visibleImages * imageWidth,
        stopPosition = (visibleWidth - totalWidth);
        jQuery("#gallery").width(totalWidth);

        jQuery("#gallery-prev").click(function () {
            if (jQuery("#gallery").position().left < 0 && !jQuery("#gallery").is(":animated")) {
                jQuery("#gallery").animate({ left: "+=" + 3 * imageWidth + "px" });
            }
            return false;
        });

        jQuery("#gallery-next").click(function () {
            if (jQuery("#gallery").position().left > stopPosition && !jQuery("#gallery").is(":animated")) {
                jQuery("#gallery").animate({ left: "-=" + 3 * imageWidth + "px" });
            }
            return false;
        });
    }
}


function convertLowerToCaps(str) {
    return str.toUpperCase();
}

function clearAmount() {
    $('#Amount').val('');
    $('#DestinationAmount').val('');
    $('#AmountWithCurrency').val(' ');
    $('#DestinationAmount').val('');
    $('#DestinationAmountWithCurrency').val('');
    $('#DestinationAmountFromFeeEnquiry').val('');
    $('#CouponPromoCode').val('');
}

function fillReceiverDetailsFromFrequentReceiver(selectedReceiverId) {
    clearAmount();
    var populateReceiverDetailsURL = initSendMoneyPopulateReceiverDetails;
    showSpinner();
    $.ajax({
        type: "GET",
        url: populateReceiverDetailsURL + '?ReceiverId=' + selectedReceiverId,
        dataType: "json",
        contentType: "application/json; charset=UTF-8",
        data: {},
        success: function (data) {
            if (handleException(data)) {
                hideSpinner();
                return;
            }
            var fqRLogo = imgSendMoneyfreqRec;

            // If any frequent receiver has been highlighted already, then remove that highlighting
            // and the associated image.
            $('a[id^=fqRcvr]').css("background-color", "#006937");
            $('a[id^=fqRcvr]').css("color", "#ffffff");
            $('a[id^=fqRcvr]').find('#fqRcrLogo').attr("src", fqRLogo);

            var receiverId = '#fqRcvr_';

            populateReceiver(data);
            if (data.Country != null && data.ReceiverName != null) {
                $('#editReceiver').css("background-color", "#006937");
                $('#continueButton').css("background-color", "#006937");
                $('#editReceiver').attr("disabled", false);
                $('#continueButton').attr("disabled", false);

                //set currency code
                setCountryCurrencyCode();

                //Fill State DropDownLists based on country
                fillStateDropDown();

                //Fill City based on the state
                fillCityDropDown();

                //Fill up Delivery Method DDL
                var wuMethods = data.LDelivertyMethods;
                bindDropdownList($('select#DeliveryMethod'), wuMethods);
                $('select#DeliveryMethod').val(data.DeliveryMethod);

                //Fill Delivery Options
                var wuOptions = data.LDeliveryOptions;
                bindDropdownList($('select#DeliveryOptions'), wuOptions);
                $('select#DeliveryOptions').val(data.DeliveryOptions);

                receiverId += data.ReceiverID;

                highlightFrequentReceiver(receiverId);
            }
            else {
                $('#editReceiver').css("background-color", "##808080");
                $('#editReceiver').attr("disabled", true);
            }
            $('#ReceiverID').val(data.ReceiverID);
            $('#editreceiver').prop('href', initSendMoneyGetReceiverForEdit + data.ReceiverID);
            selectPickUpStateAndCity();
            hideSpinner();
        },
        error: function (data) {
            if (data.status == "404")
                showExceptionPopupMsg("Could not find receiver " + selectedReceiver);
            hideSpinner();
        },
        complete: function () {
            $('#addreceiver').css('display', 'none');
            $('#editreceiver').css('display', 'block');
        }
    });
}

function highlightFrequentReceiver(receiverId) {
    if ($(receiverId).length) {
        $(receiverId).css("background-color", "#fdbe57");
        $(receiverId).css("color", "#000000");
        $(receiverId).css("font-size", "12px");
        $(receiverId).find("#fqRcrLogo").attr("src", imgSendMoneyselectedRec);

        var totalImages = jQuery("#gallery > li").length,
		imageWidth = jQuery("#gallery > li:first").outerWidth(true),
		totalWidth = imageWidth * totalImages,
		visibleImages = Math.round(jQuery("#gallery-wrap").width() / imageWidth),
		visibleWidth = visibleImages * imageWidth,
		stopPosition = (visibleWidth - totalWidth);

        // if the selected receiver( from the auto-complete) is present in the list of favorite receivers
        if (jQuery(receiverId).position().left != undefined && jQuery("#gallery") != undefined) {

            // if the the selected receiver is the left-most & beyond the visible area, then move the trolley to the right.
            if (jQuery("#gallery").position().left < 0 && jQuery(receiverId).position().left == 0 && !jQuery("#gallery").is(":animated")) {
                jQuery("#gallery").animate({ left: "+=" + imageWidth + "px" });
            }

            // if the selected receiver is the right-most & beyond the visible area, then move the trolley to the left.
            if (jQuery("#gallery").position().left > stopPosition && jQuery(receiverId).position().left == visibleWidth && !jQuery("#gallery").is(":animated")) {
                jQuery("#gallery").animate({ left: "-=" + imageWidth + "px" });
            }
        }
    }
}

function populateReceiver(data) {
    $('#FirstName').val(data.FirstName);
    $('#LastName').val(data.LastName);
    $('#SecondLastName').val(data.SecondLastName);
    $('#Country').val(data.Country);
    $('#StateProvince').val(data.StateProvince);
    $('#City').val(data.City);
    $('#CountryCode').val(data.CountryCode);
    $('#StateProvinceCode').val(data.StateProvinceCode);
    $('#CityID').val(data.CityID);
    $('#DeliveryMethod').val(data.DeliveryMethod);
    $('#DeliveryOptions').val(data.DeliveryOptions);
    $('#PickUpMethodId').val(data.PickUpMethodId);
    $('#PickUpOptionsId').val(data.PickUpOptionsId);
    $('#CityName').val(data.CityName);
}

///*** may we can remove this one
function resetOverlays() {
    var dialogs = $("div.ui-dialog");
    if (dialogs.length == 0) {
        $(".ui-widget-overlay").remove();
    }
}

///*** may we can remove this one
function jqGridOverlayRemove() {
    $(".ui-widget-overlay jqgrid-overlay").remove();
}

///*** may we can remove this one
function addReceiver() {
    var $confirm = $("<div id='findReceiver'></div>");
    $confirm.empty();
    $confirm.dialog({
        autoOpen: false,
        title: "Receivers",
        width: 650,
        draggable: false,
        resizable: false,
        closeOnEscape: false,
        modal: true,
        height: 500,
        cache: false,
        open: function (event, ui) {
            $confirm.load(sendMoneyDisplayAddReceiverURL);
        }
    });
    $confirm.dialog("open");
}

///*** may we can remove this one
function createReceiver() {
    var $confirm = $("<div id='addReceiver'></div>");
    $confirm.empty();
    $confirm.dialog({
        autoOpen: false,
        title: "Create Receiver",
        width: 650,
        draggable: false,
        resizable: false,
        closeOnEscape: false,
        modal: true,
        height: 550,
        cache: false,
        open: function (event, ui) {
            $confirm.load(createReceiverURL);
        }
    });
    $confirm.dialog("open");
}

///*** may we can remove this one
function editReceiver(id) {
    var $confirm = $("<div id='editReceiver'></div>");
    $confirm.empty();
    $confirm.dialog({
        autoOpen: false,
        title: "Edit Receiver",
        width: 650,
        draggable: false,
        resizable: false,
        closeOnEscape: false,
        modal: true,
        height: 550,
        cache: false,
        open: function (event, ui) {
            var url = getReceiverForEditURL + '?ReceiverId=' + id + '&cheatDate=' + Date.now();
            $confirm.load(url);
        }
    });

    $confirm.find('input:text, textarea').val('').attr('placeholder', 'Enter some value');
    $confirm.find('select').val(''); //you need to have and option like <option value="">Please Select</option> in your select options list
    $confirm.dialog("open");
}

///*** may we can remove this one
function SearchReceiver() {
    location.href = SearchReciverGrid;
}

function saveReceiverConfirmation() {
    var $confirm = $("<div id='saveReceiverConfirmMsg'></div>");
    $confirm.empty();
    $confirm.dialog({
        autoOpen: false,
        title: "Save Receiver",
        width: 300,
        draggable: false,
        resizable: false,
        closeOnEscape: false,
        modal: true,
        height: 125,
        cache: false,
        open: function (event, ui) {
            $confirm.load(SaveReceiverConfirmationURL);
        }
    });
    $confirm.dialog("open");
}

function resetValidation() {
    //Removes validation from input-fields
    $('.input-validation-error').addClass('field-validation-valid').removeClass('field-validation-valid');
    //Removes validation message after input-fields
    $('.field-validation-error').addClass('field-validation-valid').removeClass('field-validation-error');
    //Removes validation summary 
    $('.validation-summary-errors').addClass('validation-summary-valid').removeClass('validation-summary-errors');
}


//dropdown_box

//function SendNotification
