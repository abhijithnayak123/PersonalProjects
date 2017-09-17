$(document).ready(function () {
    $('#btnConfirm').focus();
    var totcashCollected = parseFloat(gprTotalCashCollected);
    var totalduefromcustomer = parseFloat($("input#DueToCustomer").val());

    $('#btnDone').click(function (event) {

        $('#GPRWithdrawalAmountPopup').dialog('destroy').remove();
        
        var NetDueToCust = totalduefromcustomer + totcashCollected;
        if (NetDueToCust < 0)
            CollectCashFromCustomerPopup(Math.abs(NetDueToCust));
        else {
            $('#WithdrawFromCard').val("0.00");
            $("input#CashCollected").val("0.00");
            $('input#CashToCustomer').val(NetDueToCust.toFixed(2));
            $('#btnSubmit').trigger('click');
        }
    });
});

function CollectCashFromCustomerPopup(netdue) {
       
    var $confirmation = $("<div id='CollectCashFromCustomerPopup'></div>");
  
    $('#WithdrawFromCard').val("0.00");
    $("input#CashCollected").val("0.00");
    var CollectCashFromCustomerPopupURL = gprCollectCashFromCustomerPopupURL;
    $confirmation.empty();
    $confirmation.dialog({
        autoOpen: false,
        title: "Message",
        width: 373,
        draggable: false,
        modal: true,
        minHeight: 150,
        resizable: false,
        closeOnEscape: false,
        open: function (event, ui) {
            var url = CollectCashFromCustomerPopupURL + "?collectcash=" + netdue;
            $confirmation.load(url);
        }
    });
    $confirmation.dialog("open");
    return false;
} 
