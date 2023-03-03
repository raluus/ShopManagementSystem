using System.ComponentModel.DataAnnotations;

namespace ShopManagementSystem.Models.Account
{
    public class SignUpM
    {
        [Key]
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public long PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}
