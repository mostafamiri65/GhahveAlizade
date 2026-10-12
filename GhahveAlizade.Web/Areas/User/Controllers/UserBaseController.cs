using ErrorOr;
using GhahveAlizade.Web.Controllers;
using GhahveAlizade.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.User.Controllers;

/// <summary>
/// BaseController برای همه‌ی Controllerهای User Area (پنل کاربری).
/// نیاز به احراز هویت داره.
/// UserId همیشه از Claim خوانده می‌شود — هرگز از route/query پذیرفته نمی‌شود.
/// </summary>
[Area("User")]
[Authorize]
[Route("user/[controller]/[action]")]
public abstract class UserBaseController : BaseController
{
	protected long CurrentUserId =>
		User.GetUserId()
		?? throw new UnauthorizedAccessException("کاربر احراز هویت نشده است.");

	protected IActionResult ErrorOrView<T>(ErrorOr<T> result, string? viewName = null)
	{
		if (result.IsError)
		{
			TempData["ErrorMessage"] = result.FirstError.Description;
			return RedirectToAction(nameof(Index));
		}
		return viewName is null ? View(result.Value) : View(viewName, result.Value);
	}
}
