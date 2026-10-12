using GhahveAlizade.Application.Features.Identity.Roles.Commands.CreateRole;
using GhahveAlizade.Application.Features.Identity.Roles.Commands.DeleteRole;
using GhahveAlizade.Application.Features.Identity.Roles.Commands.UpdateRole;
using GhahveAlizade.Application.Features.Identity.Roles.Commands.UpdateRolePermissions;
using GhahveAlizade.Application.Features.Identity.Roles.Queries.GetRolePermissions;
using GhahveAlizade.Application.Features.Identity.Roles.Queries.GetRolesList;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class RolesController : AdminBaseController
{
	[HasPermission(Infrastructure.Persistence.Seed.Permissions.Roles.View)]
	public async Task<IActionResult> Index(CancellationToken ct)
	{
		ViewData["Title"] = "نقش‌ها و دسترسی‌ها";
		ViewData["PageIcon"] = "bi-shield-lock";

		var result = await Mediator.Send(new GetRolesListQuery(), ct);
		if (result.IsError) return Problem(result.Errors);
		return View(result.Value);
	}

	[HasPermission(Infrastructure.Persistence.Seed.Permissions.Roles.Create)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(string name, string? description, CancellationToken ct)
	{
		var result = await Mediator.Send(new CreateRoleCommand(name, description), ct);
		if (result.IsError)
			return Json(new { success = false, message = result.FirstError.Description });
		return Json(new { success = true, message = "نقش ایجاد شد.", id = result.Value });
	}

	[HasPermission(Infrastructure.Persistence.Seed.Permissions.Roles.Edit)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Update(long id, string name, string? description, CancellationToken ct)
	{
		var result = await Mediator.Send(new UpdateRoleCommand(id, name, description), ct);
		if (result.IsError)
			return Json(new { success = false, message = result.FirstError.Description });
		return Json(new { success = true, message = "نقش ویرایش شد." });
	}

	[HasPermission(Infrastructure.Persistence.Seed.Permissions.Roles.Delete)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(long id, CancellationToken ct)
	{
		var result = await Mediator.Send(new DeleteRoleCommand(id), ct);
		if (result.IsError)
			return Json(new { success = false, message = result.FirstError.Description });
		return Json(new { success = true, message = "نقش حذف شد." });
	}

	[HasPermission(Infrastructure.Persistence.Seed.Permissions.Roles.ManagePermissions)]
	[HttpGet]
	public async Task<IActionResult> Permissions(long id, CancellationToken ct)
	{
		ViewData["Title"] = "مدیریت دسترسی‌ها";
		ViewData["PageIcon"] = "bi-key";
		ViewData["RoleId"] = id;

		var result = await Mediator.Send(new GetRolePermissionsQuery(id), ct);
		if (result.IsError) return Problem(result.Errors);
		return View(result.Value);
	}

	[HasPermission(Infrastructure.Persistence.Seed.Permissions.Roles.ManagePermissions)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> UpdatePermissions(
		long roleId, List<long> permissionIds, CancellationToken ct)
	{
		var result = await Mediator.Send(new UpdateRolePermissionsCommand(
			roleId, permissionIds ?? new()), ct);

		if (result.IsError)
			return Json(new { success = false, message = result.FirstError.Description });
		return Json(new { success = true, message = "دسترسی‌ها ذخیره شد." });
	}
}