using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;


namespace ShopManagementSystem.Models
{
    public class Cart
    {
        public Cart()
        {
            User = new Users();
            Product = new Product();
        }

        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public decimal Quantity { get; set; }

        public Users User { get; set; }

        public Product Product { get; set; }

    }
}
