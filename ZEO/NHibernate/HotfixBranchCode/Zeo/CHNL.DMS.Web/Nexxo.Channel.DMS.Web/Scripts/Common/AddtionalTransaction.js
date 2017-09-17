
$(document).ready(function () {
    $('#AddTransaction').bind('click', function () {
        AddTransactionDialog();
    });
});

function AddTransactionDialog() {
    
    var $confirmation = $("<div id='dlgCancel'></div>");
    $confirmation.empty();
    $confirmation.dialog({
        autoOpen: false,
        title: "MGiAlloy",
        width: 400,
        draggable: false,
        modal: true,
        minHeight: 150,
        open: function (event, ui) {
            $confirmation.load('/AddTransaction/AddTransaction');
        }
    });
    $confirmation.dialog("open");
}