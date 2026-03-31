using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tebenkova_Services.DTOs
{
    public class DashboardSummaryDto
    {
        public int TotalPartners { get; set; }
        public int TotalProducts { get; set; }
        public int TotalSuppliers { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalTransactions { get; set; }
        public decimal AverageTransactionValue { get; set; }
        public List<string> TopPartners { get; set; }
        public int LowStockProducts { get; set; }
    }

    public class SalesReportDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalTransactions { get; set; }
        public decimal AverageDailySales { get; set; }
        public List<ProductSalesDto> TopProducts { get; set; }
    }
}
