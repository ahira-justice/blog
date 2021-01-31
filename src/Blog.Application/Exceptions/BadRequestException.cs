using Blog.Domain.Exceptions;

namespace Blog.Application.Exceptions
{
    public class BadRequestException : ApplicationDomainException
    {
        public BadRequestException(string message) : base(message, "BadRequest") { }
    }
}
