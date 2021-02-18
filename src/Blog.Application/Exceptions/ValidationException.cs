using System.Collections.Generic;
using System.Linq;
using Blog.Domain.Exceptions;
using FluentValidation.Results;

namespace Blog.Application.Exceptions
{
    public class ValidationException : ApplicationDomainException
    {
        public ValidationException() : base("One or more validation failures have occurred", "BadRequest")
        {
            Failures = new Dictionary<string, string[]>();
        }

        public ValidationException(List<ValidationFailure> failures) : this()
        {
            var failureGroups = failures.GroupBy(x => x.PropertyName, x => x.ErrorMessage);

            foreach (var failureGroup in failureGroups)
            {
                var propertyName = failureGroup.Key;
                var propertyFailures = failureGroup.ToArray();

                Failures.Add(propertyName, propertyFailures);
            }
        }

        public IDictionary<string, string[]> Failures { get; set; }
    }
}
