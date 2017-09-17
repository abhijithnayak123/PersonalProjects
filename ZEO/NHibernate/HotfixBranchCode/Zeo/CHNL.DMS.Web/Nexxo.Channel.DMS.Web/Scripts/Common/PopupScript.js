function GenericPopUp(url, newtitle)
{
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
			$confirm.load(url);
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
			$confirm.load(url);
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
			$confirm.load(url, { "data": dataValue });
		}
	});
	$confirm.dialog("open");
}

function RemovePopUp() {
	$('#divPopUp').dialog('destroy').remove();
}