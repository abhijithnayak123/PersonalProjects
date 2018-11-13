// add tool tip for dropdown list as in IE 7 doesn't show lenghty values
$(document).ready(function () {
	$('option').each(function () {
		$(this).attr('title', $(this).val());
	});
});
