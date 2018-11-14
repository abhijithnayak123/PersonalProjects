﻿$(document).ready(function () {
    $("#SSN").rules("add", {
        ssnnumber: "ActualSSN",
        messages: {
            ssnnumber: '_NexxoTranslate(IdentificationSSNITINRegularExpression)'
        }
    });
    $("#DateOfBirth").rules("add", {
        sqlMinDateValidation: { minvalue: '01/01/1900' },
        messages: {
            sqlMinDateValidation: '_NexxoTranslate(SqlMinDateValidation)'
        }
    });

    $("#TCFCheckDateOfBirth").rules("add", {
        sqlMinDateValidation: { minvalue: '01/01/1900' },
        messages: {
            sqlMinDateValidation: '_NexxoTranslate(SqlMinDateValidation)'
        }
    });


    // ************************ Customer Card Search **********************************************************************

    $("#TCFCheckDateOfBirth").mask("99/99/9999");

    handelDisableFindButton();

    var cardNumber = $("#CardNumber").val();
    // if we get an error from controller (Ex: voltage error), then enable the submit button and show DOB field for TCF card
    if (cardNumber != '' && cardNumber != null) {
        $("#cardsearchsubmit").prop('disabled', false);

        if ($('#IsZeoCard').val() == 'false') {
            $("#dob_hide").show();
        }
    }


    // If the card number is masked, Clear the card number when teller click on back space button and hide DOB field for TCF card.
    $("#MaskCardNumber").keydown(function (event) {

        cardSwipeTrackData = cardSwipeTrackData + event.char;

        if ((cardSwipeTrackData.length == 1 && cardSwipeTrackData.substring(0, 1) != '%') || (cardSwipeTrackData.length == 2 && cardSwipeTrackData.substring(0, 2) != '%B')) {
            cardSwipeTrackData = '';
        }

        if (event.keyCode == 8 && $("#MaskCardNumber").val().indexOf('*') != -1) {
            clearCardSearchFields();
            $("#dob_hide").hide();
        }
        restrictCharacters(event);
    });

    //Clear the Card search field.
    function clearCardSearchFields() {
        $("#MaskCardNumber").val('');
        $("#CardNumber").val('');
        $("#TCFCheckDateOfBirth").val('');
        $("#cardsearchsubmit").prop('disabled', true);
    }


    // When teller click on crossmark button in Card number field, hide the DOB field for TCF card and disable find button.
    setTimeout(function () {
        $('#MaskCardNumber').siblings('.clearlink').mousedown(function () {
            $("#dob_hide").hide();
            $("#TCFCheckDateOfBirth").val('');
            $('#cardsearchsubmit').prop('disabled', true);
        });
    }, 10);

    // when teller click on crossmark button in DOB field, disable the find button
    setTimeout(function () {
        $('#TCFCheckDateOfBirth').siblings('.clearlink').mousedown(function () {
            $('#cardsearchsubmit').prop('disabled', true);
        });
    }, 10);


    // when teller enters/swipes the card number and DOB, then enable or Disable the find button based on the form validation.
    $('input').on('blur keyup', function () {

        if ($("#MaskCardNumber").val().indexOf('*') == -1) {
            $('#cardsearchsubmit').prop('disabled', true);
        }
        else if ($("#MaskCardNumber").val().indexOf('*') != -1 && $('#IsZeoCard').val() == 'false' && ($("#TCFCheckDateOfBirth").val() == '__/__/____' || $("#TCFCheckDateOfBirth").val() == '')) {
            $('#cardsearchsubmit').prop('disabled', true);
        }
        else {
            if ($("#CardSearchForm").valid()) {
                $('#cardsearchsubmit').prop('disabled', false);
            } else {
                $('#cardsearchsubmit').prop('disabled', true);
            }
        }
    });

    //********************************************** End ***************************************************************


    //***************************************** Customer Search Criteria ************************************************

    var regexAlphabetsOnly = /^[a-zA-Z\- ']*$/;

    $("#SSN").mask("999-99-9999");

    $("#DateOfBirth").mask("99/99/9999");

    $('#LastName').keypress(function (e) {
        var startPos = e.currentTarget.selectionStart;
        if (e.which === 32 && startPos == 0) {
            e.preventDefault();
        }
        if (e.keyCode === 42 && $('#LastName').val().length >= 3 && startPos >= 3 && $('#LastName').val().indexOf('*') == -1) {
            return true;
        }

        if (e.keyCode != 13) {
            validateKey(e, regexAlphabetsOnly);
        }
    });


    $("#AccountNumber").keydown(function (event) {
        restrictCharacters(event);
    });

    if (CustomerSearchCriteriaFlag == "True") {
        searchCustomerGrid();
    }

    $("#customersearchgridrow").live('click', function (e) {
        var dataHref = this.getAttribute('data-href');
        ShowPopUpMinHeight(dataHref, 'Customer Details', 380, 300);
    });

    $('.input_box.search_criteria').on('blur keyup', function (event) {
        handelDisableFindButton(event);
    });

    setTimeout(function () {
        $('.input_box.search_criteria').siblings('.clearlink').mousedown(function () {
            handelDisableFindButton();
        });
    }, 10);

});

function validateKey(e, regex) {
    var enterKey = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (regex.test(enterKey)) {
        return true;
    }
    e.preventDefault();
    return false;
}

function searchCustomerGrid() {
    jQuery(document).ready(function () {
        if (($("#SSN").val() != "" && $("#SSN").val() != "___-__-____") || $("#MaskCardNumber").val() != "" || $("#LastName").val() != "" || $('#AccountNumber').val() != "" || ($('#DateOfBirth').val() != "" && $('#DateOfBirth').val() != "__ / __ / ____")) {
            $("#loadingmsgspinner").text("Loading Customer Data......");
            showSpinner();

            jQuery("#jqTable").jqGrid({
                // Ajax related configurations
                url: CustomerSearchUrl,
                datatype: "json",
                mtype: "POST",
                // Specify the column names
                colNames: ["Customer Name", "Date of Birth", "ZEO Card Number", "Address"],

                // Configure the columns
                colModel: [
                    { name: "Customer Name", index: "FName", width: 280, align: "left", formatter: 'showlink', formatter: customerIDGenerator }, // Created a new method to avoid QueryString
                    { name: "Date of Birth", index: "DateOfBirth", width: 130, align: "center" },
                    { name: "ZEO Card Number", index: "Cardnumber", width: 230, align: "center", formatter: cardMaskFormatter },
                    { name: "Address", index: "Address", width: 300, align: "center" },
                ],
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
                        $("#customer_search_grid_div").show();
                        $("#NoRecords").css("visibility", "hidden");
                    }
                    else if (data.data != "" && data.success == false) {
                        var exception = data.data;
                        if (exception.indexOf("1001.100.6018") != -1) {
                            ShowPopUpdataMinHeight(noCustomerPopupUrl, "SYSTEM MESSAGE", 505, 240, data.data);
                        }
                        else {
                            showExceptionPopupMsg(exception);
                        }
                    }
                    else {

                        $("#customer_search_grid_div").hide();
                        $("#NoRecords").text('No Matching Results found').css('color', 'red');
                    }
                    hideSpinner();
                }
            }).navGrid("#jqTablePager",
                { add: false, edit: false, del: false, search: false, refresh: false },
                {}, // settings for edit
                {}, // settings for add
                {}, // settings for delete
                { sopt: ["cn"] } // Search options. Some options can be set on column level
                );
        }
    })

    function cardMaskFormatter(cellvalue, options, rowObject) {
        if (cellvalue.length == 4)
            cellvalue = (new Array(16 - String(cellvalue).length + 1)).join("*").concat(cellvalue);
        else
            cellvalue = cellvalue.replace(/.(?=.{4})/g, '*');
        return cellvalue;
    }

    //This new method is created to avoid QueryString # US1877 # TA4084

    function customerIDGenerator(cellvalue, options, rowObject) {
        var i = customerConformation.lastIndexOf("CustomerConformation") + 27;
        customerConformation = customerConformation.substring(0, i);
        cellvalue = "<a id='customersearchgridrow' href='#' data-href='" + customerConformation + "/" + options.rowId + "'>" + cellvalue + "</a>";
        return cellvalue;
    }
}

function getCardTypeByBIN(cardNumber) {
    $.ajax({
        url: CardBINs_URL,
        data: { cardNumber: cardNumber },
        type: 'POST',
        datatype: 'json',
        success: function (jsonData) {
            $('#IsZeoCard').val(jsonData.IsZeoCard);

            if (!jsonData.IsZeoCard) {
                $("#dob_hide").show();
                $("#TCFCheckDateOfBirth").focus();
                $("#cardsearchsubmit").prop('disabled', true);
            }
            else {
                $("#dob_hide").hide();
                $("#cardsearchsubmit").prop('disabled', false).focus();
            }
        },
        error: function (err) {
            showExceptionPopupMsg(err.data);
        }
    });
}



function validSSN(value) {
    var regexSSNValidation = /^[?!(000|666))([0-9]\d{2}|7([0-9]\d|7[012])-?(?!00)\d{2}-\d{4}}$/;
    if (!regexSSNValidation.test(value)) {
        return false;
    }
    return true;
}//end validSSN function

function handelDisableFindButton(event) {

    var dob = $("#DateOfBirth").val();
    var ssn = $("#SSN").val();

    if (dob.replace(/[/_-]/g, '').length === 0) {
        allowOnlyTwoFiled(event);
        $('#criseachsubmit').prop('disabled', true);
    }
    else if (dob.replace(/[/_-]/g, '').length !== 0 && (
        (ssn.replace(/[_-]/g, '').length !== 0 && ssn.indexOf('_') === -1 && $("#LastName").val() === "" && $('#AccountNumber').val() === "") ||
        ($("#LastName").val() !== "" && ssn.replace(/[_-]/g, '').length === 0 && $('#AccountNumber').val() === "") ||
        ($('#AccountNumber').val() !== "" && ssn.replace(/[_-]/g, '').length === 0 && $("#LastName").val() === ""))) {
        if ($("#CriSearchForm").valid()) {
            $("#NoRecords").text('');
            $("#NoRecords").css("visibility", "hidden");
            $('#criseachsubmit').prop('disabled', false);
        }
        else {
            $('#criseachsubmit').prop('disabled', true);
        }
    }
    else {
        allowOnlyTwoFiled(event);
        $('#criseachsubmit').prop('disabled', true);
    }
}

function allowOnlyTwoFiled(event) {
    var count = 0;

    $(".input_box.search_criteria").each(function () {
        var element = $(this);
        if (element.val() != "") {
            count = count + 1;
        }
    });

    if (count > 2) {
        getMessage(CustomerExceptions.Customer_Search_MoreThen_TwoOption_Error);
        $("#" + event.target.id).val('');
    }

}
