namespace ShopManagementSystem.Models
{
    public class ProductCategory
    {
     
        public int Id { get; set; }
        public int ProductId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public Product? Product { get; set; }
    }
}
