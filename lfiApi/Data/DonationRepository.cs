using AutoMapper;
using AutoMapper.QueryableExtensions;
using lfiApi.DTOs;
using lfiApi.DTOs.Enums;
using lfiApi.Entities;
using lfiApi.Helpers;
using lfiApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lfiApi.Data
{
    public class DonationRepository : IDonationRepository
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public DonationRepository(DataContext context, IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<DonationDto> DonationDtoFromDonation(Donation donation)
        {
            var status = Enum.GetName(typeof(EnumDonationStatus), donation.DonationStatusId);
            return new DonationDto()
            {
                DonationId = donation.Id,
                NoOfMeals = donation.NoOfMeals,
                Status = status,
                DonatedBy = await _userRepository.GetMemberById(donation.DonorId),
                CollectedBy = (donation.CollectorId != null) ? await _userRepository.GetMemberById(donation.CollectorId ?? 0) : null,
                FeedbackByCollector = donation.FeedbackByCollector,
                FeedbackByDonor = donation.FeedbackByDonor,
            };
        }
        public async Task<List<DonationDto>> DonationDtoFromDonation(List<Donation> donationList)
        {
            List<DonationDto> DonationDtoList = new List<DonationDto>();
            foreach (Donation donation in donationList)
            {
                DonationDtoList.Add(await DonationDtoFromDonation(donation));
            }
            return DonationDtoList;
        }

        public async Task<IEnumerable<DonationDto>> GetDonationsAsync()
        {
            return await DonationDtoFromDonation(await _context.Donations.ToListAsync());
        }
        public async Task<DonationDto> GetDonationByIdAsync(int id)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation == null) return null;
            return await DonationDtoFromDonation(donation);
        }
        public async Task<DonationDto> GetFullDonaitonByIdAsync(int id)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation == null) return null;
            return await DonationDtoFromDonation(donation);
        }
        public async Task<List<DonationDto>> GetDonationsByDonorIdAsync(int donorId)
        {
            var donations = from donation in _context.Donations
                            where donation.DonorId == donorId
                            select donation;

            if (donations == null) return null;

            return await DonationDtoFromDonation(await _context.Donations.Where(p => p.DonorId == donorId).ToListAsync());
        }
        public async Task<List<DonationDto>> GetDonationsByCollectorIdAsync(int collectorId)
        {
            return await DonationDtoFromDonation(await _context.Donations.Where(p => p.CollectorId == collectorId).ToListAsync());
        }

        public async Task<TopDonorCollectorDto> GetTopDonorCollector()
        {
            var topUsers = new TopDonorCollectorDto();

            var topDonors = await _context.Donations.GroupBy(l => l.DonorId)
            .Select(cl => new
            {
                DonorId = cl.First().DonorId,
                NoOfMeals = cl.Sum(c => c.NoOfMeals),
            }).ToListAsync();
            topDonors = topDonors.OrderByDescending(x => x.NoOfMeals).Take(3).ToList();

            if (topDonors.Count > 0)
            {
                foreach (var donor in topDonors)
                {
                    TopUserDto temp = new TopUserDto();
                    temp.User = await _userRepository.GetMemberById(donor.DonorId);
                    temp.NoOfMeals = donor.NoOfMeals;
                    topUsers.TopDonors.Add(temp);
                }
            }

            var topCollectors = await _context.Donations.GroupBy(l => l.CollectorId)
            .Select(cl => new
            {
                CollectorId = cl.First().CollectorId,
                NoOfMeals = cl.Sum(c => c.NoOfMeals),
            }).Where(x => x.CollectorId != null).ToListAsync();
            topCollectors = topCollectors.OrderByDescending(x => x.NoOfMeals).Take(3).ToList();

            if (topCollectors.Count > 0)
            {
                foreach (var collection in topCollectors)
                {
                    TopUserDto temp = new TopUserDto();
                    temp.User = await _userRepository.GetMemberById(collection.CollectorId ?? 0);
                    temp.NoOfMeals = collection.NoOfMeals;
                    topUsers.TopCollectors.Add(temp);
                }
            }
            return topUsers;
        }
        public void Update(Donation donation)
        {
            _context.Entry(donation).State = EntityState.Modified;
        }

        public async Task<Donation> AddDonation(Donation donation)
        {
            // Add the user to the database.
            _context.Donations.Add(donation);

            // Save the changes to the database.
            await _context.SaveChangesAsync();

            // return the UserTokenDto
            return donation;
        }
        public async Task<ActionResult> AddCollector(Donation donation)
        {
            var d = await _context.Donations.FindAsync(donation.Id);
            if (d == null) return new StatusCodeResult(404);

            Console.WriteLine(donation.DonationStatusId);
            Console.WriteLine((int)EnumDonationStatus.Available);

            if (d.DonationStatusId != (int)EnumDonationStatus.Available) return new StatusCodeResult(400);

            d.DonationStatusId = (int)EnumDonationStatus.Booked;
            d.CollectorId = d.CollectorId;
            d.FeedbackByCollector = d.FeedbackByCollector;
            d.FeedbackByDonor = d.FeedbackByDonor;
            _context.Donations.Update(d);

            await _context.SaveChangesAsync();
            return new StatusCodeResult(204);
        }

        public async Task<ActionResult> updateDonationStatusToCollected(Donation donation)
        {
            var d = await _context.Donations.FindAsync(donation.Id);
            if (d == null) return new StatusCodeResult(404);

            if (donation.DonationStatusId != (int)EnumDonationStatus.Collected) return new StatusCodeResult(400);

            d.DonationStatusId = (int)EnumDonationStatus.Collected;
            d.FeedbackByCollector = d.FeedbackByCollector;
            d.FeedbackByDonor = d.FeedbackByDonor;
            _context.Donations.Update(d);

            await _context.SaveChangesAsync();

            return new StatusCodeResult(204);
        }

        public async Task<ActionResult> updateDonationStatusToDonated(Donation donation)
        {
            var d = await _context.Donations.FindAsync(donation.Id);
            if (d == null) return new StatusCodeResult(404);

            if (donation.DonationStatusId != (int)EnumDonationStatus.Donated) return new StatusCodeResult(400);

            d.DonationStatusId = (int)EnumDonationStatus.Donated;
            d.FeedbackByCollector = d.FeedbackByCollector;
            d.FeedbackByDonor = d.FeedbackByDonor;
            _context.Donations.Update(d);

            await _context.SaveChangesAsync();

            return new StatusCodeResult(204);
        }

        public async Task<List<DonationDto>> GetCurrentUserDonationAsync(int userId)
        {
            var donations = await _context.Donations.Where(p => p.DonorId == userId).ToListAsync();
            if (donations == null) return null;
            return await DonationDtoFromDonation(donations);
        }

        public async Task<List<DonationDto>> GetCurrentUserCollectionAsync(int userId)
        {
            var donations = await _context.Donations.Where(p => p.CollectorId == userId).ToListAsync();
            if (donations == null) return null;
            return await DonationDtoFromDonation(donations);
        }
    }
}