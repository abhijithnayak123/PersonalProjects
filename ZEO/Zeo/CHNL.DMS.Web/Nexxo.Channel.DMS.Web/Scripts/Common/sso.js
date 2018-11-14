$(document).ready(function () {   
    if (isAnyError && isAnyError == 'True') {
            PopulateHostName(fx, this);
    }
});

function fx(document) {
    document.forms[0].submit();
}

function PopulateHostName(fx, doc) {   
    var localNps = $("#hdnIsNpsLocal").val();
    var restFulServiceUrl = PSBaseUrl + "GetHostName?localNps=" + localNps;
    $.ajax({
        data: "{}",
        dataType: "jsonp",
        type: "GET",
        url: restFulServiceUrl,
        contentType: "application/json; charset=UTF-8",
        processData: true,
        timeout: 5000,
        success: function (data) {
            if (data) {
                var hName = data.GetHostNameResult.HostName;
                $("#hostName").val(hName);
                fx(doc);
                return;
            }
        },
        error: function (data, errmessage, errdetails) { fx(doc); }
    });
}


function DisableBackButton() {
    window.history.forward()
}

DisableBackButton();
window.onload = DisableBackButton;
window.onpageshow = function (evt) { if (evt.persisted) DisableBackButton() }
window.onunload = function () { void (0) }
