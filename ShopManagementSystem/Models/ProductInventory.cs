using System.Data.SqlTypes;

namespace ShopManagementSystem.Models
{
    public class ProductInventory
    {
        public ProductInventory()
        {
            Product = new Product();
        }
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }

        public string Location { get; set; } = string.Empty;
        public string BatchNumber { get; set; }= string.Empty;
        public string SerialNumber { get; set; }= string.Empty;
        public int ReorderPoint { get; set; }
        public int SafetyStock { get; set; }

        public DateTime LastUpdated { get; set; }

        public string Supplier { get; set; } = string.Empty;
        public decimal Cost { get; set; }

        public decimal RetailPrice { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int Status { get; set; } 

        public string Notes { get; set; } = string.Empty;

        public Product Product { get; set; }


    }
}
