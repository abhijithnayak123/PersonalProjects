//calling from ApprovedCheckTransaction.cshtml
function showCancelAcceptFeePopup() {
    _initPopUp(CancelAcceptFeeURL);
}

//calling from checkAmount.cshtml
function _showCancelTransaction() {
    _initPopUp(cancelURL);
}

//calling from _AddReceiver.cshtml, _ReceiverForEdit.cshtml
function _showCancelPopUpTransaction() {
    _initPopUp(cancelPopURL);
}

function _initPopUp(popupURL) {
    var $confirmation = $("<div id='dlgCancel'></div>");
    $confirmation.empty();
    $confirmation.dialog({
        autoOpen: false,
        title: "Zeo",
        width: 400,
        draggable: false,
        modal: true,
        minHeight: 150,
        resizable: false,
        closeOnEscape: false,
        open: function (event, ui) {
            $confirmation.load(popupURL);
        }
    });
    $confirmation.dialog("open");
}

//calling from _AddReceiver.cshtml, _ReceiverForAdd.cshtml
function _showCancelReceiverPopUp() {
    var $confirmation = $("<div id='dlgCancel'></div>");
    $confirmation.empty();
    $confirmation.dialog({
        autoOpen: false,
        title: "Zeo",
        width: 400,
        draggable: false,
        modal: true,
        minHeight: 150,
        resizable: false,
        closeOnEscape: false,
        // zIndex: 3000,                
        open: function (event, ui) {
            $(".ui-dialog").css("z-index", 4000);
            $confirmation.load(cancelReceiverURL);
        }
    });
    $confirmation.dialog("open");
}

//calling from AddReceiver.js
function _receiverPopUp() {

    var $confirm = $("<div id='findReceiver'></div>");
    $confirm.empty();
    $confirm.dialog({
        autoOpen: false,
        title: "Create Receiver",
        width: 650,
        draggable: false,
        closeOnEscape: false,
        resizable: false,
        modal: true,
        height: 750,
        open: function (event, ui) {
            $confirm.load(createReceiverURL);
        }
    });

    $confirm.dialog("open");
}

//Calling from Login.cshtml
function PopulateHostName() {
    var localNps = $("#hdnIsNpsLocal").val();
    var baseURL = "https://nps.nexxofinancial.com:18732/Peripheral/";
    var restFulServiceUrl = baseURL + "GetHostName?localNps=" + localNps;
    $.ajax({
        data: "{}",
        dataType: "jsonp",
        type: "GET",
        url: restFulServiceUrl,
        contentType: "application/json; charset=UTF-8",
        processData: true,
        success: function (data) {
            if (data) {
                var hostName = data.GetHostNameResult.HostName;
                $("#HostName").val(hostName);
                return;
            }
        },
        error: function (data, errmessage, errdetails) { }
    });
}

//Calling from EnterNewLocation.cshtml
function saveLocationConfirmation() {
    var $confirm = $("<div id='saveLocationConfirmMsg'></div>");
    $confirm.empty();
    $confirm.dialog({
        autoOpen: false,
        title: "Save Location",
        width: 300,
        draggable: false,
        resizable: false,
        closeOnEscape: false,
        modal: true,
        cache: false,
        open: function (event, ui) {
            $confirm.load(SaveLocationConfirmationURL, { dt: (new Date()).getTime() });
        }
    });
    $confirm.dialog("open");
}

//Calling from _CustomerCardSearch.cshtml
function maskCardNumber(isNewCustomer) {
	var track_data = $("#MaskCardNumber").val();
	var cardNumber = getCardNumber(track_data);

	$("#CardNumber").val('');
    if(cardNumber!=null&& isNewCustomer!=true) //AL-228 changes
    {
        $("#CardNumber").val(cardNumber);

        switch (cardNumber.length)
        {
            case 16:
                setMaxLength(19);
                $("#MaskCardNumber").val("**** **** **** " + cardNumber.substring(12, 16));
                break;
            case 17:
                setMaxLength(20);
                $("#MaskCardNumber").val("**** **** **** *" + cardNumber.substring(13, 17));
                break;
            case 18:
                setMaxLength(21);
                $("#MaskCardNumber").val("**** **** **** **" + cardNumber.substring(14, 18));
                break;
        }
    }
}

function setMaxLength(maxlength) {
    $("#MaskCardNumber").attr("maxLength", maxlength)
}

//Calling from _Receiver.cshtml
function showReceiverCancelPopUp() {
    var lastzindex = $('div[role=dialog]:last')[0].style.zIndex;
    var $confirmationCancel = $("<div id='dlgCancel'></div>");
    $confirmationCancel.empty();
    $confirmationCancel.dialog({
        autoOpen: false,
        title: "Zeo",
        width: 400,
        draggable: false,
        modal: true,
        minHeight: 150,
        resizable: false,
        closeOnEscape: false,
        zIndex: lastzindex,
        open: function (event, ui) {
            $confirmationCancel.load(cancelReceiverURL);
        }
    });
    $confirmationCancel.dialog("open");
}

function showReceiverDialoguePopup() {
    var lastzindex = $('div[role=dialog]:last')[0].style.zIndex;
    var $confirmation = $("<div id='dlgCancel'></div>");
    $confirmation.empty();
    $confirmation.dialog({
        autoOpen: false,
        title: "Zeo",
        width: 400,
        draggable: false,
        modal: true,
        minHeight: 150,
        resizable: false,
        closeOnEscape: false,
        zIndex: lastzindex,
        open: function (event, ui) {
            $confirmation.load(cancelPopURL);
        }
    });
    $confirmation.dialog("open");
}

// this is the method to get card number from track data.
// implemented  by refering http://blog.joelchristner.com/2013/04/simple-method-for-parsing-magnetic.html
function getCardNumber(str) {
    if (str == '')
        return;

    var track_data = "";
    var track1 = "";
    var track1_ccn = "";
    var in_track_1 = true;
    var track1_caret1_found = false;

    track_data = str;

    for (var i = 0; i < track_data.length; i++) {
        if (in_track_1) {
            if (!track1_caret1_found) {
                c = track_data[i];
                if ((c != '%') && (c != 'B') && (c != '^')) {
                    track1_ccn += c;
                }

                if (c == '^') {
                    track1_caret1_found = true;
                    track1 += c;
                    break;
                }
            }
        }
    }
    return track1_ccn;
}