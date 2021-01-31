using Blog.Domain.Exceptions;

namespace Blog.Application.Exceptions
{
    public class NotFoundException : ApplicationDomainException
    {
        public NotFoundException(string message) : base(message, "NotFound") { }

        public NotFoundException(string name, object key) : base($"Resource \"{name}\" ({key}) was not found", "NotFound") { }
    }
}
