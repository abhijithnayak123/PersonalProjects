var EpsonException = {
	MoneyOrder_Scan_Error: '1006.100.8500',
	MoneyOrder_Not_Scaned: '1006.100.8501',
	Service_Connectivity: '1006.100.8502',
	MoneyOrder_Not_Printed: '1006.100.8503',
	Printer_Not_Detected: '1006.100.8504',
	Printer_Not_Accesable: '1006.100.8505',

	Print_Service_Connectivity: '1000.100.8021',
	Check_Error_declined: '1000.100.8022',
	DodFrank_Receipt_Template_Not_Found: '1000.100.8023',
	PS_Not_Running: '1000.100.8024',
	Receipt_Template_Not_Found: '1000.100.8025',
	Printing_Receipt_Error: '1000.100.8026',

	Check_Scan_Error: '1002.100.8250',
	Test_Check_Scan_Error: '1002.100.8251',
	Check_Endosrse_Error: '1002.100.8252',
	Check_Print_Error: '1002.100.8253'

}

var CustomerExceptions = {
	Customer_Card_Error: '1001.100.8603',
	Customer_Profile_Error: '1001.100.8604'
}

var MoneyTransferException = {
	MoneyTransfer_deliveryOptions: '1005.100.8300',
	MoneyTransfer_ActOnMyBehalf: '1005.100.8301',
	MoneyTransfer_ActOnMyBehalf_Yes: '1005.100.8302'
}

var VisaException = {
	Visa_Enter_Valid_Card: '1005.100.8100'
}
function getMessage(messageKey, errorDetails) {
	$.ajax({
		type: "POST",
		dataType: "json",
		async: false,
		contentType: "application/json;",
		data: "{messageKey :'" + messageKey + "'}",
		url: MessageStore_Url,
		success: function (result) {
			if (handleException(result))
				return;
			if (errorDetails) {
				var arr = result.message.split('|');
				arr[3] = arr[3] + ', ' + errorDetails;				
				result.message = arr.join('|');
			}
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
