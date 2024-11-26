using System.Security.Claims;
using Application.Interfaces;

namespace Application.Services;

public class ClaimTypeService : IClaimTypeService
{
    public string GetFullName(ClaimsPrincipal claimsPrincipal)
    {
        var userName = claimsPrincipal.FindFirstValue("playerName");

        return string.IsNullOrEmpty(userName) ? "Unknown" : userName;
    }
}