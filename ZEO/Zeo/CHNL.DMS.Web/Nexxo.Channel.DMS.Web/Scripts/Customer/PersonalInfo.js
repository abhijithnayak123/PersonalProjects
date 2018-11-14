var isGridLoaded = false;
var prevSSN = '';

$(document).ready(function () {

    $("#divUserSessionId").css("display", "");

    $("#DateOfBirth").mask("99/99/9999");

    maskSSN();

    if($("#SSN").val() != '' && isAutoSearchRequired === false){
        SSNValidation();
    }

    $("#SSN").focusout(function () {
        var presentSSN = $("#ActualSSN").val();
        if (prevSSN != presentSSN)
        {
            prevSSN = presentSSN;
            $("#SSN").trigger("change");
        }

        if(isAutoSearchRequired === false)
            SSNValidation();
    });

    $("#SSN").focusin(function() {

        var ssnVal = this.value;

        if (ssnVal != null || ssnVal != "" || ssnVal.length > 0)
            $("#SSN").val(ssnVal).select();

        if (ssnVal.charAt(0) != "*")
            $("#ActualSSN").val(ssnVal);

        return true;
    });

    $("#LastName, #SSN, #DateOfBirth").change(function (event) {
        maskSSN();
        if ($("#DateOfBirth").hasClass('valid')) {
            autoSearchCustomers(event);
        }
        
    });

    $("#autosearchgrid").live('click', function (e) {
        var dataHref = this.getAttribute('data-href');
        ShowPopUpMinHeight(dataHref, 'Customer Details', 380, 300);
    });

    //to handle the cross button
    setTimeout(function () {
        $('#SSN').siblings('.clearlink').mousedown(function () {
            $("#ActualSSN").val('');
            $("#SSN").val('');
        });
    }, 10);


    $("#SSN").keydown(function (e) {        
        var ssnVal = $("#SSN").val();
        var key = e.which || e.keyCode;       

        if (!e.shiftKey && !e.altKey && !e.ctrlKey && key == 8 || key == 46 || key == 45) {                
            $("#ActualSSN").val('');
            $("#SSN").val('');
        } 

        if ((e.shiftKey && key == 37) || (e.shiftKey && key == 39) || key == 35 || key == 36) {
            
            if (ssnVal.length > 0 || ssnVal != "") {
                $("#SSN").val(ssnVal).focus();
                $("#SSN").val(ssnVal).select();
            }
        }

        restrictPaste(e);

        if (($("#SSN").val().length) >= 9 && key != 9) {
            return false;
        }
    });

    var clientProfileStatus = $('#ClientProfileStatus').val();

    if (canEnableProfileStatus != 'True') {
        if (clientProfileStatus.toLowerCase() == 'inactive') {
            $("#CustomerProfileStatus").prop("disabled", true);
        }
    }

    $('#CustomerProfileStatus').change(function () {
        var status = $('#CustomerProfileStatus').val();
        $('#hdnCustomerProfileStatus').val($('#CustomerProfileStatus').val());
        $.ajax(
			{
			    url: Profile_URL,
			    type: "POST",
			    dataType: "json",
			    data: { profileStatus: status },
			    success: function (data) {
			        if (data.success == true) {
			            if (status == "Closed" && data.cartStatus == "empty") {
			                var $confirm = $("<div id='divTrans'></div>");
			                $confirm.empty();
			                $confirm.dialog({
			                    autoOpen: false,
			                    title: "Message",
			                    width: 410,
			                    draggable: false,
			                    resizable: false,
			                    closeOnEscape: false,
			                    modal: true,
			                    height: 190,
			                    open: function (event, ui) {
			                        var url = CustomerStatusCheck;
			                        $confirm.load(url);
			                    }
			                });
			                $confirm.dialog("open");
			            }
			            else if (status == "Closed" && data.cartStatus == "nonempty") {
			                displayShoppingCartStatusClosedPopup("nonempty");
			            }
			            else {
			                return true;
			            }
			        }
			        else if (data.success == false) {
			            showExceptionPopupMsg(data.data);
			        }
			    }
			});
    })

});

function displayShoppingCartStatusClosedPopup(shoppingCartStatus) {

    var $confirm = $("<div id='divshoppingcardstatus'></div>");
    $confirm.empty();
    $confirm.dialog({
        autoOpen: false,
        title: 'Message',
        width: 420,
        draggable: false,
        resizable: false,
        closeOnEscape: false,
        modal: true,
        height: 160,
        open: function (event, ui) {
            var url = ShoppingCartEmptyWhenClosedURL;
            $confirm.load(url, { shoppingCartStatus: shoppingCartStatus });
        }
    });
    $confirm.dialog("open");
}

function maskSSN() {
    var ssnVal = $('#SSN').val();

    if (ssnVal.length == 11 || ssnVal.length == 9) {

        var SSN = ssnVal.replace(/-/g, "");
        var maskedValue = "***-**-";

        if (SSN.charAt(0) != "*" || ssnVal == "")
            $("#ActualSSN").val(SSN);

        if (SSN != "" && SSN.length == 9) {
            SSN = maskedValue + SSN.substring(5, SSN.length);
            $('#SSN').val(SSN);
        }
    }
}

function autoSearchCustomers(e) {
    var actualSSN = $("#ActualSSN").val();
    var ssn =  $("#SSN").val();
    var dob = $("#DateOfBirth").val();
    var lastName = $("#LastName").val();
       
    if(isAutoSearchRequired === true && isGridLoaded === false)
    {
        if ((dob != '' && dob != null) && (lastName != null && lastName.replace(/ /g, '').length > 0) && (ssn.length >= 9 || ssn === '' )) {
            showSpinner();
            isGridLoaded = true;
            $.ajax({
                url: customerAutoSearchURL,
                data: { ssn: actualSSN, dob: dob, lastName: lastName },
                type: 'POST',
                datatype: 'json',
                success: function (jsonData) {
                    if(!handleException(jsonData) && jsonData.success === true && jsonData.isCustomerFound === true) {
                        hideSpinner();
                        ShowPUPWithDynId(autoSearchCustomersPopUp, "Customer was found. Click customer name to select and continue:", 855, 190, "idAutoSearch");
                    }
                    else {
                        isGridLoaded = false;
                    }

                    hideSpinner();
                },
                error: function (err) {
                    hideSpinner();
                    showExceptionPopupMsg(err.data);
                }
            });
        }
    }
}

function SSNValidation() {
    $('#divError').text('');
    $('#divError').hide();
    var ssnval = $("#ActualSSN").val();
    var isDuplicateSSNCheckRequired = true;//AL-232
    if (ssnval == '888888888') {
        isDuplicateSSNCheckRequired = false;
    }

    if (ssnval != null && ssnval != '' && isDuplicateSSNCheckRequired)//AL-232
    {
        showSpinner();
        $.ajax({
            url: PersonalInfoValidateSSNURL,
            data: { SSN: ssnval },
            type: 'POST',
            datatype: 'json',
            success: function (jsonData) {
                if (!jsonData.success) {
                    hideSpinner();
                    showExceptionPopupMsg(jsonData.data);
                }
                else if (jsonData.msg != null && jsonData.msg != '') {
                    $('#divError').text(jsonData.msg);
                    $('#SSN').val('');
                    hideSpinner();
                    showExceptionPopupMsg(jsonData.msg);
                }
                hideSpinner();
            },
            complete: function () {
                hideSpinner();
            },
            error: function (err) {
                hideSpinner();
                showExceptionPopupMsg(err.data);
            }
        });
    }
}



