using System;
using Microsoft.AspNetCore.Authentication;

namespace Blog.API.Auth.Token
{
    public static class TokenAuthenticationExtensions
    {
        public static AuthenticationBuilder AddTokenAuthentication(this AuthenticationBuilder builder, string authScheme, string displayName, Action<TokenAuthenticationOptions> options)
        {
            return builder.AddScheme<TokenAuthenticationOptions, TokenAuthenticationHandler>(authScheme, displayName, options);
        }
    }
}
