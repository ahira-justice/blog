using Blog.Application.Models;

namespace Blog.Application.Services.Auth
{
    public interface ITokenHandlerService
    {
        ValidationResult Validate(string token);
    }
}
