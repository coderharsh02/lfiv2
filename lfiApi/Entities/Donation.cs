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

        [ForeignKey("Users")]
        public int DonorId { get; set; }

        [ForeignKey("Users")]
        public Nullable<int> CollectorId { get; set; }
    }
}
