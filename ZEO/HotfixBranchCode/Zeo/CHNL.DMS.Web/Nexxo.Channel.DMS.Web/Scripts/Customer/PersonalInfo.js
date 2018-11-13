$(document).ready(function () {
    $("#divUserSessionId").css("display", "");
	var clientProfileStatus = $('#ClientProfileStatus').val();

	if (canEnableProfileStatus != 'True') {
		if (clientProfileStatus.toLowerCase() == 'inactive')
			{
			$("#CustomerProfileStatus").prop("disabled", true);
		}
	}
	$('#CustomerProfileStatus').change(function () {
		var status = $('#CustomerProfileStatus').val();
		$('#hdnCustomerProfileStatus').val($('#CustomerProfileStatus').val());
		$.ajax(
			{
				url: Profile_URL,
				type: "POST",
				dataType: "json",
				data: { profileStatus: status },
				success: function (data) {
					if (data.success) {
						if (status == "Closed" && data.cartStatus == "empty") {
							var $confirm = $("<div id='divTrans'></div>");
							$confirm.empty();
							$confirm.dialog({
								autoOpen: false,
								title: "Message",
								width: 410,
								draggable: false,
								resizable: false,
								closeOnEscape: false,
								modal: true,
								height: 190,
								open: function (event, ui) {
									var url = CustomerStatusCheck;
									$confirm.load(url);
								}
							});
							$confirm.dialog("open");
						}
						else if (status == "Closed" && data.cartStatus == "nonempty") {
							displayShoppingCartStatusClosedPopup("nonempty");
						}
						else {
							return true;
						}
					};
				}
			});
	})
});
function displayShoppingCartStatusClosedPopup(shoppingCartStatus) {

	var $confirm = $("<div id='divshoppingcardstatus'></div>");
	$confirm.empty();
	$confirm.dialog({
		autoOpen: false,
		title: 'Message',
		width: 420,
		draggable: false,
		resizable: false,
		closeOnEscape: false,
		modal: true,
		height: 160,
		open: function (event, ui) {
			var url = ShoppingCartEmptyWhenClosedURL;
			$confirm.load(url, { shoppingCartStatus: shoppingCartStatus });
		}
	});
	$confirm.dialog("open");
}




