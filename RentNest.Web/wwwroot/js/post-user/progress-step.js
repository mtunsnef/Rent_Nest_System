//step 
let currentStep = 1;
function showStep(step) {
    document.querySelectorAll('.step').forEach((el, idx) => {
        el.classList.toggle('d-none', idx + 1 !== step);
    });
    document.getElementById('current-step').innerText = step;
    const progress = (step / 3) * 100;
    document.getElementById('progress-bar').style.width = progress + '%';

    window.scrollTo({ top: 0, behavior: 'smooth' });
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