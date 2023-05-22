using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagementSystem.Models;

namespace ShopManagementSystem.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserManager<Users> _userManager;

        public IndexModel(ILogger<IndexModel> logger, UserManager<Users> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

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
            return Page();
        }
    }
}