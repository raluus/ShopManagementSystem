using Microsoft.AspNetCore.Identity;

namespace ShopManagementSystem.Models
{
    public class UsersRole : IdentityRole
    {
        public UsersRole() : base() { }

        public UsersRole(string roleName) : base(roleName) { }
    }
}
