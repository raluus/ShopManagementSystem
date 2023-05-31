namespace ShopManagementSystem.Models
{
    public class Reviews
    { 
        public int Id { get; set; }

        public string? UserId { get; set; }

        public int ProductId { get; set; }

        public string? ReviewText { get; set; }

        public int Rating { get; set; }

        public Users? User { get; set; }

        public Product? Product { get; set; }
    }
}
