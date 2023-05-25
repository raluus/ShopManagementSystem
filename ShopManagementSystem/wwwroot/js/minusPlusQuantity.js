document.addEventListener('DOMContentLoaded', function () {

    
    var totalPriceElements = document.querySelectorAll('.total-price-number')
    var tva = 0;

    totalPriceElements.forEach(function (totalPriceElement) {
        var itemContainer = totalPriceElement.closest('.item-product');
        var quantityElement = itemContainer.querySelector('.quantity-pph')
        var priceElement = itemContainer.querySelector('.price-pph');

        if (parseInt(quantityElement.textContent) > 1) {
            var currentPrice = parseFloat(priceElement.textContent);
            currentPrice = (parseFloat(priceElement.textContent) * parseInt(quantityElement.textContent)).toFixed(2);
            totalPriceElement.textContent = "";
            totalPriceElement.textContent = currentPrice;
        }
        else if (parseInt(quantityElement.textContent) == 1)
        {
            totalPriceElement.textContent = parseFloat(priceElement.textContent).toFixed(2);
        }
    });

    calculateSubtotal();

    function minusQuantity(minusButton) {
        var itemContainer = minusButton.closest('.item-product');
        var priceElement = itemContainer.querySelector('.price-pph');
        var quantityElement = itemContainer.querySelector('.quantity-pph');
        var totalPriceElement = itemContainer.querySelector('.total-price-number');

        if (priceElement && quantityElement) {
            if (parseInt(quantityElement.textContent) > 1) {
                var currentQuantity = parseInt(quantityElement.textContent);
                var price = parseFloat(priceElement.textContent);
                currentQuantity--;
                currentPrice = (price * currentQuantity).toFixed(2);
                quantityElement.textContent = "";
                totalPriceElement.textContent = "";
                quantityElement.textContent = currentQuantity;
                totalPriceElement.textContent = currentPrice;
                calculateSubtotal();

            }
        } else {
            console.log('Price or quantity element not found.');
        }
    }

    function plusQuantity(plusButton) {
        var itemContainer = plusButton.closest('.item-product');
        var priceElement = itemContainer.querySelector('.price-pph');
        var quantityElement = itemContainer.querySelector('.quantity-pph');
        var totalPriceElement = itemContainer.querySelector('.total-price-number');

        if (priceElement && quantityElement) {
            if (parseInt(quantityElement.textContent) < 10) {
                var currentQuantity = parseInt(quantityElement.textContent);
                var price = parseFloat(priceElement.textContent);
                currentQuantity++;
                currentPrice = (price * currentQuantity).toFixed(2);
                quantityElement.textContent = "";
                totalPriceElement.textContent = "";
                quantityElement.textContent = currentQuantity;
                totalPriceElement.textContent = currentPrice;
                calculateSubtotal();
            }
            else {
                console.log("Can't add more than 10 products!");
            }
        } else {
            console.log('Price or quantity element not found.');
        }
    }

    function calculateSubtotal() {
        var subtotalValue = 0;

        var itemContainers = document.querySelectorAll('.item-product');
        itemContainers.forEach(function (itemContainer) {
            var totalPriceElement = itemContainer.querySelector('.total-price-number');

            if (totalPriceElement) {
                var totalPrice = parseFloat(totalPriceElement.textContent);
                subtotalValue += totalPrice;
            }
        });

        calculateTVA();
        var subtotalValueElement = document.getElementById('subtotal-value');
        subtotalValueElement.value = subtotalValue.toFixed(2);

        var subtotalDisplayElement = document.getElementById('subtotal-display');
        subtotalDisplayElement.textContent = subtotalValue.toFixed(2) + "(from which " + tva.toFixed(2) + " TVA)";
    }


    function calculateTVA() {
        tva = 0;
        var itemContainers = document.querySelectorAll('.item-product');

        itemContainers.forEach(function (itemContainer) {
            var basePrice = itemContainer.querySelector('.price-without-tva');
            var currentPrice = itemContainer.querySelector('.price-pph');
            var quantity = itemContainer.querySelector('.quantity-pph');
            tva += (parseFloat(currentPrice.textContent) - parseFloat(basePrice.textContent)) * parseInt(quantity.textContent);
        });
        var tvaValueElement = document.getElementById('tva-value');
        tvaValueElement.value = tva.toFixed(2);
    }

    calculateTVA();

    var minusButtons = document.querySelectorAll('.minusBtn');
    minusButtons.forEach(function (minusButton) {
        minusButton.addEventListener('click', function () {
            minusQuantity(minusButton);
        });
    });

    var plusButtons = document.querySelectorAll('.plusBtn');
    plusButtons.forEach(function (plusButton) {
        plusButton.addEventListener('click', function () {
            plusQuantity(plusButton);
        });
    });
});