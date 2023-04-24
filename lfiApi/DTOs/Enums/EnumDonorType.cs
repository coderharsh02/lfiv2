using System.ComponentModel.DataAnnotations;

namespace lfiApi.DTOs.Enums
{
    public enum EnumDonorType
    {
        [Display(Name = "Individual")]
        Individual = 1,

        [Display(Name = "Organization")]
        Organization = 2,

        [Display(Name = "Restaurant")]
        Restaurant = 3,

        [Display(Name = "PartyPlot")]
        PartyPlot = 4
    }
}
