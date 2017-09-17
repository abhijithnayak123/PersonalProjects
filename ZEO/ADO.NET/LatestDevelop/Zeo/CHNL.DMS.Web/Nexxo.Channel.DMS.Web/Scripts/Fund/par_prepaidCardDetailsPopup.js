$(document).ready(function () {
    $('#divActivationFee').hide();
    $('#divLoadFee').hide();
    $('#divWithdrawFee').hide();

    if (cdPopupActivationFee != null && parseFloat(cdPopupActivationFee) != 0) {
        $('#divActivationFee').show();
    }
    $('#divLoadAmount').show();
    if (cdPopupLoadFee != null && parseFloat(cdPopupLoadFee) > 0) {
        $('#divLoadFee').show();
    }
    $('#divWithdrawAmount').show();
    if (cdPopupWithdrawFee != null && parseFloat(cdPopupWithdrawFee) > 0) {
        $('#divWithdrawFee').show();
    }
    $('#divLoadBalanceImpact').show();
    $('#divWithdrawBalanceImpact').show();

    if (transactionType.toLowerCase() == "activation") {
        $('#divActivationFee').show();
        $('#divLoadAmount').hide();
        $('#divLoadFee').hide();
        $('#divLoadBalanceImpact').hide();
        $('#divWithdrawAmount').hide();
        $('#divWithdrawFee').hide();
        $('#divWithdrawBalanceImpact').hide();
    }

    if (transactionType.toLowerCase() == "load") {
        $('#divLoadFee').show();
        $('#divActivationFee').hide();
        $('#divWithdrawAmount').hide();
        $('#divWithdrawFee').hide();
        $('#divWithdrawBalanceImpact').hide();
    }

    if (transactionType.toLowerCase() == "withdraw") {
        $('#divWithdrawFee').show();
        $('#divActivationFee').hide();
        $('#divLoadAmount').hide();
        $('#divLoadFee').hide();
        $('#divLoadBalanceImpact').hide();
    }
});
 