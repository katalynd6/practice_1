using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tebenkova_Models.Interfaces;
using tebenkova_Models.Models;
using tebenkova_Services.DTOs;

namespace tebenkova_Services.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ISupplierRepository _supplierRepository;

        public ProductService(
            IProductRepository productRepository,
            ISupplierRepository supplierRepository)
        {
            _productRepository = productRepository;
            _supplierRepository = supplierRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetByIdWithDetailsAsync(id);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Article))
                throw new ArgumentException("Артикул обязателен");

            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Наименование товара обязательно");

            if (product.QuantityInStock < 0)
                throw new ArgumentException("Количество на складе не может быть отрицательным");

            return await _productRepository.AddAsync(product);
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync()
        {
            return await _productRepository.GetLowStockAsync();
        }

        public async Task<StockSummaryDto> GetStockSummaryAsync()
        {
            var products = await _productRepository.GetAllAsync();

            return new StockSummaryDto
            {
                TotalProducts = products.Count(),
                TotalItemsInStock = products.Sum(p => p.QuantityInStock),
                LowStockItems = products.Count(p => p.QuantityInStock <= p.MinStockQuantity),
                OutOfStockItems = products.Count(p => p.QuantityInStock <= 0),
                TotalStockValue = products.Sum(p => p.QuantityInStock * (p.CostPrice ?? 0)),
                ProductsByType = products
                    .GroupBy(p => p.ProductType?.Name ?? "Без типа")
                    .ToDictionary(g => g.Key, g => g.Count())
            };
        }
    }
}
