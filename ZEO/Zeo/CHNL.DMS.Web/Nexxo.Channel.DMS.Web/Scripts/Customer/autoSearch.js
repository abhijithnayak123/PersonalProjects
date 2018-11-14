$(document).ready(function () {
    searchCustomerGrid();
});

function removeGridPopUP() {
    //Once teller click on cancel button we need to make the below property to false for next autosearch customer call
    isGridLoaded = false;
    RemoveDynIdPUP('idAutoSearch');
}


function searchCustomerGrid() {
    jQuery(document).ready(function () {
        showSpinner();

        jQuery("#jqTable").jqGrid({
            // Ajax related configurations
            url: autoSearchGrid,
            datatype: "json",
            mtype: "POST",
            // Specify the column names
            colNames: ["Customer Name", "Date of Birth", "ZEO Card Number", "Address"],

            // Configure the columns
            colModel: [
    { name: "Customer Name", index: "FName", width: 230, align: "left", formatter: 'showlink', formatter: customerIDGenerator }, // Created a new method to avoid QueryString
    { name: "Date of Birth", index: "DateOfBirth", width: 130, align: "center" },
    { name: "ZEO Card Number", index: "Cardnumber", width: 200, align: "center", formatter: cardMaskFormatter },
    { name: "Address", index: "Address", width: 250, align: "center" },
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
                hideSpinner();
            }
        }).navGrid("#jqTablePager",
        { add: false, edit: false, del: false, search: false, refresh: false },
            {}, // settings for edit
            {}, // settings for add
            {}, // settings for delete
            { sopt: ["cn"] } // Search options. Some options can be set on column level
        );
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
        cellvalue = "<a id='autosearchgrid' href='#' data-href='" + customerConformation + "/" + options.rowId + "'>" + cellvalue + "</a>";
        return cellvalue;
    }
}