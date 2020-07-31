function updateTotalPrice(discount) {
    var discountValue = Number(discount.value);
    var totalPrice = Number(document.getElementById('total-price-default').value);
    var totalPriceInput = document.getElementById('preview-total-price');
    var calculatedDiscount = 0;

    console.log("discount value: " + discountValue)
    if (discountValue !== 0) {
        document.getElementById('total-price-default').value;
        calculatedDiscount = (discountValue / 100) * totalPrice;
        totalPrice = totalPrice - (calculatedDiscount);
        totalPriceInput.value = totalPrice.toFixed(2);
    } else {
        totalPriceInput.value = totalPrice;
    }
    document.getElementById('discount-label').innerHTML = "Отстъпка (" + calculatedDiscount + "лв)";
}

function removeSelectedProductsDisabled() {
    document.getElementById('items-select').disabled = false;
}