$(function () {
	var restFulServiceUrl = NPSbaseURL + "DeviceStatus";
	$.ajax({
		data: "{}",
		dataType: "jsonp",
		type: "GET",
		url: restFulServiceUrl,
		contentType: "application/json; charset=UTF-8",
		processData: true,
		success: function (data) {
			DeviceStatus();
			PDS();
			SDK();
			PS();
			hideSpinner();
		},
		error: function (data) {
			hideSpinner();
			showExceptionPopupMsg('Error while retreiving Epson Diagnostics information');
		}
	});

	
});
//var NPSbaseURL = "https://opt-lap-0126.opteamix.com:18732/Peripheral/";
var isExceptionThrown = 0;
function DeviceStatus() {
	var restFulServiceUrl = NPSbaseURL + "DeviceStatus";
	var out = '<table width="400"><tr><th>Device Name</th><th>Status</th></tr>';
	$.ajax({
		data: "{}",
		dataType: "jsonp",
		type: "POST",
		url: restFulServiceUrl,
		contentType: "application/json; charset=UTF-8",
		processData: true,
		timeout: 5000,
		success: function (data) {
			if (data != null) {
				for (var i = 0; i < data.GetDeviceStatusResult.length; i++) {
					out += '<tr><td>' + data.GetDeviceStatusResult[i].DeviceName + '</td><td>' + (data.GetDeviceStatusResult[i].Status) + '</td></tr>'
				}
				$('#ds').html(out + '</table>');
			}
		},
		error: function (data, errmessage, errdetails) {
			if (isExceptionThrown == 0) {
				showException(errmessage, errdetails);
				isExceptionThrown = 1;
			}
		}
	});
}

function PDS() {
	var restFulServiceUrl = NPSbaseURL + "PDSDriverVerification";
	$.ajax({
		data: "{}",
		dataType: "jsonp",
		type: "POST",
		url: restFulServiceUrl,
		contentType: "application/json; charset=UTF-8",
		processData: true,
		timeout: 5000,
		success: function (data) {
			if (data != null) {
				var out = '<p><ul><li>' + data.PDSDriverVerificationResult.Name + '</li><li>' + data.PDSDriverVerificationResult.Version + '</li></ul></p>';
				$('#pds').html(out);
			}
		},
		error: function (data, errmessage, errdetails) {
			if (isExceptionThrown == 0) {
				showException(errmessage, errdetails);
				isExceptionThrown = 1;
			}
		}
	});
}

function SDK() {
	var restFulServiceUrl = NPSbaseURL + "SDKDriverVerification";
	$.ajax({
		data: "{}",
		dataType: "jsonp",
		type: "POST",
		url: restFulServiceUrl,
		contentType: "application/json; charset=UTF-8",
		processData: true,
		timeout: 5000,
		success: function (data) {
			if (data != null && data.SDKDriverVerificationResult != null && data.SDKDriverVerificationResult.Name != null && data.SDKDriverVerificationResult.Version != null) {
				var out = '<p><ul><li>' + data.SDKDriverVerificationResult.Name + '</li><li>' + data.SDKDriverVerificationResult.Version + '</li></ul></p>';
				$('#sdk').html(out);
			}
		},
		error: function (data, errmessage, errdetails) {
			if (isExceptionThrown == 0) {
				showException(errmessage, errdetails);
				isExceptionThrown = 1;
			}
		}
	});
}

function PS() {
	var restFulServiceUrl = NPSbaseURL + "PSFileVerification";
	var out = '<table width="400"><tr><th>Name</th><th>Type</th><th>Installed</th></tr>';
	$.ajax({
		data: "{}",
		dataType: "jsonp",
		type: "POST",
		url: restFulServiceUrl,
		contentType: "application/json; charset=UTF-8",
		processData: true,
		timeout: 5000,
		success: function (data) {
			if (data != null) {
				for (var i = 0; i < data.PSFileVerificationResult.FileStatus.length; i++) {
					out += '<tr><td>' + data.PSFileVerificationResult.FileStatus[i].Name + '</td><td>' + data.PSFileVerificationResult.FileStatus[i].Type + '</td><td>' + (data.PSFileVerificationResult.FileStatus[i].Exists ? ' Yes' : 'No') + '</td>'
				}
				out += '<tr><th>Installed Path</th><th colspan="2">' + data.PSFileVerificationResult.Path + '</th></tr></table>'
				$('#ps').html(out);
			}
		},
		error: function (data, errmessage, errdetails) {
			if (isExceptionThrown == 0) {
				showException(errmessage, errdetails);
				isExceptionThrown = 1;
			}
		}
	});
}

function showException(errMessage, errDetails) {
	showExceptionPopupMsg(errMessage + errDetails);
	isExceptionThrown = 0;
}

