using System.Net;
using Blog.Application.Exceptions;
using Blog.Application.Extensions;
using Blog.Application.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Blog.API.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilter> _logger;
        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(new EventId(context.Exception.HResult), context.Exception, context.Exception.Message);

            HttpStatusCode code;
            ErrorResponse response;

            switch (context.Exception)
            {
                case BadRequestException ex:
                    code = HttpStatusCode.BadRequest;
                    response = ex.ToErrorResponse();
                    break;
                case ValidationException ex:
                    code = HttpStatusCode.BadRequest;
                    response = ex.ToErrorResponse();
                    break;
                case ForbiddenException ex:
                    code = HttpStatusCode.Forbidden;
                    response = ex.ToErrorResponse();
                    break;
                case NotFoundException ex:
                    code = HttpStatusCode.NotFound;
                    response = ex.ToErrorResponse();
                    break;
                case SystemErrorException ex:
                    code = HttpStatusCode.InternalServerError;
                    response = ex.ToErrorResponse();
                    break;
                case UnauthorizedException ex:
                    code = HttpStatusCode.Unauthorized;
                    response = ex.ToErrorResponse();
                    break;
                default:
                    code = HttpStatusCode.InternalServerError;
                    response = context.Exception.ToErrorResponse();
                    break;
            }

            var resolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            var settings = new JsonSerializerSettings
            {
                ContractResolver = resolver,
                Formatting = Formatting.Indented
            };

            var result = JsonConvert.SerializeObject(response, settings);

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int) code;
            context.HttpContext.Response.WriteAsync(result);
            context.ExceptionHandled = true;
        }
    }
}
