using GhahveAlizade.Application.Features.Identity.Users.Commands.AdminResetPassword;
using GhahveAlizade.Application.Features.Identity.Users.Commands.AssignRoles;
using GhahveAlizade.Application.Features.Identity.Users.Commands.ToggleUserActive;
using GhahveAlizade.Application.Features.Identity.Users.Commands.UpdateUser;
using GhahveAlizade.Application.Features.Identity.Users.Queries.GetUserById;
using GhahveAlizade.Application.Features.Identity.Users.Queries.GetUsersList;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using GhahveAlizade.Web.Areas.Admin.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class UsersController : AdminBaseController
{
	private readonly RoleManager<Domain.Identity.ApplicationRole> _roleManager;

	public UsersController(RoleManager<Domain.Identity.ApplicationRole> roleManager)
		=> _roleManager = roleManager;

	[HasPermission(Permissions.Users.View)]
	public async Task<IActionResult> Index(
		int pageNumber = 1, int pageSize = 20, string? search = null,
		string? role = null, bool? isActive = null,
		string? sortBy = "createdAt", string? sortDir = "desc",
		CancellationToken ct = default)
	{
		ViewData["Title"] = "مدیریت کاربران";
		ViewData["PageIcon"] = "bi-people";

		var result = await Mediator.Send(new GetUsersListQuery(
			pageNumber, pageSize, search, role, isActive, sortBy, sortDir), ct);

		if (result.IsError) return Problem(result.Errors);

		ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name!).ToListAsync(ct);
		ViewBag.Search = search;
		ViewBag.Role = role;
		ViewBag.IsActive = isActive;

		return View(result.Value);
	}

	[HasPermission(Permissions.Users.View)]
	public async Task<IActionResult> Detail(long id, CancellationToken ct)
	{
		ViewData["Title"] = "جزئیات کاربر";
		ViewData["PageIcon"] = "bi-person";

		var result = await Mediator.Send(new GetUserByIdQuery(id), ct);
		if (result.IsError)
		{
			TempData["ErrorMessage"] = result.FirstError.Description;
			return RedirectToAction(nameof(Index));
		}

		ViewBag.AllRoles = await _roleManager.Roles.Select(r => r.Name!).ToListAsync(ct);
		return View(result.Value);
	}

	[HasPermission(Permissions.Users.Edit)]
	[HttpGet]
	public async Task<IActionResult> Edit(long id, CancellationToken ct)
	{
		ViewData["Title"] = "ویرایش کاربر";
		ViewData["PageIcon"] = "bi-pencil";

		var result = await Mediator.Send(new GetUserByIdQuery(id), ct);
		if (result.IsError)
		{
			TempData["ErrorMessage"] = result.FirstError.Description;
			return RedirectToAction(nameof(Index));
		}

		var u = result.Value;
		return View(new UserEditViewModel
		{
			Id = u.Id,
			UserName = u.UserName,
			Email = u.Email,
			PhoneNumber = u.PhoneNumber,
			FirstName = u.FirstName,
			LastName = u.LastName,
			NationalCode = u.NationalCode,
			BirthDate = u.BirthDate,
			Gender = u.Gender,
			IsActive = u.IsActive
		});
	}

	[HasPermission(Permissions.Users.Edit)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(long id, UserEditViewModel model, CancellationToken ct)
	{
		if (!ModelState.IsValid) return View(model);

		var result = await Mediator.Send(new UpdateUserCommand(
			id, model.FirstName, model.LastName, model.Email, model.PhoneNumber,
			model.NationalCode, model.BirthDate, model.Gender, model.IsActive), ct);

		if (result.IsError)
		{
			AddErrorsToModelState(result.Errors);
			return View(model);
		}

		TempData["SuccessMessage"] = "اطلاعات کاربر ذخیره شد.";
		return RedirectToAction(nameof(Detail), new { id });
	}

	[HasPermission(Permissions.Users.Edit)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ToggleActive(long id, CancellationToken ct)
	{
		var result = await Mediator.Send(new ToggleUserActiveCommand(id), ct);
		if (result.IsError)
			return Json(new { success = false, message = result.FirstError.Description });
		return Json(new { success = true, isActive = result.Value });
	}

	[HasPermission(Permissions.Users.ManageRoles)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> AssignRoles(long id, List<string> roles, CancellationToken ct)
	{
		var result = await Mediator.Send(new AssignRolesCommand(id, roles ?? new()), ct);
		if (result.IsError)
			return Json(new { success = false, message = result.FirstError.Description });
		return Json(new { success = true, message = "نقش‌ها ذخیره شد." });
	}

	[HasPermission(Permissions.Users.ResetPassword)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ResetPassword(
		long id, string newPassword, CancellationToken ct)
	{
		if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 8)
			return Json(new { success = false, message = "رمز عبور باید حداقل 8 کاراکتر باشد." });

		var result = await Mediator.Send(new AdminResetPasswordCommand(id, newPassword), ct);
		if (result.IsError)
			return Json(new { success = false, message = result.FirstError.Description });
		return Json(new { success = true, message = "رمز عبور با موفقیت تغییر یافت." });
	}
}