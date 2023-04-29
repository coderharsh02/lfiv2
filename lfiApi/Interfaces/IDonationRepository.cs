using lfiApi.DTOs;
using lfiApi.Entities;
using lfiApi.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace lfiApi.Interfaces
{
    public interface IDonationRepository
    {
        void Update(Donation donation);
        Task<IEnumerable<DonationDto>> GetDonationsAsync();
        Task<DonationDto> GetDonationByIdAsync(int id);
        Task<DonationDto> GetFullDonaitonByIdAsync(int id);
        Task<List<DonationDto>> GetCurrentUserDonationAsync(int userId);
        Task<List<DonationDto>> GetCurrentUserCollectionAsync(int userId);
        Task<List<DonationDto>> GetDonationsByDonorIdAsync(int donorId);
        Task<List<DonationDto>> GetDonationsByCollectorIdAsync(int collectorId);
        Task<TopDonorCollectorDto> GetTopDonorCollector();
        Task<Donation> AddDonation(Donation donation);
        Task<ActionResult> AddCollector(Donation donation);
        Task<ActionResult> updateDonationStatusToCollected(Donation donation);
        Task<ActionResult> updateDonationStatusToDonated(Donation donation);
    }
}