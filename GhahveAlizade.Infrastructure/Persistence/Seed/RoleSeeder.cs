using GhahveAlizade.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GhahveAlizade.Infrastructure.Persistence.Seed;

public class RoleSeeder
{
	private readonly RoleManager<ApplicationRole> _roleManager;
	private readonly AppDbContext _context;
	private readonly ILogger<RoleSeeder> _logger;

	public RoleSeeder(
		RoleManager<ApplicationRole> roleManager,
		AppDbContext context,
		ILogger<RoleSeeder> logger)
	{
		_roleManager = roleManager;
		_context = context;
		_logger = logger;
	}

	public async Task SeedAsync(CancellationToken cancellationToken = default)
	{
		// 1. SuperAdmin — همه‌ی دسترسی‌ها
		await EnsureRoleWithAllPermissionsAsync(
			name: "SuperAdmin",
			description: "دسترسی کامل به تمام بخش‌های سیستم",
			isSystemRole: true);

		// 2. Admin — اکثر دسترسی‌ها بجز حساس‌ها
		await EnsureRoleWithPermissionsAsync(
			name: "Admin",
			description: "مدیر فروشگاه",
			isSystemRole: true,
			excludedPermissions: new[]
			{
				Permissions.Settings.ManageAccounting,
				Permissions.Users.ManageRoles,
				Permissions.Roles.ManagePermissions
			});

		// 3. ContentManager — مدیریت محتوا و SEO
		await EnsureRoleWithPermissionsAsync(
			name: "ContentManager",
			description: "مدیر محتوا و سئو",
			isSystemRole: true,
			allowedPermissions: new[]
			{
				Permissions.Blog.ViewPosts,
				Permissions.Blog.CreatePost,
				Permissions.Blog.EditPost,
				Permissions.Blog.PublishPost,
				Permissions.Blog.ManageCategories,
				Permissions.Blog.ManageTags,
				Permissions.Seo.ViewMetadata,
				Permissions.Seo.EditMetadata,
				Permissions.Seo.ManageRedirects,
				Permissions.Seo.ManageLandingPages,
				Permissions.Products.View,
				Permissions.Categories.View,
				Permissions.Brands.View,
				Permissions.Marketing.ViewBanners,
				Permissions.Marketing.ManageBanners
			});

		// 4. OrderManager — مدیریت سفارشات
		await EnsureRoleWithPermissionsAsync(
			name: "OrderManager",
			description: "مدیر سفارشات و پرداخت‌ها",
			isSystemRole: true,
			allowedPermissions: new[]
			{
				Permissions.Orders.View,
				Permissions.Orders.ViewDetails,
				Permissions.Orders.Edit,
				Permissions.Orders.Cancel,
				Permissions.Orders.ChangeStatus,
				Permissions.Orders.Print,
				Permissions.Orders.Export,
				Permissions.Payments.View,
				Permissions.Payments.Verify,
				Permissions.Shipments.View,
				Permissions.Shipments.Create,
				Permissions.Shipments.Edit,
				Permissions.Shipments.UpdateTracking,
				Permissions.Customers.View,
				Permissions.Customers.ViewDetails
			});

		// 5. InventoryManager — مدیریت انبار
		await EnsureRoleWithPermissionsAsync(
			name: "InventoryManager",
			description: "مدیر انبار و موجودی",
			isSystemRole: true,
			allowedPermissions: new[]
			{
				Permissions.Inventory.View,
				Permissions.Inventory.Adjust,
				Permissions.Inventory.Transfer,
				Permissions.Inventory.ViewTransactions,
				Permissions.Inventory.ManageWarehouses,
				Permissions.Inventory.ManageBranches,
				Permissions.Coffee.ManageBatches,
				Permissions.Products.View,
				Permissions.Products.Edit
			});

		// 6. Customer — کاربر عادی
		await EnsureRoleAsync(
			name: "Customer",
			description: "مشتری فروشگاه",
			isSystemRole: true);

		_logger.LogInformation("Roles seeded successfully.");
	}

	private async Task EnsureRoleAsync(string name, string description, bool isSystemRole)
	{
		var role = await _roleManager.FindByNameAsync(name);
		if (role is null)
		{
			role = new ApplicationRole
			{
				Name = name,
				Description = description,
				IsSystemRole = isSystemRole,
				CreatedAtUtc = DateTime.UtcNow
			};
			await _roleManager.CreateAsync(role);
			_logger.LogInformation("Created role: {Role}", name);
		}
	}

	private async Task EnsureRoleWithAllPermissionsAsync(string name, string description, bool isSystemRole)
	{
		await EnsureRoleAsync(name, description, isSystemRole);
		var role = await _roleManager.FindByNameAsync(name);
		if (role is null) return;

		var allPermissions = await _context.Permissions
			.Where(p => !p.IsDeleted)
			.ToListAsync();

		await SyncRolePermissionsAsync(role.Id, allPermissions.Select(p => p.Id));
	}

	private async Task EnsureRoleWithPermissionsAsync(
		string name,
		string description,
		bool isSystemRole,
		IEnumerable<string>? allowedPermissions = null,
		IEnumerable<string>? excludedPermissions = null)
	{
		await EnsureRoleAsync(name, description, isSystemRole);
		var role = await _roleManager.FindByNameAsync(name);
		if (role is null) return;

		var permissionsQuery = _context.Permissions.Where(p => !p.IsDeleted);

		if (allowedPermissions is not null)
		{
			var allowedList = allowedPermissions.ToList();
			permissionsQuery = permissionsQuery.Where(p => allowedList.Contains(p.Name));
		}

		if (excludedPermissions is not null)
		{
			var excludedList = excludedPermissions.ToList();
			permissionsQuery = permissionsQuery.Where(p => !excludedList.Contains(p.Name));
		}

		var permissionIds = await permissionsQuery.Select(p => p.Id).ToListAsync();
		await SyncRolePermissionsAsync(role.Id, permissionIds);
	}

	private async Task SyncRolePermissionsAsync(long roleId, IEnumerable<long> permissionIds)
	{
		var roleIdString = roleId;
		var existing = await _context.RolePermissions
			.Where(rp => rp.RoleId == roleIdString)
			.ToListAsync();

		var existingPermissionIds = existing.Select(rp => rp.PermissionId).ToHashSet();
		var targetPermissionIds = permissionIds.ToHashSet();

		// اضافه کردن جدیدها
		var toAdd = targetPermissionIds
			.Except(existingPermissionIds)
			.Select(pid => new RolePermission
			{
				RoleId = roleIdString,
				PermissionId = pid,
				CreatedAtUtc = DateTime.UtcNow
			})
			.ToList();

		if (toAdd.Count > 0)
		{
			await _context.RolePermissions.AddRangeAsync(toAdd);
		}

		// حذف قدیمی‌ها (Soft Delete)
		var toRemove = existing.Where(rp => !targetPermissionIds.Contains(rp.PermissionId)).ToList();
		foreach (var rp in toRemove)
		{
			rp.IsDeleted = true;
			rp.DeletedAtUtc = DateTime.UtcNow;
		}

		if (toAdd.Count > 0 || toRemove.Count > 0)
		{
			await _context.SaveChangesAsync();
		}
	}
}