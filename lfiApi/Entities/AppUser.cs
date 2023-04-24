using Microsoft.AspNetCore.Identity;
namespace lfiApi.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public string DonorType { get; set; }
        public string VolunteerType { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public int Pincode { get; set; }
        override public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public string LifeGoals { get; set; }
        public DateTime MemberSince { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public List<UserLike> LikedByUsers { get; set; }
        public List<UserLike> LikedUsers { get; set; }
        public List<Message> MessagesSent { get; set; }
        public List<Message> MessagesReceived { get; set; }

        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
