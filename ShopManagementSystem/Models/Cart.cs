using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ShopManagementSystem.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
