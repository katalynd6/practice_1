using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tebenkova_Models.Models;

namespace tebenkova_Models.Interfaces
{
    public interface ISaleHistoryRepository
    {
        Task<IEnumerable<SaleHistory>> GetAllAsync();
        Task<SaleHistory> GetByIdAsync(int id);
        Task<IEnumerable<SaleHistory>> GetByPartnerIdAsync(int partnerId);
        Task<IEnumerable<SaleHistory>> GetByProductIdAsync(int productId);
        Task<IEnumerable<SaleHistory>> GetByDateRangeAsync(System.DateTime startDate, System.DateTime endDate);
        Task<SaleHistory> AddAsync(SaleHistory saleHistory);
        Task UpdateAsync(SaleHistory saleHistory);
        Task DeleteAsync(int id);
        Task<decimal> GetTotalSalesByPartnerAsync(int partnerId);
    }
}
