using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Models;
using ShopManagementSystem.Models.Account;

namespace ShopManagementSystem.Data
{
    public class ShopManagementSystemContext : DbContext
    {
        public ShopManagementSystemContext (DbContextOptions<ShopManagementSystemContext> options)
            : base(options)
        {
        }
        public DbSet<ShopManagementSystem.Models.Product> Product { get; set; } = default!;
        public DbSet<ShopManagementSystem.Models.Account.SignUpM> SignUpM { get; set; } = default!;
        public DbSet<ShopManagementSystem.Models.Account.LoginM> LoginM { get; set; } = default!;

        
    }
}
