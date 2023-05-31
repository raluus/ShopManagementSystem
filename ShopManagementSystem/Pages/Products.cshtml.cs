using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using ShopManagementSystem.Data;
using ShopManagementSystem.Models;
using static NuGet.Packaging.PackagingConstants;

namespace ShopManagementSystem.Pages
{
    public class ProductsModel : PageModel
    {
        private readonly ShopManagementSystem.Data.ShopManagementSystemContext _context;

        [BindProperty(SupportsGet = true)] 
        public string Category { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public List<string> Subcategory { get; set; } = new List<string>();

        [BindProperty(SupportsGet = true)]
        public List<string> NestedCategory { get; set; } = new List<string>();

        public ProductsModel(ShopManagementSystem.Data.ShopManagementSystemContext context)
        {
            _context = context;
            FilteredProducts = new List<Product>();
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

        public List<Product> FilteredProducts { get; set; }

        public async Task OnGetAsync()
        {
            var productInventory = from m in _context.ProductInventory
                                   select m;
            ProductInventory = await productInventory.ToListAsync();
            if (FilteredProducts.Count > 0)
            {
                Product = FilteredProducts;
            }
            else
            {
                Product = new List<Product>();
                if (_context.Product != null)
                {
                    if (Category != "" && Subcategory.IsNullOrEmpty())
                    {
                        Product = await _context.Product
                       .Join(
                        _context.ProductCategory.Where(pc => pc.CategoryName == GetCategoryDisplayName(Category)),
                        p => p.Id,
                        pc => pc.ProductId,
                        (p, pc) => p
                        )
                        .ToListAsync();
                    }
                    else if (!Subcategory.IsNullOrEmpty() && NestedCategory.IsNullOrEmpty())
                    {
                        Product = await _context.Product
                       .Join(
                        _context.ProductSubCategory.Where(pc => pc.SubCategoryName == GetSubCategoryDisplayName(Subcategory.FirstOrDefault())),
                        p => p.Id,
                        pc => pc.ProductId,
                        (p, pc) => p
                        )
                        .ToListAsync();
                    }
                    else
                    {
                        Product = await _context.Product
                      .Join(
                       _context.ProductNestedCategory.Where(pc => pc.NestedCategoryName == GetNestedCategoryDisplayName(NestedCategory.FirstOrDefault())),
                       p => p.Id,
                       pc => pc.ProductId,
                       (p, pc) => p
                       )
                       .ToListAsync();
                    }
                    
                }
            }
        }

        public async Task<IActionResult> OnPostFilterProductsAsync(List<string> checkboxFiltersForSubcategory,List<string> checkboxFiltersForNestedCategory,List<string> checkboxFiltersForBrand)
        {
            checkboxFiltersForSubcategory = Request.Form["subcategoryFilters"].ToList();
            checkboxFiltersForNestedCategory = Request.Form["nestedCategoryFilters"].ToList();
            checkboxFiltersForBrand = Request.Form["brandFilters"].ToList();
            bool done = false;
            if (checkboxFiltersForSubcategory.Count > 0)
            {
                Category = Request.Form["Category"];
                foreach(var subcategory in checkboxFiltersForSubcategory)
                {
                    Subcategory.Add(subcategory);
                }
                FilteredProducts = await GetFilteredProductsAsync(checkboxFiltersForSubcategory);
                if(checkboxFiltersForBrand.Count > 0)
                {
                    done = true;
                    List<Product> filteredProductsBrand = await _context.Product.Where(p => checkboxFiltersForBrand.Contains(p.ProductBrand)).ToListAsync();
                    if (filteredProductsBrand.Count != 0)
                    {
                        FilteredProducts = FilteredProducts
                        .Where(p => filteredProductsBrand.Contains(p)).ToList();
                    }
                }
            }
            if(checkboxFiltersForNestedCategory.Count > 0)
            {
                Category = Request.Form["Category"];
                FilteredProducts = await GetFilteredProductsAsync(checkboxFiltersForNestedCategory);
                foreach (var subcategory in Request.Form["Subcategory"])
                {
                    Subcategory.Add(subcategory);
                }
                foreach(var nestedCategory in checkboxFiltersForNestedCategory)
                {
                    NestedCategory.Add(nestedCategory);
                }
                if (checkboxFiltersForBrand.Count > 0)
                {
                    done = true;
                    List<Product> filteredProductsBrand = await _context.Product.Where(p => checkboxFiltersForBrand.Contains(p.ProductBrand)).ToListAsync();
                    if (filteredProductsBrand.Count != 0)
                    {
                        FilteredProducts = FilteredProducts
                        .Where(p => filteredProductsBrand.Contains(p)).ToList();
                    }
                }
            }
            if (checkboxFiltersForBrand.Count > 0 && !done)
            {
                Category = Request.Form["Category"];
                FilteredProducts = await GetFilteredProductsAsync(checkboxFiltersForBrand);
                foreach (var subcategory in Request.Form["Subcategory"])
                {
                    Subcategory.Add(subcategory);
                }
                foreach (var nestedCategory in checkboxFiltersForNestedCategory)
                {
                    NestedCategory.Add(nestedCategory);
                }
            }
            await OnGetAsync();
            return Page();

        }

        private async Task<List<Product>> GetFilteredProductsAsync(List<string> checkboxFilters)
        {
            var product = from m in _context.Product
                                   select m;
            List<Product> filteredProducts = await _context.Product
                   .Join(
                    _context.ProductSubCategory.Where(pc => checkboxFilters.Contains(pc.SubCategoryName)),
                    p => p.Id,
                    pc => pc.ProductId,
                    (p, pc) => p
                    ).ToListAsync();
            if (filteredProducts.Count == 0)
            {
                Subcategory.Clear();
                filteredProducts =  await _context.Product
                      .Join(
                       _context.ProductNestedCategory.Where(pc => checkboxFilters.Contains(pc.NestedCategoryName)),
                       p => p.Id,
                       pc => pc.ProductId,
                       (p, pc) => p
                       ).ToListAsync();
            }
            if(filteredProducts.Count == 0)
            {
                filteredProducts = await _context.Product.Where(p => checkboxFilters.Contains(p.ProductBrand)).ToListAsync();
            }
            return filteredProducts;
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
                    return "Home,Furniture and Appliances";
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
