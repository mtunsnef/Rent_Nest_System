document.addEventListener("DOMContentLoaded", function () {
    const provinceSelect = document.getElementById('provinceSelect');
    const districtSelect = document.getElementById('districtSelect');
    const wardSelect = document.getElementById('wardSelect');
    const streetInput = document.getElementById('streetInput');
    const confirmBtn = document.getElementById('confirmAddressBtn');
    const selectedAddressDisplay = document.getElementById('selectedAddressDisplay');
    const mapPreviewIframe = document.getElementById('mapPreviewIframe');
    const mapPreviewContainer = document.getElementById('mapPreviewContainer');

    confirmBtn.disabled = true;

    function checkEnableConfirmButton() {
        if (provinceSelect.value && districtSelect.value && wardSelect.value) {
            confirmBtn.disabled = false;
        } else {
            confirmBtn.disabled = true;
        }
    }

    provinceSelect.addEventListener('change', checkEnableConfirmButton);
    districtSelect.addEventListener('change', checkEnableConfirmButton);
    wardSelect.addEventListener('change', checkEnableConfirmButton);

    confirmBtn.addEventListener('click', function () {
        const provinceText = provinceSelect.options[provinceSelect.selectedIndex].text;
        const districtText = districtSelect.options[districtSelect.selectedIndex].text;
        const wardText = wardSelect.options[wardSelect.selectedIndex].text;
        const streetText = streetInput.value.trim();

        const fullAddress = `${streetText ? streetText + ', ' : ''}${wardText}, ${districtText}, ${provinceText}`;
        selectedAddressDisplay.textContent = fullAddress;

        const mapUrl = `https://maps.google.com/maps?q=${encodeURIComponent(fullAddress)}&z=13&output=embed`;
        mapPreviewIframe.src = mapUrl;
        mapPreviewContainer.style.display = 'block';
    });

});
