namespace ShopManagementSystem.Models
{
    public class ProductSubCategory
    {
        public int Id {get;set;}

        public int ProductId { get; set; }

        public string SubCategoryName { get; set; } = string.Empty;

        public Product? Product { get; set; }
    }
}
