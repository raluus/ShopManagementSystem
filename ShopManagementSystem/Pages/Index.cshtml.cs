using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Data;
using ShopManagementSystem.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ShopManagementSystem.Pages
{
    public partial class IndexModel : PageModel
    {
        private readonly ShopManagementSystemContext _context;
        private readonly UserManager<Users> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly int maxCartLimit = 10;
        public List<string> MainCategories { get; set; }

        public IndexModel(ShopManagementSystemContext context, UserManager<Users> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;   
        }

        public IList<ProductCategory> ProductCategory { get; set; } = default!;
        public IList<Product> Product { get; set; } = default!;
        public IList<ProductInventory> ProductInventory { get; set; } = default!;

        public Cart Cart { get; set; }= default!;
        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Admin"))
                {
                    return RedirectToPage("/Admin/Index");
                }
            }

            GetJsonData();
            var products = from m in _context.Product
                           select m;
            var productCategory = from m in _context.ProductCategory select m;
            var productInventory = from m in _context.ProductInventory select m;

            var categoryProducts = new List<Product>();

            foreach (var category in MainCategories)
            {
                var limitedProducts = await _context.Product
                   .Join(_context.ProductCategory,
                    p => p.Id,
                    pnc => pnc.ProductId,
                (p, pnc) => new { Product = p, ProductCategory = pnc })
               .Where(joined => joined.ProductCategory.CategoryName == category)
               .Select(joined => joined.Product)
               .Take(10)
               .ToListAsync();
                categoryProducts.AddRange(limitedProducts);
            }

            Product = categoryProducts;
            ProductCategory = await productCategory.ToListAsync();
            ProductInventory = await productInventory.ToListAsync();
            return Page();
        }

        private void GetJsonData()
        {
            var jsonFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Json");
            var jsonFilePath = Path.Combine(jsonFolderPath, "categoryData.json");

            if (System.IO.File.Exists(jsonFilePath))
            {
                var jsonData = System.IO.File.ReadAllText(jsonFilePath);
                var parsedJson = JsonDocument.Parse(jsonData);

                var mainCategoriesArray = parsedJson.RootElement.GetProperty("mainCategories").EnumerateArray();

                MainCategories = new List<string>();

                foreach (var category in mainCategoriesArray)
                {
                    MainCategories.Add(category.GetString());
                }
            }
            else
            {
                MainCategories = new List<string>();
            }
        }


        public async Task<IActionResult> OnPostAddToCart(string productId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                var userId = user.Id;

                var productInventory = await _context.ProductInventory.FirstOrDefaultAsync(pi => pi.ProductId == int.Parse(productId));
                
                var existingCart = await _context.Cart
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.UserId == userId);

                if (existingCart == null)
                {
                   
                    var cart = new Cart
                    {
                        UserId = userId,
                        Products = new List<CartProduct>
                        {
                              new CartProduct
                               {
                                ProductId = int.Parse(productId),
                                Quantity = 1
                               }
                         }
                    };

                   
                    _context.Cart.Add(cart);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var found = false;
                    foreach(var product in existingCart.Products)
                    {
                        if(product.ProductId == int.Parse(productId))
                        {
                            if (product.Quantity <= maxCartLimit)
                            {
                                product.Quantity++;
                                found = true;
                                break;
                            }
                        }
                    }
                    if (!found)
                    {
                        if (existingCart.Products.Count == maxCartLimit)
                        {

                        }
                        else
                        {
                            existingCart.Products.Add(new CartProduct
                            {
                                ProductId = int.Parse(productId),
                                Quantity = 1
                            });
                        }

                    }
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                return RedirectToPage("/Account/Login");
            }

            await OnGetAsync();
         
            string previousPageUrl = Request.Headers["Referer"].ToString();
            return Redirect(previousPageUrl);

        }
    }
}