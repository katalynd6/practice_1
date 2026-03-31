using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tebenkova_Models.Models;

namespace tebenkova_Models.Interfaces
{
    public interface IPartnerService
    {
        Task<IEnumerable<Partner>> GetAllPartnersAsync();
        Task<Partner> GetPartnerByIdAsync(int id);
        Task<Partner> GetPartnerDetailAsync(int id);
        Task<Partner> CreatePartnerAsync(Partner partner);
        Task UpdatePartnerAsync(Partner partner);
        Task DeletePartnerAsync(int id);
        Task<int> CalculateDiscountAsync(int partnerId);
        Task<IEnumerable<Partner>> SearchPartnersAsync(string searchTerm);
    }
}
