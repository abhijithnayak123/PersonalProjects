$(document).ready(function () {
    $("#divUserSessionId").css("display", "");
    $('#CancelTransaction').bind('click', function () {
        showDialogue();
    });
    $('#CancelTransactionPopUp').bind('click', function () {
        showDialoguePopup();
    });
    $('#doneTransaction').bind('click', function () {
        showDoneDialogue();
    });
    $('.del_img').bind('click', function () {
    	var item = this;
    	showDeleteDialogue(item.id, item.name, item.attributes['product'].value);
    	return false;
    });

    $('.deleteItemImg').bind('click', function () {
    	var item = this;
    	showDeleteDialogueAlert(item.id, item.name, item.attributes['product'].value);
    	return false;
    });

    $('.parkImg').bind('click', function () {
    	var item = this;
    	showParkDialogue(item.id, item.name, item.attributes['product'].value, item.attributes['status'].value);
    	return false;
    });
  

    $('.deleteItemImg').bind('click', function () {
    	var item = this;
    	showDeleteDialogueAlert(item.id, item.name, item.attributes['product'].value);
    	return false;
    });

    $('.parkImg').bind('click', function () {
    	var item = this;
    	showParkDialogue(item.id, item.name, item.attributes['product'].value, item.attributes['status'].value);
    	return false;
    });
});

function showDoneDialogue() {
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
            $confirmation.load(doneURL);
        }
    });
    $confirmation.dialog("open");
}

function closethis() {
    var dlgCancel = $("#dlgCancel");
    var dialogBlock = $("div[role=dialog]");
    dlgCancel.remove();
    dialogBlock.remove();
}

function showDialogue() {
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
            $confirmation.load(cancelURL);
        }
    });
    $confirmation.dialog("open");
}

function showDialoguePopup() {
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
            $(".ui-dialog").css("z-index", 4000);
            $confirmation.load(cancelPopURL);
        }
    });
    $confirmation.dialog("open");
}

function showDeleteDialogue(id, screenName, product, status) {	
	var $confirmation = $("<div id='dlgRemoveCheck'></div>");
    $confirmation.empty();
    $confirmation.dialog({
        autoOpen: false,
        title: "Zeo",
        width: 450,
        draggable: false,
        modal: true,
        resizable: false,
		closeOnEscape: false,
        minHeight: 150,
        open: function (event, ui) {
        	var url = DeleteURL + '?id=' + id + '&screenName=' + screenName + '&product=' + product + '&status=' + status;
            $confirmation.load(url);
        }
    });
    $confirmation.dialog("open");
}

function showDeleteDialogueAlert(id, screenName, product) {
    var $confirmation = $("<div id='dlgCancel'></div>");
    $confirmation.empty();
    $confirmation.dialog({
        autoOpen: false,
        title: "Zeo",
        width: 400,
        draggable: false,
        modal: true,
        resizable: false,
        closeOnEscape: false,
        minHeight: 150,
        open: function (event, ui) {
            var url = DeleteCartItemAlertURL + '?id=' + id + '&screenName=' + screenName + '&product=' + product;
            $confirmation.load(url);
        }
    });
    $confirmation.dialog("open");
}

function showParkDialogue(id, screenName, product, status) {
    var $confirmation = $("<div id='dlgCancel'></div>");
    $confirmation.empty();
    $confirmation.dialog({
        autoOpen: false,
        title: "Zeo",
        width: 460,
        draggable: false,
        modal: true,
        resizable: false,
        closeOnEscape: false,
        minHeight: 150,
        open: function (event, ui) {
            var url = ParkURL + '?id=' + id + '&screenName=' + screenName + '&product=' + product + '&status=' + status;
            $confirmation.load(url);
        }
    });
    $confirmation.dialog("open");
}