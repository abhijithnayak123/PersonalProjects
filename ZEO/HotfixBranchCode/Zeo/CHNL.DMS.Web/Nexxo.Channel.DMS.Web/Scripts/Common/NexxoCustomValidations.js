jQuery.validator.unobtrusive.adapters.add
    ("mycustomvalidation", ['defaultvalue', 'dependingproperty', 'dependingpropertyvalue', 'isrequired'],
    function (options) {
    	options.rules['mycustomvalidation'] = { other: options.params.other,
    		defaultvalue: options.params.defaultvalue,
    		dependingproperty: options.params.dependingproperty,
    		dependingpropertyvalue: options.params.dependingpropertyvalue,
    		isrequired: options.params.isrequired
    	};
    	options.messages['mycustomvalidation'] = options.message;
    }
);


jQuery.validator.addMethod("mycustomvalidation", function (value, element, params) {
	if (params.isrequired == "True") {
		if ($(element)) {
			retVal = $(element).attr("value");
		}
		if (params.dependingproperty != "" && params.dependingpropertyvalue != "") {
			if (params.dependingpropertyvalue == "Cell" && retVal == params.defaultvalue) {
				return false;
			}
		}
		else if (retVal == params.defaultvalue) {
			return false;
		}
	}
	return true;
});

jQuery.validator.unobtrusive.adapters.add("phonevalidation", ['defaultvalue', 'dependingproperty', 'dependingpropertyvalue'],
    function (options) {
    	options.rules['phonevalidation'] = { other: options.params.other,
    		defaultvalue: options.params.defaultvalue,
    		dependingproperty: options.params.dependingproperty,
    		dependingpropertyvalue: options.params.dependingpropertyvalue
    	};
    	options.messages['phonevalidation'] = options.message;
    });


jQuery.validator.addMethod("phonevalidation", function (value, element, params) {

	if ($(element)) {
		retVal = $(element).attr("value");
	}

	if ($('#AlternativePhone'))
	{ var dependingpropertyvalue = $('#AlternativePhone').attr("value"); }

	if (dependingpropertyvalue != "" && retVal == params.defaultvalue) {
		return false;
	}
	return true;
});



$.validator.unobtrusive.adapters.addSingleVal("dateofbirthformat", "other");

jQuery.validator.addMethod("dateofbirthformat", function (value, element, other) {
	if ($(element)) {
		if (value == null || value == "" || value == '__/__/____' || value == 'MM/DD/YYYY') {
			return true;
		}
		var month = 0;
		var day = 0;
		var year = 0;
		month = parseInt(value.split('/')[0], 10); //month
		day = parseInt(value.split('/')[1], 10); //date
		year = parseInt(value.split('/')[2]); //year

		var valid = true;

		if ((month < 1) || (month > 12)) valid = false;
		else if ((day < 1) || (day > 31)) valid = false;
		else if (((month == 4) || (month == 6) || (month == 9) || (month == 11)) && (day > 30)) valid = false;  
		else if ((month == 2) && ((((year % 4) == 0) && ((year % 100) != 0)) || ((year % 400) == 0)) && (day > 29)) valid = false;
		else if ((month == 2) && ((year % 4) != 0) && (day > 28)) valid = false;

		if (isNaN(year) || year.toString().length != 4)
		{ valid = false; }

		return valid;
	}
	return true;
});


$.validator.unobtrusive.adapters.addSingleVal("dateofbirthnofuturedate", "other");

jQuery.validator.addMethod("dateofbirthnofuturedate", function (value, element, other) {
if(value == '' || value == '__/__/____' || value == 'MM/DD/YYYY'){
	return true;
    }
	if ($(element)) {
		var valid = false;
		var myDates = new Array()
		myDates[0] = value.split('/')[0]; //month
		myDates[1] = value.split('/')[1]; //date
		myDates[2] = value.split('/')[2]; //year

		var enteredDate = new Date();

		enteredDate.setMonth(myDates[0] - 1);
		enteredDate.setDate(myDates[1]);
		enteredDate.setFullYear(myDates[2]);

		var today = new Date();

		if (enteredDate > today)
			valid = false;
		else
			valid = true;
		return valid;
	}
	return true;
});


jQuery.validator.unobtrusive.adapters.add
    ("mydatecustomvalidation", ['minvalue', 'maxvalue'],
    function (options) {
    	options.rules['mydatecustomvalidation'] = { other: options.params.other,
    		minvalue: options.params.minvalue,
    		maxvalue: options.params.maxvalue
    	};
    	options.messages['mydatecustomvalidation'] = options.message;
    }
);


    jQuery.validator.addMethod("mydatecustomvalidation", function (value, element, params)
    {
	if (value == "" || value == '__/__/____' || value == 'MM/DD/YYYY'){
		return true;
        }
	if ($(element)) {
		var myDates = new Array()
		myDates[0] = value.split('/')[0]; //month
		myDates[1] = value.split('/')[1]; //date
		myDates[2] = value.split('/')[2]; //year

		var enteredDate = new Date();

		enteredDate.setMonth(myDates[0] - 1);
		enteredDate.setDate(myDates[1]);
		enteredDate.setFullYear(myDates[2]);

		var maxDate = new Date();
		maxDate.setFullYear(maxDate.getFullYear() - params.minvalue);
		var minDate = new Date();
		minDate.setFullYear(minDate.getFullYear() - params.maxvalue);
		if (minDate <= enteredDate && enteredDate <= maxDate) {
			return true;
		}
		return false;
	}
	return true;
});


//MyConditionalRequiredCustomValidation Development.
jQuery.validator.unobtrusive.adapters.add
    ("myconditionalrequiredcustomvalidation", ['propertyone', 'propertytwo'],
    function (options) {
    	options.rules['myconditionalrequiredcustomvalidation'] = { other: options.params.other,
    		propertyone: options.params.propertyone,
    		propertytwo: options.params.propertytwo
    	};
    	options.messages['myconditionalrequiredcustomvalidation'] = options.message;
    }
);

jQuery.validator.addMethod("myconditionalrequiredcustomvalidation", function (value, element, params) {
	if (params.propertytwo == 'IssueDateCondition') {
		if ($('#' + params.propertyone)) {
			var propertyonevalue = $('#' + params.propertyone).val();
			if (propertyonevalue.toLowerCase() == 'driver\'s license' || propertyonevalue.toLowerCase() == 'employment authorization card (ead)' || propertyonevalue.toLowerCase() == 'green card / permanent resident card') {
				if (value == null || value == "")
					return false;
				else
					return true;
			}
			else {
				return true;
			}
		}
	}


	if ($('#' + params.propertyone)) {
		var propertyonevalue = $('#' + params.propertyone).val();
	}
	if ($('#' + params.propertytwo)) {
		var propertytwovalue = $('#' + params.propertytwo).val();
	}

	if ((propertyonevalue.toLowerCase() == 'united states' && propertytwovalue.toLowerCase() == 'driver\'s license') || (propertyonevalue.toLowerCase() == "united states" && propertytwovalue.toLowerCase() == "u.s. state identity card") || (propertyonevalue.toLowerCase() == 'mexico' && propertytwovalue.toLowerCase() == 'licencia de conducir')) {
		if (value == '')
			return false;
	}

	if (params.propertytwo == "GovtIdIssueState" || params.propertytwo == "StateProvinceCode") {
		if ($('select#' + params.propertyone)) {
			var propertyonevalue = $('select#' + params.propertyone).val();
		}
		if ($('select#' + params.propertytwo)) {
			var propertytwovalue = $('select#' + params.propertytwo).val();
		}

		if (propertyonevalue.toLowerCase() == 'us' || propertyonevalue.toLowerCase() == 'united states' || propertyonevalue.toLowerCase() == 'usa' || propertyonevalue.toLowerCase() == 'canada' || propertyonevalue.toLowerCase() == 'mexico' || propertyonevalue.toLowerCase() == 'mx' && propertytwovalue.toLowerCase() == '') {
			if (value == '')
				return false;
		}
	}

	if (params.propertytwo == "City" || params.propertytwo == "CityID") {
		if ($('select#' + params.propertyone)) {
			var propertyonevalue = $('select#' + params.propertyone).val();
		}
		if ($('select#' + params.propertytwo)) {
			var propertytwovalue = $('select#' + params.propertytwo).val();
		}
		if (propertyonevalue.toLowerCase() == 'mexico' || propertyonevalue.toLowerCase() == 'mx') {
			if (value == '')
				return false;
		}
	}

    if (params.propertytwo == "TestAnswer" || params.propertytwo == "TestQuestion") {
        var propertyonevalue = '';
        var propertytwovalue = '';
        if ($('#' + params.propertyone)) {
            propertyonevalue = $('#' + params.propertyone).val();
        }
        if ($('#' + params.propertytwo)) {
            propertytwovalue = $('#' + params.propertytwo).val();
        }
        if (propertyonevalue != '' && propertytwovalue == '')
            return false;
    }
    
    if (params.propertytwo == "DeliveryOptions") {
        var propertyonevalue = '';
        var propertytwovalue = '';
        if ($('#' + params.propertyone)) {
            propertyonevalue = $('#' + params.propertyone).val();
        }
        if ($('#' + params.propertytwo)) {
            propertytwovalue = $('#' + params.propertytwo +' option:selected').text();
        }
        if (propertyonevalue == '' && propertytwovalue.toLowerCase() == 'phone notification')
            return false;
    }

	return true;
});


//SSN Number
jQuery.validator.unobtrusive.adapters.add("ssnnumber", ["acutalssn"],
	   function (options) {
	   	
	   	options.rules["ssnnumber"] = options.params.acutalssn;
	   	options.messages["ssnnumber"] = options.message;
	   });

	   jQuery.validator.addMethod("ssnnumber", function (value, element, param) {
	    var myRegExp = /^(?!(000|666))([0-9]\d{2}|7([0-9]\d|7[012]))-?(?!00)\d{2}-?(?!0000)\d{4}$/;
	   	var myRegExp1 = /^[0-9]([0-9]|-(?!-))+$/;
	    var ssnvalue = $("#SSN").val();

	    if (ssnvalue == '___-__-____')
	        return true;

	   	if (ssnvalue.charAt(0) != "*")
	   		$("#ActualSSN").val(ssnvalue);

	   	var ssn = $('#ActualSSN').val();
	   	if (ssn != null && ssn.length > 0) {
	   		var result = false;
	   		 ssn = ssn.replace(/-/g, '');
	   		if (ssn.substr(0,1) == 9)
	   		{
	   			var value1 = parseInt(ssn.substr(3, 2));
	   			if (value1 >= 70 & value1 <= 88 | value1 >= 90 & value1 <= 92 | value1 >= 94 & value1 <= 99)
	   			{
	   				result = myRegExp1.test(ssn);
	   			}	   			
	   			else
	   				result = myRegExp.test(ssn);
	   		}
			else
	   		result = myRegExp.test(ssn);

	   		return result;
	   	}
	   	return true;
	   });

//Create new users
jQuery.validator.unobtrusive.adapters.add("temppwd", ["userstatus"],
	   function (options) {
	   	options.rules["temppwd"] = options.params.userstatus;
	   	options.messages["temppwd"] = options.message;
	   });

jQuery.validator.addMethod("temppwd", function (value, element, param) {
	var status = $('#UserStatus').val();

	if (status == "2" || status == "3") {
		return true;
	}

	if (status == "1") {
		if (value == null || value.length == 0)
			return false;
	}
	if (value == null || value.length == 0)
		return false;
	return true;
});

//Phone number sequence
jQuery.validator.unobtrusive.adapters.add("phonenumbersequence", ["phone"],
	   function (options) {
	   	options.rules["phonenumbersequence"] = options.params.phone;
	   	options.messages["phonenumbersequence"] = options.message;
	   });


	   jQuery.validator.addMethod("phonenumbersequence", function (value, element, param)
	   {
	   	var myRegExp1 = /^[2-9]\d{9}$/;
	   	var myRegExp2 = /2{9}|3{9}|4{9}|5{9}|6{9}|7{9}|8{9}|9{9}/;
	   	var phoneNumber = null;

	   	phoneNumber = $('#' + param).val();
	   	if (phoneNumber == null || phoneNumber == '' || phoneNumber == '___-___-____')
	   		return true;

	   	if (phoneNumber != null && phoneNumber.length != 0)
	   	{
	   		phoneNumber = phoneNumber.replace(/-/g, "");

	   		var result = (!myRegExp1.test(phoneNumber) || myRegExp2.test(phoneNumber));

	   		return !result;
	   	}
	   	return true;
	   });


//MyConditionalCompareRequiredCustomValidation Development.
jQuery.validator.unobtrusive.adapters.add
    ("myconditionalcomparerequiredcustomvalidation", ['firstvalue', 'secondvalue', 'cardLength'],
    function (options) {
    	options.rules['myconditionalcomparerequiredcustomvalidation'] = { other: options.params.other,
    		propertyone: options.params.firstvalue,
    		propertytwo: options.params.secondvalue
    	};
    	options.messages['myconditionalcomparerequiredcustomvalidation'] = options.message;
    }
);

jQuery.validator.addMethod("myconditionalcomparerequiredcustomvalidation", function (value, element, params) {
	if ($('#channelPartner_AuthenticationType')) {
		var propertyvalue = $('#channelPartner_AuthenticationType').val();
	}

	if ($('#CardLength')) {
		var cardLength = $('#CardLength').val();
	}
	if (params.propertyone == propertyvalue) {
		if (value == "")
			return false;
		else if (value.length != cardLength)
			return false;
	}
	return true;
});

//Phone Provider Hide/Show Script
$(document).ready(function () {
    if ($('#PrimaryPhoneType').val() != "Cell" && $('#AlternativePhoneType').val() != "Cell")
		    $('#ReceiveTextMessage').attr('disabled', true);
    else
    $('#ReceiveTextMessage').attr('disabled', false);

	if ($('#PrimaryPhoneType').val() != "Cell") {
		$('#PrimaryPhoneProvider').hide();
		$('#phoneProvider').hide();
	}
	else {
		$('#PrimaryPhoneProvider').show();
		$('#phoneProvider').show();
	}
	if ($('#AlternativePhoneType').val() != "Cell") {
		$('#AlternativePhoneProvider').hide();
		$('#alternatePhoneProvider').hide();
		$('span[htmlFor="AlternativePhoneProvider"]').hide();
	}
	else {
		$('#AlternativePhoneProvider').show();
		$('#alternatePhoneProvider').show();
	}
	$('#PrimaryPhoneType,#AlternativePhoneType').change(function () {
	    if ($('#PrimaryPhoneType').val() == "Cell") {
	        $('#PrimaryPhoneProvider').show();	        
	        $('#PrimaryPhoneProvider').val($('#PrimaryPhoneProvider option:selected').text());
	         $('#phoneProvider').show();
			//$('#ReceiveTextMessage').attr('disabled', false);
		}
		else {
			$('#PrimaryPhoneProvider').hide();
			$('#phoneProvider').hide();
			$('span[htmlFor="PrimaryPhoneProvider"]').hide();
			//$('#ReceiveTextMessage').attr("checked", false);
			//$('#ReceiveTextMessage').attr('disabled', true);
		}
	    if ($('#AlternativePhoneType').val() == "Cell") {
	        $('#AlternativePhoneProvider').val($('#AlternativePhoneProvider option:selected').text());
			$('#AlternativePhoneProvider').show();
			$('#alternatePhoneProvider').show();
		}
		else {
			$('#AlternativePhoneProvider').hide();
			$('#alternatePhoneProvider').hide();
			$('span[htmlFor="AlternativePhoneProvider"]').hide();
        }
        if ($('#PrimaryPhoneType').val() == "Cell" || $('#AlternativePhoneType').val() == "Cell")
            $('#ReceiveTextMessage').attr('disabled', false);
        else {
            $('#ReceiveTextMessage').attr("checked", false);
            $('#ReceiveTextMessage').attr('disabled', true);
                }
	});



	$('input[type="submit"]').click(function () {
		if ($('#PrimaryPhoneType').val() == "Cell") {
			if ($('#PrimaryPhoneProvider').val() == "") {
				// ValidationGroupString += "Please Select Cell Phone Provider.\n";
			}
		}
	});
});

function clearPinNumbers() {
	var pin = $("#Pin").val();
	var rePin = $("#ReEnter").val();
	if (pin != rePin) {
		$('#Pin').val("");
		$('#ReEnter').val("");
		$('#Pin').focus();
	}
}


$.validator.unobtrusive.adapters.addSingleVal("expirationdatewithfuturedate", "other");

jQuery.validator.addMethod("expirationdatewithfuturedate", function (value, element, other) {
	if ($(element)) {
		var valid = false;
		var myDates = new Array()
		var today;
		var enteredDate;
		if (value.split('/').length == 3) {
		    myDates[0] = value.split('/')[0]; //month
		    myDates[1] = value.split('/')[1]; //date
		    myDates[2] = value.split('/')[2]; //year

		    enteredDate = new Date(myDates[2], myDates[0] - 1, myDates[1]);
		    today = new Date();
		    today.setHours(0, 0, 0, 0);
		}
		else if (value.split('/').length == 2) {
		    myDates[0] = value.split('/')[0]; //month
		    myDates[1] = value.split('/')[1]; //year
		    enteredDate = new Date(myDates[1], myDates[0], 0);
            
		    today = new Date();
		    today.setHours(0, 0, 0, 0);
		    today.setDate(enteredDate.getDate());
		}
		else {
		    valid = true;
		    return valid;
		}
		if (enteredDate < today)
			valid = false;
		else
			valid = true;
		return valid;
	}
	return true;
});

///AL-545 idexpirationdatevalidation
$.validator.unobtrusive.adapters.addSingleVal("idexpirationdatevalidation", "other");

jQuery.validator.addMethod("idexpirationdatevalidation", function (value, element, other) {
	if ($(element)) {
		var valid = false;
		var myDates = new Array()
		var today;
		var enteredDate;
		if (value.split('/').length == 3) {
			myDates[0] = value.split('/')[0]; //month
			myDates[1] = value.split('/')[1]; //date
			myDates[2] = value.split('/')[2]; //year
			enteredDate = new Date(myDates[2], myDates[0] - 1, myDates[1]);
			today = new Date();
			today = new Date((today.getFullYear() + 50), today.getMonth(), today.getDate());
		}
		else {
			valid = true;
			return valid;
		}
		if (enteredDate > today)
			valid = false;
		else
			valid = true;
		return valid;
	}
	return true;
});

//MyConditionalRequiredCustomValidation Development.
jQuery.validator.unobtrusive.adapters.add
    ("phoneprovidervalidation", ['propertyone', 'propertytwo', 'phonetype'],
    function (options) {
    	options.rules['phoneprovidervalidation'] = { other: options.params.other,
    		propertyone: options.params.propertyone,
    		propertytwo: options.params.propertytwo,
    		phonetype: options.params.phonetype
    	};
    	options.messages['phoneprovidervalidation'] = options.message;
    }
);

jQuery.validator.addMethod("phoneprovidervalidation", function (value, element, params) {
	if ($('#AlternativePhoneType')) {
		var propertyonevalue = $('#AlternativePhoneType').val();
	}
	if ($('#AlternativePhoneProvider')) {
		var propertytwovalue = $('#AlternativePhoneProvider').val();
	}
	if (params.phonetype == False) {

		if ((propertyonevalue.toLowerCase() == 'cell' && propertytwovalue.toLowerCase() == '')) {
			return false;
		}
	}
	return true;
});

//Validation for load to card
jQuery.validator.unobtrusive.adapters.add
    ("minimumloadamount", ['actualloadamount'],
    function (options) {
    	options.rules['minimumloadamount'] = { other: options.params.other,
    		phonetype: options.params.actualloadamount
    	};
    	options.messages['minimumloadamount'] = options.message;
    	$.validator.messages.minimumloadamount = options.message;
    }
);
    jQuery.validator.addMethod("minimumloadamount", function (value, element, params) {
    	var minimumload = parseFloat($('#MinimumLoadAmount').val());
    	var loadAmt = parseFloat(value);
    	var activecard = $('#IsCardActivationTrx').val();

    	if (activecard.toLocaleLowerCase() == 'true' && (minimumload <= value || loadAmt == 0)) {    	  
    		return true;
    	}
    	else if (activecard.toLocaleLowerCase() != 'true' && (minimumload <= loadAmt || loadAmt == 0)) {
    		return true;
    	}
    	else
    	{    	   
    	    hideSpinners($("#spinnerWithNoMsg"));
    	    hideSpinner();    	   
    		return false;
    	}
    });  
   

	//Validation for BankId and  BranchID
    jQuery.validator.unobtrusive.adapters.add
		("channelpartner", ['channelpartner', 'channelpartnerpropvalue'],
		function (options) {
    		options.rules['channelpartner'] = { other: options.params.other,
    			channelpartner: options.params.channelpartner,
    			channelpartnerpropvalue: options.params.channelpartnerpropvalue
    		};
    		options.messages['channelpartner'] = options.message;
		}
	);

    jQuery.validator.addMethod("channelpartner", function (value, element, params) {
    	var channelPartner = params.channelpartner.toString();
    	var channelPartnerPropValue = params.channelpartnerpropvalue.toString();

    	var id = $.trim(value.toString());
    	if ((channelPartner.toLowerCase() == channelPartnerPropValue.toLowerCase()) && (id == '' || id == NaN)) {
    		return false;
    	}
    	return true;
    });

    //Validation for Gender
    jQuery.validator.unobtrusive.adapters.add
		("requiredifnotchannelpartner", ['channelpartnername'],
		function (options) {
			options.rules['requiredifnotchannelpartner'] = {
				other: options.params.other,
				channelpartnername: options.params.channelpartnername,
			};
			options.messages['requiredifnotchannelpartner'] = options.message;
		}
	);
    jQuery.validator.addMethod("requiredifnotchannelpartner", function (value, element, params) {
    	var radioGroup = element.name;
    	var expression = "input[name ='" + radioGroup + "']:checked";
    	var currentchannelpartnername = params.channelpartnername.toString();
    	if ((currentchannelpartnername.toLowerCase() != "synovus")
			&& (!$(expression).val())) {
    		return false;
    	}
    	return true;
    });

//validation for Coupon/Promo/Alias code
jQuery.validator.unobtrusive.adapters.add
    ("couponpromoaliascode", ['couponpromoalias'],
    function (options) {
    	options.rules['couponpromoaliascode'] = { other: options.params.other,
    		propertyone: options.params.propertyone
    	};
    	options.messages['couponpromoaliascode'] = options.message;
    }
);

jQuery.validator.addMethod("couponpromoaliascode", function (value, element, params) {
	var CouponPromoCode = $('#CouponPromoCode').val();
	var regexItem = new RegExp("^[a-zA-Z0-9-]*$");
	var regex2 = new RegExp("^[a-zA-Z]*$");
	var regex3 = new RegExp("^[0-9]*$");
	var regex4 = new RegExp("^[a-zA-Z0-9]*$");
	if (CouponPromoCode == null || CouponPromoCode == "") {
		return true;
	}
	if (regexItem.test(CouponPromoCode) && (CouponPromoCode.length >= 3 && CouponPromoCode.length <= 20)) {
		var cpcode = CouponPromoCode.toString();
		//Coupon Promo Code
		if ((cpcode.indexOf("-") == 5) && cpcode.length == 15 && regex3.test(cpcode.substring(1, 4))) {
			if (regex2.test(cpcode.substring(0, 1))) {
				return true;
			}
			else {
				return false;
			}
		}
		//Coupon Code
		else if (regexItem.test(cpcode) && cpcode.length == 5) {

			if (regex2.test(cpcode.substring(0, 1)) && regex3.test(cpcode.substring(1, 4))) {
				return true;
			}
			else {
				return false;
			}
		}
		//Alias Code
		if ((cpcode.length >= 3 || cpcode.length <= 20) && regex4.test(cpcode) && regex2.test(cpcode.substring(0, 1))) {
			return true;
		}
		else {
			return false;
		}
	}
	else {
		return false;
	}
});





    //Validation for send money 
    jQuery.validator.unobtrusive.adapters.add
    ("sendmoneyamount", ['sendmoneyamount'],
    function (options) {
    	options.rules['sendmoneyamount'] = { other: options.params.other,
    		propertyone: options.params.propertyone,
    		propertytwo: options.params.propertytwo,
    		phonetype: options.params.phonetype
    	};
    	options.messages['sendmoneyamount'] = options.message;
    }
);

    jQuery.validator.addMethod("sendmoneyamount", function (value, element, params) {
    	var Amount1 = $('#Amount').val().toString();
    	var Amount2 = $('#DestinationAmount').val().toString();
    	if (((Amount1 == null) || (Amount1 == "") || (Amount1 == 0) || (Amount1 == NaN)) && ((Amount2 == null) || (Amount2 == "") || (Amount2 == 0) || (Amount2 == NaN))) {
    		return false;
    	}
    	return true;
    });


 //Validation for TestQuetionAnswer
 jQuery.validator.unobtrusive.adapters.add
    ("testquestion", ['testquestion'],
    function (options) {
        options.rules['testquestion'] = { other: options.params.other,
            testquestion: options.params.testquestion
        };
        options.messages['testquestion'] = options.message;
    }
);

 jQuery.validator.addMethod("testquestion", function (value, element, params) {
     var testquestionoption = $("#TestQuestionOption").val();
     var testQuestionOption = testquestionoption.toString();
     if ((testQuestionOption.toLowerCase() == 'y') && (value == '' || value == NaN)) {
         return false;
     }
     else {
         return true;
     }    
 });

  //Validation for HasPhotoIDTestQuestionAnswer
 jQuery.validator.unobtrusive.adapters.add
    ("photoidtestquestion", ['testquestion'],
    function (options) {
        options.rules['photoidtestquestion'] = { other: options.params.other,
            testquestion: options.params.testquestion
        };
        options.messages['photoidtestquestion'] = options.message;
    }
);

 jQuery.validator.addMethod("photoidtestquestion", function (value, element, params) {
     var testquestionoption =$("input[name='IsReceiverHasPhotoId']:checked").val();   
	 var propertyval = $("#TestQuestion").val();
	 if (testquestionoption == 'False' && propertyval == '') {
         return false;
     }
     else {
         return true;
     }    
 });

$.validator.unobtrusive.adapters.addSingleVal("dropdownrequired", "other");

jQuery.validator.addMethod("dropdownrequired", function (value, element, other) {
    if ($(element)) {
        var valid = true;

        if (value != null && value != "")
            valid = true;
        else {
            if (document.getElementById(element.id).options.length > 1)
                valid = false;
            else
                valid = true;
        }

        return valid;
    }
    return true;
});