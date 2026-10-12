using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Identity.Roles.Commands.DeleteRole;

public record DeleteRoleCommand(long Id) : IRequest<ErrorOr<Success>>;