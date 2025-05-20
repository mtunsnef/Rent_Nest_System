function getPackageStyle(packageTypeName) {
    let style = {
        borderClass: 'border-secondary',
        textClass: 'text-dark',
        badgeClass: 'bg-secondary',
        badgeText: packageTypeName
    };

    switch (packageTypeName) {
        case 'VIP Kim Cương':
            style.borderClass = 'border-danger';
            style.textClass = 'text-danger';
            style.badgeClass = 'bg-danger';
            break;
        case 'VIP Vàng':
            style.borderClass = 'border-warning';
            style.textClass = 'text-warning';
            style.badgeClass = 'bg-warning';
            break;
        case 'VIP Bạc':
            style.borderClass = 'border-info';
            style.textClass = 'text-info';
            style.badgeClass = 'bg-info';
            break;
    }

    return style;
}

$(document).ready(function () {
    let defaultTimeUnitId = 1;
    let defaultPackageTypeName = "Tin thường";
    let defaultDurationValue = 7;

    $('#timeUnitDropdown').on('change', function () {
        let timeUnitId = $(this).val();
        $.get(`/api/v1/package-types/${timeUnitId}`, function (data) {
            let html = '';

            data.forEach(pkg => {
                let unitPriceFormatted = pkg.unitPrice ? pkg.unitPrice.toLocaleString() : '';
                let style = getPackageStyle(pkg.packageTypeName);

                html += `
                <div class="card ${style.borderClass} p-3 m-2 package-type-card cursor"
                     data-unitprice=${unitPriceFormatted}
                     data-id="${pkg.packageTypeId}"
                     data-name="${pkg.packageTypeName}"
                     data-badgeclass="${style.badgeClass}"
                     style="width: 12rem; border-radius: 15px;">
                    <div class="font-weight-bold ${style.textClass}">${pkg.packageTypeName}</div>
                    <div class="badge ${style.badgeClass} mt-2" style="color: #fff !important">${pkg.description}</div>
                    <div class="font-weight-bold text-dark mt-2">${unitPriceFormatted} đ/${pkg.timeUnitName.toLowerCase()}</div>
                </div>`;
            });

            $('#packageTypeContainer').html(html);

            let defaultCard = $(`.package-type-card[data-name='${defaultPackageTypeName}']`);
            if (defaultCard.length > 0) {
                defaultCard.trigger('click');
            }
        });
    });


    $(document).on('click', '.package-type-card', function () {
        let packageTypeId = $(this).data('id');
        let packageTypeName = $(this).data('name');
        let badgeClass = $(this).data('badgeclass');
        let timeUnitId = $('#timeUnitDropdown').val();

        $('.package-type-card').removeClass('border-primary border-3');
        $(this).addClass('border-primary border-3');

        if (packageTypeName.toLowerCase() === 'tin thường') {
            $('#packageBadgeVIP').hide(); 
        } else {
            $('#packageBadgeVIP')
                .removeClass()
                .addClass(`badge position-absolute ${badgeClass}`)
                .text("VIP")
                .show(); 
        }

        $.get(`/api/v1/durations?timeUnitId=${timeUnitId}&packageTypeId=${packageTypeId}`, function (data) {
            let html = '';
            let defaultDuration = data.find(d => d.durationValue === defaultDurationValue) || data[0];

            data.forEach(d => {
                let isChecked = (d.durationValue === defaultDuration.durationValue) ? 'checked' : '';
                html += `
                <div class="duration-card card p-3 mr-3 cursor" style="border-radius: 20px;">
                    <div class="form-check">
                        <input class="form-check-input duration-radio" type="radio" name="duration" data-totalprice=${d.totalPrice} data-duration="${d.durationValue}" data-unitname=${d.timeUnitName} ${isChecked}>
                        <label class="cursor form-check-label">
                            ${d.durationValue} ${d.timeUnitName} <br><strong>${d.totalPrice.toLocaleString()} đ</strong>
                        </label>
                    </div>
                    <span class="discount-badge"></span>
                </div>`;
            });

            $('#durationContainer').html(html);
            let selected = $('#durationContainer input[name="duration"]:checked');
            selected.closest('.duration-card').addClass('border-1');
            updateAllDiscountBadges();
            selected.trigger('change');
        });
    });


    $(document).on('click', '.duration-card', function (e) {
        if ($(e.target).is('input[type=radio]')) return;
        let radio = $(this).find('input[type=radio]');
        radio.prop('checked', true).trigger('change');
    });

    $(document).on('change', 'input[name="duration"]', function () {
        $('.duration-card').removeClass('border-1');

        if (this.checked) {
            const card = $(this).closest('.duration-card');
            card.addClass('border-1');

            const priceLabel = card.find('label').html();
            const match = priceLabel.match(/<strong>([\d,.]+)\s?đ<\/strong>/i);

            if (match && match[1]) {
                const rawPrice = match[1].replaceAll('.', '').replaceAll(',', '');
                const price = parseInt(rawPrice);
                if (!isNaN(price)) {
                    $('#total-price').text(`${price.toLocaleString()} đ`);
                }
            }

            updateEndDateLabel();
        }
    });

    $('#timeUnitDropdown').trigger('change');
});


$(document).ready(function () {
    let today = new Date();
    let yyyy = today.getFullYear();
    let mm = String(today.getMonth() + 1).padStart(2, '0');
    let dd = String(today.getDate()).padStart(2, '0');
    let todayStr = `${yyyy}-${mm}-${dd}`;

    $('#startDate').val(todayStr);
    $('#startDate').attr('min', todayStr);

    function generateHourOptions(startHour = 0) {
        let options = '<option>Đăng ngay bây giờ</option>';
        for (let h = startHour; h < 24; h++) {
            let hourStr = h.toString().padStart(2, '0') + ':00';
            options += `<option value="${hourStr}">${hourStr}</option>`;
        }
        return options;
    }

    function isVipPackage(packageName) {
        return ['VIP Kim Cương', 'VIP Vàng', 'VIP Bạc'].includes(packageName);
    }

    function updateScheduleSelect(packageName) {
        if (isVipPackage(packageName)) {
            $('#scheduleSelect').prop('disabled', false);

            let selectedDateStr = $('#startDate').val();
            let selectedDate = new Date(selectedDateStr + 'T00:00:00'); 

            let now = new Date();

            if (selectedDate.toDateString() === now.toDateString()) {
                let startHour = now.getHours() + 5;
                if (startHour > 23) startHour = 23;
                $('#scheduleSelect').html(generateHourOptions(startHour));
            } else if (selectedDate > now) {
                $('#scheduleSelect').html(generateHourOptions(0));
            } else {
                $('#scheduleSelect').html(generateHourOptions(0));
            }
        } else {
            $('#scheduleSelect').prop('disabled', true);
            $('#scheduleSelect').html('<option>Đăng ngay bây giờ</option>');
        }
    }

    $(document).on('click', '.package-type-card', function () {
        let packageName = $(this).data('name');
        updateScheduleSelect(packageName);
    });

    $('#startDate').on('change', function () {
        let activePackageCard = $('.package-type-card.border-primary.border-3');
        if (activePackageCard.length > 0) {
            let packageName = activePackageCard.data('name');
            updateScheduleSelect(packageName);
        }

        updateEndDateLabel();
    });

    let defaultPackageCard = $(`.package-type-card[data-name="Tin thường"]`);
    if (defaultPackageCard.length) {
        defaultPackageCard.trigger('click');
    }
});

function calculateDiscount(duration, unit) {
    duration = parseInt(duration);
    unit = unit.toLowerCase();

    let durationInDays = duration;
    if (unit.includes("tuần")) {
        durationInDays = duration * 7;
    } else if (unit.includes("tháng")) {
        durationInDays = duration * 30;
    }
    if (durationInDays < 7) return 0;
    if (durationInDays === 10) return 5;
    if (durationInDays === 14 || durationInDays === 15) return 10;
    if (durationInDays === 21) return 12;
    if (durationInDays === 28 || durationInDays === 30) return 15;
    if (durationInDays === 60) return 20;  // 2 tháng
    if (durationInDays === 90) return 25;  // 3 tháng
    if (durationInDays === 120) return 30; // 4 tháng

    return 0;
}

function updateAllDiscountBadges() {
    $('.duration-card').each(function () {
        const radio = $(this).find('input.duration-radio');
        const badge = $(this).find('.discount-badge');

        if (radio.length && badge.length) {
            const duration = parseInt(radio.data('duration'));
            const unit = $(this).find('label').text(); 

            const discount = calculateDiscount(duration, unit);

            if (discount > 0) {
                badge.text(`-${discount}%`);
                badge.show();
            } else {
                badge.hide();
            }
        }
    });
}

function calculateEndDate(startDateStr, duration, unitName) {
    let startDate = new Date(startDateStr);
    if (isNaN(startDate)) return null;

    let durationInDays = duration;

    unitName = unitName.toLowerCase();
    if (unitName.includes("tuần")) {
        durationInDays = duration * 7;
    } else if (unitName.includes("tháng")) {
        durationInDays = duration * 30;
    }

    let endDate = new Date(startDate);
    endDate.setDate(startDate.getDate() + durationInDays);

    let dd = String(endDate.getDate()).padStart(2, '0');
    let mm = String(endDate.getMonth() + 1).padStart(2, '0');
    let yyyy = endDate.getFullYear();
    return `${dd}/${mm}/${yyyy}`;
}

function updateEndDateLabel() {
    let selectedDurationInput = $('input[name="duration"]:checked');
    let startDateStr = $('#startDate').val();

    if (!selectedDurationInput.length || !startDateStr) {
        $('#endDateLabel').text('');
        return;
    }

    let duration = selectedDurationInput.data('duration');
    let unitName = selectedDurationInput.data('unitname');

    let endDateStr = calculateEndDate(startDateStr, duration, unitName);
    if (endDateStr) {
        $('#endDateLabel').text(`Ngày kết thúc dự kiến: ${endDateStr}`);
    }
}


$(document).on('click', 'a[href="#"]', function (e) {
    e.preventDefault();

    if ($(this).text().includes('Khuyến mãi')) {
        $('#promotionModal').modal('show');
    }
});

$('#promotionSearchBtn').on('click', function () {
    let keyword = $('#promotionSearchInput').val().trim();

    /*
    $.get(`/api/v1/promotions?keyword=${keyword}`, function (data) {
        let html = '';
        data.forEach(p => {
            html += `<div class="p-2 border rounded mb-2">${p.name}</div>`;
        });
        $('#promotionList').html(html || '<p class="text-muted">Không tìm thấy khuyến mãi nào.</p>');
    });
    */

    // render giả
    $('#promotionList').html(`
            <div class="p-2 border rounded mb-2">Khuyến mãi 10% cho bài đăng trên 2 tuần</div>
            <div class="p-2 border rounded mb-2">Miễn phí 3 ngày đăng VIP Bạc</div>
        `);
});