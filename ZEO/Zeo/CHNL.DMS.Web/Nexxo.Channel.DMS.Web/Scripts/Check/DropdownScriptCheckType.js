$(document).ready(function () {

    var canOverridePromo = true; 
    $("#CheckDate").rules("add", {
        sqlMinDateValidation: { minvalue: '01/01/1753' },
        messages: {
            sqlMinDateValidation: '_NexxoTranslate(CheckSqlMinDateValidation)'
        }
    });

    $("#CheckAmount").on('change', function () {
        var checkAmount = $('#CheckAmount').val();
        var productProviderCode = $('#ProductProviderCode').val();
        if (productProviderCode === 'TCFCheck' && parseFloat(maxOnUsCheckAmount) < parseFloat(checkAmount)) {
            getCheckTypes(200);
        }
        else if (productProviderCode === 'TCFCheck' && parseFloat(maxOnUsCheckAmount) > parseFloat(checkAmount)) {
            getCheckTypes(202);
        }

        //Clearing the Check Type here because if we change the amount, All Check types will be loaded once again. So we need to clear all the Fee as well.
        $('#CheckType').val('');
        getApplicablePromotion();
    });

    $("#CheckType").on('change', function () {
        getApplicablePromotion();
    });
});

function getApplicablePromotion() {
    var selectedCheckTypeId = $('#CheckType').val();
    var checkAmount = $('#CheckAmount').val();
    var isSystemapplied = $('#IsSystemApplied').val();
    var productProviderCode = $('#ProductProviderCode').val();

    if ((checkAmount == "" || checkAmount == "0" || checkAmount == "0.00") || selectedCheckTypeId == '') {
        $('#CheckEstablishmentFee').val('');
        $('#BaseFee').val('0');
        $('#NetFee').val('0');
        $('#DiscountName').val('');
        $('#DiscountApplied').val('0');
        $('#BaseFeeWithCurrency').val('$ 0');
        $('#DiscountAppliedWithCurrency').val('$ 0');
        $('#NetFeeWithCurrency').val('$ 0');
        return;
    }

    showSpinner();
    $.ajax({
        type: "GET",
        url: base_url_GetCheckFee,
        data: { CheckTypeId: selectedCheckTypeId, CheckAmount: checkAmount, promotionCode: "", productProviderCode: productProviderCode, IsSystemApplied: isSystemapplied },
        dataType: "json",
        contentType: "application/json; charset=UTF-8",
        success: function (data) {
            if (handleException(data)) {
                hideSpinner();
                return;
            }
            promotionCodes = $('select#PromotionCode');
            bindDropdownListData(promotionCodes, data.data);
            $('#ManualPromocode').val('');
            $(".manual_entry").hide();
            $('.manual_promo span').text('');
            hideSpinner();
        },
        error: function () {
            showExceptionPopupMsg(defaultErrorMessage);
            hideSpinner();
        }
    });
}

function bindDropdownListData(dropdownList, list) {
    dropdownList.empty();
    dropdownList.prop("disabled", false);
    var items = '';
    var promoName = 'NONE';
    if (list.length > 0) {
        $.each(list, function (i, item) {
            if (item.Selected) {
                dropdownList.prop("disabled", true);
                items += '<option value="' + item.Value + '" selected>' + item.Text + '</option>';
                promoName = item.Value;
            }
            else {
                items += '<option value="' + item.Value + '">' + item.Text + '</option>';
            }
        });
    }
    else {
        items = '<option value="">Not Applicable</option>';
    }
    dropdownList.html(items);
    getCheckFee(promoName);
}

function getCheckTypes(providerId) {

    showSpinner();
    $.ajax({
        url: getCheckType_URL,
        data: { providerId: providerId }, 
        type: 'GET',
        datatype: 'json',
        success: function (checkTypes) {
            if (handleException(checkTypes)) {
                hideSpinner();
                return;
            }
            checkDropDown = $('select#CheckType');
            checkDropDown.attr("disabled", false);
            bindDropdownList(checkDropDown, checkTypes);
            hideSpinner();
        },
        error: function () {
            showExceptionPopupMsg(defaultErrorMessage);
            hideSpinner();
        }
    });
}
