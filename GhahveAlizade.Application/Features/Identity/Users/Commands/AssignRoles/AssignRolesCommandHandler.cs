using ErrorOr;
using GhahveAlizade.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GhahveAlizade.Application.Features.Identity.Users.Commands.AssignRoles;

public class AssignRolesCommandHandler
	: IRequestHandler<AssignRolesCommand, ErrorOr<Success>>
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly RoleManager<ApplicationRole> _roleManager;

	public AssignRolesCommandHandler(
		UserManager<ApplicationUser> userManager,
		RoleManager<ApplicationRole> roleManager)
	{
		_userManager = userManager;
		_roleManager = roleManager;
	}

	public async Task<ErrorOr<Success>> Handle(AssignRolesCommand request, CancellationToken ct)
	{
		var user = await _userManager.FindByIdAsync(request.UserId.ToString());
		if (user is null || user.IsDeleted)
			return Error.NotFound("User.NotFound", "کاربر یافت نشد.");

		// چک وجود همه نقش‌ها
		foreach (var role in request.Roles)
		{
			if (!await _roleManager.RoleExistsAsync(role))
				return Error.Validation("Role.NotFound", $"نقش '{role}' یافت نشد.");
		}

		var current = await _userManager.GetRolesAsync(user);
		var toAdd = request.Roles.Except(current).ToList();
		var toRemove = current.Except(request.Roles).ToList();

		if (toAdd.Any())
		{
			var r = await _userManager.AddToRolesAsync(user, toAdd);
			if (!r.Succeeded) return Error.Failure("User.AddRoleFailed", "خطا در افزودن نقش.");
		}

		if (toRemove.Any())
		{
			var r = await _userManager.RemoveFromRolesAsync(user, toRemove);
			if (!r.Succeeded) return Error.Failure("User.RemoveRoleFailed", "خطا در حذف نقش.");
		}

		return Result.Success;
	}
}