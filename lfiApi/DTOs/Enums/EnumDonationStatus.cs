using System.ComponentModel.DataAnnotations;

namespace lfiApi.DTOs.Enums
{
    public enum EnumDonationStatus
    {
        [Display(Name = "Available")]
        Available = 1,

        [Display(Name = "Booked")]
        Booked = 2,

        [Display(Name = "Collected")]
        Collected = 3,

        [Display(Name = "Donated")]
        Donated = 4
    }
}
