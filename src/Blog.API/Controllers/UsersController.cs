using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Blog.API.Filters;
using Blog.Application.Repositories.UserRepo;
using Blog.Application.Services.UserProfile;
using Blog.Domain.Dtos.Auth.Response;
using Blog.Domain.Dtos.UserProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/blog/users")]
    [ApiController]
    [ServiceFilter(typeof(LogUserActivityFilter))]
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

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> Get([FromRoute] long id)
        {
            var userProfile = await _userRepo.GetUserProfileById(id);
            var response = _mapper.Map<UserDto>(userProfile);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userProfiles = await _userRepo.GetUserProfiles();
            var response = new List<UserDto>();

            foreach (var userProfile in userProfiles)
            {
                response.Add(_mapper.Map<UserDto>(userProfile));
            }

            return Ok(response);
        }

        [Route("{id}")]
        [HttpPut]
        public async Task<IActionResult> Update([FromRoute] long id, [FromBody] UpdateUserDto request)
        {
            var user = await _userRepo.GetUserProfileById(id);
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            var userProfile = await _userRepo.UpdateUserProfile(user);
            var response = _mapper.Map<UserDto>(userProfile);
            return Ok(response);
        }
    }
}
