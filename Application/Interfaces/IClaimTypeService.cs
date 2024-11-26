using System.Security.Claims;

namespace Application.Interfaces;

public interface IClaimTypeService
{
    string GetFullName(ClaimsPrincipal claimsPrincipal);
}