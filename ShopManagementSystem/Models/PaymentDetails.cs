namespace ShopManagementSystem.Models
{
    public class PaymentDetails
    {
        public int Id { get; set; }

        public string? UserId { get; set; }

        public float TotalPriceWithoutTva { get; set; }

        public float TotalPriceWithTva { get; set; }

        public DateTime DateOfPayment { get; set; }

        public Users? User { get; set; }

        public List<BoughtProducts> PayedProducts { get; set; }

        public int? TypeOfDelivery { get; set; }

        public PaymentDetails()
        {
            PayedProducts = new List<BoughtProducts>();
        }
    }
}
