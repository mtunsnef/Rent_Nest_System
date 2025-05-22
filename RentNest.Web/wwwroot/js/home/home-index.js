document.addEventListener("DOMContentLoaded", function () {
    const minMoneyInput = document.getElementById("minMoney");
    const maxMoneyInput = document.getElementById("maxMoney");

    minMoneyInput.addEventListener("input", function () {
        const minValue = parseInt(minMoneyInput.value) || 0;
        const newMinForMax = minValue + 10000;

        maxMoneyInput.min = newMinForMax;

        if (parseInt(maxMoneyInput.value) < newMinForMax) {
            maxMoneyInput.value = newMinForMax;
        }
    });
});