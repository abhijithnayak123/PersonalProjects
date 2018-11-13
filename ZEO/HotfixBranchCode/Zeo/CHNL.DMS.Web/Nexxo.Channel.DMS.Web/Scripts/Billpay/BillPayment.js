$(document).ready(function () {
	// Gallery

    if (jQuery("#gallery").length) {
        // Declare variables
        var totalImages = jQuery("#gallery > li").length,
            imageWidth = jQuery("#gallery > li:first").outerWidth(true),
			totalWidth = imageWidth * totalImages,
            visibleImages = Math.round(jQuery("#gallery-wrap").width() / imageWidth),
            visibleWidth = visibleImages * imageWidth,
            stopPosition = (visibleWidth - totalWidth);

        jQuery("#gallery").width(totalWidth);

        jQuery("#gallery-prev").click(function () {
            if (jQuery("#gallery").position().left < 0 && !jQuery("#gallery").is(":animated")) {
                jQuery("#gallery").animate({ left: "+=" + imageWidth + "px" });
            }
            return false;
        });

        jQuery("#gallery-next").click(function () {
            if (jQuery("#gallery").position().left > stopPosition && !jQuery("#gallery").is(":animated")) {
                jQuery("#gallery").animate({ left: "-=" + imageWidth + "px" });
            }
            return false;
        });
    }
});