using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tebenkova_Models.Models;

namespace tebenkova_Models.Interfaces
{
    public interface IPartnerRepository
    {
        Task<IEnumerable<Partner>> GetAllAsync();
        Task<Partner> GetByIdAsync(int id);
        Task<Partner> GetByIdWithDetailsAsync(int id);
        Task<Partner> AddAsync(Partner partner);
        Task UpdateAsync(Partner partner);
        Task DeleteAsync(int id);
        Task<IEnumerable<Partner>> SearchAsync(string searchTerm);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Partner>> GetTopPartnersAsync(int count);
    }
}
