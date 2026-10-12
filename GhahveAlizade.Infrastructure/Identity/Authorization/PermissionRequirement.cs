using Microsoft.AspNetCore.Authorization;

namespace GhahveAlizade.Infrastructure.Identity.Authorization;

public class PermissionRequirement : IAuthorizationRequirement
{
	public string Permission { get; }

	public PermissionRequirement(string permission)
	{
		if (string.IsNullOrWhiteSpace(permission))
			throw new ArgumentException("Permission cannot be empty.", nameof(permission));

		Permission = permission;
	}
}