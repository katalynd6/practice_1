using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tebenkova_Models.Models;

namespace tebenkova_Models.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> GetByIdWithDetailsAsync(int id);
        Task<Product> AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<IEnumerable<Product>> GetLowStockAsync();
        Task<IEnumerable<Product>> SearchAsync(string searchTerm);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Product>> GetBySupplierIdAsync(int supplierId);
    }
}
