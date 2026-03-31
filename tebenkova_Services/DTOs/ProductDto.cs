using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tebenkova_Services.DTOs
{
    public class ProductDto
    {
        public string ProductType { get; set; }
        public string Unit { get; set; }
        public decimal QuantityInStock { get; set; }
        public decimal MinStockQuantity { get; set; }
        public string StockStatus { get; set; }
        public string SupplierName { get; set; }
        public string Location { get; set; }
        public decimal? CostPrice { get; set; }
        public int Id { get; set; }
        public string Article { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

    }

    public class StockSummaryDto
    {
        public int TotalProducts { get; set; }
        public decimal TotalItemsInStock { get; set; }
        public int LowStockItems { get; set; }
        public int OutOfStockItems { get; set; }
        public decimal TotalStockValue { get; set; }
        public Dictionary<string, int> ProductsByType { get; set; }
    }

    public class ProductSalesDto
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
