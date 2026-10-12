using GhahveAlizade.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace GhahveAlizade.Infrastructure.Persistence.Seed;

public class PermissionSeeder
{
	private readonly AppDbContext _context;
	private readonly ILogger<PermissionSeeder> _logger;

	public PermissionSeeder(AppDbContext context, ILogger<PermissionSeeder> logger)
	{
		_context = context;
		_logger = logger;
	}

	public async Task SeedAsync(CancellationToken cancellationToken = default)
	{
		var existingPermissions = await _context.Permissions
			.Select(p => p.Name)
			.ToListAsync(cancellationToken);

		var permissionsToAdd = new List<Permission>();
		var allPermissions = GetAllPermissions();

		foreach (var (group, name, displayName, displayOrder) in allPermissions)
		{
			if (existingPermissions.Contains(name)) continue;

			permissionsToAdd.Add(new Permission
			{
				Name = name,
				DisplayName = displayName,
				Group = group,
				DisplayOrder = displayOrder
			});
		}

		if (permissionsToAdd.Count == 0)
		{
			_logger.LogInformation("No new permissions to seed.");
			return;
		}

		await _context.Permissions.AddRangeAsync(permissionsToAdd, cancellationToken);
		await _context.SaveChangesAsync(cancellationToken);

		_logger.LogInformation("Seeded {Count} permissions.", permissionsToAdd.Count);
	}

	/// <summary>
	/// همه‌ی Permissionها رو از کلاس Permissions با Reflection استخراج می‌کنه
	/// </summary>
	private static List<(string Group, string Name, string DisplayName, int Order)> GetAllPermissions()
	{
		var result = new List<(string, string, string, int)>();
		var permissionType = typeof(Permissions);

		// گروه‌های اصلی (Nested Classes)
		var groups = permissionType.GetNestedTypes(BindingFlags.Public | BindingFlags.Static);

		int globalOrder = 0;
		foreach (var group in groups)
		{
			var groupName = group.Name;

			var constants = group.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
				.Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
				.OrderBy(f => f.Name);

			foreach (var constant in constants)
			{
				var value = (string)constant.GetRawConstantValue()!;
				var displayName = GetDisplayName(groupName, constant.Name);

				result.Add((groupName, value, displayName, globalOrder++));
			}
		}

		return result;
	}

	private static string GetDisplayName(string group, string name)
	{
		// می‌تونی فارسی‌سازی رو اینجا اضافه کنی
		return $"{group}.{name}";
	}
}