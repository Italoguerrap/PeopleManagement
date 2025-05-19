using Microsoft.IdentityModel.Tokens;
using PeopleManagement.Application.Interfaces;
using PeopleManagement.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PeopleManagement.Infrastructure.Auth;

public class TokenService : ITokenService
{
    public Token GenerateToken(long userId)
    {
        IEnumerable<Claim> claims =
        [
            new Claim("jti", Guid.NewGuid().ToString()),
            new Claim("sub", userId.ToString()),
            new Claim("id", userId.ToString()),
        ];

        JwtSecurityTokenHandler handler = new();
        byte[] key = Encoding.UTF8.GetBytes("PeopleManagementSecretKey1234567890");
        SecurityTokenDescriptor securityTokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = "PeopleManagement",
            Audience = "PeopleManagement.API",
            Expires = DateTime.UtcNow.AddHours(1),
            NotBefore = DateTime.UtcNow,
            IssuedAt = DateTime.UtcNow,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };

        SecurityToken securityToken = handler.CreateToken(securityTokenDescriptor);
        string jwtToken = handler.WriteToken(securityToken);

        return new Token
        {
            AccessToken = jwtToken,
            Expiration = DateTime.UtcNow.AddHours(1),
            UserId = userId
        };
    }
}
