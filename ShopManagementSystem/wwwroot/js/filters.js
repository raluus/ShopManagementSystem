document.addEventListener('DOMContentLoaded', function () {

    fetch('/Json/categoryData.json')
        .then(response => response.json())
        .then(data => {

            var mainCategories = data.mainCategories;
            var subcategories = data.subcategories;
            var nestedCategories = data.nestedCategories;
            var brands = data.brands;

            var chosenMainCategory = document.getElementById('chosen-category');
            var subcategoryContainer = document.getElementById('filter-for-subcategory');
            for (var subcategory of subcategories[chosenMainCategory.value]) {
                var checkbox = document.createElement('input');
                checkbox.type = 'checkbox';
                checkbox.classList.add('subcategory-checkbox');
                checkbox.value = subcategory;

                var label = document.createElement('label');
                label.textContent = subcategory;

                var container = document.createElement('div');

                container.appendChild(checkbox);
                container.appendChild(label);
                subcategoryContainer.appendChild(container);
            }

        })
        .catch(error => {
            console.error('Error fetching category data:', error);
        });
});