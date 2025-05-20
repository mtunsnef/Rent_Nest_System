//step 
let currentStep = 1;
const stepLabels = [
    "Thông tin cơ bản",
    "Hình ảnh",
    "Chọn gói đăng và thanh toán"
];
function showStep(step) {
    document.querySelectorAll('.step').forEach((el, idx) => {
        el.classList.toggle('d-none', idx + 1 !== step);
    });

    document.getElementById('current-step').innerText = step;
    document.getElementById('step-label').innerText = stepLabels[step - 1];

    const progress = (step / stepLabels.length) * 100;
    document.getElementById('progress-bar').style.width = progress + '%';

    window.scrollTo({ top: 110, behavior: 'smooth' });
}

function nextStep() {
    if (currentStep < 3) {
        currentStep++;
        showStep(currentStep);
    }
}

function prevStep() {
    if (currentStep > 1) {
        currentStep--;
        showStep(currentStep);
    }
}

showStep(currentStep);