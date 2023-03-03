using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagementSystem.Data;
using ShopManagementSystem.Models.Account;

namespace ShopManagementSystem.Pages.Account
{
    public class SignUpModel : PageModel
    {
        private readonly ShopManagementSystem.Data.ShopManagementSystemContext _context;

        public SignUpModel(ShopManagementSystem.Data.ShopManagementSystemContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public new SignUpM SignUpM { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.SignUpM == null || SignUpM == null)
            {
                return Page();
            }

            _context.SignUpM.Add(SignUpM);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
