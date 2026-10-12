using System.Net;

namespace GhahveAlizade.Application.Common.Exceptions;

/// <summary>
/// برای نقض قوانین Business Logic (مثلاً موجودی کافی نیست).
/// </summary>
public class BusinessRuleException : AppException
{
	public BusinessRuleException(string message, string errorCode = "BusinessRule.Violated")
		: base(message, errorCode, (int)HttpStatusCode.UnprocessableEntity)
	{
	}
}