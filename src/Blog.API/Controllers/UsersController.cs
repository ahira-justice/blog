using AutoMapper;
using Blog.Application.Repositories.UserRepo;
using Blog.Application.Services.UserProfile;
using Blog.Domain.Dtos.Auth.Request;
using Blog.Domain.Dtos.Auth.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/blog/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository userRepo, ICurrentUserService currentUserService, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        [Route("me")]
        [HttpGet]
        public IActionResult Me()
        {
            var userProfile = _currentUserService.CurrentUser.Profile;
            var response = _mapper.Map<UserDto>(userProfile);
            return Ok(response);
        }
    }
}
