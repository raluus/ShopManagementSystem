using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShopManagementSystem.Pages.Carts
{
    public class PaymentErrorModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string ProductName { get; set; }
        public void OnGet()
        {
        }
    }
}
