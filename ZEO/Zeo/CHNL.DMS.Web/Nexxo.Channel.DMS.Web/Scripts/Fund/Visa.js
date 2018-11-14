$(document).ready(function () {
    $("#MaskCardNumber").focusout(function () {
        var cardNumber = $("#MaskCardNumber").val();
        if (!($("#MaskCardNumber").attr('readonly') == 'readonly') && cardNumber.indexOf('*') == -1) {
            //EncryptCardNo();
        }
    });
    var trxId = $('#TransactionId').val();
    if (trxId != "0" && trxId) {
        //EncryptCardNo();
        productMenuDisable();
        $("#ProxyId").attr('readonly', true);
        $("#ProxyId").addClass("disable_txt");
        $("#MaskCardNumber").attr('readonly', true);
        $("#MaskCardNumber").addClass("disable_txt");
        $("#MaskCardNumber").addClass("margin_right_0");
        $("#PseudoDDA").attr('readonly', true);
        $("#PseudoDDA").addClass("disable_txt");
        $("#ExpirationDate").attr('readonly', true);
        $("#ExpirationDate").addClass("disable_txt");
        $('#swipeCard').attr("disabled", "disabled");
        $("#swipeCard").addClass("OpaqueViewCart");
        $('#associateCard').attr("disabled", "disabled");
        $('#associateCard').addClass("OpaqueViewCart");
    }

    $("#MaskCardNumber").keydown(function (event) {
        $('#pan_validationMessage').empty();
        restrictCharactersAlloyNumbers(event);
        $('#pan_message').empty();
    });

    $("#MaskCardNumber").focusout(function (event) {
        setMaxLength(19);
    });

    $('#btnRemoveCartItem').click(function () {
        showDeleteDialogue(ItemId, ScreenName, Product, "pending");
    });

    $('#submitPrepaidPurchase').click(function () {
        var isValid = $('#ProductCredentialForm').valid();
        if (isValid) {
            showSpinner();
        }
    });

    $("#ExpirationDate").mask("9?9/9999");
    $('#ExpirationDate[placeholder]').placeholder();
    $().maxlength();
    $("#MaskedPAN").mask("9999-9999-9999-9999");

    $('#InitialLoad').keypress(function (e) {

        var number = $(this).val().split('.');

        if (number.length == 2 && number[1].length > 1) {
            if (number[0].length < $(this)[0].selectionStart) {
                e.preventDefault();
            }
        }

        avoidSpecialCharNegSign(e, this);

        if (event.keyCode == 37) {  
            e.preventDefault();
        }

    });

    $('#InitialLoad').keyup(function (e) {

        if ($(this).val() >= 25 && $(this).val() <= 1000) {
            $('#InitialLoad').parent('div').next('div').empty();
        }

    });
    $('#InitialLoad').blur(function () {
        if ($(this).val() < 25)
            $('#InitialLoad').parent('div').next('div').html("<span style='color:red'>" + minimumLoadAmountErrorMessage + "</span>");

        if ($(this).val() > 1000)
            $('#InitialLoad').parent('div').next('div').html("<span style='color:red'>" + maximumLoadAmountErrorMessage + "</span>");
    });

    $("#ProxyId").keypress(function (e) {
        var enterKey = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        var regexNumbers = /^[\d]*$/;
        if (regexNumbers.test(enterKey)) {
            return true;
        }
        e.preventDefault();
        return false;
    });

    $("#PseudoDDA").keypress(function (e) {
        var enterKey = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        var regexNumbers = /^[\d]*$/;
        if (regexNumbers.test(enterKey)) {
            return true;
        }
        e.preventDefault();
        return false;
    });

    //SHIPPING TYPES
    //Express Shipping = 0 , Standard Mail =2,Instant Issue = -2
    $('#CardShippingType').change(function () {
        validateControls();
        $('#cardSubmit').attr("disabled", false);
        $('#pan_div').hide();
        $('#shipping_message').empty();
        $('#visa_error_txt').empty();
        $('#shipping_message_error').empty();
        var cardBalance = $('#CardBalance').val();
        var shippingType = $('#CardShippingType').val();
        var prepaidAction = $('#PrepaidAction').val();

        if ((shippingType == "0") && (prepaidAction == "14" || prepaidAction == "5" || prepaidAction == "6")) {
            if ($('#visa_message').text() == "") {
                GetFundFee(prepaidAction, customerSessionId);
            }
            $('#visa_message').show();
        }
        else if ((shippingType == "2") && (prepaidAction == "14" || prepaidAction == "5" || prepaidAction == "6")) {
            $('#shipping_message').append("<p>No fee</p>")
            if ($('#visa_message').text() == "") {
                GetFundFee(prepaidAction, customerSessionId);
            }
        } else if (shippingType == "-2") {
            $('#MaskedPAN').focus();
            $('#pan_div').show();
            $('#visa_message').hide();
        }
        GetShippingFee(shippingType, cardBalance, customerSessionId);
    });

    //VISA CARD status
    //Active = 0,Suspended = 3,Lost = 5,Stolen = 6
    $('#PrepaidAction').change(function () {
        validateControls();
        $('#visa_message').empty();
        var prepaidAction = $('#PrepaidAction').val();
        var cardBalance = $('#CardBalance').val();
        var shippingType = $('#CardShippingType').val();
        $('#CardShippingType').prop('disabled', false);
        if (prepaidAction == "0") {
            $('#CardShippingType').prop('disabled', true);
            $('#shipping_message_error').empty();
            $('#shipping_message').empty();
            $('#CardShippingType').val('');
        }
        if (prepaidAction == "5" || prepaidAction == "6" || prepaidAction == "0" || prepaidAction == "14") {
            $('#visa_message_error').empty();
            if (shippingType != "-2")
                GetFundFee(prepaidAction, customerSessionId);
        } else if (prepaidAction == "3") {
            $('#visa_message_error').empty();
            $('#visa_message').show();
            $('#visa_message').empty();
            $('#visa_message').append("<p>Do Not Replace</p>");
            $('#shipping_message_error').empty();
            $('#visa_error_txt').empty();
            $('#CardShippingType').prop('disabled', true);
            $('#cardSubmit').attr("disabled", false);
        } else if (prepaidAction == "") {

        } else {
            $('#visa_message').empty();
            $('#visa_message').append("<p>No replacement fee</p>");
        }
        if (prepaidAction != "0" && prepaidAction != "3" && shippingType != "") {
            GetShippingFee(shippingType, cardBalance, customerSessionId);
        }
    });

    $('#MaskedPAN').blur(function () {
        $('#pan_message').empty();
        $("#MaskedPAN [data-val]").attr("data-val", true);
        $('#pan_validationMessage').show();
        var pan = $(this).val();
        var maskedValue = "****-****-****-";
        pan = pan.replace(/-/g, "");
        if (pan.length == 16) {
            $('#PAN').val(pan);
            $('#MaskedPAN').val(maskedValue + pan.substring(12, pan.length));
        }
    });

    //Checking whether the Promotion meets all the qualifier if it is registered in ZEO or not.
    $("#PromoCode").change(function (event) {
        validateVisaPromoCode();
    });

    $('#PromoCode').siblings('.clearlink').mousedown(function () {
       validateVisaPromoCode();
    })

});

function validateVisaPromoCode() {
    var promoCode = $("#PromoCode").val();
    var amount = 0;
    var type = 6;
    validatePromocode(type, amount, promoCode, 'PromoCode', 103);
}


function EncryptCardNo() {
    // perform PIE encryption, and set results in hidden form fields for submission
    maskCardNumber();
    var pan = $("#CardNumber").val();
    var cvv = '000';

    var result = ProtectPANandCVV(pan, cvv);
    if (result != null) {
        protected_pan = result[0];
        protected_cvv = result[1];
        $('#CardNumber').val(protected_pan);
        $('#CVV').val(protected_cvv);
    }
    else if ($("#CardNumber").val()) {
        // Encryption fails if invalid card number used
        $('#MaskCardNumber').val('').focus();
        showExceptionPopupMsg(VisaException.Visa_Enter_Valid_Card);
    }
}

function CardMaintenanceValidation() {

    var prepaidAction = $('#PrepaidAction').val();
    var shippingType = $('#CardShippingType').val();
    var panNumber = $('#CardNumber').val();
    var maskcardNumber = $('#MaskCardNumber').val();
    var cvv = $('#CVV').val();

    if (validateControls())
        CardMaintenance(customerSessionId, prepaidAction, shippingType, panNumber, cvv);
}

function validateControls() {
    var prepaidAction = $('#PrepaidAction').val();
    var shippingType = $('#CardShippingType').val();
    var panNumber = $('#CardNumber').val();
    var maskcardNumber = $('#MaskCardNumber').val();
    var cvv = $('#CVV').val();
    maskcardNumber = maskcardNumber.replace(/-/g, "");

    $('#pan_message').empty();
    $('#MaskCardNumber').val('');
    $('#CardNumber').val('');

    if (prepaidAction == "Select" || prepaidAction == "") {
        $('#visa_message').empty();
        $('#visa_message_error').empty();
        $('#visa_message_error').append("<p style='color:red'>Please Select Action</p>");
        return false;
    }
    else if ((shippingType == "Select" || shippingType == "") && (prepaidAction != "3" && prepaidAction != "0")) {
        $('#shipping_message').empty();
        $('#shipping_message_error').empty();
        $('#visa_error_txt').empty();
        $('#shipping_message_error').append("<p style='color:red'>Please Select Shipping Type</p>");
        return false;
    }
    else if ((maskcardNumber == "") && ($('#pan_div').css('display') != 'none')) {
        $("#MaskedPAN [data-val]").attr("data-val", false);
        $('#pan_validationMessage').hide();
        $('#pan_message').empty();
        $('#pan_message').append("<p style='color:red'>Card number is required</p>");
        if (prepaidAction == "3" || prepaidAction == "0") {
            $('#pan_div').hide();
            $('#shipping_message').empty();
            $('#CardShippingType option:first-child').attr("selected", "selected");
        }
        return false;
    }
        //AL-4657
    else if ((prepaidAction == "3") && (shippingType == "0" || shippingType == "2")) {
        $('#pan_div').hide();
        $('#shipping_message').empty();
        $('#CardShippingType option:first-child').attr("selected", "selected");
        $('#cardSubmit').removeAttr("disabled");
        return false;
    }
    if ((shippingType == "-2") && (prepaidAction == "5" || prepaidAction == "6")) {
        $('#shipping_message_error').empty();
        $('#pan_div').show();
        $('#visa_message').hide();
    }

    return true;
}

function CloseAccount(customerSessionId) {
    $('#divPopUp').css("z-index", -1);
    showSpinner();
    $.ajax({
        data: "{customerSessionId :'" + customerSessionId + "'}",
        dataType: "json",
        type: "POST",
        url: CloseAccountSubmit_Url,
        contentType: "application/json;",
        processData: true,
        success: function (data) {
            hideSpinner();
            if (data == true) {
                RemovePopUp();
                ShowPopUp(successfullCardClosureConformation_URL, 'Message', 410, 150);
            }
            else {
                showExceptionPopupMsg(data.data);
            }
        },
        error: function (result) {
            hideSpinner();
            showExceptionPopupMsg(result.data);
        }
    });
}

function CardMaintenance(customerSessionId, prepaidAction, shippingType, panNumber, cvv) {
    $('#cardSubmit').attr("disabled", "disabled");
    $("#cardCancel").attr("disabled", "disabled");
    var currentCardStatus = $('#CardBalance_CardStatusId').val();
    showSpinner();
    var jsonData = "{customerSessionId:'" + customerSessionId + "'," + "prepaidAction:'" + prepaidAction + "'," + "shippingType:'" + shippingType + "'," + "panNumber:'" + panNumber + "'," + "cvv:'" + cvv + "'}";
    $.ajax({
        data: jsonData,
        datatype: "json",
        type: "POST",
        url: CardMaintenanceSubmit_Url,
        contentType: "application/json;",
        processData: true,
        success: function (data) {
            hideSpinner();
            if (data.success == false) {
                showExceptionPopupMsg(data.data);
                RemovePopUp();
            }
            else {
                if (data == true) {
                    RemovePopUp();
                    if ((prepaidAction == "0" && currentCardStatus == "3") || (prepaidAction == "3" && currentCardStatus == "0")) {
                        ShowPopUpdata(url, newMessage, 390, 160, prepaidAction);
                    }
                    else {
                        ShowPopUpdata(url, newMessage, 390, 160, "");
                    }
                }
            }
            $('#cardSubmit').removeAttr("disabled");
            $("#cardCancel").removeAttr("disabled");
        },
        error: function (data) {
            hideSpinner();
            $('#cardSubmit').removeAttr("disabled");
            $("#cardCancel").removeAttr("disabled");
            showExceptionPopupMsg(data);
        }
    });

}


function GetShippingFee(shippingType, cardBalance, customerSessionId) {
    var jsonData = "{shippingType:'" + shippingType + "', customerSessionId:'" + customerSessionId + "'}";
    $.ajax({
        data: jsonData,
        datatype: "json",
        type: "POST",
        async: false,
        url: shippingType_url,
        contentType: "application/json;",
        processData: true,
        success: function (data) {
            if (data.success == false) {
                showExceptionPopupMsg(data.data);
                RemovePopUp();
            }
            else if (data.Data.success == true) {
                var shippingFee = data.Data.fee;
                var totalFee = shippingFee + charges;
                if (shippingType == '-2') {
                    totalFee = shippingFee;
                }
                if (cardBalance < totalFee) {
                    //$('#visa_message').hide();
                    //$('#shipping_message').hide();
                    $('#visa_error_txt').show();
                    $('#visa_error_txt').empty();
                    $('#pan_message').empty();
                    $('#visa_error_txt').append("<p>Not enough funds to request for replacement of card or shipping charges. Click cancel and load for card replacement.</p>");
                    $('#cardSubmit').attr("disabled", "disabled");
                }
                else {
                    if (shippingType != "-2")
                        $('#visa_message').show();
                    $('#shipping_message').show();
                    $('#visa_error_txt').empty();
                    $('#cardSubmit').removeAttr("disabled");
                }
                if (shippingType != "" && shippingType != "2") {
                    $('#shipping_message').empty();
                    $('#shipping_message').append("<p>" + data.Data.message + "<p>");
                }
            }
        }
    });
}

function RemoveCloseConformationPopUp(url) {
    RemovePopUp();
    RedirectToUrl(url);
}

function GetFundFee(prepaidAction, customerSessionId) {
    var jsonData = "{prepaidAction:'" + prepaidAction + "', customerSessionId:'" + customerSessionId + "'}";
    $.ajax({
        data: jsonData,
        datatype: "json",
        type: "POST",
        async: false,
        url: VisaFee_url,
        contentType: "application/json;",
        processData: true,
        success: function (data) {
            if (data.success == false) {
                showExceptionPopupMsg(data.data);
                RemovePopUp();
            }
            else if (data.Data.success == true) {
                //Don't apply Fee for Action - "Suspended"
                if (prepaidAction != "3") {
                    charges = data.Data.fee;
                }
                $('#visa_message').empty();
                $('#visa_message').append("<p>" + data.Data.message + "<p>");
            }
        },
        error: function (data) {
            showExceptionPopupMsg(data);
        }
    });
}

function OrderCompanionCard() {

    $.ajax({
        data: "{}",
        datatype: "json",
        type: "POST",
        async: false,
        url: OrderCompanionCard_url,
        contentType: "application/json;",
        processData: true,
        success: function (data) {
            if (data.Data.IsPrimaryCardHolder) {
                var customerLookUpURL = CustomerLookUP_Url + '?IsException=false&ExceptionMessage=""&isPrimaryCardHolder=' + data.Data.IsPrimaryCardHolder;
                RedirectToUrl(customerLookUpURL);
            }
            else {
                showExceptionPopupMsg(data.Data.Err_Msg);
            }
        },
        error: function (data) {
            showExceptionPopupMsg(data);
        }
    });
}

function restrictCharactersAlloyNumbers(event) {
    var cardNumberCount = $("#" + event.target.id).val().replace(/ /g, '').length;

    if (cardNumberCount == 16) {
        if (event.keyCode == 13) {
            formatCardNumber();
            return;
        }
        else if (event.keyCode != 8 && event.keyCode != 9 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 46) {
            return false;
        }
    }

    // Allow: backspace, delete, tab, escape, and enter
    if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 ||
        // Allow: Ctrl+A
(event.keyCode == 65 && event.ctrlKey === true) ||
        // Allow: home, end, left, right
(event.keyCode >= 35 && event.keyCode <= 39)) {
        // let it happen, don't do anything		
        return;
    }
    else {
        // Ensure that it is a number and stop the keypress
        if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && event.keyCode != 13) {
            event.preventDefault();
        }
        else if (event.keyCode == 13) {
            $("#MaskCardNumber").focusout();
        }
    }
}
