using System;

namespace Blog.Domain.Exceptions
{
    public class ApplicationDomainException : Exception
    {
        public string Code { get; set; }

        public ApplicationDomainException() { }

        public ApplicationDomainException(string message) : base(message) { }

        public ApplicationDomainException(string message, string code) : base(message)
        {
            Code = code;
        }

        public ApplicationDomainException(string message, Exception innerException) : base(message, innerException) { }

        public override string ToString()
        {
            return $"Error: \n\nCode: {Code}\n\nMessage: {Message}";
        }
    }
}
