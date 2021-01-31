using Blog.Domain.Exceptions;

namespace Blog.Application.Exceptions
{
    public class SystemErrorException : ApplicationDomainException
    {
        public SystemErrorException() : base("An unexpected error occured. Please try again or confirm current operation status", "SystemError") { }
        public SystemErrorException(string message) : base(message, "SystemError") { }
    }
}
