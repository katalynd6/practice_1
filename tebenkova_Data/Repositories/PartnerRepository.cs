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
    public class PartnerRepository : IPartnerRepository
    {
        private readonly AppDbContext _context;

        public PartnerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Partner>> GetAllAsync()
        {
            return await _context.Partners
                .Include(p => p.PartnerType)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Partner>> GetTopPartnersAsync(int count)
        {
            return await _context.Partners
                .Include(p => p.PartnerType)
                .Include(p => p.SalesHistories)
                .OrderByDescending(p => p.SalesHistories.Sum(s => s.Quantity * s.SalePrice))
                .Take(count)
                .ToListAsync();
        }

        public async Task<Partner> GetByIdAsync(int id)
        {
            return await _context.Partners
                .Include(p => p.PartnerType)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Partner> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Partners
                .Include(p => p.PartnerType)
                .Include(p => p.SalesHistories)
                    .ThenInclude(sh => sh.Product)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Partner> AddAsync(Partner partner)
        {
            _context.Partners.Add(partner);
            await _context.SaveChangesAsync();
            return partner;
        }

        public async Task UpdateAsync(Partner partner)
        {
            _context.Entry(partner).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var partner = await _context.Partners.FindAsync(id);
            if (partner != null)
            {
                _context.Partners.Remove(partner);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Partner>> SearchAsync(string searchTerm)
        {
            return await _context.Partners
                .Include(p => p.PartnerType)
                .Where(p => p.Name.Contains(searchTerm) ||
                           (p.Phone != null && p.Phone.Contains(searchTerm)) ||
                           (p.Email != null && p.Email.Contains(searchTerm)))
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Partners.AnyAsync(p => p.Id == id);
        }
    }
}
