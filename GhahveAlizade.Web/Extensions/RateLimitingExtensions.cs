using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace GhahveAlizade.Web.Extensions;

public static class RateLimitingExtensions
{
	public const string OtpPolicy = "otp";
	public const string LoginPolicy = "login";
	public const string ApiPolicy = "api";
	public const string PublicPolicy = "public";

	public static IServiceCollection AddAppRateLimiting(this IServiceCollection services)
	{
		services.AddRateLimiter(options =>
		{
			// پاسخ در صورت رد شدن
			options.OnRejected = async (context, cancellationToken) =>
			{
				context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
				context.HttpContext.Response.ContentType = "application/json; charset=utf-8";

				var retryAfter = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retry)
					? (int)retry.TotalSeconds
					: 60;

				context.HttpContext.Response.Headers["Retry-After"] = retryAfter.ToString();

				await context.HttpContext.Response.WriteAsJsonAsync(new
				{
					type = "https://tools.ietf.org/html/rfc6585#section-4",
					title = "تعداد درخواست‌ها بیش از حد مجاز",
					status = 429,
					detail = $"لطفاً {retryAfter} ثانیه دیگر دوباره تلاش کنید.",
					errorCode = "RateLimit.Exceeded",
					retryAfter
				}, cancellationToken);
			};

			// OTP: حداکثر 3 بار در 5 دقیقه برای هر شماره موبایل + IP
			options.AddPolicy(OtpPolicy, httpContext =>
				RateLimitPartition.GetFixedWindowLimiter(
					partitionKey: $"otp_{GetClientIp(httpContext)}_{httpContext.Request.Query["phoneNumber"]}",
					factory: _ => new FixedWindowRateLimiterOptions
					{
						PermitLimit = 3,
						Window = TimeSpan.FromMinutes(5),
						QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
						QueueLimit = 0
					}));

			// Login: حداکثر 5 بار در 10 دقیقه برای هر IP
			options.AddPolicy(LoginPolicy, httpContext =>
				RateLimitPartition.GetFixedWindowLimiter(
					partitionKey: $"login_{GetClientIp(httpContext)}",
					factory: _ => new FixedWindowRateLimiterOptions
					{
						PermitLimit = 5,
						Window = TimeSpan.FromMinutes(10),
						QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
						QueueLimit = 0
					}));

			// API عمومی: 100 درخواست در دقیقه
			options.AddPolicy(ApiPolicy, httpContext =>
				RateLimitPartition.GetFixedWindowLimiter(
					partitionKey: $"api_{GetClientIp(httpContext)}",
					factory: _ => new FixedWindowRateLimiterOptions
					{
						PermitLimit = 100,
						Window = TimeSpan.FromMinutes(1),
						QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
						QueueLimit = 10
					}));

			// Global fallback
			options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
				httpContext => RateLimitPartition.GetFixedWindowLimiter(
					partitionKey: GetClientIp(httpContext),
					factory: _ => new FixedWindowRateLimiterOptions
					{
						PermitLimit = 500,
						Window = TimeSpan.FromMinutes(1),
						QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
						QueueLimit = 20
					}));
		});

		return services;
	}

	private static string GetClientIp(HttpContext context)
	{
		// بررسی X-Forwarded-For برای پشت Proxy
		var forwarded = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
		if (!string.IsNullOrEmpty(forwarded))
			return forwarded.Split(',')[0].Trim();

		return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
	}
}