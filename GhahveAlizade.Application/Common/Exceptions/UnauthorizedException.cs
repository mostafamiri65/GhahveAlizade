using System.Net;

namespace GhahveAlizade.Application.Common.Exceptions;

public class UnauthorizedException : AppException
{
	public UnauthorizedException(string message = "ابتدا وارد حساب کاربری خود شوید.", string errorCode = "Auth.Unauthorized")
		: base(message, errorCode, (int)HttpStatusCode.Unauthorized)
	{
	}
}