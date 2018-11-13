var PayStatus = "";
var FirstName = "";
var MiddleName = "";
var LastName = "";
var SecondLastName = "";

$(document).ready(function () {

    $('#btnRefund').click(function () {
        showSpinner();
        $('#divFNameValidation').hide();
        $('#divLNameValidation').hide();
        $('#divSecLNameValidation').hide();
        $('#divTestQuestValidation').hide();
        $('#divTestAnswerValidation').hide();

        $.ajax({
            url: RefundClick_URL,
            type: 'POST',
            datatype: 'json',
            success: function (jsonData) {

                if (jsonData.success) {
                    $("#divRefund").css("display", "block");
                    $("#divModify").css("display", "none");
                    $("#lblRefundStatus").html(jsonData.tranDetails.RefundStatusDesc);
                    $('#FeeRefund').val('');

                    RefundCategory = $('#RefundCategory');
                    RefundCategory.empty();

                    if (jsonData.tranDetails.RefundStatus == RefundStatus || jsonData.tranDetails.FeeRefund == 'Y') {
                        var items = '';
                        $.each(jsonData.tranDetails.RefundCategory, function (i, refundCategory) {
                            if (refundCategory.Text != undefined) {
                                items += '<option value="' + refundCategory.Value + '">' + refundCategory.Text + '</option>';
                            }
                            else {
                                items = '<option value="">Select</option>';
                            }
                        });
                        $('#RefundCategory').html(items);
                        $("#divReasonForRefund").css("display", "block");
                        $('#FeeRefund').val(jsonData.tranDetails.FeeRefund);
                    }

                    $('#RefundStatus').val(jsonData.tranDetails.RefundStatus);
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

        $.ajax({
            url: ModifyClick_URL,
            type: 'POST',
            datatype: 'json',
            success: function (jsonData) {
                if (jsonData.success) {

                    $("#txtFirstName").val(jsonData.FirstName);
                    $("#txtLastName").val(jsonData.LastName);
                    $("#txtSecLastName").val(jsonData.SecondLastName);
                    $("#txtMiddleName").val(jsonData.MiddleName);
                    $("#divRefund").css("display", "none");
                    $("#divModify").css("display", "block");

                    if (jsonData.FirstName != null)
                        FirstName = jsonData.FirstName;
                    if (jsonData.MiddleName != null)
                        MiddleName = jsonData.MiddleName;
                    if (jsonData.LastName != null)
                        LastName = jsonData.LastName;
                    if (jsonData.SecondLastName != null)
                        SecondLastName = jsonData.SecondLastName;

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

    $('#btnModifySubmit').click(function () {

        $("#divModifyTransactionMsg").css("display", "none");

        if (VerifyChanges()) {

            $('#divFNameValidation').hide();
            $('#divLNameValidation').hide();
            $('#divSecLNameValidation').hide();
            $('#divMNameValidation').hide();
            $('#divFNameReqValidation').hide();
            $('#divLNameReqValidation').hide();

            var FName = $('#txtFirstName').val();
            var LName = $('#txtLastName').val();
            var SecLName = $('#txtSecLastName').val();
            var MName = $('#txtMiddleName').val();

            var Alphareg = /^[a-zA-Z\- ']*$/;

            if (!FName.match(/^[\s\t\r\n]*\S+/ig)) {
                $('#divFNameReqValidation').show();
                return false;
            }
            if (!LName.match(/^[\s\t\r\n]*\S+/ig)) {
                $('#divLNameReqValidation').show();
                return false;
            }

            if (!Alphareg.test(FName)) {
                $('#divFNameValidation').show();
                return false;
            }
            if (!Alphareg.test(MName)) {
                $('#divMNameValidation').show();
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


            var dataValues = {
                TransactionId: TransactionId,
                ConfirmationNumber: ConfirmationNumber,
                FirstName: $('#txtFirstName').val(),
                LastName: $('#txtLastName').val(),
                SecondLastName: $('#txtSecLastName').val(),
                MiddleName: $('#txtMiddleName').val()
            };

            $.ajax({
                url: ModifySubmitClick_URL,
                data: dataValues,
                type: 'POST',
                datatype: 'json',
                success: function (jsonData) {
                    if (jsonData.success) {
                        _spinner();
                        $('#divPrepaid').dialog('destroy').remove();
                        RedirectToUrl(SendMoneyConfirm_URL);
                    } else {
                        showExceptionPopupMsg(jsonData.data);
                        hideSpinner();
                    }
                },
                error: function (err) {

                    showExceptionPopupMsg(err);
                }
            });
        }
        else {
            $("#divModifyTransactionMsg").css("display", "block");
            return false;
        }
    });

    $('#btnRefSubmit').click(function () {
        $('#divRefundCatValidation').hide();

        var RefundCategory = $('#RefundCategory').val();
        if (RefundCategory == "") {
            $('#divRefundCatValidation').show();
            return false;
        }

        showSpinner();

        var dataValues = {
            TransactionId: TransactionId,
            ConfirmationNumber: ConfirmationNumber,
            CategoryCode: $('#RefundCategory').val(),
            CategoryDescription: $("#RefundCategory option:selected").text(),
            RefundStatus: $('#RefundStatus').val(),
            FeeRefund: $('#FeeRefund').val()
        };
        $.ajax({
            url: SendMoneyStageRefundSubmit_URL,
            data: dataValues,
            type: 'POST',
            datatype: 'json',
            success: function (jsonData) {
                if (jsonData.success || jsonData.doRedirect) {
                    _spinner();
                    $('#divPrepaid').dialog('destroy').remove();
                    RedirectToUrl(ShoppingCartCheckout_URL);
                } else {
                    showExceptionPopupMsg(jsonData.data);
                    hideSpinner();
                }
            },
            error: function (err) {
                hideSpinner();
                showExceptionPopupMsg(err);

            }
        });
    });
});

function SendMoneyModifySuccess() {
    jQuery.ajaxSetup({ cache: false });

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

            $SendMoneyCancelSuccess.load(SendMoneyModifySuccess_URL, function () {
                $('#CancelTransaction').focus();
            });
        },
        error: function (err) {
            showExceptionPopupMsg(err);
        }
    });
    $SendMoneyCancelSuccess.dialog("open");
}

function VerifyChanges() {
    if (FirstName.toLowerCase() != $("#txtFirstName").val().toLowerCase() || MiddleName.toLowerCase() != $('#txtMiddleName').val().toLowerCase() || LastName.toLowerCase() != $('#txtLastName').val().toLowerCase() || SecondLastName.toLowerCase() != $('#txtSecLastName').val().toLowerCase())
        return true;
    else {
        return false;
    }
}