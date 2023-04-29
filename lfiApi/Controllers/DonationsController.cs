using lfiApi.DTOs;
using lfiApi.Entities;
using lfiApi.Extensions;
using lfiApi.Helpers;
using lfiApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lfiApi.Controllers
{
    [AllowAnonymous]
    public class DonationsController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public DonationsController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET api/donations
        [HttpGet]
        public async Task<ActionResult<List<DonationDto>>> GetDonations()
        {
            return Ok(await _uow.DonationRepository.GetDonationsAsync());
        }

        [HttpGet("top")]
        public async Task<ActionResult<TopDonorCollectorDto>> GetTopDonorCollector()
        {
            return Ok(await _uow.DonationRepository.GetTopDonorCollector());
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<DonationDto>> GetDonationById(int id)
        {
            var res = await _uow.DonationRepository.GetDonationByIdAsync(id);
            if (res == null) return NotFound();
            return Ok(res);
        }

        [HttpGet("userid/{userId}")]
        public async Task<ActionResult<List<DonationDto>>> GetDonationsByDonorId(int userId)
        {
            return Ok(await _uow.DonationRepository.GetDonationsByDonorIdAsync(userId));
        }

        [HttpGet("currentUserDonations")]
        public async Task<ActionResult<List<DonationDto>>> GetCurrentUserDonationAsync()
        {
            return Ok(await _uow.DonationRepository.GetCurrentUserDonationAsync(User.GetUserId()));
        }
        [HttpGet("currentUserCollections")]
        public async Task<ActionResult<List<DonationDto>>> GetCurrentUserCollectionAsync()
        {
            return Ok(await _uow.DonationRepository.GetCurrentUserCollectionAsync(User.GetUserId()));
        }

        [HttpPost]
        public async Task<ActionResult<Donation>> AddDonation(DonationFormDto dfd)
        {
            var donation = new Donation
            {
                NoOfMeals = dfd.NoOfMeals,
                DonationStatusId = 1,
                DonorId = User.GetUserId()
            };
            return Ok(await _uow.DonationRepository.AddDonation(donation));
        }

        [HttpPut("addcollector")]
        public async Task<ActionResult> AddCollector(Donation d)
        {   
            d.CollectorId = User.GetUserId();
            return await _uow.DonationRepository.AddCollector(d);
        }

        [HttpPut("updateDonationStatusToCollected")]
        public async Task<ActionResult> updateDonationStatusToCollected(Donation d)
        {
            return await _uow.DonationRepository.updateDonationStatusToCollected(d);
        }

        [HttpPut("updateDonationStatusToDonated")]
        public async Task<ActionResult> updateDonationStatusToDonated(Donation donation)
        {
            return await _uow.DonationRepository.updateDonationStatusToDonated(donation);
        }
    }
}