using System.Net;

namespace GhahveAlizade.Application.Common.Exceptions;

public class NotFoundException : AppException
{
	public NotFoundException(string message, string errorCode = "Resource.NotFound")
		: base(message, errorCode, (int)HttpStatusCode.NotFound)
	{
	}

	public NotFoundException(string resourceName, object key)
		: base($"{resourceName} با شناسه‌ی '{key}' یافت نشد.", $"{resourceName}.NotFound", (int)HttpStatusCode.NotFound)
	{
	}
}