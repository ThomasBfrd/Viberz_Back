using AutoMapper;
using Microsoft.OpenApi.Extensions;
using Viberz.Application.DTO.Playlist;
using Viberz.Application.DTO.Songs;
using Viberz.Application.Factory.PlaylistFactory;
using Viberz.Application.Interfaces.Playlist;
using Viberz.Application.Interfaces.Spotify;
using Viberz.Domain.Entities;
using Viberz.Domain.Enums;
using Viberz.Domain.Interfaces.LikedPlaylistRepository;

namespace Viberz.Application.Services.Playlists;

public class PlaylistService(
    IPlaylistRepository playlistRepository,
    ILikedPlaylistsRepository likedPlaylistsRepository,
    PlaylistStrategyFactory playlistStrategyFactory,
    IUserRepository userRepository,
    ISpotifyService spotifyService,
    IMapper mapper) : IPlaylistService
{
    private readonly IPlaylistRepository _playlistRepository = playlistRepository;
    private readonly ILikedPlaylistsRepository _likedPlaylistsRepository = likedPlaylistsRepository;
    private readonly PlaylistStrategyFactory _playlistStrategyFactory = playlistStrategyFactory;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ISpotifyService _spotifyService = spotifyService;
    private readonly IMapper _mapper = mapper;

    public async Task<bool> DeletePlaylist(int playlistId, string userId)
    {
        Playlist? playlist = await _playlistRepository.GetByIdAsync(playlistId) ??
            throw new Exception("This playlist does not exist in database");

        User? user = await _userRepository.GetByIdAsync(userId) ??
            throw new Exception("This user does not exist in database");

        if (playlist.UserId != user.Id) 
            throw new Exception("You are not the owner of this playlist");

        await _playlistRepository.Delete(playlist);
        return true;
    }

    public async Task<List<PlaylistDTO>> GetPlaylist(string userId, bool? owner, int? playlistId, FamilyGenres? family, string? search, bool likes, int? page, int? pageSize)
    {
        IPlaylistStategy stategy = _playlistStrategyFactory.GetStategy(owner, playlistId, family, page, pageSize);

        List<PlaylistDTO> playlists = await stategy.BuildResult(userId, owner, playlistId, family, search, likes, page, pageSize);

        return playlists;
    }

    public async Task<bool> LikePlaylist(int playlistId, string userId)
    {
        Playlist? playlist = await _playlistRepository.GetByIdAsync(playlistId) ??
            throw new Exception("This playlist does not exist in database");

        User? user = await _userRepository.GetByIdAsync(userId) ??
            throw new Exception("This user does not exist in database");

        bool alreadyLiked = await _likedPlaylistsRepository.IsPlaylistLikedAsync(playlist.Id, user.Id);

        if (alreadyLiked)
        {
            LikedPlaylists? likedPlaylists = await _likedPlaylistsRepository.GetLikedPlaylistAsync(playlist.Id, user.Id) ??
                throw new Exception("Liked playlist entry not found");
            
            await _likedPlaylistsRepository.Delete(likedPlaylists);
            await _playlistRepository.DislikePlaylist(playlist.Id, user.Id);
            return false;
        };

        LikedPlaylists newLikedPlaylists = new LikedPlaylists
        {
            PlaylistId = playlist.Id,
            UserId = user.Id
        };

        await _playlistRepository.LikePlaylist(playlist.Id, user.Id);
        await _likedPlaylistsRepository.Add(newLikedPlaylists);

        return true;
    }

    public async Task<PlaylistDTO> CreatePlaylist(AddPlaylistDTO addPlaylist, string userId)
    {
        if (addPlaylist.SpotifyPlaylistId is null || addPlaylist.Name is null || addPlaylist.GenreList.Count == 0)
            throw new ArgumentException("The given data is invalid");

        SongFromSpotifyPlaylistDTO? songFromSpotifyPlaylist = await _spotifyService.GetSongFromSpotifyPlaylist(null, addPlaylist.SpotifyPlaylistId) ??
            throw new Exception("Playlist not found");

        User? user = await _userRepository.GetByIdAsync(userId) ??
            throw new Exception("This user does not exist in database");

        Playlist? newPlaylist = new()
        {
            SpotifyPlaylistId = addPlaylist.SpotifyPlaylistId,
            UserId = user.Id,
            Name = addPlaylist.Name,
            GenreList = addPlaylist.GenreList,
            ImageUrl = songFromSpotifyPlaylist.Images.FirstOrDefault()?.Url ?? string.Empty,
        };

        await _playlistRepository.Add(newPlaylist);

        PlaylistDTO playlist = _mapper.Map<Playlist, PlaylistDTO>(newPlaylist);

        return playlist;
    }

    public async Task<PlaylistDTO> UpdatePlaylist(UpdatePlaylistDTO updatePlaylist, string userId)
    {
        Playlist? existingPlaylist = await _playlistRepository.GetByIdAsync(updatePlaylist.Id) ??
            throw new Exception("Playlist not found");

        User? user = await _userRepository.GetUser(userId) ??
            throw new Exception("This user does not exist in database");

        if (existingPlaylist.UserId != user.Id) throw new Exception("You are not the owner of this playlist");

        Playlist playlistToUpdate = _mapper.Map<UpdatePlaylistDTO, Playlist>(updatePlaylist);

        Playlist playlist = await _playlistRepository.UpdatePlaylist(playlistToUpdate);

        PlaylistDTO updatedPlaylist = _mapper.Map<Playlist, PlaylistDTO>(playlist);

        return updatedPlaylist;
    }
}
