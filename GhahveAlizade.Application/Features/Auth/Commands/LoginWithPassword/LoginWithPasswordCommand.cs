using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Auth.Commands.LoginWithPassword;

public record LoginWithPasswordCommand(string UsernameOrEmail, string Password)
	: IRequest<ErrorOr<LoginWithPasswordResponse>>;

public record LoginWithPasswordResponse(
	long UserId,
	string UserName,
	string? FullName,
	string? Email);