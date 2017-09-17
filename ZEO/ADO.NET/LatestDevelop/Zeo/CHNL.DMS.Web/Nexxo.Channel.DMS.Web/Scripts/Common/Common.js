$(document).ready(function () {

    disableNextButton();
    $('#ConsumerProtectionMessage').click(function () {
        disableNextButton();
    });
    $('#ProvidedTermsandConditonsMessage').click(function () {
        disableNextButton();
    });
    $('#DoddFrankDisclosure').click(function () {
        disableNextButton();
    });
    reOpenChatDialog();

    $(document).ajaxSuccess(function () {

        resetCrossMark();

        $('input[readonly]').focus(function () {
            this.blur();
        });

        $('textarea[readonly]').focus(function () {
            this.blur();
        })
    });

    $("input.input_box , select").on('change blur keyup focus', function () {
        resetCrossMark();
    });

    $("#MaskCardNumber").focusout(function (event) {
        var cardNumber = $("#MaskCardNumber").val();
        if (!($("#MaskCardNumber").attr('readonly') == 'readonly') && cardNumber.indexOf('*') == -1) {
            setMaxLength(19);
            maskCardNumber();
            // perform PIE encryption, and set results in hidden form fields for submission
            var pan = $("#CardNumber").val();
            var cvv = '000';
            var result = ProtectPANandCVV(pan, cvv);
            if (result != null) {
                // we only need the PAN for this demo
                protected_pan = result[0];
                protected_cvv = result[1];
                $('#CardNumber').val(protected_pan);
                $('#CVV').val(protected_cvv);
                getCardTypeByBIN(cardNumber);
            }
            else if ($("#CardNumber").val()) {
                $('#MaskCardNumber').val('').focus();

                getMessage(CustomerExceptions.Customer_Card_Error);
                return;

            }
            else if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                event.preventDefault();
            }
        }
    });

    //Added to prevent the user to focus in the disabled text box
    $('input[readonly]').focus(function () {
        this.blur();
    });

    $('textarea[readonly]').focus(function () {
        this.blur();
    })
});
//Allow only numbers for Card Number Field
function restrictCharacters(event) {
    var cardNumberCount = $("#" + event.target.id).val().replace(/ /g, '').length;
    if (cardNumberCount == 16) {
        if (event.keyCode == 13) {
            formatCardNumber();
            return;
        }
        else if (event.keyCode != 8 && event.keyCode != 9 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 46) {
            return false;
        }
    }
    // Allow: backspace, delete, tab, escape, and enter
    if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 ||
        // Allow: Ctrl+A
(event.keyCode == 65 && event.ctrlKey === true) ||
        // Allow: home, end, left, right
(event.keyCode >= 35 && event.keyCode <= 39)) {
        // let it happen, don't do anything			           
        return;
    }
    else {
        // Ensure that it is a number and stop the keypress
        if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && event.keyCode != 13) {
            event.preventDefault();
        }
        else if (event.keyCode == 13 || event.keyCode == 65) {
            $("#MaskCardNumber").focusout();
        }
    }
}
//Format the Card Number field
function formatCardNumber() {
    var temp = "";
    //Format them into the mask format "xxxx xxxx xxxx xxxx"
    var cardNumber = $("#CardNumber").val().replace(/ /g, '');
    for (var i = 3; i < 16; i += 4) {
        for (var j = i - 3; j <= i; j++) {
            temp = temp + cardNumber.charAt(j);
        }
        temp = temp + " ";
    }
    temp = $.trim(temp);
    $('#CardNumber').val(temp);
}
function disableNextButton() {
    if (!$('input[type=checkbox]:not(:checked)').length) {
        $('#btnSubmit').removeAttr("disabled").removeClass('opaqueViewCart');
    }
    else {
        $('#btnSubmit').attr("disabled", "disabled").addClass('opaqueViewCart');
    }
}

function checkShoppingCartStatus() {
    $.ajax({
        url: shoppingcartStatusCheckURL,
        data: {},
        type: 'POST',
        datatype: 'json',
        success: function (jsonData) {
        	if (jsonData && jsonData.success == false) {
        		RemovePopUp();
        		showExceptionPopupMsg(jsonData.data);
        	}
        	else
        	{
            displayShoppingCartStatusPopup(jsonData.data);
        	}
        },
        error: function (err) {
            showExceptionPopupMsg(err.data);
        }
    });
}

function displayShoppingCartStatusPopup(shoppingCartStatus) {
    var newtitle;

    showCartAbandonmentConfirm = true;

    if (shoppingCartStatus == "empty") {
        newtitle = "Confirmation";
    } else {
        newtitle = 'Message';
    }

    var $confirm = $("<div id='divshoppingcardstatus'></div>");
    $confirm.empty();
    $confirm.dialog({
        autoOpen: false,
        title: newtitle,
        width: 420,
        draggable: false,
        resizable: false,
        closeOnEscape: false,
        modal: true,
        height: 160,
        open: function (event, ui) {
            var url = shoppingCartStatusPopupURL;
            $confirm.load(url, { shoppingCartStatus: shoppingCartStatus });
        }
    });
    $confirm.dialog("open");
}

(function ($) {
    $.fn.decimalOnly = function () {
        $(this).keydown(function (event) {
            // Allow: backspace, delete, tab, escape, and enter
            if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
                // Allow: Ctrl+A
                (event.keyCode == 65 && event.ctrlKey === true) ||
                // Allow: home, end, left, right
                (event.keyCode >= 35 && event.keyCode <= 39)
               ) {
                // let it happen, don't do anything
                return;
            } else
                if (event.keyCode == 190 || event.keyCode == 110) {  // period
                    if ($(this).val().indexOf('.') !== -1) // period already exists
                        event.preventDefault();
                    else
                        return;
                } else {
                    // Ensure that it is a number and stop the keypress
                    if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                        event.preventDefault();
                    }
                }
        });
    }
})(jQuery);

function redirectSessionWarning() {
    clearTimeout(rotateSession);
    var sTimeout = parseInt(sessionTimeoutWarning) * 60 * 1000;
    rotateSession = setTimeout('showSessionWarningPopup()', sTimeout);
}

function _sessionWarnPopup(SessionWarningURL) {
    showCartAbandonmentConfirm = true;

    //console.log("SessionWarningURL:" + SessionWarningURL);
    var $confirm = $("<div id='divTimer'></div>");
    $confirm.empty();
    $confirm.dialog({
        autoOpen: false,
        title: 'Session Timeout',
        width: 400,
        draggable: false,
        resizable: false,
        closeOnEscape: false,
        modal: true,
        height: 200,
        cache: false,
        open: function (event, ui) {
            $confirm.load(SessionWarningURL);
        }
    });

    $confirm.dialog("open");
    return false;
}

function _spinner() {
    $("<div>", { id: "loading" }).appendTo('body');
    $("#loading").css({ "z-index": 99999 });

    if ($(".field-validation-error").length > 0) {
        if ($(".field-validation-error")[0].innerText != "") {
            event.preventDefault();
            return false;
        }
    } else {
        showSpinner();
    }
}

//this is placeholder validation in IE8
$(document).ready(function () {
    $('[placeholder]').parents('form').submit(function () {
        $(this).find('[placeholder]').each(function () {
            var input = $(this);
            if (input.val() == input.attr('placeholder')) {
                input.val('');
            }
        })
    });
});
//$(function () {
//	$('#ConsumerProtectionMessage').toggle(
//	$('#btnSubmit').removeAttr("disabled"), $('#btnSubmit').attr("disabled", "disabled")
//	);
//});
$(window).load(function () {
    if (($("#PanelStatus").length > 0)) {
        for (var i = 0; i < $('.DisablePanels').find('img').length; i++) {
            $('.DisablePanels').find('img')[i].id = "";
        }
    }
});
$(document).ready(function () {
    if (($("#PanelStatus").length > 0)) {
        productMenuDisable();
    }
});

function productMenuDisable() {
    $(".nav_item").each(function () {
        $(this).addClass("DisablePanels");
        $(this).attr('disabled', 'disabled');
        //$(this).removeClass("nav_item");
    });
    $(".nav_name").each(function () {
        $(this).attr('disabled', 'disabled');
    });
    $('#layout_left_pane_shadow').show();
}

$(document).on('submit', 'form', function () {
    var i = 0
    var submitbutton = $(this).find('input[type="submit"]');
    var button = $(this).find('input[type="button"]');
    setTimeout(function () {
        button.addClass('DisableInputButtons');
        submitbutton.addClass('DisableSubmitButtons');
        button.addClass('OpaqueViewCart');
        submitbutton.addClass('OpaqueViewCart');
        $('.anc_link_btn').addClass('DisableHyperLinks');
        $('.anc_link_btn').addClass('OpaqueViewCart');
        submitbutton.attr('disabled', 'disabled');
        button.attr('disabled', 'disabled');
        $('#layout_left_pane_shadow').show();
        $('.anc_link_btn').attr('disabled', 'disabled');
        $(".nav_item").each(function () {
            $(this).addClass("DisablePanels");
            $(this).attr('disabled', 'disabled');
        });
        $(".nav_name").each(function () {
            $(this).attr('disabled', 'disabled');
        });
    }, 0);

});
$(document).on('submit', 'form', function () {
    var errorIndicator = 0
    var submitbutton = $(this).find('input[type="submit"]');
    var button = $(this).find('input[type="button"]');
    setTimeout(function () {
        $(".error_align_left span").each(function () {
            if (this.innerHTML != "") {
                errorIndicator = 1;
                button.removeClass('DisableInputButtons');
                submitbutton.removeClass('DisableSubmitButtons');
                $('.anc_link_btn').removeClass('DisableHyperLinks');
                button.removeClass('OpaqueViewCart');
                submitbutton.removeClass('OpaqueViewCart');
                $('.anc_link_btn').removeClass('OpaqueViewCart');
                submitbutton.removeAttr('disabled');
                button.removeAttr('disabled');
                $('#layout_left_pane_shadow').hide();
                $('.anc_link_btn').removeAttr('disabled');
                $(".nav_item").each(function () {
                    $(this).removeClass("DisablePanels");
                    $(this).removeAttr('disabled');
                });
                $(".nav_name").each(function () {
                    $(this).removeAttr('disabled');
                });
                return false;
            }
        });
        if (errorIndicator == 0) {
            button.addClass('DisableInputButtons');
            submitbutton.addClass('DisableSubmitButtons');
            $('.anc_link_btn').addClass('DisableHyperLinks');
            $('.anc_link_btn').addClass('OpaqueViewCart');
            submitbutton.attr('disabled', 'disabled');
            $('#layout_left_pane_shadow').show();
            button.attr('disabled', 'disabled');
            $('.anc_link_btn').attr('disabled', 'disabled');
            $(".nav_item").each(function () {
                $(this).addClass("DisablePanels");
                $(this).attr('disabled', 'disabled');
            });
            $(".nav_name").each(function () {
                $(this).attr('disabled', 'disabled');
            });
        }
    }, 1000);

});

function handleException(data) {
    if (data != null && data.success != undefined && !data.success) {
        showExceptionPopupMsg(data.data);
        return true;
    }
    else
        return false;
}

; (function ($) {

    $.fn.maxlength = function () {

        $("textarea[maxlength], input[maxlength]").keypress(function (event) {
            var key = event.which;

            //all keys including return.
            if (key >= 33 || key == 13 || key == 32) {
                var maxLength = $(this).attr("maxlength");
                var length = this.value.length;
                if (length >= maxLength) {
                    event.preventDefault();
                }
            }
        });
    }

})(jQuery);

function reOpenChatDialog() {
    if (sessionStorage.getItem("isChatOpen") != null && sessionStorage.getItem("isChatOpen") == "true") {
        if (sessionStorage.getItem("left") != null) {
            var left = parseInt(sessionStorage.getItem("left"));
            var top = parseInt(sessionStorage.getItem("top"));
            var ticketNumber = sessionStorage.getItem("ticketNumber");

            $('#chatCenter').dialog('destroy').remove();
            var $chatCenter = $("<div id='chatCenter'></div>");
            $chatCenter.empty();

            $chatCenter.dialog({
                autoOpen: false,
                title: "Chat",
                width: 505,
                draggable: true,
                modal: false,
                resizable: false,
                closeOnEscape: false,
                minHeight: 225,
                scroll: false,
                cache: false,
                position: [left, top],
                open: function (event, ui) {
                    $chatCenter.load(ChatCenterMsgPopupURL + '?transactionId=' + ticketNumber, { dt: (new Date()).getTime() });
                },
                close: function (event, ui) {
                    sessionStorage.removeItem("isChatOpen");
                    sessionStorage.removeItem("left");
                    sessionStorage.removeItem("top");
                },
                dragStop: function (event, ui) {
                    var elem = $(this);
                    sessionStorage.setItem("left", elem.offset().left);
                    sessionStorage.setItem("top", elem.offset().top);
                }
            });
            $chatCenter.dialog("open");
        }
    }
}
$.fn.spin = function (opts) {
    this.each(function () {
        var $this = $(this),
          data = $this.data();

        if (data.spinner) {
            data.spinner.stop();
            delete data.spinner;
        }
        if (opts !== false) {
            data.spinner = new Spinner($.extend({ color: $this.css('color') }, opts)).spin(this);
        }
    });
    return this;
};
function showSpinners(control) {
    control.fadeIn();
    //clearTimeout(rotate);
    var opts = {
        lines: 12, // The number of lines to draw
        length: 15, // The length of each line
        width: 4, // The line thickness
        radius: 10, // The radius of the inner circle
        color: '#000', // #rgb or #rrggbb
        speed: 1, // Rounds per second
        trail: 100, // Afterglow percentage
        shadow: false, // Whether to render a shadow
        hwaccel: false // Whether to use hardware acceleration
    };
    if (control.spin)
        control.spin(opts);
}

function hideSpinners(control) {
    var browsername = navigator.userAgent;

    var msie = browsername.indexOf("MSIE")
    var version = parseInt(browsername.substring(msie + 5, browsername.indexOf(".", msie)));

    //In case of IE 8 use Hide() method else use Fadeout() method.
    if (browsername.indexOf("MSIE") != -1 && version == 8) {
        control.hide();
    }
    else {
        control.fadeOut();
    }
}

function showSpinner() {
    showSpinners($('#loading'));
    if ($('#loading span').text() != '') {
        $('#loading .spinner_msg_banner').css('background-color', '#fcf9f9');
        $('#loading .spinner_msg_banner').css('opacity', '0.9');
        var bannerheight = ($('#loading span').text().length / 48).toFixed(2) * 10 + "%";
        if (parseInt(bannerheight) > 16) {
            $('#loading .spinner_msg_banner').css('height', bannerheight);
        }
        if (parseInt(bannerheight) > 20) {
            $('#loading .spinner_msg').css('top', '48%');
        }
        else {
            $('#loading .spinner_msg').css('top', '68%');
        }
    }
}

function hideSpinner() {
    hideSpinners($('#loading'));
    $("#loadingmsgspinner").text("");
    $('#loading .spinner_msg_banner').css('background-color', '');
}

function bindDropdownList(dropdownList, list) {
    dropdownList.empty();
    var items = '';
    if (list.length > 1) {
        $.each(list, function (i, item) {
            items += '<option value="' + item.Value + '">' + item.Text + '</option>';
        });
    }
    else {
        items = '<option value="">Not Applicable</option>';
    }
    dropdownList.html(items);
}

function setDropDownNotApplicable(dropdownList) {
    dropdownList.prop("disabled", true);
    dropdownList.empty();
    dropdownList.html('<option value="">Not Applicable</option>');
}

function setDropDownSelect(dropdownList) {
    dropdownList.prop("disabled", false);
    dropdownList.empty();
    dropdownList.html('<option value="">Select</option>');
}

function parkAndDeleteShoppingCartItems() {
    $.ajax({
        url: shoppingCartItemsUrl,
        data: {},
        async: 'false',
        type: 'GET',
        datatype: 'json',
        success: function (jsonData) {
            RedirectToUrl(LogoutUrl);
        },
        error: function (err) {
        }
    });
}

function parkAndDeleteShoppingCartItemsWithoutRedirect() {
    $.ajax({
        url: shoppingCartItemsUrl,
        data: {},
        async: false,
        type: 'GET',
        datatype: 'json',
        success: function (jsonData) {
        },
        error: function (err) {
        }
    });
}

function redirectToLogin() {
    hasTransaction = localStorage.getItem('hasTransaction');
    localStorage.removeItem('hasTransaction');
    if (hasTransaction != null && hasTransaction == "true") {
        parkAndDeleteShoppingCartItems();
    }
    else {
        RedirectToUrl(LogoutUrl);
    }
}

//This method Should be used to redirect inside customer session
function RedirectToUrl(url) {
    showCartAbandonmentConfirm = false; //Dont Show cart Abandonment Popup
    window.location.href = url;
}

function UpdateCounterId() {
    if (UpdateCounterIdUrl != '') {
        $.ajax({
            type: "POST",
            async: false,
            url: UpdateCounterIdUrl,
            data: "{}",
            processData: true,
            success: function (data) {
            },
            error: function (data) {
            }
        });
    }
}
function avoidSpecialCharNegSign(event, e) {
    if ($.browser.mozilla == true) {
        if (event.which == 8 || event.keyCode == 37 || event.keyCode == 39 || event.keyCode == 9 || event.keyCode == 16 || event.keyCode == 46) {
            return true;
        }
    }
    if ((event.which != 46 || $(e).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
        event.preventDefault();
    }
}
//Author : Abhijith
//Description : Show popup in case of errors in Post Flush in ShoppingcartCheckoutSuccess screen.
//Starts Here
function showExceptionPopupMsg(exceptionMessage, redirect) {
	exceptionMessage = exceptionMessage.replace(/&#39;/g, "'");
	exceptionMessage = exceptionMessage.replace("\\u0027", "'");
    var $confirmation = $("<div id='divOk'></div>");
	$('#divOk').dialog('destroy').remove();
    $confirmation.empty();
    
    var url = ExceptionMsgPopupURL;
    if (redirect == true) {
        url = ExceptionRedirectPopupURL;
    }

    $confirmation.dialog({
        autoOpen: false,
        title: "SYSTEM MESSAGE",
        width: 505,
        draggable: false,
        modal: true,
        resizable: false,
        closeOnEscape: false,
        minHeight: 225,
        scroll: false,
        cache: false,
        open: function (event, ui) {
            $confirmation.load(url, { dt: (new Date()).getTime(), msg: exceptionMessage }, function () {
                $('#btnOk').focus();
            });

        }
    });
    $confirmation.dialog("open");
    return false;
}
//Ends Here

(function ($) {
    "use strict";
    // A nice closure for our definitions

    function getjQueryObject(string) {
        // Make string a vaild jQuery thing
        var jqObj = $("");
        try {
            jqObj = $(string).clone();
        } catch (e) {
            jqObj = $("<span />").html(string);
        }
        return jqObj;
    }

    function isNode(o) {
        return !!(typeof Node === "object" ? o instanceof Node : o && typeof o === "object" && typeof o.nodeType === "number" && typeof o.nodeName === "string");
    }
    $.print = $.fn.print = function () {
        // Print a given set of elements
        var options, $this, self = this;
        if (self instanceof $) {
            // Get the node if it is a jQuery object
            self = self.get(0);
        }
        if (isNode(self)) {
            // If `this` is a HTML element, i.e. for
            // $(selector).print()
            $this = $(self);
            if (arguments.length > 0) {
                options = arguments[0];
            }
        } else {
            if (arguments.length > 0) {
                // $.print(selector,options)
                $this = $(arguments[0]);
                if (isNode($this[0])) {
                    if (arguments.length > 1) {
                        options = arguments[1];
                    }
                } else {
                    // $.print(options)
                    options = arguments[0];
                    $this = $("html");
                }
            } else {
                // $.print()
                $this = $("html");
            }
        }
        // Default options
        var defaults = {
            globalStyles: true,
            mediaPrint: false,
            stylesheet: null,
            noPrintSelector: ".no-print",
            iframe: true,
            append: null,
            prepend: null
        };
        // Merge with user-options
        options = $.extend({}, defaults, (options || {}));
        var $styles = $("");
        if (options.globalStyles) {
            // Apply the stlyes from the current sheet to the printed page
            $styles = $("style, link, meta, title");
        } else if (options.mediaPrint) {
            // Apply the media-print stylesheet
            $styles = $("link[media=print]");
        }
        if (options.stylesheet) {
            // Add a custom stylesheet if given
            $styles = $.merge($styles, $('<link rel="stylesheet" href="' + options.stylesheet + '">'));
        }
        // Create a copy of the element to print
        var copy = $this.clone();
        // Wrap it in a span to get the HTML markup string
        copy = $("<span/>").append(copy);
        // Remove unwanted elements
        copy.find(options.noPrintSelector).remove();
        // Add in the styles
        copy.append($styles.clone());
        // Appedned content
        copy.append(getjQueryObject(options.append));
        // Prepended content
        copy.prepend(getjQueryObject(options.prepend));
        // Get the HTML markup string
        var content = copy.html();
        // Destroy the copy
        copy.remove();
        var w, wdoc;
        if (options.iframe) {
            // Use an iframe for printing
            try {
                var $iframe = $(options.iframe + "");
                var iframeCount = $iframe.length;
                if (iframeCount === 0) {
                    // Create a new iFrame if none is given
                    $iframe = $('<iframe height="0" width="0" border="0" wmode="Opaque"/>').prependTo('body').css({
                        "position": "absolute",
                        "top": -999,
                        "left": -999
                    });
                }
                w = $iframe.get(0);
                w = w.contentWindow || w.contentDocument || w;
                wdoc = w.document || w.contentDocument || w;
                wdoc.open();
                wdoc.write(content);
                wdoc.close();
                setTimeout(function () {
                    // Fix for IE : Allow it to render the iframe
                    w.focus();
                    w.print();
                    setTimeout(function () {
                        // Fix for IE
                        if (iframeCount === 0) {
                            // Destroy the iframe if created here
                            $iframe.remove();
                        }
                    }, 100);
                }, 250);
            } catch (e) {
                // Use the pop-up method if iframe fails for some reason
                console.error("Failed to print from iframe", e.stack, e.message);
                w = window.open();
                w.document.write(content);
                w.document.close();
                w.focus();
                w.print();
                w.close();
            }
        } else {
            // Use a new window for printing
            w = window.open();
            w.document.write(content);
            w.document.close();
            w.focus();
            w.print();
            w.close();
        }
        return this;
    };
})(jQuery);

function ClosePopup() {
    $('#divPoupClose').dialog('destroy').remove();
    $('#divPrepaid').dialog('destroy').remove();
}

function LinkFormatter(cellvalue, options, rowObject) {
    return '&action = ' + cellvalue;
}

//AL-436 Allow Only Numbers for Certegy Site Id field
var regexNumbersOnly = /^([0-9])$/;
function ValidateKey(e, regex) {
    var enterKey = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (regex.test(enterKey)) {
        return true;
    }
    e.preventDefault();
    return false;
}

// AL-4034 : window.event is used for IE browser only and else part have if condition is access for other browser ex. chrome, firefox
// User clicks on browser back it's redirect to logout page.
function logOutOnBackButton() {
    hasTransaction = localStorage.getItem('hasTransaction');
    localStorage.removeItem('hasTransaction');
    if (hasTransaction != null && hasTransaction == "true") {
        parkAndDeleteShoppingCartItems();
        RedirectToUrl(LogoutUrl);
    }
    else {
        RedirectToUrl(LogoutUrl);
    }
}

function CommonSSNMasking(element) {
    var $this = $(element);
    var value = $this.val();
    if (value.charAt(0) != "*" && value != "")
        $("#ActualSSN").val(value);
    if (value.length != 0 && value.replace("_", "").length == 11 && value != "_________") {
        var maskedValue = "***-**-";
        var ssn = maskedValue + value.substring(7, value.length).replace("-", "");
        $this.val(ssn);
    }
    else {
        $('#ActualSSN').val('');
        $("#SSN").mask("999-99-9999");
    }
}
//AL-6293 starts
function sanitizeTextOnPaste(inputControl, reg) {
    var inputval = $(inputControl).val();
    r = reg;
    var isSplChar = reg.test(inputval);
    if (isSplChar) {
        var no_spl_char = inputval.replace(reg, '');
        $(inputControl).val(no_spl_char);
    }
}//ends

function parseData(data) {
    try {
        var obj = JSON.parse(data);
        return obj;
    } catch (ex) {
        return null;
    }
}

function resetCrossMark()
{
    $('input.input_box:disabled').each(function () {
        $('#' + $(this).attr('id')).siblings('a').hide();
    });

    $('input.input_box:enabled').each(function () {
        $('#' + $(this).attr('id')).siblings('a').show();
    });

    $('input.input_box.disable_txt').each(function () {
        $('#' + $(this).attr('id')).siblings('a').hide();
    })
}