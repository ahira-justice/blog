using System.Linq;
using System.Security.Claims;
using Blog.Domain.Entities;
using Blog.Persistence.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Services.UserProfile
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        private User _cachedUser;

        public CurrentUserService(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public bool TryGetUserId(out long userId)
        {
            var result = long.TryParse(_contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
            return result;
        }

        public string GetUsername()
        {
            return _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
        }

        public User CurrentUser
        {
            get
            {
                if (_cachedUser != null)
                    return _cachedUser;

                if (TryGetUserId(out var userId))
                {
                    var user = _context.Users.Include(x => x.Profile).FirstOrDefault(x => x.Id == userId);

                    if (user != null)
                        _cachedUser = user;
                }

                return _cachedUser;
            }
        }
    }
}
