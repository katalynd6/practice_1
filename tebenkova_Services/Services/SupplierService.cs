using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tebenkova_Models.Interfaces;
using tebenkova_Models.Models;
using tebenkova_Services.DTOs;
using tebenkova_Services.Helpers;

namespace tebenkova_Services.Services
{
    public class SupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IProductRepository _productRepository;

        public SupplierService(
            ISupplierRepository supplierRepository,
            IProductRepository productRepository)
        {
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Supplier>> GetAllSuppliersAsync()
        {
            return await _supplierRepository.GetAllAsync();
        }

        public async Task<Supplier> GetSupplierByIdAsync(int id)
        {
            return await _supplierRepository.GetByIdWithProductsAsync(id);
        }

        public async Task<Supplier> CreateSupplierAsync(Supplier supplier)
        {
            if (string.IsNullOrWhiteSpace(supplier.Name))
                throw new ArgumentException("Наименование поставщика обязательно");

            if (!string.IsNullOrWhiteSpace(supplier.INN) && !ValidationHelper.IsValidINN(supplier.INN))
                throw new ArgumentException("Неверный формат ИНН");

            return await _supplierRepository.AddAsync(supplier);
        }

        public async Task UpdateSupplierAsync(Supplier supplier)
        {
            await _supplierRepository.UpdateAsync(supplier);
        }

        public async Task DeleteSupplierAsync(int id)
        {
            var products = await _productRepository.GetBySupplierIdAsync(id);
            if (products.Any())
                throw new InvalidOperationException("Нельзя удалить поставщика, у которого есть товары");

            await _supplierRepository.DeleteAsync(id);
        }

        public async Task<SupplierStatisticsDto> GetSupplierStatisticsAsync(int supplierId)
        {
            var supplier = await _supplierRepository.GetByIdWithProductsAsync(supplierId);
            var products = supplier.Products ?? new List<Product>();

            return new SupplierStatisticsDto
            {
                SupplierId = supplier.Id,
                SupplierName = supplier.Name,
                TotalProducts = products.Count(),
                TotalStockValue = products.Sum(p => p.QuantityInStock * (p.CostPrice ?? 0)),
                AverageProductPrice = products.Any() ? products.Average(p => p.CostPrice ?? 0) : 0,
                ProductsByType = products
                    .GroupBy(p => p.ProductType?.Name ?? "Без типа")
                    .ToDictionary(g => g.Key, g => g.Count()),
                Rating = supplier.Rating
            };
        }
    }
}
