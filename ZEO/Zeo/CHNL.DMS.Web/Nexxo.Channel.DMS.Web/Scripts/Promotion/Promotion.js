$(document).ready(function () {

    $('#promonamecount').text($('#PromotionName').attr('maxlength'));

    $('#promodesccount').text($('#PromoDescription').attr('maxlength'));

    wordCounter($('#PromotionName'), 'promonamecount');

    wordCounter($('#PromoDescription'), 'promodesccount');

    $('#PromotionName').keydown(function (e) {
        restrictPaste(e);
        wordCounter($('#PromotionName'), 'promonamecount');
    });

    $('#PromoDescription').keydown(function (e) {
        wordCounter($('#PromoDescription'), 'promodesccount');
    });

    $("#ProductType").change(function () {
        setProviders();
    });

    $("#PromotionName").blur(function () {
        Toupper();
    });

    if ($("#ProductType").val() !== "") {
        setProviders();
    }

    $("#Priority").keypress(function (e) {
        ValidateText(e);
    });

    $("#copy_from_existing").click(function () {
        ShowPUPWithDynId(copyExisting_Url, "Click promotion name to select and continue:", 855, 190, "idCopyPromo");
    });
        
    $('#PromotionName').keypress(function (e) {
        ValidatePromoName(e)
    });


    $("#promostartdate").datepicker({
        inline: true,
        dateFormat: 'mm/dd/yy',
        showOtherMonths: true,
        dayNamesMin: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
        minDate: 0,
        changeMonth: true,
        onClose: function () {
            if (this.value != undefined && this.value != '') {
                resetEndDatePicker(this.value);
                $("#promoenddate").attr("disabled", false);
            }
            else {
                $("#promoenddate").attr("disabled", "disabled");
            }
        }
    });

    if ($('#promoenddate').val() == "") {
        $('#promoenddate').attr('disabled', 'disabled')
    }
    else {
        $('#promoenddate').attr('disabled', false)
    }

    if ($('#promoenddate').val() != undefined || $('#promoenddate').val() != '') {
        resetEndDatePicker($('#promostartdate').val());
    }

    function resetEndDatePicker(startDate) {
        if ($('#promoenddate').val() != null) {
            var endDate = $('#promoenddate').val().toString();
            $('#promoenddate').datepicker('destroy');

            if (endDate != undefined && endDate != '') {
                var startPickerDate = new Date(startDate.toString());
                var endPickerDate = new Date(endDate);

                if (startPickerDate > endPickerDate) {
                    $('#promoenddate').val(startDate);
                }
            }

            $("#promoenddate").datepicker({
                inline: true,
                showOtherMonths: true,
                dayNamesMin: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
                minDate: startDate,
                changeMonth: true
            });
        }
    }

    $("#promostartdatehis").datepicker({
        inline: true,
        dateFormat: 'mm/dd/yy',
        showOtherMonths: true,
        dayNamesMin: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
        minDate: -179,
        changeMonth: true,
        onClose: function () {
            if (this.value != undefined && this.value != '') {
                resetEndDatePickerhis(this.value);
                $("#promoenddatehis").attr("disabled", false);
            }
            else {
                $("#promoenddatehis").attr("disabled", "disabled");
            }
            }
    });

    if ($('#promoenddatehis').val() != undefined || $('#promoenddatehis').val() != '') {
        resetEndDatePickerhis($('#promostartdatehis').val());
    }

    if ($('#promoenddatehis').val() == "") {
        $('#promoenddatehis').attr('disabled', 'disabled')
    }
    else {
        $('#promoenddatehis').attr('disabled', false)
    }

    function resetEndDatePickerhis(startDate) {
        if ($('#promoenddatehis').val() != null) {
            var endDate = $('#promoenddatehis').val().toString();
            $('#promoenddatehis').datepicker('destroy');

            if (endDate != undefined && endDate != '') {
                var startPickerDate = new Date(startDate.toString());
                var endPickerDate = new Date(endDate);

                if (startPickerDate > endPickerDate) {
                    $('#promoenddatehis').val(startDate);
                }
            }

            $("#promoenddatehis").datepicker({
                inline: true,
                showOtherMonths: true,
                dayNamesMin: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
                minDate: startDate,
                changeMonth: true
            });
        }
    }

    $("#showpopppromotion").live('click', function (e) {
        var dataHref = this.getAttribute('data-href');
        var $confirm = $("<div id='idCopyPromo'></div>");
        $confirm.empty();
        $confirm.dialog({
            autoOpen: false,
            title: 'Promotion Summary:',
            width: 988,
            draggable: false,
            resizable: false,
            closeOnEscape: false,
            modal: true,
            cache: false,
            position: 'top',
            overflow: 'hidden',
            open: function (event, ui) {
                $confirm.load(dataHref,
                    function (responseText, textStatus, XMLHttpRequest) {
                        var data = parseData(responseText);
                        if (data && data.success == false) {
                            RemoveDynIdPUP('idCopyPromo');
                            showExceptionPopupMsg(data.data);
                        }
                    });
            }
        });
        $confirm.dialog("open");
    });

});

function validatePromoName() {
    var promotionName = $('#PromotionName').val();
    var promotionId = $('#PromotionId').val();
    var promoCode = promotionName.toUpperCase();
    if (promotionName !== '') {
        showSpinner();
        $.ajax({
            type: "GET",
            url: validatePromoName_Url + '?promoName=' + promotionName + '&promotionId=' + promotionId,
            dataType: "json",
            contentType: "application/json; charset=UTF-8",
            data: {},
            success: function (data) {
                if (!data.success) {
                    showExceptionPopupMsg('This Promotion "' + promoCode + '" name has been used already. Please search and modify');
                    wordCounter($('#PromotionName'), 'promonamecount');
                    $("#PromotionName").val('');
                }
                hideSpinner();
            },
            error: function () {
                showExceptionPopupMsg(defaultErrorMessage);
                hideSpinner();
            }
        });
    }
}

function setProviders() {
    var productselected = $('#ProductType').val();
    showSpinner();
    $.ajax({
        type: "GET",
        url: getProviders_Url + '?productId=' + productselected,
        dataType: "json",
        contentType: "application/json; charset=UTF-8",
        data: {},
        success: function (data) {
            if (handleException(data)) {
                hideSpinner();
                return;
            }
            statesDropDown = $('select#Provider');
            bindDropdownList(statesDropDown, data);
            if (productselected === '') {
                $("#ProviderId").val('');
            }
            $('select#Provider').val($("#ProviderId").val());
            hideSpinner();
        },
        error: function () {
            showExceptionPopupMsg(defaultErrorMessage);
            hideSpinner();
        }
    });
}

function Toupper() {
    var promoCode = $('#PromotionName').val().toUpperCase();
    $('#PromotionName').val(promoCode);
}