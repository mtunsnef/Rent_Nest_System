const maxImages = 20;
let uploadedImages = [];

const imageUpload = document.getElementById('imageUpload');
const preview = document.getElementById('preview');
const nextButton = document.getElementById('nextStepBtn2');
nextButton.disabled = true;

function showToast(message) {
    const toast = document.getElementById('myToast');
    toast.querySelector('.toast-body').textContent = message;
    toast.classList.remove('hide');
    toast.classList.add('showing');

    setTimeout(() => {
        toast.classList.remove('showing');
        toast.classList.add('show');
    }, 300);

    setTimeout(hideToast, 3000);
}

function hideToast() {
    const toast = document.getElementById('myToast');
    toast.classList.remove('show');
    toast.classList.add('hide');
}

function checkStep2Images() {
    nextButton.disabled = uploadedImages.length < 3;
}

imageUpload.addEventListener('change', function () {
    const files = Array.from(this.files).filter(file => file.type.startsWith('image/'));

    if (uploadedImages.length + files.length > maxImages) {
        showToast(`Bạn chỉ được upload tối đa ${maxImages} ảnh!`);
        return;
    }

    let duplicateFound = false;

    files.forEach((file) => {
        const isDuplicate = uploadedImages.some(img =>
            img.file?.name === file.name &&
            img.file?.size === file.size &&
            img.file?.lastModified === file.lastModified
        );

        if (isDuplicate) {
            duplicateFound = true;
            return;
        }

        const reader = new FileReader();
        reader.onload = function (e) {
            const imageData = {
                file,
                dataURL: e.target.result
            };

            uploadedImages.push(imageData);

            const div = document.createElement('div');
            div.className = 'position-relative mr-3 mb-2';
            div.style.width = '100px';
            div.innerHTML = `
            <img src="${imageData.dataURL}" class="img-thumbnail" style="width:100px; height:100px; object-fit:cover;">
            <div class="position-absolute bg-dark rounded-circle d-flex justify-content-center align-items-center"
                 style="width:24px; height:24px; cursor:pointer; top: 4px; right: 4px;">
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


    if (duplicateFound) {
        showToast('Một hoặc nhiều ảnh đã tồn tại, vui lòng chọn ảnh khác!');
    }
    this.value = '';
});

preview.addEventListener('click', function (e) {
    const closeBtn = e.target.closest('.position-absolute');
    if (closeBtn) {
        const index = Array.from(preview.children).indexOf(closeBtn.parentElement);
        uploadedImages.splice(index, 1); 
        closeBtn.parentElement.remove();
        checkStep2Images();
    }
});