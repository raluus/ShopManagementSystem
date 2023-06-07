
document.addEventListener('DOMContentLoaded', function () {

    var defaultValue = 1;

    function populateOptions(selectElement, options) {
        if (!defaultValue)
           selectElement.innerHTML = '';
        for (let option of options) {
                    const optionElement = document.createElement('option');
                    optionElement.value = option;
                    optionElement.textContent = option;
                    selectElement.appendChild(optionElement);
        }
    }

    fetch('/Json/categoryData.json')
        .then(response => response.json())
        .then(data => {
          
            const mainCategories = data.mainCategories;
            const subcategories = data.subcategories;
            const nestedCategories = data.nestedcategories;
            const brandNames = data.brand;

            
            function setDefaultSubcategory() {
                const mainCategorySelect = document.getElementById('main-category');
                const subcategorySelect = document.getElementById('subcategory');
                const nestedCategoriesSelect = document.getElementById('nested-category');
                const brandSelect = document.getElementById('product-brand');
                const nestedCategoriesLabel = document.getElementById('nested-category-label')
                const selectedMainCategory = mainCategorySelect.value;

                populateOptions(subcategorySelect, subcategories[selectedMainCategory]);
                populateOptions(brandSelect, brandNames[selectedMainCategory]);

                const selectedSubCategory = subcategorySelect.value;

                if (nestedCategories[selectedSubCategory]) {
                    populateOptions(nestedCategoriesSelect, nestedCategories[selectedSubCategory]);
                    nestedCategoriesSelect.style.display = 'block';
                    nestedCategoriesLabel.style.display = 'block';

                } else {
                    nestedCategoriesSelect.innerHTML = ''; 
                    nestedCategoriesSelect.style.display = 'none'; 
                    nestedCategoriesLabel.style.display = 'none';
                }
            }

           
            function populateNestedOptions(selectedSubcategory) {
                const nestedCategoriesSelect = document.getElementById('nested-category');
                const nestedCategoriesLabel = document.getElementById('nested-category-label')
                if (nestedCategories[selectedSubcategory]) {
                    populateOptions(nestedCategoriesSelect, nestedCategories[selectedSubcategory]);
                    nestedCategoriesSelect.style.display = 'block'; 
                    nestedCategoriesLabel.style.display = 'block';

                } else {
                    nestedCategoriesSelect.innerHTML = ''; 
                    nestedCategoriesSelect.style.display = 'none'; 
                    nestedCategoriesLabel.style.display = 'none';
                }
            }
          
            const mainCategorySelect = document.getElementById('main-category');
            populateOptions(mainCategorySelect, mainCategories);

            setDefaultSubcategory();

            mainCategorySelect.addEventListener('change', () => {
                defaultValue = 0;
                const selectedMainCategory = mainCategorySelect.value;
               
                const subcategorySelect = document.getElementById('subcategory');
                var selectedSubcategory = subcategorySelect.value;
                populateOptions(subcategorySelect, subcategories[selectedMainCategory]);

                const brandSelect = document.getElementById('product-brand');
                populateOptions(brandSelect, brandNames[selectedMainCategory]);

                selectedSubcategory = subcategorySelect.value;
                populateNestedOptions(selectedSubcategory);

            });

            
            const subcategorySelect = document.getElementById('subcategory');
            subcategorySelect.addEventListener('change', () => {
                const selectedSubcategory = subcategorySelect.value;

             
                populateNestedOptions(selectedSubcategory);
            });

           

           
  
        })
        .catch(error => {
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
