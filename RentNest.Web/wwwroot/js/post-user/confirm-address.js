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

    //confirmBtn.addEventListener('click', function () {
    //    const provinceText = provinceSelect.options[provinceSelect.selectedIndex].text;
    //    const districtText = districtSelect.options[districtSelect.selectedIndex].text;
    //    const wardText = wardSelect.options[wardSelect.selectedIndex].text;
    //    const streetText = streetInput.value.trim();

    //    const fullAddress = `${streetText ? streetText + ', ' : ''}${wardText}, ${districtText}, ${provinceText}`;
    //    selectedAddressDisplay.textContent = fullAddress;

    //    const apiKey = 'AIzaSyCefU2k2yKpub6v3hON9PWj60scBhmg82o';
    //    const geocodeUrl = `https://maps.googleapis.com/maps/api/geocode/json?address=${encodeURIComponent(fullAddress)}&key=${apiKey}`;

    //    fetch(geocodeUrl)
    //        .then(response => response.json())
    //        .then(data => {
    //            if (data.status === 'OK') {
    //                const location = data.results[0].geometry.location;
    //                const lat = location.lat;
    //                const lng = location.lng;

    //                const mapUrl = `https://www.google.com/maps?q=${lat},${lng}&z=15&output=embed`;
    //                mapPreviewIframe.src = mapUrl;
    //                mapPreviewContainer.style.display = 'block';
    //            } else {
    //                console.error('Không tìm thấy vị trí:', data.status);
    //            }
    //        })
    //        .catch(error => {
    //            console.error('Lỗi khi gọi Geocoding API:', error);
    //        });
    //});
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
