using Blog.Domain.Entities;

namespace Blog.Application.Services.Auth
{
    public interface IAuthService
    {
        string GenerateJWT(User user, int? expiresAt);
    }
}
