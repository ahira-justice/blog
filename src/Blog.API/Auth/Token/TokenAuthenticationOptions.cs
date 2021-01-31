using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Blog.API.Auth.Token
{
    public class TokenAuthenticationOptions : AuthenticationSchemeOptions
    {
        public ClaimsIdentity Identity { get; set; }
        public string AuthorizationHeader { get => "Authorization"; }
        public string ChannelHeader { get => "Channel"; }
        public string Authentication { get => "Bearer"; }
    }
}
