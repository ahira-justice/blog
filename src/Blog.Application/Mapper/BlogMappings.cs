using AutoMapper;
using Blog.Domain.Dtos.Auth.Response;
using Blog.Domain.Entities;

namespace Blog.Application.Mapper
{
    public class BlogMappings : Profile
    {
        public BlogMappings()
        {
            //requests

            // responses
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Profile.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Profile.LastName));
            CreateMap<UserProfile, UserDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username));

        }
    }
}
