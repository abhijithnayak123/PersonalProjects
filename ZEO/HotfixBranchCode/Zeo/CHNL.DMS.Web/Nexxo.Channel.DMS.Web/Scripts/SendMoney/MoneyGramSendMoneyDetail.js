$(document).ready(function () {

    $('textarea[placeholder]').placeholder();

    var DestinationAmount = $("#DestinationAmount").val();
    var transferAmount = $("#TransferAmount").val();

    if (IsException) {
        $("#btnSubmit").attr("disabled", true);
        $("#btnUpdate").removeAttr("disabled");
    } else {
        $("#btnSubmit").attr("disabled", false);
        $("#btnUpdate").attr("disabled", true);
    }

    $("#TransferAmount").change(function () {
        if (DestinationAmount > 0) {
            $("#DestinationAmount").val('');
            $("#DestinationAmountWithCurrency1").val('');
        }
    });

    $("#DestinationAmount").change(function () {
        if (transferAmount > 0) {
            $("#TransferAmount").val('');
            $("#TransferAmountWithCurrency").val('');
        }
    });

    $('#TransferAmount').keypress(function (event) {
        restrictCharacters(event);
    });

    $('#DestinationAmount').keypress(function (event) {
        restrictCharacters(event);
    });

    $("#TransferAmount").keyup(function () {
        $("#btnSubmit").attr("disabled", true);
        $("#btnUpdate").removeAttr("disabled");
    });

    $("#CouponPromoCode").keyup(function () {
        $("#btnSubmit").attr("disabled", true);
        $("#btnUpdate").removeAttr("disabled");
    });

    $("#DestinationAmount").keyup(function () {
        $("#btnSubmit").attr("disabled", true);
        $("#btnUpdate").removeAttr("disabled");
    });

    var destval = $('#DestinationAmountWithCurrency1').val();
    if (destval === 'Not Applicable') {
        $('#DestinationAmountWithCurrency1').removeClass("input_box");
        $('#DestinationAmountWithCurrency1').attr('disabled', 'disabled').addClass("disable_txt");
    }

    $('#DestinationAmountWithCurrency1').focus(function () {
        $('#destAmountSymbols').hide();
        $('#destAmount').show();
        $('#DestinationAmount').focus();
    });

    $('#DestinationAmount').blur(function () {
        $('#destAmountSymbols').show();
        $('#destAmount').hide();
        var currencyCode = $('#CurrencyType').val();
        if (currencyCode === null)
            currencyCode = '';
        if ($('#DestinationAmount').val() !== '') {
            var amountModified = $('#DestinationAmount').val() + ' ' + currencyCode;
            $('#DestinationAmountWithCurrency1').val(amountModified);
        } else {
            $('#DestinationAmountWithCurrency1').val('');
        }
    });

    $('#TransferAmountWithCurrency').focus(function () {
        $('#transamtwithSymbols').hide();
        $('#transamt').show();
        $('#TransferAmount').focus();
    });

    $('#TransferAmount').blur(function () {
        $('#transamtwithSymbols').show();
        $('#transamt').hide();
        var amount = $('#TransferAmount').val();
        if (amount !== '') {
            var amountModified = "$ " + parseFloat(amount).toFixed(2) + " USD";
            $('#TransferAmountWithCurrency').val(amountModified);
            $('#dvAmountError').text("").removeClass("field-validation-error");
        }
        else {
            $('#TransferAmountWithCurrency').val('');
        }
    });

    $("#btnSubmit").click(function (e) {
        validateAmount(e);
    });

    $('#btnUpdate').click(function (e) {
        validateAmount(e);
    });

    $('#CurrencyType').change(function () {
        PopulateDeliveryOptions();
    });

    $('#DeliveryMethod').change(function () {
        PopulateReceiveAgents();
    });

    $('#ReceiveAgent').change(function () {
        BindFeeInformation();
    });

    if (receiveCurrency) {
        $('#CurrencyType').val(receiveCurrency);
        PopulateDeliveryOptions();
    }

    $('#divDynamicControl').hide();

    if ($('#IsReceiveAmount').val() && $('#IsReceiveAmount').val() == "True") {
        $('#TransferAmount, #TransferAmountWithCurrency').addClass('disable_txt').attr('readOnly', true);
    } else {
        $('#DestinationAmount, #DestinationAmountWithCurrency1').addClass('disable_txt').attr('readOnly', true);
    }

});   // end of document.ready

function PopulateDeliveryOptions() {
    var selectedCurrencyCode = $('#CurrencyType').val();

    if (selectedCurrencyCode) {
        showSpinner();
        $.ajax({
            type: "POST",
            url: URL_PopulateDeliveryOption,
            dataType: "json",
            contentType: "application/json; charset=UTF-8",
            data: "{currencyCode: '" + selectedCurrencyCode + "'}",
            processData: true,
            success: function (data) {
                if (handleException(data)) {
                    hideSpinner();
                    return;
                }

                deliveryOptionDropdown = $('#DeliveryMethod');
                deliveryOptionDropdown.empty();

                var items = '<option value="">Select</option>';
                if (data) {
                    $.each(data, function (i, deliveryService) {
                        items += '<option value="' + deliveryService.Code + '">' + deliveryService.Name + '</option>';
                    });
                    deliveryOptionDropdown.html(items);
                    hideSpinner();
                }

                if (deliveryOption) {
                    $('#DeliveryMethod').val(deliveryOption);
                    PopulateReceiveAgents();
                }
                else {
                    items = '<option value="">Select</option>';
                    receiveAgentDropdown = $('#ReceiveAgent');
                    receiveAgentDropdown.html(items);
                    receiveAgentDropdown.prop("disabled", true);

                    $('#divDynamicControl').hide();
                }

                hideSpinner();
            },
            error: function (data) {
                showExceptionPopupMsg(defaultErrorMessage);
                hideSpinner();
            }
        });
    }
}

function BindFeeInformation() {
    var selectedCurrencyCode = $('#CurrencyType').val();
    var selectedDeliveryOption = $('#DeliveryMethod').val();
    var selectedReceiveAgent = $('#ReceiveAgent').val();
    var estimatedDestAmtYN = false;

    if (selectedCurrencyCode && selectedDeliveryOption) {
        showSpinner();
        var params = "{currencyCode: '" + selectedCurrencyCode + "', deliveryOption: '" + selectedDeliveryOption + "', receiveAgent: '" + selectedReceiveAgent + "'}";
        $.ajax({
            type: "POST",
            url: URL_BindFeeInformation,
            dataType: "json",
            contentType: "application/json; charset=UTF-8",
            data: params,
            processData: true,
            success: function (feeInfo) {
                if (handleException(feeInfo)) {
                    hideSpinner();
                    return;
                }

                var totalAmount = (feeInfo.Amount + feeInfo.Fee + feeInfo.Tax).toFixed(2);
                var netFee = (feeInfo.Fee + feeInfo.Discount).toFixed(2);
                var receiveCurrency = selectedCurrencyCode;

                $('#Amount').val(totalAmount);
                $('#transamt').val(feeInfo.Amount);
                $('#TransferFee').val(feeInfo.Fee);
                $('#TransferTax').val(feeInfo.Tax);
                $('#PromoDiscount').val(feeInfo.Discount);
                $('#TransferAmount').val(feeInfo.Amount);
                $('#ExchangeRate').val(feeInfo.ExchangeRate);
                $('#ReceiveAgentAbbreviation').val(feeInfo.ReceiveAgentAbbreviation);

                $('#AmountWithCurrency').val('$ ' + totalAmount + ' USD');
                $('#OriginalFee').val(feeInfo.Fee + feeInfo.Discount);
                $('#TransferFeeWithCurrency').val('$ ' + netFee + ' USD');

                if (feeInfo.Discount) {
                    $('#PromoDiscountWithCurrency').val('$ ' + feeInfo.Discount.toFixed(2) + ' USD');
                }

                $('#TransferAmountWithCurrency').val('$ ' + feeInfo.Amount.toFixed(2) + ' USD');
                $('#TransferTaxWithCurrency').val('$ ' + feeInfo.Tax.toFixed(2) + ' USD');
                $('#DestinationAmountWithCurrency1').val(feeInfo.ReceiveAmount.toFixed(2) + ' ' + selectedCurrencyCode);
                $('#destAmount').val(feeInfo.ReceiveAmount);
                $('#DestinationAmount').val(feeInfo.ReceiveAmount);
                $('#ReferenceNo').val(feeInfo.ReferenceNumber);
                $('#ExchangeRateConversion').val('1.00 USD = ' + feeInfo.ExchangeRate.toFixed(4) + ' ' + selectedCurrencyCode);

                if (feeInfo.MetaData["ValidCurrencyIndicator"] != undefined && !feeInfo.MetaData["ValidCurrencyIndicator"]) {
                    if (feeInfo.MetaData["EstimatedReceiveCurrency"] && feeInfo.MetaData["EstimatedExchangeRate"]) {
                        receiveCurrency = feeInfo.MetaData["EstimatedReceiveCurrency"];
                        $('#ExchangeRateConversion').val('1.00 USD = ' + feeInfo.MetaData["EstimatedExchangeRate"].toFixed(4) + ' ' + receiveCurrency);
                        $('#DestinationAmountWithCurrency1').val(feeInfo.ReceiveAmount + ' ' + receiveCurrency);
                    }

                    $('.estimated_amount').show();
                    $('.amount').hide();

                    $('.estimated_tax').show();
		            $('.tax_amount').hide();
		            $('.estimated_fees').show();
		            $('.fee_amount').hide();

		            estimatedDestAmtYN = true;
                } else {
		            $('.amount').show();
		            $('.estimated_amount').hide();

		            if (feeInfo.MetaData["ReceiveTaxesAreEstimated"]) {
		                $('.estimated_tax').show();
		                $('.tax_amount').hide();
		                estimatedDestAmtYN = true;
		            }
		            else {
		                $('.tax_amount').show();
		                $('.estimated_tax').hide();
		            }

		            if (feeInfo.MetaData["ReceiveFeesAreEstimated"]) {
		                $('.estimated_fees').show();
		                $('.fee_amount').hide();
		                estimatedDestAmtYN = true;

		            } else {
		                $('.fee_amount').show();
		                $('.estimated_fees').hide();

		            }
		        }


                if (estimatedDestAmtYN) {
                    $('.estimated_destamount').show();
                    $('.dest_amount').hide();
                }
                else {
                    $('.estimated_destamount').hide();
                    $('.dest_amount').show();
                }

                if (feeInfo.MetaData["OtherFees"] != undefined) {
                    var otherFees = parseFloat(feeInfo.MetaData["OtherFees"]).toFixed(2) + ' ' + receiveCurrency;
                    $('#OtherFees').val(otherFees);
                }

                if (feeInfo.MetaData["OtherTaxes"] != undefined) {
                    var otherTaxes = parseFloat(feeInfo.MetaData["OtherTaxes"]).toFixed(2) + ' ' + receiveCurrency;
                    $('#OtherTaxes').val(otherTaxes);
                }

                //if (document.getElementById("ReceiveAgent").options.length <= 1 || ($('#ReceiveAgent').val() != null && $('#ReceiveAgent').val() != ""))
                showDynamicControls();
                //		        else
                //		            hideSpinner();
            },
            error: function (data) {
                showExceptionPopupMsg(defaultErrorMessage);
                hideSpinner();
            }
        });
    }
}

function showDynamicControls() {
    var amount = $('#transamt').val();
    var countryCode = $('#CountryCode').val();
    var currencyCode = $('#CurrencyType').val();
    var deliveryOption = $('#DeliveryMethod').val();
    var receiveAgent = $('#ReceiveAgent').val();

    var jsonData = "{amount:'" + amount + "', deliverOption:'" + deliveryOption + "',countryCode: '" + countryCode + "',currencyCode: '" + currencyCode + "',receiveAgentId: '" + receiveAgent + "'}";

    $('#divDynamicControl').empty();
    $('#divDynamicControl').hide();

    $.ajax({
        type: "POST",
        url: URL_PopulateDynamicControls,
        contentType: "application/json; charset=UTF-8",
        data: jsonData,
        success: function (data) {
            if (handleException(data)) {
                hideSpinner();
                $("#btnSubmit").prop("disabled", true);
                $("#btnUpdate").prop("disabled", false);
                return;
            }

            $('#divDynamicControl').show();
            $('#divDynamicControl').html(data);

            hideSpinner();
        },
        error: function (data) {
            showExceptionPopupMsg(defaultErrorMessage);
            hideSpinner();
        }
    });
}

function PopulateReceiveAgents() {

    var selectedCurrencyCode = $('#CurrencyType').val();
    var selectedDeliveryOption = $('#DeliveryMethod').val();

    if (selectedCurrencyCode && selectedDeliveryOption) {
        showSpinner();
        var params = "{currencyCode: '" + selectedCurrencyCode + "', deliveryOption: '" + selectedDeliveryOption + "'}";

        $.ajax({
            type: "POST",
            url: URL_BindReceiveAgents,
            dataType: "json",
            contentType: "application/json; charset=UTF-8",
            data: params,
            processData: true,
            success: function (data) {
                if (handleException(data)) {
                    hideSpinner();
                    return;
                }

                receiveAgentDropdown = $('#ReceiveAgent');
                receiveAgentDropdown.empty();
                receiveAgentDropdown.removeAttr("disabled");

                var items = '<option value="">Select</option>';

                if (data && data.length > 0) {
                    $.each(data, function (i, receiveAgent) {
                        items += '<option value="' + receiveAgent.Id + '">' + receiveAgent.Name + '</option>';
                    });
                    receiveAgentDropdown.html(items);

                    if (receiveAgent) {
                        $('#ReceiveAgent').val(receiveAgent);
                        BindFeeInformation();
                    }

                } else {
                    items = '<option value="">Not Available</option>';
                    receiveAgentDropdown.html(items);
                    receiveAgentDropdown.attr("disabled", true);
                    BindFeeInformation();
                }

                hideSpinner();
            },
            error: function (data) {
                showExceptionPopupMsg(defaultErrorMessage);
                hideSpinner();
            }
        });
    }
}

$('#DestinationAmount').focus(function () {
    $('#dvDestAmountError').text("").removeClass("field-validation-error");
});

function validateAmount(e) {
    var amount = $('#TransferAmountWithCurrency').val();
    var actualAmount = $('#TransferAmount').val();
    var actualDestinationAmount = $('#DestinationAmount').val();
    var destinationAmount = $('#DestinationAmountWithCurrency1').val();

    $('#dvAmountError').text("").removeClass("field-validation-error");
    $('#dvDestinationAmountError').text("").removeClass("field-validation-error");

    if (actualAmount > 99999.99) {
        $('#sendMoneyDetailForm').validate();
        $('#dvAmountError').append("You can only send money up to 100000").addClass("field-validation-error");
        e.preventDefault();
        return;
    }

    if (actualDestinationAmount > 999999999.99) {
        $('#sendMoneyDetailForm').validate();
        $('#dvDestAmountError').append("You can only send money up to 1000000000").addClass("field-validation-error");
        e.preventDefault();
        return;
    }

    if ((!amount || amount === '$ 0.00 USD' || amount === '$ 0.0 USD' || amount === '$ 0 USD')) {
        if (!amount || amount === '$ 0.00 USD' || amount === '$ 0.0 USD' || amount === '$ 0 USD') {
            $('#sendMoneyDetailForm').validate();
            $('#dvAmountError').append("Please enter a valid amount").addClass("field-validation-error");
            e.preventDefault();
        }
        else {
            $('#dvAmountError').hide();
            $('#DestinationAmount').hide();
            $('#sendMoneyDetailForm').submit();
        }
    }
}

function restrictCharacters(event) {
    if (event.keyCode === 46 || event.keyCode === 8 || event.keyCode === 9 || event.keyCode === 27 || event.keyCode === 13 || ((event.keyCode === 65 || event.keyCode === 86) && event.ctrlKey === true)) {
        return;
    } else {
        if (event.keyCode < 48 || event.keyCode > 57) {
            event.preventDefault();
        }
    }
}