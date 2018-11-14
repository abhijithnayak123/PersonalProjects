function GenericPopUp(url, newtitle) {
	var $confirm = $("<div id='divPopUp'></div>");
	$confirm.empty();
	$confirm.dialog({
		autoOpen: false,
		title: newtitle,
		width: 425,
		draggable: false,
		resizable: false,
		closeOnEscape: false,
		modal: true,
		height: 225,
		cache: false,
		open: function (event, ui) {
			$confirm.load(url,
				function (responseText, textStatus, XMLHttpRequest) {
					var data = parseData(responseText);
					if (data && data.success == false) {
						RemovePopUp();
						showExceptionPopupMsg(data.data);
					}
				});
		}
	});
	$confirm.dialog("open");
}

function ShowPopUp(url, newtitle, newwidth, newheight) {
	var $confirm = $("<div id='divPopUp'></div>");
	$confirm.empty();
	$confirm.dialog({
		autoOpen: false,
		title: newtitle,
		width: newwidth,
		draggable: false,
		resizable: false,
		closeOnEscape: false,
		modal: true,
		height: newheight,
		cache: false,
		open: function (event, ui) {
			$confirm.load(url,
				function (responseText, textStatus, XMLHttpRequest) {
					var data = parseData(responseText);
					if (data && data.success == false) {
						RemovePopUp();
						showExceptionPopupMsg(data.data);
					}
				});
		}
	});
	$confirm.dialog("open");
}

function ShowPopUpdata(url, newtitle, newwidth, newheight, dataValue) {
	var $confirm = $("<div id='divPopUp'></div>");
	$confirm.empty();
	$confirm.dialog({
		autoOpen: false,
		title: newtitle,
		width: newwidth,
		draggable: false,
		resizable: false,
		closeOnEscape: false,
		modal: true,
		height: newheight,
		cache: false,
		open: function (event, ui) {
			$confirm.load(url, { "data": dataValue },
				function (responseText, textStatus, XMLHttpRequest) {
					var data = parseData(responseText);
					if (data && data.success == false) {
						RemovePopUp();
						showExceptionPopupMsg(data.data);
					}
				});
		}
	});
	$confirm.dialog("open");
}

function ShowPopUpMinHeight(url, newtitle, newwidth, minheight) {
    var $confirm = $("<div id='divPopUp'></div>");
    $confirm.empty();
    $confirm.dialog({
        autoOpen: false,
        title: newtitle,
        width: newwidth,
        draggable: false,
        resizable: false,
        closeOnEscape: false,
        modal: true,
        minHeight: minheight,
        cache: false,
        open: function (event, ui) {
            $confirm.load(url,
				function (responseText, textStatus, XMLHttpRequest) {
				    var data = parseData(responseText);
				    if (data && data.success == false) {
				        RemovePopUp();
				        showExceptionPopupMsg(data.data);
				    }
				});
        }
    });
    $confirm.dialog("open");
}

function RemovePopUp() {
	$('#divPopUp').dialog('destroy').remove();
}

function RemoveDynIdPUP(idpopup) {
    $("#"+idpopup+ "").dialog('destroy').remove();
}

function parseData(data) {
	try {
		var obj = JSON.parse(data);
		return obj;
	} catch (ex) {
		return null;
	}
}


function ShowPopUpdataMinHeight(url, newtitle, newwidth, minheight, dataValue) {
   
    var $confirm = $("<div id='divPopUp'></div>");
    $confirm.empty();
    $confirm.dialog({
        autoOpen: false,
        title: newtitle,
        width: newwidth,
        draggable: false,
        resizable: false,
        closeOnEscape: false,
        modal: true,
        minheight: minheight,
        cache: false,
        open: function (event, ui) {
            $confirm.load(url, { "data": dataValue },
				function (responseText, textStatus, XMLHttpRequest) {
				    var data = parseData(responseText);
				    if (data && data.success == false) {
				        RemovePopUp();
				        showExceptionPopupMsg(data.data);
				    }
				});
        }
    });
    $confirm.dialog("open");
}

function ShowPUPWithDynId(url, newtitle, newwidth, minheight, idpopup) {
    var $confirm = $("<div id=" + idpopup + "></div>");
    $confirm.empty();
    $confirm.dialog({
        autoOpen: false,
        title: newtitle,
        width: newwidth,
        draggable: false,
        resizable: false,
        closeOnEscape: false,
        modal: true,
        minHeight: minheight,
        cache: false,
        open: function (event, ui) {
            $confirm.load(url,
				function (responseText, textStatus, XMLHttpRequest) {
				    var data = parseData(responseText);
				    if (data && data.success == false) {
				        RemoveDynIdPUP(idpopup);
				        showExceptionPopupMsg(data.data);
				    }
				});
        }
    });
    $confirm.dialog("open");
}

function DisplayMessage(message, btnText, newtitle, newwidth, minheight)
{
    var $confirm = $("<div id='divDisplayMessage'></div>");
    $confirm.empty();
    $confirm.dialog({
        autoOpen: false,
        title: newtitle,
        width: newwidth,
        draggable: false,
        resizable: false,
        closeOnEscape: false,
        modal: true,
        minHeight: minheight,
        cache: false,
        open: function (event, ui) {
            $confirm.load(dispalyMessage, { "message": message, "btnText": btnText},
				function (responseText, textStatus, XMLHttpRequest) {
				    var data = parseData(responseText);
				    if (data && data.success == false) {
				        RemoveDynIdPUP("divDisplayMessage");
				        showExceptionPopupMsg(data.data);
				    }
				    $('#btnOk').focus();
				});
        }
    });
    $confirm.dialog("open");
}

function showSSNRequiredPopup(url, newtitle, newwidth, minheight, message, btnText) {
    var $confirmation = $("<div id='divDisplayMessage'></div>");
    $confirmation.empty();
    $confirmation.dialog({
        autoOpen: false,
        title: newtitle,
        width: newwidth,
        draggable: false,
        modal: true,
        resizable: false,
        closeOnEscape: false,
        minHeight: minheight,
        scroll: false,
        cache: false,
        open: function (event, ui) {
            $confirmation.load(url, { "msg": message, "btnText": btnText },
                 function (responseText, textStatus, XMLHttpRequest) {
                     var data = parseData(responseText);
                     if (data && data.success == false) {
                         RemoveDynIdPUP("divDisplayMessage");
                         showExceptionPopupMsg(data.data);
                     }
                     $('#btnOk').focus();
            });
        }
    });
    $confirmation.dialog("open");
    return false;
}