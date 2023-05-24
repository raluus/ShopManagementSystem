using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopManagementSystem.Models
{
    public class Cart
    {

        [Key]
        public int Id { get; set; }

        public string? UserId { get; set; }

        public Users? User { get; set; }

        public List<CartProduct> Products { get; set; }

        public Cart()
        {
            Products = new List<CartProduct>();
        }

    }
}
