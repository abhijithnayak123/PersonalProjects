//var NPSbaseURL = "https://opt-lap-0126.opteamix.com:18732/Peripheral/";
function ScanCheckTest() {
    showSpinner();
    $.ajax({
        data: "{}",
        dataType: "jsonp",
        type: "GET",
        url: NPSbaseURL + "CheckScanTest",
        contentType: "application/json; charset=UTF-8",
        processData: true,
        success: function (data) {
            if (data != null) {
                if (data.CheckScanTestResult != null) {
                    if (data.CheckScanTestResult.MicrErrorMessage != null) {
                        showExceptionPopupMsg(data.CheckScanTestResult.MicrErrorMessage);
                        hideSpinner();
                        return;
                    }
                    $("#CheckFrontImage").val(data.CheckScanTestResult.Scan_FrontImageJPG);
                    $("#CheckBackImage").val(data.CheckScanTestResult.Scan_BackImageJPG);
                    $("#MICRCode").val(data.CheckScanTestResult.Micr);
                    if (data.CheckScanTestResult.MicrError == 1) {
                        $('#MicrErrorMessage').val($('#BadMicrError').val());
                    }
                    $("#Send").click();
                }
                else {
					showExceptionPopupMsg(data.ErrorDetails);
                }
            }
            hideSpinner();
        },
        error: function (data, errmessage, errdetails) {
            hideSpinner();
            if (data.ErrorNo != undefined) {
                showExceptionPopupMsg(data.ErrorMessage);
                return;
            }
            else {
                showExceptionPopupMsg("The printer is not accessible. There could be another scan or printing in progress or the device has been powered off, Try again later.");
                return;
            }
        },
        timeout: 30000
    });
}

function SetCheckImages(frontImageData, backImageData) {
    var frontImageSrc = "data:image/jpeg;base64," + frontImageData;
    var backImageSrc = "data:image/jpeg;base64," + backImageData;
    $("#CheckFrontImage").attr("src", frontImageSrc);
    if ($("#CheckBackImage") != undefined) {
        $("#CheckBackImage").attr("src", backImageSrc);
    }
}

function MoneyOrderTest() {
    PrintMoneyOrderDiagnostics(NPSbaseURL, moneyOrderPrintData);
}

function PrintMoneyOrderDiagnostics(baseURL, checkPrintdata) {
    showSpinner();

    var postUrl = baseURL + "PrintCheckStream?printparams=";
    var now = new Date();

    var splitSize = 1000;
    var currTime = now.getFullYear() + '' + ("0" + (now.getMonth() + 1)).slice(-2) + '' + ("0" + (now.getDate())).slice(-2) + ("0" + now.getHours()).slice(-2) + ("0" + now.getMinutes()).slice(-2) + ("0" + now.getSeconds()).slice(-2) + ("00" + now.getMilliseconds()).slice(-3);
    var imageDataLen = checkPrintdata.length;
    var splits = Math.ceil(imageDataLen / splitSize);
    for (var i = 0; i < splits; i++) {
        var endSplit = i * splitSize + splitSize;
        if (endSplit > imageDataLen)
            endSplit = imageDataLen;
        var splitData = checkPrintdata.substring(i * splitSize, endSplit);
        add_api_call_to_queue_moprint(currTime, postUrl, "A", currTime, splitData);
    }
    add_api_call_to_queue_moprint(currTime, postUrl, "E", currTime, '');
    $(document).dequeue(currTime);
}

function add_api_call_to_queue_moprint(qname, baseUrl, type, currTime, splitData) {
    $(document).queue(qname, function () {
        var sendUrl = baseUrl + type + currTime + splitData;
        $.ajax({
            type: "GET",
            url: sendUrl,
            dataType: "jsonp",
			cache: false,
            success: function (data, textStatus, jqXHR) {
                if (data.ErrorNo != undefined) {
                    hideSpinner();
                    showExceptionPopupMsg(data.ErrorDetails);
                    return;
                }
                // activate the next ajax call when this one finishes
                $(document).dequeue(qname);
                if (type == "E") {
                    $("#CheckFrontImage").val(data.PrintCheckStreamResult.Scan_FrontImageJPG);
                    $("#CheckBackImage").val(data.PrintCheckStreamResult.Scan_BackImageJPG);
                    $("#Send").click();
                }
            },
            error: function (data, errmessage, errdetails) {
                hideSpinner();
                if (data.ErrNo != undefined) {
                    showExceptionPopupMsg(data.ErrorMessage);
                    return;
                }
                else {
                    showExceptionPopupMsg("The printer is not accessible. There could be another scan or printing in progress or the device has been powered off, Try again later.");
                    return;
                }
            },
            timeout: 30000
        });

    });
}

function ReceiptPrint() {
    $('#loading span').text("Printing Receipts...");
    showSpinner();
    $.ajax({
        data: "{}",
        dataType: "jsonp",
        type: "GET",
        url: NPSbaseURL + "ReceiptPrintTest",
        contentType: "application/json; charset=UTF-8",
        processData: true,
        success: function (data) {
            if (data != null) {
				if (data.ErrorNo != undefined)
				{
                    hideSpinner();
                    showExceptionPopupMsg(data.ErrorDetails);
                    return;
                }

                $('#print').text("Receipt Print Successful");
            }
            hideSpinner();
        },
        error: function (data, errmessage, errdetails) {

            if (data.ErrorNo == undefined) {
                hideSpinner();
                showExceptionPopupMsg('The printer is not accessible. There could be another scan or printing in progress or the device has been powered off, Try again later.');
                return;
            }
        },
        timeout: 30000
    });
}