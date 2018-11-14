$(function () {

  
    var regexNumber = /^[0-9]*$/;
    //To allow only numbers 
    $("#CheckNumber, #AccountNumber, #RoutingNumber").keypress(function (e) {
        ValidateKey(e, regexNumber);
    });
    //To avoid copy paste
    $("#CheckNumber, #AccountNumber, #RoutingNumber").keydown(function (e) {
        restrictPaste(e);
    });
    //To enabled next button
    //$("#CheckNumber, #AccountNumber, #RoutingNumber").blur(function (e) {
    //    if ($("#RoutingNumber").val() !== '' && $("#AccountNumber").val() !== '' && $("#CheckNumber").val() !== '') {
    //        $("#btnsubmit").attr('disabled', false);
    //    }
    //    else if (showMicrErrorPop.toLowerCase() === "false") {
    //        $("#btnsubmit").attr('disabled', true);
    //    }
    //});

    //$("#btnsubmit").click(function (event) {
    //    if (($("#RoutingNumber").val() === '' || $("#AccountNumber").val() === '' || $("#CheckNumber").val() === '') && showMicrErrorPop.toLowerCase() === "true") {
    //        event.preventDefault();
    //        DisplayMessage(errorMessage, "OK", "Message", 400, 130);
    //    }
    //});
	
});