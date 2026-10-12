using ErrorOr;
using GhahveAlizade.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Controllers;

/// <summary>
/// BaseController مخصوص MVC (View-based).
/// - از View و PartialView پشتیبانی می‌کنه
/// - خطاها به صورت ModelState نمایش داده می‌شن
/// </summary>
public abstract class MvcBaseController : BaseController
{
	// ⬅️ حالا از BaseController ارث می‌بره، پس Mediator رو داره

	/// <summary>
	/// تبدیل ErrorOr&lt;T&gt; به ModelState + نمایش View.
	/// </summary>
	protected IActionResult HandleErrorsAndReturn<T>(
		ErrorOr<T> result,
		Func<ErrorOr<T>, IActionResult> onSuccess)
	{
		if (result.IsError)
		{
			AddErrorsToModelState(result.Errors);
			return View();
		}

		return onSuccess(result);
	}

	/// <summary>
	/// نمایش صفحه خطای عمومی سایت برای خطاهای سطح صفحه (مثلاً داده پیدا نشد).
	/// </summary>
	protected IActionResult HandleError(Error error)
	{
		ViewData["ErrorTitle"] = error.Description;
		return View("~/Views/Shared/Error.cshtml", new ErrorViewModel
		{
			TraceId = HttpContext.TraceIdentifier
		});
	}

	/// <summary>
	/// افزودن Errorها به ModelState.
	/// </summary>
	protected void AddErrorsToModelState(IEnumerable<Error> errors)
	{
		foreach (var error in errors)
		{
			var key = string.IsNullOrEmpty(error.Code) ? string.Empty : error.Code;
			ModelState.AddModelError(key, error.Description);
		}
	}

	/// <summary>
	/// Redirect به URL محلی یا Home.
	/// </summary>
	protected IActionResult RedirectToLocal(string? returnUrl)
	{
		if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
			return Redirect(returnUrl);

		return RedirectToAction("Index", "Home", new { area = "" });
	}
}