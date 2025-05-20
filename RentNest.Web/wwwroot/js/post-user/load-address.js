//load tinh quan huyen dn
const provinceSelect = document.getElementById('provinceSelect');
const districtSelect = document.getElementById('districtSelect');
const wardSelect = document.getElementById('wardSelect');
const streetInput = document.getElementById('streetInput');

const mapContainer = document.querySelector('#addressSelect iframe').parentElement;
const mapIframe = document.querySelector('#addressSelect iframe');

mapContainer.style.display = 'none';
wardSelect.disabled = true;
districtSelect.disabled = true;
streetInput.disabled = true;

function updateMap() {
    const provinceText = provinceSelect.options[provinceSelect.selectedIndex]?.text || '';
    const districtText = districtSelect.options[districtSelect.selectedIndex]?.text || '';
    const wardText = wardSelect.options[wardSelect.selectedIndex]?.text || '';
    const streetText = streetInput.value.trim();

    if (provinceSelect.value && districtSelect.value && wardSelect.value) {
        const fullAddress = `${streetText ? streetText + ', ' : ''}${wardText}, ${districtText}, ${provinceText}`;
        const mapUrl = `https://maps.google.com/maps?q=${encodeURIComponent(fullAddress)}&z=14&output=embed`;
        mapIframe.src = mapUrl;
        mapContainer.style.display = 'block';
    } else {
        mapContainer.style.display = 'none';
    }
}


function loadProvinces() {
    fetch('https://esgoo.net/api-tinhthanh/1/0.htm')
        .then(response => response.json())
        .then(data => {
            if (data.error === 0) {
                provinceSelect.innerHTML = '<option value="">-- Chọn Tỉnh/Thành phố --</option>';
                districtSelect.innerHTML = '<option value="">-- Chọn Quận/Huyện --</option>';
                wardSelect.innerHTML = '<option value="">-- Chọn Phường/Xã --</option>';
                districtSelect.disabled = true;
                wardSelect.disabled = true;
                streetInput.disabled = true;

                data.data.forEach(province => {
                    const option = document.createElement('option');
                    option.value = province.id;
                    option.textContent = province.full_name;
                    provinceSelect.appendChild(option);
                });
            }
        })
        .catch(error => console.error('Lỗi load tỉnh/thành:', error));
}

function loadDistricts(provinceId) {
    fetch(`https://esgoo.net/api-tinhthanh/2/${provinceId}.htm`)
        .then(response => response.json())
        .then(data => {
            if (data.error === 0) {
                districtSelect.innerHTML = '<option value="">-- Chọn Quận/Huyện --</option>';
                wardSelect.innerHTML = '<option value="">-- Chọn Phường/Xã --</option>';
                districtSelect.disabled = false;
                wardSelect.disabled = true;
                streetInput.disabled = true;
                streetInput.value = '';

                data.data.forEach(district => {
                    const option = document.createElement('option');
                    option.value = district.id;
                    option.textContent = district.full_name;
                    districtSelect.appendChild(option);
                });
            }
        })
        .catch(error => console.error('Lỗi load quận/huyện:', error));
}


function loadWards(districtId) {
    fetch(`https://esgoo.net/api-tinhthanh/3/${districtId}.htm`)
        .then(response => response.json())
        .then(data => {
            if (data.error === 0) {
                wardSelect.innerHTML = '<option value="">-- Chọn Phường/Xã --</option>';
                wardSelect.disabled = false;
                streetInput.disabled = true;
                streetInput.value = '';

                data.data.forEach(ward => {
                    const option = document.createElement('option');
                    option.value = ward.id;
                    option.textContent = ward.full_name;
                    wardSelect.appendChild(option);
                });
            }
        })
        .catch(error => console.error('Lỗi load phường/xã:', error));
}


provinceSelect.addEventListener('change', function () {
    const provinceId = provinceSelect.value;
    if (provinceId) {
        loadDistricts(provinceId);
    } else {
        districtSelect.innerHTML = '<option value="">-- Chọn Quận/Huyện --</option>';
        wardSelect.innerHTML = '<option value="">-- Chọn Phường/Xã --</option>';
        districtSelect.disabled = true;
        wardSelect.disabled = true;
        streetInput.disabled = true;
    }
    updateMap();
});

districtSelect.addEventListener('change', function () {
    const districtId = districtSelect.value;
    if (districtId) {
        loadWards(districtId);
    } else {
        wardSelect.innerHTML = '<option value="">-- Chọn Phường/Xã --</option>';
        wardSelect.disabled = true;
        streetInput.disabled = true;
    }
    updateMap();
});

wardSelect.addEventListener('change', function () {
    if (wardSelect.value) {
        streetInput.disabled = false;
    } else {
        streetInput.value = '';
        streetInput.disabled = true;
    }
    updateMap();
});

streetInput.addEventListener('input', updateMap);
loadProvinces();
