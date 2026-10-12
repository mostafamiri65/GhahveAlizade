using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Extensions;

/// <summary>
/// تبدیل ErrorOr Result به IActionResult مناسب برای Controller.
/// </summary>
public static class ResultExtensions
{
	/// <summary>
	/// تبدیل ErrorOr<T> به IActionResult بر اساس نوع خطا.
	/// </summary>
	public static IActionResult ToActionResult<T>(this ErrorOr<T> result)
		=> result.Match(
			value => new OkObjectResult(value),
			errors => errors.ToProblemDetails());

	/// <summary>
	/// تبدیل ErrorOr<T> به IActionResult با StatusCode دلخواه برای موفقیت.
	/// </summary>
	public static IActionResult ToActionResult<T>(this ErrorOr<T> result, int successStatusCode)
		=> result.Match(
			value => new ObjectResult(value) { StatusCode = successStatusCode },
			errors => errors.ToProblemDetails());

	/// <summary>
	/// تبدیل ErrorOr<Success> به IActionResult (204 No Content).
	/// </summary>
	public static IActionResult ToNoContentResult(this ErrorOr<Success> result)
		=> result.Match<IActionResult>(
			_ => new NoContentResult(),
			errors => errors.ToProblemDetails());

	/// <summary>
	/// تبدیل ErrorOr<Created> به IActionResult (201 Created).
	/// </summary>
	public static IActionResult ToCreatedResult(this ErrorOr<Created> result, string? location = null)
		=> result.Match<IActionResult>(
			_ =>
			{
				if (!string.IsNullOrEmpty(location))
					return new CreatedResult(location, null);
				return new StatusCodeResult(StatusCodes.Status201Created);
			},
			errors => errors.ToProblemDetails());

	/// <summary>
	/// تبدیل ErrorOr<T> به IActionResult مخصوص Redirect (برای MVC).
	/// </summary>
	public static IActionResult ToRedirectResult<T>(this ErrorOr<T> result, string redirectUrl)
		=> result.Match<IActionResult>(
			_ => new RedirectResult(redirectUrl),
			errors => errors.ToProblemDetails());

	/// <summary>
	/// تبدیل لیست Error به ProblemDetails.
	/// </summary>
	public static IActionResult ToProblemDetails(this List<Error> errors)
	{
		if (errors.Count == 0)
			return new BadRequestResult();

		// اگه همه‌ی خطاها Validation هستن → ValidationProblemDetails
		if (errors.All(e => e.Type == ErrorType.Validation))
			return CreateValidationProblemDetails(errors);

		// در غیر این صورت، اولین خطا رو به عنوان خطای اصلی در نظر بگیر
		var firstError = errors.First();
		return CreateProblemDetails(firstError);
	}

	private static IActionResult CreateValidationProblemDetails(List<Error> errors)
	{
		var errorDict = errors
			.GroupBy(e => string.IsNullOrEmpty(e.Code) ? "General" : e.Code)
			.ToDictionary(
				g => g.Key,
				g => g.Select(e => e.Description).ToArray());

		var problemDetails = new ValidationProblemDetails(errorDict)
		{
			Title = "خطای اعتبارسنجی",
			Status = StatusCodes.Status400BadRequest,
			Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
		};

		return new BadRequestObjectResult(problemDetails);
	}

	private static IActionResult CreateProblemDetails(Error error)
	{
		var statusCode = error.Type switch
		{
			ErrorType.Validation => StatusCodes.Status400BadRequest,
			ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
			ErrorType.Forbidden => StatusCodes.Status403Forbidden,
			ErrorType.NotFound => StatusCodes.Status404NotFound,
			ErrorType.Conflict => StatusCodes.Status409Conflict,
			ErrorType.Failure => StatusCodes.Status500InternalServerError,
			ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
			_ => StatusCodes.Status400BadRequest
		};

		var problemDetails = new ProblemDetails
		{
			Title = GetTitleForErrorType(error.Type),
			Detail = error.Description,
			Status = statusCode,
			Type = GetTypeUriForErrorType(error.Type),
			Extensions = { ["errorCode"] = error.Code }
		};

		return new ObjectResult(problemDetails) { StatusCode = statusCode };
	}

	private static string GetTitleForErrorType(ErrorType type) => type switch
	{
		ErrorType.Validation => "خطای اعتبارسنجی",
		ErrorType.Unauthorized => "احراز هویت نشده",
		ErrorType.Forbidden => "دسترسی ممنوع",
		ErrorType.NotFound => "یافت نشد",
		ErrorType.Conflict => "تعارض",
		ErrorType.Failure => "خطا در پردازش",
		ErrorType.Unexpected => "خطای غیرمنتظره",
		_ => "خطا"
	};

	private static string GetTypeUriForErrorType(ErrorType type) => type switch
	{
		ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
		ErrorType.Unauthorized => "https://tools.ietf.org/html/rfc7235#section-3.1",
		ErrorType.Forbidden => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
		ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
		ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
		ErrorType.Failure => "https://tools.ietf.org/html/rfc7231#section-6.6.1",
		ErrorType.Unexpected => "https://tools.ietf.org/html/rfc7231#section-6.6.1",
		_ => "https://tools.ietf.org/html/rfc7231#section-6.5.1"
	};
}