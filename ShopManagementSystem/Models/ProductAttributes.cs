namespace ShopManagementSystem.Models
{
    public class ProductAttributes
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public string AttributeKey { get; set; } = string.Empty;
        public string AttributeValue { get; set; } = string.Empty;

        public Product Product { get; set; }
    }
}
