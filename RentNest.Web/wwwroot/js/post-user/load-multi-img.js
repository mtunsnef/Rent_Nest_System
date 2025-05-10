//load anh0
function showToast(message) {
    const toast = document.getElementById('myToast');
    toast.querySelector('.toast-body').textContent = message;
    toast.classList.remove('hide');
    toast.classList.add('showing');

    setTimeout(() => {
        toast.classList.remove('showing');
        toast.classList.add('show');
    }, 300);

    setTimeout(() => {
        hideToast();
    }, 3000);
}

function hideToast() {
    const toast = document.getElementById('myToast');
    toast.classList.remove('show');
    toast.classList.add('hide');
}

function checkStep2Images() {
    const imageCount = preview.querySelectorAll('div.position-relative').length;
    const nextButton = document.querySelector('.step-2 button.btn-primary');

    if (imageCount >= 3) {
        nextButton.disabled = false;
    } else {
        nextButton.disabled = true;
    }
}


const imageUpload = document.getElementById('imageUpload');
const preview = document.getElementById('preview');
const maxImages = 20;
document.querySelector('.step-2 button.btn-primary').disabled = true;

imageUpload.addEventListener('change', function () {
    const currentImages = preview.querySelectorAll('div.position-relative').length;
    const newImages = Array.from(this.files).filter(file => file.type.startsWith('image/'));

    if (currentImages + newImages.length > maxImages) {
        showToast('Bạn chỉ được upload tối đa 20 ảnh!');
        this.value = '';
        return;
    }

    newImages.forEach((file, index) => {
        const reader = new FileReader();
        reader.onload = function (e) {
            const div = document.createElement('div');
            div.className = 'position-relative mr-3';
            div.style.width = '100px';

            div.innerHTML = `
                    <img src="${e.target.result}" class="img-thumbnail mb-2" style="width:100px; height:100px; object-fit:cover;">
                    <div class="position-absolute bg-dark rounded-circle d-flex justify-content-center align-items-center"
                         style="width:24px; height:24px; transform: translate(50%, -50%); cursor:pointer; top: 4px; right: 4px;">
                        <svg xmlns="http://www.w3.org/2000/svg" width="12" height="12" fill="white" viewBox="0 0 16 16">
                            <path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708z"/>
                        </svg>
                    </div>
                `;

            preview.appendChild(div);
            checkStep2Images();
        };
        reader.readAsDataURL(file);
    });

    this.value = '';
});

preview.addEventListener('click', function (e) {
    if (e.target.closest('.position-absolute')) {
        const btn = e.target.closest('.position-absolute');
        btn.parentElement.remove();
        checkStep2Images();
    }
});