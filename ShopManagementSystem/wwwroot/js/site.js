

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

document.addEventListener('DOMContentLoaded', function () {
    var mainCategoryLinks = document.querySelectorAll('.main-category-list a');
    var subcategoryNavigation = document.querySelector('.subcategory-navigation');

    mainCategoryLinks.forEach(function (link) {
        link.addEventListener('mouseenter', function (event) {
            event.preventDefault();

            var mainCategory = this.dataset.category;
            var subcategories = getSubcategories(mainCategory);

            subcategoryNavigation.innerHTML = '';

            if (subcategories.length > 0) {
                var subcategoryList = document.createElement('ul');
                subcategoryList.className = 'subcategory-list';

                subcategories.forEach(function (subcategory) {
                    var subcategoryItem = document.createElement('li');
                    subcategoryItem.className = 'subcategory-item';
                    var subcategoryForLink = subcategory.toLowerCase().replace(/\s+/g, '-');
                    var subcategoryLink = document.createElement('a');
                    subcategoryLink.href = '/Products/?category=' + mainCategory + "&subcategory=" + subcategoryForLink;
                    subcategoryLink.textContent = subcategory;
                    subcategoryItem.appendChild(subcategoryLink);
                    subcategoryList.appendChild(subcategoryItem);

                    if (getNestedCategories(subcategory).length > 0) {
                        var nestedCategoryContainer = document.createElement('div');
                        nestedCategoryContainer.className = 'nestedCategory-container';

                        var nestedCategoryList = document.createElement('ul');
                        nestedCategoryList.className = 'nestedCategories-list';

                        var nestedCategories = getNestedCategories(subcategory);
                        nestedCategories.forEach(function (nestedCategory) {
                            var nestedCategoriesItem = document.createElement('li');
                            nestedCategoriesItem.className = 'nestedCategories-item';
                            var nestedCategoryForLink = nestedCategory.toLowerCase().replace(/\s+/g, '-');
                            var nestedCategoriesLink = document.createElement('a');
                            nestedCategoriesLink.href = '/Products/?category=' + mainCategory + "&subcategory=" + subcategoryForLink + "&nestedCategory=" + nestedCategoryForLink;
                            nestedCategoriesLink.textContent = nestedCategory;
                            nestedCategoriesItem.appendChild(nestedCategoriesLink);
                            nestedCategoryList.appendChild(nestedCategoriesItem);
                        });
                        subcategoryList.appendChild(nestedCategoryList);
                    }
                   
                });

                subcategoryNavigation.appendChild(subcategoryList);
            }
        });
    });
});
function getSubcategories(mainCategory) {

    switch (mainCategory) {
        case 'grocery':
            return ["Fruits", "Vegetables", "Sweets", "Nuts", "Seeds", "Salads", "Herbs", "Dairy Products", "Eggs", "Cereal", "Basic Foodstuffs", "Meat", "Fish", "Deli", "Tobacco", "Water", "Tea", "Coffee", "Wine", "Champagne", "Beer", "Cider", "Hard Liquors", "Non-Alcoholic Drinks"];
        case 'personalCareAndBeauty':
            return ["Dental Hygiene", "Perfumes", "Hair Care Products", "Body Care Products", "Natural Cosmetics", "Facial Care Products"];
        case 'homeFurnitureAndAppliances':
            return ["Home", "Furniture", "Bathroom", "Kitchen", "Home Maintenance", "Home Decorations", "Laundry Detergent & Fabric Softener", "Kitchen Appliances", "Large Appliances"];
        case 'sportsAndOutdoors':
            return ["Sports", "Outdoor"];
        case 'toysAndGames':
            return ["Toys", "Video Games"];
        case 'electronics':
            return ["PC and Peripherals", "Laptops and Mobiles", "TV and Audio", "Smartwatches"];
        case 'baby':
            return ["Baby Food", "Baby Milk and Drinks", "Baby Toys", "Diapers"];
        case 'pets':
            return ["Dog", "Cat", "Birds", "Rodents", "Fishes"];
        case 'gardenAndAuto':
            return ["Garden", "Auto", "Flowers"];
        case 'booksAndMoviesAndMusic':
            return ["Books", "Music", "Movies"];
        default:
            return [];
    }
}

function getNestedCategories(subcategory) {
    switch (subcategory) {
        case "Fruits":
            return ["Organic Fruits", "Frozen Fruits", "Fresh Fruits", "Dried Fruits"];
        case "Vegetables":
            return ["Organic Vegetables", "Frozen Vegetables", "Fresh Vegetables", "Dried Vegetables"];
        case "Sweets":
            return ["Chocolate", "Biscuits", "Wafer Biscuits"];
        case "Dairy Products":
            return ["Milk", "Yogurt", "Cheese", "Sour Cream", "Whipped Cream", "Butter", "Margarine"];
        case "Basic Foodstuffs":
            return ["Oil", "Vinegar", "Sugar", "Rice", "Flour"];
        case "Meat":
            return ["Chicken Meat", "Pork Meat", "Beef Meat", "Sheep Meat", "Frozen Meat"];
        case "Fish":
            return ["Fresh Fish", "Frozen Fish", "Seafood", "Sushi"];
        case "Deli":
            return ["Salami", "Ham", "Sausages"];
        case "Tobacco":
            return ["Cigarettes", "E-cigarettes"];
        case "Water":
            return ["Still Water", "Sparkling Water", "Flavored Water"];
        case "Coffee":
            return ["Ground Coffee", "Coffee Beans", "Instant Coffee"];
        case "Wine":
            return ["Red Wine", "White Wine", "Rosé Wine"];
        case "Champagne":
            return ["For Kids"];
        case "Beer":
            return ["Bottled Beer", "Canned Beer", "Craft Beer", "Non-alcoholic Beer"];
        case "Hard Liquors":
            return ["Whiskey", "Rum", "Vodka", "Tequilla"];
        case "Non-Alcoholic Drinks":
            return ["Sparkling Juice", "Still Juice", "Fresh Juice", "Energy Drinks"];
        case "Dental Hygiene":
            return ["Toothpaste", "Toothbrush", "E-Toothbrush", "Mouthwash"];
        case "Perfumes":
            return ["Men", "Women"];
        case "Hair Care Products":
            return ["Shampoo", "Hair Dye", "Conditioner", "Hair Treatment", "Styling"];
        case "Body Care Products":
            return ["Body Wash", "Soap", "Lotions and Creams", "Hair Removal Products", "Hand Care Products", "Foot Care Products", "Deodorants"];
        case "Facial Care Products":
            return ["Face Cream", "Makeup", "Lipsticks", "After Shave", "Facial Cleansing", "Shavers"];
        case "Home":
            return ["Rug", "Bedspread", "Bedding", "Beds", "Pillows", "Duvet"];
        case "Furniture":
            return ["Office Furniture", "Kitchen Furniture", "Kids's Furniture", "Mattresses"];
        case "Bathroom":
            return ["Beach Towels", "Baby Towels", "Children's Towels and Robes", "Bath Towels and Robes"];
        case "Kitchen":
            return ["Cooking Accessories", "Glasses", "Cutlery", "Cooking Utensils"];
        case "Home Maintenance":
            return ["Garbage Bins", "Brooms", "Mops", "Laundry Baskets", "Ironing Boards", "Clothes Dryers"];
        case "Home Decorations":
            return ["Mirrors", "Decorative Objects"];
        case "Laundry Detergent & Fabric Softener":
            return ["Powder Detergent", "Liquid Detergent", "Detergent Capsules", "Fabric Softener", "Descaling Solutions."];
        case "Kitchen Appliances":
            return ["Blenders", "Mixers", "Kitchen Scale", "Water Dispensers"];
        case "Large Appliances":
            return ["Refrigerator", "Washing Machines", "Clothes Dryers", "Stove", "Dishwashers", "Microwave"];
        case "Sports":
            return ["Tennis", "Fitness", "Camping", "Fishing", "Athletics", "Outdoor Activities", "Cycling"];
        case "Outdoor":
            return ["Plants", "Trees", "Irrigation", "Pools and Accessories"];
        case "Toys":
            return ["Plush Toys", "Dolls", "Boys' Toys", "Girls' Toys", "Board Games", "Puzzles", "Figurines"];
        case "Video Games":
            return ["PC Games", "Console Games", "Accesories"];
        case "PC and Peripherals":
            return ["PC and Accessories", "Printers and Consumables", "Storage Media and Supports", "PC Peripherals"];
        case "Laptops and Mobiles":
            return ["Laptop and Accessories", "Phones and Accessories"];
        case "TV and Audio":
            return ["Televisions and Accessories", "Speakers", "Audio Systems", "Home Cinema Systems", "Headphones"];
        case "Baby Food":
            return ["Meat Dishes", "Vegetable Dishes", "Fruit Desserts", "Dairy Desserts"];
        case "Baby Milk and Drinks":
            return ["Baby Water", "Tea", "Milk Formulas", "Fruit/Vegetable Juice"];
        case "Baby Toys":
            return ["0-5 Months", "6-11 Months", "12-23 Months", "24+ Months"];
        case "Dog":
            return ["Dog Food", "Dog Accesories"];
        case "Cat":
            return ["Cat Food", "Cat Accesories"];
        case "Birds":
            return ["Birds Food", "Birds Accesories"];
        case "Rodents":
            return ["Rodents Food", "Rodents Accesories"];
        case "Fishes":
            return ["Fish Food", "Fish Accesories"];
        case "Garden":
            return ["Garden Furniture", "Garden Decor Items", "Garden Hand Tools", "Barbecues"];
        case "Auto":
            return ["Auto Parts", "Auto Oils", "Tires and Rims", "Auto Interior Accessories", "Auto Maintenance and Cosmetics"];
        case "Flowers":
            return ["Flower Bouquets", "Houseplants"];
        case "Books":
            return ["Romance Books", "Sci-fi Books", "Mistery books", "Drama Books", "Comedy Books", "Manga"];
        case "Music":
            return ["Music Accesories", "Guitars", "Pianos", "Flutes"];
        case "Movies":
            return ["Romance Movies", "Animated Movies", "Comedy Movies", "Drama Movies"];
        default:
            return [];
    }
}

