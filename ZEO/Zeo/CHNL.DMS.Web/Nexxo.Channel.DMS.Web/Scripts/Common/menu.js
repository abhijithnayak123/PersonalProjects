// JScript source code
$(document).ready(function () {

    $(".disableLink").click(function () {
        return false;
    });
    $('#hardwareNthl').hover(function () {
        $('#hardware_txt').css({ "color": "#FFFFFF" });
        $(this).find('.sf-menu').show();
    });
    $('#hardwareNthl').mouseleave(function () {
        $('#hardware_txt').css({ "color": "#006837" });
        $(this).find('.sf-menu').hide();
    });

    $('#maintenance').hover(function () {
        $('#maintenance_txt').css({ "color": "#FFFFFF" });
        $(this).find('.sf-menu').show();
    });
    $('#maintenance').mouseleave(function () {
        $('#maintenance_txt').css({ "color": "#006837" });
        $(this).find('.sf-menu').hide();
    });

    //console.log("isTerminalSetup:" + isTerminalSetup + ", isPeripheralServerSetUp:" + isPeripheralServerSetUp);
    if ((isTerminalSetup && isPeripheralServerSetUp)) {
        $('#liDiagnostics').css('display', '');
        $('#liPeripheralServer').css('display', '');
        $('#liTerminal').css('display', '');
    }
    else {
        $('#liDiagnostics').css('display', 'none');
        $('#liPeripheralServer').css('display', '');
        $('#liTerminal').css('display', '');
    }
});

function redirectSessionWarning() {
	clearTimeout(rotateSession);
	var sTimeout = parseInt(sessionTimeoutWarning) * 60 * 1000;
	rotateSession = setTimeout('showSessionWarningPopup()', sTimeout);
}

function _sessionWarnPopup(SessionWarningURL) {
    //console.log("SessionWarningURL:" + SessionWarningURL); 

    var $confirm = $("<div id='divTimer'></div>");
    $confirm.empty();
    $confirm.dialog({
        autoOpen: false,
        title: 'Session Timeout',
        width: 400,
        draggable: false,
        resizable: false,
        closeOnEscape: false,
        modal: true,
        height: 200,
        cache: false,
        open: function (event, ui) {
            $confirm.load(SessionWarningURL);
        }
    });

    $confirm.dialog("open");
    return false;
}
// AL-4034 : window.event is used for IE browser only and else part have if condition is access for other browser ex. chrome, firefox
// User clicks on browser back it's redirect to logout page.
function logOutOnBack() {
	if (window.event) {
		if (window.event.clientX < 40 && window.event.clientY < 0) {
			UpdateCounterId()
			RedirectToUrl(LogoutUrl);
		}
	}
	else {
		if (event.currentTarget.performance.navigation.type == 2) {
			UpdateCounterId()
			RedirectToUrl(LogoutUrl);
		}
	}
}
