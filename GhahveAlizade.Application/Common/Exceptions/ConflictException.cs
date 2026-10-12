using System.Net;

namespace GhahveAlizade.Application.Common.Exceptions;

public class ConflictException : AppException
{
	public ConflictException(string message, string errorCode = "Resource.Conflict")
		: base(message, errorCode, (int)HttpStatusCode.Conflict)
	{
	}
}