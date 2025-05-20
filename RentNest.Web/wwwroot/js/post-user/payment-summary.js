function updatePreviewImage() {
    const previewImage = document.querySelector('#paymentSummary img');
    if (uploadedImages.length > 0) {
        previewImage.src = uploadedImages[0].dataURL;
    } else {
        previewImage.src = 'default.jpg';
    }
}
function updatePaymentSummary() {
    const title = document.getElementById("titleInput")?.value?.trim() || "Chưa có tiêu đề";
    const address = document.getElementById("selectedAddressDisplay")?.innerText?.trim() || "Chưa có địa chỉ";

    const timeUnitText = document.getElementById("timeUnitDropdown")?.selectedOptions[0]?.textContent || "Thời gian";
    const packageCard = document.querySelector('.package-type-card.border-primary.border-3');
    const checkedDuration = document.querySelector('input[name="duration"]:checked');
    const packageType = packageCard?.dataset?.name || "Chưa chọn gói";
    const unitName = checkedDuration?.dataset?.unitname?.toLowerCase() || "";
    const unitPrice = packageCard?.dataset?.unitprice || "0";
    const duration = checkedDuration?.dataset?.duration || "0";
    const totalPrice = checkedDuration?.dataset?.totalprice || "0";

    const startDate = document.getElementById("startDate")?.value || "Hôm nay";
    const schedule = document.getElementById("scheduleSelect")?.value || "Đăng ngay";

    const fullText = document.getElementById("endDateLabel")?.textContent || "";
    const endDate = fullText.replace("Ngày kết thúc dự kiến: ", "").trim();

    document.getElementById("postTitle").textContent = title;
    document.getElementById("postAddress").textContent = address;

    document.getElementById("summaryPackageType").textContent = packageType;
    document.getElementById("summaryPaymentType").textContent = timeUnitText;
    document.getElementById("summaryUnitPrice").textContent = `${unitPrice} đ / ${unitName}`;

    document.getElementById("summaryDuration").textContent = `${duration} ${unitName}`;
    document.getElementById("summaryStart").textContent = schedule;
    document.getElementById("summaryEnd").textContent = endDate;

    document.getElementById("summaryTotalPrice").textContent = `${totalPrice} đ`;
    document.getElementById("summaryTotalAmount").textContent = `${totalPrice} đ`;
}
document.addEventListener("DOMContentLoaded", function () {
    updatePreviewImage();

    const nextStepBtn = document.getElementById("nextStepBtn3");
    const paymentSummaryDiv = document.getElementById("paymentSummary");
    const step3Div = document.querySelector(".step-3");

    nextStepBtn.addEventListener("click", function () {
        window.scrollTo({ top: 110, behavior: 'smooth' });
        step3Div.classList.add("d-none");
        paymentSummaryDiv.classList.remove("d-none");

        updatePaymentSummary();
        updatePreviewImage();
    });
});
function backToEdit() {
    document.getElementById("paymentSummary").classList.add("d-none");
    document.querySelector(".step-3").classList.remove("d-none");
}

