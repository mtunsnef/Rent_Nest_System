document.getElementById('confirmAIOptionBtn').addEventListener('click', async function () {
    const btn = this;
    setLoadingState(btn, true);

    try {
        const data = collectAIFormData();
        console.log('Dữ liệu gửi lên:', data);

        const response = await fetch('/Posts/GeneratePostWithAI', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });

        if (!response.ok) {
            throw new Error(`Lỗi mạng hoặc server! Status: ${response.status}`);
        }

        const result = await response.json();
        handleAIResponse(result);

        const closeBtn = document.querySelector('#selectAIOptions .btn-close');
        if (closeBtn) closeBtn.click();
    } catch (err) {
        console.error('Có lỗi xảy ra:', err);
        showErrorMessage('Đã có lỗi xảy ra khi tạo bài viết. Vui lòng thử lại.');
    } finally {
        setLoadingState(btn, false);
    }
});

function collectAIFormData() {
    const form = document.getElementById('filterForm');
    const inputs = form.querySelectorAll('input, textarea');
    const selects = form.querySelectorAll('select');
    const data = {};

    inputs.forEach(input => {
        const { name, type, checked, value } = input;
        if (!name || name === "__RequestVerificationToken") return;

        if (['checkbox', 'radio'].includes(type)) {
            if (checked) data[name] = value;
        } else if (value) {
            data[name] = value;
        }
    });

    selects.forEach(select => {
        const name = select.name;
        const selectedOption = select.options[select.selectedIndex];

        if (!name || !selectedOption) return;

        const value = selectedOption.value;
        const displayName = selectedOption.dataset.name || selectedOption.text.trim();

        data[name] = value;
        data[`${name}_text`] = displayName;
    });

    const addressDisplay = document.getElementById('selectedAddressDisplay');
    const addressValue = addressDisplay ? addressDisplay.innerText.trim() : '';
    if (addressValue && addressValue !== 'Chọn địa chỉ') {
        data['address'] = addressValue;
    }

    const aiStyle = document.querySelector('input[name="aiStyleOption"]:checked');
    if (aiStyle) {
        data['aiStyle'] = aiStyle.value;
    }

    const amenityBtns = document.querySelectorAll('.amenity-btn.active');
    const selectedAmenities = [];
    const selectedAmenityNames = [];

    amenityBtns.forEach(btn => {
        const value = btn.dataset.value;
        const name = btn.dataset.name || btn.querySelector('span')?.innerText?.trim();

        if (value) selectedAmenities.push(value);
        if (name) selectedAmenityNames.push(name);
    });

    if (selectedAmenities.length > 0) {
        data['selectedAmenities'] = selectedAmenities.join(',');
        data['selectedAmenities_text'] = selectedAmenityNames.join(', ');
    }

    return data;
}


function setLoadingState(button, isLoading) {
    if (!button) return;

    if (isLoading) {
        button.dataset.originalContent = button.innerHTML;
        button.innerHTML = `
            <div class="d-flex align-items-center justify-content-center gap-2">
                <span>Đang tạo...</span>
                <div class="spinner-border spinner-border-sm text-light" role="status"></div>
            </div>
        `;
        button.disabled = true;
    } else {
        button.innerHTML = button.dataset.originalContent || 'Xác nhận';
        button.disabled = false;
    }
}

function handleAIResponse(data) {
    const titleInput = document.getElementById('titleInput');
    const contentInput = document.getElementById('contentInput');

    if (!data || !data.content) {
        showErrorMessage('Dữ liệu trả về không hợp lệ.');
        return;
    }

    const titleMatch = data.content.match(/Tiêu đề:(.*?)\n/);
    const contentMatch = data.content.match(/Nội dung:(.*)/s);

    if (titleMatch && contentMatch) {
        titleInput.value = titleMatch[1].trim();
        contentInput.value = contentMatch[1].trim();

        titleInput.dispatchEvent(new Event('input', { bubbles: true }));
        contentInput.dispatchEvent(new Event('input', { bubbles: true }));
        titleInput.dispatchEvent(new Event('blur', { bubbles: true }));
        contentInput.dispatchEvent(new Event('blur', { bubbles: true }));
    } else {
        showErrorMessage('Không thể phân tích dữ liệu từ AI.');
    }
}

function showErrorMessage(message) {
    alert(message);
}
