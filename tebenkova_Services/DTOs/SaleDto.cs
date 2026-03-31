using System;

namespace tebenkova_Services.DTOs
{
    public class SaleDto
    {
        public DateTime Date { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
    }
}