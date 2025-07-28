using AutoMapper;
using LearnGenres.DTO;
using LearnGenres.Entities;

namespace LearnGenres.Utilities
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            ConfigureUsers();
        }

        private void ConfigureUsers()
        {
            CreateMap<UserCreationDTO, User>();
            CreateMap<User, UserDTO>();
        }
    }
}
