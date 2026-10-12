using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GhahveAlizade.Web.Controllers;

[Route("Error")]
public class ErrorController : Controller
{
	private readonly ILogger<ErrorController> _logger;

	public ErrorController(ILogger<ErrorController> logger) => _logger = logger;

	[Route("")]
	[Route("{statusCode:int}")]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Index(int? statusCode = null)
	{
		var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
		var originalPath = feature?.OriginalPath ?? HttpContext.Request.Path;

		var model = new ErrorViewModel
		{
			StatusCode = statusCode ?? HttpContext.Response.StatusCode,
			OriginalPath = originalPath,
			TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
		};

		_logger.LogWarning(
			"صفحه‌ی خطا نمایش داده شد. StatusCode: {StatusCode}, Path: {Path}, TraceId: {TraceId}",
			model.StatusCode, originalPath, model.TraceId);

		return View(model);
	}

	[Route("Problem")]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Problem()
	{
		if (HttpContext.Items["ProblemDetails"] is Microsoft.AspNetCore.Mvc.ProblemDetails problem)
		{
			var model = new ErrorViewModel
			{
				StatusCode = problem.Status ?? 500,
				Title = problem.Title,
				Detail = problem.Detail,
				TraceId = problem.Extensions.TryGetValue("traceId", out var tid) ? tid?.ToString() : null,
				OriginalPath = HttpContext.Request.Path
			};

			return View("Index", model);
		}

		return RedirectToAction(nameof(Index));
	}
}

public class ErrorViewModel
{
	public int StatusCode { get; set; } = 500;
	public string? Title { get; set; }
	public string? Detail { get; set; }
	public string? OriginalPath { get; set; }
	public string? TraceId { get; set; }

	public string GetTitle() => Title ?? StatusCode switch
	{
		400 => "درخواست نامعتبر",
		401 => "احراز هویت نشده",
		403 => "دسترسی ممنوع",
		404 => "صفحه یافت نشد",
		429 => "تعداد درخواست بیش از حد",
		500 => "خطای داخلی سرور",
		502 => "خطای درگاه",
		503 => "سرویس در دسترس نیست",
		_ => "خطا"
	};

	public string GetDetail() => Detail ?? StatusCode switch
	{
		404 => "صفحه‌ای که به دنبال آن هستید یافت نشد.",
		403 => "شما به این صفحه دسترسی ندارید.",
		500 => "خطایی رخ داد. لطفاً بعداً تلاش کنید یا با پشتیبانی تماس بگیرید.",
		429 => "لطفاً چند لحظه صبر کنید و دوباره تلاش کنید.",
		_ => "خطایی رخ داد."
	};
}