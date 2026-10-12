using System.Net;

namespace GhahveAlizade.Application.Common.Exceptions;

public class ForbiddenException : AppException
{
	public ForbiddenException(string message = "شما دسترسی لازم برای این عملیات را ندارید.", string errorCode = "Access.Forbidden")
		: base(message, errorCode, (int)HttpStatusCode.Forbidden)
	{
	}
}