namespace lfiApi.Helpers
{
    public class UserParams : PaginationParams
    {
        public string CurrentUsername { get; set; }
        public string City { get; set; } = "None";
        public string OrderBy { get; set; } = "lastActive";
    }
}
