

document.addEventListener('DOMContentLoaded', function () {
    var productsField = document.querySelector('.products-field');
    var mainCategoryList = document.querySelector('.main-category-list');
    var subcategoryNavigation = document.querySelector('.subcategory-navigation');

    productsField.addEventListener('click', function (event) {
        event.preventDefault();
        mainCategoryList.classList.toggle('open');
        subcategoryNavigation.classList.toggle('open');
    });


});

$(document).ready(function () {
    // Handle click event on main category links
    
    $('.main-category-list a').mouseenter(function (e) {
        e.preventDefault();

        var mainCategory = $(this).data('category');
        var subcategories = getSubcategories(mainCategory);

        // Clear subcategory menu
        $('.subcategory-navigation').empty();

        // Populate subcategory menu
        if (subcategories.length > 0) {
            var subcategoryList = $('<ul class="subcategory-list"></ul>');

            subcategories.forEach(function (subcategory) {
                var subcategoryItem = $('<li class="subcategory-item"></li>');
                var subcategoryLink = $('<a href="#">' + subcategory + '</a>');
                subcategoryItem.append(subcategoryLink);
                subcategoryList.append(subcategoryItem);
            });

            $('.subcategory-navigation').append(subcategoryList);

            
           
        }
    });

   
});

// Helper function to get subcategories based on main category
function getSubcategories(mainCategory) {
    // Add your logic to retrieve subcategories based on main category
    // For this example, let's assume we have predefined subcategories

    switch (mainCategory) {
        case 'grocery':
            return ['Fruits', 'Vegetables', 'Meat'];
        case 'beauty':
            return ['Skincare', 'Haircare', 'Cosmetics'];
        case 'home':
            return ['Kitchen Appliances', 'Cleaning Appliances'];
        case 'sports':
            return ['Outdoors Sports', 'Tracksuits'];
        case 'toys':
            return ['Pc games', '1-2 yo toys'];
        case 'electronics':
            return ['TV', 'Mobile'];
        case 'baby':
            return ['10-12 months', 'baby clothes'];
        case 'kids':
            return ['kids clothes', 'back to school'];
        case 'pets':
            return ['food', 'toys'];
        case 'garden':
            return ['outdoor tools', 'auto'];
        case 'books':
            return ['genre', 'movies'];
        case 'clothes':
            return ['women', 'men'];
        default:
            return [];
    }
}

