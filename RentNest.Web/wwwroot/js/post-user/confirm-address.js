document.addEventListener("DOMContentLoaded", function () {
    const districtSelect = document.getElementById('districtSelect');
    const wardSelect = document.getElementById('wardSelect');
    const streetInput = document.getElementById('streetInput');
    const confirmBtn = document.getElementById('confirmAddressBtn');
    const selectedAddressDisplay = document.getElementById('selectedAddressDisplay');


    confirmBtn.disabled = true;

    function checkEnableConfirmButton() {
        if (districtSelect.value && wardSelect.value) {
            confirmBtn.disabled = false;
        } else {
            confirmBtn.disabled = true;
        }
    }

    districtSelect.addEventListener('change', checkEnableConfirmButton);
    wardSelect.addEventListener('change', checkEnableConfirmButton);

    confirmBtn.addEventListener('click', function () {
        const districtText = districtSelect.options[districtSelect.selectedIndex].text;
        const wardText = wardSelect.options[wardSelect.selectedIndex].text;
        const streetText = streetInput.value.trim();

        const fullAddress = `${streetText ? streetText + ', ' : ''}${wardText}, ${districtText}, Đà Nẵng`;
        selectedAddressDisplay.textContent = fullAddress;
    });
});
