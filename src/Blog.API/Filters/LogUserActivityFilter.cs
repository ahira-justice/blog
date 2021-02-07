using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Blog.Application.Repositories.UserRepo;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Blog.API.Filters
{
    public class LogUserActivityFilter : IAsyncActionFilter
    {
        private readonly ILogger<LogUserActivityFilter> _logger;

        public LogUserActivityFilter(ILogger<LogUserActivityFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("Logging user last active datetime");
            var resultContext = await next();

            var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();

            var userId = int.Parse(resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = await repo.GetUserById(userId);

            if (user != null && user.IsActive)
            {
                var now = DateTime.UtcNow;
                _logger.LogInformation($"User {user.Username} last active at {now.ToString()}");

                user.LastActive = now;
                await repo.UpdateUser(user);
            }
            else
            {
                _logger.LogInformation($"Couldn't find user with Id: {userId}");
            }
        }
    }
}
