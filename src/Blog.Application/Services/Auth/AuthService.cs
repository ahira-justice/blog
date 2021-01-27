using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Blog.Application.Settings;
using Blog.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationSettings _applicationSettings;
        public AuthService(IOptions<ApplicationSettings> applicationSettingsOptions)
        {
            _applicationSettings = applicationSettingsOptions.Value;
        }

        public string GenerateJWT(User user, int? expiresAt)
        {
            var claims = new []
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_applicationSettings.Token));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var expiry = expiresAt.HasValue ? expiresAt.Value : _applicationSettings.JwtDefaultExpiry;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expiry),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
