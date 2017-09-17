
$(document).ready(function () {
	var rowsToColor = [];
	var flag = 0;
	// Set up the jquery grid
	$("#jqTable").jqGrid({

		// Ajax related configurations
		url: editReceiverJqDataUrl,
		datatype: "json",
		mtype: "POST",

		// Specify the column names
		colNames: ["First Name", "Last Name", "Country", "State", "City"],

		// Configure the columns
		colModel: [
        { name: "FirstName", index: "FirstName", width: 150, align: "left" },
        { name: "LastName", index: "LastName", width: 180, align: "left" },
        { name: "Country", index: "Status", width: 200, align: "center" },
        { name: "StateProvince", index: "StateProvince", width: 160, align: "center" },
        { name: "City", index: "City", width: 160, align: "center" }
        ],

		// Grid total width and height
		cmTemplate: { sortable: false },
		width: 600,
		height: 200,

		// Paging
		toppager: false,
		pager: jQuery("#jqTablePager"),
		rowNum: 5,
		rowList: [5, 10, 20],
		viewrecords: true, // Specify if "total number of records" is displayed

		// Default sorting
		// sortname: "FName",
		// sortorder: "asc",

		// Grid caption
		caption: "",
		loadComplete: function () {
			$("tr.jqgrow:even").css("background", "#ebf0ee");
		}
	}).navGrid("#jqTablePager",
        { refresh: false, search: false, add: false, edit: false, del: false },
            {}, // settings for edit
            {}, // settings for add
            {}, // settings for delete
            {sopt: ["cn"]} // Search options. Some options can be set on column level
        );
});

function placeRadioButton(cellvalue, options, rowObject) {
	if (rowObject[3] === true)
		return '<input type="radio" id="radioSelect" name="select" value="' + rowObject[0] + '"/>'
	else
		return '<input type="radio" id="radioSelect" disabled="true" name="select" value="' + rowObject[0] + '"/>'
}



//Reload grid based on searched value
function findReceiver() {
	var canRef = document.getElementById("FindReceiver");
	canRef.addEventListener("click", reloadGrid(), false);
}

function reloadGrid() {
	var receiverID = document.getElementById("ReceiverSearch").value;
	//var url = '@Url.Action("SearchReceiverGrid", "SendMoney")';
	reloadGridURL += '/?searchTerm=' + receiverID;
	$('#jqTable').setGridParam({ url: reloadGridURL }).trigger('reloadGrid');
}

//Selected radio button value
function selectReceiver() {
	var canRef = document.getElementById("SelectReceiver");
	canRef.addEventListener("click", SelReceiver(), false);
}

function SelReceiver() {
	var receiverID = $("input:radio[name='select']:checked").val();
	if (receiverID != undefined) {
		var url = selectReceiverURL + '?ReceiverID=' + receiverID;
		window.location.href = url; //"/SendMoney/SelectReceiver/?ReceiverID=" + receiverID;
	}
	else {
	    showExceptionPopupMsg("Select a Receiver");
	}
}

//Edit receiver
function editReceiver(id) {
	var ReceiverId = id;
	if (ReceiverId != undefined) {
		var url = getReceiverForEditURL + '?ReceiverId=' + ReceiverId;
		window.location.href = url;
	}
	else {
	    showExceptionPopupMsg("Select a Receiver for Edit");
	}
}
