$(document).ready(function () {
		$("#CardStatusLabel").css("card_status");  
		if (cardStatus == 'Active' || cardStatus == 'CardIssued') {
			$("#CardStatusLabel").addClass('green_backgrund')
		}
		else if (cardStatus == 'LostCard' || cardStatus == 'StolenCard' || cardStatus == 'ClosedForFraud' || cardStatus == 'Expired' || cardStatus == 'Closed') {
			$("#CardStatusLabel").addClass('red_backgrund'); //Red Color based on GPR Card Status
		}
		else if (cardStatus == 'Suspended') {
			$("#CardStatusLabel").addClass('yellow_backgrund')
		}
		else
		{
			$("#CardStatusLabel").addClass('otherstatus_backgrund')
		}



    $('#ProfilePopUp').bind('click', function () {
    	var customerPAN = $("#AlloyID").val();
        displaycustomerprofile(customerPAN);
    });
});

function closethis() {
    var dlgCancel = $("#dlgCustProfile");
    dlgCancel.dialog("destroy").remove();
}

function displaycustomerprofile(customerPAN) {
    var $confirm = $("<div id='dlgCustProfile'></div>");
    $confirm.empty();
    $confirm.dialog({

        autoOpen: false,
        title: "Customer Profile",
        width: 800,
        draggable: false,
        closeOnEscape: false,
        modal: true,
        height: 600,
        resizable : false,
        open: function (event, ui) {
            var url = profileURL + '?customerPAN=' + customerPAN;
            $confirm.load(url);                       
        }
    });
   
    $confirm.dialog("open");   
}



