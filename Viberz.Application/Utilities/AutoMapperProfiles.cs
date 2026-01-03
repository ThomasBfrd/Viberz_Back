using AutoMapper;
using Viberz.Application.DTO.Genres;
using Viberz.Application.DTO.Playlist;
using Viberz.Application.DTO.User;
using Viberz.Application.DTO.Xp;
using Viberz.Domain.Entities;
using Viberz.Domain.Models;

namespace Viberz.Application.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserUpdateDTO, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UpdatePlaylistDTO, Playlist>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<User, UserDTO>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.Xp, opt => opt.Ignore());

            CreateMap<UserXpInfo, UserXpDTO>();
            CreateMap<UserXpDTO, UserXpInfo>();
            CreateMap<Genre, GenresWithSpotifyId>();
            CreateMap<Playlist, PlaylistDTO>();
        }
    }
}