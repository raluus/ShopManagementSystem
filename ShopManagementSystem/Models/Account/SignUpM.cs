using System.ComponentModel.DataAnnotations;

namespace ShopManagementSystem.Models.Account
{
    public class SignUpM
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Please Enter Username")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Please Enter Email")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}",ErrorMessage ="Please enter Valid Email")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Please Enter Phone Number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please enter Valid Phone Number")]
        [Display(Name = "Phone Number")]
        public long PhoneNumber { get; set; }
        [Required(ErrorMessage = "Please enter your password")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Please enter confirm password")]
        [Compare("Password", ErrorMessage ="The passwords don't match!")]
        [Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set; }
        [Display(Name = "Active")]
        public bool IsActive { get; set; }
        
    }
}
