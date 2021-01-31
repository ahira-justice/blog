using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Blog.Application.Models;
using Blog.Application.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Application.Services.Auth
{
    public class TokenHandlerService : ITokenHandlerService
    {
        private readonly ApplicationSettings _applicationSettings;
        public TokenHandlerService(IOptions<ApplicationSettings> applicationSettingsOptions)
        {
            _applicationSettings = applicationSettingsOptions.Value;
        }

        public ValidationResult Validate(string token)
        {
            var validationResult = new ValidationResult();
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                if (tokenHandler.CanReadToken(token))
                {
                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_applicationSettings.Token)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                    var claims = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                    validationResult.Payload = claims;
                }
                else
                {
                    validationResult.AddError("Token verification failed");
                }
            }
            catch (ArgumentException)
            {
                validationResult.AddError("Unable to decode token");
            }
            catch (Exception ex)
            {
                validationResult.AddError(ex.Message);
            }

            return validationResult;
        }
    }
}
