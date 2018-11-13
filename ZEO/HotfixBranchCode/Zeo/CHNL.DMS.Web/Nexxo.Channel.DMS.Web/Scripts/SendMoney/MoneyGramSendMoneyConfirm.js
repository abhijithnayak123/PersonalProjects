var isDomesticTransfer;

$(document).ready(function () {
	isDomesticTransfer = $("#IsDomesticTransfer").val();

	if (isDomesticTransfer.toLowerCase() == "false") {
		showSpinner();
    	//Author : Abhijith
    	//Bug : AL-2014
    	//Description : Check whether Dodd Frank receipt is printed or not.
    	//Moved the onload code to function - "PrintPDSReceipt" 
    	//Starts Here
    	PrintPDSReceipt();
    	//Ends Here
    }

    DisableEnableSubmit();

    $("#ConsumerProtectionMessage").click(function () {
        DisableEnableSubmit();
    });
    $("#ProvidedTermsandConditionsMessage").click(function () {
        DisableEnableSubmit();
    });
    if (isDomesticTransfer == "False") {
        $("#DoddFrankDisclosure").click(function () {
            DisableEnableSubmit();
        });
    }

});

function DisableEnableSubmit() {
    var isValid = true;
    if (!$("#ConsumerProtectionMessage").attr('checked')) {
        isValid = false;
    }
    if (!$("#ProvidedTermsandConditionsMessage").attr('checked')) {
        isValid = false;
    }
    if (isDomesticTransfer=="False") {
        if (!$("#DoddFrankDisclosure").attr('checked')) {
            isValid = false;
        }
    }
    $("#btnConfirm").attr("disabled", !isValid);
}