using System;
using System.Collections.Generic;

namespace tebenkova_Services.DTOs
{
    public class PartnerDto
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Rating { get; set; }
        public int Discount { get; set; }
        public string DiscountDescription { get; set; }
        public decimal TotalSales { get; set; }
        public List<SaleHistoryDto> SalesHistories { get; set; }
    }

    public class PartnerDetailDto : PartnerDto
    {
        public string LegalAddress { get; set; }
        public string INN { get; set; }
        public string DirectorName { get; set; }
        public List<SaleHistoryDto> SalesHistories { get; set; }
    }

    public class PartnerStatisticsDto
    {
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalTransactions { get; set; }
        public decimal AverageCheck { get; set; }
        public DateTime? FirstPurchaseDate { get; set; }
        public DateTime? LastPurchaseDate { get; set; }
        public int Discount { get; set; }
    }
}