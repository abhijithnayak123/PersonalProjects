$(document).ready(function () {
    var jqDataUrl = "CheckStatus/CheckStatus";
    // Set up the jquery grid
    $("#jqTable").jqGrid({
        // Ajax related configurations
        url: jqDataUrl,
        datatype: "json",
        mtype: "POST",

        // Specify the column names
        colNames: ["Check Number", "Check Submitted Date & Time", "Check Amount", "Estimated Wait Time", "Status"],

        // Configure the columns
        colModel: [
            { name: "Check Number", index: "Id", width: 140, align: "center" },
            { name: "Submitted", index: "SubmissionDate", width: 225, align: "center" },
            { name: "CheckAmout", index: "CheckAmout", width: 100, align: "center" },
            { name: "WaitTime", index: "Fee", width: 180, align: "right" },
            { name: "Status", index: "Status", width: 100, align: "center"  //,formatter: 'showlink'
                , formatter: function (cellvalue, options, rowObject) {

                    // alert(rowObject[4].toString());
                    if (rowObject[4].toString() === 'Pending')
                        return rowObject[4].toString();
                    else
                        return '<a style="text-decoration:underline;color:#0000ff" href="/CheckStatus/AcceptDecline?id='
                        + rowObject[0].toString() + '">' + rowObject[4].toString() + '</a>';
                }

            , formatoptions: { baseLinkUrl: 'AcceptDecline' }
            }
            ],
        //colModel :[{name:'EDIT',edittype:'select',formatter:'showlink', width:5,xmlmap:"Edit",formatoptions:{baseLinkUrl:'someurl.php', addParam: '&action=edit'}},

        // Grid total width and height
        cmTemplate: { sortable: false },
        width: 700,
        height: 200,

        // Paging
        toppager: true,
        pager: jQuery("#jqTablePager"),
        rowNum: 5,
        rowList: [5, 10, 20],
        viewrecords: true, // Specify if "total number of records" is displayed

        // Default sorting
        // sortname: "FName",
        // sortorder: "asc",

        // Grid caption
        caption: "Check Status"
    }).navGrid("#jqTablePager",
        { refresh: false, search: false, add: false, edit: false, del: false },
            {}, // settings for edit
            {}, // settings for add
            {}, // settings for delete
            {sopt: ["cn"]} // Search options. Some options can be set on column level
    );
});

function formatLink(cellvalue, options, rowObject) {
    if (cellvalue = 'Pending') return 'showlink'; else return 'showlink';
} 