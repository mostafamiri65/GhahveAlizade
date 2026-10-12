using ErrorOr;
using GhahveAlizade.Application.Features.Identity.Roles.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Identity.Roles.Queries.GetRolePermissions;

public record GetRolePermissionsQuery(long RoleId) : IRequest<ErrorOr<List<RolePermissionGroupDto>>>;