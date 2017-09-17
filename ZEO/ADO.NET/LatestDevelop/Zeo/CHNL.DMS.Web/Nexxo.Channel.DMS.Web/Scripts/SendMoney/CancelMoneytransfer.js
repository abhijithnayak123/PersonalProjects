$(document).ready(function () {

	$('.cancelMoneyTransfer').bind('click', function () {
		var item = this;
		showCancelDialogue(item.id, item.name);
		return false;
	});
});

function showCancelDialogue(id, name) {
	var $confirmation = $("<div id='dlgCancelMT'></div>");
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
			var url = CancelMTURL + '?id=' + id + '&screenName=' + name;
			$confirmation.load(url);
		}
	});
	$confirmation.dialog("open");
}