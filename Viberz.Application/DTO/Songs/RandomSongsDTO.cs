using Viberz.Application.Models;

namespace Viberz.Application.DTO.Songs;

public class RandomSongsDTO
{
    public string AccessToken { get; set; } = string.Empty;
    public List<RandomSong> RandomSong { get; set; } = new();
}
