using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace ShopManagementSystem.Pages.Account
{
    public class SignUpModel : PageModel
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly RoleManager<UsersRole> _roleManager;
        private readonly ShopManagementSystem.Data.ShopManagementSystemContext _context;
        public SignUpModel(UserManager<Users> userManager, SignInManager<Users> signInManager, RoleManager<UsersRole> roleManager, ShopManagementSystem.Data.ShopManagementSystemContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string? Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string? Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string? ConfirmPassword { get; set; }

         
        }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Check if the "RegularUser" role already exists
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                // Create the "RegularUser" role
                var role = new UsersRole { Name = "User" };
                await _roleManager.CreateAsync(role);
            }

            // Check if the "Admin" role already exists
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                // Create the "Admin" role
                var role = new UsersRole { Name = "Admin" };
                await _roleManager.CreateAsync(role);
            }

           
            if (ModelState.IsValid)
            {
                var user = new Users { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    await _userManager.AddToRoleAsync(user, "User");
                    _context.SaveChanges();
                    return RedirectToPage("/Index");
                }

                

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

           
            return Page();
        }
    }
}
