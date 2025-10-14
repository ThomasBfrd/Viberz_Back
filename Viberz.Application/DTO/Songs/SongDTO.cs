using Viberz.Domain.Entities;
using Viberz.Domain.Enums;

namespace Viberz.Application.DTO.Songs;

public class SongDTO
{
    public ItemDto Song { get; set; }
    public string Genre { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public XpGames EarnedXp { get; set; }
    public List<string> OtherGenres { get; set; } = new();
}
