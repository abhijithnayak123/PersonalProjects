function ActOnMyBehalf() {
    $('#ActOnMyBehalf').val('0');
    $('#divOk').dialog('destroy').remove();

    if (customerHasGoldCard == "true") {
        RedirectToUrl(SendMoney_URL);
        //window.location.href = SendMoney_URL;
    } else {
        try {
            RedirectToUrl(SkipGoldCardEnrollment_URL + "?editgoldcardfrom=sendmoney");
            //window.location.href = SkipGoldCardEnrollment_URL + "?editgoldcardfrom=sendmoney";
        } catch (e) { }
    }
}