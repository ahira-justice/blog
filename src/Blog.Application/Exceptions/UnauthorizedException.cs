using Blog.Domain.Exceptions;

namespace Blog.Application.Exceptions
{
    public class UnauthorizedException : ApplicationDomainException
    {
        public UnauthorizedException(string message) : base(message, "Unauthorized") { }
    }
}
