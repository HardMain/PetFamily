using System.Text;
using Accounts.Infrastructure.Options;
using Microsoft.IdentityModel.Tokens;

namespace Accounts.Infrastructure.Jwt
{
    public static class TokenValidationParametersFactory
    {
        public static TokenValidationParameters CreateWithLifeTime(JwtOptions jwtOptions)
        {
            return new TokenValidationParameters
            {
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };
        }

        public static TokenValidationParameters CreateWithoutLifeTime(JwtOptions jwtOptions)
        {
            return new TokenValidationParameters
            {
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };
        }
    }
}