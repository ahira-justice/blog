using System.Threading.Tasks;
using Blog.Application.Repositories.AuthRepo;
using Blog.Domain.Dtos.Auth.Request;
using Blog.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/blog/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        public AuthController(IAuthRepository repo)
        {
            _repo = repo;
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            var userToCreate = new User { Username = request.Username.ToLower() };

            var createdUser = await _repo.Register(userToCreate, request.Password);

            return StatusCode(201);
        }
    }
}
