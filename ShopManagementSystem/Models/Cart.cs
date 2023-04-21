using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ShopManagementSystem.Models
{
    public class Cart
    {
        public Cart()
        {
            User = new User();
            Product = new Product();
        }

        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public decimal Quantity { get; set; }

        public User User { get; set; }

        public Product Product { get; set; }

    }
}
