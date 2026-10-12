using System.Net;

namespace GhahveAlizade.Application.Common.Exceptions;

public class ValidationException : AppException
{
	public Dictionary<string, string[]> Errors { get; }

	public ValidationException(Dictionary<string, string[]> errors)
		: base("یک یا چند خطای اعتبارسنجی رخ داده است.", "Validation.Failed", (int)HttpStatusCode.BadRequest)
	{
		Errors = errors;
	}

	public ValidationException(string field, string message)
		: this(new Dictionary<string, string[]> { [field] = new[] { message } })
	{
	}
}