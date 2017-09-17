var ssnRegExp = /^(?!(000|666))([0-9]\d{2}|7([0-9]\d|7[012]))-?(?!00)\d{2}-?(?!0000)\d{4}$/;
var zipCodeRegExp = /^(\d)(?!\1{4}$)\d*$/;
var regexAlpabetsOnly = "^[a-zA-Z\- ']*$";

$(document).ready(function () {
    $("#divUserSessionId").css("display", "");
    addRules();

    $('#LastName').focus();
    $("#griddiv").hide();
    $('#NoRecords').hide();
    $('#criteriaSubmit').unbind();

    $('#criteriaSubmit').bind('click', function () {
        populateGrid();
    });

    $("#DateOfBirth").mask("99/99/9999");
    $('#LastName').text = "hi";
    $("#PrimaryPhone").mask("999-999-9999");

    //Handle Enter Key Press
    $(document).keypress(function (e) {
        if (e.which == 13) {
            populateGrid();
        }
    });
});

function populateGrid() {
    $("#griddiv").hide();
    $("#NoRecords").css("display", "none");
    
    //Populate the grid, if all the validations are successful
    if ($('#LastName').valid() && $('#DateOfBirth').valid() && $('#PrimaryPhone').valid() && $('#ZipCode').valid()) {
        if (requiredFieldValidation()) {
            $('#criteriaSubmit').unbind();
            _spinner();
            jqgridfunction();
        }
    }
}

//*************************************** SET UP THE JQUERY GRID ***************************************//
function jqgridfunction() {
    var jqDataUrl = CustomerLookUpUrl;
    jQuery("#jqTable").jqGrid(
        {
            // Ajax related configurations
            url: jqDataUrl,
            datatype: "json",
            mtype: "POST",
            postData:
                { LastName: function () {
                    return $('#LastName').val();
                },
                    PrimaryPhone: function () {
                        return $("#PrimaryPhone").val();
                    },
                    ZipCode: function () {
                        return $("#ZipCode").val();
                    },
                    DateOfBirth: function () {
                    	return $("#DateOfBirth").val();
                    },
                    gridColFormat: "CarverFormat"
                }, //parameters go here in object literal form 
            //loadonce: true,
            gridview: true,
            autoencode: true,
            jsonReader: { repeatitems: false },

            // Configure the columns
            colModel: [
                { name: "Customer Name", index: "FName", width: 200, align: "left", jsonmap: "customername", classes: "customerlookup_jqgridhyperlink" }, 
                {name: "SSN/TIN", index: "SSN", width: 110, align: "center", jsonmap: "ssn" },
                { name: "Address", index: "Address1", width: 270, align: "left", jsonmap: "address" }
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

                $('#loading').fadeOut('slow', function () { $('#loading').remove(); });
            },
            onSelectRow: function (id) {
                var rowData = jQuery(this).getRowData(id);
                validateAndPopulateCustomerDetails(id, rowData["SSN/TIN"]);
            }
        }).navGrid("#jqTablePager",
            { add: false, edit: false, del: false, search: false, refresh: false },
            {}, // settings for edit
            {}, // settings for add
            {}, // settings for delete
            {sopt: ["cn"]} // Search options. Some options can be set on column level
        ).trigger("reloadGrid");

    $('#criteriaSubmit').bind('click', function () {
        populateGrid();
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
            if (data.success == undefined) {
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

//*************************************** VALIDATIONS ***************************************//

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

$.validator.addMethod("dateformat", function (value, element, other) {
    if ($(element)) {
        if (value == null || value == "" || value == '__/__/____') {
            return true;
        }
        var month = 0;
        var day = 0;
        var year = 0;
        month = parseInt(value.split('/')[0], 10); //month
        day = parseInt(value.split('/')[1], 10); //date
        year = parseInt(value.split('/')[2]); //year

        var valid = true;

        if ((month < 1) || (month > 12)) valid = false;
        else if ((day < 1) || (day > 31)) valid = false;
        else if (((month == 4) || (month == 6) || (month == 9) || (month == 11)) && (day > 30)) valid = false;
        else if ((month == 2) && (((year % 400) == 0) || ((year % 4) == 0)) && ((year % 100) == 0) && (day > 29)) valid = false;
        else if ((month == 2) && ((year % 4) != 0) && (day > 28)) valid = false;

        if (isNaN(year) || year.toString().length != 4)
        { valid = false; }

        return valid;
    }
    return true;
});

jQuery.validator.addMethod("isfuturedate", function (value, element, other) {
    if (value == '' || value == '__/__/____')
        return true;
    if ($(element)) {
        var valid = false;
        var myDates = new Array()
        myDates[0] = value.split('/')[0]; //month
        myDates[1] = value.split('/')[1]; //date
        myDates[2] = value.split('/')[2]; //year

        var enteredDate = new Date();

        enteredDate.setMonth(myDates[0] - 1);
        enteredDate.setDate(myDates[1]);
        enteredDate.setFullYear(myDates[2]);

        var today = new Date();

        if (enteredDate > today)
            valid = false;
        else
            valid = true;
        return valid;
    }
    return true;
});

function requiredFieldValidation() {
    //Alleast two fields should have value
    var LN = $('#LastName').val() ;
    var Dob = $('#DateOfBirth').val() ;
    var PhNo = $('#PrimaryPhone').val() ;
    var Zip = $('#ZipCode').val() ;
    if ((LN == "" && Zip == "" && PhNo == "") || (LN == "" && Zip == "" && Dob == "") || (Zip == "" && PhNo == "" && Dob == "") || (PhNo == "" && LN == "" && Dob == ""))
    {
        $('#errorLabel').addClass("field-validation-error");
        $('#errorLabel').text("Please enter atleast two criterias");
        return false;
    }
    //Alleast one of the fields should have value
    //if ($('#PrimaryPhone').val() == "" && $('#LastName').val() == "" && ($('#DateOfBirth').val() == "" || $('#DateOfBirth').val() == '__/__/____') && $('#ZipCode').val() == "") {
    //    $('#errorLabel').addClass("field-validation-error");
    //    $('#errorLabel').text("Please enter atleast one criteria");
    //    return false;
    //} 
    //If phone no. is not entered
    //if ($('#PrimaryPhone').val() == "") {
    //    //Date of Birth or Zip Code should be there along with Last Name
    //    if ($('#LastName').val() != "") {
    //        if (($('#DateofBirth').val() == "" || $('#DateofBirth').val() == '__/__/____') && ($('#ZipCode').val() == "")) {
    //            $('#errorLabel').addClass("field-validation-error");
    //            $('#errorLabel').text("Please enter Zip Code or Date of Birth");
    //            return false;
    //        }
    //    }
    //    //Last Name should be there along with Date of Birth or Zip Code
    //    if ((($('#DateofBirth').val() != "" && $('#DateOfBirth').val() != '__/__/____') || ($('#ZipCode').val() != "")) && ($('#LastName').val() == "")) {
    //        $('#errorLabel').addClass("field-validation-error");
    //        $('#errorLabel').text("Please enter Last Name");
    //        return false;
    //    }
    //}

    $('#errorLabel').removeClass("field-validation-error");
    $('#errorLabel').text("");
    return true;
}


//*************************************** ADD RULES ***************************************//
function addRules() {

	// Author : Abhijith
	// Description : As Alloy, I need a minimum age for customers to register in the system to be configurable.
	// User Story: AL-1626
	// Starts Here
	var minAge;
	var minimumAgeMessage;

	if ($("#customerMinimumAge") && $("#customerMinimumAge").val())
		minAge = $("#customerMinimumAge").val();

	if ($("#minimumAgeMessage") && $("#minimumAgeMessage").val())
		minimumAgeMessage = $("#minimumAgeMessage").val();


	$("#DateOfBirth").rules("add", {
		dateofbirthnofuturedate: "DateOfBirth",
		required: true,
		dateofbirthformat: "DateOfBirth",
		mydatecustomvalidation: { minvalue: minAge, maxvalue: 100 },
		messages: {
			dateofbirthnofuturedate: '_NexxoTranslate(IdentificationDateOfBirthDateTimeNotFuture)',
			required: '_NexxoTranslate(IdentificationDateOfBirthRequired)',
			dateofbirthformat: '_NexxoTranslate(IdentificationDateOfBirthDateTime)',
			mydatecustomvalidation: minimumAgeMessage
		}
	});
	// Ends Here

    $("#LastName").rules("add", {
        maxlength: 30,
        regex: regexAlpabetsOnly,
        messages: {
            maxlength: '_NexxoTranslate(PersonalLastNameStringLength)',
            regex: '_NexxoTranslate(PersonalLastNameRegularExpression)'
        }
    });
  
    $("#PrimaryPhone").rules("add", {
        minlength: 10,
        maxlength: 12,
        phonenumbersequence: "PrimaryPhone",
        messages: {
            minlength: '_NexxoTranslate(PersonalPrimaryPhoneStringLength)',
            maxlength: '_NexxoTranslate(PersonalPrimaryPhoneStringLength)',
            phonenumbersequence: '_NexxoTranslate(PersonalPrimaryPhoneRequired)'
        }
    });

    $("#ZipCode").rules("add", {
        maxlength: 5,
        minlength: 5,
        regex: zipCodeRegExp,
        messages: {
            maxlength: '_NexxoTranslate(PersonalZipCodeStringLength)',
            minlength: '_NexxoTranslate(PersonalZipCodeStringLength)',
            regex: '_NexxoTranslate(PersonalZipCodeRegularExpression)'
        }
    });
}

