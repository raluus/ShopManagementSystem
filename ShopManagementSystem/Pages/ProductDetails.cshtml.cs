using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Data;
using ShopManagementSystem.Models;

namespace ShopManagementSystem.Pages
{
    public class ProductDetailsModel : PageModel
    {
        private readonly ShopManagementSystem.Data.ShopManagementSystemContext _context;

        public ProductDetailsModel(ShopManagementSystem.Data.ShopManagementSystemContext context)
        {
            _context = context;
        }

        public Product Product { get; set; } = default!;

        public List<Product> SimilarProducts { get; set; } = default!;

        public ProductInventory ProductInventory { get; set; } = default!;

        public List<ProductInventory> SimilarProductsInventory { get; set; } = default!;

        public ProductCategory ProductCategory { get; set; } = default!;
        public ProductNestedCategory ProductNestedCategory { get; set; } = default!;
        public ProductSubCategory ProductSubCategory { get; set; } = default!;

        public List<ProductAttributes> ProductAttributes { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FirstOrDefaultAsync(m => m.Id == id);
            var productInventory = await _context.ProductInventory.FirstOrDefaultAsync(pi => pi.ProductId == id);
            var productNestedCategory = await _context.ProductNestedCategory.FirstOrDefaultAsync(pn => pn.ProductId == id);
            var productAttributes = await _context.ProductAttributes.Where(pa => pa.ProductId == id).ToListAsync();
            var productSubCategory = await _context.ProductSubCategory.FirstOrDefaultAsync(ps => ps.ProductId == id);
            var productCategory = await _context.ProductCategory.FirstOrDefaultAsync(pc => pc.ProductId== id);
            if (product == null || productInventory == null || productNestedCategory == null || productSubCategory == null || productCategory == null)
            {
                return NotFound();
            }
            else
            {
                Product = product;
                ProductInventory = productInventory;
                ProductNestedCategory = productNestedCategory;
                ProductAttributes = productAttributes;
                ProductCategory = productCategory; 
                ProductSubCategory = productSubCategory;
            }
            var similarProducts = await _context.Product
            .Join(_context.ProductNestedCategory,
             p => p.Id,
             pnc => pnc.ProductId,
             (p, pnc) => new { Product = p, ProductNestedCategory = pnc })
            .Where(joined => joined.ProductNestedCategory.NestedCategoryName == productNestedCategory.NestedCategoryName && joined.Product.Id != id)
            .Select(joined => joined.Product)
            .Take(10)
            .ToListAsync();
            if(similarProducts.Count == 0)
            {
                 similarProducts = await _context.Product
                .Join(_context.ProductSubCategory,
                 p => p.Id,
                 pnc => pnc.ProductId,
             (p, pnc) => new { Product = p, ProductSubCategory = pnc })
            .Where(joined => joined.ProductSubCategory.SubCategoryName == productSubCategory.SubCategoryName && joined.Product.Id != id)
            .Select(joined => joined.Product)
            .Take(10)
            .ToListAsync();
            }

            SimilarProducts = similarProducts;

            var allProductInventory = await _context.ProductInventory.ToListAsync();
            SimilarProductsInventory = allProductInventory;

            return Page();
        }
    }
}
