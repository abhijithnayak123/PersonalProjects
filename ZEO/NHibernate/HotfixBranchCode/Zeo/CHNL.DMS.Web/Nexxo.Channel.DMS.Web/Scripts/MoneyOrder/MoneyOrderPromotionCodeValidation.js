$(document).ready(function () {
	var promoCode = $('#PromotionCode').val();
	var isValid = false;
	if (promoCode != '') {
		isValid = true;
	}
	$('#PromotionCode').focusout(function () {
		promoCode = $('#PromotionCode').val();
		var amount = $('#txtAmount').val();
		var isSystemApplied = $('#IsSystemApplied').val();
		$('#promodiv span').text('').show();
		if (amount != '' && promoCode != '') {
			Toupper();
			$.ajax({
				type: 'GET',
				url: MO_Promotion_Url,
				data: { promotionCode: promoCode, Amount: amount, IsSystemApplied: isSystemApplied },
				dataType: 'json',
				success: function (result) {
					if (result.success != 'NotValid') {
						$('#PromotionCode').val(result.data.DiscountName);
						var discountDesc = result.data.DiscountName == null ? 'Not Applicable' : result.data.DiscountDescription
						var baseFee = parseFloat(Math.round(result.data.BaseFee * 100) / 100).toFixed(2);
						var netFee = parseFloat(Math.round(result.data.NetFee * 100) / 100).toFixed(2);
						$('#DiscountName').val(discountDesc);
						$('#DiscountApplied').val(result.data.DiscountApplied);
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
						Toupper();
					}
					else if (result.success == 'NotValid') {
						$('#promodiv span').text(result.data).show();
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
			return;
		}
		showSpinners($("#loading"));

		getMoneyOrderFee();	
	});

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

	$('#PromotionCode').change(function () {
		promoCode = $('#PromotionCode').val();
		var amount = $('#txtAmount').val();
		var isSystemapplied = $('#IsSystemApplied').val();

		if ($('#SystemPromotionCode').val().trim().toUpperCase() != this.value.trim().toUpperCase()) {
			if ($('#PromotionCode').val() != null && $('#PromotionCode').val() != '') {
				$('#IsSystemApplied').val(false);
			}
		}

		getMoneyOrderFee();
	});
	$("#txtAmount").decimalOnly();
	$("#txtAmount").focus();

	$('#btnSubmit').click(function (e) {
		promoCode = $('#PromotionCode').val();
		var errorMsg = $('#promodiv span').text();
		
		//check for promotion code and error message. If there is no promo code then submit the form irrespective of Isvalid.
		if (promoCode != "") {
			if (isValidPromotion()) {
				$("form").submit();
			}
			else {
				$('#promodiv span').text(errMsg).show();
				$('#PromotionCode').focus();
				e.preventDefault();
			}
		}
		else {
			$("form").submit();
		}
	});

	function Toupper() {
		var promoCode = $('#PromotionCode').val().toUpperCase();
		$('#PromotionCode').val(promoCode);
	}

	function getMoneyOrderFee() {
		promoCode = $('#PromotionCode').val();
		var amount = $('#txtAmount').val();
		var isSystemapplied = $('#IsSystemApplied').val();

		$.ajax({
			url: url_GetMoneyOrderFee,
			data: { moneyOrderAmount: amount, promotionCode: promoCode, IsSystemApplied: isSystemapplied },
			type: 'GET',
			datatype: 'json',
			success: function (result) {
				if (result.success) {
					$('#BaseFee').val(result.data.BaseFee);
					$('#NetFee').val(result.data.NetFee);
					$('#txtFee').val(result.data.NetFee);
					$('#Fee').val(result.data.NetFee);

					if (promoCode == "") {
						var discount = result.data.DiscountName;
						$('#PromotionCode').val(discount);
						var discountDesc = result.data.DiscountName == '' ? 'Not Applicable' : result.data.DiscountDescription
						$('#DiscountName').val(discountDesc);
						$('#DiscountApplied').val(result.data.DiscountApplied);
					}

					var baseFee = parseFloat(Math.round(result.data.BaseFee * 100) / 100).toFixed(2);
					var discountApplied = parseFloat(Math.round(result.data.DiscountApplied * 100) / 100).toFixed(2);
					var netFee = parseFloat(Math.round(result.data.NetFee * 100) / 100).toFixed(2);
					$('#IsSystemApplied').val(result.data.IsSystemApplied);
					$('#BaseFeeWithCurrency').val('$ ' + baseFee);
					$('#DiscountAppliedWithCurrency').val('$ ' + discountApplied);
					$('#NetFeeWithCurrency').val('$ ' + netFee);
					Toupper();
					TotalAmount();
					hideSpinner();
					$("#btnSubmit").removeAttr('disabled');
				}
				else if (result.success == "NotValid") {
					$('#promodiv span').text(result.data).show();
					$('#PromotionCode').focus();
					$("#btnSubmit").attr('disabled', 'disabled');
					
					hideSpinner();
				}
				else {
					showExceptionPopupMsg(result.data);
					hideSpinner();
					$("#btnSubmit").removeAttr('disabled');
				}
			}
		});

	}


	function isValidPromotion() {
		promoCode = $('#PromotionCode').val();
		var selectedCheckTypeId = $('#CheckType').val();
		var amount = $('#txtAmount').val();
		var isSystemApplied = $('#IsSystemApplied').val();

		$.ajax({
			type: 'GET',
			url: MO_Promotion_Url,
			data: { promotionCode: promoCode, Amount: amount, IsSystemApplied: isSystemApplied },
			dataType: 'json',
			async: false,
			success: function (result) {
				if (result.success != 'NotValid') {
					$("#btnSubmit").removeAttr('disabled');
					$('#promodiv span').text('').show();
					isValid = true;
				}
				else if (result.success == 'NotValid') {
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



