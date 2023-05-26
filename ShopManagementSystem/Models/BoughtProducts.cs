namespace ShopManagementSystem.Models
{
    public class BoughtProducts
    {
        public int Id { get; set; }

        public int PaymentDetailsId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public float TotalPrice { get; set; }

        public PaymentDetails? PaymentDetails { get; set; }

        public Product? Product { get; set; }

       
    }
}
