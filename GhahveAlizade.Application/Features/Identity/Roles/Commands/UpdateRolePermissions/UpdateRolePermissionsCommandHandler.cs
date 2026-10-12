using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Identity.Roles.Commands.UpdateRolePermissions;

public class UpdateRolePermissionsCommandHandler
	: IRequestHandler<UpdateRolePermissionsCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;

	public UpdateRolePermissionsCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(
		UpdateRolePermissionsCommand request, CancellationToken ct)
	{
		var roleIdString = request.RoleId;
		var current = await _context.RolePermissions
			.Where(rp => rp.RoleId == roleIdString && !rp.IsDeleted)
			.ToListAsync(ct);

		var currentIds = current.Select(rp => rp.PermissionId).ToHashSet();
		var targetIds = request.PermissionIds.ToHashSet();

		// اضافه‌ها
		var toAdd = targetIds.Except(currentIds).Select(pid => new RolePermission
		{
			RoleId = roleIdString,
			PermissionId = pid,
			CreatedAtUtc = DateTime.UtcNow
		}).ToList();

		if (toAdd.Any())
			await _context.RolePermissions.AddRangeAsync(toAdd, ct);

		// حذف‌ها (Soft Delete)
		foreach (var rp in current.Where(x => !targetIds.Contains(x.PermissionId)))
		{
			rp.IsDeleted = true;
			rp.DeletedAtUtc = DateTime.UtcNow;
		}

		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}