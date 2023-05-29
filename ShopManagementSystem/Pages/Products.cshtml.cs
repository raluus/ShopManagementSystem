using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using ShopManagementSystem.Data;
using ShopManagementSystem.Models;

namespace ShopManagementSystem.Pages
{
    public class ProductsModel : PageModel
    {
        private readonly ShopManagementSystem.Data.ShopManagementSystemContext _context;

        [BindProperty(SupportsGet = true)]
        public string Category { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Subcategory { get; set; }

        [BindProperty(SupportsGet = true)]
        public string NestedCategory { get; set; }

        public ProductsModel(ShopManagementSystem.Data.ShopManagementSystemContext context)
        {
            _context = context;
            Category = "";
            Subcategory = "";
            NestedCategory = "";
        }

        public List<Product> Product { get;set; } = default!;

        [BindProperty]
        public List<ProductCategory> ProductCategory { get; set; } = default!;

        [BindProperty]
        public ProductSubCategory ProductSubCategory { get; set; } = default!;

        [BindProperty]
        public ProductNestedCategory ProductNestedCategory { get; set; } = default!;

        [BindProperty]
        public List<ProductInventory> ProductInventory { get; set; } = default!;

        [BindProperty]
        public List<ProductAttributes> ProductAttributes { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Product = new List<Product>();
            var productInventory = from m in _context.ProductInventory
                                   select m;
            if (_context.Product != null)
            {
                if(Category != "" && Subcategory == "")
                {
                    Product = await _context.Product
                   .Join(
                    _context.ProductCategory.Where(pc => pc.CategoryName == GetCategoryDisplayName(Category)),
                    p => p.Id,
                    pc => pc.ProductId,
                    (p, pc) => p
                    )
                    .ToListAsync();
                }else if(Subcategory != "" && NestedCategory == "")
                {
                    Product = await _context.Product
                   .Join(
                    _context.ProductSubCategory.Where(pc => pc.SubCategoryName == GetSubCategoryDisplayName(Subcategory)),
                    p => p.Id,
                    pc => pc.ProductId,
                    (p, pc) => p
                    )
                    .ToListAsync();
                }else
                {
                    Product = await _context.Product
                  .Join(
                   _context.ProductNestedCategory.Where(pc => pc.NestedCategoryName == GetNestedCategoryDisplayName(NestedCategory)),
                   p => p.Id,
                   pc => pc.ProductId,
                   (p, pc) => p
                   )
                   .ToListAsync();
                }
                ProductInventory = await productInventory.ToListAsync();
            }
        }

        public IActionResult OnPostFilterProducts([FromBody] dynamic filters)
        {
            return new OkResult();

        }

            public string GetCategoryDisplayName(string categoryName)
        {
            switch (categoryName)
            {
                case "grocery":
                    return "Grocery";
                case "personalCareAndBeauty":
                    return "Personal Care & Beauty";
                case "homeFurnitureAndAppliances":
                    return "Home, Furniture and Appliances";
                case "sportsAndOutdoors":
                    return "Sports & Outdoors";
                case "toysAndGames":
                    return "Toys & Video Games";
                case "electronics":
                    return "Electronics";
                case "baby":
                    return "Baby";
                case "pets":
                    return "Pet Shop";
                case "gardenAndAuto":
                    return "Garden & Auto";
                case "booksAndMoviesAndMusic":
                    return "Books,Music and Movies";
                default:
                    return categoryName; 
            }
        }

        public string GetSubCategoryDisplayName(string subcategoryName)
        {
            string subcategoryDisplayName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(subcategoryName.Replace('-', ' '));
            if(subcategoryDisplayName == "Non Alcoholic Drinks")
            {
                subcategoryDisplayName = "Non-Alcoholic Drinks";
            }
            return subcategoryDisplayName;
        }

        public string GetNestedCategoryDisplayName(string nestedCategoryName)
        {
            string nestedCategoryDisplayName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nestedCategoryName.Replace('-', ' '));
            switch (nestedCategoryDisplayName)
            {
                case "Non Alcoholic Beer" :
                    return "Non-alcoholic Beer";
                case "E Cigarettes":
                    return "E-Cigarettes";
                case "E Toothbrush":
                    return "E-Toothbrush";
                case "0 5 Months":
                    return "0-5 Months";
                case "6 11 Months":
                    return "6-11 Months";
                case "12 23 Months":
                    return "12-23 Months";
                case "24 Months":
                    return "24+ Months";
                case "Sci Fi Books":
                    return "Sci-Fi Books";
                default:
                    return nestedCategoryDisplayName;
            }
        }

    }
}
