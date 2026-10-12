using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Identity.Users.Commands.AssignRoles;

public record AssignRolesCommand(long UserId, List<string> Roles) : IRequest<ErrorOr<Success>>;