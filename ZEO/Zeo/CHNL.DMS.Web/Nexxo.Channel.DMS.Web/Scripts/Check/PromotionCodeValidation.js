$(document).ready(function () {

    $('#PromotionCode').change(function () {
        var promoCode = $('#PromotionCode').val();
        if (promoCode == '') {
            promoCode = 'NONE';
        }
        if (promoCode.toLowerCase() === "other") {
            $(".manual_entry").show();
            $('.manual_promo span').text('');
            $("#btnSubmit").attr('disabled', true);
        }
        else {
            $(".manual_entry").hide();
            $('.manual_promo span').text('');
            $("#btnSubmit").removeAttr('disabled');
            getCheckFee(promoCode);
        }
    });

    $('#btnSubmit').click(function (e) {
        if (CheckAmount()) {
            ShowPopUpMinHeight(WarrningPopUp_URL, 'Message', 373, 120);
            return false;
        }
        $('select#PromotionCode').prop("disabled", false);
        $("form").submit();
    });

    $('#ManualPromocode').live('change', function (e) {
        GetFeeBasedOnPromo();
    });

    setTimeout(function () {
        $('#ManualPromocode').siblings('.clearlink').mousedown(function () {
            GetFeeBasedOnPromo();
        });
    }, 10);
});

function getCheckFee(promoCode) {
    if (promoCode != '') {
        showSpinners($("#loading"));
        $.ajax({
            type: 'GET',
            url: Calculate_Fee_URL,
            data: { promoCode: promoCode },
            dataType: 'json',
            success: function (result) {
                if (result.success == true) {
                    var baseFee = parseFloat(Math.round(result.data.BaseFee * 100) / 100).toFixed(2);
                    var discountApplied = parseFloat(Math.round(result.data.DiscountApplied * 100) / 100).toFixed(2);
                    var netFee = parseFloat(Math.round(result.data.NetFee * 100) / 100).toFixed(2);
                    $('#BaseFee').val(result.data.BaseFee);
                    $('#NetFee').val(result.data.NetFee);
                    var discountDesc = result.data.PromotionCode == null ? 'Not Applicable' : result.data.PromotionDescription
                    $('#DiscountName').val(discountDesc);
                    $('#DiscountApplied').val(result.data.DiscountApplied);
                    $('#CheckEstablishmentFee').val(parseFloat(baseFee) + parseFloat(discountApplied));
                    $('#BaseFeeWithCurrency').val('$ ' + baseFee);
                    $('#DiscountAppliedWithCurrency').val('$ ' + discountApplied);
                    $('#NetFeeWithCurrency').val('$ ' + netFee);
                    $('#IsSystemApplied').val(result.data.IsSystemApplied);
                    $('#FeeAdjustmentId').val(result.data.PromotionId);
                    if ($("#PromotionCode").val().toLowerCase() != "other") {
                        $("#btnSubmit").removeAttr('disabled');
                    }
                    $('.manual_promo span').text('');
                    $('#ManualPromocode').val('');
                }
                else {
                    showExceptionPopupMsg(result.data);
                }
                hideSpinner();
            },
            error: function (result) {
                hideSpinner();
                showExceptionPopupMsg(result.data);
                isValid = false;
            }
        });
    }
}

function GetFeeBasedOnPromo() {
    var selectedCheckTypeId = $('#CheckType').val();
    var checkAmount = $('#CheckAmount').val();
    var promocode = $('#ManualPromocode').val();
    if (promocode != '') {
        showSpinner();
        $.ajax({
            type: "GET",
            url: Calculate_Fee_Based_Promo_URL,
            data: { checkTypeId: selectedCheckTypeId, checkAmount: checkAmount, promotionCode: promocode },
            dataType: "json",
            contentType: "application/json; charset=UTF-8",
            success: function (result) {
                if (result.success == true) {
                    var baseFee = parseFloat(Math.round(result.data.BaseFee * 100) / 100).toFixed(2);
                    var discountApplied = parseFloat(Math.round(result.data.DiscountApplied * 100) / 100).toFixed(2);
                    var netFee = parseFloat(Math.round(result.data.NetFee * 100) / 100).toFixed(2);
                    $('#BaseFee').val(result.data.BaseFee);
                    $('#NetFee').val(result.data.NetFee);
                    var discountDesc = result.data.PromotionCode == null ? 'Not Applicable' : result.data.PromotionDescription
                    $('#DiscountName').val(discountDesc);
                    $('#DiscountApplied').val(result.data.DiscountApplied);
                    $('#CheckEstablishmentFee').val(parseFloat(baseFee) + parseFloat(discountApplied));
                    $('#BaseFeeWithCurrency').val('$ ' + baseFee);
                    $('#DiscountAppliedWithCurrency').val('$ ' + discountApplied);
                    $('#NetFeeWithCurrency').val('$ ' + netFee);
                    $('#IsSystemApplied').val(result.data.IsSystemApplied);
                    $('#FeeAdjustmentId').val(result.data.PromotionId);
                    $("#btnSubmit").removeAttr('disabled');
                    $('.manual_promo span').text('');
                }
                else {
                    $('.manual_promo span').text(result.errorMessage);
                    $("#btnSubmit").attr('disabled', true);
                }

                hideSpinner();
            },
            error: function () {
                showExceptionPopupMsg(defaultErrorMessage);
                hideSpinner();
            }
        });
    }
    else {
        getCheckFee('NONE');
        $("#btnSubmit").attr('disabled', true);
    }
}

    function CheckAmount() {
        if (isShow) {
            var checkAmount = $("#CheckAmount").val();
            if (parseFloat(checkAmount) >= parseFloat(amount)) {
                isShow = false;
                return true;
            }
        }
        return false;
    }


