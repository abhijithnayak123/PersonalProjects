var isStatusChanged = false;
$(document).ready(function () {
	populateChat();
	$('textarea[placeholder]').placeholder();
	$().maxlength();
	$('#chatCenter').parent().addClass('chatDialogBorder').removeClass('ui-corner-all');
	$('#chatCenter').addClass('chatDialogBorderInside').removeClass('ui-dialog-content');

	var pollMessageHandler = setInterval(function () { getMessages(ticketNumber); }, parseFloat(messageCenterRefreshTime) * 60 * 1000);

});

function populateChat() {
	getMessages(ticketNumber);
	$("#txtTextMessage").keypress(function (e) {
		if (e.which == 13) {
			var textMsg = $("#txtTextMessage").val();
			var ticketNo = $("#hdnTicketNumber").val();
			composeMessage(ticketNo, textMsg);
			e.preventDefault();
			$("#txtTextMessage").val('');
			$('#chatMessagePanel').empty();
		}
	});
}

function getMessages(ticketNo) {
	// service call to get for specific ticketNo
	var restServiceUrl = NPSbaseURL + "CheckForMessages?ticketno=" + ticketNo;
	showSpinners($('#loadingShadow'));
	$.ajax({
		data: "{}",
		dataType: "jsonp",
		type: "GET",
		url: restServiceUrl,
		contentType: "application/json; charset=UTF-8",
		timeout: 3000, // sets timeout to 3 seconds
		processData: true,
		async: true,
		success: function (data, textStatus, jqXHR) {
			var lastMessageId = '';
			if (!data.ErrorMessage) {
				messages = data.CheckForMessagesResult;
				if (messages.length == 0) {
					if (isStatusChanged == true) {
						ShowPopUp(checkStatusURL, "System Message", 300, 120);
						$('#chatMessagePanel').empty();
						isStatusChanged = false;
					}
					hideSpinners($('#loadingShadow'));
					return true;
				}
				isStatusChanged = true;
				$('#chatMessagePanel').empty();
				lastMessageId = messages[messages.length - 1].MessageId;
				var chatPannel = ''
				$.each(data.CheckForMessagesResult, function (i, msg) {
					var name = msg.IsIncoming ? 'Ingo CSR' : 'Me';
					var cssclass = msg.IsIncoming ? '' : 'chatMe';
					chatPannel += '<tr class=' + cssclass + '><td style="width: 20%;">' + name + '</td><td style="width: 55%;">' + msg.Text + '</td><td style="width: 25%; ">' + formatToDate(msg.Date) + '</td></tr>'
				});
				$('#chatMessagePanel').append(chatPannel);
			}
			else
			{
			    $('#chatMessagePanel').empty();
			    var chatPannel = '<tr class="chatMe"><td style="width: 20%;"></td><td style="width: 80%;">' + data.ErrorMessage + '</td></tr>';
			    $('#chatMessagePanel').append(chatPannel);
			}
			var box = document.getElementById('chatPanel');
			if (box) {
				box.scrollTop = box.scrollHeight;
			}
			hideSpinners($('#loadingShadow'));
			if (lastMessageId) {
				confirmMessages(ticketNo, lastMessageId);
			}
		},
		error: function (x, t, m) {
		    $('#chatMessagePanel').empty();
		    var chatPannel = '<tr class="chatMe"><td style="width: 20%;"></td><td style="width: 80%;"> The chat functionality is unavailable at this time. <br /><br />Please contact INGO Money directly for immediate assistance.</td></tr>';
		    $('#chatMessagePanel').append(chatPannel);
			hideSpinners($('#loadingShadow'));
		}
	});
}

function formatToDate(dateString) {
	var datetime = new Date(parseInt(dateString.substr(6)))
	var formatDate = formatAMPM(datetime) + " (" + formatLeadingZero((datetime.getMonth() + 1)) + "/" + formatLeadingZero(datetime.getDate()) + ")";
	return formatDate;
}

function formatAMPM(date) {
	var hours = date.getHours();
	var minutes = date.getMinutes();
	var ampm = hours >= 12 ? 'pm' : 'am';
	hours = hours % 12;
	hours = hours ? hours : 12; // the hour '0' should be '12'
	minutes = minutes < 10 ? '0' + minutes : minutes;
	var strTime = formatLeadingZero(hours) + ':' + formatLeadingZero(minutes) + ' ' + ampm;
	return strTime;
}

function formatLeadingZero(number) {
	return ('0' + number).slice(-2);
}

function composeMessage(ticketNo, message) {
	var restServiceUrl = NPSbaseURL + "ComposeMessage?ticketno=" + ticketNo + "&message=" + message;
	$.ajax({
		data: "{}",
		dataType: "jsonp",
		type: "GET",
		url: restServiceUrl,
		contentType: "application/json; charset=UTF-8",
		processData: true,
		async: false,
		success: function (data, textStatus, jqXHR) {
			if (data)
				getMessages(ticketNo);
		},
		error: function () {
			//*** write a code to show exception popup
		}
	});
}

function confirmMessages(ticketNo, lastMessageId) {
	var restServiceUrl = NPSbaseURL + "ConfirmAllMessages?ticketno=" + ticketNo + "&lastmessageid=" + lastMessageId;
	$.ajax({
		data: "{}",
		dataType: "jsonp",
		type: "GET",
		url: restServiceUrl,
		contentType: "application/json; charset=UTF-8",
		processData: true,
		cache: false,
		success: function (data, textStatus, jqXHR) {
		},
		error: function () {
			//*** write a code to show exception popup
		},
		timeout: 10000
	});
}


// private method to invoke NPS service
function invokeService(methodInfo, data) {
	var serviceUrl = NPSbaseURL + methodInfo;

	$.ajax({
		data: "{}",
		dataType: "jsonp",
		type: "GET",
		url: baseURL + "ScanCheck?scanparams=" + JSON.stringify(scanParams),
		contentType: "application/json; charset=UTF-8",
		processData: true,
		success: function (data, textStatus, jqXHR) {

		},
		error: function () {

		},
		timeout: 10000
	});
}

function closeChatDialog() {
	$('#chatCenter').dialog('close').remove();
	clearInterval(pollMessageHandler);
}
