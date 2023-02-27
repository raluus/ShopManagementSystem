namespace ShopManagementSystem.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductCategory{ get; set; } = string.Empty;
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
    }
}
