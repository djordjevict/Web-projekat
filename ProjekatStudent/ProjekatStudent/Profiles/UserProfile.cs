using AutoMapper;

namespace ProjekatStudent.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Entities.User, Models.UserDto>();
            CreateMap<Models.UserDto, Entities.User>();
        }
    }
}
