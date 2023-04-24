using System.ComponentModel.DataAnnotations;

namespace lfiApi.DTOs.Enums
{
    public enum EnumCollectorType
    {
        [Display(Name = "Individual")]
        Individual = 1,

        [Display(Name = "Organization")]
        Organization = 2
    }
}
