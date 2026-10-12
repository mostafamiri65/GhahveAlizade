using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Identity.Users.Commands.UpdateUser;

public record UpdateUserCommand(
	long Id,
	string? FirstName,
	string? LastName,
	string? Email,
	string? PhoneNumber,
	string? NationalCode,
	DateOnly? BirthDate,
	string? Gender,
	bool IsActive) : IRequest<ErrorOr<Success>>;