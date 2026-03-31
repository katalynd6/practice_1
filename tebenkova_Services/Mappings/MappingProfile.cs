using tebenkova_Models.Models;
using tebenkova_Services.DTOs;
using System.Linq;

namespace tebenkova_Services.Mappings
{
    public static class MappingProfile
    {
        public static PartnerDto ToDto(this Partner partner, int discount = 0)
        {
            if (partner == null) return null;

            return new PartnerDto
            {
                Id = partner.Id,
                TypeName = partner.PartnerType?.Name,
                Name = partner.Name,
                Phone = partner.Phone,
                Email = partner.Email,
                Rating = partner.Rating,
                Discount = discount,
                DiscountDescription = discount > 0 ? $"{discount}%" : "Нет скидки",
                TotalSales = partner.SalesHistories?.Sum(s => s.Quantity * s.SalePrice) ?? 0
            };
        }

        public static PartnerDetailDto ToDetailDto(this Partner partner)
        {
            if (partner == null) return null;

            return new PartnerDetailDto
            {
                Id = partner.Id,
                TypeName = partner.PartnerType?.Name,
                Name = partner.Name,
                Phone = partner.Phone,
                Email = partner.Email,
                Rating = partner.Rating,
                LegalAddress = partner.LegalAddress,
                INN = partner.INN,
                DirectorName = partner.DirectorName,
                SalesHistories = partner.SalesHistories?.Select(s => s.ToDto()).ToList() // <- исправлено
            };
        }

        public static SaleHistoryDto ToDto(this SaleHistory saleHistory)
        {
            if (saleHistory == null) return null;

            return new SaleHistoryDto
            {
                Id = saleHistory.Id,
                PartnerName = saleHistory.Partner?.Name,
                ProductName = saleHistory.Product?.Name,
                SaleDate = saleHistory.SaleDate,
                Quantity = saleHistory.Quantity,
                Price = saleHistory.SalePrice,
                Total = saleHistory.Quantity * saleHistory.SalePrice
            };
        }

        public static ProductDto ToDto(this Product product)
        {
            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Article = product.Article,
                Name = product.Name,
                ProductType = product.ProductType?.Name,
                Unit = product.Unit,
                QuantityInStock = product.QuantityInStock,
                MinStockQuantity = product.MinStockQuantity,
                StockStatus = GetStockStatus(product),
                SupplierName = product.Supplier?.Name,
                Location = product.Location,
                CostPrice = product.CostPrice
            };
        }

        private static string GetStockStatus(Product product)
        {
            if (product.QuantityInStock <= 0) return "Отсутствует";
            if (product.QuantityInStock < product.MinStockQuantity) return "Меньше минимума";
            return "Норма";
        }
    }
}