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
        public Cart Cart { get; set; } = default!;
        [BindProperty]
        public List<CartProduct> CartProducts { get; set; } = default!;

        [BindProperty]
        public List<Product> Product { get; set; } = default!;

        [BindProperty]
        public List<ProductInventory> ProductInventory { get; set; } = default!;

        private string SubtotalValue = string.Empty;
        private string TvaValue = string.Empty;

        private Dictionary<int, int> selectedQuantities = new();
        

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
                    Cart = cart;
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


            foreach (var key in Request.Form.Keys)
            {
                if (key.StartsWith("selectedQuantities["))
                {
                    int productId = int.Parse(key.Substring(19, key.Length - 20)); 
                    int quantity = int.Parse(Request.Form[key]);

                    selectedQuantities.Add(productId, quantity);
                }
            }

            foreach(var item in selectedQuantities)
            {
                var productQuantity = await _context.ProductInventory.FirstOrDefaultAsync(pi => pi.ProductId == item.Key);
                productQuantity.Quantity -= item.Value;
                await _context.SaveChangesAsync();
            }

            var user = await _userManager.GetUserAsync(User);
            var cartItems = await _context.Cart
            .Where(c => c.UserId == user.Id)
            .ToListAsync();

            if (cartItems != null && cartItems.Count > 0)
            {
                _context.Cart.RemoveRange(cartItems);
                await _context.SaveChangesAsync();
            }


            SubtotalValue = Request.Form["SubtotalValue"];
            TvaValue = Request.Form["TvaValue"];
            

            return RedirectToPage("/Carts/PaymentSuccess");
            
        }

        public async Task OnPostRemoveAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var productId = Request.Form["productId"];


            var cart = await _context.Cart
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);
           
            
            if (cart != null)
            {
                var cartProducts = cart.Products;
                var itemToRemove = await _context.CartProduct.FirstOrDefaultAsync(cp => cp.ProductId == int.Parse(productId) && cp.CartId == cart.Id);
                if (itemToRemove != null)
                {
                    _context.CartProduct.Remove(itemToRemove);
                    await _context.SaveChangesAsync();
                }
                if(cartProducts.Count() == 0)
                {
                    _context.Cart.Remove(cart);
                    await _context.SaveChangesAsync();
                }

            }

           
        }

   

    }

   
}
