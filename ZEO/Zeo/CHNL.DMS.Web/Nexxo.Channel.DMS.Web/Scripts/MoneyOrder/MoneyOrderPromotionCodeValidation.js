$(document).ready(function () {
    var promoCode = $('#PromotionCode').val();
    var isValid = false;
    if (promoCode != '') {
        isValid = true;
    }
    $('#PromotionCode').change(function () {
        var promoCode = $('#PromotionCode').val();
        if (promoCode.toLowerCase() === "other") {
            $(".manual_entry_mo").show();
            $('.manual_promo span').text('');
            $("#btnSubmit").attr('disabled', true);
        }
        else {
            $(".manual_entry_mo").hide();
            $('.manual_promo span').text('');
            $("#btnSubmit").removeAttr('disabled');
            CalculateMoneyOrderFeeByPromoCode();
        }
    });

    $("#txtAmount").change(function () {
        var amount = $('#txtAmount').val();
        var promoCode = $('#PromotionCode').val();
        var isSystemapplied = $('#IsSystemApplied').val();

        if ((amount == '' || amount == '0' || amount == '0.00')) {

            $('#BaseFee').val('0');
            $('#PromotionCode').val('');
            $('#DiscountApplied').val('0');
            $('#NetFee').val('$ 0.00');
            $('#promodiv span').hide();
            return;
        }

        getMoneyOrderFee();
    });

    $('#ManualPromocode').live('change', function (e) {
        getFeeBasedOnPromoCode();
    });

    $("#txtAmount").decimalOnly();
    $("#txtAmount").focus();

    $('#btnSubmit').click(function (e) {
    		$("form").submit();
    });

    setTimeout(function () {
        $('#ManualPromocode').siblings('.clearlink').mousedown(function () {
            getFeeBasedOnPromoCode();
        });
    }, 10);
});

function getFeeBasedOnPromoCode() {
    var promotionCode = $('#ManualPromocode').val();
    var amount = $('#txtAmount').val();
    if (promotionCode != '') {
        showSpinner();
        $.ajax({
            type: 'GET',
            url: GetFee_URL_PC,
            data: { promotionCode: promotionCode, amount: amount, },
            dataType: 'json',
            success: function (result) {
                if (result.success == true) {
                    $('#ManualPromocode').val(result.data.PromotionCode);
                    var discountDesc = result.data.PromotionCode == null ? 'Not Applicable' : result.data.PromotionDescription
                    var baseFee = parseFloat(Math.round(result.data.BaseFee * 100) / 100).toFixed(2);
                    var netFee = parseFloat(Math.round(result.data.NetFee * 100) / 100).toFixed(2);
                    $('#DiscountName').val(discountDesc);
                    $('#DiscountApplied').val(result.data.DiscountApplied);
                    $('#DiscountAppliedWithCurrency').val(result.data.DiscountApplied);
                    $('#NetFee').val(result.data.NetFee);
                    $('#Fee').val(result.data.NetFee);
                    $('#txtFee').val(result.data.NetFee);
                    $('#BaseFee').val(result.data.BaseFee);
                    $('#BaseFeeWithCurrency').val('$ ' + baseFee);
                    $('#NetFeeWithCurrency').val('$ ' + netFee);
                    $('#IsSystemApplied').val(result.data.IsSystemApplied);
                    $("#btnSubmit").removeAttr('disabled');
                    isValid = true;
                    TotalAmount();
                    // Toupper();
                    $('.manual_promo span').text('');
                }
                else {
                    $('.manual_promo span').text(result.errorMessage);
                    $("#btnSubmit").attr('disabled', true);
                    isValid = false;
                }
                hideSpinner();
            },
            error: function (result) {
                showExceptionPopupMsg(result.data);
                isValid = false;
            }
        });
    }
    else {
        $("#btnSubmit").attr('disabled', true);
    }
}

//function Toupper() {
//    var promoCode = $('#PromotionCode').val().toUpperCase();
//    $('#PromotionCode').val(promoCode);
//}

function getMoneyOrderFee() {
    promoCode = $('#PromotionCode').val();
    var amount = $('#txtAmount').val();
    var isSystemapplied = $('#IsSystemApplied').val();

    showSpinner();

    $.ajax({
        url: url_GetMoneyOrderFee,
        data: { moneyOrderAmount: amount, promotionCode: promoCode, IsSystemApplied: isSystemapplied },
        type: 'GET',
        datatype: 'json',
        success: function (data) {
            if (handleException(data)) {
                hideSpinner();
                return;
            }
            promotionCodes = $('select#PromotionCode');
            bindDropdownListData(promotionCodes, data.data);
            $('.manual_promo span').text('');
            $('#ManualPromocode').val('');
            $(".manual_entry_mo").hide();
            hideSpinner();
        }
    });

}

function CalculateMoneyOrderFeeByPromoCode() {
    promoCode = $('#PromotionCode').val();

    $.ajax({
        type: 'GET',
        url: MO_Promotion_Url,
        data: { promocode: promoCode },
        dataType: 'json',
        success: function (result) {
            if (result.success == true) {
                $('#PromotionCode').val(result.data.PromotionCode);
                var discountDesc = result.data.PromotionCode == null ? 'Not Applicable' : result.data.PromotionDescription
                var baseFee = parseFloat(Math.round(result.data.BaseFee * 100) / 100).toFixed(2);
                var netFee = parseFloat(Math.round(result.data.NetFee * 100) / 100).toFixed(2);
                $('#DiscountName').val(discountDesc);
                $('#DiscountApplied').val(result.data.DiscountApplied);
                $('#DiscountAppliedWithCurrency').val(result.data.DiscountApplied);
                $('#NetFee').val(result.data.NetFee);
                $('#Fee').val(result.data.NetFee);
                $('#txtFee').val(result.data.NetFee);
                $('#BaseFee').val(result.data.BaseFee);
                $('#BaseFeeWithCurrency').val('$ ' + baseFee);
                $('#NetFeeWithCurrency').val('$ ' + netFee);
                $('#IsSystemApplied').val(result.data.IsSystemApplied);
                $("#btnSubmit").removeAttr('disabled');
                $('#ManualPromocode').val('');
                $('.manual_promo span').text('');
                isValid = true;
                TotalAmount();
                // Toupper();
            }
            else {
                showExceptionPopupMsg(result.data);
                isValid = false;
            }
        },
        error: function (result) {
            showExceptionPopupMsg(result.data);
            isValid = false;
        }
    });
}

function bindDropdownListData(dropdownList, list) {
    dropdownList.empty();
    dropdownList.prop("disabled", false);
    var items = '';
    if (list.length > 0) {
        $.each(list, function (i, item) {
            if (item.Selected) {
                dropdownList.prop("disabled", true);
                items += '<option value="' + item.Value + '" selected>' + item.Text + '</option>';
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
    CalculateMoneyOrderFeeByPromoCode();
}

function TotalAmount() {
    var Amount = $("#txtAmount").val();
    if (!(isNaN(Number(Amount))) && Amount != "") {
        var total = Number(Amount) + Number($("#txtFee").val());
        $("#txtTotal").val(total.toFixed(2));
    }
    else {
        $("#txtTotal").val("0.00");
    }
}
