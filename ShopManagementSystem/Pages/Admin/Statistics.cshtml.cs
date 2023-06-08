using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
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
            int minSupport = 2;
            double minConfidence = 0.3;

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
            await GetProductNames(apriori);

            var chartData = new
            {
                labels = AntecedentProductNames.Zip(ConsequentProductNames, (antecedent, consequent) => $"{antecedent} => {consequent}"),
                supportData = apriori.associationRules.Select(rule => rule.Support),
                confidenceData = apriori.associationRules.Select(rule => rule.Confidence)
            };

           var productsSold = _context.BoughtProducts
           .GroupBy(bp => bp.ProductId)
           .Select(group => new
           {
               ProductId = group.Key,
               numberOfProductsSold = group.Sum(bp => bp.Quantity)
           })
          .ToList();
            var productIds = productsSold.Select(p => p.ProductId).ToList();
            var productNames = _context.Product
                .Where(p => productIds.Contains(p.Id))
                .Select(p => new { p.Id, p.ProductName })
                .ToList();
            var productLabel = productNames.ToDictionary(p => p.Id, p => p.ProductName);

            var productChartData = new
            {
                labels = productsSold.Select(p => productLabel[p.ProductId]),
                productData = productsSold.Select(p => p.numberOfProductsSold)
            };

            var totalSalesPerDay = paymentDetailsList
            .GroupBy(pd => pd.DateOfPayment.Date)
            .Select(g => new { Date = g.Key, TotalProfit = g.Sum(pd => pd.TotalPriceWithoutTva) })
            .ToList();

            var totalSalesChartData = new
            {
                labels = totalSalesPerDay.Select(p => p.Date.ToString("yyyy-MM-dd")),
                totalSalesData = totalSalesPerDay.Select(p => p.TotalProfit)
            };

            ViewData["ChartData"] = chartData;
            ViewData["productChartData"] = productChartData;
            ViewData["totalSalesChartData"] = totalSalesChartData;
        }

        private async Task GetProductNames(Apriori apriori)
        {
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
        }
    }
}
