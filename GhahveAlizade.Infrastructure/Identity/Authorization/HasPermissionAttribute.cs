using Microsoft.AspNetCore.Authorization;

namespace GhahveAlizade.Infrastructure.Identity.Authorization;

/// <summary>
/// چک می‌کنه کاربر Permission مشخصی رو داره یا نه.
/// استفاده: [HasPermission("Products.Create")]
/// یا: [HasPermission(Permissions.Products.Create)]
/// </summary>
[AttributeUsage(
	AttributeTargets.Class | AttributeTargets.Method,
	AllowMultiple = true,
	Inherited = true)]
public sealed class HasPermissionAttribute : AuthorizeAttribute
{
	public const string PolicyPrefix = "Permission:";

	public HasPermissionAttribute(string permission)
		: base(PolicyPrefix + permission)
	{
	}
}