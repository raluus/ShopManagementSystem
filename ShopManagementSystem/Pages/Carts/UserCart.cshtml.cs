using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Data;
using ShopManagementSystem.Models;

namespace ShopManagementSystem.Pages.Carts
{
    public class UserCartModel : PageModel
    {
        private readonly ShopManagementSystem.Data.ShopManagementSystemContext _context;
        private readonly UserManager<Users> _userManager;

        public UserCartModel(ShopManagementSystem.Data.ShopManagementSystemContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [BindProperty]
        public Cart Cart { get;set; } = default!;
        [BindProperty]
        public List<CartProduct> CartProducts { get; set; } = default!;

        [BindProperty]
        public List<Product> Product { get; set; } = default!;

        [BindProperty]
        public List<ProductInventory> ProductInventory { get; set; } = default!;

        private string SubtotalValue { get; set; }

        private string TvaValue { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                var cart = await _context.Cart
                .Include(c => c.Products.OrderBy(p => p.ProductId))
                .FirstOrDefaultAsync(c => c.UserId == user.Id);
                if (cart == null)
                {
                    return Page();
                }
                var productIds = cart.Products.Select(item => item.ProductId).ToList();
                var products = await _context.Product.Where(p => productIds.Contains(p.Id)).OrderBy(p => p.Id).ToListAsync();
                var productInventory = await _context.ProductInventory.Where(pi => productIds.Contains(pi.ProductId)).OrderBy(pi => pi.ProductId).ToListAsync();


                Cart = cart;
                CartProducts = cart.Products;
                Product = products;
                ProductInventory = productInventory;
            }
            else
            {
                return Page();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            SubtotalValue = Request.Form["SubtotalValue"];
            TvaValue = Request.Form["TvaValue"];
          
            return RedirectToPage("/PaymentSuccess");
        }

    }
}
