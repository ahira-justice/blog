using System.Threading.Tasks;
using Blog.Application.Repositories.AuthRepo;
using Blog.Application.Repositories.UserRepo;
using Blog.Domain.Dtos.Auth.Request;
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
        public AuthController(IAuthRepository authRepo, IUserRepository userRepo)
        {
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
            var createdProfile = _userRepo.CreateUser(profileToCreate);

            return StatusCode(201);
        }
    }
}
