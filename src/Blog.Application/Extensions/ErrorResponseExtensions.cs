using System;
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
    }
}
