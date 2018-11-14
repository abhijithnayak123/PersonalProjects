﻿$('body').ready(function () {
    // Add the 'hover' event listener to our drop down class
    $('#dropdown').hover(function () {
        
        if (document.getElementById("checkTrans") == null) {
            $(this).find('.sub_navigation').show();
        } else { $(this).find('.sub_navigation').hide(); }


    });

    $('#dropdown').mouseleave(function () {

        $(this).find('.sub_navigation').hide();
    });

    $('#helpmenu').hover(function () {

        if (document.getElementById("checkTrans") == null) {
            $(this).find('.sub_navigation').show();
        }
        else {
            $(this).find('.sub_navigation').hide();
        }
    });

    $('#helpmenu').mouseleave(function () {
        $(this).find('.sub_navigation').hide();
    });

});

function cancelId() {
    
    var browsername = navigator.userAgent;

    if (browsername.indexOf("Firefox") != -1) {

        $('#identificationId').css("margin-top", "-312px");


    }

}

function showExceptionPopupMsg(exceptionMessage) {
	exceptionMessage = exceptionMessage.replace(/&#39;/g, "'");
	exceptionMessage = exceptionMessage.replace("\\u0027", "'");
    var $confirmation = $("<div id='divOk'></div>");
    $confirmation.empty();
    $confirmation.dialog({
        autoOpen: false,
        title: "SYSTEM MESSAGE",
        width: 505,
        draggable: false,
        modal: true,
        resizable: false,
        closeOnEscape: false,
        minHeight: 175,
        scroll: false,
        cache: false,
        open: function (event, ui) {
            $confirmation.load(ExceptionMsgPopupURL, { dt: (new Date()).getTime(), msg: exceptionMessage }, function () {
                $('#btnOk').focus();
            });
           
        }
    });
    $confirmation.dialog("open");
    return false;
   }

   //This is added for User Story # US1956. - Start
   function showExceptionPopupMsgForActOnMyBehalf(exceptionmsg) 
   {
   	var $confirmation = $("<div id='divOk'></div>");
   	$confirmation.empty();
   	$confirmation.dialog({
   		autoOpen: false,
   		title: "SYSTEM MESSAGE",
   		width: 505,
   		draggable: false,
   		modal: true,
   		resizable: false,
   		closeOnEscape: false,
   		minHeight: 176,
   		scroll: false,
   		cache: false,
   		open: function (event, ui) {
   			$confirmation.load(ExceptionMsgPopupURLForActOnMyBehalf, { dt: (new Date()).getTime(), message: exceptionmsg }, function () {
   				$('#btnOk').focus();
   			});

   		}
   	});
   	$confirmation.dialog("open");
   	return false;
   }

   //This is added for User Story # US1956. - Completed


   //This is added for User Story # US1856. - Start
  

   function DisplayChooseLocation() {
       var $confirmation = $("<div id='divOk'></div>");
       $confirmation.empty();
       $confirmation.dialog({
           autoOpen: false,
           title: "Select Location",
           width: 505,
           draggable: false,
           modal: true,
           resizable: false,
           closeOnEscape: false,
           minHeight: 176,
           scroll: false,
           cache: false,
           open: function (event, ui) {
               $confirmation.load(DisplayURLForChooseLocation,
                       { dt: (new Date()).getTime() },
                       function (responseText, textStatus, XMLHttpRequest) {
                           var data = parseData(responseText);
                           if (data && data.success == false) {
                               $('#divOk').dialog('destroy').remove();
                               showExceptionPopupMsg(data.data);
                           }
                       });
           }
       });
       $confirmation.dialog("open");
       return false;
   }

 
   //This is added for User Story # US1856. - Completed