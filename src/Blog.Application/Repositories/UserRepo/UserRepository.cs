using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Domain.Entities;
using Blog.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Repositories.UserRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserById(long id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<bool> UpdateUser(User user)
        {
            _context.Users.Update(user);
            return await Save();
        }

        public async Task<List<UserProfile>> GetUserProfiles()
        {
            return await _context.UserProfiles.ToListAsync();
        }

        public async Task<UserProfile> GetUserProfileById(long id)
        {
            return await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<UserProfile> GetUserProfileByUsername(string username)
        {
            return await _context.UserProfiles.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Username == username);
        }

        public async Task<bool> CreateUserProfile(UserProfile user)
        {
            await _context.UserProfiles.AddAsync(user);
            return await Save();
        }

        public async Task<bool> UpdateUserProfile(UserProfile user)
        {
            _context.UserProfiles.Update(user);
            return await Save();
        }

        private async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
