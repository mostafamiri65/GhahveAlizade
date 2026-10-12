using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Customers.Commands.UpdateCustomer;

public record UpdateCustomerCommand(
	long Id,
	string? FirstName,
	string? LastName,
	string? Email,
	string? PhoneNumber,
	string? NationalCode,
	DateOnly? BirthDate,
	string? Gender,
	bool IsActive,
	bool IsVerified,
	bool AcceptsMarketing,
	long? CustomerGroupId) : IRequest<ErrorOr<Success>>;