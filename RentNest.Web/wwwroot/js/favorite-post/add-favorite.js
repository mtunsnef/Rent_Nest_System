$(document).ready(function () {
	$(".favorite-btn").each(function () {
		var postId = $(this).data("post-id");
		var heartIcon = $("#heart-" + postId);

		$.get("/Accommodations/IsFavorite", { postId: postId }, function (isFavorite) {
			if (isFavorite) {
				heartIcon.addClass("favorite-red");
			}
		});
	});

	$(document).on("click", ".favorite-btn", function () {
		var postId = $(this).data("post-id");
		var token = $('input[name="__RequestVerificationToken"]').val();
		var heartIcon = $("#heart-" + postId);
		var isAdding = !heartIcon.hasClass("favorite-red");

		$.ajax({
			url: isAdding ? '/Accommodations/AddToFavorite' : '/Accommodations/RemoveFromFavorite',
			type: 'POST',
			data: { postId: postId },
			headers: {
				'RequestVerificationToken': token
			},
			success: function () {
				heartIcon.toggleClass("favorite-red");
				showToast(isAdding ? "Đã thêm vào yêu thích!" : "Đã xóa khỏi yêu thích!");
			},

			error: function () {
				window.location.href = "/Auth/Login?returnUrl=" + encodeURIComponent(window.location.pathname);
			}
		});
	});
});