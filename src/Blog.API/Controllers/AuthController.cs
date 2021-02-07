using System.Threading.Tasks;
using Blog.Application.Repositories.AuthRepo;
using Blog.Application.Repositories.UserRepo;
using Blog.Application.Services.Auth;
using Blog.Domain.Dtos.Auth.Request;
using Blog.Domain.Dtos.Auth.Response;
using Blog.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/blog/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        private readonly IUserRepository _userRepo;
        private readonly IAuthService _authService;
        public AuthController(IAuthRepository authRepo, IUserRepository userRepo, IAuthService authService)
        {
            _authService = authService;
            _authRepo = authRepo;
            _userRepo = userRepo;
        }

        [Route("register")]
        [HttpPost]
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
            var createdProfile = _userRepo.CreateUserProfile(profileToCreate);

            return StatusCode(201);
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var user = await _authRepo.Login(request.Username.ToLower(), request.Password);

            if (user == null)
                return Unauthorized();

            var jwt = _authService.GenerateJWT(user, request.ExpiresAt);

            return Ok(new TokenDto { Token = jwt });
        }
    }
}
