using GhahveAlizade.Application.Features.Coffee.Commands.SaveFlavorNote;
using GhahveAlizade.Application.Features.Coffee.Queries.GetFlavorNotes;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using GhahveAlizade.Web.Areas.Admin.Models.Coffee;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class FlavorNotesController : AdminBaseController
{
	[HasPermission(Permissions.Coffee.ViewFlavorNotes)]
	public async Task<IActionResult> Index(
		int pageNumber = 1, int pageSize = 20, string? search = null,
		string? category = null, bool? isActive = null,
		string? sortBy = "displayOrder", string? sortDir = "asc",
		CancellationToken ct = default)
	{
		ViewData["Title"] = "نُت‌های طعمی";
		ViewData["PageIcon"] = "bi-flower1";

		var result = await Mediator.Send(new GetFlavorNotesListQuery(
			pageNumber, pageSize, search, category, isActive, sortBy, sortDir), ct);

		if (result.IsError) return Problem(result.Errors);

		ViewBag.Search = search;
		ViewBag.Category = category;
		ViewBag.IsActive = isActive;
		return View(result.Value);
	}

	[HasPermission(Permissions.Coffee.ManageFlavorNotes)]
	[HttpGet]
	public IActionResult Create()
	{
		ViewData["Title"] = "افزودن نُت طعمی";
		ViewData["PageIcon"] = "bi-plus-circle";
		return View(new FlavorNoteFormViewModel { IsActive = true });
	}

	[HasPermission(Permissions.Coffee.ManageFlavorNotes)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(FlavorNoteFormViewModel model, CancellationToken ct)
	{
		if (!ModelState.IsValid) return View(model);

		var result = await Mediator.Send(new CreateFlavorNoteCommand(
			model.Name, model.Slug, model.Category, model.IconUrl,
			model.ColorCode, model.IsActive, model.DisplayOrder), ct);

		if (result.IsError) { AddErrorsToModelState(result.Errors); return View(model); }

		TempData["SuccessMessage"] = "نُت طعمی ایجاد شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Coffee.ManageFlavorNotes)]
	[HttpGet]
	public async Task<IActionResult> Edit(long id, CancellationToken ct)
	{
		ViewData["Title"] = "ویرایش نُت طعمی";
		ViewData["PageIcon"] = "bi-pencil";

		var result = await Mediator.Send(new GetFlavorNoteByIdQuery(id), ct);
		if (result.IsError)
		{
			TempData["ErrorMessage"] = result.FirstError.Description;
			return RedirectToAction(nameof(Index));
		}

		var f = result.Value;
		return View(new FlavorNoteFormViewModel
		{
			Id = f.Id,
			Name = f.Name,
			Slug = f.Slug,
			Category = f.Category,
			IconUrl = f.IconUrl,
			ColorCode = f.ColorCode,
			IsActive = f.IsActive,
			DisplayOrder = f.DisplayOrder
		});
	}

	[HasPermission(Permissions.Coffee.ManageFlavorNotes)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(long id, FlavorNoteFormViewModel model, CancellationToken ct)
	{
		if (!ModelState.IsValid) return View(model);

		var result = await Mediator.Send(new UpdateFlavorNoteCommand(
			id, model.Name, model.Slug, model.Category, model.IconUrl,
			model.ColorCode, model.IsActive, model.DisplayOrder), ct);

		if (result.IsError) { AddErrorsToModelState(result.Errors); model.Id = id; return View(model); }

		TempData["SuccessMessage"] = "نُت طعمی ویرایش شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Coffee.ManageFlavorNotes)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(long id, CancellationToken ct)
	{
		var result = await Mediator.Send(new DeleteFlavorNoteCommand(id), ct);
		TempData[result.IsError ? "ErrorMessage" : "SuccessMessage"] =
			result.IsError ? result.FirstError.Description : "نُت طعمی حذف شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Coffee.ManageFlavorNotes)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ToggleActive(long id, CancellationToken ct)
	{
		var result = await Mediator.Send(new ToggleFlavorNoteActiveCommand(id), ct);
		if (result.IsError) return Json(new { success = false, message = result.FirstError.Description });
		return Json(new { success = true, isActive = result.Value });
	}
}