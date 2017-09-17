$(document).ready(function () {

    disableNextButton();
    $('#ConsumerProtectionMessage').click(function () {
        disableNextButton();
    });

    var maskedAcNo = '';
    maskedAcNo = $.trim($("#BillPaymentAccount").val());
    maskedAcNo = maskedAcNo.replace(/.(?=.{4})/g, '*');

    $("#billPymtMask").text(maskedAcNo);
    $("#BillPaymentAccount").val(maskedAcNo);
});
