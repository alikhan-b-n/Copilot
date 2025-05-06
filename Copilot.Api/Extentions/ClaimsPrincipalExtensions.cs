using System.Security.Claims;

namespace Copilot.Api.Extentions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetId(this ClaimsPrincipal user)
    {
        return Guid.TryParse(user.Claims.FirstOrDefault()?.Value, out var value) 
            ? value
            : throw new ArgumentException("Forbidden");
    }
}