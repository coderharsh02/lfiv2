using System.ComponentModel.DataAnnotations.Schema;

namespace lfiApi.Entities
{
    public class Donation
    {
        public int Id { get; set; }
        public int NoOfMeals { get; set; }
        public int DonationStatusId { get; set; }
        public string FeedbackByDonor { get; set; }
        public string FeedbackByCollector { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime pickUpTimeFrom { get; set; } = DateTime.Now;
        public DateTime pickUpTimeTo { get; set; } = DateTime.Now + TimeSpan.FromHours(3);
        public int DonorId { get; set; }
        public AppUser Donor { get; set; }
        public Nullable<int> CollectorId { get; set; }
        public AppUser? Collector { get; set; }
    }
}
