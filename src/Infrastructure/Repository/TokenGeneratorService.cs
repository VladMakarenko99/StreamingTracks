using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Abstractions;
using Domain.Entities;
using Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Repository;

public class TokenGeneratorService(IOptions<JwtOptions> jwtOptions) : ITokenGeneratorService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    
    public Task<string> GenerateJwtToken(ApplicationUser user, List<string> userRoles)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key ?? ""));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var userClaims = new List<Claim>
        {
            new Claim("FirstName", user?.FirstName ?? ""),
            new Claim("LastName", user?.LastName ?? ""),
            new Claim("Email", user?.Email ?? "")
        };

        foreach (var role in userRoles)
        {
            userClaims.Add(new Claim("Role", role));
        }

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: userClaims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
        );

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }
}