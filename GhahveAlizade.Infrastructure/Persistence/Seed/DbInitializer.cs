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