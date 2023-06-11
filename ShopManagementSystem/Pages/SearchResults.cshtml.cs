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
    public class SearchResultsModel : PageModel
    {
        private readonly ShopManagementSystem.Data.ShopManagementSystemContext _context;
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public SearchResultsModel(ShopManagementSystem.Data.ShopManagementSystemContext context)
        {
            _context = context;
        }
        [BindProperty]
        public List<Product> Product { get; set; } = default!;

        [BindProperty]
        public List<ProductInventory> ProductInventory { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync()
        {
            SearchString = HttpContext.Session.GetString("SearchString");
            var products = from m in _context.Product
                           select m;
            var productInventory = from m in _context.ProductInventory select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                products = products.Where(s => s.ProductName.Contains(SearchString));
            }

            Product = await products.ToListAsync();
            ProductInventory = await productInventory.ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostSearchResult()
        {
            HttpContext.Session.Clear();
            SearchString = Request.Form["searchString"];
            var products = from m in _context.Product
                          select m;
            var productInventory = from m in _context.ProductInventory select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                products = products.Where(s => s.ProductName.Contains(SearchString));
            }

            Product = await products.ToListAsync();
            ProductInventory = await productInventory.ToListAsync();
            HttpContext.Session.SetString("SearchString", SearchString);

            return Page();
        }
    }
}
