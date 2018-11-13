var PayStatus = "";

$(document).ready(function () {

    $('#btnRefund').click(function () {
            showSpinner();
        $('#divFNameValidation').hide();
        $('#divLNameValidation').hide();
        $('#divSecLNameValidation').hide();
        $('#divTestQuestValidation').hide();
        $('#divTestAnswerValidation').hide();

           
        $("#divRefund").css("display", "block");
        $("#divModify").css("display", "none");
            
            var MTCN = wuMoneyMTCN;

        $.ajax({
            url: wuGetRefundStatusURL + "?MTCN=" + MTCN,
            type: 'POST',
            datatype: 'json',
            success: function (jsonData) {

                if (jsonData.success) {
                    $("#lblRefundStatus").html(jsonData.RefundStatus);
                    hideSpinner();
                }
                else {
                    hideSpinner();
                    showExceptionPopupMsg(jsonData.data);
                }
                    
            },
            error: function (err) {
                showExceptionPopupMsg(err);
                hideSpinner();
            }
        });

    });

    $('#btnModify').click(function () {
        showSpinner();
        var MTCN = wuMoneyMTCN;

        $.ajax({
            url: wuGetTransactionForModifyURL + "?MTCN=" + MTCN,
            // data: dataValues,
                type: 'POST',
            datatype: 'json',
            success: function (jsonData) {
                if (jsonData.success) {
                    
                    $("#txtFirstName").val(jsonData.FirstName);
                    $("#txtLastName").val(jsonData.LastName);
                    $("#txtSecLastName").val(jsonData.SecondLastName);
                    $("#txtModifyTestQuestion").val(jsonData.TestQuestion);
                    $("#txtModifyTestAnswer").val(jsonData.TestAnswer);
                        
                    if (wuTestQuestionAvailable == "Yes") {
                        $('#txtModifyTestQuestion').attr("disabled", true);
                    $('#txtModifyTestAnswer').attr("disabled", true);
                    }
                    hideSpinner();

                }
                else {
                    hideSpinner();
                    showExceptionPopupMsg(jsonData.data);
                }
            },
            error: function (err) {
                showExceptionPopupMsg(err);
                hideSpinner();
            }
        });
        $("#divRefund").css("display", "none");
        $("#divModify").css("display", "block");
    });

    $('#btnRefSubmit').click(function () {
    	$('#divReasonForRefundValidation').hide();
    	$('#divRFRefundEmptyValidation').hide();
    	$('#divRefundCatValidation').hide();

    	var ReasonForRefund = $('#txtReasonForRefund').val();
    	var RefundCategory = $('#RefundCategory').val();
    	var SpecialCharRegex = /^[0-9a-zA-Z',-\s]*$/;
    	if (RefundCategory == "") {
    		$('#divRefundCatValidation').show();
    		return false;
    	}
    	if (!SpecialCharRegex.test(ReasonForRefund)) {
    		$('#divReasonForRefundValidation').show();
    		return false;
    	}
    	if (ReasonForRefund == "") {
    		$('#divRFRefundEmptyValidation').show();

    		return false;
    	}

    	showSpinner();
    	var dataValues = {
    		transactionId: wuTransactionId,
    		MTCN: wuMoneyMTCN,
    		categoryCode: $('#RefundCategory').val(),
    		categoryDescription: $("#RefundCategory option:selected").text(),
    		Reason: $('#txtReasonForRefund').val()
    	};
    	$.ajax({
    		url: wuSendMoneyRefundSubmit,
    		data: dataValues,
    		type: 'POST',
    		datatype: 'json',
    		success: function (jsonData) {
    			if (jsonData.success) {

    				SendMoneyRefundSuccess();
    			}
    			else {
    				hideSpinner();
    				//SendMoneyRefundFail();
    				showExceptionPopupMsg(jsonData.data);
    			}
    		},
    		error: function (err) {
    			hideSpinner();
    			showExceptionPopupMsg(err);

    		}
    	});
    });
    $('#btnModifySubmit').click(function () {

    	$('#divFNameValidation').hide();
    	$('#divLNameValidation').hide();
    	$('#divSecLNameValidation').hide();
    	$('#divTestQuestValidation').hide();
    	$('#divTestAnswerValidation').hide();

    	var FName = $('#txtFirstName').val();
    	var LName = $('#txtLastName').val();
    	var SecLName = $('#txtSecLastName').val();
    	var TestQuest = $('#txtModifyTestQuestion').val();
    	var TestAnsw = $('#txtModifyTestAnswer').val();

    	var Alphareg = /^[a-zA-Z ]*$/;
    	var SpecialCharRegex = /^[0-9a-zA-Z',-\s]*$/;

    	if (!Alphareg.test(FName)) {
    		$('#divFNameValidation').show();
    		//_spinner.stop();
    		return false;
    	}
    	if (!Alphareg.test(LName)) {
    		$('#divLNameValidation').show();
    		return false;
    	}
    	if (!Alphareg.test(SecLName)) {
    		$('#divSecLNameValidation').show();
    		return false;
    	}
    	if (!SpecialCharRegex.test(TestQuest)) {
    		$('#divTestQuestValidation').show();
    		return false;
    	}
    	if (!SpecialCharRegex.test(TestAnsw)) {
    		$('#divTestAnswerValidation').show();
    		return false;
    	}

    	var dataValues = {
    		transactionId: wuTransactionId,
    		MTCN: wuMoneyMTCN,
    		FirstName: $('#txtFirstName').val(),
    		LastName: $('#txtLastName').val(),
    		SecondLastName: $('#txtSecLastName').val(),
    		TestQuestion: $('#txtModifyTestQuestion').val(),
    		TestAnswer: $('#txtModifyTestAnswer').val()
    	};
    	$.ajax({

    		url: wuSendMoneyModifySubmit,
    		data: dataValues,
    		type: 'POST',
    		datatype: 'json',
    		success: function (jsonData) {
    			if (jsonData.success) {
    				// showSpinner();
    				_spinner();
    				$('#divPrepaid').dialog('destroy').remove();
    				window.location.href = wuSendMoneyConfirm;
    			}
    			else {
    				//SendMoneyRefundFail();
    				showExceptionPopupMsg(jsonData.data);
    				hideSpinner();
    			}

    		},
    		error: function (err) {
    			showExceptionPopupMsg(err);
    		}
    	});
    });
});

function SendMoneyRefundFail() {

    jQuery.ajaxSetup({ cache: false });

    //     $('#divPrepaid').dialog('destroy').remove();
    var $MoneyTransferRefundFail = $("<div id='MoneyTransferRefundFailPopup'></div>");
    $MoneyTransferRefundFail.empty();
    $MoneyTransferRefundFail.dialog({
        autoOpen: false,
        title: "Message",
        width: 430,
        draggable: false,
        modal: true,
        minHeight: 170,
        resizable: false,
        closeOnEscape: false,
        open: function (event, ui) {
            var URL = wuSendMoneyRefundFail;
            $MoneyTransferRefundFail.load(URL, function () {
                $('#CancelTransaction').focus();
            });
        },
        error: function (err) {
            showExceptionPopupMsg(err);
        }
    });
    $MoneyTransferRefundFail.dialog("open");
}


function SendMoneyRefundSuccess() {
    jQuery.ajaxSetup({ cache: false });
    if ($('#txtReasonForRefund').val() == '') {
        $('#divCheckNumberValidation').show();
        return false;
    }

    var MTCN = wuMoneyMTCN;
    $('#divPrepaid').dialog('destroy').remove();
    var $MoneyTransferRefundSuccess = $("<div id='MoneyTransferRefundSuccessPopup'></div>");
    $MoneyTransferRefundSuccess.empty();
    $MoneyTransferRefundSuccess.dialog({
        autoOpen: false,
        title: "Message",
        width: 430,
        draggable: false,
        modal: true,
        minHeight: 170,
        resizable: false,
        closeOnEscape: false,
        open: function (event, ui) {
            var URL = wuSendMoneyRefundSuccess + "?MTCN=" + MTCN;
            $MoneyTransferRefundSuccess.load(URL, function () {
                $('#CancelTransaction').focus();
            });
        },
        error: function (err) {
            showExceptionPopupMsg(err);
        }
    });
    $MoneyTransferRefundSuccess.dialog("open");
}

function SendMoneyModifySuccess() {
    jQuery.ajaxSetup({ cache: false });




    var MTCN = wuMoneyMTCN;
    $('#divPrepaid').dialog('destroy').remove();
    var $SendMoneyCancelSuccess = $("<div id='SendMoneyModifySuccessPopup'></div>");
    $SendMoneyCancelSuccess.empty();
    $SendMoneyCancelSuccess.dialog({
        autoOpen: false,
        title: "Message",
        width: 330,
        draggable: false,
        modal: true,
        minHeight: 170,
        resizable: false,
        closeOnEscape: false,
        open: function (event, ui) {
            var URL = wuSendMoneyModifySuccess + "?MTCN=" + MTCN;
            $SendMoneyCancelSuccess.load(URL, function () {
                $('#CancelTransaction').focus();
            });
        },
        error: function (err) {
            showExceptionPopupMsg(err);
        }
    });
    $SendMoneyCancelSuccess.dialog("open");
}
