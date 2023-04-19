using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Data;
using ShopManagementSystem.Models;

namespace ShopManagementSystem.Pages.UserCart
{
    public class IndexModel : PageModel
    {
        private readonly ShopManagementSystem.Data.ShopManagementSystemContext _context;

        public IndexModel(ShopManagementSystem.Data.ShopManagementSystemContext context)
        {
            _context = context;
        }

        public IList<Cart> Cart { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Cart != null)
            {
                Cart = await _context.Cart.ToListAsync();
            }
        }
    }
}
