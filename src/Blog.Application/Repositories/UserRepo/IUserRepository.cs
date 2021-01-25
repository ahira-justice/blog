using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Domain.Entities;

namespace Blog.Application.Repositories.UserRepo
{
    public interface IUserRepository
    {
        Task<List<UserProfile>> GetUsers();
        Task<UserProfile> GetUserById(long id);
        Task<UserProfile> GetUserByUsername(string username);
        Task<bool> CreateUser(UserProfile user);
        Task<bool> UpdateUser(UserProfile user);
    }
}
