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
    public class SaleHistoryRepository : ISaleHistoryRepository
    {
        private readonly AppDbContext _context;

        public SaleHistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SaleHistory>> GetAllAsync()
        {
            return await _context.SalesHistories
                .Include(sh => sh.Partner)
                .Include(sh => sh.Product)
                .OrderByDescending(sh => sh.SaleDate)
                .ToListAsync();
        }

        public async Task<SaleHistory> GetByIdAsync(int id)
        {
            return await _context.SalesHistories
                .Include(sh => sh.Partner)
                .Include(sh => sh.Product)
                .FirstOrDefaultAsync(sh => sh.Id == id);
        }

        public async Task<IEnumerable<SaleHistory>> GetByPartnerIdAsync(int partnerId)
        {
            return await _context.SalesHistories
                .Include(sh => sh.Product)
                .Where(sh => sh.PartnerId == partnerId)
                .OrderByDescending(sh => sh.SaleDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<SaleHistory>> GetByProductIdAsync(int productId)
        {
            return await _context.SalesHistories
                .Include(sh => sh.Partner)
                .Where(sh => sh.ProductId == productId)
                .OrderByDescending(sh => sh.SaleDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<SaleHistory>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.SalesHistories
                .Include(sh => sh.Partner)
                .Include(sh => sh.Product)
                .Where(sh => sh.SaleDate >= startDate && sh.SaleDate <= endDate)
                .OrderByDescending(sh => sh.SaleDate)
                .ToListAsync();
        }

        public async Task<SaleHistory> AddAsync(SaleHistory saleHistory)
        {
            _context.SalesHistories.Add(saleHistory);
            await _context.SaveChangesAsync();
            return saleHistory;
        }

        public async Task UpdateAsync(SaleHistory saleHistory)
        {
            _context.Entry(saleHistory).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var saleHistory = await _context.SalesHistories.FindAsync(id);
            if (saleHistory != null)
            {
                _context.SalesHistories.Remove(saleHistory);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<decimal> GetTotalSalesByPartnerAsync(int partnerId)
        {
            return await _context.SalesHistories
                .Where(sh => sh.PartnerId == partnerId)
                .SumAsync(sh => sh.Quantity * sh.SalePrice);
        }
    }
}
