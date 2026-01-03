namespace Viberz.Application.DTO.Playlist;

public class PaginatedPlaylistResponse<T> where T : class
{
    public string AccessToken { get; set; } = string.Empty;
    public List<T> Items { get; set; } = [];
    public int TotalItems { get; set; }
    public int PageSize { get; set; } = 0;
    public int CurrentPage { get; set; } = 0;
}
