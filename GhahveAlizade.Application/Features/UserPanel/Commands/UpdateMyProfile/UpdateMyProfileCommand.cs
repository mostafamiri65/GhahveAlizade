using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.UserPanel.Commands.UpdateMyProfile;

public record UpdateMyProfileCommand(
	long UserId,
	string? FirstName,
	string? LastName,
	string? Email,
	string? NationalCode,
	DateOnly? BirthDate,
	string? Gender,
	bool AcceptsMarketing) : IRequest<ErrorOr<Updated>>;
