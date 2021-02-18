using System;
using System.Linq;
using Blog.Application.Exceptions;
using Blog.Application.ViewModels;
using Blog.Domain.Exceptions;

namespace Blog.Application.Extensions
{
    public static class ErrorResponseExtensions
    {
        public static ErrorResponse ToErrorResponse(this Exception ex)
        {
            return new ErrorResponse
            {
                Code = "SystemError",
                Message = "An unexpected error occured. Please try again or confirm current operation status"
            };
        }

        public static ErrorResponse ToErrorResponse(this ApplicationDomainException ex)
        {
            return new ErrorResponse
            {
                Code = ex.Code,
                Message = ex.Message
            };
        }

        public static ErrorResponse ToErrorResponse(this ValidationException ex)
        {
            return new ValidationErrorResponse
            {
                Code = ex.Code,
                Message = ex.Message,
                Errors = ex.Failures
            };
        }
    }
}
