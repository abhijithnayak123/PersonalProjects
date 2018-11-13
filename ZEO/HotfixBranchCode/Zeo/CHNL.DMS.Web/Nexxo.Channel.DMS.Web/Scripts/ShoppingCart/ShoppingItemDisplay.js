function closethis() {
    var dlgCancel = $("#dlgCheckDetails");
    dlgCancel.remove();
}

function displayitemdetails(cartID, SummaryTitle, status) {
    var displayTitle = SummaryTitle;
    var dynamicHeight = 550;
    if (displayTitle == 'Bill Payment')
        dynamicHeight = 350;
    if (displayTitle == 'Process Check')
        dynamicHeight = 450;
    if (displayTitle == 'Prepaid Card')
        var dynamicHeight = 400;
    if (displayTitle == 'Money Order')
        var dynamicHeight = 275;
    var $confirm = $("<div id='dlgCheckDetails'></div>");
    $confirm.empty();
    $confirm.dialog({

        autoOpen: false,
        title: displayTitle,
        width: 600,
        draggable: false,
        modal: true,
        closeOnEscape: false,
        height: dynamicHeight,
        // height: 400,
        //minHeight: 250,
        resizable: false,
        open: function (event, ui) {
                $confirm.load(cartDetailsURL + '?id=' + cartID + '&status=' + status);
        }
    });
    
    $confirm.dialog("open");

}


