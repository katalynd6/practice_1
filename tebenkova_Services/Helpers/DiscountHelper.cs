using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tebenkova_Models.Models;

namespace tebenkova_Services.Helpers
{
    public static class DiscountHelper
    {
        public static int CalculateDiscount(Partner partner)
        {
            if (partner == null)
                return 0;

            if (partner.SalesHistories == null || !partner.SalesHistories.Any())
                return 0;

            // Суммируем общую сумму продаж (количество * цена)
            decimal totalSalesAmount = partner.SalesHistories.Sum(sh => sh.Quantity * sh.SalePrice);

            if (totalSalesAmount >= 300000)
                return 15;
            else if (totalSalesAmount >= 50000)
                return 10;
            else if (totalSalesAmount >= 10000)
                return 5;
            else
                return 0;
        }
        public static string GetDiscountDescription(int discountPercent)
        {
            return discountPercent > 0 ? $"Скидка {discountPercent}%" : "Нет скидки";
        }

        public static int CalculateDiscount(IEnumerable<SaleHistory> sales)
        {
            if (sales == null || !sales.Any())
                return 0;

            decimal totalSales = sales.Sum(s => s.Quantity * s.SalePrice);

            if (totalSales >= 300000) return 15;
            if (totalSales >= 50000) return 10;
            if (totalSales >= 10000) return 5;
            return 0;
        }
    }
}
