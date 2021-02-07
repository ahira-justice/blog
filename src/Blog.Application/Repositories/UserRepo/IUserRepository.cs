using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Domain.Entities;

namespace Blog.Application.Repositories.UserRepo
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsers();
        Task<User> GetUserById(long id);
        Task<User> GetUserByUsername(string username);
        Task<bool> UpdateUser(User user);
        Task<List<UserProfile>> GetUserProfiles();
        Task<UserProfile> GetUserProfileById(long id);
        Task<UserProfile> GetUserProfileByUsername(string username);
        Task<bool> CreateUserProfile(UserProfile user);
        Task<bool> UpdateUserProfile(UserProfile user);
    }
}
