﻿using System;
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
    public class LoginModel : PageModel
    {
        private readonly ShopManagementSystem.Data.ShopManagementSystemContext _context;

        public LoginModel(ShopManagementSystem.Data.ShopManagementSystemContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public LoginM LoginM { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.LoginM == null || LoginM == null)
            {
                return Page();
            }

            _context.LoginM.Add(LoginM);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
