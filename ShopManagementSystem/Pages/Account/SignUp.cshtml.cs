using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;

namespace ShopManagementSystem.Pages.Account
{
    public class SignUpModel : PageModel
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly RoleManager<UsersRole> _roleManager;
        private readonly ShopManagementSystem.Data.ShopManagementSystemContext _context;
        private readonly ILogger<SignUpModel> _logger;
        private readonly IEmailSender _emailSender;
        public SignUpModel(UserManager<Users> userManager, SignInManager<Users> signInManager, RoleManager<UsersRole> roleManager, ShopManagementSystem.Data.ShopManagementSystemContext context, ILogger<SignUpModel> logger, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _logger = logger;
            _emailSender = emailSender;
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
            [StringLength(10, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
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
           
            if (!await _roleManager.RoleExistsAsync("User"))
            {
              
                var role = new UsersRole { Name = "User" };
                await _roleManager.CreateAsync(role);
            }

           
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                
                var role = new UsersRole { Name = "Admin" };
                await _roleManager.CreateAsync(role);
            }

            if (ModelState.IsValid)
            {
                var user = new Users { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                   
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                   
                    var confirmationLink = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, token = token, email = Input.Email },
                        protocol: Request.Scheme);

                  
                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(confirmationLink)}'>clicking here</a>.");

                    
                   
                    if (_userManager.Options.SignIn.RequireConfirmedEmail)
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                        _context.SaveChanges();
                        return RedirectToPage("/Account/SignUpThankYou");
                    }
                    else
                    {
                       
                        return RedirectToPage("/Index");
                    }
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
