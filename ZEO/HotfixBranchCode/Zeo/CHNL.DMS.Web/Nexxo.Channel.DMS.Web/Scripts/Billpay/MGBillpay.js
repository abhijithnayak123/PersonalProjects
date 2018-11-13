$(document).ready(function () {
    var selectedBiller = $('#BillerName').val();

    $("#billAmountList").hide();
    $("#Submit").attr("disabled", "disabled").addClass('opaqueViewCart');
    $('#DynamicFields').hide();
    hideSpinner();

    if (Model_AccountAuthMask != "") {
        $("input#AccountNumber").mask(Model_AccountAuthMask);
        $("input#ConfirmAccountNumber").mask(Model_AccountAuthMask);
    }

    if (selectedBiller) {
        if ($("#BillerNotes").val() != '') {
            $("#DeliveryOption").val($("#BillerNotes").val());
        }
        else {
            $("#DeliveryOption").val('');
        }

        if ($("#BillerDenominations").val() != "") {
            var denominations = $("#BillerDenominations").val().split(",");
            LoadDenominations(denominations, varBillAmount1);
        }
    }

    /********************************************************************************/
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
            },
            open: function () {
                var outerDivWidth = $('#BillerName').width();
                var autoCompleteWidth = parseInt(outerDivWidth) + parseInt(outerDivWidth * .35);
                $('.ui-autocomplete').css('width', autoCompleteWidth);
            }
        });
    /*******************************************************************************/

    $('#BillerName').blur(function () {
        if (this.value.length >= 4) {
            ClearBillPayFeeFields();
            $("#AccountNumber").val("");
            $("#ConfirmAccountNumber").val("");
            $('#BillAmount').val("0");
            var index = isAutoCompleteTriggered == 1 ? jQuery.inArray($(this).val(), billers) : 0;
            if (index == -1) {
                $(this).val("");
                return false;
            }
            showSpinners($("#loading"));
            selectedBiller = this.value;
            PopulateBillPayeeAccount(selectedBiller);
        }
        else {
            $(this).val('');
        }
    });

    $("#BillAmount").blur(function () {
        var billpayAmount = $('#BillAmount').val();
        var billerCode = $('#hdnBillerCode').val();
        if (billpayAmount > 0 && billerCode != '') {
            ClearBillPayFeeFields();

            if ($('#AccountNumber').val() != '' && ($('#AccountNumber').val() == $('#ConfirmAccountNumber').val())) {

                PopulateBillpayeeFee(billpayAmount, billerCode, $('#AccountNumber').val());
            }
        }
        else {
            if (billpayAmount <= 0) {
                $(this).val("0");
                ClearBillPayFeeFields();
            }
        }
    });

    $("#BillAmount1").blur(function () {
        var billpayAmount = $('#BillAmount1').val();
        var billerCode = $('#hdnBillerCode').val();
        if (billpayAmount > 0 && billerCode != '') {
            ClearBillPayFeeFields();

            if ($('#AccountNumber').val() != '' && ($('#AccountNumber').val() == $('#ConfirmAccountNumber').val())) {
                PopulateBillpayeeFee(billpayAmount, billerCode, $('#AccountNumber').val());
            }
        }
        else {
            if (billpayAmount <= 0) {
                $(this).val("0");
                ClearBillPayFeeFields();
            }
        }
    });

    $("#BillAmount").focus(function () {
        if (this.value <= 0) {
            $(this).val("");
        }
    });

    if (isException == 'True' && FieldList) {
        showSpinners($("#loading"));
        showDynamicControls();
    }
    else if (isException == 'True') {
        $("#Submit").removeAttr("disabled").removeClass('opaqueViewCart');
    }

    $('#DynamicFields .required').each(function (i, obj) {
        MakeMandatory($("#" + obj.id));
    });

});
//end of $(document).ready() -----------------------------------------------

function ClearBillPayFeeFields() {
    $("#Submit").attr("disabled", "disabled").addClass('opaqueViewCart');
    
    $("#BillPaymentFee").val("0.00");
    $("#DeliveryOption").val("");
    $('#DynamicFields').html("");

    IsException = 'False';
    FieldList = null;
}

function SelectBiller(BillerName) {
    focusSelectedBiller(BillerName);
    showSpinners($("#loading"));
    $('#AccountNumber').val("");
    $('#ConfirmAccountNumber').val("");
    ClearBillPayFeeFields();
    $('#BillAmount').val("0");
    if (BillerName != '') {
        PopulateBillPayeeAccount(BillerName);
    }
}

function PopulateBillPayeeAccount(selectedBillPayee) {
    //Separate the Biller Name and Code
    var selectedBiller = selectedBillPayee.split("/");
    var selectedBillerCode = selectedBiller[1] == null ? selectedBillPayee : selectedBiller[1].trim();

    $('#hdnBillerCode').val(selectedBillerCode);

    $.ajax({
        type: "POST",
        url: URL_PopulateBillpayee,
        dataType: "json",
        contentType: "application/json; charset=UTF-8",
        data: "{billPayeeNameOrCode: '" + selectedBillerCode + "'}",
        processData: true,
        success: function (data) {
            if (handleException(data)) {
                hideSpinner();
                return;
            }
            showCartAbandonmentConfirm = true;

            if (data != null && data != "") {
                focusSelectedBiller(selectedBillPayee);
                if (data.BillerId != null && data.BillerId != "") {
                    $('#BillerId').val(data.BillerId);
                }

                if (data.BillerName) {
                    $('#BillerName').val(data.BillerName);
                }

                if (data.BillerCode) {
                    $('#BillerName').val($('#BillerName').val() + "/" + data.BillerCode);
                    $('#hdnBillerCode').val(data.BillerCode);
                }

                if (data.AccountNumber) {
                    $('#AccountNumber').val(data.AccountNumber);
                    $('#ConfirmAccountNumber').val(data.AccountNumber);
                }
            }
            PopulateBillerMessage(selectedBillerCode);
        },
        error: function () {
            showExceptionPopupMsg(defaultErrorMessage);
            focusSelectedBiller(selectedBillPayee);
            hideSpinner();
        }
    });
}

function PopulateBillerMessage(billerCode) {
    $("#BillAmount1").empty();
    if (billerCode && (typeof val == "undefined")) {
        $.ajax({
            type: "POST",
            url: URL_PopulateBillerMessage,
            dataType: "json",
            contentType: "application/json; charset=UTF-8",
            data: "{billerNameOrCode: '" + billerCode + "'}",
            processData: true,
            success: function (data) {
                if (handleException(data)) {
                    hideSpinner();
                    return;
                }
                if (data.DeliveryOption) {
                    $("#BillerNotes").val(data.DeliveryOption);
                }

                if (data.Denominations != null && data.Denominations.length > 0) {
                    LoadDenominations(data.Denominations, "");
                } else {
                    $("#BillerDenominations").val("");
                    $("#billAmountList").hide();
                    $("#billAmount").show();
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

function LoadDenominations(denominationsarray, selectedValue) {
    $("#billAmount").hide();
    $("#billAmountList").show();
    var denominations = "";
    $("#BillAmount1").append($('<option/>', { value: "", text: "--Select--" }));
    for (var i = 0; i < denominationsarray.length; i++) {
        denominations += "," + denominationsarray[i];
        if (denominationsarray[i] == selectedValue)
            $("#BillAmount1").append($('<option/>', { value: denominationsarray[i], selected: true, text: denominationsarray[i] }));
        else {
            $("#BillAmount1").append($('<option/>', { value: denominationsarray[i], text: denominationsarray[i] }));

        }
    }
    $("#BillerDenominations").val(denominations.substring(1));
    $("#BillerAmount").val("");
}

function PopulateBillpayeeFee(amount, billerCode, accountNumber) {
    showSpinners($("#loading"));
    $.ajax({
        type: "POST",
        url: URL_PopulateBillFee,
        dataType: "json",
        contentType: "application/json; charset=UTF-8",
        data: "{amount: '" + amount + "', billerCode: '" + billerCode + "', accountNumber: '" + accountNumber + "'}",
        processData: true,
        success: function (data) {

            if (handleException(data)) {
                hideSpinner();
                return;
            }

            $("#BillPaymentFee").val(data.DeliveryMethods[0].FeeAmount);

            if ($("#BillerNotes").val() != '') {
                $("#DeliveryOption").val($("#BillerNotes").val());
            }
            else {
                $("#DeliveryOption").val('');
            }
            showDynamicControls();
        },
        error: function () {
            showExceptionPopupMsg(defaultErrorMessage);
            hideSpinner();
        }
    });
}

function showDynamicControls() {
    var billerCode = $('#hdnBillerCode').val();
    var jsonData = "{billerName: '" + billerCode + "'}";

    $.ajax({
        type: "POST",
        url: URL_PopulateDynamicControls,
        contentType: "application/json; charset=UTF-8",
        data: jsonData,
        success: function (data) {
            if (handleException(data)) {
                hideSpinner();
                return;
            }

            $('#DynamicFields').show();
            $('#DynamicFields').html(data);
            $("#Submit").removeAttr("disabled").removeClass('opaqueViewCart');

            $('#DynamicFields .input_box, #DynamicFields .dropdown_box').each(function (i, obj) {
                RemoveMandatory($("#" + obj.id));
            });

            $('#DynamicFields .required').each(function (i, obj) {
                MakeMandatory($("#" + obj.id));
            });
            hideSpinner();
        },
        error: function (data) {
            showExceptionPopupMsg(defaultErrorMessage);
            hideSpinner();
        }
    });
}

function focusSelectedBiller(BillerName) {
    $('.freq_items').addClass('non_sel_freq_item').removeClass('sel_freq_item');
    $('.freq_items').each(function (i) {
        if (this.innerText.trim() == BillerName) {
            var id = '#' + this.id;
            $(id).removeClass('non_sel_freq_item');
            $(id).addClass('sel_freq_item');
        }
    });
}

function MakeMandatory(control) {
    control.rules("add", {
        required: true,
        messages: {
            //            required: 'Required'
        }
    });
}

function RemoveMandatory(control) {
    control.rules("remove","required");
}