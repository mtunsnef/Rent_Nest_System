function setupAmenityButton({
    button,
    selectedList,
    hiddenInput,
    value
}) {
    const svg = button.querySelector('svg');

    button.addEventListener('click', () => {
        const isSelected = selectedList.includes(value);

        if (isSelected) {
            const index = selectedList.indexOf(value);
            if (index > -1) selectedList.splice(index, 1);
            button.classList.remove('btn-dark');
            button.classList.add('btn-outline-dark');
            if (svg) svg.setAttribute('fill', '#000000');
        } else {
            selectedList.push(value);
            button.classList.remove('btn-outline-dark');
            button.classList.add('btn-dark');
            if (svg) svg.setAttribute('fill', '#ffffff');
        }

        if (hiddenInput) hiddenInput.value = selectedList.join(',');
    });

    button.addEventListener('mouseenter', () => {
        const isSelected = selectedList.includes(value);
        if (!isSelected && svg) {
            svg.setAttribute('fill', '#ffffff');
        }
    });

    button.addEventListener('mouseleave', () => {
        const isSelected = selectedList.includes(value);
        if (!isSelected && svg) {
            svg.setAttribute('fill', '#000000');
        }
    });
}


//button for AI
function setupAIButton(button) {
    const svg = button.querySelector('svg');

    button.addEventListener('mouseenter', () => {
        if (svg) svg.setAttribute('fill', '#ffffff');
    });

    button.addEventListener('mouseleave', () => {
        if (svg) svg.setAttribute('fill', '#000000');
    });
}

const amenityButtons = document.querySelectorAll('.amenity-btn');
const hiddenInput = document.getElementById('selectedAmenities');
let selectedAmenities = [];

amenityButtons.forEach(button => {
    const value = button.getAttribute('data-value');
    setupAmenityButton({
        button,
        selectedList: selectedAmenities,
        hiddenInput: hiddenInput,
        value: value
    });
});

//button AI
const createWithAIButton = document.getElementById('createWithAI');
if (createWithAIButton) {
    setupAIButton(createWithAIButton);
}
