using ErrorOr;
using GhahveAlizade.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GhahveAlizade.Application.Features.Identity.Roles.Commands.DeleteRole;

public class DeleteRoleCommandHandler
	: IRequestHandler<DeleteRoleCommand, ErrorOr<Success>>
{
	private readonly RoleManager<ApplicationRole> _roleManager;
	private readonly UserManager<ApplicationUser> _userManager;

	public DeleteRoleCommandHandler(
		RoleManager<ApplicationRole> roleManager,
		UserManager<ApplicationUser> userManager)
	{
		_roleManager = roleManager;
		_userManager = userManager;
	}

	public async Task<ErrorOr<Success>> Handle(DeleteRoleCommand request, CancellationToken ct)
	{
		var role = await _roleManager.FindByIdAsync(request.Id.ToString());
		if (role is null || role.IsDeleted)
			return Error.NotFound("Role.NotFound", "نقش یافت نشد.");

		if (role.IsSystemRole)
			return Error.Forbidden("Role.SystemRoleDelete", "نقش‌های سیستمی قابل حذف نیستند.");

		// چک: آیا کاربری این نقش رو داره؟
		var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);

		if (usersInRole.Any())
			return Error.Conflict("Role.HasUsers",
				$"این نقش به {usersInRole.Count} کاربر اختصاص داده شده است و قابل حذف نیست.");

		// Soft Delete
		role.IsDeleted = true;
		var result = await _roleManager.UpdateAsync(role);
		if (!result.Succeeded)
			return Error.Failure("Role.DeleteFailed", "خطا در حذف نقش.");

		return Result.Success;
	}
}