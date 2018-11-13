
//Regular expression method
$.validator.addMethod('regex', function (value, element, param) {
	return (this.optional(element) != false) || value.match(typeof param == 'string' ? new RegExp(param) : param);
});
function formatedate(value, delimiter) {
	var DateElements = new Array();
	DateElements[0] = value.split(delimiter)[0]; //month
	DateElements[1] = value.split(delimiter)[1]; //date
	DateElements[2] = value.split(delimiter)[2]; //year

	var FormatedDate = new Date();

	FormatedDate.setMonth(DateElements[0] - 1);
	FormatedDate.setDate(DateElements[1]);
	FormatedDate.setFullYear(DateElements[2]);

	return FormatedDate;
}
$.validator.addMethod("dateformat", function (value, element, other) {
	if ($(element)) {
		if (value == null || value == "" || value == '__/__/____') {
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
jQuery.validator.addMethod("isfuturedate", function (value, element, other) {
	if (value == '' || value == '__/__/____')
		return true;
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

$.validator.addMethod('dategreaterthan', function (value, element, param) {
	var targetdatevalue = $('#' + param.dependingproperty).val();
	var valid = false;
	if (value == "" || value == '__/__/____' || targetdatevalue == "" || targetdatevalue == '__/__/____')
		valid = true;

	if ($(element)) {
		var entereddate = formatedate(value, '/');
		var targetdate = formatedate(targetdatevalue, '/');

		if (entereddate > targetdate)
			valid = true;
	}
	return valid;

});



//Regular expression validation expression
var regexAlphabetsOnly = /^[a-zA-Z\- ']*$/;
var regexCity = /^[a-zA-Z .-]*$/;
var regexEmail = /^(([^<>()[\]\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
var regexAddress = /^((?!.*(P|p)\.? ?(O|o)\.? ?(Box|bOx|boX|BOX|BOx|box))[-a-zA-Z\d .,/@!])*(#?)((?!.*(P|p)\.? ?(O|o)\.? ?(Box|bOx|boX|BOX|BOx|box))[-a-zA-Z\d .,/@!])*$/;
var regexMailingAddress = /^([-a-zA-Z\d .,/@!])*(#?)([-a-zA-Z\d .,/@!])*$/;
var regexZipCode = /^(\d)(?!\1{4}$)\d*$/;  //  /^\d{5}$/ ;
var regexReferenceNumber = /^[0-9]{8}$/;
var regexIDNumber = /^[a-zA-Z0-9-*_ ]*$/;
var regexProfession = /^[a-zA-Z0-9\-\_\ ]*$/;
var regexPin = /^\d{4}$/;
var regexNumbersOnly = /^([0-9])$/;
var regexDescription = /^[a-zA-Z\- ]*$/;
var regexNotes = /^[^\\\%&/\*\?\<>\|]{1}[^\\%&/\*\?\<>\|]{0,250}$/;
var regexAlphabetsAndNumericOnly = /^[a-zA-Z0-9\- ']*$/;

function PersonalInformationValidator() {

	//var channelPartnerName = $('#ChannelPartnerName').val().toUpperCase();
	//if(channelPartnerName != "CARVER" && channelPartnerName != "TCF")
	//{

	$("#SSN").rules("add", {
		required: true,
		ssnnumber: "ActualSSN",
		messages: {
			required: '_NexxoTranslate(SSNITINRequired)',
			ssnnumber: '_NexxoTranslate(IdentificationSSNITINRegularExpression)'
		}
	});
	//}

	$('#SSN').keypress(function (e) {
		ValidateKey(e, regexNumbersOnly);
	});

	$("#FirstName").rules("add", {
		required: true,
		maxlength: 20,
		regex: regexAlphabetsOnly,
		messages: {
			required: '_NexxoTranslate(PersonalFirstNameRequired)',
			maxlength: '_NexxoTranslate(PersonalFirstNameStringLength)',
			regex: '_NexxoTranslate(PersonalFirstNameRegularExpression)'
		}
	});

	$("#FirstName").attr("maxlength", 20);
	$('#FirstName').keypress(function (e) {
		ValidateKey(e, regexAlphabetsOnly);
	});
	//$("#FirstName").attr("title", "Test");

	$("#MiddleName").rules("add", {
		required: false,
		maxlength: 20,
		regex: regexAlphabetsOnly,
		messages: {
			maxlength: '_NexxoTranslate(PersonalMiddleNameStringLength)',
			regex: '_NexxoTranslate(PersonalMiddleNameRegularExpression)'
		}
	});
	$("#MiddleName").attr("maxlength", 20);
	$('#MiddleName').keypress(function (e) {
		ValidateKey(e, regexAlphabetsOnly);
	});

	$("#LastName").rules("add", {
		required: true,
		maxlength: 30,
		regex: regexAlphabetsOnly,
		messages: {
			required: '_NexxoTranslate(PersonalLastNameRequired)',
			maxlength: '_NexxoTranslate(PersonalLastNameStringLength)',
			regex: '_NexxoTranslate(PersonalLastNameRegularExpression)'
		}
	});
	$("#LastName").attr("maxlength", 30);
	$('#LastName').keypress(function (e) {
		ValidateKey(e, regexAlphabetsOnly);
	});

	$("#SecondLastName").rules("add", {
		maxlength: 30,
		regex: regexAlphabetsOnly,
		messages: {
			maxlength: '_NexxoTranslate(PersonalLastNameStringLength)',
			regex: '_NexxoTranslate(PersonalLastNameRegularExpression)'
		}
	});
	$("#SecondLastName").attr("maxlength", 30);
	$('#SecondLastName').keypress(function (e) {
		ValidateKey(e, regexAlphabetsOnly);
	});

	$('input[name="Gender"]').rules("add", {
		required: true,
		messages: {
			required: '_NexxoTranslate(PersonalGenderRequired)'
		}
	});

	$("#PrimaryPhone").rules("add", {
		required: true,
		minlength: 10,
		maxlength: 12,
		phonenumbersequence: "PrimaryPhone",
		messages: {
			required: '_NexxoTranslate(PersonalPrimaryPhoneRequired)',
			phonenumbersequence: '_NexxoTranslate(PersonalPrimaryPhoneRequired)'
		}
	});

	$('#PrimaryPhoneType').rules('add', {
		required: true,
		messages: {
			required: '_NexxoTranslate(PersonalPrimaryPhoneTypeDefault)'
		}
	});

	$('#PrimaryPhoneProvider').rules('add', {
		required: true,
		mycustomvalidation: ["", "PrimaryPhoneType", "Cell", true],
		messages: {
			required: '_NexxoTranslate(PersonalPrimaryPhoneProviderRequired)',
			mycustomvalidation: '_NexxoTranslate(PersonalPrimaryPhoneProviderDefault)'
		}
	});

	$("#AlternativePhone").rules("add", {
		phonenumbersequence: "AlternativePhone",
		messages: {
			phonenumbersequence: '_NexxoTranslate(PersonalAlternativePhoneRegularExpression)'
		}
	});

	$("#AlternativePhoneType").rules("add", {
		phonevalidation: ["", "AlternativePhone", ""],
		messages: {
			phonevalidation: '_NexxoTranslate(PersonalAlternativePhoneTypeDefault)'
		}
	});

	$("#AlternativePhoneProvider").rules("add", {
		required: true,
		mycustomvalidation: ["", "AlternativePhoneType", "Cell", true],
		messages: {
			required: '_NexxoTranslate(PersonalPrimaryPhoneProviderRequired)',
			mycustomvalidation: '_NexxoTranslate(PersonalPrimaryPhoneProviderDefault)'
		}
	});

	$("#Email").rules("add", {
		maxlength: 200,
		regex: regexEmail,
		messages: {
			maxlength: '_NexxoTranslate(PersonalEmailStringLength)',
			regex: '_NexxoTranslate(PersonalEmailRegularExpression)'
		}
	});

	$("#Address1").rules("add", {
		required: true,
		maxlength: 50,
		regex: regexAddress,
		messages: {
			required: '_NexxoTranslate(PersonalAddress1Required)',
			maxlength: '_NexxoTranslate(PersonalAddress1StringLength)',
			regex: '_NexxoTranslate(PersonalAddress1RegularExpression)'
		}
	});
	$("#Address1").attr("maxlength", 50);
	$('#Address1').keypress(function (e) {
		ValidateKey(e, regexAddress);
	});

	$("#Address2").rules("add", {
		maxlength: 50,
		regex: regexAddress,
		messages: {
			maxlength: '_NexxoTranslate(PersonalAddress2StringLength)',
			regex: '_NexxoTranslate(PersonalAddress2RegularExpression)'
		}
	});
	$("#Address2").attr("maxlength", 50);
	$('#Address2').keypress(function (e) {
		ValidateKey(e, regexAddress);
	});

	$("#City").rules("add", {
		required: true,
		maxlength: 50,
		regex: regexCity,
		messages: {
			required: '_NexxoTranslate(PersonalCityRequired)',
			maxlength: '_NexxoTranslate(PersonalCityStringLength)',
			regex: '_NexxoTranslate(PersonalCityRegularExpression)'
		}
	});
	$("#City").attr("maxlength", 50);
	$('#City').keypress(function (e) {
		ValidateKey(e, regexCity);
	});

	$('#State').rules('add', {
		required: true,
		messages: {
			required: '_NexxoTranslate(PersonalStateRequired)'
		}
	});

	$("#ZipCode").rules("add", {
		required: true,
		maxlength: 5,
		minlength: 5,
		regex: regexZipCode,
		messages: {
			required: '_NexxoTranslate(PersonalZipCodeRequired)',
			maxlength: '_NexxoTranslate(PersonalZipCodeStringLength)',
			minlength: '_NexxoTranslate(PersonalZipCodeStringLength)',
			regex: '_NexxoTranslate(PersonalZipCodeRegularExpression)'
		}
	});
	$("#ZipCode").attr("maxlength", 5);
	$('#ZipCode').keypress(function (e) {
		ValidateKey(e, regexZipCode);
	});

	if (IsMailingAddressEnable != "False") {
		$("#MailingAddress1").rules("add", {
			required: true,
			maxlength: 50,
			regex: regexMailingAddress,
			messages: {
				required: '_NexxoTranslate(MailingAddress1Required)',
				maxlength: '_NexxoTranslate(MailingAddress1StringLength)',
				regex: '_NexxoTranslate(MailingAddress1RegularExpression)'
			}
		});
		$("#MailingAddress1").attr("maxlength", 50);
		$('#MailingAddress1').keypress(function (e) {
			ValidateKey(e, regexMailingAddress);
		});

		$("#MailingAddress2").rules("add", {
			maxlength: 50,
			regex: regexMailingAddress,
			messages: {
				maxlength: '_NexxoTranslate(MailingAddress2StringLength)',
				regex: '_NexxoTranslate(MailingAddress2RegularExpression)'
			}
		});
		$("#MailingAddress2").attr("maxlength", 50);
		$('#MailingAddress2').keypress(function (e) {
			ValidateKey(e, regexMailingAddress);
		});

		$("#MailingCity").rules("add", {
			required: true,
			maxlength: 50,
			regex: regexCity,
			messages: {
				required: '_NexxoTranslate(MailingCityRequired)',
				maxlength: '_NexxoTranslate(MailingCityStringLength)',
				regex: '_NexxoTranslate(MailingCityRegularExpression)'
			}
		});
		$("#MailingCity").attr("maxlength", 50);
		$('#MailingCity').keypress(function (e) {
			ValidateKey(e, regexCity);
		});

		$('#MailingState').rules('add', {
			required: true,
			messages: {
				required: '_NexxoTranslate(MailingStateRequired)'
			}
		});

		$("#MailingZipCode").rules("add", {
			required: true,
			maxlength: 5,
			minlength: 5,
			regex: regexZipCode,
			messages: {
				required: '_NexxoTranslate(MailingZipCodeRegularExpression)',
				maxlength: '_NexxoTranslate(MailingZipCodeStringLength)',
				minlength: '_NexxoTranslate(MailingZipCodeStringLength)',
				regex: '_NexxoTranslate(MailingZipCodeRegularExpression)'
			}
		});
		$("#MailingZipCode").attr("maxlength", 5);
		$('#MailingZipCode').keypress(function (e) {
			ValidateKey(e, regexZipCode);
		});
	}

	$("#ReferralNumber").rules("add", {
		regex: regexReferenceNumber,
		messages: {
			regex: '_NexxoTranslate(PersonalReferralNumberRegularExpression)'
		}
	});
	$("#ReferralNumber").attr("maxlength", 8);
	$('#ReferralNumber').keypress(function (e) {
		ValidateKey(e, regexNumbersOnly);
	});
	if ($("#Notes").length > 0) {
		$("#Notes").rules("add", {
			regex: regexNotes,
			maxlength: 250,
			messages: {
				regex: '_NexxoTranslate(AlphaNumericValue)',
				maxlength: '_NexxoTranslate(NotesLength)'
			}
		});
		$('#Notes').keypress(function (e) {
			ValidateKey(e, regexNotes);
		});
	}
}

function IdentificationInformationValidator() {
	// Author : Abhijith
	// Description : As Alloy, I need a minimum age for customers to register in the system to be configurable.
	// User Story: AL-1626
	// Starts Here
	var minAge;
	var minimumAgeMessage;

	if ($("#customerMinimumAge") && $("#customerMinimumAge").val())
		minAge = $("#customerMinimumAge").val();

	if ($("#minimumAgeMessage") && $("#minimumAgeMessage").val())
		minimumAgeMessage = $("#minimumAgeMessage").val();


	$("#DateOfBirth").rules("add", {
		dateofbirthnofuturedate: "DateOfBirth",
		required: true,
		dateofbirthformat: "DateOfBirth",
		mydatecustomvalidation: { minvalue: minAge, maxvalue: 100 },
		messages: {
			dateofbirthnofuturedate: '_NexxoTranslate(IdentificationDateOfBirthDateTimeNotFuture)',
			required: '_NexxoTranslate(IdentificationDateOfBirthRequired)',
			dateofbirthformat: '_NexxoTranslate(IdentificationDateOfBirthDateTime)',
			mydatecustomvalidation: minimumAgeMessage
		}
	});
	// Ends Here

    $("#divUserSessionId").css("display", "");
	$("#MotherMaidenName").rules("add", {
		required: true,
		maxlength: 20,
		regex: regexAlphabetsOnly,
		messages: {
			required: '_NexxoTranslate(IdentificationMotherMaidenNameRequired)',
			maxlength: '_NexxoTranslate(IdentificationMotherMaidenNameStringLength)',
			regex: '_NexxoTranslate(IdentificationMotherMaidenNameRegularExpression)'
		}
	});

	$("#MotherMaidenName").attr("maxlength", 20);
	$('#MotherMaidenName').keypress(function (e) {
		ValidateKey(e, regexAlphabetsOnly);
	});

	$('#CountryOfBirth').rules('add', {
		required: true,
		messages: {
			required: '_NexxoTranslate(IdentificationCountryOfBirthRequired)'
		}
	});

	$('#Country').rules('add', {
		required: true,
		messages: {
			required: '_NexxoTranslate(IdentificationCountryRequired)'
		}
	});

	$('#GovtIDType').rules('add', {
		required: true,
		messages: {
			required: '_NexxoTranslate(IdentificationGovtIDTypeRequired)'
		}
	});
	$('#GovtIdIssueState').rules('add', {
		myconditionalrequiredcustomvalidation: {
			propertyone: "Country",
			propertytwo: "GovtIDType"
		},
		messages: {
			myconditionalrequiredcustomvalidation: '_NexxoTranslate(IdentificationGovtIdIssueStateConditionalRequired)'
		}
	});
	$("#GovernmentId").rules("add", {
		required: true,
		maxlength: 20,
		regex: regexIDNumber,
		messages: {
			required: '_NexxoTranslate(IdentificationIDNumberRequired)',
			maxlength: '_NexxoTranslate(IdentificationIDNumberStringLength)',
			regex: '_NexxoTranslate(IdentificationIDNumberRegularExpression)'
		}
	});
	$("#GovernmentId").attr("maxlength", 20);
	$('#GovernmentId').keypress(function (e) {
		ValidateKey(e, regexIDNumber);
	});

    $("#IDIssuedDate").rules("add", {
        dateformat: "IDIssuedDate",
        isfuturedate: "IDIssuedDate",
        dategreaterthan:{dependingproperty:"DateOfBirth"},
        myconditionalrequiredcustomvalidation:{ 
    		propertyone: "GovtIDType",
    		propertytwo: "IssueDateCondition"
    	},
        messages: {
            dateformat:'_NexxoTranslate(IdentificationIDIssuedDateDateTime)',
            isfuturedate: '_NexxoTranslate(IdentificationIDIssuedDateDateTimeNotFuture)',
            dategreaterthan:'_NexxoTranslate(IdentificationIDIssuedDateCannotDob)',
            myconditionalrequiredcustomvalidation:'_NexxoTranslate(IdentificationIssuedDate)'
        }
    });
     $("#IDExpireDate").rules("add", {
        dateformat:"IDExpireDate",
        expirationdatewithfuturedate: "",
        idexpirationdatevalidation: "IDExpireDate",
        messages: {
            dateformat: '_NexxoTranslate(IdentificationIDExpireDateDateTime)',
            expirationdatewithfuturedate: '_NexxoTranslate(IdentificationIDExpireDateDateTimeWithFuture)',
            idexpirationdatevalidation: '_NexxoTranslate(IdentificationIDExpireDateValidation)'
        }
    });
    
}
function EmployeeDetailsValidator() {
    $("#divUserSessionId").css("display", "");
	$("#Profession").rules("add", {
		maxlength: 100,
		regex: regexProfession,
		messages: {
			maxlength: '_NexxoTranslate(EmployeeProfessionMaxlength)',
			regex: '_NexxoTranslate(EmployeeProfessionRequired)'
		}
	});
	$("#Profession").attr("maxlength", 100);
	$('#Profession').keypress(function (e) {
		ValidateKey(e, regexProfession);
	});

	//$("#EmployerName").rules("add", {
	//    maxlength: 100,
	//    regex: regexAlphabetsOnly,
	//    messages: {
	//        maxlength:'_NexxoTranslate(EmployeeNameMaxlength)',
	//        regex:'_NexxoTranslate(EmployeeNameRequired)'
	//    }
	//});
	//$("#EmployerName").attr("maxlength", 100);
	//$('#EmployerName').keypress(function (e) {
	//    ValidateKey(e, regexAlphabetsOnly);
	//});

	$("#EmployerPhoneNumber").rules("add", {
		phonenumbersequence: "EmployerPhoneNumber",
		messages: {
			phonenumbersequence: '_NexxoTranslate(EmployeePhoneNumberRequired)'
		}
	});

}
function PinDetailsValidator() {
    $("#divUserSessionId").css("display", "");
	$("#Pin").rules("add", {
		required: true,
		regex: regexPin,
		messages: {
			required: '_NexxoTranslate(PINNumberRequired)',
			regex: '_NexxoTranslate(PinPinRegularExpression)'
		}
	});
	$("#ReEnter").rules("add", {
		required: true,
		regex: regexPin,
		equalTo: "#Pin",
		messages: {
			required: '_NexxoTranslate(PinReEnterRequired)',
			regex: '_NexxoTranslate(PinReEnterRegularExpression)',
			equalTo: '_NexxoTranslate(PinReEnterCompare)'
		}
	});
}

function ProfileSummaryValidator() {
    $("#divUserSessionId").css("display", "");
	var channelPartnerName = $('#ChannelPartnerName').val().toUpperCase();
    if(channelPartnerName != "CARVER" && channelPartnerName != "TCF")
{
		$("#SSN").rules("add", {
			required: true
		});
	}

	$("#Name").rules("add", {
		required: true
	});

	$("#Gender").rules("add", {
		required: true
	});

	$("#PrimaryPhone").rules("add", {
		required: true
	});

	$("#Address").rules("add", {
		required: true
	});

	$("#MotherMaidenName").rules("add", {
		required: true
	});

	$("#DateOfBirth").rules("add", {
		required: true
	});

	$("#Country").rules("add", {
		required: true
	});

	$("#CountryOfBirth").rules("add", {
		required: true
	});

	$("#GovtIDType").rules("add", {
		required: true
	});

	$('#GovtIdIssueState').rules('add', {
		myconditionalrequiredcustomvalidation: {
			propertyone: "Country",
			propertytwo: "GovtIDType"
		}
	});

	$("#GovernmentId").rules("add", {
		required: true
	});

	$("#Pin").rules("add", {
		required: true
	});
}

function LoadMandatorySymbol(formId) {
	$('#' + formId + ' input[type=text],#' + formId + ' input[type=radio],#' + formId + ' select,#' + formId + ' input[type=password]').each(
        function () {
        	var element = $(this);
        	var id = element.prop('id');
        	var name = element.prop('name');
        	var type = element.prop('type');
        	var rule = $('#' + id).rules();
        	for (var prop in rule) {
        		if (rule.hasOwnProperty(prop)) {
        			if (prop == 'required' && rule[prop] == true) {
        				if (type == 'radio') {
        					var radiolabletext = $('label[for=' + name + ']').text().toString();
        					var radiolablelength = radiolabletext.length;
        					if (radiolabletext.charAt(radiolablelength - 1) != '*') {
        						$('label[for=' + name + ']').append('<span class=\'requiredfield-label\'>*</span>');
        					}
        				} else {
        					$('label[for=' + id + ']').append('<span class=\'requiredfield-label\'>*</span>');
        				}
        				break;
        			}
        		}
        	}
        }
    );
}

function ValidateKey(e, regex) {
	var enterKey = String.fromCharCode(!e.charCode ? e.which : e.charCode);
	if (regex.test(enterKey)) {
		return true;
	}
	e.preventDefault();
	return false;
}
