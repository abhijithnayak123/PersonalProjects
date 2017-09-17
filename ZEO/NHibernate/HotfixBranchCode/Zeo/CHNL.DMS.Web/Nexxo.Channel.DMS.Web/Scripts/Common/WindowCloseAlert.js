$(document).ready(function () {
    $('a').live('click', function () {
        showCartAbandonmentConfirm = false;
    });

    $('form').live('submit', function () {

        showCartAbandonmentConfirm = false;
    });
    // AL-4034 : window.event is used for IE browser(click on browser back button) only and else part have if condition access for other browser ex. chrome, firefox
    // User clicks on browser back it's redirect to logout page.
    $(window).bind('beforeunload', function (eventObject) {
        if (window.event) {
            if (window.event.clientX < 40 && window.event.clientY < 0) {
                showCartAbandonmentConfirm = false;
                UpdateCounterId()
                logOutOnBackButton()
            }
        }
        else {
            if (event.currentTarget.performance.navigation.type == 2) {
                UpdateCounterId()
                RedirectToUrl(LogoutUrl);
            }
        }
        var returnValue = undefined;
        if (showCartAbandonmentConfirm) {
            returnValue = windowCloseAlertMessage;
        }
        eventObject.returnValue = returnValue;
        return returnValue;
    });

    $(window).bind('unload', function (eventObject) {
        if (showCartAbandonmentConfirm) {
            parkAndDeleteShoppingCartItemsWithoutRedirect();
        }
    });
});
