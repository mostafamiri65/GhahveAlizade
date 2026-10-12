using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Identity.Roles.Commands.UpdateRole;

public record UpdateRoleCommand(long Id, string Name, string? Description) : IRequest<ErrorOr<Success>>;