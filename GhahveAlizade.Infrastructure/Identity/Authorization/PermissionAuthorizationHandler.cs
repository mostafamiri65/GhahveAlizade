using GhahveAlizade.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GhahveAlizade.Infrastructure.Identity.Authorization;

/// <summary>
/// بررسی می‌کنه که آیا کاربر (از طریق نقش‌هایش) Permission درخواست‌شده رو داره.
/// </summary>
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
	private readonly IServiceScopeFactory _scopeFactory;
	private readonly ILogger<PermissionAuthorizationHandler> _logger;

	public PermissionAuthorizationHandler(
		IServiceScopeFactory scopeFactory,
		ILogger<PermissionAuthorizationHandler> logger)
	{
		_scopeFactory = scopeFactory;
		_logger = logger;
	}

	protected override async Task HandleRequirementAsync(
		AuthorizationHandlerContext context,
		PermissionRequirement requirement)
	{
		// 1. اگه کاربر Authenticate نیست، رد کن
		if (context.User?.Identity?.IsAuthenticated != true)
		{
			_logger.LogDebug(
				"Permission check failed: user is not authenticated. Required: {Permission}",
				requirement.Permission);
			return;
		}

		// 2. اگه SuperAdmin هست، همه چیز رو قبول کن
		if (context.User.IsInRole("SuperAdmin"))
		{
			_logger.LogDebug(
				"Permission granted to SuperAdmin: {Permission}",
				requirement.Permission);
			context.Succeed(requirement);
			return;
		}

		// 3. استخراج UserId
		var userIdClaim = context.User.FindFirst(
			System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

		if (!long.TryParse(userIdClaim, out var userId))
		{
			_logger.LogWarning("Permission check failed: UserId claim is missing or invalid.");
			return;
		}

		// 4. استخراج نقش‌های کاربر (از Claim)
		var roles = context.User
			.FindAll(System.Security.Claims.ClaimTypes.Role)
			.Select(c => c.Value)
			.ToList();

		if (roles.Count == 0)
		{
			_logger.LogDebug(
				"Permission check failed: user {UserId} has no roles.", userId);
			return;
		}

		// 5. بررسی در دیتابیس (در Scope جداگانه چون Handler Singleton نیست همیشه)
		using var scope = _scopeFactory.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

		var hasPermission = await dbContext.RolePermissions
			.AsNoTracking()
			.AnyAsync(rp =>
				!rp.IsDeleted
				&& roles.Contains(rp.Role.Name!)
				&& rp.Permission.Name == requirement.Permission
				&& !rp.Permission.IsDeleted);

		if (hasPermission)
		{
			_logger.LogDebug(
				"Permission granted to user {UserId} for {Permission}",
				userId, requirement.Permission);

			context.Succeed(requirement);
		}
		else
		{
			_logger.LogDebug(
				"Permission denied to user {UserId} for {Permission}",
				userId, requirement.Permission);
		}
	}
}