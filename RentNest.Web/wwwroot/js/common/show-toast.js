function showToast(message) {
	var toast = $('#custom-toast');
	toast.text(message).fadeIn(200);

	setTimeout(function () {
		toast.fadeOut(500);
	}, 3000);
}