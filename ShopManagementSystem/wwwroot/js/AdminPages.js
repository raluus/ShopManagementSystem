
document.addEventListener('DOMContentLoaded', function () {

    function populateOptions(selectElement, options) {
        selectElement.innerHTML = '';
        for (let option of options) {
            const optionElement = document.createElement('option');
            optionElement.value = option;
            optionElement.textContent = option;
            selectElement.appendChild(optionElement);
        }
    }
    // Fetch the JSON file
    fetch('/Json/categoryData.json')
        .then(response => response.json())
        .then(data => {
            // Extract the data from the JSON object
            const mainCategories = data.mainCategories;
            const subcategories = data.subcategories;
            const nestedCategories = data.nestedcategories;
            const brandNames = data.brand;
            const priceUnits = data.priceUnit;
            const attributes = data.attributes;
            var maxAllowedInputs = 0;


            // Function to populate select element options
           

            // Function to set the default value of the subcategory select
            function setDefaultSubcategory() {
                const mainCategorySelect = document.getElementById('main-category');
                const subcategorySelect = document.getElementById('subcategory');
                const nestedCategoriesSelect = document.getElementById('nested-category');
                const brandSelect = document.getElementById('product-brand');
                const priceUnitSelect = document.getElementById('price-unit');
                const nestedCategoriesLabel = document.getElementById('nested-category-label')
                const selectedMainCategory = mainCategorySelect.value;

                populateOptions(subcategorySelect, subcategories[selectedMainCategory]);
                populateOptions(brandSelect, brandNames[selectedMainCategory]);
                populateOptions(priceUnitSelect, priceUnits[selectedMainCategory]);
                maxAllowedInputs = attributes[mainCategorySelect.value].length;

                const selectedSubCategory = subcategorySelect.value;

                if (nestedCategories[selectedSubCategory]) {
                    populateOptions(nestedCategoriesSelect, nestedCategories[selectedSubCategory]);
                    nestedCategoriesSelect.style.display = 'block';
                    nestedCategoriesLabel.style.display = 'block';

                } else {
                    nestedCategoriesSelect.innerHTML = ''; // Clear the options in the sub-subcategory select
                    nestedCategoriesSelect.style.display = 'none'; // Hide the sub-subcategory select
                    nestedCategoriesLabel.style.display = 'none';
                }
            }

            // Function to populate sub-subcategory options
            function populateNestedOptions(selectedSubcategory) {
                const nestedCategoriesSelect = document.getElementById('nested-category');
                const nestedCategoriesLabel = document.getElementById('nested-category-label')
                if (nestedCategories[selectedSubcategory]) {
                    populateOptions(nestedCategoriesSelect, nestedCategories[selectedSubcategory]);
                    nestedCategoriesSelect.style.display = 'block'; // Show the sub-subcategory select
                    nestedCategoriesLabel.style.display = 'block';

                } else {
                    nestedCategoriesSelect.innerHTML = ''; // Clear the options in the sub-subcategory select
                    nestedCategoriesSelect.style.display = 'none'; // Hide the sub-subcategory select
                    nestedCategoriesLabel.style.display = 'none';
                }
            }
            // Populate main category options
            const mainCategorySelect = document.getElementById('main-category');
            populateOptions(mainCategorySelect, mainCategories);

            setDefaultSubcategory();

            // Event listener for main category change
            mainCategorySelect.addEventListener('change', () => {
                const selectedMainCategory = mainCategorySelect.value;

                const subcategorySelect = document.getElementById('subcategory');
                var selectedSubcategory = subcategorySelect.value;
                populateOptions(subcategorySelect, subcategories[selectedMainCategory]);

                const brandSelect = document.getElementById('product-brand');
                populateOptions(brandSelect, brandNames[selectedMainCategory]);

                const priceUnitSelect = document.getElementById('price-unit');
                populateOptions(priceUnitSelect, priceUnits[selectedMainCategory]);

                selectedSubcategory = subcategorySelect.value;
                populateNestedOptions(selectedSubcategory);

                maxAllowedInputs = attributes[mainCategorySelect.value].length;

            });

            // Event listener for subcategory change
            const subcategorySelect = document.getElementById('subcategory');
            subcategorySelect.addEventListener('change', () => {
                const selectedSubcategory = subcategorySelect.value;

                // Populate sub-subcategory options based on selected subcategory
                populateNestedOptions(selectedSubcategory);
            });  

    //additional attri js

    console.log(maxAllowedInputs);
    var currentGeneratedInputs = 0;
    let chosenAttributes = [];
    let chosenAttributesKeysAndValues = [];

    function populateOptionsForAdditionalAttributes(selectElement, options) {
        selectElement.innerHTML = '';
        const optionElement = document.createElement('option');
        optionElement.value = "";
        optionElement.text = "Please select a value";
        optionElement.disabled = true;
        optionElement.selected = true;
        selectElement.appendChild(optionElement);
        for (let option of options) {
            if (chosenAttributes.includes(option)) {
                const optionElement = document.createElement('option');
                optionElement.value = option;
                optionElement.textContent = option;
                optionElement.disabled = true;
                selectElement.appendChild(optionElement);
            }
            else {
                const optionElement = document.createElement('option');
                optionElement.value = option;
                optionElement.textContent = option;
                selectElement.appendChild(optionElement);
            }
        }
        currentGeneratedInputs++;
    }

    function removeInputs(event) {
        var button = event.target; // Get the clicked button
        var inputWrapper = button.parentElement; // Get the parent element containing the inputs
        var selectElement = inputWrapper.querySelector('select');
        let valueToRemove = selectElement.value;
        chosenAttributesKeysAndValues.splice(chosenAttributesKeysAndValues.indexOf(valueToRemove));
        chosenAttributes.splice(chosenAttributes.indexOf(valueToRemove));
        inputWrapper.remove(); // Remove the input wrapper element
        currentGeneratedInputs--;
    }

    function addChangeSelectListener(selectElement) {
        selectElement.addEventListener("change", function () {
            selectElement.disabled = true;
            chosenAttributes.push(selectElement.value);
            var input = selectElement.nextElementSibling;

            var attributeKey = selectElement.value;
            var attributeValue = "";

            if (input.disabled == true) {
                attributeValue = input.value;
                var attribute = { attributeKey: attributeKey, attributeValue: attributeValue };
                chosenAttributesKeysAndValues.push(attribute);

            } else {
                var attribute = { attributeKey: attributeKey, attributeValue: attributeValue };
                chosenAttributesKeysAndValues.push(attribute);

            }
        });
    }

    function updateAttributeValueByKey(key, newValue) {
        for (var i = 0; i < chosenAttributesKeysAndValues.length; i++) {
            if (chosenAttributesKeysAndValues[i].attributeKey === key) {
                chosenAttributesKeysAndValues[i].attributeValue = newValue;
                break;
            }
        }
    }

    function addKeyDownInputListener(selectElement) {
        selectElement.addEventListener("keydown", function (event) {
            if (event.key === "Enter") {
                selectElement.disabled = true;
                var select = selectElement.parentElement.querySelector("select");
                if (select.disabled == true) {
                    updateAttributeValueByKey(select.value, selectElement.value);
                }
            }
        });
    }

    function submitForm() {
        // Assign the attributes to hidden input fields or send them using AJAX
        var attributeKeys = chosenAttributesKeysAndValues.map(a => a.attributeKey);
        var attributeValues = chosenAttributesKeysAndValues.map(a => a.attributeValue);
        var form = document.getElementById('create-form');

        for (var i = 0; i < attributeKeys.length; i++) {
            var hiddenKeyInput = document.createElement("input");
            hiddenKeyInput.id = "AttributeKeys";
            hiddenKeyInput.type = "hidden";
            hiddenKeyInput.name = "attributeKeys";
            hiddenKeyInput.value = attributeKeys[i];

            var hiddenValueInput = document.createElement("input");
            hiddenValueInput.type = "hidden";
            hiddenValueInput.id = "AttributeValues";
            hiddenValueInput.name = "attributeValues";
            hiddenValueInput.value = attributeValues[i];

            form.appendChild(hiddenKeyInput);
            form.appendChild(hiddenValueInput);
        }

        form.submit();
    }

    document.getElementById("createBtn").addEventListener('click', () => {
        submitForm();
    });


    document.getElementById('addAttributeButton').addEventListener('click', () => {
        if (currentGeneratedInputs < maxAllowedInputs) {
            var attributeContainer = document.getElementById("attributeList");

            var inputWrapper = document.createElement("div");
            inputWrapper.classList.add("form-group");

            var select = document.createElement("select");
            select.classList.add("additional-attributes");
            select.name = "additional-attributes";
            populateOptionsForAdditionalAttributes(select, attributes[mainCategorySelect.value]);

            var input = document.createElement("input");
            input.classList.add("form-control");
            input.setAttribute("type", "text");

            var validationSpan = document.createElement("span");
            validationSpan.classList.add("text-danger");

            var removeButton = document.createElement("button");
            removeButton.type = "button";
            removeButton.classList.add("btn", "btn-danger");
            removeButton.textContent = "Remove";
            removeButton.addEventListener("click", removeInputs);

            inputWrapper.appendChild(select);
            inputWrapper.appendChild(input);
            inputWrapper.appendChild(validationSpan);
            inputWrapper.appendChild(removeButton);

            attributeContainer.appendChild(inputWrapper);
            addChangeSelectListener(select);
            addKeyDownInputListener(input);
        }
        else {
            alert("You can't add more inputs! Maximum number reached");
        }

    });
        })
     .catch (error => {
        console.error('Error fetching category data:', error);
     });

    fetch('/Json/inventoryData.json')
        .then(response => response.json())
        .then(data => {

            const suppliers = data.suppliers;
            const locations = data.locations;
            const supplierSelect = document.getElementById('product-supplier');
            const locationSelect = document.getElementById('product-location');

            populateOptions(supplierSelect, suppliers);
            populateOptions(locationSelect, locations);

        })
        .catch(error => {
            console.error('Error fetching inventory data:', error);
        });

});
