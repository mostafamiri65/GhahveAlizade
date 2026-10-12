using GhahveAlizade.Application.Features.Seo.Commands.SaveRedirect;
using GhahveAlizade.Application.Features.Seo.Queries.GetRedirectById;
using GhahveAlizade.Application.Features.Seo.Queries.GetRedirectsList;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using GhahveAlizade.Web.Areas.Admin.Models.Seo;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class RedirectsController : AdminBaseController
{
	[HasPermission(Permissions.Seo.ManageRedirects)]
	public async Task<IActionResult> Index(
		int pageNumber = 1, int pageSize = 20, string? search = null,
		bool? isActive = null, string? sortBy = "createdAt", string? sortDir = "desc",
		CancellationToken ct = default)
	{
		ViewData["Title"] = "ریدایرکت‌ها";
		ViewData["PageIcon"] = "bi-arrow-left-right";

		var result = await Mediator.Send(new GetRedirectsListQuery(
			pageNumber, pageSize, search, isActive, sortBy, sortDir), ct);

		if (result.IsError) return Problem(result.Errors);

		ViewBag.Search = search;
		ViewBag.IsActive = isActive;
		return View(result.Value);
	}

	[HasPermission(Permissions.Seo.ManageRedirects)]
	[HttpGet]
	public IActionResult Create()
	{
		ViewData["Title"] = "افزودن ریدایرکت";
		ViewData["PageIcon"] = "bi-plus-circle";
		return View(new RedirectFormViewModel { IsActive = true, Type = Domain.Enums.RedirectType.Permanent301 });
	}

	[HasPermission(Permissions.Seo.ManageRedirects)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(RedirectFormViewModel model, CancellationToken ct)
	{
		if (!ModelState.IsValid) return View(model);

		var result = await Mediator.Send(new CreateRedirectCommand(
			model.FromPath, model.ToPath, model.Type, model.IsActive, model.Notes), ct);

		if (result.IsError) { AddErrorsToModelState(result.Errors); return View(model); }

		TempData["SuccessMessage"] = "ریدایرکت ایجاد شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Seo.ManageRedirects)]
	[HttpGet]
	public async Task<IActionResult> Edit(long id, CancellationToken ct)
	{
		ViewData["Title"] = "ویرایش ریدایرکت";
		ViewData["PageIcon"] = "bi-pencil";

		var result = await Mediator.Send(new GetRedirectByIdQuery(id), ct);
		if (result.IsError)
		{
			TempData["ErrorMessage"] = result.FirstError.Description;
			return RedirectToAction(nameof(Index));
		}

		var r = result.Value;
		return View(new RedirectFormViewModel
		{
			Id = r.Id,
			FromPath = r.FromPath,
			ToPath = r.ToPath,
			Type = r.Type,
			IsActive = r.IsActive,
			Notes = r.Notes
		});
	}

	[HasPermission(Permissions.Seo.ManageRedirects)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(long id, RedirectFormViewModel model, CancellationToken ct)
	{
		if (!ModelState.IsValid) return View(model);

		var result = await Mediator.Send(new UpdateRedirectCommand(
			id, model.FromPath, model.ToPath, model.Type, model.IsActive, model.Notes), ct);

		if (result.IsError) { AddErrorsToModelState(result.Errors); model.Id = id; return View(model); }

		TempData["SuccessMessage"] = "ریدایرکت ویرایش شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Seo.ManageRedirects)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(long id, CancellationToken ct)
	{
		var result = await Mediator.Send(new DeleteRedirectCommand(id), ct);
		TempData[result.IsError ? "ErrorMessage" : "SuccessMessage"] =
			result.IsError ? result.FirstError.Description : "ریدایرکت حذف شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Seo.ManageRedirects)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ToggleActive(long id, CancellationToken ct)
	{
		var result = await Mediator.Send(new ToggleRedirectActiveCommand(id), ct);
		if (result.IsError)
			return Json(new { success = false, message = result.FirstError.Description });
		return Json(new { success = true, isActive = result.Value });
	}
}