$(function () {
	var isDomesticTransfer = $("#isDomesticTransferVal").val().toString();
	var printDialog = localStorage.getItem("PrintDialog");
	if (isDomesticTransfer.toLowerCase() == "false" && (printDialog || isTnCForcePrintRequired.toLowerCase() == 'false')) {
		showSpinner();

		//Author : Abhijith
		//Bug : AL-2014
		//Description : Check whether Dodd Frank receipt is printed or not.
		//Moved the onload code to function - "PrintPDSReceipt" 
		//Starts Here
		PrintPDSReceipt();
		//Ends Here
	}

	if (isTnCForcePrintRequired.toLowerCase() == 'true') {
		if (!printDialog) {
			var $PrintTermsConditions = $("<div id='TermsandConditionsPrintPopup'></div>");
			$PrintTermsConditions.empty();
			localStorage.setItem("PrintDialog", "true");
			$PrintTermsConditions.dialog({
				autoOpen: false,
				title: "Terms and Conditions",
				width: 630,
				draggable: false,
				modal: true,
				minHeight: 200,
				resizable: false,
				closeOnEscape: false,
				open: function (event, ui) {
					var URL = WUTermsConditionsPopupURL;
					$PrintTermsConditions.load(URL, function () {
						$('#btnPrint').focus();
					});
				},
				error: function (err) {
					showExceptionPopupMsg(err);
				}
			});
			$PrintTermsConditions.dialog("open");
		}
		else
			localStorage.removeItem("PrintDialog");
	}
})
