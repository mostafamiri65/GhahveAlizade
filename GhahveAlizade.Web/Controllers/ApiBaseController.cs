using ErrorOr;
using GhahveAlizade.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Controllers;

/// <summary>
/// BaseController مخصوص API.
/// - فقط JSON برمی‌گردونه
/// - خطاها به ProblemDetails تبدیل می‌شن
/// </summary>
[ApiController]
[Produces("application/json")]
public abstract class ApiBaseController : BaseController
{
	/// <summary>
	/// درخواست API با پاسخ JSON.
	/// </summary>
	protected IActionResult ApiResponse<T>(ErrorOr<T> result) => result.ToActionResult();
}