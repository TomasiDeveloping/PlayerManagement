using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Database.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Application.Interfaces;

public interface IJwtService
{
    SigningCredentials GetSigningCredentials();

    Task<List<Claim>> GetClaimsAsync(User user);

    JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims);
}