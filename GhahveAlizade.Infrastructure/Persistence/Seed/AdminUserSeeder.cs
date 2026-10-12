using GhahveAlizade.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GhahveAlizade.Infrastructure.Persistence.Seed;

public class AdminUserSeeder
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly RoleManager<ApplicationRole> _roleManager;
	private readonly IConfiguration _configuration;
	private readonly ILogger<AdminUserSeeder> _logger;

	public AdminUserSeeder(
		UserManager<ApplicationUser> userManager,
		RoleManager<ApplicationRole> roleManager,
		IConfiguration configuration,
		ILogger<AdminUserSeeder> logger)
	{
		_userManager = userManager;
		_roleManager = roleManager;
		_configuration = configuration;
		_logger = logger;
	}

	public async Task SeedAsync(CancellationToken cancellationToken = default)
	{
		var settings = _configuration.GetSection("AdminUser").Get<AdminUserSettings>()
			?? new AdminUserSettings();

		var existingUser = await _userManager.FindByEmailAsync(settings.Email);
		if (existingUser is not null)
		{
			_logger.LogInformation("Admin user already exists: {Email}", settings.Email);
			return;
		}

		var admin = new ApplicationUser
		{
			UserName = settings.UserName,
			Email = settings.Email,
			EmailConfirmed = true,
			PhoneNumber = settings.PhoneNumber,
			PhoneNumberConfirmed = true,
			FirstName = settings.FirstName,
			LastName = settings.LastName,
			IsActive = true,
			CreatedAtUtc = DateTime.UtcNow,
			CreatedBy = 1
		};

		var result = await _userManager.CreateAsync(admin, settings.Password);

		if (!result.Succeeded)
		{
			var errors = string.Join(", ", result.Errors.Select(e => e.Description));
			_logger.LogError("Failed to create admin user: {Errors}", errors);
			throw new InvalidOperationException($"Failed to create admin user: {errors}");
		}

		// اختصاص نقش SuperAdmin
		if (await _roleManager.RoleExistsAsync("SuperAdmin"))
		{
			await _userManager.AddToRoleAsync(admin, "SuperAdmin");
		}

		_logger.LogInformation("Admin user created successfully: {Email}", settings.Email);
	}
}