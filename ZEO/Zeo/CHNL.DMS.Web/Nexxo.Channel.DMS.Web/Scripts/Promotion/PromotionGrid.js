function provisionGrid() {
    jQuery(document).ready(function () {
        showSpinner();

        jQuery("#jqProvision").jqGrid({
            // Ajax related configurations
            url: getProvisions_Url,
            datatype: "json",
            mtype: "POST",
            // Specify the column names
            colNames: ["Promo Name", "Location", "Check Type", "Group(s)", "Min.Amount", "Max.Amount", "Value", "Discount Type", ""],

            // Configure the columns
            colModel: [
                { name: "Promo Name", index: "PromotionName", width: 120, align: "left", formatter: 'showlink', formatter: provisionIdGenerator }, // Created a new method to avoid QueryString
                { name: "Location", index: "Location", width: 160, align: "left" },
                { name: "Check Type", index: "CheckType", width: 160, align: "center" },
                { name: "Groups", index: "Groups", width: 160, align: "center" },
                { name: "Min.Amount", index: "MinAmount", width: 100, align: "center" },
                { name: "Max.Amount", index: "MaxAmount", width: 100, align: "center" },
                { name: "Value", index: "Value", width: 100, align: "center" },
                { name: "Discount Type", index: "DiscountType", width: 100, align: "center" },
                { name: "", index:"",width: 60,align:"left", formatter: deleteLink },
            ],
            // Grid total width and height
            cmTemplate: { sortable: false },

            width: '805px !important',
            height: 'auto',

            // Paging
            toppager: false,
            pager: jQuery("#jqTableProvisionPager"),
            rowNum: 5,
            rowList: [5, 10, 20],
            viewrecords: true, // Specify if "total number of records" is displayed

            // Default sorting
            multiSort: true,
            sortname: 'Status asc, PromotionName',
            sortorder: 'asc',

            // Grid caption
            caption: "",
            loadComplete: function (data) {
                $("#jqProvision tr.jqgrow:even").css("background", "rgb(235, 240, 238)");
                hideSpinner();
            }
        }).navGrid("#jqTableProvisionPager",
            { add: false, edit: false, del: false, search: false, refresh: false },
            {}, // settings for edit
            {}, // settings for add
            {}, // settings for delete
            { sopt: ["cn"] } // Search options. Some options can be set on column level
            );
    })

    //This new method is created to avoid QueryString # US1877 # TA4084

    function provisionIdGenerator(cellvalue, options, rowObject) {
        var i = EditProvisions_Url.lastIndexOf("AddProvision") + 27;
        EditProvisions_Url = EditProvisions_Url.substring(0, i);
        cellvalue = "<a id='promoriongridrow' href='#' data-href='" + EditProvisions_Url + "/?id=" + options.rowId + "'>" + cellvalue + "</a>";
        return cellvalue;
    }

    function deleteLink(cellvalue, options, rowdata, action) {
        var i = ProvisionDeleteConfirm_Url.lastIndexOf("ShowPopWarringProvision") + 27;
        ProvisionDeleteConfirm_Url = ProvisionDeleteConfirm_Url.substring(0, i);
        cellvalue = "<a id='provisiondelete' href='#' data-href='" + ProvisionDeleteConfirm_Url + "/?provisionId=" + options.rowId + "' class='ui-icon ui-icon-closethick'>" + cellvalue + "</a>";
        return cellvalue;
    }

}

function qualifierGrid() {
    jQuery(document).ready(function () {
        showSpinner();

        jQuery("#jqQualifier").jqGrid({
            // Ajax related configurations
            url: getQualifiers,
            datatype: "json",
            mtype: "POST",
            // Specify the column names
            colNames: ["Promo Name", "Qualifier Product", "Txn Status", "Txn End Date", "Amount", "Min.Txn Count", "Paid Fee", ""],

            // Configure the columns
            colModel: [
                { name: "Promo Name", index: "PromotionName", width: 120, align: "left", formatter: 'showlink', formatter: qualifierIdGenerator }, // Created a new method to avoid QueryString
                { name: "Qualifier Product", index: "QualifierProduct", width: 120, align: "left" },
                { name: "Txn States", index: "TransactionStates", width: 130, align: "left" },
                { name: "Txn End Date", index: "TransactionEndDate", width: 100, align: "center" },
                { name: "Amount", index: "Amount", width: 90, align: "center" },
                { name: "Min.Txn Count", index: "MinTrxCount", width: 90, align: "center" },
                { name: "Paid Fee", index: "MaxTrxCout", width: 90, align: "center" },
                { name: "", index: "", width: 40, align: "left", formatter: deleteLink },
            ],
            // Grid total width and height
            cmTemplate: { sortable: false },

            width: 'auto',
            height: 'auto',

            // Paging
            toppager: false,
            pager: jQuery("#jqTablePagerQualifier"),
            rowNum: 5,
            rowList: [5, 10, 20],
            viewrecords: true, // Specify if "total number of records" is displayed

            // Default sorting
            multiSort: true,
            sortname: 'Status asc, PromotionName',
            sortorder: 'asc',

            // Grid caption
            caption: "",
            loadComplete: function (data) {
                $("#jqQualifier tr.jqgrow:even").css("background", "rgb(235, 240, 238)");
                hideSpinner();
            }
        }).navGrid("#jqTablePagerQualifier",
            { add: false, edit: false, del: false, search: false, refresh: false },
            {}, // settings for edit
            {}, // settings for add
            {}, // settings for delete
            { sopt: ["cn"] } // Search options. Some options can be set on column level
            );
    })

    function qualifierIdGenerator(cellvalue, options, rowObject) {
        var i = EditQualifiers_Url.lastIndexOf("AddQualifier") + 27;
        EditQualifiers_Url = EditQualifiers_Url.substring(0, i);
        cellvalue = "<a id='promoriongridrow' href='#' data-href='" + EditQualifiers_Url + "/?id=" + options.rowId + "'>" + cellvalue + "</a>";
        return cellvalue;
    }

    function deleteLink(cellvalue, options, rowdata, action) {
        var i = QualifierDeleteConfirm_Url.lastIndexOf("ShowPopWarringQualifier") + 27;
        QualifierDeleteConfirm_Url = QualifierDeleteConfirm_Url.substring(0, i);
        cellvalue = "<a id='qualifierdelete' href='#' data-href='" + QualifierDeleteConfirm_Url + "/?qualifierId=" + options.rowId + "' class='ui-icon ui-icon-closethick'>" + cellvalue + "</a>";
        return cellvalue;
    }
}

function getPromotions() {
    jQuery(document).ready(function () {
        showSpinner();

        jQuery("#jqPromo").jqGrid({
            // Ajax related configurations
            url: getPromotions_Url,
            datatype: "json",
            mtype: "POST",
            // Specify the column names
            colNames: ["Promo Name", "Promo Description", "Product", "Provider", "Priority", "Start Date", "End Date", "Status"],

            // Configure the columns
            colModel: [
                { name: "Promo Name", index: "PromotionName", width: 120, align: "left", formatter: 'showlink', formatter: showPopDetails }, // Created a new method to avoid QueryString
                { name: "Promo Description", index: "PromotionDescription", width: 210, align: "left" },
                { name: "Product", index: "Product", width: 80, align: "center" },
                { name: "Provider", index: "Provider", width: 75, align: "center" },
                { name: "Priority", index: "Priority", width: 70, align: "center", formatter: CheckNull },
                { name: "Promo Start Date", index: "PromotionStartDate", width: 100, align: "center" },
                { name: "Promo End Date", index: "PromotionEndDate", width: 100, align: "center" },
                { name: 'Status', search: false, index: 'PromotionStatus', width: 100, sortable: false, formatter: editLink, align: "center" },
            ],
            // Grid total width and height
            cmTemplate: { sortable: false },

            width: 905,
            height: 'auto',

            // Paging
            toppager: false,
            pager: jQuery("#jqTablePromoPager"),
            rowNum: 5,
            rowList: [5, 10, 20],
            viewrecords: true, // Specify if "total number of records" is displayed

            // Default sorting
            multiSort: true,
            sortname: 'Status asc, PromotionName',
            sortorder: 'asc',

            // Grid caption
            caption: "",
            loadComplete: function (data) {
                $("tr.jqgrow:even").css("background", "rgb(235, 240, 238)");
                hideSpinner();
            }
        }).navGrid("#jqTablePromoPager",
            { add: false, edit: false, del: false, search: false, refresh: false },
            {}, // settings for edit
            {}, // settings for add
            {}, // settings for delete
            { sopt: ["cn"] } // Search options. Some options can be set on column level
            );
    })

    function showPopDetails(cellvalue, options, rowdata) {
        var i = ShowPopPromotion_Url.lastIndexOf("ShowPromotionDetails") + 27;
        ShowPopPromotion_Url = ShowPopPromotion_Url.substring(0, i);
        cellvalue = "<a id='showpopppromotion' href='#' data-href='" + ShowPopPromotion_Url + "/?id=" + options.rowId + "'>" + cellvalue + "</a>";
        return cellvalue;
    }

    function editLink(cellvalue, options, rowdata, action) {
        var i = EditPromotion_Url.lastIndexOf("AddPromotion") + 27;
        EditPromotion_Url = EditPromotion_Url.substring(0, i);
        if (rowdata[8] === 4 || rowdata[8] === 5) {
            cellvalue = cellvalue + "&nbsp;<a id='promoriongridrow' href='#' data-href='" + EditPromotion_Url + "/?id=" + options.rowId + "' class='ui-icon ui-icon-pencil display_inblock'>&nbsp</a>";
        }
        if (navigator.userAgent.indexOf("Edge") != -1) {
            slider = "sliderEdge round";
        }
        else {
            slider = "slider round";
        }
        if (rowdata[8] === 1) {
            cellvalue = "<label class='switch'><input type='checkbox' checked onclick='confirmationPopUp(" + options.rowId + ", 7)'><span class='" + slider + "'></span></label>";
        }
        if (rowdata[8] === 7) {
            cellvalue = "<label class='switch'><input type='checkbox' onclick='confirmationPopUp(" + options.rowId + ", 1)'><span class='" + slider + "'></span></label>";
        }
        return cellvalue;
    }

    function CheckNull(cellvalue, options, rowdata, action) {
        if (cellvalue == null)
            cellvalue = "NA";

        return cellvalue;
    }
}

function qulifierForSummary() {
    jQuery(document).ready(function () {
        showSpinner();

        jQuery("#jqQualifier").jqGrid({
            // Ajax related configurations
            url: getQualifiers,
            datatype: "json",
            mtype: "POST",
            // Specify the column names
            colNames: ["Promo Name", "Qualifier Product", "Txn Status", "Txn End Date", "Amount", "Min.Txn Count", "Paid Fee"],

            // Configure the columns
            colModel: [
                { name: "Promo Name", index: "PromotionName", width: 130, align: "left" }, 
                { name: "Qualifier Product", index: "QualifierProduct", width: 100, align: "left" },
                { name: "Txn States", index: "TransactionStates", width: 165, align: "left" },
                { name: "Txn End Date", index: "TransactionEndDate", width: 100, align: "center" },
                { name: "Amount", index: "Amount", width: 100, align: "center" },
                { name: "Min.Txn Count", index: "MinTrxCount", width: 100, align: "center" },
                { name: "Paid Fee", index: "MaxTrxCout", width: 100, align: "center" },
            ],
            // Grid total width and height
            cmTemplate: { sortable: false },

            width: 'auto',
            height: 'auto',

            // Paging
            toppager: false,
            pager: jQuery("#jqTablePagerSummaryQualifier"),
            rowNum: 5,
            rowList: [5, 10, 20],
            viewrecords: true, // Specify if "total number of records" is displayed

            // Default sorting
            multiSort: true,
            sortname: 'Status asc, PromotionName',
            sortorder: 'asc',

            // Grid caption
            caption: "",
            loadComplete: function (data) {
                $("#jqQualifier tr.jqgrow:even").css("background", "rgb(235, 240, 238)");
                hideSpinner();
            }
        }).navGrid("#jqTablePagerQualifier",
            { add: false, edit: false, del: false, search: false, refresh: false },
            {}, // settings for edit
            {}, // settings for add
            {}, // settings for delete
            { sopt: ["cn"] } // Search options. Some options can be set on column level
            );
    })
}

function provisionsForSummary() {
    jQuery(document).ready(function () {
        showSpinner();

        jQuery("#jqProvision").jqGrid({
            // Ajax related configurations
            url: getProvisions_Url,
            datatype: "json",
            mtype: "POST",
            // Specify the column names
            colNames: ["Promo Name", "Location", "Check Type", "Group(s)", "Min.Amount", "Max.Amount", "Value", "Discount Type"],

            // Configure the columns
            colModel: [
                { name: "Promo Name", index: "PromotionName", width: 160, align: "left" },
                { name: "Location", index: "Location", width: 180, align: "left" },
                { name: "Check Type", index: "CheckType", width: 180, align: "center" },
                { name: "Groups", index: "Groups", width: 180, align: "center" },
                { name: "Min.Amount", index: "MinAmount", width: 90, align: "center" },
                { name: "Max.Amount", index: "MaxAmount", width: 90, align: "center" },
                { name: "Discount Value", index: "DiscountValue", width: 90, align: "center" },
                { name: "Discount Type", index: "DiscountType", width: 100, align: "center" },
            ],
            // Grid total width and height
            cmTemplate: { sortable: false },

            width: 'auto',
            height: 'auto',

            // Paging
            toppager: false,
            pager: jQuery("#jqTableProSummaryPager"),
            rowNum: 5,
            rowList: [5, 10, 20],
            viewrecords: true, // Specify if "total number of records" is displayed

            // Default sorting
            multiSort: true,
            sortname: 'Status asc, PromotionName',
            sortorder: 'asc',

            // Grid caption
            caption: "",
            loadComplete: function (data) {
                $("#jqProvision tr.jqgrow:even").css("background", "rgb(235, 240, 238)");
                hideSpinner();
            }
        }).navGrid("#jqTableProSummaryPager",
            { add: false, edit: false, del: false, search: false, refresh: false },
            {}, // settings for edit
            {}, // settings for add
            {}, // settings for delete
            { sopt: ["cn"] } // Search options. Some options can be set on column level
            );
    })
}

function qulifierForSummaryPopUp() {
    jQuery(document).ready(function () {
        showSpinner();

        jQuery("#jqQualifier").jqGrid({
            // Ajax related configurations
            url: getQualifiers,
            datatype: "json",
            mtype: "POST",
            // Specify the column names
            colNames: ["Promo Name", "Qualifier Product", "Txn Status", "Txn End Date", "Amount", "Min.Txn Count", "Paid Fee"],

            // Configure the columns
            colModel: [
                { name: "Promo Name", index: "PromotionName", width: 130, align: "left" },
                { name: "Qualifier Product", index: "QualifierProduct", width: 100, align: "left" },
                { name: "Txn States", index: "TransactionStates", width: 140, align: "left" },
                { name: "Txn End Date", index: "TransactionEndDate", width: 100, align: "center" },
                { name: "Amount", index: "Amount", width: 100, align: "center" },
                { name: "Min.Txn Count", index: "MinTrxCount", width: 100, align: "center" },
                { name: "Paid Fee", index: "MaxTrxCout", width: 80, align: "center" },
            ],
            // Grid total width and height
            cmTemplate: { sortable: false },

            width: 890,
            height: 'auto',

            // Paging
            toppager: false,
            pager: jQuery("#PagerQualifierPopUp"),
            rowNum: 5,
            rowList: [5, 10, 20],
            viewrecords: true, // Specify if "total number of records" is displayed

            // Default sorting
            multiSort: true,
            sortname: 'Status asc, PromotionName',
            sortorder: 'asc',

            // Grid caption
            caption: "",
            loadComplete: function (data) {
                $("#jqQualifier tr.jqgrow:even").css("background", "rgb(235, 240, 238)");
                hideSpinner();
            }
        }).navGrid("#PagerQualifierPopUp",
            { add: false, edit: false, del: false, search: false, refresh: false },
            {}, // settings for edit
            {}, // settings for add
            {}, // settings for delete
            { sopt: ["cn"] } // Search options. Some options can be set on column level
            );
    })
}

function provisionsForSummaryPopUp() {
    jQuery(document).ready(function () {
        showSpinner();

        jQuery("#jqProvision").jqGrid({
            // Ajax related configurations
            url: getProvisions_Url,
            datatype: "json",
            mtype: "POST",
            // Specify the column names
            colNames: ["Promo Name", "Location", "Check Type", "Group(s)", "Min.Amount", "Max.Amount", "Value", "Discount Type"],

            // Configure the columns
            colModel: [
                { name: "Promo Name", index: "PromotionName", width: 160, align: "left" },
                { name: "Location", index: "Location", width: 180, align: "left" },
                { name: "Check Type", index: "CheckType", width: 180, align: "center" },
                { name: "Groups", index: "Groups", width: 180, align: "center" },
                { name: "Min.Amount", index: "MinAmount", width: 90, align: "center" },
                { name: "Max.Amount", index: "MaxAmount", width: 90, align: "center" },
                { name: "Value", index: "Value", width: 90, align: "center" },
                { name: "Discount Type", index: "DiscountType", width: 100, align: "center" },
            ],
            // Grid total width and height
            cmTemplate: { sortable: false },

            width: 890,
            height: 'auto',

            // Paging
            toppager: false,
            pager: jQuery("#PagerProvisionPopUp"),
            rowNum: 5,
            rowList: [5, 10, 20],
            viewrecords: true, // Specify if "total number of records" is displayed

            // Default sorting
            multiSort: true,
            sortname: 'Status asc, PromotionName',
            sortorder: 'asc',

            // Grid caption
            caption: "",
            loadComplete: function (data) {
                $("#jqProvision tr.jqgrow:even").css("background", "rgb(235, 240, 238)");
                hideSpinner();
            }
        }).navGrid("#PagerProvisionPopUp",
            { add: false, edit: false, del: false, search: false, refresh: false },
            {}, // settings for edit
            {}, // settings for add
            {}, // settings for delete
            { sopt: ["cn"] } // Search options. Some options can be set on column level
            );
    })
}

var regexNumbersOnly = /^([0-9.])$/;

var regexPromo = /^[a-zA-Z0-9]$/;

function ValidateText(e) {
    var enterKey = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (regexNumbersOnly.test(enterKey)) {
        return true;
    }
    e.preventDefault();
    return false;
}

function ValidatePromoName(e) {
    var enterKey = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (regexPromo.test(enterKey)) {
        return true;
    }
    e.preventDefault();
    return false;
}

function updateStatus(id) {
    RemovePopUp();
    showSpinner();
    $.ajax({
        type: "GET",
        url: DeletePromotion_Url + '?promotionId=' + id,
        dataType: "json",
        contentType: "application/json; charset=UTF-8",
        data: {},
        success: function (data) {
            if (data.success === false) {
                getMessage(PromotionException.InValid_Promotion_Status_Change);
            }
            RemoveDynIdPUP('idCopyPromo');
            $("#jqPromo").trigger("reloadGrid");
            hideSpinner();
        },
        error: function () {
            showExceptionPopupMsg(defaultErrorMessage);
            hideSpinner();
        }
    });
}

function disabledStatus(id, status) {
    RemovePopUp();
    showSpinner();
    $.ajax({
        type: "GET",
        url: Update_Promotion_Status_Url + '?promotionId=' + id + '&status=' + status,
        dataType: "json",
        contentType: "application/json; charset=UTF-8",
        data: {},
        success: function (data) {
            $("#jqPromo").trigger("reloadGrid");
            hideSpinner();
        },
        error: function () {
            showExceptionPopupMsg(defaultErrorMessage);
            hideSpinner();
        }
    });
}

function clearSession(id) {
    RemoveDynIdPUP(id);
    $.ajax({
        type: "GET",
        url: clearSession_Url,
        dataType: "json",
        contentType: "application/json; charset=UTF-8",
        data: {},
        success: function (data) {
        },
        error: function () {
        }
    });
}

function updateStatusConfirmation(id) {
    var url = ShowConfirmationPopUp_Url + '?promotionId=' + id + '&isActive=' + status;
    ShowPopUp(url, "SYSTEM MESSAGE", 400, 125);
}

function confirmationPopUp(id, status) {
    var url = ShowConfirmationPopUpStatus_Url + '?promotionId=' + id + '&status=' + status;
    ShowPopUp(url, "SYSTEM MESSAGE", 400, 125);
}

function AppendDollor(originTextBoxId, appendTextBoxId, prefix, suffix) {
    var originText = $('#' + originTextBoxId).val();
    if (originText !== '' && originText !== undefined) {
        $('#' + originTextBoxId).hide();
        $('#' + appendTextBoxId).show();
        $('#' + appendTextBoxId).val(prefix  + originText + suffix);
    }
}

function removePopUp() {
    ReloadGrid('jqPromo');
    RemovePopUp();
}

function FocusAppendTextBox(showTextBoxId, focusTextBoxId) {
    $('#' + showTextBoxId).show();
    $('#' + focusTextBoxId).hide();
    $('#' + showTextBoxId).focus();
}

function ReloadGrid(gridId) {
    $("#" + gridId).trigger("reloadGrid");
}

$(document).ready(function () {
    $("#promoriongridrow").live('click', function (e) {
        showSpinner();
        var dataHref = this.getAttribute('data-href');
        window.location.href = dataHref;
        hideSpinner();
    });
});