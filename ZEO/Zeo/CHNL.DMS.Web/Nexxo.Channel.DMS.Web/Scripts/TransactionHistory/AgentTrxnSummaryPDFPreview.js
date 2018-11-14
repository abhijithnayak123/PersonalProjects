$(document).ready(function () {
    //as we are giving option to click on close button to destroy popup
    $(".ui-dialog-titlebar-close").click(function () {
        $('#divPreviewPDF').empty().dialog('destroy').remove();
    });
});