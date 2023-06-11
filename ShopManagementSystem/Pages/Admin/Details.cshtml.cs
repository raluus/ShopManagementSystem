using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Data;
using ShopManagementSystem.Models;

namespace ShopManagementSystem.Pages.Products
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly ShopManagementSystem.Data.ShopManagementSystemContext _context;

        public DetailsModel(ShopManagementSystem.Data.ShopManagementSystemContext context)
        {
            _context = context;
        }

      public Product Product { get; set; } = default!;
      public ProductCategory ProductCategory { get; set; } = default!; 
      public ProductSubCategory ProductSubCategory { get; set; } = default!;
      public ProductNestedCategory ProductNestedCategory { get; set; } = default!;
      public IList<ProductAttributes> ProductAttributes { get; set; } = default!;
      public ProductInventory ProductInventory { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            else 
            {
                Product = product;
                var productCategory = await _context.ProductCategory.FirstOrDefaultAsync(pc => pc.ProductId == product.Id);
                var productSubCategory = await _context.ProductSubCategory.FirstOrDefaultAsync(pc => pc.ProductId == product.Id);
                var productNestedCategory = await _context.ProductNestedCategory.FirstOrDefaultAsync(pc => pc.ProductId == product.Id);
                var productInventory = await _context.ProductInventory.FirstOrDefaultAsync(pc => pc.ProductId == product.Id);
                if (productCategory == null || productSubCategory == null || productInventory == null)
                {
                    return NotFound();
                }
                else
                {
                    ProductCategory = productCategory;
                    ProductSubCategory = productSubCategory;
                    ProductNestedCategory = productNestedCategory;
                    ProductInventory = productInventory;

                    ProductAttributes = await _context.ProductAttributes
                   .Where(pa => pa.ProductId == product.Id)
                   .ToListAsync();
                }
                
            }
            return Page();
        }
    }
}
