using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Data;
using ShopManagementSystem.Models;

namespace ShopManagementSystem.Pages.Products
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ShopManagementSystemContext _context;

        public IndexModel(ShopManagementSystemContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; } = default!;

        public IList<ProductInventory> ProductInventory { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public async Task OnGetAsync()
        {

            var products = from m in _context.Product
                          select m;
            var productInventory = from m in _context.ProductInventory select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                products = products.Where(s => s.ProductName.Contains(SearchString));
            }

            Product = await products.ToListAsync();
            ProductInventory = await productInventory.ToListAsync();
            //if (_context.Product != null)
            //{
            //    Product = await _context.Product.ToListAsync();
            //}
        }
    }
}
