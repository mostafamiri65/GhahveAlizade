namespace GhahveAlizade.Application.Common.Exceptions;

/// <summary>
/// کلاس پایه برای تمام Exceptionهای برنامه.
/// فقط برای خطاهایی که واقعاً Exception هستن (نه Validation).
/// </summary>
public abstract class AppException : Exception
{
	public string ErrorCode { get; }
	public int StatusCode { get; }

	protected AppException(string message, string errorCode, int statusCode)
		: base(message)
	{
		ErrorCode = errorCode;
		StatusCode = statusCode;
	}

	protected AppException(string message, string errorCode, int statusCode, Exception innerException)
		: base(message, innerException)
	{
		ErrorCode = errorCode;
		StatusCode = statusCode;
	}
}