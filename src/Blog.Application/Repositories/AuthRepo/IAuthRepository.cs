using System.Threading.Tasks;
using Blog.Domain.Entities;

namespace Blog.Application.Repositories.AuthRepo
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string username);
    }
}
