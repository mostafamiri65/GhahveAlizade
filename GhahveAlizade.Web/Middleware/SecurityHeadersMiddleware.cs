namespace GhahveAlizade.Web.Middleware;

/// <summary>
/// اضافه کردن Headerهای امنیتی به تمام Responseها.
/// این هدرها برای محافظت از XSS، Clickjacking، MIME-Sniffing و ... استفاده می‌شن.
/// </summary>
public sealed class SecurityHeadersMiddleware
{
	private readonly RequestDelegate _next;
	private readonly IWebHostEnvironment _environment;

	public SecurityHeadersMiddleware(RequestDelegate next, IWebHostEnvironment environment)
	{
		_next = next;
		_environment = environment;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		var headers = context.Response.Headers;

		// Content Security Policy
		// توجه: در Development برای Hot Reload شل‌تر هست
		var csp = _environment.IsDevelopment()
			? "default-src 'self' 'unsafe-inline' 'unsafe-eval' data: blob: ws: wss: https:; " +
			  "img-src 'self' data: blob: https:; " +
			  "font-src 'self' data: https:; " +
			  "frame-ancestors 'none';"
			: "default-src 'self'; " +
			  "script-src 'self' https://www.googletagmanager.com https://www.google-analytics.com; " +
			  "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com; " +
			  "font-src 'self' https://fonts.gstatic.com; " +
			  "img-src 'self' data: https:; " +
			  "connect-src 'self' https://www.google-analytics.com; " +
			  "frame-ancestors 'none'; " +
			  "base-uri 'self'; " +
			  "form-action 'self';";

		headers["Content-Security-Policy"] = csp;

		// جلوگیری از Clickjacking
		headers["X-Frame-Options"] = "DENY";

		// جلوگیری از MIME-Sniffing
		headers["X-Content-Type-Options"] = "nosniff";

		// Referrer Policy
		headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

		// Permissions Policy (جلوگیری از دسترسی‌های حساس)
		headers["Permissions-Policy"] =
			"camera=(), " +
			"microphone=(), " +
			"geolocation=(self), " +
			"interest-cohort=()";

		// XSS Protection (برای مرورگرهای قدیمی)
		headers["X-XSS-Protection"] = "1; mode=block";

		// HSTS - فقط در Production
		if (!_environment.IsDevelopment())
		{
			headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains; preload";
		}

		// حذف هدرهایی که اطلاعات سرور رو لو می‌دن
		headers.Remove("Server");
		headers.Remove("X-Powered-By");
		headers.Remove("X-AspNet-Version");
		headers.Remove("X-AspNetMvc-Version");

		await _next(context);
	}
}

public static class SecurityHeadersMiddlewareExtensions
{
	public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
		=> app.UseMiddleware<SecurityHeadersMiddleware>();
}