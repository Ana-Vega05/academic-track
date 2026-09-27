using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AcademicTrack.Application.Auth.Interfaces;
using AcademicTrack.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AcademicTrack.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public (string Token, DateTime Expiration) GenerateToken(User user, IEnumerable<string>? permissions = null)
    {
        var secretKey = _configuration["Jwt:Key"] ?? "AcademicTrack_DefaultSecretKey_For_Jwt_Security_Token_2024!";
        var issuer = _configuration["Jwt:Issuer"] ?? "AcademicTrackAPI";
        var audience = _configuration["Jwt:Audience"] ?? "AcademicTrackClient";
        var expirationHours = int.TryParse(_configuration["Jwt:ExpirationHours"], out var hours) ? hours : 24;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role),
            new("fullName", user.FullName)
        };

        if (permissions != null)
        {
            foreach (var perm in permissions)
            {
                if (!string.IsNullOrWhiteSpace(perm))
                {
                    claims.Add(new Claim("permission", perm.Trim()));
                }
            }
        }

        var expiration = DateTime.UtcNow.AddHours(expirationHours);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiration,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return (tokenHandler.WriteToken(token), expiration);
    }
}
