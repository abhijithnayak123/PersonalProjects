function showSwipeMsg() {
    var $confirmation = $("<div id='SwipeMessage'></div>");
    $confirmation.empty();
    $confirmation.dialog({
        autoOpen: false,
        title: "Message",
        width:321 ,
        draggable: false,
        modal: true,
        minHeight: 150,
        resizable: false,
		closeOnEscape: false,
        open: function (event, ui) {
            $confirmation.load(swipeURL);
        }
    });
    $confirmation.dialog("open");
    return false;
}

function showIDConfirmationMsg() {
    var $confirmation = $("<div id='IDConfirmMessage'></div>");
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
            $confirmation.load(idConfirmationURL);
        }
    });
    $confirmation.dialog("open");
}

function SwipeCard(baseURL) {
    $.ajax({
        data: "{}",
        dataType: "jsonp",
        type: "GET",
        url: baseURL + "ScanCard",
        contentType: "application/json; charset=UTF-8",
        processData: true,
        success: function (data) {
            if (data.ErrorNo != undefined) {
                showExceptionPopupMsg(data.ErrorDetails);
                var swipeSearch = '@Url.Action("SearchCustomerFromSwipe","CustomerSearch")';
                swipeSearch += '?Number=' + data.card_number+'&CVV='+$('#CVV').val();
                location.href = swipeSearch;
                return;
            }
        },
        error: function (data, errmessage, errdetails) {
            if (data.ErrorNo != undefined) {
                showExceptionPopupMsg(data.ErrorDetails);
                return;
            }
            else {
                showExceptionPopupMsg("Connectivity Error");
                return;
            }
        }
    });
}


