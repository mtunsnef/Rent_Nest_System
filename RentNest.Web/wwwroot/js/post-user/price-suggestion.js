//price suggesstion
function parseFormattedNumber(str) {
    return parseFloat(str.replace(/\./g, '').replace(/[^0-9]/g, ''));
}

function formatInputNumber(num) {
    return num.toLocaleString('vi-VN');
}

document.querySelectorAll('.price-input').forEach(input => {
    const key = input.dataset.target;
    const suggestEnabled = input.dataset.suggest === "true";
    const suggestionBox = document.querySelector(`[data-suggest="${key}"]`);
    const summaryBox = document.querySelector(`[data-summary="${key}"]`);

    input.addEventListener('input', function () {
        let raw = parseFormattedNumber(input.value);
        if (!raw) {
            if (suggestionBox) suggestionBox.innerHTML = '';
            if (summaryBox) summaryBox.innerHTML = '';
            return;
        }

        input.value = formatInputNumber(raw);

        if (suggestEnabled && suggestionBox) {
            suggestionBox.innerHTML = '';
            suggestionBox.classList.add('show');

            const multipliers = [100, 1000, 10000];
            multipliers.forEach(mul => {
                const suggestion = raw * mul;
                const label = formatCurrencyLabel(suggestion);

                const button = document.createElement('button');
                button.type = 'button';
                button.className = 'btn btn-outline-secondary btn-sm rounded-pill me-2 mb-1 mr-3 mt-2';
                button.textContent = label;

                button.addEventListener('click', () => {
                    input.value = formatInputNumber(suggestion);
                    if (summaryBox)
                        summaryBox.innerHTML = `<small>Tổng trị giá <strong>${formatCurrencyLabel(suggestion)}</strong></small>`;
                    suggestionBox.classList.remove('show');
                });

                suggestionBox.appendChild(button);
            });
        }

        if (summaryBox)
            summaryBox.innerHTML = `<small>Tổng trị giá <strong>${formatCurrencyLabel(raw)}</strong></small>`;
    });

    input.addEventListener('focus', () => {
        if (suggestEnabled && suggestionBox && input.value.trim() !== '') {
            suggestionBox.classList.add('show');
        }
    });

    input.addEventListener('keypress', function (e) {
        if (!/\d/.test(e.key)) {
            e.preventDefault();
        }
    });
    document.addEventListener('click', (e) => {
        if (suggestEnabled && suggestionBox &&
            !input.contains(e.target) && !suggestionBox.contains(e.target)) {
            suggestionBox.classList.remove('show');
        }
    });
});

function formatCurrencyLabel(value) {
    if (value >= 1_000_000_000) {
        return `${(value / 1_000_000_000).toLocaleString('vi-VN')} tỷ / tháng`;
    } else if (value >= 1_000_000) {
        return `${(value / 1_000_000).toLocaleString('vi-VN')} triệu / tháng`;
    } else if (value >= 1_000) {
        return `${(value / 1_000).toLocaleString('vi-VN')} nghìn / tháng`;
    } else {
        return `${value.toLocaleString('vi-VN')} đ / tháng`;
    }
}