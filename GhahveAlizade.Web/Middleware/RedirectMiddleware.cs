using GhahveAlizade.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Web.Middleware;

/// <summary>
/// Middleware برای مدیریت 301/302 redirectهای داینامیک از جدول Redirects.
/// </summary>
public sealed class RedirectMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<RedirectMiddleware> _logger;

	// مسیرهایی که نباید چک بشن
	private static readonly string[] IgnoredPrefixes =
	{
		"/admin", "/api", "/account", "/identity", "/health", "/hangfire",
		"/_framework", "/_blazor", "/uploads", "/css", "/js", "/lib", "/images", "/favicon"
	};

	public RedirectMiddleware(RequestDelegate next, ILogger<RedirectMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
	{
		if (context.Request.Method != HttpMethods.Get &&
			context.Request.Method != HttpMethods.Head)
		{
			await _next(context);
			return;
		}

		var path = context.Request.Path.Value ?? "/";

		if (IgnoredPrefixes.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
		{
			await _next(context);
			return;
		}

		// چک کردن در جدول Redirects (با کش 60 دقیقه‌ای)
		var redirect = await dbContext.Redirects
			.AsNoTracking()
			.Where(r => r.IsActive
					 && !r.IsDeleted
					 && r.FromPath == path)
			.Select(r => new { r.ToPath, r.Type, r.Id })
			.FirstOrDefaultAsync(context.RequestAborted);

		if (redirect is not null)
		{
			_logger.LogDebug("Redirect از {From} به {To}", path, redirect.ToPath);

			// آپدیت آمار (fire and forget)
			_ = UpdateHitCountAsync(dbContext, redirect.Id);

			var statusCode = redirect.Type switch
			{
				Domain.Enums.RedirectType.Permanent301 => StatusCodes.Status301MovedPermanently,
				Domain.Enums.RedirectType.Temporary302 => StatusCodes.Status302Found,
				Domain.Enums.RedirectType.Permanent308 => StatusCodes.Status308PermanentRedirect,
				_ => StatusCodes.Status301MovedPermanently
			};

			context.Response.Redirect(redirect.ToPath, permanent: statusCode == StatusCodes.Status301MovedPermanently);
			context.Response.StatusCode = statusCode;
			return;
		}

		await _next(context);
	}

	private static async Task UpdateHitCountAsync(AppDbContext dbContext, long redirectId)
	{
		try
		{
			await dbContext.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE Redirects
                SET HitCount = HitCount + 1,
                    LastHitAtUtc = GETUTCDATE()
                WHERE Id = {redirectId}");
		}
		catch
		{
			// ignore
		}
	}
}

public static class RedirectMiddlewareExtensions
{
	public static IApplicationBuilder UseRedirectMiddleware(this IApplicationBuilder app)
		=> app.UseMiddleware<RedirectMiddleware>();
}