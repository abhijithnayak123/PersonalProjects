// JScript source code

var sessionTimeoutWarningVal, sessionTimeoutVal, timeMinutes, timeSeconds;

$(document).ready(function () {
    sessionTimeoutWarningVal = sessionTimeoutWarning;
    sessionTimeoutVal = sessionTimeout;

    timeMinutes = (parseInt(sessionTimeoutVal) - parseInt(sessionTimeoutWarningVal));
    timeSeconds = timeMinutes * 60;

    setTimeout('displayWarningTime()', 1000);
});

function displayWarningTime() {
 	$('#sessionTimeoutCountdown').text(mmssFormat(timeSeconds));
 	timeSeconds = timeSeconds - 1;
 	if (timeSeconds > 0) {
 		setTimeout('displayWarningTime()', 1000);
 	}
}

function mmssFormat(time) {
 	var minites = time / 60;
 	var seconds = time % 60;
 	return zeroPad(Math.floor(minites), 2) + " : " + zeroPad(Math.ceil(seconds), 2);
}

function zeroPad(num, places) {
 	var zero = places - num.toString().length + 1;
 	return Array(+(zero > 0 && zero)).join("0") + num;
}

function destroyPopup() {
 	$('#divTimer').dialog('destroy').remove();
 	redirectAfterTimeout();
}

