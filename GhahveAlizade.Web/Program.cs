using GhahveAlizade.Application;
using GhahveAlizade.Infrastructure;
using GhahveAlizade.Infrastructure.Persistence;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using GhahveAlizade.Web.Extensions;
using GhahveAlizade.Web.Filters;
using GhahveAlizade.Web.Middleware;
using Hangfire;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Data.SqlClient;
using Serilog;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

// ============ Serilog ============
builder.Host.UseSerilog((context, services, config) =>
	config.ReadFrom.Configuration(context.Configuration)
		  .ReadFrom.Services(services)
		  .Enrich.FromLogContext()
		  .Enrich.WithProperty("Application", "GhahveAlizade"));

// ============ Application + Infrastructure ============
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ============ MVC ============
builder.Services.AddControllersWithViews(options =>
{
	// فیلتر امنیتی عمومی
	options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
})
.AddRazorOptions(options =>
{
	// اضافه کردن مسیر Areas
	options.AreaViewLocationFormats.Clear();
	options.AreaViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}.cshtml");
	options.AreaViewLocationFormats.Add("/Areas/{2}/Views/Shared/{0}.cshtml");
	options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
});

builder.Services.AddRazorPages();

// ============ Output Caching ============
builder.Services.AddOutputCache(options =>
{
	options.AddBasePolicy(b => b.Expire(TimeSpan.FromMinutes(5)));

	options.AddPolicy("Products", b => b
		.Expire(TimeSpan.FromMinutes(15))
		.SetVaryByQuery("page", "category", "brand", "sort"));

	options.AddPolicy("HomePage", b => b
		.Expire(TimeSpan.FromMinutes(30)));

	options.AddPolicy("Sitemap", b => b
		.Expire(TimeSpan.FromHours(1)));
});

// ============ Response Compression ============
builder.Services.AddResponseCompression(options =>
{
	options.EnableForHttps = true;
	options.Providers.Add<BrotliCompressionProvider>();
	options.Providers.Add<GzipCompressionProvider>();
	options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
	{
		"application/xml",
		"application/rss+xml",
		"image/svg+xml"
	});
});

builder.Services.Configure<BrotliCompressionProviderOptions>(o =>
	o.Level = CompressionLevel.Fastest);

builder.Services.Configure<GzipCompressionProviderOptions>(o =>
	o.Level = CompressionLevel.SmallestSize);
// ============ ProblemDetails ============
builder.Services.AddProblemDetails(options =>
{
	options.CustomizeProblemDetails = ctx =>
	{
		ctx.ProblemDetails.Extensions["traceId"] = ctx.HttpContext.TraceIdentifier;
		ctx.ProblemDetails.Extensions["timestamp"] = DateTime.UtcNow;
	};
});

// ============ Global Exception Handler ============
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// ============ HSTS ============
builder.Services.AddHsts(options =>
{
	options.MaxAge = TimeSpan.FromDays(365);
	options.IncludeSubDomains = true;
	options.Preload = true;
});

// ============ HSTS + HTTPS ============
builder.Services.AddHttpsRedirection(options =>
{
	options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
});
builder.Services.Configure<FormOptions>(options =>
{
	options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10 MB
});
// ============ Rate Limiting ============
builder.Services.AddAppRateLimiting();

// ============ Health Checks ============
builder.Services.AddHealthChecks()
	.AddSqlServer(
		builder.Configuration.GetConnectionString("DefaultConnection")!,
		name: "sqlserver",
		tags: new[] { "db", "sql" })
	.AddRedis(
		builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379",
		name: "redis",
		tags: new[] { "cache" });

// ============ Hangfire ============

// ۱. یک Connection String موقت به دیتابیس master بساز
string hangfireConnectionString = builder.Configuration.GetConnectionString("HangfireConnection")!;

using (var connection = new SqlConnection(hangfireConnectionString.Replace("Database=GhahveAlizadeJobs;", "Database=master;")))
{
	connection.Open();
	using (var command = new SqlCommand(
		@"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'GhahveAlizadeJobs') 
          CREATE DATABASE [GhahveAlizadeJobs];", connection))
	{
		command.ExecuteNonQuery();
	}
}

builder.Services.AddHangfire(config => config
	.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
	.UseSimpleAssemblyNameTypeSerializer()
	.UseRecommendedSerializerSettings()
	.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));

builder.Services.AddHangfireServer(options =>
{
	options.WorkerCount = Environment.ProcessorCount * 2;
});

// ============ Forwarded Headers (برای پشت Nginx/Cloudflare) ============
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
	options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

	// پاک کردن مجموعه قدیمی (توسط کامپایلر هشدار داده می‌شود، با pragma آن را نادیده بگیرید)
#pragma warning disable ASPDEPR005
	options.KnownNetworks.Clear();
#pragma warning restore ASPDEPR005

	// پاک کردن مجموعه جدید
	options.KnownIPNetworks.Clear();

	options.KnownProxies.Clear();
});

var app = builder.Build();

// ============ Migrate & Seed ============
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
	using var scope = app.Services.CreateScope();
	var initializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
	await initializer.InitializeAsync();
}

// ============ Middleware Pipeline ============
app.UseForwardedHeaders();

// Exception Handler اول از همه
app.UseExceptionHandler();

// StatusCode Pages برای 404 و ...
app.UseStatusCodePagesWithReExecute("/Error/{0}");

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}
else
{
	app.UseHsts();
}

// CorrelationId قبل از همه چیز
app.UseCorrelationId();

app.UseResponseCompression();

// Security Headers
app.UseSecurityHeaders();

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
	OnPrepareResponse = ctx =>
	{
		// Cache استاتیک‌ها برای یک سال
		const int durationInSeconds = 60 * 60 * 24 * 365;
		ctx.Context.Response.Headers.CacheControl = $"public,max-age={durationInSeconds},immutable";
	}
});

app.UseRouting();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();
app.UseRedirectMiddleware();

app.UseOutputCache();

// Serilog Request Logging - بعد از Auth تا بتونیم UserId رو لاگ کنیم
app.UseSerilogRequestLogging(options =>
{
	options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
	{
		diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
		diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
		diagnosticContext.Set("UserAgent", httpContext.Request.Headers.UserAgent.ToString());
		diagnosticContext.Set("UserId",
			httpContext.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "Anonymous");
	};
});

// Areas route
app.MapControllerRoute(
	name: "areas",
	pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// Default route
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// Health Checks
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
	ResponseWriter = async (context, report) =>
	{
		context.Response.ContentType = "application/json";
		var result = System.Text.Json.JsonSerializer.Serialize(new
		{
			status = report.Status.ToString(),
			checks = report.Entries.Select(e => new
			{
				name = e.Key,
				status = e.Value.Status.ToString(),
				duration = e.Value.Duration.TotalMilliseconds,
				description = e.Value.Description
			}),
			totalDuration = report.TotalDuration.TotalMilliseconds
		});
		await context.Response.WriteAsync(result);
	}
});

// Hangfire Dashboard - فقط برای ادمین
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
	Authorization = new[] { new HangfireAdminAuthorizationFilter() },
	DashboardTitle = "GhahveAlizade Background Jobs"
});

app.Run();