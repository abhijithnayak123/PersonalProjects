$(document).ready(function () {
    $(document).on('click', '.msgCenterTicketNumber', function () {
        openChatDialog(this);
        $('.ui-widget-overlay').hide();
    });

    $.ajaxSetup({ cache: false });
    messageCenter();

    initializeChannelPartner();
    updateMessageStatus();
});

// service call to inlialize partner
function initializeChannelPartner() {
	if (channelPartnerId && checkProcessorUrl) {
		if (NPSbaseURL)
		{
			var restServiceUrl = NPSbaseURL + "InitializeChannelPartner?channelpartnerid=" + channelPartnerId + "&serviceurl=" + checkProcessorUrl;
			$.ajax({
				data: "{}",
				dataType: "jsonp",
				type: "GET",
				url: restServiceUrl,
				contentType: "application/json; charset=UTF-8",
				processData: true,
				success: function (data, textStatus, jqXHR) {
				},
				error: function () {
				}
			});
		}
    }
}

function changeMessageStatusImage(ticketNumber) {
    var img = $('.msgCenterTable img[alt=' + ticketNumber + ']');
    img[0].src = ImageBaseUrl + "Chat_Green.png";
}

function openChatDialog($this) {
    $('#chatCenter').dialog('destroy').remove();
    var transactionId = $this.id;
    var $chatCenter = $("<div id='chatCenter'></div>");
    $chatCenter.empty();

    $chatCenter.dialog({
        autoOpen: false,
        title: "Chat",
        width: 505,
        draggable: true,
        modal: false,
        resizable: false,
        closeOnEscape: false,
        minHeight: 225,
        scroll: false,
        cache: false,
        open: function (event, ui) {
            $chatCenter.load(ChatCenterMsgPopupURL + '?transactionId=' + transactionId, { dt: (new Date()).getTime() });
            sessionStorage.setItem("isChatOpen", true);
            sessionStorage.setItem("ticketNumber", transactionId);
            var elem = $(this);
            sessionStorage.setItem("left", elem.offset().left);
            sessionStorage.setItem("top", elem.offset().top);
        },
        close: function (event, ui) {
            sessionStorage.removeItem("isChatOpen");
            sessionStorage.removeItem("left");
            sessionStorage.removeItem("top");
        },
        dragStop: function (event, ui) {
            var elem = $(this);
            sessionStorage.setItem("left", elem.offset().left);
            sessionStorage.setItem("top", elem.offset().top);
        }
    });
    $chatCenter.dialog("open");
}

function messageCenter() {
    showSpinners($("#loadingMessageCenter"));
    $.ajaxSetup({ cache: false });
    $.getJSON(url, function (data) {
        $("#tbodymessagecenter").empty();
        var tickets = new Array();
        $.each(data, function (i, item) {
            var CustomerLastName = item.CustomerLastName.charAt(0).toUpperCase() + item.CustomerLastName.substr(1, item.CustomerLastName.length).toLowerCase();
            if (item.CustomerLastName.length > 6)
                CustomerLastName = CustomerLastName.substr(0, 6);
            var name = item.CustomerFirstName + " " + item.CustomerLastName;
            var html = "<tr><td class='msgCenterFirstName' title='" + name + "'>" + item.CustomerFirstName.charAt(0).toUpperCase() + " " + CustomerLastName + "</td>";
            html += "<td class='msgCenterAmount'>" + item.Amount + "</td>";
            if (item.TransactionState == "Pending") {
            	html += "<td class='msgCenterTransactionImage'>" + ' <img src="' + ImageBaseUrl + 'HourglassIcon.png" class="msgCenterStatus" width="16" height="16" alt="Process Check" Title="Pending"/>' + "</td>";
                html += "<td class='msgCenterTicketNumber' id='" + item.TransactionId + "'>" + ' <img alt="' + item.TicketNumber + '" src="' + ImageBaseUrl + 'Chat_White.png" alt="' + item.TicketNumber + '" />' + "</td></tr>";
            } else if (item.TransactionState == "Approved") {
               	html += "<td class='msgCenterTransactionImage'>" + ' <img src="' + ImageBaseUrl + 'Approved_Icon.png" class="msgCenterStatus" width="16" height="16" alt="Process Check" Title="Approved"/>' + "</td>";
            } else {
               	html += "<td class='msgCenterTransactionImage'>" + ' <img src="' + ImageBaseUrl + 'Declined_Icon.png" class="msgCenterStatus" alt="Process Check" width="16" height="16" Title="' + item.DeclineMessage + '"/>' + "</td>";
            }
            $("#grdMessageCenter tbody").append(html);
            tickets.push(item.TicketNumber);
        });
        ticketNumbers = tickets.join();
        hideSpinners($("#loadingMessageCenter"));
        getMessageStatus(ticketNumbers);

        clearInterval(refreshMsgCenter);
        if (data.length > 0) {
            refreshMsgCenter = setInterval(function () { messageCenter(); }, parseFloat(messageCenterRefreshTime) * 60 * 1000);
        }
    });
}

function getMessageStatus(ticketNumbers) {
	// service call to update map with tockenNo, employeeId & ticketNumbers
	//var methodInfo = "/CheckMessageStatus?channelpartnerid=33&tokenno=F59DD48B&employeeid=897&ticketnos=39459";
	var methodInfo = "CheckMessageStatus?channelpartnerid=" + channelPartnerId + "&tokenno=" + checkProcessorTocken + "&employeeid=" + employeeId + "&ticketnos=" + ticketNumbers;
	if (NPSbaseURL) {
		var restServiceUrl = NPSbaseURL + methodInfo;
			$.ajax({
				data: "{}",
				dataType: "jsonp",
				type: "GET",
				url: restServiceUrl,
				contentType: "application/json; charset=UTF-8",
				processData: true,
				success: function (data, textStatus, jqXHR) {
					if (!data.ErrorMessage) {
						$.each(data.CheckMessageStatusResult, function (i, item) {
							if (item.HasNewMessage) {
								changeMessageStatusImage(item.TicketNo);
							}
						});
					}
				},
				error: function () {
				}
			});
		}
	}


function updateMessageStatus() {
    setTimeout(function () { getMessageStatus(ticketNumbers); }, parseFloat(messageCenterRefreshTime) * 60 * 1000);
}