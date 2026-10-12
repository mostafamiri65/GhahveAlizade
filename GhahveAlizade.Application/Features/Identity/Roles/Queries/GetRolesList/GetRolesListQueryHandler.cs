using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Identity.Roles.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Identity.Roles.Queries.GetRolesList;

public class GetRolesListQueryHandler
	: IRequestHandler<GetRolesListQuery, ErrorOr<List<RoleDto>>>
{
	private readonly IAppDbContext _context;

	public GetRolesListQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<List<RoleDto>>> Handle(
		GetRolesListQuery request, CancellationToken ct)
	{
		var roles = await _context.Roles
			.AsNoTracking()
			.Where(r => !r.IsDeleted)
			.Select(r => new RoleDto
			{
				Id = r.Id,
				Name = r.Name ?? "",
				Description = r.Description,
				IsSystemRole = r.IsSystemRole,
				CreatedAtUtc = r.CreatedAtUtc,
				UserCount = 0, // بعداً محاسبه
				PermissionCount = _context.RolePermissions.Count(rp =>
					rp.RoleId == r.Id && !rp.IsDeleted)
			})
			.OrderBy(r => r.Name)
			.ToListAsync(ct);

		return roles;
	}
}