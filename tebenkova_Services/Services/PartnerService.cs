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
    public class PartnerService : IPartnerService
    {
        private readonly IPartnerRepository _partnerRepository;
        private readonly ISaleHistoryRepository _saleHistoryRepository;

        public PartnerService(
            IPartnerRepository partnerRepository,
            ISaleHistoryRepository saleHistoryRepository)
        {
            _partnerRepository = partnerRepository;
            _saleHistoryRepository = saleHistoryRepository;
        }

        public async Task<IEnumerable<Partner>> GetAllPartnersAsync()
        {
            return await _partnerRepository.GetAllAsync();
        }

        public async Task<Partner> GetPartnerByIdAsync(int id)
        {
            return await _partnerRepository.GetByIdAsync(id);
        }

        public async Task<Partner> GetPartnerDetailAsync(int id)
        {
            return await _partnerRepository.GetByIdWithDetailsAsync(id);
        }

        public async Task<Partner> CreatePartnerAsync(Partner partner)
        {
            if (string.IsNullOrWhiteSpace(partner.Name))
                throw new ArgumentException("Наименование партнера обязательно");

            if (partner.Rating < 0 || partner.Rating > 10)
                throw new ArgumentException("Рейтинг должен быть от 0 до 10");

            if (!string.IsNullOrWhiteSpace(partner.INN) && !ValidationHelper.IsValidINN(partner.INN))
                throw new ArgumentException("Неверный формат ИНН");

            if (!string.IsNullOrWhiteSpace(partner.Phone) && !ValidationHelper.IsValidPhone(partner.Phone))
                throw new ArgumentException("Неверный формат телефона");

            if (!string.IsNullOrWhiteSpace(partner.Email) && !ValidationHelper.IsValidEmail(partner.Email))
                throw new ArgumentException("Неверный формат email");

            return await _partnerRepository.AddAsync(partner);
        }

        public async Task UpdatePartnerAsync(Partner partner)
        {
            if (!await _partnerRepository.ExistsAsync(partner.Id))
                throw new KeyNotFoundException($"Партнер с ID {partner.Id} не найден");

            await _partnerRepository.UpdateAsync(partner);
        }

        public async Task DeletePartnerAsync(int id)
        {
            await _partnerRepository.DeleteAsync(id);
        }

        public async Task<int> CalculateDiscountAsync(int partnerId)
        {
            var sales = await _saleHistoryRepository.GetByPartnerIdAsync(partnerId);
            return DiscountHelper.CalculateDiscount(sales);
        }

        public async Task<IEnumerable<Partner>> SearchPartnersAsync(string searchTerm)
        {
            return await _partnerRepository.SearchAsync(searchTerm);
        }

        public async Task<PartnerStatisticsDto> GetPartnerStatisticsAsync(int partnerId)
        {
            var partner = await _partnerRepository.GetByIdWithDetailsAsync(partnerId);
            var sales = await _saleHistoryRepository.GetByPartnerIdAsync(partnerId);

            return new PartnerStatisticsDto
            {
                PartnerId = partner.Id,
                PartnerName = partner.Name,
                TotalSales = sales.Sum(s => s.Quantity * s.SalePrice),
                TotalTransactions = sales.Count(),
                AverageCheck = sales.Any() ? sales.Average(s => s.Quantity * s.SalePrice) : 0,
                FirstPurchaseDate = sales.Any() ? sales.Min(s => s.SaleDate) : (DateTime?)null,
                LastPurchaseDate = sales.Any() ? sales.Max(s => s.SaleDate) : (DateTime?)null,
                Discount = DiscountHelper.CalculateDiscount(sales)
            };
        }
    }
}
