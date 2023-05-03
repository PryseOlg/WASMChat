using System.Security.Claims;

namespace WASMChat.Server.Models;

public static class ClaimsPrincipalExtensions
{
    public static string? GetAppUserId(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.NameIdentifier);
}