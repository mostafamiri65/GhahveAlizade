using GhahveAlizade.Application.Features.Coffee.Commands.SaveCoffeeProcess;
using GhahveAlizade.Application.Features.Coffee.Queries.GetCoffeeProcesses;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using GhahveAlizade.Web.Areas.Admin.Models.Coffee;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class CoffeeProcessesController : AdminBaseController
{
	[HasPermission(Permissions.Coffee.ViewProcesses)]
	public async Task<IActionResult> Index(
		int pageNumber = 1, int pageSize = 20, string? search = null,
		bool? isActive = null, string? sortBy = "displayOrder", string? sortDir = "asc",
		CancellationToken ct = default)
	{
		ViewData["Title"] = "فرآیندهای قهوه";
		ViewData["PageIcon"] = "bi-droplet";

		var result = await Mediator.Send(new GetCoffeeProcessesListQuery(
			pageNumber, pageSize, search, isActive, sortBy, sortDir), ct);

		if (result.IsError) return Problem(result.Errors);

		ViewBag.Search = search;
		ViewBag.IsActive = isActive;
		return View(result.Value);
	}

	[HasPermission(Permissions.Coffee.ManageProcesses)]
	[HttpGet]
	public IActionResult Create()
	{
		ViewData["Title"] = "افزودن فرآیند";
		ViewData["PageIcon"] = "bi-plus-circle";
		return View(new CoffeeProcessFormViewModel { IsActive = true });
	}

	[HasPermission(Permissions.Coffee.ManageProcesses)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(CoffeeProcessFormViewModel model, CancellationToken ct)
	{
		if (!ModelState.IsValid) return View(model);

		var result = await Mediator.Send(new CreateCoffeeProcessCommand(
			model.Name, model.Slug, model.Description,
			model.ImageUrl, model.IconUrl, model.IsActive, model.DisplayOrder), ct);

		if (result.IsError) { AddErrorsToModelState(result.Errors); return View(model); }

		TempData["SuccessMessage"] = "فرآیند ایجاد شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Coffee.ManageProcesses)]
	[HttpGet]
	public async Task<IActionResult> Edit(long id, CancellationToken ct)
	{
		ViewData["Title"] = "ویرایش فرآیند";
		ViewData["PageIcon"] = "bi-pencil";

		var result = await Mediator.Send(new GetCoffeeProcessByIdQuery(id), ct);
		if (result.IsError)
		{
			TempData["ErrorMessage"] = result.FirstError.Description;
			return RedirectToAction(nameof(Index));
		}

		var p = result.Value;
		return View(new CoffeeProcessFormViewModel
		{
			Id = p.Id,
			Name = p.Name,
			Slug = p.Slug,
			Description = p.Description,
			ImageUrl = p.ImageUrl,
			IconUrl = p.IconUrl,
			IsActive = p.IsActive,
			DisplayOrder = p.DisplayOrder
		});
	}

	[HasPermission(Permissions.Coffee.ManageProcesses)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(long id, CoffeeProcessFormViewModel model, CancellationToken ct)
	{
		if (!ModelState.IsValid) return View(model);

		var result = await Mediator.Send(new UpdateCoffeeProcessCommand(
			id, model.Name, model.Slug, model.Description,
			model.ImageUrl, model.IconUrl, model.IsActive, model.DisplayOrder), ct);

		if (result.IsError) { AddErrorsToModelState(result.Errors); model.Id = id; return View(model); }

		TempData["SuccessMessage"] = "فرآیند ویرایش شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Coffee.ManageProcesses)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(long id, CancellationToken ct)
	{
		var result = await Mediator.Send(new DeleteCoffeeProcessCommand(id), ct);
		TempData[result.IsError ? "ErrorMessage" : "SuccessMessage"] =
			result.IsError ? result.FirstError.Description : "فرآیند حذف شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Coffee.ManageProcesses)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ToggleActive(long id, CancellationToken ct)
	{
		var result = await Mediator.Send(new ToggleCoffeeProcessActiveCommand(id), ct);
		if (result.IsError) return Json(new { success = false, message = result.FirstError.Description });
		return Json(new { success = true, isActive = result.Value });
	}
}