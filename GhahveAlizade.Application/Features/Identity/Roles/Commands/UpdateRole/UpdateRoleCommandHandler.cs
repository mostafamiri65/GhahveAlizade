using ErrorOr;
using GhahveAlizade.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GhahveAlizade.Application.Features.Identity.Roles.Commands.UpdateRole;

public class UpdateRoleCommandHandler
	: IRequestHandler<UpdateRoleCommand, ErrorOr<Success>>
{
	private readonly RoleManager<ApplicationRole> _roleManager;

	public UpdateRoleCommandHandler(RoleManager<ApplicationRole> roleManager)
		=> _roleManager = roleManager;

	public async Task<ErrorOr<Success>> Handle(UpdateRoleCommand request, CancellationToken ct)
	{
		var role = await _roleManager.FindByIdAsync(request.Id.ToString());
		if (role is null || role.IsDeleted)
			return Error.NotFound("Role.NotFound", "نقش یافت نشد.");

		if (role.IsSystemRole && role.Name != request.Name)
			return Error.Forbidden("Role.SystemRoleRename", "نام نقش‌های سیستمی قابل تغییر نیست.");

		role.Name = request.Name.Trim();
		role.Description = request.Description?.Trim();

		var result = await _roleManager.UpdateAsync(role);
		if (!result.Succeeded)
		{
			var errors = result.Errors.Select(e => Error.Failure(e.Code, e.Description)).ToList();
			return errors;
		}

		return Result.Success;
	}
}