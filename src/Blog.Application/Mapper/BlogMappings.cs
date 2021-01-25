using AutoMapper;
using Blog.Domain.Dtos.Auth.Response;
using Blog.Domain.Dtos.UserProfile.Response;
using Blog.Domain.Entities;

namespace Blog.Application.Mapper
{
    public class BlogMappings : Profile
    {
        public BlogMappings()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserProfile, UserProfileDto>().ReverseMap();
        }
    }
}
