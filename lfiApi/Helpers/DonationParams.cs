namespace lfiApi.Helpers
{
    public class DonationParams : PaginationParams
    {
        public string CurrentUsername { get; set; }
        public string City { get; set; } = "None";
        public int MinMeals { get; set; } = 5;
        public int MaxMeals { get; set; } = 20;
        public string OrderBy { get; set; } = "createdOn";
    }
}
