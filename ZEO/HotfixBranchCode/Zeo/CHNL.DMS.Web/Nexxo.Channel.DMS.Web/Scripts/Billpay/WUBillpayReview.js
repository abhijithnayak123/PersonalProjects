$(document).ready(function () {
    $('.cancelBillPayReview').bind('click', function (e) {
        var item = this;
        showCancelDialogue(item.id);
        e.preventDefault();
    });


    //show cancel BillpayReviewTransaction
    function showCancelDialogue(id) {
        var $confirmation = $("<div id='dlgCancel'></div>");
        $confirmation.empty();
        $confirmation.dialog({
            autoOpen: false,
            title: "Message",
            width: 400,
            draggable: false,
            modal: true,
            resizable: false,
            closeOnEscape: false,
            minHeight: 150,
            open: function (event, ui) {
                var url = CancelBillPayReviewTransaction + '?id=' + id;
                $confirmation.load(url);
            }
        });
        $confirmation.dialog("open");
    }
});