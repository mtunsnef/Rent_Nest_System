class Province {
    constructor(id, name, code, typeText) {
        this.id = id;
        this.name = name;
        this.code = code;
        this.typeText = typeText
    }
}
class District {
    constructor(id, name, provinceId, type, typeText) {
        this.id = id;
        this.name = name;
        this.provinceId = provinceId;
        this.type = type;
        this.typeText = typeText;
    }
}
class Ward {
    constructor(id, name, provinceId, type, typeText) {
        this.id = id;
        this.name = name;
        this.provinceId = provinceId;
        this.type = type;
        this.typeText = typeText;
    }
}

async function fetchProvinces() {
    try {
        const response = await fetch('https://open.oapi.vn/location/provinces?page=0&size=63');
        const data = await response.json();

        const dropdown = document.getElementById('provinceDropdown');
        const provinceInput = document.getElementById('provinceName');

        dropdown.innerHTML = '<option value="">Chọn tỉnh/thành phố...</option>';

        data.data.forEach(item => {
            const province = new Province(item.id, item.name, item.code, item.typeText);
            const option = document.createElement('option');
            option.value = province.id;
            option.textContent = province.typeText + ' ' + province.name;
            dropdown.appendChild(option);
        });

        dropdown.addEventListener('change', function () {
            const selectedProvinceId = dropdown.value;
            const selectedProvinceText = dropdown.options[dropdown.selectedIndex].text;
            provinceInput.value = selectedProvinceText;

            if (selectedProvinceId) {
                fetchDistrict(selectedProvinceId);
            }
        });
    } catch (error) {
        console.error('Lỗi khi lấy dữ liệu từ API:', error);
    }
}

async function fetchDistrict(id) {
    try {
        const response = await fetch(`https://open.oapi.vn/location/districts/${id}?page=0&size=63`);
        const data = await response.json();

        const dropdown = document.getElementById('districtDropdown');
        const districtInput = document.getElementById('districtName'); 

        dropdown.innerHTML = '<option value="">Chọn quận/huyện...</option>';

        data.data.forEach(item => {
            const district = new District(item.id, item.name, item.provinceId, item.type, item.typeText);
            const option = document.createElement('option');
            option.value = district.id;
            option.textContent = district.name;
            dropdown.appendChild(option);
        });

        dropdown.addEventListener('change', function () {
            const selectedDistrictId = dropdown.value;
            const selectedDistrictText = dropdown.options[dropdown.selectedIndex].text;
            districtInput.value = selectedDistrictText; 

            if (selectedDistrictId) {
                fetchWard(selectedDistrictId);
            }
        });
    } catch (error) {
        console.error("Lỗi khi lấy dữ liệu quận:", error);
    }
}

async function fetchWard(id) {
    try {
        const response = await fetch(`https://open.oapi.vn/location/wards/${id}?page=0&size=63`);
        const data = await response.json();

        const dropdown = document.getElementById('wardDropdown');
        const wardInput = document.getElementById('wardName'); 

        dropdown.innerHTML = '<option value="">Chọn phường/xã...</option>';

        data.data.forEach(item => {
            const ward = new District(item.id, item.name, item.provinceId, item.type, item.typeText);
            const option = document.createElement('option');
            option.value = ward.id;
            option.textContent = ward.name;
            dropdown.appendChild(option);
        });

        dropdown.addEventListener('change', function () {
            const selectedWardText = dropdown.options[dropdown.selectedIndex].text;
            wardInput.value = selectedWardText; 
        });
    } catch (error) {
        console.error("Lỗi khi lấy dữ liệu phường:", error);
    }
}

fetchProvinces();