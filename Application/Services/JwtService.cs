using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Database;
using Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class JwtService(IConfiguration configuration, UserManager<User> userManager, ApplicationContext context) : IJwtService
{
    private readonly IConfigurationSection _jwtSection = configuration.GetSection("Jwt");
    public SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtSection["Key"]!);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    public async Task<List<Claim>> GetClaimsAsync(User user)
    {
        var claims = new List<Claim>
        {
            new("email", user.Email!),
            new("playerName", user.PlayerName),
            new("userId", user.Id.ToString()),
            new("allianceId", user.AllianceId.ToString()),
        };

        var roles = await userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        var userAlliance = await context.Alliances.FirstOrDefaultAsync(alliance => alliance.Id == user.AllianceId);
        claims.Add(new("allianceName", userAlliance!.Name));
        return claims;
    }

    public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken(
            issuer: _jwtSection["Issuer"],
            audience: _jwtSection["Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(Convert.ToInt32(_jwtSection["DurationInDays"])),
            signingCredentials: signingCredentials
        );

        return tokenOptions;
    }
}