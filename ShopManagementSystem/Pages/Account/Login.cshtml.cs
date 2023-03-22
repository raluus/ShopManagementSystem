using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagementSystem.Data;
using ShopManagementSystem.Models;

namespace ShopManagementSystem.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ShopManagementSystem.Data.ShopManagementSystemContext _context;

        private string Msg;

        public LoginModel(ShopManagementSystem.Data.ShopManagementSystemContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        { 
            return Page();
        }
        public IActionResult OnGetLogout()
        {
            HttpContext.Session.Remove("username");
            return RedirectToPage("./Index");
        }

      

        [BindProperty]
        public new User User { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          

                var user = Login( User.Username, User.Password);
                if (user == null)
                {
                    Msg = "Invalid account!";
                    return Page();
                }
                else
                {
                    HttpContext.Session.SetString("username", user.Username);
                    return RedirectToPage("../Welcome");

                }
            

        }

        private User? Login(string username, string password)
        {
            var user = _context.User.SingleOrDefault(a => a.Username.Equals(username));
            if (user != null) {

                if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    return user;
                }
            }
            return null;
        }
    }
}
