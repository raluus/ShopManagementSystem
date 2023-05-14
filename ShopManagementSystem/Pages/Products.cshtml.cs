using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Data;
using ShopManagementSystem.Models;

namespace ShopManagementSystem.Pages
{
    public class ProductsModel : PageModel
    {
        private readonly ShopManagementSystem.Data.ShopManagementSystemContext _context;
        [BindProperty(SupportsGet = true)]
        public string Category { get; set; }

        public ProductsModel(ShopManagementSystem.Data.ShopManagementSystemContext context)
        {
            _context = context;
            Category = "grocery";
        }

        public IList<Product> Product { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Product != null)
            {
                Product = await _context.Product.ToListAsync();
            }
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
                case "kids":
                    return "Kids";
                case "pets":
                    return "Pet Shop";
                case "gardenAndAuto":
                    return "Garden, Auto & Brico";
                case "booksAndMoviesAndMusic":
                    return "Books, Movies & Music";
                case "clothesAndAccesories":
                    return "Clothes & Accesories";
                default:
                    return categoryName; 
            }
        }

    }
}
