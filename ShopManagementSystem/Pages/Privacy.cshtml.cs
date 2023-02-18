using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace ShopManagementSystem.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            string dateTime = DateTime.Now.ToString("d", new CultureInfo("ro-RO"));
            ViewData["TimeStamp"] = dateTime;
        }
    }
}