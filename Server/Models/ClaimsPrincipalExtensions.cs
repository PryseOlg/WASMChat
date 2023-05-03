using System.Security.Claims;

namespace WASMChat.Server.Models;

public static class ClaimsPrincipalExtensions
{
	/// <summary>
	/// Gets this <see cref="ClaimsPrincipal"/>s name that corresponds with
	/// <see cref="ApplicationUser"/>s id.
	/// </summary>
	/// <param name="principal"></param>
	/// <returns></returns>
	public static string? GetUserId(this ClaimsPrincipal principal)
		=> principal.FindFirstValue(ClaimTypes.NameIdentifier);
}
