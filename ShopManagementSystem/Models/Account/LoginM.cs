using System.ComponentModel.DataAnnotations;

namespace ShopManagementSystem.Models.Account
{
    public class LoginM
    {
        [Key]
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool IsActive { get; set; }
        [Display(Name="Remember me")]
        public bool IsRemember { get; set; }

    }
}
