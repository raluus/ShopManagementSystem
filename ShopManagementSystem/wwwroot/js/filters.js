document.addEventListener('DOMContentLoaded', function () {

    var labelForSubcategories = document.getElementById('label-for-subcategory');
    var labelForNestedCategories = document.getElementById('label-for-nestedcategory');
    var labelForBrands = document.getElementById('label-for-brand');
    if (labelForSubcategories != null) {
        labelForSubcategories.addEventListener('click', function () {
            setTimeout(function () {
                var subcategoryContainer = document.getElementById('subcategory-filters-container');
                subcategoryContainer.classList.toggle('hidden');
            }, 200);
        });
    }
    if (labelForNestedCategories != null) {
        labelForNestedCategories.addEventListener('click', function () {
            setTimeout(function () {
                var nestedCategoryContainer = document.getElementById('nestedcategory-filters-container');
                nestedCategoryContainer.classList.toggle('hidden');
            }, 200);
        });
    }
    if (labelForBrands != null) {
        labelForBrands.addEventListener('click', function () {
            setTimeout(function () {
                var brandsContainer = document.getElementById('brand-filters-container');
                brandsContainer.classList.toggle('hidden');
            }, 200);
        });
    }

    fetch('/Json/categoryData.json')
        .then(response => response.json())
        .then(data => {

            var mainCategories = data.mainCategories;
            var subcategories = data.subcategories;
            var nestedCategories = data.nestedcategories;
            var brands = data.brand;

            var chosenMainCategory = document.getElementById('chosen-category');
            var subcategoryContainer = document.getElementById('subcategory-filters-container');
            if (subcategoryContainer != null) {
                for (var subcategory of subcategories[chosenMainCategory.value]) {
                    var checkbox = document.createElement('input');
                    checkbox.type = 'checkbox';
                    checkbox.classList.add('subcategory-checkbox');
                    checkbox.name = "subcategoryFilters";
                    checkbox.value = subcategory;

                    var label = document.createElement('label');
                    label.textContent = subcategory;

                    var container = document.createElement('div');

                    container.appendChild(checkbox);
                    container.appendChild(label);
                    subcategoryContainer.appendChild(container);
                }
            }

            var chosenSubcategory = document.querySelectorAll('.chosen-subcategory');
            if (chosenSubcategory != null) {
                var nestedCategoryContainer = document.getElementById('nestedcategory-filters-container');
                for (var subcategory of chosenSubcategory) {
                    for (var nestedCategory of nestedCategories[subcategory.value]) {
                        var checkbox = document.createElement('input');
                        checkbox.type = 'checkbox';
                        checkbox.classList.add('nestedcategory-checkbox');
                        checkbox.name = "nestedCategoryFilters";
                        checkbox.value = nestedCategory;

                        var label = document.createElement('label');
                        label.textContent = nestedCategory;

                        var container = document.createElement('div');

                        container.appendChild(checkbox);
                        container.appendChild(label);
                        nestedCategoryContainer.appendChild(container);
                    }
                }
            }

            var brandContainer = document.getElementById('brand-filters-container');
            if (brandContainer != null) {
                for (var brand of brands[chosenMainCategory.value]) {
                    var checkbox = document.createElement('input');
                    checkbox.type = 'checkbox';
                    checkbox.classList.add('brand-checkbox');
                    checkbox.name = "brandFilters";
                    checkbox.value = brand;

                    var label = document.createElement('label');
                    label.textContent = brand;

                    var container = document.createElement('div');

                    container.appendChild(checkbox);
                    container.appendChild(label);
                    brandContainer.appendChild(container);
                }
            }

        })
        .catch(error => {
            console.error('Error fetching category data:', error);
        });
});