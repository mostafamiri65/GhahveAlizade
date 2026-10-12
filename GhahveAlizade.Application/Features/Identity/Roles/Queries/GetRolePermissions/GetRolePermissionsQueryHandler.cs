using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Identity.Roles.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Identity.Roles.Queries.GetRolePermissions;

public class GetRolePermissionsQueryHandler
	: IRequestHandler<GetRolePermissionsQuery, ErrorOr<List<RolePermissionGroupDto>>>
{
	private readonly IAppDbContext _context;

	public GetRolePermissionsQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<List<RolePermissionGroupDto>>> Handle(
		GetRolePermissionsQuery request, CancellationToken ct)
	{
		//var roleIdLong = long.TryParse(request.RoleId, out var rid) ? rid : 0;

		var assignedIds = await _context.RolePermissions
			.Where(rp => rp.RoleId == request.RoleId && !rp.IsDeleted)
			.Select(rp => rp.PermissionId)
			.ToListAsync(ct);

		var permissions = await _context.Permissions
			.AsNoTracking()
			.Where(p => !p.IsDeleted)
			.OrderBy(p => p.DisplayOrder)
			.Select(p => new
			{
				p.Id,
				p.Name,
				p.DisplayName,
				p.Group,
				p.DisplayOrder
			})
			.ToListAsync(ct);

		return permissions
			.GroupBy(p => p.Group ?? "Other")
			.Select(g => new RolePermissionGroupDto
			{
				Group = g.Key,
				Permissions = g.Select(p => new PermissionItemDto
				{
					Id = p.Id,
					Name = p.Name,
					DisplayName = p.DisplayName,
					IsAssigned = assignedIds.Contains(p.Id)
				}).ToList()
			})
			.OrderBy(g => g.Group)
			.ToList();
	}
}