using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace GhahveAlizade.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

	public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
		=> _logger = logger;

	public async Task<TResponse> Handle(
		TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		var requestName = typeof(TRequest).Name;
		_logger.LogInformation("Handling {RequestName}", requestName);

		var sw = Stopwatch.StartNew();
		try
		{
			var response = await next();
			sw.Stop();

			if (sw.ElapsedMilliseconds > 500)
				_logger.LogWarning("Long running request {RequestName} took {ElapsedMs}ms", requestName, sw.ElapsedMilliseconds);
			else
				_logger.LogInformation("Handled {RequestName} in {ElapsedMs}ms", requestName, sw.ElapsedMilliseconds);

			return response;
		}
		catch (Exception ex)
		{
			sw.Stop();
			_logger.LogError(ex, "Error handling {RequestName} after {ElapsedMs}ms", requestName, sw.ElapsedMilliseconds);
			throw;
		}
	}
}