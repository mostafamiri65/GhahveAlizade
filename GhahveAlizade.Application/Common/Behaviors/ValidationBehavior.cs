using ErrorOr;
using FluentValidation;
using MediatR;

namespace GhahveAlizade.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
	where TResponse : IErrorOr
{
	private readonly IEnumerable<IValidator<TRequest>> _validators;

	public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
		=> _validators = validators;

	public async Task<TResponse> Handle(
		TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		if (!_validators.Any())
			return await next();

		var context = new ValidationContext<TRequest>(request);

		// ⬅️ اینجا ValidateAsync استفاده می‌کنیم
		var validationResults = await Task.WhenAll(
			_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

		var failures = validationResults
			.SelectMany(r => r.Errors)
			.Where(f => f is not null)
			.ToList();

		if (failures.Count != 0)
		{
			var errors = failures
				.Select(f => Error.Validation(
					code: f.PropertyName,
					description: f.ErrorMessage))
				.ToList();

			// چون TResponse : IErrorOr، با reflection یا dynamic تبدیل می‌کنیم
			return (dynamic)errors;
		}

		return await next();
	}
}