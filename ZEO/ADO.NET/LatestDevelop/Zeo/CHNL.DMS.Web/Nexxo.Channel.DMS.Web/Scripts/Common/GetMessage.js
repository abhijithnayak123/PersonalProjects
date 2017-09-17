function getMessage(messageKey) {
	$.ajax({
		type: "POST",
		dataType: "json",
		async: true,
		contentType: "application/json;",
		data: "{messageKey :'" + messageKey + "'}",
		url: MessageStore_Url,
		success: function (result) {
			showExceptionPopupMsg(result.message);
		}
	});
}
//Added made by kaushik
function getExceptionMessage(messageKey) {
	var message = '';
	$.ajax({
		type: "POST",
		dataType: "json",
		async: false,
		contentType: "application/json;",
		data: "{messageKey :'" + messageKey + "'}",
		url: MessageStore_Url,
		success: function (result) {
			message = result.message;
		}
	});
	return message;
}