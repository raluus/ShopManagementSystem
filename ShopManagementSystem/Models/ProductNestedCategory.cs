namespace ShopManagementSystem.Models
{
    public class ProductNestedCategory
    {
        public ProductNestedCategory()
        {
            Product= new Product();
        }
        public int Id { get; set; }

        public int ProductId { get; set; }
        public string NestedCategoryName { get; set; } = string.Empty;

        public Product Product { get; set; }
    }
}
