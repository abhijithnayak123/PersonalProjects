$(document).ready(function () {

    clearCardSearchFields();

    $("#MaskCardNumber").keydown(function (event) {

        cardSwipeTrackData = cardSwipeTrackData + event.char;

        if ((cardSwipeTrackData.length == 1 && cardSwipeTrackData.substring(0, 1) != '%') || (cardSwipeTrackData.length == 2 && cardSwipeTrackData.substring(0, 2) != '%B')) {
            cardSwipeTrackData = '';
        }

        if (event.keyCode == 8 && $("#MaskCardNumber").val().indexOf('*') != -1) {
            clearCardSearchFields();
        }        

        restrictCharacters(event);
    });

    $("#MaskCardNumber").focusout(function (event) {
        var cardNumber = $("#MaskCardNumber").val();
        if (!($("#MaskCardNumber").attr('readonly') == 'readonly') && cardNumber.indexOf('*') == -1 && cardNumber.length >= 16) {
            setMaxLength(19);
            maskCardNumber();
            cardSwipeTrackData = '';
            // perform PIE encryption, and set results in hidden form fields for submission
            var pan = $("#CardNumber").val();
            var cvv = '000';
            getCardTypeByBIN(pan);
            var result = ProtectPANandCVV(pan, cvv);
            if (result != null) {
                // we only need the PAN for this demo
                protected_pan = result[0];
                protected_cvv = result[1];
                $('#CardNumber').val(protected_pan);
                $('#CVV').val(protected_cvv);
            }
            else if ($("#CardNumber").val()) {
                $('#MaskCardNumber').val('').focus();
                getMessage(CustomerExceptions.Customer_Card_Error);
                clearCardSearchFields();
                return;
            }
            else if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                event.preventDefault();
            }
            if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                event.preventDefault();
            }
        }
    });

    $("#MaskCardNumber").keyup(function (event) {
        disableEnableSubmit();
    });
});

function clearCardSearchFields() {
    $("#MaskCardNumber").val('');
    $("#CardNumber").val('');
    $("#submitCardData").addClass('DisableButtons').attr('disabled', 'disabled');
}

function getCardTypeByBIN(cardNumber) {
    $.ajax({
        url: CardBINs_URL,
        data: { cardNumber: cardNumber },
        type: 'POST',
        datatype: 'json',
        async: false,
        success: function (jsonData) {
            if (!jsonData.IsZeoCard) {
                getMessage(CardSwipeExceptions.Not_ZeoCard_Error);
                clearCardSearchFields();
            }
            else {
                $("#submitCardData").removeClass('DisableButtons').attr('disabled', false).focus();
            }
        },
        error: function (err) {
            showExceptionPopupMsg(err.data);
        }
    });
}

function disableEnableSubmit() {
    
    if ($("#MaskCardNumber").val().length >= 16) {
        $("#submitCardData").removeClass('DisableButtons').attr('disabled', false);
    }
    else {
        $("#submitCardData").addClass('DisableButtons').attr('disabled', 'disabled');
    }
}