using AutoMapper;
using Viberz.Application.DTO;
using Viberz.Application.DTO.User;
using Viberz.Domain.Entities;

namespace Viberz.Application.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserUpdateDTO, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
