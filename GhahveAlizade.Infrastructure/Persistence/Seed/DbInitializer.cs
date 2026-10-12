			// 2.4 داده‌های پایه‌ی قهوه
			await RunSeederAsync<CoffeeDataSeeder>(scope, cancellationToken);

			// 2.4.1 برندهای دمو
			await RunSeederAsync<BrandSeeder>(scope, cancellationToken);

			// 2.5 دسته‌بندی‌ها
			await RunSeederAsync<CategorySeeder>(scope, cancellationToken);

			// 2.6 ویژگی‌ها
			await RunSeederAsync<AttributeSeeder>(scope, cancellationToken);

			// 2.7 روش‌های ارسال
			await RunSeederAsync<ShippingMethodSeeder>(scope, cancellationToken);
			await RunSeederAsync<SettingSeeder>(scope, cancellationToken);

			// 2.8 محصولات دمو (بعد از دسته‌بندی‌ها و برندها)
			await RunSeederAsync<ProductSeeder>(scope, cancellationToken);

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