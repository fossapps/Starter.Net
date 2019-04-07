using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Starter.Net.Api.Configs;

namespace Starter.Net.Api.Services
{
    public class TokenFactory : ITokenFactory
    {
        private readonly JwtBearerOptions _jwt;
        public TokenFactory(IOptions<Configs.Authentication> authConfig)
        {
            _jwt = authConfig.Value.JwtBearerOptions;
        }

        public string GenerateToken(int size)
        {
            var randomNumber = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public string GenerateJwtToken(ClaimsPrincipal principal)
        {
            var key = Encoding.ASCII.GetBytes(_jwt.SigningKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwt.Issuer,
                Audience = _jwt.Audience,
                Subject = new ClaimsIdentity(principal.Claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwt.JwtTtl),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
