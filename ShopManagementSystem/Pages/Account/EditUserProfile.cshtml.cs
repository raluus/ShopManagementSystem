using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Data;
using ShopManagementSystem.Models;

namespace ShopManagementSystem.Pages.Account
{
    public class EditUserProfileModel : PageModel
    {
        private readonly ShopManagementSystem.Data.ShopManagementSystemContext _context;
        private readonly UserManager<Users> _userManager;

        public EditUserProfileModel(ShopManagementSystem.Data.ShopManagementSystemContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Users Users { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

                Users = await _context.Users.FirstOrDefaultAsync(m => m.Id == user.Id);
                if (Users == null)
                {
                    return NotFound();
                }
            }
            else
            {
                return RedirectToPage("/Account/Login");
            }
            return Page();
        }


        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(m => m.Id == Users.Id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.UserName = Users.UserName;
            existingUser.FirstName = Users.FirstName;
            existingUser.LastName = Users.LastName;
            existingUser.Address = Users.Address;
            existingUser.PhoneNumber = Users.PhoneNumber;
            existingUser.ZipCode = Users.ZipCode;
            existingUser.City = Users.City;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(Users.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Index");
        }


        private bool UsersExists(string id)
        {
          return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
