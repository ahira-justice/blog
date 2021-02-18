using System.Collections.Generic;

namespace Blog.Application.ViewModels
{
    public class ValidationErrorResponse : ErrorResponse
    {
        public IDictionary<string, string[]> Errors { get; set; }
    }
}
