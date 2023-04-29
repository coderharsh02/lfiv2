namespace lfiApi.DTOs
{
    public class DonationDto
    {
        public int DonationId { get; set; }
        public int NoOfMeals { get; set; }
        public string Status { get; set; }
        public string FeedbackByCollector { get; set; }
        public string FeedbackByDonor { get; set; }
        public MemberDto DonatedBy { get; set; }
        public MemberDto CollectedBy { get; set; }
    }
}