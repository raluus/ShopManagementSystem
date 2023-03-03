using System.ComponentModel.DataAnnotations;

namespace ShopManagementSystem.Models.Account
{
    public class LoginM
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Username")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Please enter your password")]
        public string? Password { get; set; }
        [Display(Name = "Active")]
        public bool IsActive { get; set; }
        [Display(Name="Remember me")]
        public bool IsRemember { get; set; }

    }
}
