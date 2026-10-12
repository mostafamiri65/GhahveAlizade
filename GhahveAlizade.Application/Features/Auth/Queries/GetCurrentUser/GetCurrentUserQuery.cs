using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Auth.Queries.GetCurrentUser;

public record GetCurrentUserQuery(long UserId) : IRequest<ErrorOr<CurrentUserDto>>;

public record CurrentUserDto(
	long Id,
	string UserName,
	string? Email,
	string? PhoneNumber,
	string? FullName,
	string? AvatarUrl,
	bool HasPassword,
	bool IsPhoneVerified,
	IReadOnlyList<string> Roles,
	IReadOnlyList<string> Permissions);