$(document).ready(function () {
	$('#CheckAmount').blur(function () {
		var selectedCheckTypeId = $('#CheckType').val();
		var checkAmount = $('#CheckAmount').val();
		var isSystemapplied = $('#IsSystemApplied').val();
		var promoCode = $('#PromotionCode').val();
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
        
		$.ajax({
			url: base_url_GetCheckFee,
			data: { CheckTypeId: selectedCheckTypeId, CheckAmount: checkAmount, promotionCode: promoCode, IsSystemApplied: isSystemapplied },
			type: 'GET',
			datatype: 'json',
			success: function (result) {
			    if (result.success == true) {
			        var baseFee = parseFloat(Math.round(result.data.BaseFee * 100) / 100).toFixed(2);
			        var discountApplied = parseFloat(Math.round(result.data.DiscountApplied * 100) / 100).toFixed(2);
			        var netFee = parseFloat(Math.round(result.data.NetFee * 100) / 100).toFixed(2);
					$('#BaseFee').val(result.data.BaseFee);
					$('#NetFee').val(result.data.NetFee);
					var discount = result.data.DiscountName;
					$('#PromotionCode').val(discount);
					var discountDesc = result.data.DiscountName == '' ? 'Not Applicable' : result.data.DiscountDescription
					$('#DiscountName').val(discountDesc);
					$('#DiscountApplied').val(result.data.DiscountApplied);
					$('#CheckEstablishmentFee').val(parseFloat(baseFee) + parseFloat(discountApplied));
					$('#BaseFeeWithCurrency').val('$ ' + baseFee);
					$('#DiscountAppliedWithCurrency').val('$ ' + discountApplied);
					$('#NetFeeWithCurrency').val('$ ' + netFee);
					$('#IsSystemApplied').val(result.data.IsSystemApplied);
					$('#FeeAdjustmentId').val(result.data.FeeAdjustmentId);
					isValid = true;
					Toupper();
				}
			    else if (result.success == 'NotValid') {
					$('#promodiv span').text(result.data).show();
					$('#PromotionCode').val('');
					$('#CheckEstablishmentFee').val('');
					$('#BaseFee').val('0');
					$('#NetFee').val('0');
					$('#DiscountName').val('');
					$('#DiscountApplied').val('0');
					$('#BaseFeeWithCurrency').val('$ 0.00');
					$('#DiscountAppliedWithCurrency').val('$ 0.00');
					$('#NetFeeWithCurrency').val('$ 0.00');
					$('#PromotionCode').focus();
					isValid = false;
				}
				else {
				    $('#BaseFee').val('0');
				    $('#NetFee').val('0');
				    $('#DiscountName').val('');
				    $('#DiscountApplied').val('0');
				    $('#BaseFeeWithCurrency').val('$ 0.00');
				    $('#DiscountAppliedWithCurrency').val('$ 0.00');
				    $('#NetFeeWithCurrency').val('$ 0.00');
					showExceptionPopupMsg(result.data);
					isValid = false;

				}
			}

		});

	});

	$('#CheckType').change(function () {
		var selectedCheckTypeId = $('#CheckType').val();
		var checkAmount = $('#CheckAmount').val();
		var promoCode = $('PromotionCode').val();
		var isSystemapplied = $('#IsSystemApplied').val();
		if ((checkAmount == '' || checkAmount == '0' || checkAmount == '0.00') || selectedCheckTypeId == '') {
			$('#CheckEstablishmentFee').val('');
			$('#BaseFee').val('0');
			$('#NetFee').val('0');
			$('#DiscountName').val('');
			$('#DiscountApplied').val('0');
			$('#BaseFeeWithCurrency').val('$ 0.00');
			$('#DiscountAppliedWithCurrency').val('$ 0.00');
			$('#NetFeeWithCurrency').val('$ 0.00');
			return;
		}

		$.ajax({
			url: base_url_GetCheckFee,
			data: { CheckTypeId: selectedCheckTypeId, CheckAmount: checkAmount, promotionCode: promoCode, IsSystemApplied: isSystemapplied },
			type: 'GET',
			datatype: 'json',
			success: function (result) {
			    if (result.success == true) {
			        var baseFee = parseFloat(Math.round(result.data.BaseFee * 100) / 100).toFixed(2);
			        var discountApplied = parseFloat(Math.round(result.data.DiscountApplied * 100) / 100).toFixed(2);
			        var netFee = parseFloat(Math.round(result.data.NetFee * 100) / 100).toFixed(2);
					$('#BaseFee').val(result.data.BaseFee);
					$('#NetFee').val(result.data.NetFee);
					var discount = result.data.DiscountName;
					$('#PromotionCode').val(discount);
					var discountDesc = result.data.DiscountName == '' ? 'Not Applicable' : result.data.DiscountDescription
					$('#DiscountName').val(discountDesc);
					$('#DiscountApplied').val(result.data.DiscountApplied);
					$('#CheckEstablishmentFee').val(parseFloat(baseFee) + parseFloat(discountApplied));
					$('#IsSystemApplied').val(result.data.IsSystemApplied);
					var one = $('#CheckEstablishmentFee').val();
					$('#BaseFeeWithCurrency').val('$ ' + baseFee);
					$('#DiscountAppliedWithCurrency').val('$ ' + discountApplied);
					$('#NetFeeWithCurrency').val('$ ' + netFee);
					$('#FeeAdjustmentId').val(result.data.FeeAdjustmentId);
					isValid = true;
					Toupper();
				}
				else if (result.success == "NotValid") {
					$('#promodiv span').text(result.data).show();
					$('#PromotionCode').val('');
					$('#DiscountApplied').val('');
					$('#DiscountAppliedWithCurrency').val('');
					$('#PromotionCode').focus();
					isValid = false;
				}
				else {
					showExceptionPopupMsg(result.data);
					isValid = false;
				}
			}
		});
	});
	$('#PromotionCode').change(function () {
		if ($('#SystemPromotionCode').val().trim().toUpperCase() != this.value.trim().toUpperCase()) {
		    if ($('#PromotionCode').val() != null || $('#PromotionCode').val() == '') {
				$('#IsSystemApplied').val(false);
			}
		}
	});
	function Toupper() {
		var promoCode = $('#PromotionCode').val().toUpperCase();
		$('#PromotionCode').val(promoCode);
	}
});
