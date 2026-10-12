using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Identity.Roles.Commands.UpdateRolePermissions;

public record UpdateRolePermissionsCommand(long RoleId, List<long> PermissionIds) : IRequest<ErrorOr<Success>>;