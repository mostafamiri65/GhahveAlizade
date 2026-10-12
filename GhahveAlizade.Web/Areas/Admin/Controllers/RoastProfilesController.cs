using GhahveAlizade.Application.Features.Coffee.Commands.SaveRoastProfile;
using GhahveAlizade.Application.Features.Coffee.Queries.GetRoastProfiles;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using GhahveAlizade.Web.Areas.Admin.Models.Coffee;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class RoastProfilesController : AdminBaseController
{
	[HasPermission(Permissions.Coffee.ViewRoastProfiles)]
	public async Task<IActionResult> Index(
		int pageNumber = 1, int pageSize = 20, string? search = null,
		bool? isActive = null, string? sortBy = "displayOrder", string? sortDir = "asc",
		CancellationToken ct = default)
	{
		ViewData["Title"] = "پروفایل‌های رست";
		ViewData["PageIcon"] = "bi-fire";

		var result = await Mediator.Send(new GetRoastProfilesListQuery(
			pageNumber, pageSize, search, isActive, sortBy, sortDir), ct);

		if (result.IsError) return Problem(result.Errors);

		ViewBag.Search = search;
		ViewBag.IsActive = isActive;
		return View(result.Value);
	}

	[HasPermission(Permissions.Coffee.ManageRoastProfiles)]
	[HttpGet]
	public IActionResult Create()
	{
		ViewData["Title"] = "افزودن پروفایل رست";
		ViewData["PageIcon"] = "bi-plus-circle";
		return View(new RoastProfileFormViewModel { IsActive = true, RoastLevel = 5 });
	}

	[HasPermission(Permissions.Coffee.ManageRoastProfiles)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(RoastProfileFormViewModel model, CancellationToken ct)
	{
		if (!ModelState.IsValid) return View(model);

		var result = await Mediator.Send(new CreateRoastProfileCommand(
			model.Name, model.Slug, model.Description, model.ColorCode,
			model.RoastLevel, model.IsActive, model.DisplayOrder), ct);

		if (result.IsError) { AddErrorsToModelState(result.Errors); return View(model); }

		TempData["SuccessMessage"] = "پروفایل رست ایجاد شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Coffee.ManageRoastProfiles)]
	[HttpGet]
	public async Task<IActionResult> Edit(long id, CancellationToken ct)
	{
		ViewData["Title"] = "ویرایش پروفایل رست";
		ViewData["PageIcon"] = "bi-pencil";

		var result = await Mediator.Send(new GetRoastProfileByIdQuery(id), ct);
		if (result.IsError)
		{
			TempData["ErrorMessage"] = result.FirstError.Description;
			return RedirectToAction(nameof(Index));
		}

		var r = result.Value;
		return View(new RoastProfileFormViewModel
		{
			Id = r.Id,
			Name = r.Name,
			Slug = r.Slug,
			Description = r.Description,
			ColorCode = r.ColorCode,
			RoastLevel = r.RoastLevel,
			IsActive = r.IsActive,
			DisplayOrder = r.DisplayOrder
		});
	}

	[HasPermission(Permissions.Coffee.ManageRoastProfiles)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(long id, RoastProfileFormViewModel model, CancellationToken ct)
	{
		if (!ModelState.IsValid) return View(model);

		var result = await Mediator.Send(new UpdateRoastProfileCommand(
			id, model.Name, model.Slug, model.Description, model.ColorCode,
			model.RoastLevel, model.IsActive, model.DisplayOrder), ct);

		if (result.IsError) { AddErrorsToModelState(result.Errors); model.Id = id; return View(model); }

		TempData["SuccessMessage"] = "پروفایل رست ویرایش شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Coffee.ManageRoastProfiles)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(long id, CancellationToken ct)
	{
		var result = await Mediator.Send(new DeleteRoastProfileCommand(id), ct);
		TempData[result.IsError ? "ErrorMessage" : "SuccessMessage"] =
			result.IsError ? result.FirstError.Description : "پروفایل رست حذف شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Coffee.ManageRoastProfiles)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ToggleActive(long id, CancellationToken ct)
	{
		var result = await Mediator.Send(new ToggleRoastProfileActiveCommand(id), ct);
		if (result.IsError) return Json(new { success = false, message = result.FirstError.Description });
		return Json(new { success = true, isActive = result.Value });
	}
}