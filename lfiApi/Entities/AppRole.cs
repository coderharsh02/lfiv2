using Microsoft.AspNetCore.Identity;

namespace lfiApi.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}