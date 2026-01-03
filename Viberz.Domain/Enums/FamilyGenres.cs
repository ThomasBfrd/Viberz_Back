using System.ComponentModel.DataAnnotations;

namespace Viberz.Domain.Enums;

public enum FamilyGenres
{
    [Display(Name = "Bass Music")]
    BassMusic = 0,
    [Display(Name = "Hard Music")]
    HardMusic = 1,
    [Display(Name = "House & Tech")]
    HouseAndTech = 2,
    [Display(Name = "EDM")]
    EDM = 3,
    [Display(Name = "All")]
    All = 4,
    [Display(Name = "Owner")]
    Owner = 5
}
