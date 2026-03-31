using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tebenkova_Models.Interfaces;
using tebenkova_Services.DTOs;

namespace tebenkova_Services.Services
{
    public class ReportService
    {
        private readonly IPartnerRepository _partnerRepository;
        private readonly IProductRepository _productRepository;
        private readonly ISaleHistoryRepository _saleHistoryRepository;
        private readonly ISupplierRepository _supplierRepository;

        public ReportService(
            IPartnerRepository partnerRepository,
            IProductRepository productRepository,
            ISaleHistoryRepository saleHistoryRepository,
            ISupplierRepository supplierRepository)
        {
            _partnerRepository = partnerRepository;
            _productRepository = productRepository;
            _saleHistoryRepository = saleHistoryRepository;
            _supplierRepository = supplierRepository;
        }

        public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
        {
            var partners = await _partnerRepository.GetAllAsync();
            var products = await _productRepository.GetAllAsync();
            var sales = await _saleHistoryRepository.GetAllAsync();
            var suppliers = await _supplierRepository.GetAllAsync();

            return new DashboardSummaryDto
            {
                TotalPartners = partners.Count(),
                TotalProducts = products.Count(),
                TotalSuppliers = suppliers.Count(),
                TotalSales = sales.Sum(s => s.Quantity * s.SalePrice),
                TotalTransactions = sales.Count(),
                AverageTransactionValue = sales.Any() ? sales.Average(s => s.Quantity * s.SalePrice) : 0,
                TopPartners = (await _partnerRepository.GetTopPartnersAsync(5)).Select(p => p.Name).ToList(),
                LowStockProducts = products.Count(p => p.QuantityInStock <= p.MinStockQuantity)
            };
        }

        public async Task<SalesReportDto> GetSalesReportAsync(DateTime startDate, DateTime endDate)
        {
            var sales = await _saleHistoryRepository.GetByDateRangeAsync(startDate, endDate);

            return new SalesReportDto
            {
                StartDate = startDate,
                EndDate = endDate,
                TotalSales = sales.Sum(s => s.Quantity * s.SalePrice),
                TotalTransactions = sales.Count(),
                AverageDailySales = sales
                    .GroupBy(s => s.SaleDate.Date)
                    .Average(g => g.Sum(s => s.Quantity * s.SalePrice)),
                TopProducts = sales
                    .GroupBy(s => s.Product.Name)
                    .Select(g => new ProductSalesDto
                    {
                        ProductName = g.Key,
                        Quantity = g.Sum(s => s.Quantity),
                        TotalAmount = g.Sum(s => s.Quantity * s.SalePrice)
                    })
                    .OrderByDescending(p => p.TotalAmount)
                    .Take(10)
                    .ToList()
            };
        }
    }
}
