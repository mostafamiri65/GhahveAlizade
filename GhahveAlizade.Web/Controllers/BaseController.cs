using ErrorOr;
using GhahveAlizade.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Controllers;

/// <summary>
/// BaseController پایه برای تمام Controllerهای MVC و API.
/// ⚠️ از Controller (نه ControllerBase) ارث می‌بره تا View() در دسترس باشه.
/// </summary>
public abstract class BaseController : Controller
{
	private ISender? _mediator;

	protected ISender Mediator =>
		_mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

	protected bool IsApiRequest
		=> Request.Path.StartsWithSegments("/api")
		   || Request.Headers.Accept.ToString().Contains("application/json", StringComparison.OrdinalIgnoreCase);

	protected IActionResult Problem(List<Error> errors) => errors.ToProblemDetails();

	protected IActionResult OkOrProblem<T>(ErrorOr<T> result) => result.ToActionResult();

	protected IActionResult NoContentOrProblem(ErrorOr<Success> result) => result.ToNoContentResult();

	protected IActionResult CreatedOrProblem(ErrorOr<Created> result, string? location = null)
		=> result.ToCreatedResult(location);
}