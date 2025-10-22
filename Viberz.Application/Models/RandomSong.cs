using Viberz.Application.DTO.Songs;
using Viberz.Domain.Enums;

namespace Viberz.Application.Models;

public class RandomSong
{
    public required ItemDto Song { get; set; }
    public required string Genre { get; set; } = string.Empty;
    public required XpGames EarnedXp { get; set; }
    public required List<string> OtherGenres { get; set; } = new();
}
