$(document).ready(function () {
    $("#divUserSessionId").css("display", "");
    $('#TerminalName').keypress(function (event) {
        if (event.keyCode == 10 || event.keyCode == 13)
            event.preventDefault();
    });

    $('#Location').change(function () {
        var selectedLocationId = $(this).val();
        if (selectedLocationId != 0) {
            $.ajax({
                url: base_url_NpsTerminal,
                data: { locationId: selectedLocationId }, //parameters go here in object literal form 
                type: 'GET',
                datatype: 'json',
                success: function (NpsTerminals) {
                    if (handleException(NpsTerminals)) return;
                    npsterminalselect = $('#NpsTerminal');
                    npsterminalselect.empty();
                    var items = "<option value='0'>Select</option>"; //'<option>Select</option>';
                    $.each(NpsTerminals, function (i, state) {
                        items += '<option value="' + state.Value + '">' + state.Text + '</option>';
                    });
                    $('#NpsTerminal').html(items);
                },
                error: function () { showExceptionPopupMsg('Error processing while retreiving locations'); }
            });           
        } else {
            var items = "<option value='0'>Select</option>";
            $('#NpsTerminal').html(items);
        }
        $("#map").attr('disabled', 'disabled').css('cursor', 'default').css('background-color', '#D7DBD6');
        $("#divSetupMessage").show();
    });


    $("#TerminalName").blur(disableSetupButton());
    $("#NpsTerminal").change(function () {
        disableSetupButton()
    });

});

function disableSetupButton() {
    var terminalName = $("#TerminalName").val();
    var location = $("#Location").val();
    var nps = $("#NpsTerminal").val();
    if (terminalName !=0 && location !=0 && nps != 0) {
        $("#map").removeAttr('disabled').css('cursor', 'pointer').css('background-color', '#006937');
        $("#divSetupMessage").hide();
    }
    else {
        $("#map").attr('disabled', 'disabled').css('cursor', 'default').css('background-color', '#D7DBD6');
        $("#divSetupMessage").show();
    }
}


$('#map').click(function (e) {
    var selectedNpsTerminal = $("#NpsTerminal option:selected").text();
    var baseURL = "https://nps.nexxofinancial.com:18732/Peripheral/";
    var restFulServiceUrl = baseURL + "SetRedirectHost?redirectHost=" + selectedNpsTerminal;
    $.ajax({
        data: "{}",
        dataType: "jsonp",
        type: "POST",
        url: restFulServiceUrl,
        contentType: "application/json; charset=UTF-8",
        processData: true,
        timeout: 5000,
        success: function (data) {
            if (data != null) {
                $('#terminalSetup').submit();
            }
        },
        error: function (data, errmessage, errdetails) {
            showExceptionPopupMsg(errmessage + errdetails)
            e.preventDefault();
        }
    });
});
