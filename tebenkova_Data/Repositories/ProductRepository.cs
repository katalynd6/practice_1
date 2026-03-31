using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tebenkova_Models;
using tebenkova_Models.Interfaces;
using tebenkova_Models.Models;

namespace tebenkova_Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.ProductType)
                .Include(p => p.Supplier)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetBySupplierIdAsync(int supplierId)
        {
            return await _context.Products
                .Include(p => p.ProductType)
                .Include(p => p.Supplier)
                .Where(p => p.SupplierId == supplierId)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.ProductType)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Products
                .Include(p => p.ProductType)
                .Include(p => p.Supplier)
                .Include(p => p.StockReceipts)
                .Include(p => p.StockIssues)
                .Include(p => p.SalesHistories)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetLowStockAsync()
        {
            return await _context.Products
                .Where(p => p.QuantityInStock <= p.MinStockQuantity)
                .OrderBy(p => p.QuantityInStock)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchAsync(string searchTerm)
        {
            return await _context.Products
                .Include(p => p.ProductType)
                .Include(p => p.Supplier)
                .Where(p => p.Name.Contains(searchTerm) ||
                           p.Article.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(p => p.Id == id);
        }
    }
}
