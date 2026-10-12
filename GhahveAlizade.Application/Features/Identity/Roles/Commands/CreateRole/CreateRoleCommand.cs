using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Identity.Roles.Commands.CreateRole;

public record CreateRoleCommand(string Name, string? Description) : IRequest<ErrorOr<long>>;