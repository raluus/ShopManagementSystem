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
    public class CreateModel : PageModel
    {
        private readonly ShopManagementSystem.Data.ShopManagementSystemContext _context;

        public CreateModel(ShopManagementSystem.Data.ShopManagementSystemContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        [BindProperty]
        public ProductCategory ProductCategory { get; set; } = default!;

        [BindProperty]
        public ProductSubCategory ProductSubCategory { get; set; } = default!;

        [BindProperty]
        public ProductNestedCategory ProductNestedCategory { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Product == null || Product == null ||   _context.ProductCategory == null || ProductCategory == null ||  _context.ProductSubCategory == null || ProductSubCategory == null ||  _context.ProductNestedCategory == null || ProductNestedCategory == null)
            {
                return Page();
            }

            _context.Product.Add(Product);
            await _context.SaveChangesAsync();

            ProductCategory.ProductId = Product.Id;
            ProductSubCategory.ProductId = Product.Id;
            ProductNestedCategory.ProductId = Product.Id;

            _context.ProductCategory.Add(ProductCategory);
            _context.ProductSubCategory.Add(ProductSubCategory);
            _context.ProductNestedCategory.Add(ProductNestedCategory);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
