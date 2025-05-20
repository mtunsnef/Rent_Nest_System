//valid
const requiredFields = document.querySelectorAll('.required-field');
const nextBtn = document.getElementById('nextStepBtn');
const titleInput = document.getElementById('titleInput');
const contentInput = document.getElementById('contentInput');
const titleCount = document.getElementById('titleCount');
const contentCount = document.getElementById('contentCount');

const TITLE_MIN = 30, TITLE_MAX = 99;
const CONTENT_MIN = 50, CONTENT_MAX = 5000;

function updateCharCount(input, count, max) {
    if (input.value.length > max) input.value = input.value.substring(0, max);
    const length = input.value.length;
    count.textContent = `${length}/${max}`;
    count.style.display = length > 0 ? 'inline' : 'none';
}

function showError(field, message) {
    const formGroup = field.closest('.form-group');
    if (!formGroup) return;
    const errorSpan = formGroup.querySelector('.error-message');
    if (errorSpan) {
        errorSpan.textContent = message;
        errorSpan.style.display = 'block';
    }
}

function hideError(field) {
    const formGroup = field.closest('.form-group');
    if (!formGroup) return;
    const errorSpan = formGroup.querySelector('.error-message');
    if (errorSpan) {
        errorSpan.style.display = 'none';
    }
}

function validateField(field) {
    let value = '';
    if (field.tagName === 'DIV') value = field.textContent.trim();
    else value = field.value.trim();

    let isValid = true;

    if (field === titleInput) {
        if (value.length < TITLE_MIN) {
            showError(field, `Tiêu đề tối thiểu ${TITLE_MIN} ký tự`);
            isValid = false;
        } else {
            hideError(field);
        }
    } else if (field === contentInput) {
        if (value.length < CONTENT_MIN) {
            showError(field, `Nội dung tối thiểu ${CONTENT_MIN} ký tự`);
            isValid = false;
        } else {
            hideError(field);
        }
    } else if (field.id === 'selectedAddressDisplay') {
        if (value === '' || value === 'Chọn địa chỉ') {
            showError(field, 'Địa chỉ không được để trống');
            isValid = false;
        } else {
            hideError(field);
        }
    } else if (field.tagName === 'SELECT') {
        if (value === '' || value === '-- Chọn loại chuyên mục --') {
            showError(field, 'Loại chuyên mục không được để trống');
            isValid = false;
        } else {
            hideError(field);
        }
    } else if (field.id === 'areaInput') {
        if (value === '') {
            showError(field, 'Diện tích không được để trống');
            isValid = false;
        } else {
            hideError(field);
        }
    } else if (field.id === 'priceInput') {
        if (value === '') {
            console.log(field);
            showError(field, 'Mức giá không được để trống');
            isValid = false;
        } else {
            hideError(field);
        }
    } else {
        if (value === '') {
            showError(field, 'Trường này không được để trống');
            isValid = false;
        } else {
            hideError(field);
        }
    }

    checkForm();
    return isValid;
}

function checkForm() {
    let allValid = true;
    requiredFields.forEach(field => {
        if (!isFieldValid(field)) allValid = false;
    });
    nextBtn.disabled = !allValid;
}

function isFieldValid(field) {
    let value = '';
    if (field.tagName === 'DIV') value = field.textContent.trim();
    else value = field.value.trim();

    if (field === titleInput) return value.length >= TITLE_MIN;
    if (field === contentInput) return value.length >= CONTENT_MIN;
    if (field.id === 'selectedAddressDisplay') return value !== '' && value !== 'Chọn địa chỉ';
    if (field.tagName === 'SELECT') return value !== '' && value !== '-- Chọn loại chuyên mục --';
    if (field.id === 'areaInput') return value !== '';
    if (field.id === 'priceInput') return value !== '';
    return value !== '';
}

requiredFields.forEach(field => {
    if (field.tagName === 'SELECT') {
        field.addEventListener('change', () => {
            validateField(field);
        });
        field.addEventListener('blur', () => validateField(field));
    }

    if (field.tagName === 'INPUT' || field.tagName === 'TEXTAREA') {
        field.addEventListener('input', () => {
            if (field === titleInput) updateCharCount(titleInput, titleCount, TITLE_MAX);
            if (field === contentInput) updateCharCount(contentInput, contentCount, CONTENT_MAX);
        });
        field.addEventListener('blur', () => {
            validateField(field);
        });
    }

    if (field.tagName === 'DIV') {
        field.addEventListener('DOMSubtreeModified', () => {
            validateField(field);
        });
        field.addEventListener('blur', () => {
            validateField(field);
        });
    }
});

nextBtn.addEventListener('click', () => {
    requiredFields.forEach(field => validateField(field));
});

