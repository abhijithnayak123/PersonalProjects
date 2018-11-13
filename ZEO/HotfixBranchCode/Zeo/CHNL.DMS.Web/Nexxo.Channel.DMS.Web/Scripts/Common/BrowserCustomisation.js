
//begin splitter
$(document).ready(function () {
    $("#opensplitter").hide();
    $(".splitter").click(function () {
        if ($(".leftpanel").width() === 0) {
            $(".leftpanel").width('15%');
            $(".rightpanel").width('55%');
            $("#closesplitter").show();
            $("#opensplitter").hide();
            $("#divHome").show();
            $("#divBillPayment").show();
            $("#divNewCustomer").show();
            $("#divMoneyTransfer").show();
        }
        else {
            $(".leftpanel").width('0%');
            $(".rightpanel").width('70%');
            $("#opensplitter").show();
            $("#closesplitter").hide();
            $("#divHome").hide();
            $("#divBillPayment").hide();
            $("#divNewCustomer").hide();
            $("#divMoneyTransfer").hide();
        }
    });

    DisableBackButton();
	//disableBackNavigation();
    disableRightClick();
    disableTextBoxAutoComplete();
    disableRefresh();


	// to remove the shopping card applet in the home page conditionally (Not required for transaction history)
    if ($('#trnxHistory').hasClass("hideTrnxHistory")) {
    	$("#sidebar-right").css('display', 'none');
    }
});
//end splitter

function changeAmountPrecision(number) {

    var amountValue = number.value;
    var amountValueDecimal = parseFloat(amountValue);
    if (amountValueDecimal.toString() != 'NaN') {
        //number.val.(val(amountValueDecimal.toFixed(2)));
        $('#' + number.id).val(amountValueDecimal.toFixed(2));
    }

}

function DisableBackButton() {
    window.history.forward()
}


DisableBackButton();
window.onload = DisableBackButton;
window.onpageshow = function (evt) { if (evt.persisted) DisableBackButton() }
window.onunload = function () {
    void (0)
}


function disableRightClick() {
    $(document).bind("contextmenu", function (e) {
        //window.location.href = '/Login/Create';
        //alert("Right Click Not Allowed.");
        return false;
    });
}

function disableTextBoxAutoComplete() {
    try {
        $('input[type="text"]').each(function () {
            $(this).attr("autocomplete", "off");
        });
    }
    catch (e) {
    }
}

function disableRefresh() {
    $(document).bind("keydown", function (e) {
        if (e.which == 116 ) {
            // window.location.href = '/Login/Create';   // || e.which == 82
                e.preventDefault();           
                //alert("Refresh Page Not Allowed.");
            return false;
        }
    });
}

document.onkeydown = function (event) {

    if (!event) { /* This will happen in IE */
        event = window.event;
    }

    var keyCode = event.keyCode;

    if (keyCode == 8 &&
		((event.target || event.srcElement).tagName != "TEXTAREA") &&
		((event.target || event.srcElement).tagName != "INPUT")) {

        if (navigator.userAgent.toLowerCase().indexOf("msie") == -1) {
            event.stopPropagation();
        } else {
            event.returnValue = false;
        }
        return false;
    }
};

function preventBackspace(e) {
    var evt = e || window.event;
    if (evt) {
        var keyCode = evt.charCode || evt.keyCode;
        if (keyCode === 8) {
            if (evt.preventDefault) {
                evt.preventDefault();
            } else {
                evt.returnValue = false;
            }
        }
    }
}