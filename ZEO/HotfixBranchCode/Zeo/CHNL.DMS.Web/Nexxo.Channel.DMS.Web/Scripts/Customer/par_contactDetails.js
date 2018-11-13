function showAlternatePhType(num)
{
    var altph = document.getElementById("altPhType");
    altph.style.display = "none";

    var alt = num.value;
    if (alt.length == 12 && alt != null && alt != "___-___-____") {
        var altph = document.getElementById("altPhType");
        altph.style.display = "block";        
        $('#AlternativePhoneType').focus();        
    }	
}

function showAltPhType()
{
	var altph = document.getElementById("altPhType");
	altph.style.display = "none";

	var alt = $("#AlternativePhone").val();
	if (alt.length == 12 && alt != null)
	{
		var altph = document.getElementById("altPhType");
		altph.style.display = "block";
	}
	else
	{
		var altph = document.getElementById("altPhType");
		altph.style.display = "none";
	}
}

$("#PrimaryPhoneType").change(function ()
{
	$("#PrimaryPhoneProvider").val('Select');
});
$("#AlternativePhoneType").change(function ()
{
	$("#AlternativePhoneProvider").val('Select');
});

function showMailingAddress(add)
{
    document.getElementById("mailingAddress").style.display = add.checked ? "block": "none";
}

      

