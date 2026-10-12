using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Auth.Commands.SetPassword;

public record SetPasswordCommand(long UserId, string NewPassword) : IRequest<ErrorOr<Success>>;