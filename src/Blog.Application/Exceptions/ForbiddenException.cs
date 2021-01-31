using Blog.Domain.Exceptions;

namespace Blog.Application.Exceptions
{
    public class ForbiddenException : ApplicationDomainException
    {
        public ForbiddenException(string username) : base($"Unauthorized: {username} is not allowed to access this resource", "Forbidden") { }

        public ForbiddenException() : base("Unauthorized: user is not allowed to access this resource", "Forbidden") { }
    }
}
