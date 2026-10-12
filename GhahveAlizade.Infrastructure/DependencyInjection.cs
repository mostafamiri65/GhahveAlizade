		services.AddScoped<PermissionSeeder>();
		services.AddScoped<RoleSeeder>();
		services.AddScoped<AdminUserSeeder>();
		services.AddScoped<CoffeeDataSeeder>();
		services.AddScoped<CategorySeeder>();
		services.AddScoped<AttributeSeeder>();
		services.AddScoped<ShippingMethodSeeder>();
		services.AddScoped<SettingSeeder>();
		services.AddScoped<BrandSeeder>();
		services.AddScoped<ProductSeeder>();
		services.AddScoped<BlogSeeder>();
		services.AddScoped<DbInitializer>();

		services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, AppClaimsPrincipalFactory>();

		services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
		services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
		// ============ OTP & SMS ============
		services.Configure<KavenegarSettings>(configuration.GetSection("Kavenegar"));

		services.AddHttpClient<ISmsService, KavenegarSmsService>(client =>
		{
			client.Timeout = TimeSpan.FromSeconds(30);
			client.DefaultRequestHeaders.Add("User-Agent", "GhahveAlizade/1.0");
		});

		services.AddScoped<IOtpService, OtpService>();
		return services;
	}
}