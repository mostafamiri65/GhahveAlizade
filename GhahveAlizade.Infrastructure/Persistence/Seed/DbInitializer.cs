using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GhahveAlizade.Infrastructure.Persistence.Seed;

public class DbInitializer
{
	private readonly IServiceProvider _serviceProvider;
	private readonly ILogger<DbInitializer> _logger;

	public DbInitializer(IServiceProvider serviceProvider, ILogger<DbInitializer> logger)
	{
		_serviceProvider = serviceProvider;
		_logger = logger;
	}

	public async Task InitializeAsync(CancellationToken cancellationToken = default)
	{
		try
		{
			using var scope = _serviceProvider.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

			// 1. اعمال Migrationها
			_logger.LogInformation("Applying migrations...");
			await context.Database.MigrateAsync(cancellationToken);

			// 2. Seederها به ترتیب
			// 2.1 Permissionها (اول از همه چون Roleها بهشون نیاز دارن)
			await RunSeederAsync<PermissionSeeder>(scope, cancellationToken);

			// 2.2 Roleها (نیاز به Permissionها دارن)
			await RunSeederAsync<RoleSeeder>(scope, cancellationToken);

			// 2.3 Admin User (نیاز به Roleها داره)
			await RunSeederAsync<AdminUserSeeder>(scope, cancellationToken);

			// 2.4 داده‌های پایه‌ی قهوه
			await RunSeederAsync<CoffeeDataSeeder>(scope, cancellationToken);

			// 2.5 دسته‌بندی‌ها
			await RunSeederAsync<CategorySeeder>(scope, cancellationToken);

			// 2.6 ویژگی‌ها
			await RunSeederAsync<AttributeSeeder>(scope, cancellationToken);

			// 2.7 روش‌های ارسال
			await RunSeederAsync<ShippingMethodSeeder>(scope, cancellationToken);
			await RunSeederAsync<SettingSeeder>(scope, cancellationToken);

			_logger.LogInformation("Database initialization completed successfully. ✅");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while initializing the database.");
			throw;
		}
	}

	private static async Task RunSeederAsync<TSeeder>(
		IServiceScope scope,
		CancellationToken cancellationToken)
		where TSeeder : class
	{
		var seeder = scope.ServiceProvider.GetRequiredService<TSeeder>();
		var method = typeof(TSeeder).GetMethod("SeedAsync");

		if (method is null) return;

			// 2.9 مقالات دمو وبلاگ
			await RunSeederAsync<BlogSeeder>(scope, cancellationToken);


			_logger.LogInformation("Database initialization completed successfully. ✅");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while initializing the database.");
			throw;
		}
	}

	private static async Task RunSeederAsync<TSeeder>(
		IServiceScope scope,
		CancellationToken cancellationToken)
		where TSeeder : class
	{
		var seeder = scope.ServiceProvider.GetRequiredService<TSeeder>();
		var method = typeof(TSeeder).GetMethod("SeedAsync");

		if (method is null) return;

		var task = method.Invoke(seeder, new object[] { cancellationToken }) as Task;
		if (task is not null)
			await task;
	}
}