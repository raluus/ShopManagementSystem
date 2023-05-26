using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using ShopManagementSystem.Data;
using ShopManagementSystem.Models;

namespace ShopManagementSystem.Pages.Carts
{
    public class UserCartModel : PageModel
    {
        private readonly ShopManagementSystem.Data.ShopManagementSystemContext _context;
        private readonly UserManager<Users> _userManager;
        private readonly IEmailSender _emailSender;

        public UserCartModel(ShopManagementSystem.Data.ShopManagementSystemContext context, UserManager<Users> userManager,IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
        }
        [BindProperty]
        public Cart Cart { get; set; } = default!;
        [BindProperty]
        public List<CartProduct> CartProducts { get; set; } = default!;

        [BindProperty]
        public List<Product> Product { get; set; } = default!;

        [BindProperty]
        public List<ProductInventory> ProductInventory { get; set; } = default!;

        [BindProperty]
        public PaymentDetails PaymentDetails { get; set; } = default!;

        private float SubtotalValue = 0;
        private float TvaValue = 0;
        private string paymentMethod = string.Empty;

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
            var error = 0;
            var productName = "";
            var prodId = 0;
            foreach (var key in Request.Form.Keys)
            {
                if (key.StartsWith("selectedQuantities["))
                {
                    int productId = int.Parse(key.Substring(19, key.Length - 20));
                    int quantity = int.Parse(Request.Form[key]);

                    selectedQuantities.Add(productId, quantity);
                }
            }

            foreach (var item in selectedQuantities)
            {

                var productQuantity = await _context.ProductInventory.FirstOrDefaultAsync(pi => pi.ProductId == item.Key);
                var product = await _context.Product.FirstOrDefaultAsync(p => p.Id ==item.Key);
                if (productQuantity.Quantity - item.Value < 0)
                {
                    prodId = item.Key;
                    productName = product.ProductName;
                    error = 1;
                    break;
                }
            }
            if (error == 0)
            {
                foreach (var item in selectedQuantities)
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

                await AddPaymentDetails(user);

                await SendEmail(user);
                return RedirectToPage("/Carts/PaymentSuccess");
            }
            else
            {
                var productInventory = await _context.ProductInventory.FirstOrDefaultAsync(pi => pi.ProductId == prodId);
                productInventory.Status = 1;
                var user = await _userManager.GetUserAsync(User);
                var cart = await _context.Cart
               .Include(c => c.Products)
               .FirstOrDefaultAsync(c => c.UserId == user.Id);


                if (cart != null)
                {
                    var cartProducts = cart.Products;
                    var itemToRemove = await _context.CartProduct.FirstOrDefaultAsync(cp => cp.ProductId == prodId && cp.CartId == cart.Id);
                    if (itemToRemove != null)
                    {
                        _context.CartProduct.Remove(itemToRemove);
                        await _context.SaveChangesAsync();
                    }
                    if (cartProducts.Count() == 0)
                    {
                        _context.Cart.Remove(cart);
                        await _context.SaveChangesAsync();
                    }

                }
                await _context.SaveChangesAsync();
                return RedirectToPage("/Carts/PaymentError", new { productName = productName });
            }

        }

        private async Task AddPaymentDetails(Users? user)
        {
            SubtotalValue = float.Parse(Request.Form["SubtotalValue"]);
            TvaValue = float.Parse(Request.Form["TvaValue"]);
            paymentMethod = Request.Form["deliveryMethod"];

            PaymentDetails.TotalPriceWithoutTva = SubtotalValue - TvaValue;
            PaymentDetails.TotalPriceWithTva = SubtotalValue;
            PaymentDetails.UserId = user.Id;
            PaymentDetails.DateOfPayment = DateTime.Now;
            if (paymentMethod == "courier")
                PaymentDetails.TypeOfDelivery = 0;
            else
                PaymentDetails.TypeOfDelivery = 1;
            PaymentDetails.PayedProducts = new List<BoughtProducts>();
            foreach (var item in selectedQuantities)
            {
                var productPrice = await _context.ProductInventory.FirstOrDefaultAsync(pp => pp.ProductId == item.Key);
                PaymentDetails.PayedProducts.Add(new BoughtProducts
                {
                    PaymentDetailsId = PaymentDetails.Id,
                    ProductId = item.Key,
                    Quantity = item.Value,
                    TotalPrice = productPrice.RetailPrice * item.Value,
                });

                await _context.SaveChangesAsync();
            }
            _context.PaymentDetails.Add(PaymentDetails);
            await _context.SaveChangesAsync();
        }

        private async Task SendEmail(Users? user)
        {
            StringBuilder bodyMessage = new StringBuilder();

            string pluralSuffix = ""; 

           
            bodyMessage.AppendLine("<html><body>");
            bodyMessage.AppendLine("<div style='text-align: center; font-family: Arial, sans-serif;'>");
            bodyMessage.AppendLine("<h1 style='color: #333;'>Payment Details</h1>");
            bodyMessage.AppendLine("<p style='font-size: 16px;'>Thank you for shopping from <span style='background-color: lightyellow; color: black;'>Lemon</span>!</p>");

            
            bodyMessage.AppendLine("<p style='font-size: 16px;'>Payment Date: " + PaymentDetails.DateOfPayment.ToString() + "</p>");

            bodyMessage.AppendLine("<table style='border-collapse: collapse; margin: 0 auto; width: 80%;'>");
            bodyMessage.AppendLine("<tr>");
            bodyMessage.AppendLine("<th style='background-color: lightyellow; color: black; padding: 10px;'>Product</th>");
            bodyMessage.AppendLine("<th style='background-color: lightyellow; color: black; padding: 10px;'>Quantity/Price Unit</th>");
            bodyMessage.AppendLine("<th style='background-color: lightyellow; color: black; padding: 10px;'>Price (1)</th>");
            bodyMessage.AppendLine("<th style='background-color: lightyellow; color: black; padding: 10px;'>Total Price</th>");
            bodyMessage.AppendLine("</tr>");


            foreach (var item in selectedQuantities)
            {
                var product = await _context.Product.FirstOrDefaultAsync(p => p.Id == item.Key);
                var productInventory = await _context.ProductInventory.FirstOrDefaultAsync(p => p.ProductId == item.Key);
                switch (product.PriceUnit)
                {
                    case "Each":
                       
                        break;
                    case "Kilogram":
                    case "Bag":
                       
                        pluralSuffix = "s";
                        break;
                    case "Box":
                   
                        pluralSuffix = "es";
                        break;
                }

                bodyMessage.AppendLine("<tr>");
                bodyMessage.AppendLine($"<td style='border: 1px solid #ccc; padding: 10px;'>{product.ProductName}</td>");
                bodyMessage.AppendLine($"<td style='border: 1px solid #ccc; padding: 10px;'>{item.Value} {product.PriceUnit}({pluralSuffix})</td>");
                bodyMessage.AppendLine($"<td style='border: 1px solid #ccc; padding: 10px;'>${productInventory.RetailPrice}</td>");
                bodyMessage.AppendLine($"<td style='border: 1px solid #ccc; padding: 10px;'>${productInventory.RetailPrice * (decimal)item.Value}</td>");
                bodyMessage.AppendLine("</tr>");
            }

            bodyMessage.AppendLine("</table>");

            // Append the total price and TVA with updated styles
            bodyMessage.AppendLine("<p style='text-align: center; margin-top: 20px;'>");
            bodyMessage.AppendLine("<strong style='font-size: 18px;'>Total Price:</strong><br>");
            bodyMessage.AppendLine("<span style='font-size: 18px; color: #333; font-weight: bold;'>$" + SubtotalValue  + " (includes transport price +$"+ (PaymentDetails.TypeOfDelivery == 0 ? 25 : 15) + ")</span>");
            bodyMessage.AppendLine("</p>");

            bodyMessage.AppendLine("<p style='text-align: center;'>");
            bodyMessage.AppendLine("<strong style='font-size: 18px;'>TVA:</strong><br>");
            bodyMessage.AppendLine("<span style='font-size: 18px; color: #333; font-weight: bold;'>$" + TvaValue + "</span>");
            bodyMessage.AppendLine("</p>");

            // Add estimated arrival message based on delivery type
            if (PaymentDetails.TypeOfDelivery == 0)
            {
                bodyMessage.AppendLine("<p style='font-size: 16px;'>Your products will arrive shortly via Express Courier.</p>");
            }
            else
            {
                bodyMessage.AppendLine("<p style='font-size: 16px;'>Your products will arrive shortly via Post Fairy.</p>");
            }

            bodyMessage.AppendLine("<p style='font-size: 16px;'>We appreciate you choosing us and hope you enjoy your purchase.</p>");
            bodyMessage.AppendLine("<p style='font-size: 16px;'>If you have any questions or need further assistance, please feel free to contact us.</p>");

            await _emailSender.SendEmailAsync(user.Email, "Payment Details",bodyMessage.ToString());
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
