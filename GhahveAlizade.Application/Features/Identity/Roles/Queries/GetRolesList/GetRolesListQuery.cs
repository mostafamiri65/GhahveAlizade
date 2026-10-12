using ErrorOr;
using GhahveAlizade.Application.Features.Identity.Roles.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Identity.Roles.Queries.GetRolesList;

public record GetRolesListQuery : IRequest<ErrorOr<List<RoleDto>>>;