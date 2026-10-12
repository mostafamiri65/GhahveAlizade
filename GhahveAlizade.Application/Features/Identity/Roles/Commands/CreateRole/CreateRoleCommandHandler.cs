using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GhahveAlizade.Application.Features.Identity.Roles.Commands.CreateRole;

public class CreateRoleCommandHandler
	: IRequestHandler<CreateRoleCommand, ErrorOr<long>>
{
	private readonly RoleManager<ApplicationRole> _roleManager;

	public CreateRoleCommandHandler(RoleManager<ApplicationRole> roleManager)
		=> _roleManager = roleManager;

	public async Task<ErrorOr<long>> Handle(CreateRoleCommand request, CancellationToken ct)
	{
		if (await _roleManager.RoleExistsAsync(request.Name))
			return Error.Conflict("Role.Exists", "این نقش قبلاً وجود دارد.");

		var role = new ApplicationRole
		{
			Name = request.Name.Trim(),
			Description = request.Description?.Trim(),
			IsSystemRole = false,
			CreatedAtUtc = DateTime.UtcNow
		};

		var result = await _roleManager.CreateAsync(role);
		if (!result.Succeeded)
		{
			var errors = result.Errors.Select(e => Error.Failure(e.Code, e.Description)).ToList();
			return errors;
		}

		return role.Id;
	}
}