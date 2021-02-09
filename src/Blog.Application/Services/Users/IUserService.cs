using Blog.Application.Models;
using Blog.Application.Queries.UserProfile;

namespace Blog.Application.Services.Users
{
    public interface IUsersService
    {
        DataSourceResult SearchUsers(SearchUsersQuery request);
    }
}
