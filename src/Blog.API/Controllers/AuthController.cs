using System.Threading.Tasks;
using AutoMapper;
using Blog.Application.Exceptions;
using Blog.Application.Extensions;
using Blog.Application.Repositories.AuthRepo;
using Blog.Application.Repositories.UserRepo;
using Blog.Application.Services.Auth;
using Blog.Application.ViewModels;
using Blog.Domain.Dtos.Auth.Request;
using Blog.Domain.Dtos.Auth.Response;
using Blog.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/v{version:apiVersion}/blog/auth")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        private readonly IUserRepository _userRepo;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthRepository authRepo, IUserRepository userRepo, IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _authRepo = authRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        [Route("register")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            var userToCreate = new User
            {
                Username = request.Username.ToLower()
            };
            var createdUser = await _authRepo.Register(userToCreate, request.Password);

            var profileToCreate = new UserProfile
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserId = createdUser.Id
            };

            await _userRepo.CreateUserProfile(profileToCreate);

            var response = _mapper.Map<UserDto>(profileToCreate);

            return CreatedAtAction(
                nameof(UsersController.Get),
                nameof(UsersController).GetControllerName(),
                new { id = createdUser.Id, version = HttpContext.GetRequestedApiVersion().ToString() },
                response
            );
        }

        [Route("login")]
        [HttpPost]
        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var user = await _authRepo.Login(request.Username.ToLower(), request.Password);

            if (user == null)
                throw new BadRequestException("Invalid request");

            var jwt = _authService.GenerateJWT(user, request.ExpiresAt);

            return Ok(new TokenDto { Token = jwt });
        }
    }
}
