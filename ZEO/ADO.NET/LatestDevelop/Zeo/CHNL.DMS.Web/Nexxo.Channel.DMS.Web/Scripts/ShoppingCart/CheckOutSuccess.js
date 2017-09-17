$(function () {
	DisableButtons();
});
function DisableControls() {
	var submitbutton = $(this).find('input[type="submit"]');
	var button = $(this).find('input[type="button"]');
	setTimeout(function () {
		$('#layout_left_pane_shadow').removeClass('displaynoneleftpanel');
		$('#layout_left_pane_shadow').addClass('displayblockleftpanel');
		button.addClass('DisableButtons');
		submitbutton.addClass('DisableButtons');
		$('.anc_link_btn').addClass('DisableButtons');
		submitbutton.attr('disabled', 'disabled');
		button.attr('disabled', 'disabled');
		$('.anc_link_btn').attr('disabled', 'disabled');
		$(".nav_item").each(function () {
			$(this).addClass("DisablePanels");
			$(this).attr('disabled', 'disabled');
		});
		$(".nav_name").each(function () {
			$(this).attr('disabled', 'disabled');
		});
		$('#btnfrankCheck').removeAttr('disabled');
		$('#btnfrankCheck').removeClass('DisableButtons');
		$('#btnYes').removeAttr('disabled');
		$('#btnYes').removeClass('DisableButtons');
		$('#divFrankPopup').find('input').removeClass('DisableButtons');
		$('#divFrankPopup').find('input').removeAttr('Disabled');
	}, 0);
}
// Methods added for Receipts printing Sync
var isretry = false;
var inPrintMode = false;
var retryCount = 0; //0 as false, 1 as true and 2 Exit
var timer;
var currentReceipt = 0;
var currentSubReceipt = 0;
var printData = {};
var receiptsListGlb = new Array();
var areAllReceiptPrinted = false;
var isApprovalPopupShown = false;
//Author : Abhijith
//Description : Added a call to post flush after doing shopping cart checkout process.
//Receipts and Post Flush will call simultaneously and if any errors in Post Flush, pop up is 
//displayed with error messages but receipt processing will be continued.	
//Starts Here
function postFlush() {
	$.ajax({
		type: 'GET',
		url: PostFlushURL,
		async: false,
		datatype: 'json',
		error: function (request) {
			AddErrorRow(currentReceipt, request.responseText, true, currentSubReceipt);
		},
		success: function (response) {
			if (response.success && response.ErrorMsg != "")
				showExceptionPopupMsg(response.ErrorMsg);
		}
	});
}
//Ends Here

function PrintReceipts(receiptArray) {
    try {
        for (var receiptcount = 0; receiptcount < receiptArray.length; receiptcount++) {
            receiptsListGlb[receiptcount] = receiptArray[receiptcount];
        }
        preparePrintList(receiptsListGlb);
        timer = setInterval(prepareForSend, 1000);
    } catch (e) {
        showRetryPrintPopupMsg(e.Message);
    }
}



function preparePrintList(receiptsListGlb) {
    $('<tr style="line-height: 280%; border-left-color: currentColor; border-left-width: 0px; border-left-style: solid;border-right:2px solid"><th style="text-align:center;padding:1%;">Receipts</th><th colspan="" style="text-align:center;padding:1%;">Print Status</th><th></th></tr>').appendTo('#ReceiptsTable');
    var CountLoop = 0;
    $.each(receiptsListGlb, function myfunction() {
        var receiptid = "receiptName" + CountLoop + "_" + currentSubReceipt;
        var textid = "Textid" + CountLoop + "_" + currentSubReceipt;
        var linkid = "linktest" + CountLoop + "_" + currentSubReceipt;
        var imageid = "Imageid" + CountLoop + "_" + currentSubReceipt;
        var desc = this;
        if (desc.indexOf("Summary") >= 0)
            desc = "Summary";

        $('<tr style="border-left-color: currentColor; border-left-width: 1px; border-left-style: solid;border-right:1px solid"><td style="text-align: left;padding:3%;"><div id= "' + receiptid + '">' + desc + '</div></td><td class="ImageClass"><div id="' + textid + '"></div><img height="20px" width="20px" style="visibility:hidden" src="" alt="Success" title="Success" id="' + imageid + '" /></td><td class="printerclass"><a disable="" class="linkprinter" id="' + linkid + '" /></td></tr>').appendTo('#ReceiptsTable');
        
        CountLoop++;
        if ($("#Textid0_0")[0])
            $("#Textid0_0")[0].innerText = "Preparing Receipts...";
    });

    $('<tr><td style="position:absolute;float: left; margin-left: 35%; text-align: left; padding-top: 3%; padding-right: 3%; padding-bottom: 3%; padding-left: 3%;" colspan="4"><div id="btndivs" class="SrchButtonWdth SrchButtonWdths" style=""><a id="linkbtn" class="anc_link_btn" style="visibility:hidden;background-color: rgb(0, 105, 55);" href="' + CustomerSearchURL + '" tabIndex="1">Done</a></div></td></tr>').appendTo('#ReceiptsTable');

    $(function () {
    	$('#linkbtn').click(function () {
    		UpdateCounterId();
    	});
    });
}


function prepareForSend() {  
    if (inPrintMode == true)
        return;

    var currentReceiptKey = currentReceipt + "_" + currentSubReceipt;
    if (typeof receiptsListGlb[parseInt(currentReceipt)] == "undefined" || (currentReceipt >= receiptsListGlb.length)) {
        EnableButtons();
        stopspinner();
        areAllReceiptPrinted = true;
        $('.linkprinter').removeAttr('disabled');
        $("#btndiv").css("visibility", "visible");
        $('#linkbtn').attr("style", "visibility:visible");
        $('#linkbtn').focus();
        return;
    }

    if (typeof printData[currentReceiptKey] == "undefined" || printData[currentReceiptKey].PrintData == "") {
        var typedata = receiptsListGlb[parseInt(currentReceipt)].split(":");
        GetReceiptTemplate(typedata[1], typedata[0], currentReceiptKey);
    }
    
    if (typeof printData[currentReceiptKey] != "undefined") {
        SendReceiptToPrinter(NPSbaseURL + "PrintDocStream?printparams=", currentReceiptKey);
    }
}

function GetReceiptTemplate(transactionId, transactiontype, currentReceiptKey) {

    $.ajax({
        type: 'GET',
        url: TransactionHistoryReceiptDataURL,
        data: { transactionId: transactionId, dt: (new Date()).getTime(), transactiontype: transactiontype },
        async: false,
        datatype: 'json',
        error: function (request) {
            AddErrorRow(currentReceipt, request.responseText, true, currentSubReceipt);
        },
        success: function (jsonData) {

            if (jsonData.success) {

                var textid = "#Textid" + currentReceiptKey;

                if (jsonData.data.length > 1) {
                    var html = '';

                    for (var subreceiptCount = currentSubReceipt; subreceiptCount < jsonData.data.length; subreceiptCount++) {
                        var desc = receiptsListGlb[currentReceipt] + " " + jsonData.data[subreceiptCount].Name;

                        var currentKey = currentReceipt + "_" + subreceiptCount;
                        var receiptid = "receiptName" + currentKey;
                        var subtextid = "Textid" + currentKey;
                        var sublinkid = "linktest" + currentKey;
                        var subimageid = "Imageid" + currentKey;

                        printData[currentKey] = jsonData.data[subreceiptCount];

                        if ($("#" + receiptid).length == 0) {
                            if (retryCount == 1 || retryCount == 2) {
                                html += '<tr style="border-left-color: currentColor; border-left-width: 1px; border-left-style: solid;border-right:1px solid"><td style="text-align: left;padding:3%;"><div id= "' + receiptid + '">' + desc + '</div></td><td class="ImageClass"><div id="' + subtextid + '"></div><img height="20px" width="20px" src="' + imageUrl + 'X-MarkDyn.gif" alt="Success" title="Success" id="' + subimageid + '" /></td><td class="printerclass"><a disable="" class="linkprinter" id="' + sublinkid + '" onclick="Reprint(this)" ><img src=' + imageUrl + 'Print-icon.png' + ' alt="Retry" title="Retry" height="20px" width="20px"/></a></td></tr>';
                            } else {
                                html += '<tr style="border-left-color: currentColor; border-left-width: 1px; border-left-style: solid;border-right:1px solid"><td style="text-align: left;padding:3%;"><div id= "' + receiptid + '">' + desc + '</div></td><td class="ImageClass"><div id="' + subtextid + '"></div><img height="20px" width="20px" style="visibility:hidden" src="" alt="Success" title="Success" id="' + subimageid + '" /></td><td class="printerclass"><a disable="" class="linkprinter" id="' + sublinkid + '" /></td></tr>';
                            }
                        }
                        else {
                            if ($("#" + receiptid)[0]) {
                                $("#" + receiptid)[0].innerText = "";
                                $("#" + receiptid)[0].innerText = desc;
                            }
                        }
                    }
                    $(textid).parents("tr").after(html);
                } else {
                    printData[currentReceiptKey] = jsonData.data[0];
                }

            }
            else {
                AddErrorRow(currentReceipt, jsonData.data, true, currentSubReceipt);
            }
        },
        complete: function () {
        }
    });

}


function SendReceiptToPrinter(baseUrl, currentReceiptKey) {
    inPrintMode = true;

    var currentReceiptData = printData[currentReceiptKey];
    if (currentReceiptData != "undefined" && currentReceiptData.PrintData != "") {
        var numberOfCopies;
        if (currentReceiptData.NumberOfCopies == "undefined" || currentReceiptData.NumberOfCopies == "0")
            numberOfCopies = 1;
        else
            numberOfCopies = currentReceiptData.NumberOfCopies;

        for (var count = 1; count <= numberOfCopies; count++) {
        	PrintSingleReceipt(baseUrl, currentReceiptData.PrintData, count, numberOfCopies);
        }
    } else {
        AddErrorRow(currentReceipt, "Template Not Found", true, currentSubReceipt);
        inPrintMode = false;
        isretry = false;
    }
}

function PrintSingleReceipt(baseUrl, printData, currentnumberofcopy, numberOfCopies) {
    var now = new Date();
    var splitSize = 1000;
    var imageData = "";
    var currTime = now.getFullYear() + '' + ("0" + (now.getMonth() + 1)).slice(-2) + '' + ("0" + (now.getDate())).slice(-2) + ("0" + now.getHours()).slice(-2) + ("0" + now.getMinutes()).slice(-2) + ("0" + now.getSeconds()).slice(-2) + ("00" + now.getMilliseconds()).slice(-3);
    imageData = printData + "\\";
    var imageDataLen = imageData.length;
    var splits = Math.ceil(imageDataLen / splitSize);
    for (var i = 0; i < splits; i++) {
        var endSplit = i * splitSize + splitSize;
        if (endSplit > imageDataLen)
            endSplit = imageDataLen;
        var splitData = imageData.substring(i * splitSize, endSplit);
        splitData = splitData.replace("#", " ");
        add_api_call_to_queue_checkout(currTime, baseUrl, "A", currTime, splitData, currentnumberofcopy, numberOfCopies);

    }
    add_api_call_to_queue_checkout(currTime, baseUrl, "E", currTime, '', currentnumberofcopy, numberOfCopies);
    $(document).dequeue(currTime);
}

function add_api_call_to_queue_checkout(qname, baseUrl, type, currTime, splitData, currentnumberofcopy, numberOfCopies) {

    if (retryCount == 2) {
        $('#DisableScreen').removeClass('disableWindow');
        return false;
    }
    $(document).queue(qname, function () {
        var sendUrl = baseUrl + type + currTime + splitData;
        $.ajax({
            type: 'GET',
            async: false,
            url: sendUrl,
            dataType: 'jsonp',
            beforeSend: function () {
                if (type == "E") {
                    var currentReceiptKey = parseInt(currentReceipt) + "_" + parseInt(currentSubReceipt);
                    var textid = "#Textid" + currentReceiptKey;

                    if (retryCount != 2) {
                        if ($(textid)) {
                            $(textid).html("");
                            $(textid).html("Printing " + currentnumberofcopy + " of " + numberOfCopies + " ...");
                        }
                    }
                }
            },
            success: function (data) {
                // activate the next ajax call when this one finishes
                $(document).dequeue(qname);
                if (type == "E") {
                    if (typeof data.ErrorNo == "undefined" && data.PrintDocStreamResult.Result == "Success") {

                        if (currentnumberofcopy != numberOfCopies) {
                            return;
                        }
                        AddSuccessRow(currentReceipt, currentSubReceipt);

                    } else {
                        var errormsg = data.ErrorNo + ": " + data.ErrorMessage;
                        AddErrorRow(currentReceipt, errormsg, true, currentSubReceipt);
                    }
                    inPrintMode = false;
                    isretry = false;
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                var errormsg = textStatus + ": " + errorThrown;
                AddErrorRow(currentReceipt, errormsg, true, currentSubReceipt);
            }
        });
    });
}

function AddErrorRow(receiptno, msg, error, subReceiptno) {
    if (retryCount == 2) {
        areAllReceiptPrinted = true;
        clearInterval(timer);
        return false;
    }
    if (retryCount == 1) {
        retryCount = 2;
    }
    
    var linkId = "#linktest" + parseInt(receiptno) + "_" + parseInt(subReceiptno);
    var imageid = "#Imageid" + parseInt(receiptno) + "_" + parseInt(subReceiptno);
    var textid = "#Textid" + parseInt(receiptno) + "_" + parseInt(subReceiptno);

    $(imageid).attr("style", "");
    if ($(textid)[0])
        $(textid)[0].innerText = "";

    if ($(linkId)) {
	    $(linkId).html("");
    }

    $('<img src=' + imageUrl + 'Print-icon.png' + ' alt="Retry" title="Retry" height="20px" width="20px"/>').appendTo(linkId);
    $(imageid)[0].src = imageUrl + 'X-MarkDyn.gif';
    $(imageid)[0].title = msg;
    $(imageid)[0].alt = msg;

    if (receiptno++ < receiptsListGlb.length && retryCount != 2) {
        subReceiptno++;
        var nxtReceiptKey = currentReceipt + "_" + subReceiptno;

        if (typeof printData[nxtReceiptKey] != "undefined") {
            currentSubReceipt++;
        } else {
            currentReceipt++;
            currentSubReceipt = 0;
        }

        var textidnxt = "#Textid" + currentReceipt + "_" + currentSubReceipt;
        if ($(textidnxt)[0]) {
            $(textidnxt)[0].innerText = "Preparing Receipts...";
        }
    }

    $(linkId).click(function () {Reprint(this);});
    if (typeof receiptsListGlb[parseInt(currentReceipt)] == "undefined" || (currentReceipt >= receiptsListGlb.length)) {
        EnableButtons();
        stopspinner();
        areAllReceiptPrinted = true;
        $('.linkprinter').removeAttr('disabled');
        $("#btndiv").css("visibility", "visible");
        $('#linkbtn').attr("style", "visibility:visible");
        return;
    }
}

function AddSuccessRow(receiptno, subreceiptno) {
    if (retryCount == 2) {
        areAllReceiptPrinted = true;
        clearInterval(timer);
    }
    if (retryCount == 1) {
        areAllReceiptPrinted = true;
        clearInterval(timer);
        retryCount = 2;
    }

    var linkId = "#linktest" + receiptno + "_" + subreceiptno;
    var imageid = "#Imageid" + receiptno + "_" + subreceiptno;
    var textid = "#Textid" + receiptno + "_" + subreceiptno;
    $(imageid).attr("style", "");
    if ($(textid)[0])
    	$(textid)[0].innerText = "";
    if ($(linkId)) {
    	$(linkId).html("");
    }
    $(imageid)[0].src = imageUrl + 'CheckImage.gif';

    if (receiptno++ < receiptsListGlb.length && retryCount != 2) {
        subreceiptno++;
        var nxtReceiptKey = currentReceipt + "_" + subreceiptno;

        if (typeof printData[nxtReceiptKey] != "undefined") {
            currentSubReceipt++;
        } else {
            currentReceipt++;
            currentSubReceipt = 0;
        }

        var textidnxt = "#Textid" + currentReceipt + "_" + currentSubReceipt;
        if ($(textidnxt)[0]) {
            $(textidnxt)[0].innerText = "Preparing Receipts...";
        }
    }
}

function stopspinner() {
    $('#btndiv').addClass('checkoutDivBtn');
    $('#btndiv').removeClass('SrchButtonWdth');
};

function showRetryPrintPopupMsg(errorMsg, receiptNo) {
    var $confirmation = $("<div id='divRetryPrintPopup'></div>");
    $confirmation.empty();
    $confirmation.dialog({
        autoOpen: false,
        title: "Zeo Error Message",
        width: 505,
        draggable: false,
        modal: true,
        resizable: false,
        closeOnEscape: false,
        minHeight: 225,
        scroll: false,
        cache: false,
        open: function (event, ui) {
            $confirmation.load(RetryPrintMsgPopupURL, { dt: (new Date()).getTime(), msg: errorMsg, receiptNo: receiptNo }, function () {
                $('#btnCancel').focus();
            });
        }
    });
    $confirmation.dialog("open");
    return false;
}


function cancelPrint() {
    $('#divRetryPrintPopup').dialog('destroy').remove();
    if (!isretry)
        currentReceipt++;
    else
        clearInterval(timer);
    inPrintMode = false;
}

function retryPrint(receiptno) {
    $('#divRetryPrintPopup').dialog('destroy').remove();
    inPrintMode = false;
    isretry = true;
    currentReceipt = parseInt(receiptno);
}

function RedirectToConfirm() {
    var url = 'ShoppingCartCheckoutConfirm';
    window.location.href = url;
}

function Reprint(link) {

    if (areAllReceiptPrinted == true) {
    
        $('.linkprinter').attr('disabled', 'disabled');
        inPrintMode = false;
        areAllReceiptPrinted = false;
        
        var currentReceiptKey = link.id.substring(8, link.id.len);
        var receiptno = currentReceiptKey.split("_");
        currentReceipt = receiptno[0];
        currentSubReceipt = receiptno[1];
        retryCount = 1;
        var textidnxt = "#Textid" + currentReceiptKey;
        var imageid = "#Imageid" + currentReceiptKey;

        $(imageid).attr("style", "visibility: hidden");
        if ($(textidnxt)[0])
            $(textidnxt)[0].innerText = "Preparing Receipts...";
        var linkId = "#linktest" + currentReceiptKey;

        if ($(linkId)) {
        	$(linkId).html("");
        }

        timer = setInterval(prepareForSend, 1000);
    }
    else {
        showExceptionPopupMsg("Please wait. Another print job in process.");
    }

}

function DisableButtons() {//to disable buttons and panel on checkout page
	if (window.location.pathname)
		if ($('#btnfrankCheck').length > 0 || $('.recieptList').length > 0) {
			DisableControls();
		}
}

function EnableButtons() {
	if (window.location.pathname)
		if (window.location.pathname.indexOf("ShoppingCart/ShoppingCartCheckout") >= 0 || window.location.pathname.indexOf("/CashaCheck/ScanACheck") >= 0) {
			var submitbutton = $(this).find('input[type="submit"]');
			var button = $(this).find('input[type="button"]');
			$('#layout_left_pane_shadow').addClass('displaynoneleftpanel');
			$('#layout_left_pane_shadow').removeClass('displayblockleftpanel');
			button.removeClass('DisableButtons');
			submitbutton.removeClass('DisableButtons');
			$('#btnNext').removeClass('DisableButtons');
			$('#btnNext').css('opacity', '1');
			$('.anc_link_btn').removeClass('DisableButtons');
			submitbutton.removeAttr('disabled');
			button.removeAttr('disabled');
			$('.anc_link_btn').removeAttr('disabled');
			$(".nav_item").each(function () {
				$(this).removeClass("DisablePanels");
				$(this).removeAttr('disabled');
			});
			$(".nav_name").each(function () {
				$(this).removeAttr('disabled', 'disabled');
			});
		}
}
function CheckRemove(transactionId) {

	var $confirm = $("<div id='removeCheckPopup'></div>");
	$confirm.empty();
	$confirm.dialog({
		autoOpen: false,
		title: "Zeo",
		width: 480,
		draggable: false,
		modal: true,
		resizable: false,
		closeOnEscape: false,
		minHeight: 150,
		open: function (event, ui) {
			$confirm.load(RemoveCheckPopupURL, { transactionId: transactionId });
		}
	});
	$confirm.dialog("open");
}

$('#btnRemoveCheckConfirm').click(function () {
	$("#removeCheckPopup").dialog('destroy').remove();
	var $confirmation = $("<div id='removeCheckConfirmPopup'></div>");
	$confirmation.empty();
	$confirmation.dialog({
		autoOpen: false,
		title: "Zeo",
		width: 480,
		draggable: false,
		modal: true,
		resizable: false,
		closeOnEscape: false,
		minHeight: 200,
		scroll: false,
		cache: false,
		open: function (event, ui) {
			$confirmation.load(DeleteCheckPopupURL, { transactionId: transactionId });
		}
	});
	$confirmation.dialog("open");
});

//ShowCertegyConfirmationPopup()
function ShowCertegyConfirmationPopup(CheckStatus) {
	if (isApprovalPopupShown == true)
		return;
	isApprovalPopupShown = true;
	var $confirm = $("<div id='CertegyApprovalConfirmPopup'></div>");
	$confirm.empty();
	$confirm.dialog({
		autoOpen: false,
		title: 'Message',
		width: 400,
		draggable: false,
		resizable: false,
		closeOnEscape: false,
		modal: true,
		height: 160,
		open: function (event, ui) {
			var url = CertegyConfirmationPopup_URL;
			$confirm.load(url, { ShowCertegyConfirmationPopup: CheckStatus });
		}
	});
	$confirm.dialog("open");
	
}

//function displayitemdetails(cartID, SummaryTitle, status) {
//	var displayTitle = SummaryTitle;
//	var dynamicHeight = 550;
//	if (displayTitle == 'Bill Payment')
//		dynamicHeight = 350;
//	if (displayTitle == 'Process Check')
//		dynamicHeight = 450;
//	if (displayTitle == 'Prepaid Card')
//		var dynamicHeight = 400;
//	if (displayTitle == 'Money Order')
//		var dynamicHeight = 275;
//	var $confirm = $("<div id='dlgCheckDetails'></div>");
//	$confirm.empty();
//	$confirm.dialog({

//		autoOpen: false,
//		title: displayTitle,
//		width: 600,
//		draggable: false,
//		modal: true,
//		closeOnEscape: false,
//		height: dynamicHeight,
//		// height: 400,
//		//minHeight: 250,
//		resizable: false,
//		open: function (event, ui) {
//			$confirm.load(cartDetailsURL + '?id=' + cartID + '&status=' + status);
//		}
//	});

//	$confirm.dialog("open");

//}
