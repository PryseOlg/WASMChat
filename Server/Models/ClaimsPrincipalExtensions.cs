using System.Security.Claims;

namespace WASMChat.Server.Models;

public static class ClaimsPrincipalExtensions
{
	private const string NameClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
	
	/// <summary>
	/// Gets this <see cref="ClaimsPrincipal"/>s name that corresponds with
	/// <see cref="ApplicationUser"/>s id.
	/// </summary>
	/// <param name="principal"></param>
	/// <returns></returns>
	public static string? GetUserId(this ClaimsPrincipal principal)
		=> principal.FindFirstValue(NameClaim);
}
