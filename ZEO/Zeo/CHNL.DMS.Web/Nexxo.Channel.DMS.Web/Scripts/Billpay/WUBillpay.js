var clickedOnDelFavButton = false;
$(document).ready(function () {
   
    if ($('#BillerName').val() == "")
    { $('#BillerName').focus(); }

    if (Model_AccountAuthMask != "") {
        $("input#AccountNumber").mask(Model_AccountAuthMask);
        $("input#ConfirmAccountNumber").mask(Model_AccountAuthMask);
    }

    var billerLocationName = $('#BillerLocationName').val();
    if (billerLocationName != "") {
        $("#BillerLocation option").each(function () {
            this.selected = $(this).text() == billerLocationName;
        });
    }



    $('#billId').blur(function () {
        if (ViewBag_ErrorMessage !== '') {
            $('#divViewBag').hide();
        } else {
            $('#divViewBag').show();
        }
    });

    $('#confirmbillId').blur(function () {
        if (ViewBag_ErrorMessage !== '') {
            $('#divViewBag').hide();
        } else {
            $('#divViewBag').show();
        }
    });

    $(".bill_pay_desc").hide();
    $(".dynamicField").hide();
    if ($("#BillerDeliveryMethod option:selected").text() != "Select") {
        GetProviderAttributes();
    }
    hideSpinner();;
    var selectedBiller = $('#BillerName').val();

    if (selectedBiller) {
        $(".bill_pay_desc").show();
        PopulateBillerMessage(selectedBiller);
    }

    var isAutoCompleteTriggered = 0;
    var billers = [];
    $('#BillerName').autocomplete(
	{
	    delay: 0,
	    source: URL_AutoCompleteBillPayee,
	    minLength: 4,
	    response: function (event, ui) {
	        billers = [];
	        $.each(ui.content, function (key, value) {
	            billers.push(value.label);
	        });
	        isAutoCompleteTriggered = 1;
	    }
	})
    /*******************************************************************************/
    $('#AccountNumber').bind('keyup', function () {

        // Get the current value of the contents within the text box
        var BillAcctVal = $('#AccountNumber').val().toUpperCase();

        // Reset the current value to the Upper Case Value
        $('#AccountNumber').val(BillAcctVal);

    });
    $('#ConfirmAccountNumber').bind('keyup', function () {

        // Get the current value of the contents within the text box
        var BillConfirmAcctVal = $('#ConfirmAccountNumber').val().toUpperCase();

        // Reset the current value to the Upper Case Value
        $('#ConfirmAccountNumber').val(BillConfirmAcctVal);

    });

    $('#CouponPromoCode, #BillAmount').on('change', function () {
        if ($('#CouponPromoCode').val()) {
            var promoCode = $(this).val().toUpperCase();
            var amount = $('#BillAmount').val();
            var type = 2;

            $(this).val(promoCode);
            validatePromocode(type, amount, promoCode, 'CouponPromoCode', 401);
        }

    });


    $('#BillerName').blur(function () {
        if (this.value.length >= 4) {
            var index = isAutoCompleteTriggered == 1 ? jQuery.inArray($(this).val(), billers) : -1;
            if (isAutoCompleteTriggered == 1 && index == -1) {
                $(this).val("");
                return false;
            }
            showSpinners($("#loading"));
            var selectedBiller = this.value;
            PopulateBillPayeeAccount(selectedBiller, false);
        }
        else {
            $(this).val('');
        }
    });


    $('#BillAmount').blur(function () {
        var billerName = $('#BillerName').val();
        var amt = $('#BillAmount').val();
        var accountNumber = $('#AccountNumber').val()
        if (this.value && !isNaN(this.value) && billerName && accountNumber && amt.length < 8) {
            showSpinners($("#loading"));
            PopulateBillPayeeLocation();
        }
    });

    $('#BillAmount').focus(function () {
        if ($(this).val()) {
            if ($(this).val() == "0") {
                $(this).val('');
            }
        }
    });

    $('#BillerLocation').change(function () {
        var selectedLocation = $("#BillerLocation option:selected").text();
        var billerName = $('#BillerName').val();
        var accountNumber = $('#AccountNumber').val()
        var amount = $('#BillAmount').val();
        if (selectedLocation && selectedLocation != 'Select' && amount && billerName && accountNumber) {
            $('#BillerLocationName').val(selectedLocation);
            showSpinners($("#loading"));
            $("#BillPaymentFee").val('0.00');
            PopulateBilDeliveryMethod();
        }
        else {
            var items = '<option value="">Select</option>';
            $('#BillerDeliveryMethod').html(items);
            $("#BillPaymentFee").val('0.00');
        }
    });

    $('#BillerDeliveryMethod').change(function () {
        var deliveryMethodName = $("#BillerDeliveryMethod option:selected").text();
        if (deliveryMethodName) {
            $("#SelectedDeliveryMethod").val(deliveryMethodName);
            PopulateBillpayeeFee();
            GetProviderAttributes();
        }
    });
    $('#Submit').click(function (event) {
        // IE feature detection
        var isIE9 = document.addEventListener;
        if (isIE9) {
            $('.dynamicField').each(function () {
                if ($(this).css('display') == "block") {
                    if ($(this).find('input').val() == "" && $(this).has('.v_visible').length > 0) {
                        $(this).find('.field-validation-valid').text('Please enter required information field');
                        $(this).find('.field-validation-valid').addClass('field-validation-error');
                        $(this).find('.field-validation-valid').removeClass('field-validation-valid');
                        event.preventDefault();
                    }
                }
            });
        }
    })
    $('#CancelTransactions').click(function () {
        window.location.href = CancelBillPayUrl + "?Id=" + $('#BillPayTransactionId').val();
    });
    $('#BillPayTransactionId').val('0');

    //Regular expression validation expression
    var regexAccountNumber = /^[a-zA-Z0-9\-']*$/;
    var regexAmount = /^[0-9.]*$/;

    $('input#AccountNumber').rules("add", {
        required: true,
        messages: {
            required: 'Please enter valid account number.'
        }
    });

    $('input#AccountNumber').keypress(function (e) {
        ValidateKey(e, regexAccountNumber);
    });

    $('input#ConfirmAccountNumber').rules("add", {
        required: true,
        messages: {
            required: 'Please enter valid account number.'
        }
    });

    $('input#ConfirmAccountNumber').keypress(function (e) {
        ValidateKey(e, regexAccountNumber);
    });

    $('input#BillAmount').keypress(function (e) {
        ValidateKey(e, regexAmount);
    });

    $('#BillerName').siblings('.clearlink')
    .mousedown(function () {
        $('#BillerName').val('');
    })
    .mouseup(function () {
        $('#BillerName').focus();
    });

});

function ValidateKey(e, regex) {
    var enterKey = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (regex.test(enterKey)) {
        return true;
    }
    e.preventDefault();
    return false;
}

function SelectBiller(BillerName) {
    if (clickedOnDelFavButton == false) {
        $('.freq_items').addClass('non_sel_freq_item').removeClass('sel_freq_item');
        $('.freq_items').each(function (i) {
            if ($.trim(this.innerText) == BillerName) {
                var id = '#' + this.id;
                $(id).removeClass('non_sel_freq_item');
                $(id).addClass('sel_freq_item')
            }
        });
        showSpinners($("#loading"));
        if (BillerName != '') {
            PopulateBillPayeeAccount(BillerName, true);
            //PopulateBillerMessage(BillerName);
        }
    }
    else {
        clickedOnDelFavButton = false;
    }
}

function PopulateBillPayeeLocation() {
    var billerName = $('#BillerName').val();
    var accountNumber = $('#AccountNumber').val()
    var amount = $('#BillAmount').val();
    if (amount) {
        $.ajax({
            type: "POST",
            url: URL_PopulateBillPayeeLocation,
            dataType: "json",
            contentType: "application/json; charset=UTF-8",
            data: "{billPayeeName: '" + billerName + "',accountNumber: '" + accountNumber + "', amount: " + amount + "}",
            processData: true,
            success: function (data) {
                if (handleException(data)) {
                    hideSpinner();
                    return;
                }
                locationSelect = $('#BillerLocation');
                locationSelect.empty();
                if (data != null && data.length > 0) {
                    var items = '<option value="">Select</option>';
                    $.each(data, function (i, state) {
                        items += '<option value="' + state.Value + '">' + state.Text + '</option>';
                    });
                    $('#BillerLocation').html(items);
                    $('#BillerLocationRequired').show();
                    $("#BillerLocation").rules("add", {
                        required: true,
                        messages: {
                            required: 'Please select a valid location.'
                        }
                    });
                    hideSpinner();
                } else {
                    var items = '<option value="">Not Applicable</option>';
                    $('#BillerLocation').html(items);
                    $('#BillerLocationRequired').hide();
                    $('#BillerLocation').rules("remove", "required");
                    $('#ValidationMsgForBillerLocation').text('');
                    PopulateBilDeliveryMethod();
                }

            },
            error: function () {
                showExceptionPopupMsg(defaultErrorMessage);
                hideSpinner();
            }
        });
    }
}

function PopulateBilDeliveryMethod() {
    var billerName = $('#BillerName').val();
    var accountNumber = $('#AccountNumber').val()
    var amount = $('#BillAmount').val();
    var locationType = $('#BillerLocation').val();
    var location = $("#BillerLocation option:selected").text();
    $.ajax({
        type: "POST",
        url: URL_PopulateBillDeliveryMethod,
        dataType: "json",
        contentType: "application/json; charset=UTF-8",
        data: "{billPayeeName: '" + billerName + "',accountNumber: '" + accountNumber + "', amount: " + amount + ", location: '" + location + "',locationType: '" + locationType + "'}",
        processData: true,
        success: function (data) {
            if (handleException(data)) {
                hideSpinner();
                return;
            }
            deliveryyMethodSelect = $("#BillerDeliveryMethod");
            deliveryyMethodSelect.empty();
            var items = '<option value="">Select</option>';
            if (data != null && data != '') {
                $.each(data.DeliveryMethods, function (i, state) {
                    items += '<option value="' + state.Value + '">' + state.Text + '</option>';
                });
                $('#AvailableBalance').val(data.avaliableBalance);
                $('#AccountHolder').val(data.accountHolderName);
                $('#BillerDeliveryMethod').html(items);
                $('#SessionCookie').val(data.SessionCookie);
                hideSpinner();
                $('#BillPayTransactionId').val(data.TransactionID.toString());
            }
        },
        error: function () {
            showExceptionPopupMsg(defaultErrorMessage);
            hideSpinner();
        }
    });
}

function PopulateBillpayeeFee() {
    showSpinners($("#loading"));
    var billpayDeliveryMethod = $('#BillerDeliveryMethod').val();
    $.ajax({
        type: "POST",
        url: URL_PopulateBillFee,
        dataType: "json",
        contentType: "application/json; charset=UTF-8",
        data: "{deliveryMethod: '" + billpayDeliveryMethod + "'}",
        processData: true,
        success: function (data) {
            if (handleException(data)) {
                hideSpinner();
                return;
            }
            $("#BillPaymentFee").val(data.toFixed(2));
        },
        error: function () {
            showExceptionPopupMsg(defaultErrorMessage);
            hideSpinner();
        }
    });
}

function PopulateBillerMessage(billerName) {
    if (billerName && (typeof val == "undefined")) {
        $.ajax({
            type: "POST",
            url: URL_PopulateBillerMessage,
            dataType: "json",
            contentType: "application/json; charset=UTF-8",
            data: "{billerNameOrCode: '" + billerName + "'}",
            processData: true,
            success: function (data) {
                if (handleException(data)) {
                    hideSpinner();
                    return;
                }
                if (data.Result.Message) {
                    $(".bill_pay_desc").show();
                    $("#billerMessage").html(data.Result.Message);
                }
                else {
                    $(".bill_pay_desc").hide();
                }
                hideSpinner();
            },
            error: function () {
                showExceptionPopupMsg(defaultErrorMessage);
                hideSpinner();
            }
        });
    } else {
        hideSpinner();
    }
}

function GetProviderAttributes() {
    var billerName = $('#BillerName').val();
    var location = $("#BillerLocation option:selected").text();
    var jsonData = "{billerName: '" + billerName + "', location: '" + location + "'}";
    $.ajax({
        type: "POST",
        url: URL_GetProviderAttributes,
        dataType: "json",
        contentType: "application/json; charset=UTF-8",
        data: jsonData,
        processData: true,
        success: function (data) {
            if (handleException(data)) {
                hideSpinner();
                return;
            }
            DisplayDynamicFields(data);
            hideSpinner();
        },
        error: function () {
            showExceptionPopupMsg(defaultErrorMessage);
            hideSpinner();

        }
    });
}

function MakeMandatory(control) {
    control.rules("add", {
        required: true,
        messages: {
            required: 'Please enter required information in field'
        }
    });
}

function DisplayDynamicFields(fields) {
    $(".dynamicField").hide();
    if (fields) {
        $.each(fields, function (i, field) {
            if (field.Label == "Attention") {
                $('#dvAttention').show();
                $('#Attention').attr('placeholder', field.ValidationMessage);
                if (field.IsMandatory == true) {
                    $('#spanAttention').addClass('v_visible');
                    var control = $("#Attention");
                    MakeMandatory(control);
                }
            }
            else if (field.Label == "Date Of Birth") {
                $('#dvDateOfBirth').show();
                $('#DateOfBirth').attr('placeholder', field.ValidationMessage);
                if (field.IsMandatory == true) {
                    $('#spanDateOfBirth').addClass('v_visible');
                    var control = $("#DateOfBirth");
                    MakeMandatory(control);
                }
            }
            else if (field.Label == "Available Balance") {
                $('#dvAailableBalance').show();
                var control = $("#AvailableBalance");

                control.addClass('disable_txt')
					.attr('readonly', 'true')
					.attr('placeholder', field.ValidationMessage);
                if (field.IsMandatory == true) {
                    $('#spanAailableBalance').addClass('v_visible');
                    MakeMandatory(control);
                }
            }
            else if (field.Label == "Account Holder") {
                $('#dvAccountHolder').show();
                var control = $("#AccountHolder");
                control.addClass('disable_txt')
					.attr('readonly', 'true')
					.attr('placeholder', field.ValidationMessage);
                if (field.IsMandatory == true) {
                    $('#spanAccountHolder').addClass('v_visible');
                    MakeMandatory(control);
                }
            }
            else if (field.Label == "Reference #") {
                $('#dvReference').show();
                $('#Reference').attr('placeholder', field.ValidationMessage);
                if (field.IsMandatory == true) {
                    $('#spanReference').addClass('v_visible');
                    var control = $("#Reference");
                    MakeMandatory(control);
                }
            }
        });

        resetCrossMark();
    }
    $('input[placeholder]').placeholder();
}

function PopulateBillPayeeAccount(selectedBillPayee, isClickedOnFavouriteBiller) {
    $.ajax({
        type: "POST",
        url: URL_PopulateBillpayee,
        dataType: "json",
        contentType: "application/json; charset=UTF-8",
        data: "{billPayeeNameOrCode: '" + selectedBillPayee + "'}",
        processData: true,
        success: function (data) {
            if (handleException(data)) {
                hideSpinner();
                return;
            }
            showCartAbandonmentConfirm = true;
            resetValidation();
            if (data != null && data.Result != null) {
                if (data.Result.ProviderName != null && data.Result.ProviderName != "") {
                    $('#BillerId').val(data.Result.BillerId);
                    if (data.Result.BillerName) {
                        $('#BillerName').val(data.Result.BillerName);
                        $('#hdnBillerName').val(data.Result.BillerName);
                    }
                    if (data.Result.AccountNumber) {
                        $('#AccountNumber').val(data.Result.AccountNumber);
                        $('#ConfirmAccountNumber').val(data.Result.AccountNumber);
                    }
                    $("#BillAmount").val('0');
                    var items = '<option value="">Select</option>';
                    $('#BillerLocation').html(items);
                    $('#BillerDeliveryMethod').html(items);
                    $("#BillPaymentFee").val('0.00');
                    $("#CouponPromoCode").val('');
                    $("#DateOfBirth").val('');
                    $("#AccountHolder").val('');
                    $("#Reference").val('');
                    $("#Attention").val('');
                    $('#dvAttention').hide();
                    $('#dvReference').hide();
                    $('#dvAccountHolder').hide();
                    $('#dvAailableBalance').hide();
                    $('#dvDateOfBirth').hide();
                    PopulateBillerMessage(selectedBillPayee);
                }
            }
        },
        error: function () {
            showExceptionPopupMsg(defaultErrorMessage);
            $('.freq_items').addClass('non_sel_freq_item').removeClass('sel_freq_item');
            $('.freq_items').each(function (i) {
                if ($.trim(this.innerText) == selectedBillPayee) {
                    var id = '#' + this.id;
                    $(id).removeClass('non_sel_freq_item');
                    $(id).addClass('sel_freq_item')
                }
            });
            hideSpinner();
        },
        complete: function () {
            /********************************************************************************/
            //        User Story Number: AL-6188 | Only Web | 
            //        Developed by: Sunil Shetty
            //        Date: 11.04.2016
            //        Purpose: On clicking select biller and then tabout from biller name textbox the biller name used to get empty. To avoid it we are calling automcompelete on Clicking of Favriote biller
            if (isClickedOnFavouriteBiller == true) {
                //$('#BillerName').autocomplete("search");
            }
        }
    });
}

function ImportPastBiller() {
    var productName = "biller";
    $("#loadingmsgspinner").text("Importing Favorite Billers......");
    showSpinner();
    $.ajax({
        type: "POST",
        url: URL_ImportPastBiller,
        contentType: "application/json; charset=UTF-8",
        data: "{productName: '" + productName + "'}",
        processData: true,
        success: function (data) {
            if (handleException(data)) {
                hideSpinner();
                return;
            }
            if (data != '') {
                $('#FrequentBillers').html(data);
                $(".bill_pay_desc").hide();
            }
            hideSpinner();
        },
        error: function (data) {
            if (data != '') {
                $('#FrequentBillers').html(data);
                $(".bill_pay_desc").hide();
            }
            hideSpinner();
        }
    });
}
//Begin TA-191 Changes
//       User Story Number: TA-191 | Web |   Developed by: Sunil Shetty     Date: 21.04.2015
//       Purpose: The user / teller will have the ability to delete a specific biller from the Favorite Billers list. clickedOnDelFavButton flag is used to know the place of call
// if clickedOnDelFavButton is false it means the call was from biller name and if true the call is from Favorite Billers delete button and the call is made to DisplayDeleteFavBiller in WesternUnionBillPaymentController
function DeleteFavBiller(billerId) {
    $('#divDeletefavBillerDialog').dialog('destroy').remove();
    showSpinner();
    $.ajax({
        type: "POST",
        url: DeleteFavoriteBiller_URL,
        contentType: "application/json; charset=UTF-8",
        data: "{billerId: '" + billerId + "'}",
        processData: true,
        success: function (data) {
            if (data.data) {
                $('#FrequentBillers').html();
                $(".bill_pay_desc").hide();
                showExceptionPopupMsg(data.data);
            }
            else {
                $('#FrequentBillers').html(data);
                $(".bill_pay_desc").hide();
            }
            hideSpinner();
        },
        error: function (data) {
            if (data != '') {
                $('#FrequentBillers').html();
                $(".bill_pay_desc").hide();
                showExceptionPopupMsg(data.data);
            }
            hideSpinner();
        }
    });
}
function btnFavBillerDeleteClick(control) {

    var billerId = control.id.split("_")[1];
    var $confirmation = $("<div id='divDeletefavBillerDialog'></div>");
    $confirmation.empty();
    $confirmation.dialog({
        autoOpen: false,
        title: "ZEO",
        width: 480,
        draggable: false,
        modal: true,
        resizable: false,
        closeOnEscape: false,
        minHeight: 125,
        scroll: false,
        cache: false,
        open: function (event, ui) {
            $confirmation.load(DelFavBillerMsgPopup_URL, { id: billerId }, function () {
                $('#btnYes').focus();
            });

        }
    });
    $confirmation.dialog("open")
    clickedOnDelFavButton = true;
}
// no where using.. so presently commented.
//function SelectPayee(billPayeeName) {
//	RedirectToUrl(billPayment + '?billPayeeName=' + encodeURIComponent(billPayeeName));
//}

function resetValidation() {
    //Removes validation message after input-fields
    $('.field-validation-error').addClass('field-validation-valid').removeClass('field-validation-error');
}
