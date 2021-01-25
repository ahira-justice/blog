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

        public async Task<List<UserProfile>> GetUsers()
        {
            return await _context.UserProfiles.ToListAsync();
        }

        public async Task<UserProfile> GetUserById(long id)
        {
            return await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<UserProfile> GetUserByUsername(string username)
        {
            return await _context.UserProfiles.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Username == username);
        }

        public async Task<bool> CreateUser(UserProfile user)
        {
            await _context.UserProfiles.AddAsync(user);
            return await Save();
        }

        public async Task<bool> UpdateUser(UserProfile user)
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
