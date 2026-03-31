using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tebenkova_Services.DTOs
{
    public class SupplierDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Rating { get; set; }
        public int ProductsCount { get; set; }
    }

    public class SupplierDetailDto : SupplierDto
    {
        public string INN { get; set; }
        public string Address { get; set; }
        public List<ProductDto> Products { get; set; }
    }

    public class SupplierStatisticsDto
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int TotalProducts { get; set; }
        public decimal TotalStockValue { get; set; }
        public decimal AverageProductPrice { get; set; }
        public Dictionary<string, int> ProductsByType { get; set; }
        public int Rating { get; set; }
    }
}
