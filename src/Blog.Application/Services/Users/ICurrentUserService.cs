using Blog.Domain.Entities;

namespace Blog.Application.Services.Users
{
    public interface ICurrentUserService
    {
        bool TryGetUserId(out long userId);

        string GetUsername();

        User CurrentUser { get; }

    }
}
