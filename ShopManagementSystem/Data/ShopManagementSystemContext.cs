using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Models;

namespace ShopManagementSystem.Data
{
    public class ShopManagementSystemContext : IdentityDbContext<Users>
    {
        public ShopManagementSystemContext (DbContextOptions<ShopManagementSystemContext> options)
            : base(options)
        {
        }
        public DbSet<ShopManagementSystem.Models.Product> Product { get; set; } = default!;
        public DbSet<ShopManagementSystem.Models.UsersRole> UsersRole { get; set; } = default!;
        public DbSet<ShopManagementSystem.Models.Cart> Cart { get; set; } = default!;
       

    

    }
}
