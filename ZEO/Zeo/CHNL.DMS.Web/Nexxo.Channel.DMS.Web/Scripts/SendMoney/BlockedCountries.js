$(document).ready(function () {
    PopulateBlockedUnblockedCountriesMutliSelect(blockedCountries, "multiselect");
    PopulateBlockedUnblockedCountriesMutliSelect(unblockedCountries, "multiselect_to");
    $("#multiselect").multiselect();
});

function PopulateBlockedUnblockedCountriesMutliSelect(countries, multiSelectId) {
    var options;
    $.each(countries, function (index, e) {
        options = '<option value="' + e.ISOCountryCode + '">' + e.CountryName + '</option>'
        $('#' + multiSelectId).append(options);
    });
};

function getBlockedCountries() {
    var blockedCountries = [];
    $("#multiselect_to option").each(function () {
        blockedCountries.push($(this).val());
    });
    return blockedCountries;
}

//saving only blocked countries - In DB, it will deleting all the records and inserting again.
function saveBlockedCountries() {
    var countries = getBlockedCountries();
    $.ajax({
        url: SaveBlockedCountriesUrl,
        data: { blockedCountries: countries },
        type: 'POST',
        datatype: 'json',
        traditional: true,
        success: function (jsonData) {
            if (jsonData && jsonData.success == false) {
                showExceptionPopupMsg(jsonData.data);
            }
            else {
                ShowPopUp(SuccessUrl, "Message", 300, 120);
            }
        },
        error: function (err) {
            showExceptionPopupMsg(err.data);
        }
    });
}