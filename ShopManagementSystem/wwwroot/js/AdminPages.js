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

        // Function to populate select element options
        function populateOptions(selectElement, options) {
            selectElement.innerHTML = '';
            for (let option of options) {
                const optionElement = document.createElement('option');
                optionElement.value = option;
                optionElement.textContent = option;
                selectElement.appendChild(optionElement);
            }
        }

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

            // Populate subcategory options based on selected main category
            const subcategorySelect = document.getElementById('subcategory');
            populateOptions(subcategorySelect, subcategories[selectedMainCategory]);

            const brandSelect = document.getElementById('product-brand');
            populateOptions(brandSelect, brandNames[selectedMainCategory]);

            const priceUnitSelect = document.getElementById('price-unit');
            populateOptions(priceUnitSelect, priceUnits[selectedMainCategory]);

            // Reset sub-subcategory options
            const nestedCategoriesSelect = document.getElementById('nested-category');
            nestedCategoriesSelect.innerHTML = '';
            nestedCategoriesSelect.style.display = 'none'; // Hide the sub-subcategory select
        });

        // Event listener for subcategory change
        const subcategorySelect = document.getElementById('subcategory');
        subcategorySelect.addEventListener('change', () => {
            const selectedSubcategory = subcategorySelect.value;

            // Populate sub-subcategory options based on selected subcategory
            populateNestedOptions(selectedSubcategory);
        });
    })
    .catch(error => {
        console.error('Error fetching category data:', error);
    });
