const amenityButtons = document.querySelectorAll('.amenity-btn');
const hiddenInput = document.getElementById('selectedAmenities');

// Thiết lập sự kiện cho từng nút tiện ích
function setupAmenityButton({ button, hiddenInput, value }) {
    const svg = button.querySelector('svg');
    const name = button.dataset.name || button.querySelector('span')?.innerText?.trim();

    button.addEventListener('click', () => {
        const isSelected = button.classList.contains('active');

        if (isSelected) {
            button.classList.remove('active', 'btn-dark');
            button.classList.add('btn-outline-dark');
            if (svg) svg.setAttribute('fill', '#000000');
        } else {
            button.classList.add('active', 'btn-dark');
            button.classList.remove('btn-outline-dark');
            if (svg) svg.setAttribute('fill', '#ffffff');
        }

        // Cập nhật giá trị hidden input sau mỗi lần click
        const activeBtns = document.querySelectorAll('.amenity-btn.active');
        const values = Array.from(activeBtns).map(btn => btn.dataset.value);
        hiddenInput.value = values.join(',');
    });

    button.addEventListener('mouseenter', () => {
        if (!button.classList.contains('active') && svg) {
            svg.setAttribute('fill', '#ffffff');
        }
    });

    button.addEventListener('mouseleave', () => {
        if (!button.classList.contains('active') && svg) {
            svg.setAttribute('fill', '#000000');
        }
    });
}

// Thiết lập nút AI
function setupAIButton(button) {
    const svg = button.querySelector('svg');

    button.addEventListener('mouseenter', () => {
        if (svg) svg.setAttribute('fill', '#ffffff');
    });

    button.addEventListener('mouseleave', () => {
        if (svg) svg.setAttribute('fill', '#000000');
    });
}

// Khởi tạo
amenityButtons.forEach(button => {
    const value = button.getAttribute('data-value');
    setupAmenityButton({
        button,
        hiddenInput: hiddenInput,
        value: value
    });
});

const createWithAIButton = document.getElementById('createWithAI');
if (createWithAIButton) {
    setupAIButton(createWithAIButton);
}
