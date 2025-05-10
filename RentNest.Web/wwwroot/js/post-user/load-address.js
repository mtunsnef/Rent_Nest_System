//load quan huyen dn
const districtSelect = document.getElementById('districtSelect');
const wardSelect = document.getElementById('wardSelect');
const streetInput = document.getElementById('streetInput');

const mapContainer = document.querySelector('#addressSelect iframe').parentElement;
const mapIframe = document.querySelector('#addressSelect iframe');

mapContainer.style.display = 'none';

wardSelect.disabled = true;
streetInput.disabled = true;

function updateMap() {
    const districtText = districtSelect.options[districtSelect.selectedIndex].text;
    const wardText = wardSelect.options[wardSelect.selectedIndex].text;
    const streetText = streetInput.value.trim();

    if (districtSelect.value && wardSelect.value) {
        let fullAddress = `${streetText ? streetText + ', ' : ''}${wardText}, ${districtText}, Đà Nẵng`;
        let mapUrl = `https://maps.google.com/maps?q=${encodeURIComponent(fullAddress)}&z=15&output=embed`;
        mapIframe.src = mapUrl;
        mapContainer.style.display = 'block';
    } else {
        mapContainer.style.display = 'none';
    }
}

function loadDistricts() {
    fetch('https://esgoo.net/api-tinhthanh/2/48.htm')
        .then(response => response.json())
        .then(data => {
            if (data.error === 0) {
                districtSelect.innerHTML = '<option value="">-- Chọn Quận/Huyện --</option>';
                wardSelect.innerHTML = '<option value="">-- Chọn Phường/Xã --</option>';
                wardSelect.disabled = true;
                streetInput.value = '';
                streetInput.disabled = true;

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

function loadWards() {
    const districtId = districtSelect.value;
    if (!districtId) {
        wardSelect.innerHTML = '<option value="">-- Chọn Phường/Xã --</option>';
        wardSelect.disabled = true;
        streetInput.value = '';
        streetInput.disabled = true;
        updateMap();
        return;
    }

    fetch(`https://esgoo.net/api-tinhthanh/3/${districtId}.htm`)
        .then(response => response.json())
        .then(data => {
            if (data.error === 0) {
                wardSelect.innerHTML = '<option value="">-- Chọn Phường/Xã --</option>';
                wardSelect.disabled = false;
                streetInput.value = '';
                streetInput.disabled = true;

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

districtSelect.addEventListener('change', function () {
    loadWards();
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
loadDistricts();
