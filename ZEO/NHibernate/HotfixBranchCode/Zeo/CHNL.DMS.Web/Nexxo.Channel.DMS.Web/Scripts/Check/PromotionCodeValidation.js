$(document).ready(function () {
	var promoCode = $('#PromotionCode').val();
	var isValid = false;
	if (promoCode != '') {
		isValid = true;
	}

	$('#PromotionCode').focusout(function () {
		var selectedCheckTypeId = $('#CheckType').val();
		var checkAmount = $('#CheckAmount').val();
		var promoCode = $('#PromotionCode').val();
		var isSystemApplied = $('#IsSystemApplied').val();
		$('#promodiv span').text('').show();
		$("#btnSubmit").removeAttr('disabled');

		if (selectedCheckTypeId != "" && (checkAmount != " " || checkAmount != "0" || checkAmount != "0.00") && promoCode != "") {
			Toupper();
			getCheckFee();
		}
	});

	$('#PromotionCode').change(function () {
		if ($('#SystemPromotionCode').val().trim().toUpperCase() != this.value.trim().toUpperCase()) {
		    if ($('#PromotionCode').val() != null || $('#PromotionCode').val() == '') {
				$('#IsSystemApplied').val(false);
			}
		}

		getCheckFee();
	});

	function Toupper() {
		var promoCode = $('#PromotionCode').val().toUpperCase();
		$('#PromotionCode').val(promoCode);
	}

	$('#btnSubmit').click(function (e) {
		promoCode = $('#PromotionCode').val();
		var errorMsg = $('#promodiv span').text();

		//check for promotion code and error message. If there is no promo code then submit the form irrespective of Isvalid.
		if (promoCode != "") {
			if (isValidPromotion()) {
				$("form").submit();
			}
			else {
				$('#PromotionCode').focus();
				e.preventDefault();
			}
		}
		else {
			$("form").submit();
		}
	});

	function getCheckFee() {
		promoCode = $('#PromotionCode').val();
		var selectedCheckTypeId = $('#CheckType').val();
		var checkAmount = $('#CheckAmount').val();
		var isSystemApplied = $('#IsSystemApplied').val();

		$.ajax({
			type: 'GET',
			url: base_url_GetCheckFee,
			data: { CheckTypeId: selectedCheckTypeId, CheckAmount: checkAmount, promotionCode: promoCode, IsSystemApplied: isSystemApplied },
			dataType: 'json',
			success: function (result) {
				if (result.success == true) {
					$('#BaseFee').val(result.data.BaseFee);
					$('#NetFee').val(result.data.NetFee);
					var discount = result.data.DiscountName
					$('#PromotionCode').val(discount);
					var discountDesc = result.data.DiscountName == null ? 'Not Applicable' : result.data.DiscountDescription
					$('#DiscountName').val(discountDesc);
					$('#DiscountApplied').val(result.data.DiscountApplied);
					$('#CheckEstablishmentFee').val(result.data.BaseFee);
					var baseFee = parseFloat(Math.round(result.data.BaseFee * 100) / 100).toFixed(2);
					var discountApplied = parseFloat(Math.round(result.data.DiscountApplied * 100) / 100).toFixed(2);
					var netFee = parseFloat(Math.round(result.data.NetFee * 100) / 100).toFixed(2);
					$('#BaseFeeWithCurrency').val('$ ' + baseFee);
					$('#DiscountAppliedWithCurrency').val('$ ' + discountApplied);
					$('#NetFeeWithCurrency').val('$ ' + netFee);
					$('#IsSystemApplied').val(result.data.IsSystemApplied);
					isValid = true;
					Toupper();
					$("#btnSubmit").removeAttr('disabled');
				}
				else if (result.success == "NotValid") {
					$('#promodiv span').text(result.data).show();
					$('#PromotionCode').val('');
					$('#DiscountApplied').val('');
					$('#DiscountAppliedWithCurrency').val('');
					$('#DiscountName').val('');
					$('#PromotionCode').focus();
					isValid = false;
					$("#btnSubmit").attr('disabled', 'disabled');
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

	function isValidPromotion() {
		promoCode = $('#PromotionCode').val();
		var selectedCheckTypeId = $('#CheckType').val();
		var checkAmount = $('#CheckAmount').val();
		var isSystemApplied = $('#IsSystemApplied').val();

		$.ajax({
			type: 'GET',
			url: base_url_GetCheckFee,
			async: false,
			data: { CheckTypeId: selectedCheckTypeId, CheckAmount: checkAmount, promotionCode: promoCode, IsSystemApplied: isSystemApplied },
			dataType: 'json',
			success: function (result) {
				if (result.success == true) {
					$("#btnSubmit").removeAttr('disabled');
					$('#promodiv span').text('').show();
					isValid = true;
				}
				else if (result.success == "NotValid") {
					$('#promodiv span').text(result.data).show();
					$('#PromotionCode').focus();
					$("#btnSubmit").attr('disabled', 'disabled');

					isValid = false;
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

		return isValid;
	}

});
