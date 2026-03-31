using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tebenkova_Services.DTOs
{
    public class SaleHistoryDto
    {
        public int Id { get; set; }
        public string PartnerName { get; set; }
        public string ProductName { get; set; }
        public DateTime SaleDate { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }

    public class DailySalesDto
    {
        public DateTime Date { get; set; }
        public int TransactionsCount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
