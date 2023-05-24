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

namespace ShopManagementSystem.Pages.Carts
{
    public class UserCartModel : PageModel
    {
        private readonly ShopManagementSystem.Data.ShopManagementSystemContext _context;

        public UserCartModel(ShopManagementSystem.Data.ShopManagementSystemContext context)
        {
            _context = context;
        }

        public Cart Cart { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart.FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return Page();
            }

            Cart = cart;

            return Page();
        }

        public IActionResult OnPostAddToCart(int productId)
        {
            // Logic for adding the product with the specified ID to the cart
            // You can access the user's cart, update the cart items, etc.

            
        
        
            // Logic to add the product to the user's cart
            // You can access the logged-in user's ID using User.Identity.Name

            // Example logic to add the product to the cart
            //var cartItem = new CartItem
            //{
            //    ProductId = ProductId,
            //    Quantity = 1
            //};

            //// Save the cart item to the database
            //_context.CartItems.Add(cartItem);
            //_context.SaveChanges();
            // Return a JSON response indicating the success status or additional information
            return new JsonResult(new { success = true });
            //return RedirectToPage("/Carts/UserCart");
        }
    }
}
