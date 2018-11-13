var jqgridfunction = function () {
	var rowsToColor = [];
	var jqDataUrl = CustomerSearchUrl;
	var flag = 0;
	jQuery(document).ready(function () {
		if (($("#SSN").val() != "" && $("#SSN").val() != "___-__-____") || $("#MaskCardNumber").val() != "" || $("#LastName").val() != "" || $('#PhoneNumber').val() != "" || ($('#DateOfBirth').val() != "" && $('#DateOfBirth').val() != "__ / __ / ____") || $('#GovernmentId').val() != "") {
			$("#loadingmsgspinner").text("Loading Customer Data......");
			showSpinner();
			// Set up the jquery grid
			jQuery("#jqTable").jqGrid({
				// Ajax related configurations
				url: jqDataUrl,
				datatype: "json",
				mtype: "POST",
				postData: {
					isIncludeClosed: $('#IsIncludeClosed').val()
				},

				// Specify the column names
				colNames: ["Customer Name", "Phone Number", "Date of Birth", "Card Number", "ID Number", "Status"],

				// Configure the columns
				colModel: [
		{ name: "Customer Name", index: "FName", width: 300, align: "left", formatter: 'showlink', formatter: customerIDGenerator }, // Created a new method to avoid QueryString
		{ name: "Phone Number", index: "PhoneNumber", width: 170, align: "center" },
		{ name: "Date of Birth", index: "DateOfBirth", width: 150, align: "center" },
		{ name: "Card Number", index: "Cardnumber", width: 230, align: "center", formatter: cardMaskFormatter },
		{ name: "ID Number", index: "GovernmentId", width: 130, align: "center" },
		{ name: "Status", index: "Status", width: 220, align: "center" }

				],
				//colModel :[{name:'EDIT',edittype:'select',formatter:'showlink', width:5,xmlmap:"Edit",formatoptions:{baseLinkUrl:'someurl.php', addParam: '&action=edit'}},

				// Grid total width and height
				cmTemplate: { sortable: false },

				width: 815,
				height: 'auto',

				// Paging
				toppager: false,
				pager: jQuery("#jqTablePager"),
				rowNum: 5,
				rowList: [5, 10, 20],
				viewrecords: true, // Specify if "total number of records" is displayed

				// Default sorting
				multiSort: true,
				sortname: 'Status asc, FName',
				sortorder: 'asc',

				// Grid caption
				caption: "",
				loadComplete: function (data) {
					$("tr.jqgrow:even").css("background", "rgb(235, 240, 238)");
					var gridRows = parseInt($('#jqTable').jqGrid('getGridParam', 'records'));
					if (gridRows > 0) {
						$("#griddiv").show();
						$("#NoRecords").css("visibility", "hidden");
					}
					else if (data.data != "" && data.success == false) {
						showExceptionPopupMsg(data.data);
					}
					else {

						$("#griddiv").hide();
						$("#NoRecords").text('No Matching Results found').css('color', 'red');
					}
					hideSpinner();
				}
			}).navGrid("#jqTablePager",
			{ refresh: true, add: false, edit: false, del: false, search: false, refresh: false },
				{}, // settings for edit
				{}, // settings for add
				{}, // settings for delete
				{ sopt: ["cn"] } // Search options. Some options can be set on column level
			);
		}
	});

	function cardMaskFormatter(cellvalue, options, rowObject) {
		if (cellvalue.length == 4)
			cellvalue = (new Array(16 - String(cellvalue).length + 1)).join("*").concat(cellvalue);
		else
			cellvalue = cellvalue.replace(/.(?=.{4})/g, '*');
		return cellvalue;
	}

	//This new method is created to avoid QueryString # US1877 # TA4084

	function customerIDGenerator(cellvalue, options, rowObject) {
		var i = validateCustomer.lastIndexOf("ValidateCustomerStatusAndId") + 27;
		validateCustomer = validateCustomer.substring(0, i);
		cellvalue = "<a href='" + validateCustomer + "/" + options.rowId + "'>" + cellvalue + "</a>";
		return cellvalue;
	}

}

function clearCriteriaFields() {
	$('.srchCriTextBox').val("");
}

function clearCardFields() {
	$('.cardTextBox').val("");
}

function ValidateKey(e, regex) {
	var enterKey = String.fromCharCode(!e.charCode ? e.which : e.charCode);
	if (regex.test(enterKey)) {
		return true;
	}
	e.preventDefault();
	return false;
}

var regexAlphabetsOnly = /^[a-zA-Z\- ']*$/;
var regexIDNumber = /^[a-zA-Z0-9-*]*$/;

$(document).ready(function () {
    $("#divUserSessionId").css("display", "");
	$("#MaskCardNumber").keydown(function (event) {
		restrictCharacters(event);
	});
	//Criteria Search Fields on Focus
	$('.srchCriTextBox').focus(function () {
		var errorSummary = $("#valid > .validation-summary-errors");
		if (errorSummary.length == 1) {
			errorSummary.hide();
		}
	});

	//Card Search Field on Focus
	$('.cardTextBox').focus(function () {

		var errorSummary = $("#valid > .validation-summary-errors");
		if (errorSummary.length == 1) {
			errorSummary.hide();
		}

		var cardValue = $('#CardNumber').val().replace(/ /g, '');
		$('#CardNumber').val(cardValue);
	});

	//Card Number text box field tabs out or focus out event handling
	$('#CardNumber').blur(function (e) {
		formatCardNumber();
	});

	$("#criteriaSubmit").click(function (event) {
		clearCardFields();
	});

	//CardSubmit Click event Trigger
	$("#cardSubmit").click(function (event) {
		clearCriteriaFields();
	});

	$("#DateOfBirth").mask("99/99/9999");
	$("#PhoneNumber").mask("999-999-9999");
	$("#SSN").mask("999-99-9999");

	$('#LastName').keypress(function (e) {
		if (e.keyCode != 13) {
			ValidateKey(e, regexAlphabetsOnly);
		}
	});

	$('#GovernmentId').keypress(function (e) {
		if (e.keyCode != 13) {
			ValidateKey(e, regexIDNumber);
		}
	});

	$('.cardTextBox').blur(function () {
		$("#NoRecords").attr("style", "visibility:hidden");
	});

	$('.srchCriTextBox').blur(function () {
		$("#NoRecords").attr("style", "visibility:hidden");
	});

	$("#griddiv").hide();

	if (CustomerSearchCriteriaFlag == "True") {
		jqgridfunction();
	}
	// For showing IDconfirm Popup
	if (showPopup == "true") {
		showIDConfirmationMsg();
		var divError = $('#NoRecords');
		divError.hide();
	}

	// Disabling all the controls
	$('.disable_txt').attr('disabled', 'disabled').attr('cursor', 'default');
	$('.disable_txt').css('display', 'none');

	// code to display location pop up for MGI
	var searchIcon = $('#searchCustomerIcon');
	searchIcon.css('visibility', 'hidden');
	var agentTitle = $('#titleInfo');
	agentTitle.removeClass('cust_search').addClass('agent_banner_title');
	if (IsChooseLocation == 'True') {
		DisplayChooseLocation();
	}

});
