
// to keep the spinner running till it prints both trasaction and summary receipts
var canHideSpinner = true;
function LoadTransactionHistoryGrid(jqDataUrl) {
	$("#layout_right_pane").css({ "display": "none" });

	var rowsToColor = [];
	var flag = 0;
	// Set up the jquery grid
	jQuery("#jqTable1").jqGrid({
		// Ajax related configurations
		url: jqDataUrl,
		datatype: "json",
		mtype: "POST",
		// Specify the column names
		colNames: GetColumnNames(transactionStatus),

		// Configure the columns
		colModel: GetColumns(transactionStatus),

		// Grid total width and height
		cmTemplate: { sortable: true },
		width: 940,
		height: 320,

		// Paging
		toppager: false,
		pager: jQuery("#jqTablePager"),
		rowNum: 8,
		viewrecords: true, // Specify if "total number of records" is displayed

		// Grid caption
		caption: "",
		shrinkToFit: false,
		loadComplete: function (data) {
			if (data.success == false) {
				showExceptionPopupMsg(data.data);
			}
			$("tr.jqgrow:even").css("background", "#ebf0ee");
		}
	}).navGrid("#jqTablePager",
            { refresh: false, search: false, add: false, edit: false, del: false },
                {}, // settings for edit
                {}, // settings for add
                {}, // settings for delete
                { sopt: ["cn"] } // Search options. Some options can be set on column level
         );
}

function GetColumnNames(TransactionStatus) {
	if (TransactionStatus == "Posted")
		return ["Posted Date/Time", "Transaction Date/Time", "Merchant Name", "Location", "Transaction Description", "Transaction Amount", "Actual Balance", "Available Balance"]
	else if (TransactionStatus == "Denied")
		return ["Transaction Date/Time", "Merchant Name", "Location", "Transaction Description", "Transaction Amount", "Decline Reason", "Actual Balance", "Available Balance"]
	else if (TransactionStatus == "Pending")
		return ["Transaction Date/Time", "Merchant Name", "Location", "Transaction Description", "Transaction Amount", "Actual Balance", "Available Balance"]
}

function GetColumns(TransactionStatus) {
	if (TransactionStatus == "Posted")
		return [
            { name: "PostedDateTime", index: "PostedDateTime", align: "left" },
            { name: "TransactionDateTime", index: "TransactionDateTime", align: "left" },
            { name: "MerchantName", index: "MerchantName", width: 180, align: "left" },
            { name: "Location", index: "Location", width: 180, align: "right" },
            { name: "TransactionDescription", index: "TransactionDescription", align: "right" },
            { name: "TransactionAmount", index: "TransactionAmount", align: "left" },
            { name: "ActualBalance", index: "ActualBalance", align: "center" },
            { name: "AvailableBalance", index: "AvailableBalance", align: "center" },
		]
	else if (TransactionStatus == "Denied")
		return [
            { name: "TransactionDateTime", index: "TransactionDateTime", align: "left" },
           { name: "MerchantName", index: "MerchantName", width: 180, align: "left" },
            { name: "Location", index: "Location", width: 180, align: "right" },
            { name: "TransactionDescription", index: "TransactionDescription", align: "right" },
            { name: "TransactionAmount", index: "TransactionAmount", align: "left" },
            { name: "DeclineReason", index: "DeclineReason", align: "right" },
            { name: "ActualBalance", index: "ActualBalance", align: "center" },
            { name: "AvailableBalance", index: "AvailableBalance", align: "center" },
		]
	else if (TransactionStatus == "Pending")
		return [
            { name: "TransactionDateTime", index: "TransactionDateTime", align: "left" },
            { name: "MerchantName", index: "MerchantName", width: 180, align: "left" },
            { name: "Location", index: "Location", width: 180, align: "right" },
            { name: "TransactionDescription", index: "TransactionDescription", align: "right" },
            { name: "TransactionAmount", index: "TransactionAmount", align: "left" },
            { name: "ActualBalance", index: "ActualBalance", align: "center" },
            { name: "AvailableBalance", index: "AvailableBalance", align: "center" },
		]
}
