using GhahveAlizade.Domain.Identity;
using GhahveAlizade.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace GhahveAlizade.Infrastructure.Identity;

/// <summary>
/// هنگام ساخت ClaimsPrincipal، Permissionها رو به Claims کاربر اضافه می‌کنه.
/// این باعث می‌شه Razor بتونه @if (User.HasClaim("permission", "...")) استفاده کنه.
/// </summary>
public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
{
	private readonly AppDbContext _context;

	public AppClaimsPrincipalFactory(
		UserManager<ApplicationUser> userManager,
		RoleManager<ApplicationRole> roleManager,
		IOptions<IdentityOptions> options,
		AppDbContext context)
		: base(userManager, roleManager, options)
	{
		_context = context;
	}

	protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
	{
		var identity = await base.GenerateClaimsAsync(user);

		// استخراج Permissionهای کاربر از دیتابیس
		var roles = await UserManager.GetRolesAsync(user);

		var permissions = await _context.RolePermissions
			.AsNoTracking()
			.Where(rp => !rp.IsDeleted
					  && roles.Contains(rp.Role.Name!)
					  && !rp.Permission.IsDeleted)
			.Select(rp => rp.Permission.Name)
			.Distinct()
			.ToListAsync();

		foreach (var permission in permissions)
		{
			identity.AddClaim(new Claim("permission", permission));
		}

		return identity;
	}
}