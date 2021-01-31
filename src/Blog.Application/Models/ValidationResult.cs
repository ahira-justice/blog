using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Blog.Application.Models
{
    public class ValidationResult
    {
        public ValidationResult()
        {
            Errors = new List<string>();
        }

        public ClaimsPrincipal Payload { get; set; }
        public bool Success { get => !Errors.Any(); }
        public List<string> Errors { get; set; }

        public void AddError(string error)
        {
            Errors.Add(error);
        }
    }
}
