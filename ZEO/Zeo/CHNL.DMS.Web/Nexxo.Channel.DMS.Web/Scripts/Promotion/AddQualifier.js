$(document).ready(function () {
    qualifierGrid();

    if (disableTrainStop === 'True') {
        $('#qualifierSummaryTrainStop').addClass('isDisabled');
    }
    else {
        $('#qualifierSummaryTrainStop').removeClass('isDisabled');
    }

    AppendDollor('TransactionAmount', 'TransactionAmountWithCurrency', '$ ', ' USD');

    $("#MinimumTrxCount").keypress(function (e) {
        ValidateText(e);
    });

    $("#TransactionAmount").keypress(function (e) {
        ValidateText(e);
    });

    $("#TransactionAmount").on('change blur', function () {
        AppendDollor('TransactionAmount', 'TransactionAmountWithCurrency', '$ ', ' USD');
    });

    $("#TransactionAmountWithCurrency").focus(function () {
        FocusAppendTextBox('TransactionAmount', 'TransactionAmountWithCurrency');
    });

    GetSelectedTxnStates();

    $("#trxenddate").datepicker({
        inline: true,
        dateFormat: 'mm/dd/yy',
        showOtherMonths: true,
        dayNamesMin: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
        minDate: promoStartDate,
        maxDate: promoEndDate,
        changeMonth: true,
    });

    $('.transactionStates').multiselect({
        placeholder: 'Select',
        search: true,
        searchOptions: {
            'default': 'Search'
        },
        minHeight: 170,
        maxHeight: 170,
        maxWidth: 170,
        minWidth: 168,
        selectAll: true,
        onControlClose: function (element) {
            GetSelectedTxnStates();
        },
        onSelectAll: function (element) {
            GetSelectedTxnStates();
        }
    });
    
    $('.transactionStates').multiselect('loadOptions', selectedTxnStates);

    $("#qualifierdelete").live('click', function (e) {
        var dataHref = this.getAttribute('data-href');
        ShowPopUp(dataHref, "SYSTEM MESSAGE", 400, 125);
    });


    $("#QualifierProduct, #TransactionAmount, #MinimumTrxCount, #TrxState").live('change', function (e) {
        qualifierValidation();
    });

    $("#qualifierForm").submit(function (e) {
        if (qualifierValidation()) {
            return true;
        }
        else {
            return false;
        }
    });

});

function qualifierValidation() {
    var qualifierProduct = $("#QualifierProduct").val();
    var trxAmount = $("#TransactionAmount").val();
    var minTrxCount = $("#MinimumTrxCount").val();
    var trxStates = $("#TrxState").val();
    if ((qualifierProduct != undefined && qualifierProduct != '') && ((trxAmount != '') || (minTrxCount != '')) && (trxStates != undefined && trxStates != '')) {
        $("#qualifier_summary_error").hide();
        return true;
    }
    else {
        $("#qualifier_summary_error").html('Please select qualifier product, Transaction state and enter transaction Amount or Count to add or update qualifier.');
        $("#qualifier_summary_error").show();
        return false;
    }
        
}

function DeleteQualifier(id) {
    showSpinner();
    $.ajax({
        type: "GET",
        url: QualifierDelete_Url + '?qualifierId=' + id,
        dataType: "json",
        contentType: "application/json; charset=UTF-8",
        data: {},
        success: function (data) {
            if (data.success) {
                RemovePopUp();
                $("#jqQualifier").trigger("reloadGrid");
            }
            hideSpinner();
        },
        error: function () {
            showExceptionPopupMsg(defaultErrorMessage);
            hideSpinner();
        }
    });
}

function GetSelectedTxnStates() {

    var txnStates = $('select[multiple].transactionStates').val();

    if (txnStates !== null)
        $("#SelectedTxnStates").val(txnStates.join(","));
    else
        $("#SelectedTxnStates").val('');
}

function SaveQualifier() {
    $("#qualifierForm").submit();
}

function ShowPageLeavePopup(isQualifier) {
    if (ValidateQualifier())
    {
        window.location.href = ProvisionSummaryRedirectUrl + '?isQualifier=' + isQualifier;
    }
    else {
        if (qualifierValidation())
            ShowPopUpdataMinHeight(PageLeavePopup_Url, "Confirmation", 516, 200, isQualifier);
    }
}

function ValidateQualifier() {
    var qualifierProduct = $("#QualifierProduct").val();
    var trxAmount = $("#TransactionAmount").val();
    var minTrxCount = $("#MinimumTrxCount").val();
    var trxStates = $("#SelectedTxnStates").val();

    if (qualifierProduct == '' && trxAmount == '' && minTrxCount == '' && trxStates == '') {
        $("#qualifier_summary_error").hide();
        return true;
    }
    return false;
}
