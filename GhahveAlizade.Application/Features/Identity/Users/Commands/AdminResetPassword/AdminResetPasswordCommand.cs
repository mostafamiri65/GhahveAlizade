using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Identity.Users.Commands.AdminResetPassword;

public record AdminResetPasswordCommand(long UserId, string NewPassword) : IRequest<ErrorOr<Success>>;