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

        public int ProductId { get; set; }

        public decimal Quantity { get; set; }

        public Users User { get; set; }

        public Product Product { get; set; }

    }
}
