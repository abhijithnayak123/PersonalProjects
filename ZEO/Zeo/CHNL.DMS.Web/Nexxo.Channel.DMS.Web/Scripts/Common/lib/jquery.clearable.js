jQuery.fn.clearable = function () {
    return this.each(function () {
        if (navigator.userAgent.indexOf("Edge") != -1) {
            clearLinkBrowser = "clearlinkEdge";
        }
        else {
            clearLinkBrowser = "clearlink";
        }
        var prop = $(this);
        $(this).css({ 'width': '100%', 'height': '100%', 'border-width': '0px', 'margin-top': '-4px', 'margin-left': '-11px', 'float': 'rigth', 'outline': 'none', 'padding': '4px 9px 4px 7px' })
          .wrap('<div id="sq" class="divclearable"></div>')
          .parent()
          .attr('class', $(this).attr('class') + ' divclearable')
          .append("<a class=" + clearLinkBrowser + " href='javascript:'></a>");

        $("." + clearLinkBrowser)
            .attr('title', 'Click to clear this textbox')
            .mousedown(function () {
                if (prop.attr('disabled') != 'disabled') {
                    $(this).prev().val('');
                }
            })
            .mouseup(function () {
                $(this).prev().focus();
            });
    });
}