using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagementSystem.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ShopManagementSystem.Pages.Products
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly ShopManagementSystem.Data.ShopManagementSystemContext _context;
        private DateTime currentDateTime = DateTime.MinValue;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly int status = 1;
        private readonly decimal tva19 = decimal.Round(19m / 100m, 2);
        private readonly decimal tva9 = decimal.Round(9m / 100m, 2);
        private readonly decimal tva5 = decimal.Round(5m / 100m, 2);

        public CreateModel(ShopManagementSystem.Data.ShopManagementSystemContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
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

        [BindProperty]
        public ProductAttributes ProductAttributes { get; set; } = default!;

        [BindProperty]
        public ProductInventory ProductInventory { get; set; } = default!;


      
        public async Task<IActionResult> OnPostAsync()
        {
          
            if (!ModelState.IsValid || _context.Product == null || Product == null || _context.ProductCategory == null || ProductCategory == null || _context.ProductSubCategory == null || ProductSubCategory == null || _context.ProductNestedCategory == null || ProductNestedCategory == null)
            {
                return Page();
            }

            currentDateTime = DateTime.Now;
            _context.Product.Add(Product);
            await _context.SaveChangesAsync();

            ProductCategory.ProductId = Product.Id;
            ProductSubCategory.ProductId = Product.Id;
            ProductNestedCategory.ProductId = Product.Id;
            ProductInventory.ProductId = Product.Id;
            ProductInventory.Status = status;
            ProductInventory.LastUpdated = currentDateTime;
            ProductInventory.BatchNumber = GenerateBatchNumber();

            

            if (Request.Form.TryGetValue("attributeKeys", out var attributeKeys) && Request.Form.TryGetValue("attributeValues", out var attributeValues))
            {
                for (var i = 0; i < attributeKeys.Count; i++)
                {
                    var key = attributeKeys[i];
                    var value = attributeValues[i];

                    ProductAttributes = new ProductAttributes
                    {
                        AttributeValue = value,
                        AttributeKey = key,
                        ProductId = Product.Id
                    };
                    _context.ProductAttributes.Add(ProductAttributes);
                    await _context.SaveChangesAsync();

                }
            }

            ProductInventory.RetailPrice = CalculateRetailPrice();

            _context.ProductCategory.Add(ProductCategory);
            _context.ProductSubCategory.Add(ProductSubCategory);
            _context.ProductNestedCategory.Add(ProductNestedCategory);
            _context.ProductInventory.Add(ProductInventory);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

     

        

        private static string GenerateBatchNumber()
        {
            string prefix = "BT";
            Random random = new Random();
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            int randomNumber = random.Next(100000); 
            string batchNumber = prefix + timestamp + randomNumber.ToString().PadLeft(5, '0');
            return batchNumber;
        }

        private decimal CalculateRetailPrice()
        {
            decimal retailPrice = 0;
            string subcategory = ProductSubCategory.SubCategoryName;
            string category = ProductCategory.CategoryName;
            if(category == "Grocery")
            {
                if(subcategory != "Tobacco" && subcategory != "Wine" && subcategory != "Beer" && subcategory != "Cider" && subcategory != "Hard Liquors" && subcategory != "Champagne")
                {
                    retailPrice = decimal.Round(Product.Price * (1 + tva9),2);
                }
            }
            else if(category == "Books")
            {
                retailPrice = decimal.Round(Product.Price * (1 + tva5),2);
            }
            else
            {
                retailPrice = decimal.Round(Product.Price * (1 + tva19), 2);
            }
            return retailPrice;
        }
    }
}
