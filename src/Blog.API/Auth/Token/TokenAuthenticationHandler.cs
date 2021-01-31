using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Blog.Application.Services.Auth;
using Blog.Persistence.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Blog.API.Auth.Token
{
    public class TokenAuthenticationHandler : AuthenticationHandler<TokenAuthenticationOptions>
    {
        private readonly ITokenHandlerService _tokenHandlerService;
        private readonly ApplicationDbContext _context;

        public TokenAuthenticationHandler(IOptionsMonitor<TokenAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, ITokenHandlerService tokenHandlerService, ApplicationDbContext context) : base(options, logger, encoder, clock)
        {
            _tokenHandlerService = tokenHandlerService;
            _context = context;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string errorReason;
            if (!Context.Request.Headers.TryGetValue(Options.AuthorizationHeader, out var headerValue))
            {
                errorReason = $"Missing or malformed '{Options.AuthorizationHeader}' header";
                return AuthenticateResult.Fail(errorReason);
            }

            var authHeader = headerValue.First();
            if (!authHeader.StartsWith(Options.Authentication + " ", StringComparison.OrdinalIgnoreCase))
            {
                errorReason = $"Malformed '{Options.AuthorizationHeader} header";
                return AuthenticateResult.Fail(errorReason);
            }

            try
            {
                var token = authHeader.Substring(Options.Authentication.Length).Trim();
                var validationResult = _tokenHandlerService.Validate(token);

                if (validationResult.Success)
                {
                    var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == validationResult.Payload.Identity.Name);
                    if (user == null)
                    {
                        errorReason = "User not active or does not exist";
                        return AuthenticateResult.Fail(errorReason);
                    }

                    var result = AuthenticateResult.Success(new AuthenticationTicket(validationResult.Payload, new AuthenticationProperties(), Scheme.Name));
                    return result;
                }

                errorReason = string.Join("; ", validationResult.Errors);
                return AuthenticateResult.Fail(errorReason);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex);
            }
        }
    }
}
