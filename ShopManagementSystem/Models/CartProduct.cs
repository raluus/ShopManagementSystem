using System.ComponentModel.DataAnnotations;

namespace ShopManagementSystem.Models
{
    public class CartProduct
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CartId { get; set; }

        public int Quantity { get; set; }

        public Product? Product { get; set; }

        public Cart? Cart { get; set; }
    }
}
