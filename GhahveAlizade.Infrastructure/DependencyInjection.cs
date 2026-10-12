using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Identity;
using GhahveAlizade.Infrastructure.Identity;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence;
using GhahveAlizade.Infrastructure.Persistence.Interceptors;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using GhahveAlizade.Infrastructure.Seo;
using GhahveAlizade.Infrastructure.Services;
using GhahveAlizade.Infrastructure.Services.Ai;
using GhahveAlizade.Infrastructure.Services.Sms;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

namespace GhahveAlizade.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		// DbContext
		services.AddScoped<AuditableEntityInterceptor>();
		services.AddScoped<SoftDeleteInterceptor>();

		services.AddDbContext<AppDbContext>((sp, options) =>
		{
			var connectionString = configuration.GetConnectionString("DefaultConnection")
				?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

			options.UseSqlServer(connectionString, sql =>
			{
				sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
				sql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), null);
				sql.CommandTimeout(60);
			});

			options.AddInterceptors(
				sp.GetRequiredService<AuditableEntityInterceptor>(),
				sp.GetRequiredService<SoftDeleteInterceptor>());

			options.EnableSensitiveDataLogging(false);
			options.EnableDetailedErrors();
		});

		services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());

		// Identity
		services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
		{
			options.Password.RequireDigit = true;
			options.Password.RequiredLength = 8;
			options.Password.RequireNonAlphanumeric = false;
			options.Password.RequireUppercase = false;
			options.Password.RequireLowercase = false;

			options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
			options.Lockout.MaxFailedAccessAttempts = 5;
			options.Lockout.AllowedForNewUsers = true;

			options.User.RequireUniqueEmail = true;
			options.SignIn.RequireConfirmedEmail = false;
			options.SignIn.RequireConfirmedPhoneNumber = false;
		})
		.AddEntityFrameworkStores<AppDbContext>()
		.AddDefaultTokenProviders();

		services.Configure<SeoOptions>(configuration.GetSection(SeoOptions.SectionName));
		services.AddScoped<ISeoHelper, SeoHelper>();
		services.AddScoped<ISchemaBuilder, SchemaBuilder>();
		// ============ JWT Settings 绑定 ============
		var jwtSettings = new JwtSettings();
		configuration.GetSection("Jwt").Bind(jwtSettings);
		services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
		services.AddScoped<IJwtTokenService, JwtTokenService>();

		// Auth Services
		services.AddScoped<IAuthenticationService, AuthenticationService>();
		services.AddScoped<ICookieAuthService, CookieAuthService>();
		// ============ Cookie + JWT 双认证 ============
		services.AddAuthentication(options =>
		{
			// 默认：Cookie（MVC 页面用）
			options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
		})
		.AddCookie(options =>
		{
			options.LoginPath = "/Account/Login";
			options.LogoutPath = "/Account/Logout";
			options.AccessDeniedPath = "/Account/AccessDenied";
			options.ExpireTimeSpan = TimeSpan.FromDays(7);
			options.SlidingExpiration = true;
			options.Cookie.Name = "GhahveAuth";
			options.Cookie.HttpOnly = true;
			options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
			options.Cookie.SameSite = SameSiteMode.Lax;
		})
		.AddJwtBearer(options =>
		{
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = jwtSettings.Issuer,
				ValidAudience = jwtSettings.Audience,
				IssuerSigningKey = new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
				ClockSkew = TimeSpan.Zero
			};
		});
		// Services
		services.AddHttpContextAccessor();
		services.AddScoped<ICurrentUserService, CurrentUserService>();
		services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
		services.AddScoped<IFileStorageService, LocalFileStorageService>();
		services.AddSingleton<IImageValidator, ImageDimensionValidator>();
		// Redis Cache
		var redisConnection = configuration.GetConnectionString("Redis")
			?? configuration["Redis:ConnectionString"];

		if (!string.IsNullOrEmpty(redisConnection))
		{
			services.AddStackExchangeRedisCache(options =>
			{
				options.Configuration = redisConnection;
				options.InstanceName = "GhahveAlizade:";
			});
			services.AddSingleton<ICacheService, RedisCacheService>();
		}
		else
		{
			services.AddDistributedMemoryCache();
			services.AddSingleton<ICacheService, RedisCacheService>();
		}
		services.AddScoped<IIdentityService, IdentityService>();
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

		// ============ AI (Gemini — free tier) ============
		services.Configure<GeminiOptions>(configuration.GetSection(GeminiOptions.SectionName));
		services.AddHttpClient<IAiChatService, GeminiChatService>(client =>
		{
			client.Timeout = TimeSpan.FromSeconds(60);
			client.DefaultRequestHeaders.Add("User-Agent", "GhahveAlizade/1.0");
		});

		return services;
	}
}
