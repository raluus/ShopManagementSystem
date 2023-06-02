using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Data;
using ShopManagementSystem.Models;
using System.Data;

namespace ShopManagementSystem.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class StatisticsModel : PageModel
    {
        private readonly ShopManagementSystemContext _context;
        public List<PaymentDetails> PaymentDetails { get; set; } = default!;

        readonly List<string> AntecedentProductNames = new();
        readonly List<string> ConsequentProductNames = new();

        public int TotalNumberOfPayments { get; set; }

        public StatisticsModel(ShopManagementSystemContext context)
        {
            _context = context;
        }
        public async Task OnGet()
        {
            List<List<string>> transactions = new List<List<string>>();
            int minSupport = 3;
            double minConfidence = 0.8;

            var paymentDetailsList = await _context.PaymentDetails
            .Include(pd => pd.PayedProducts)
            .ToListAsync();

            TotalNumberOfPayments = paymentDetailsList.Count;

            foreach (var paymentDetails in paymentDetailsList)
            {
                List<string> transaction = paymentDetails.PayedProducts
                    .Select(bp => bp.ProductId.ToString())
                    .ToList();

                transactions.Add(transaction);
            }

            Apriori apriori = new Apriori(transactions, minSupport, minConfidence);
            apriori.DoApriori();

            foreach (var rule in apriori.associationRules)
            {
                var antecedentNames = new List<string>();
                var consequentNames = new List<string>();

                foreach (var productId in rule.Antecedent)
                {
                    var product = await _context.Product.FindAsync(int.Parse(productId));
                    if (product != null)
                    {
                        antecedentNames.Add(product.ProductName);
                    }
                }

                foreach (var productId in rule.Consequent)
                {
                    var product = await _context.Product.FindAsync(int.Parse(productId));
                    if (product != null)
                    {
                        consequentNames.Add(product.ProductName);
                    }
                }

                var antecedentLabel = string.Join(", ", antecedentNames);
                var consequentLabel = string.Join(", ", consequentNames);

                AntecedentProductNames.Add(antecedentLabel);
                ConsequentProductNames.Add(consequentLabel);
            }

            var chartData = new
            {
                labels = AntecedentProductNames.Zip(ConsequentProductNames, (antecedent, consequent) => $"{antecedent} => {consequent}"),
                supportData = apriori.associationRules.Select(rule => rule.Support),
                confidenceData = apriori.associationRules.Select(rule => rule.Confidence)
            };

            ViewData["ChartData"] = chartData;
        }
    }
}
