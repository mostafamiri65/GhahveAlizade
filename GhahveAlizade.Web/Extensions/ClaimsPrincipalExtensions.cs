using System.Security.Claims;

namespace GhahveAlizade.Web.Extensions;

public static class ClaimsPrincipalExtensions
{
	public static long? GetUserId(this ClaimsPrincipal user)
	{
		var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
		return long.TryParse(id, out var userId) ? userId : null;
	}

	public static string? GetUserFullName(this ClaimsPrincipal user)
		=> user.FindFirstValue("FullName") ?? user.Identity?.Name;

	public static bool HasPermission(this ClaimsPrincipal user, string permission)
		=> user.HasClaim("permission", permission);

	public static bool IsSuperAdmin(this ClaimsPrincipal user)
		=> user.IsInRole("SuperAdmin");

	public static bool IsAdmin(this ClaimsPrincipal user)
		=> user.IsInRole("SuperAdmin") || user.IsInRole("Admin");
}