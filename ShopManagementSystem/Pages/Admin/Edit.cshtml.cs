using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ShopManagementSystem.Data;
using ShopManagementSystem.Models;

namespace ShopManagementSystem.Pages.Products
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly ShopManagementSystem.Data.ShopManagementSystemContext _context;
        private readonly decimal tva19 = decimal.Round(19m / 100m, 2);
        private readonly decimal tva9 = decimal.Round(9m / 100m, 2);
        private readonly decimal tva5 = decimal.Round(5m / 100m, 2);

        public EditModel(ShopManagementSystem.Data.ShopManagementSystemContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        [BindProperty]
        public ProductCategory ProductCategory { get; set; } = default!;

        [BindProperty]
        public ProductSubCategory ProductSubCategory { get; set; } = default!;

        [BindProperty]
        public ProductNestedCategory ProductNestedCategory { get; set; } = default!;

        [BindProperty]
        public IList<ProductAttributes> ProductAttributes { get; set; } = default!;

        [BindProperty]
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
                if (productCategory == null || productSubCategory == null || productNestedCategory == null || productInventory == null)
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
        public async Task<IActionResult> OnPostAsync()
        {
            ProductCategory.ProductId = Product.Id;
            ProductSubCategory.ProductId = Product.Id;
            ProductNestedCategory.ProductId = Product.Id;
            ProductInventory.ProductId = Product.Id;
            ProductInventory.RetailPrice = CalculateRetailPrice();
            ProductInventory.LastUpdated = DateTime.Now;
            foreach (var productAttribute in ProductAttributes)
            {
                productAttribute.ProductId = Product.Id;
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));

                // Log or display the error messages
                foreach (var error in errors)
                {
                    // You can log the error or display it to the user
                    // For example, you can use TempData to store the error message and display it on the redirected page
                    TempData["ErrorMessage"] = error;
                }
                return Page();
            }

            _context.Attach(Product).State = EntityState.Modified;
            _context.Attach(ProductCategory).State = EntityState.Modified;
            _context.Attach(ProductSubCategory).State = EntityState.Modified;
            _context.Attach(ProductNestedCategory).State = EntityState.Modified;
            foreach (var productAttribute in ProductAttributes)
            {
                _context.Attach(productAttribute).State = EntityState.Modified;
            }
            _context.Attach(ProductInventory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ProductExists(int id)
        {
          return (_context.Product?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private decimal CalculateRetailPrice()
        {
            decimal retailPrice = 0;
            string subcategory = ProductSubCategory.SubCategoryName;
            string category = ProductCategory.CategoryName;
            if (category == "Grocery")
            {
                if (subcategory != "Tobacco" && subcategory != "Wine" && subcategory != "Beer" && subcategory != "Cider" && subcategory != "Hard Liquors" && subcategory != "Champagne")
                {
                    retailPrice = decimal.Round(Product.Price * (1 + tva9), 2);
                }
            }
            else if (category == "Books")
            {
                retailPrice = decimal.Round(Product.Price * (1 + tva5), 2);
            }
            else
            {
                retailPrice = decimal.Round(Product.Price * (1 + tva19), 2);
            }
            return retailPrice;
        }
    }
}
