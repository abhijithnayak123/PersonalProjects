var ssnRegExp = /^(?!(000|666))([0-9]\d{2}|7([0-9]\d|7[012]))-?(?!00)\d{2}-?(?!0000)\d{4}$/;
var zipCodeRegExp = /^(\d)(?!\1{4}$)\d*$/;

$(document).ready(function () {
	$("#divUserSessionId").css("display", "");
	addRules();
	$("#SSN").mask("999-99-9999");
	$('#SSN').focus();
	$("#griddiv").hide();
	$('#NoRecords').hide();
	$('#criteriaSubmit').unbind();
	disableNewCustomerButton();
	$('#criteriaSubmit').click(function (e) {
		populateGrid(e);
	});

	//Handle Enter Key Press
	$(document).keypress(function (e) {
		if (e.which == 13) {
			populateGrid(e);
		}
	});

	$("#SSN").focusout(function () {
		var ssn = $("#SSN").val();
		if (ssn != "" && ssn.length == 11) {
			$("#SSN").unmask("999-99-9999");
		}
		CommonSSNMasking(this);
	});
	$("#SSN").keydown(function (event) {
		if (event.keyCode >= 65 && event.keyCode <= 90) {
			return false;
		}
		var ssn = $("#SSN").val();
		if (event.keyCode != "8" && (ssn.length == 0 || ssn.replace("_", "").length == 11) && event.keyCode != "9" && event.keyCode != "16" && event.keyCode != "17" &&
			event.keyCode != "36" && event.keyCode != "35" && event.keyCode != "65" && !(event.keyCode >= "37" && event.keyCode <= "40") || (ssn.charAt(0) == "*" && event.keyCode == "8")) {
			$("#SSN").mask("999-99-9999");
		}
		disableNewCustomerButton();
	});
	$('#btnNewCustomer').click(function (event) {
		event.preventDefault();
		$("#SSN").rules("remove", "required");
		$("#SSN").rules("remove", "ssnnumber");
		$("form").submit();
	});
});

function populateGrid(e) {
	$("#griddiv").hide();
	$("#NoRecords").css("display", "none");

	//Populate the grid, if all the validations are successful
	if ($('#SSN').valid()) {
		$('#criteriaSubmit').unbind();
		showSpinner();
		jqgridfunction();
		e.preventDefault();
	}

}

$.validator.addMethod('regex', function (value, element, param) {
	return (this.optional(element) != false) || value.match(typeof param == 'string' ? new RegExp(param) : param);
});

jQuery.validator.addMethod("phonenumbersequence", function (value, element, param) {
	var myRegExp1 = /^[2-9]\d{9}$/;
	var myRegExp2 = /2{9}|3{9}|4{9}|5{9}|6{9}|7{9}|8{9}|9{9}/;
	var phoneNumber = null;

	phoneNumber = $('#' + param).val();
	if (phoneNumber == null || phoneNumber == '' || phoneNumber == '___-___-____')
		return true;

	if (phoneNumber != null && phoneNumber.length != 0) {
		phoneNumber = phoneNumber.replace(/-/g, "");

		var result = (!myRegExp1.test(phoneNumber) || myRegExp2.test(phoneNumber));

		return !result;
	}
	return true;
});

function addRules() {
	$("#SSN").rules("add", {
		required: true,
		ssnnumber: "ActualSSN",
		messages: {
			required: '_NexxoTranslate(SSNITINRequired)',
			ssnnumber: '_NexxoTranslate(IdentificationSSNITINRegularExpression)'
		}
	});
}

// Set up the jquery grid
function jqgridfunction() {
	var jqDataUrl = CustomerLookUpUrl;
	jQuery("#jqTable").jqGrid(
        {
        	// Ajax related configurations
        	url: jqDataUrl,
        	datatype: "json",
        	mtype: "POST",
        	postData:
                {
                	SSN: function () {
                		return $('#ActualSSN').val();
                	}, IsCompanionSearch: function () {
                		return $('#IsCompanionSearch').val();
                	}
                }, //parameters go here in object literal form 
        	//loadonce: true,
        	gridview: true,
        	autoencode: true,
        	jsonReader: { repeatitems: false },
        	highlightRowsOnHover: true,

        	// Configure the columns
        	colModel: [
                { name: "Customer Name", index: "FName", width: 220, align: "left", jsonmap: "customername", classes: "customerlookup_jqgridhyperlink" },
                { name: "Date of Birth", index: "DateOfBirth", width: 110, align: "center", jsonmap: "DateOfBirth" },
                { name: "ID Number", index: "GovernmentId", width: 110, align: "center", jsonmap: "GovernmentId" },
                { name: "Address", index: "address", width: 280, align: "left", jsonmap: "address" },
                { name: "SSN", index: "SSN", width: 280, align: "left", jsonmap: "SSN", hidden: true } //This is used for validation in Nexxo DB
        	],

        	// Grid total width and height
        	cmTemplate: { sortable: false },

        	width: 600,
        	height: 'auto',

        	// Paging
        	toppager: false,
        	pager: jQuery("#jqTablePager"),
        	rowNum: 5,
        	rowList: [5, 10, 20],
        	viewrecords: true, // Specify if "total number of records" is displayed

        	// Default sorting
        	sortname: "FName",
        	sortorder: "asc",

        	// Grid caption
        	caption: "",
        	loadComplete: function () {
        		$("tr.jqgrow:even").css("background", "rgb(235, 240, 238)");
        		$("#griddiv").show();

        		if (jQuery("#jqTable").jqGrid('getGridParam', 'records') <= 0) {
        			$("#griddiv").hide();
        			$("#NoRecords").text('No Matching Results found').css('color', 'red');
        			$("#NoRecords").css("display", "block");
        		}
        		else {
        			$("#griddiv").show();
        			$("#NoRecords").css("display", "none");
        		}

        		hideSpinner();
        		enableNewCustomerButton();
        	},
        	onSelectRow: function (id) {
        		var rowData = jQuery(this).getRowData(id);
        		validateAndPopulateCustomerDetails(id, rowData["SSN"]);
        	}
        }).navGrid("#jqTablePager",
            { add: false, edit: false, del: false, search: false, refresh: false },
            {}, // settings for edit
            {}, // settings for add
            {}, // settings for delete
            { sopt: ["cn"] } // Search options. Some options can be set on column level
        ).trigger("reloadGrid");

	$('#criteriaSubmit').bind('click', function (e) {
		populateGrid(e);
	});
}


function validateAndPopulateCustomerDetails(Id, SSN) {
	var isCompanionSearch = $('#IsCompanionSearch').val();
	$.ajax({
		url: PopulateCustomerDetailsUrl,
		data: { "id": Id, "ssn": SSN, "IsCompanionSearch": isCompanionSearch }, //parameters go here in object literal form 
		type: 'POST',
		datatype: 'json',
		success: function (data) {
			if (data == "CompanionSearch" && isCompanionSearch == "True") {
				RedirectToUrl(OrderAddOnCardUrl);
			}
			else if (data.success == undefined) {
				window.location.href = PersonalInfoNavigationURL + '?IsException=false&ExceptionMsg=""';
			}
			else {
				showExceptionPopupMsg(data.data);
			}
		},
		error: function (err) {
			showExceptionPopupMsg(err);
		}
	});
}
function disableNewCustomerButton() {
	$('#btnNewCustomer').attr('disabled', 'disabled').addClass('OpaqueViewCart');
}
function enableNewCustomerButton() {
	$('#btnNewCustomer').removeAttr('disabled').removeClass('OpaqueViewCart');
}
