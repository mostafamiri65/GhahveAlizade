using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace GhahveAlizade.Infrastructure.Identity.Authorization;

/// <summary>
/// Policy Provider سفارشی که برای هر Permission یه Policy داینامیک می‌سازه.
/// این باعث می‌شه نیاز به ثبت دستی هر Policy نداشته باشیم.
/// </summary>
public class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
	private readonly DefaultAuthorizationPolicyProvider _fallbackProvider;

	public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
	{
		_fallbackProvider = new DefaultAuthorizationPolicyProvider(options);
	}

	public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
		=> _fallbackProvider.GetDefaultPolicyAsync();

	public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
		=> _fallbackProvider.GetFallbackPolicyAsync();

	public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
	{
		// اگه Policy یه Permission باشه
		if (policyName.StartsWith(HasPermissionAttribute.PolicyPrefix, StringComparison.OrdinalIgnoreCase))
		{
			var permission = policyName[HasPermissionAttribute.PolicyPrefix.Length..];

			var policy = new AuthorizationPolicyBuilder()
				.RequireAuthenticatedUser()
				.AddRequirements(new PermissionRequirement(permission))
				.Build();

			return Task.FromResult<AuthorizationPolicy?>(policy);
		}

		// در غیر این صورت، از Default استفاده کن
		return _fallbackProvider.GetPolicyAsync(policyName);
	}
}