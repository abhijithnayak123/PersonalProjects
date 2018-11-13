function TransactionHistoryPopup(transactionId) {
    var $confirm = $("<div id='divTrans'></div>");
    $confirm.empty();
    $confirm.dialog({
        autoOpen: false,
        title: "Transaction History",
        width: 400,
        draggable: false,
        resizable: false,
        closeOnEscape: false,
        modal: true,
        height: 150,
        open: function (event, ui) {
        	var url = TransHistoryPrintURL + '?transactionId=' + transactionId;
            $confirm.load(url);
        }
    });

    $confirm.dialog("open");
}

