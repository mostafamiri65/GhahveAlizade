using GhahveAlizade.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GhahveAlizade.Web.Middleware;

/// <summary>
/// Global Exception Handler بر اساس IExceptionHandler در .NET 8+
/// این handler به طور خودکار تمام Exceptionهای مدیریت‌نشده رو به ProblemDetails تبدیل می‌کنه.
/// </summary>
public sealed class GlobalExceptionHandler : IExceptionHandler
{
	private readonly ILogger<GlobalExceptionHandler> _logger;
	private readonly IHostEnvironment _environment;
	private readonly IProblemDetailsService _problemDetailsService;

	public GlobalExceptionHandler(
		ILogger<GlobalExceptionHandler> logger,
		IHostEnvironment environment,
		IProblemDetailsService problemDetailsService)
	{
		_logger = logger;
		_environment = environment;
		_problemDetailsService = problemDetailsService;
	}

	public async ValueTask<bool> TryHandleAsync(
		HttpContext httpContext,
		Exception exception,
		CancellationToken cancellationToken)
	{
		var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

		// لاگ کردن
		LogException(exception, httpContext, traceId);

		// تعیین StatusCode و ProblemDetails
		var problemDetails = CreateProblemDetails(exception, httpContext, traceId);

		httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;

		// بررسی کن آیا Client انتظار JSON داره یا HTML
		var accepts = httpContext.Request.Headers.Accept.ToString();
		var isApiRequest = httpContext.Request.Path.StartsWithSegments("/api")
						|| accepts.Contains("application/json", StringComparison.OrdinalIgnoreCase);

		if (isApiRequest)
		{
			return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
			{
				HttpContext = httpContext,
				Exception = exception,
				ProblemDetails = problemDetails
			});
		}

		// برای درخواست‌های HTML، صفحه‌ی خطا نشون بده
		httpContext.Items["ProblemDetails"] = problemDetails;
		httpContext.Request.Path = "/Error/Problem";
		return false;
	}

	private void LogException(Exception exception, HttpContext httpContext, string traceId)
	{
		var logLevel = exception switch
		{
			AppException app when app.StatusCode < 500 => LogLevel.Warning,
			OperationCanceledException => LogLevel.Information,
			_ => LogLevel.Error
		};

		_logger.Log(
			logLevel,
			exception,
			"Exception در {Method} {Path} رخ داد. TraceId: {TraceId}, UserId: {UserId}, IP: {IpAddress}",
			httpContext.Request.Method,
			httpContext.Request.Path,
			traceId,
			httpContext.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "Anonymous",
			httpContext.Connection.RemoteIpAddress?.ToString());
	}

	private ProblemDetails CreateProblemDetails(
		Exception exception,
		HttpContext httpContext,
		string traceId)
	{
		return exception switch
		{
			ValidationException validationEx => new ValidationProblemDetails(validationEx.Errors)
			{
				Title = "خطای اعتبارسنجی",
				Detail = validationEx.Message,
				Status = validationEx.StatusCode,
				Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
				Instance = httpContext.Request.Path,
				Extensions =
				{
					["errorCode"] = validationEx.ErrorCode,
					["traceId"] = traceId
				}
			},

			AppException appEx => new ProblemDetails
			{
				Title = GetTitleForStatusCode(appEx.StatusCode),
				Detail = appEx.Message,
				Status = appEx.StatusCode,
				Type = GetTypeForStatusCode(appEx.StatusCode),
				Instance = httpContext.Request.Path,
				Extensions =
				{
					["errorCode"] = appEx.ErrorCode,
					["traceId"] = traceId
				}
			},

			OperationCanceledException => new ProblemDetails
			{
				Title = "درخواست لغو شد",
				Detail = "درخواست توسط کاربر لغو شد.",
				Status = 499,
				Instance = httpContext.Request.Path
			},

			UnauthorizedAccessException => new ProblemDetails
			{
				Title = "دسترسی غیرمجاز",
				Detail = "شما دسترسی لازم برای این عملیات را ندارید.",
				Status = StatusCodes.Status403Forbidden,
				Instance = httpContext.Request.Path,
				Extensions = { ["traceId"] = traceId }
			},

			// خطای عمومی در Production
			_ => new ProblemDetails
			{
				Title = "خطای داخلی سرور",
				Detail = _environment.IsDevelopment()
					? exception.ToString()
					: "خطایی رخ داد. لطفاً دوباره تلاش کنید یا با پشتیبانی تماس بگیرید.",
				Status = StatusCodes.Status500InternalServerError,
				Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
				Instance = httpContext.Request.Path,
				Extensions =
				{
					["traceId"] = traceId,
					["exceptionType"] = _environment.IsDevelopment() ? exception.GetType().Name : null
				}
			}
		};
	}

	private static string GetTitleForStatusCode(int statusCode) => statusCode switch
	{
		400 => "درخواست نامعتبر",
		401 => "احراز هویت نشده",
		403 => "دسترسی ممنوع",
		404 => "یافت نشد",
		409 => "تعارض",
		422 => "قوانین Business نقض شد",
		429 => "تعداد درخواست بیش از حد مجاز",
		_ => "خطای داخلی سرور"
	};

	private static string GetTypeForStatusCode(int statusCode) => statusCode switch
	{
		400 => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
		401 => "https://tools.ietf.org/html/rfc7235#section-3.1",
		403 => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
		404 => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
		409 => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
		422 => "https://tools.ietf.org/html/rfc4918#section-11.2",
		429 => "https://tools.ietf.org/html/rfc6585#section-4",
		_ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
	};
}