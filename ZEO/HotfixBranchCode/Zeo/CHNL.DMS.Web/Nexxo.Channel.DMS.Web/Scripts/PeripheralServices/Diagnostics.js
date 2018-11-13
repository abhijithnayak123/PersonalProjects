$(document).ready(function () {
    $("#divUserSessionId").css("display", "");
	showSpinner();
	var peripheralDiagnosticsUrl = NPSbaseURL + 'PrinterDiagnostics?printparams=' + JSON.stringify('test');
	$.ajax({
		data: "{}",
		dataType: "jsonp",
		type: "GET",
		url: peripheralDiagnosticsUrl,
		contentType: "application/json; charset=UTF-8",
		processData: true,
		success: function (diagnostics) {
		  	    if (handleException(diagnostics)) {
		  	        hideSpinner();
		        return;
		    }
			if (diagnostics.ErrorNo != undefined) {
				hideSpinner();
				showExceptionPopupMsg('Error processing while retreiving peripheral diagnostics information');
				return;
			}
			BindDiagnostics(diagnostics);
			hideSpinner();
		},
		error: function (data) {
		    hideSpinner();
			showExceptionPopupMsg('Error processing while retreiving peripheral diagnostics information');
		}
	});
});

function BindDiagnostics(diagnostics) {
	$('#dvPeripheralService').html((diagnostics.PrinterDiagnosticsResult.Status == "Connected") ? "Running" : "");
	$('#dvDeviceName').html(diagnostics.PrinterDiagnosticsResult.DeviceName).addClass("cFont");
	$('#dvDeviceStatus').html(diagnostics.PrinterDiagnosticsResult.DeviceStatus).addClass("cFont");
	$('#dvStatus').html(diagnostics.PrinterDiagnosticsResult.Status);
	$('#dvFirmware').html(diagnostics.PrinterDiagnosticsResult.FirmwareRevision);
	$('#dvSerial').html(diagnostics.PrinterDiagnosticsResult.SerialNumber);
	$('#dvVersion').html(diagnostics.PrinterDiagnosticsResult.NpsVersion);
	if (diagnostics.PrinterDiagnosticsResult.DeviceName) {
		$('#divDeviceName').html(diagnostics.PrinterDiagnosticsResult.DeviceName + ":").addClass("labelTextForRpt");
		$('#divDeviceStatus').html(diagnostics.PrinterDiagnosticsResult.DeviceName + " Status:").addClass("labelTextForRpt margin_right_0");
	}
}