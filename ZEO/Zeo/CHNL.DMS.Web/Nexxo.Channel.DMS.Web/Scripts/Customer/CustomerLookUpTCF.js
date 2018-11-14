var ssnRegExp = /^(?!(000|666))([0-9]\d{2}|7([0-9]\d|7[012]))-?(?!00)\d{2}-?(?!0000)\d{4}$/;
var zipCodeRegExp = /^(\d)(?!\1{4}$)\d*$/;

var retrieveValue = function (element) {
    var $this = $(element);
    var value = $('#ActualSSN').val();
    $this.val(value);
};

var hideCardNumber = function (element) {
    var $this = $(element);
    var value = $this.val();
    if (value.length != 0 && value.length >= 16) {
        $('#ActualCardNumber').val(value);
        var val = value.replace(/-/g, "");
        if (value.length == 16) {
            var maskedValue = "**** **** **** ";
            var CardNumber = maskedValue + val.substring(12, val.length);
            $this.val(CardNumber);
            ValidateCustomerCard(value);
        }
        else if (value.length == 17) {// AL-228 changes
            var maskedValue = "**** **** **** *";
            var CardNumber = maskedValue + val.substring(13, val.length);
            $this.val(CardNumber);
            ValidateCustomerCard(value);

        }
        else if (value.length == 18) {// AL-228 changes
            var maskedValue = "**** **** **** **";
            var CardNumber = maskedValue + val.substring(14, val.length);
            $this.val(CardNumber);
            ValidateCustomerCard(value);
        }
    }
    else if (value.length != 0 && value.length < 16) {
        var element = $('span[data-valmsg-for="MaskCardNumber"]')
        .text('_NexxoTranslate(TCFCustomerSearchCardNumberRequired)')
        .show();
    }
    else if (value.length == 0) {
        var element = $('span[data-valmsg-for="MaskCardNumber"]')
        .text('')
        .hide();
    }
};

$(document).ready(function () {
    $("#divUserSessionId").css("display", "");
    addRules();
    $("#SSN").mask("999-99-9999");
    $("#DateOfBirth").mask("99/99/9999");
    $('#SSN').focus();
    $("#griddiv").hide();
    $('#criteriaSubmit').unbind();
    disableNewCustomerButton();
    $('#criteriaSubmit').click(function (e) {
        ValidateSearhcriteria(e);
    });
    $("#DateOfBirth").blur(function () {
        var dateofbirth = $('#DateOfBirth').val();
        if (dateofbirth.length == 0) {
            CheckDOB();
        }
        else {
            var element = $(".valid_error").find(".field-validation-valid").text('');
        }
    });
    //Handle Enter Key Press
    $(document).keypress(function (e) {
        if (e.which == 13) {
            if ($("#DateOfBirth").val() != '') {
                $('#criteriaSubmit').trigger('click');
            }
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

    $("#MaskCardNumber").blur(function () {
        var cardnumber = $("#MaskCardNumber").val();
        if (cardnumber != null) {
            if (cardnumber.trim() == "" || cardnumber.trim().length > 18)
                $("#validationMessageforAccount").text('');

        }
        hideCardNumber(this);
    });

    $("#Account").blur(function () {
        var account = $("#Account").val();
        if (account == "") {
            $('input[name=AccountType]').attr('checked', false);
            $("#validationMessageforAccount").text('');
        }
    });

    $('.align_acc_right').find('input:radio').each(function () {
        if ($(this).checked) {
            $("#validationMessageforAccount").text('');
        }
    });

    $('#btnNewCustomer').click(function (event) {
        //event.preventDefault();
        $("#SSN").rules("remove", "ssnnumber");
        $("form").submit();
    });

    $("#MaskCardNumber").keydown(function (event) {
        restrictCharacters(event);
    });
    $("#Account").keydown(function (event) {
        restrictCharacters(event);
    });
});

function ValidateCustomerCard(ccField) {
    if (ccField == undefined || ccField.length <= 0) {
        return true;
    }
    var cardNumber = ccField;
    var cvv = '000';
    var result = ProtectPANandCVV(cardNumber, cvv);
    if (result != null) {
        // we only need the AlloyID for this demo
        protected_pan = result[0];
        protected_cvv = result[1];
        $("#CardNumber").val(protected_pan);
        $("#CVV").val(protected_cvv);
    }
    else {
        $("#MaskCardNumber").val("");
        var element = $('span[data-valmsg-for="MaskCardNumber"]')
        .text('_NexxoTranslate(TCFCustomerSearchCardNumberRequired)')
        .show();
    }
}

function CheckDOB() {
    var element = $(".valid_error").find(".field-validation-valid")
    .text('_NexxoTranslate(CustomerSearchDOBRequired)')
    .css({ "color": "red", "margin-top": "15px" })
    .show();
}


function ValidateSearhcriteria(e) {
    $("#validationMessage").text('');
    $("#validationMessageforAccount").text('');
    var validationtext = $(".field-validation-error").text();
    var validate = $(".valid_error").find(".field-validation-valid").text();
    var accountValidation = $('span[data-valmsg-for="MaskCardNumber"]').text();

    var dateofbirth = $('#DateOfBirth').val();
    if (dateofbirth.length == 0 || dateofbirth == "__/__/____") {
        CheckDOB();
        return false;
    }

    if (validationtext != "" || validate != "" || accountValidation != "")
        return;

    if ($("#SSN").val() != "" && $("#SSN").val() != "___-__-____" || $("#Account").val() != "" || $("#MaskCardNumber").val() != "") {
        $("#validationMessage").text('');
        populateGrid(e);
    }
    else {
        $("#validationMessage").text('_NexxoTranslate(CustomerSearchAtleastOneAttribute)');
        return false;
    }
}
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
        mydatecustomvalidation: { minvalue: minAge, maxvalue: 999 },
        messages: {
            dateofbirthnofuturedate: '_NexxoTranslate(IdentificationDateOfBirthDateTimeNotFuture)',
            required: '_NexxoTranslate(IdentificationDateOfBirthRequired)',
            dateofbirthformat: '_NexxoTranslate(IdentificationDateOfBirthDateTime)',
            mydatecustomvalidation: minimumAgeMessage
        }
    });
    // Ends Here

    $("#SSN").rules("add", {
        required: false,
        ssnnumber: "ActualSSN",
        messages: {
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

                    },
                    AccountNumber: function () {
                        return $('#Account').val();

                    },
                    CardNumber: function () {
                        if ($('#MaskCardNumber').val() != "")
                            return $('#CardNumber').val();
                        else return $('#MaskCardNumber').val();
                    },
                    DateOfBirth: function () {
                        return $('#DateOfBirth').val();

                    },
                    CVV: function () {
                        return $('#CVV').val();
                    }
                }, //parameters go here in object literal form 
            loadonce: true,
            gridview: true,
            autoencode: true,
            jsonReader: { repeatitems: false },
            highlightRowsOnHover: true,

            // Configure the columns
            colModel: [
                { name: "Customer Name", index: "FName", width: 220, align: "left", jsonmap: "customername", classes: "customerlookup_jqgridhyperlink" },
                { name: "Date of Birth", index: "DateofBirth", width: 110, align: "center", jsonmap: "DateOfBirth" },
                { name: "ID Number", index: "GovernmentId", width: 110, align: "center", jsonmap: "GovernmentId" },
                { name: "Address", index: "Address1", width: 280, align: "left", jsonmap: "address" },
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
            loadComplete: function (data) {
                $("tr.jqgrow:even").css("background", "rgb(235, 240, 238)");
                //AL-4348 starts
                if (data != null && data.data != null) {
                    showExceptionPopupMsg(data.data);
                    hideSpinner();
                    enableNewCustomerButton();
                    return;
                }
                //AL-4348 end

                $("#griddiv").show();

                if (jQuery("#jqTable").jqGrid('getGridParam', 'records') <= 0) {
                    $("#griddiv").hide();
                    $("#NoRecords").text('No Matching Results found').css('color', 'red');
                    enableNewCustomerButton();
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
        ).setGridParam({ datatype: 'json', page: 1 }).trigger("reloadGrid");

    $('#criteriaSubmit').bind('click', function (e) {
        ValidateSearhcriteria(e);
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
                if (data == "True") {
                    window.location.href = ProfileSummaryNavigationURL + '?IsException=false&ExceptionMessage=""';
                }
                else {
                    window.location.href = PersonalInfoNavigationURL + '?IsException=false&ExceptionMessage=""';
                }
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
