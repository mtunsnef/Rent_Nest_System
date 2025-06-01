$(document).ready(function () {
    $("form").on("submit", function (e) {
        const min = parseInt($("#minMoney").val(), 10);
        const max = parseInt($("#maxMoney").val(), 10);

        if (!isNaN(min) && !isNaN(max) && min > max) {
            e.preventDefault();
            showNotification("⚠️ Giá thấp nhất không được lớn hơn giá cao nhất.");
        }
    });
});
function showNotification(message) {
    // Xóa thông báo cũ nếu có
    $("#notificationBox").remove();

    let notification = `
    <div id="notificationBox" style="
                position: fixed; right: 20px; top: 20px; background: white; padding: 15px;
                border-radius: 8px; box-shadow: 0 4px 6px rgba(0,0,0,0.1); max-width: 300px;
                display: flex; align-items: center; gap: 10px; z-index: 1000; opacity: 0;
                transform: translateY(-10px); transition: opacity 0.3s ease, transform 0.3s ease;">
        <img src="https://cdn-icons-png.flaticon.com/512/1828/1828665.png" alt="Notification" 
             style="width: 40px; height: 40px; border-radius: 5px;">
        <div style="color: black;">
            <strong>Thông báo</strong><br>
            <span>${message}</span>
        </div>
        <button id="closeNotification" style="
                    background: transparent; border: none; font-size: 16px; cursor: pointer;">
            ✖
        </button>
    </div>`;

    $("body").append(notification);

    setTimeout(() => {
        $("#notificationBox").css({ opacity: 1, transform: "translateY(0)" });
    }, 10);

    setTimeout(() => {
        $("#notificationBox").fadeOut(500, function () { $(this).remove(); });
    }, 3000);

    $(document).on("click", "#closeNotification", function () {
        $("#notificationBox").remove();
    });
}
