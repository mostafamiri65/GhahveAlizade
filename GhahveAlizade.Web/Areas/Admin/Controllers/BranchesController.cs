using GhahveAlizade.Application.Features.Inventory.Commands.SaveBranch;
using GhahveAlizade.Application.Features.Inventory.Queries.GetBranchById;
using GhahveAlizade.Application.Features.Inventory.Queries.GetBranchesList;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using GhahveAlizade.Web.Areas.Admin.Models.Brands;
using GhahveAlizade.Web.Areas.Admin.Models.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class BranchesController : AdminBaseController
{
	// ================ لیست ================

	[HasPermission(Permissions.Inventory.ManageBranches)]
	public async Task<IActionResult> Index(
		int pageNumber = 1,
		int pageSize = 20,
		string? search = null,
		bool? isActive = null,
		string? sortBy = "displayOrder",
		string? sortDir = "asc",
		CancellationToken ct = default)
	{
		ViewData["Title"] = "شعبه‌ها";
		ViewData["PageIcon"] = "bi-shop";

		var r = await Mediator.Send(new GetBranchesListQuery(
			pageNumber, pageSize, search, isActive, sortBy, sortDir), ct);

		if (r.IsError) return Problem(r.Errors);

		ViewBag.Search = search;
		ViewBag.IsActive = isActive;
		ViewBag.SortBy = sortBy;

		return View(r.Value);
	}

	// ================ ایجاد ================

	[HasPermission(Permissions.Inventory.ManageBranches)]
	[HttpGet]
	public IActionResult Create()
	{
		ViewData["Title"] = "افزودن شعبه";
		ViewData["PageIcon"] = "bi-plus-circle";
		ViewData["Breadcrumb"] = "شعبه‌ها / افزودن";

		return View(new BranchFormViewModel { IsActive = true });
	}

	[HasPermission(Permissions.Inventory.ManageBranches)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(BranchFormViewModel model, CancellationToken ct)
	{
		if (!ModelState.IsValid) return View(model);

		var r = await Mediator.Send(new CreateBranchCommand(
			model.Name, model.Code, model.Address, model.City, model.Province,
			model.PostalCode, model.Phone, model.Latitude, model.Longitude,
			model.WorkingHours, model.IsActive, model.IsDefault, model.DisplayOrder), ct);

		if (r.IsError)
		{
			AddErrorsToModelState(r.Errors);
			return View(model);
		}

		TempData["SuccessMessage"] = "شعبه با موفقیت ایجاد شد.";
		return RedirectToAction(nameof(Index));
	}

	// ================ ویرایش ================

	[HasPermission(Permissions.Inventory.ManageBranches)]
	[HttpGet]
	public async Task<IActionResult> Edit(long id, CancellationToken ct)
	{
		ViewData["Title"] = "ویرایش شعبه";
		ViewData["PageIcon"] = "bi-pencil";
		ViewData["Breadcrumb"] = "شعبه‌ها / ویرایش";

		var r = await Mediator.Send(new GetBranchByIdQuery(id), ct);
		if (r.IsError)
		{
			TempData["ErrorMessage"] = r.FirstError.Description;
			return RedirectToAction(nameof(Index));
		}

		var b = r.Value;
		return View(new BranchFormViewModel
		{
			Id = b.Id,
			Name = b.Name,
			Code = b.Code,
			Address = b.Address,
			City = b.City,
			Province = b.Province,
			PostalCode = b.PostalCode,
			Phone = b.Phone,
			Latitude = b.Latitude,
			Longitude = b.Longitude,
			WorkingHours = b.WorkingHours,
			IsActive = b.IsActive,
			IsDefault = b.IsDefault,
			DisplayOrder = b.DisplayOrder
		});
	}

	[HasPermission(Permissions.Inventory.ManageBranches)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(long id, BranchFormViewModel model, CancellationToken ct)
	{
		if (!ModelState.IsValid) return View(model);

		var r = await Mediator.Send(new UpdateBranchCommand(
			id, model.Name, model.Code, model.Address, model.City, model.Province,
			model.PostalCode, model.Phone, model.Latitude, model.Longitude,
			model.WorkingHours, model.IsActive, model.IsDefault, model.DisplayOrder), ct);

		if (r.IsError)
		{
			AddErrorsToModelState(r.Errors);
			model.Id = id;
			return View(model);
		}

		TempData["SuccessMessage"] = "شعبه با موفقیت ویرایش شد.";
		return RedirectToAction(nameof(Index));
	}

	// ================ حذف ================

	[HasPermission(Permissions.Inventory.ManageBranches)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(long id, CancellationToken ct)
	{
		var r = await Mediator.Send(new DeleteBranchCommand(id), ct);
		TempData[r.IsError ? "ErrorMessage" : "SuccessMessage"] =
			r.IsError ? r.FirstError.Description : "شعبه حذف شد.";
		return RedirectToAction(nameof(Index));
	}

	// ================ Toggle Active ================

	[HasPermission(Permissions.Inventory.ManageBranches)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ToggleActive(long id, CancellationToken ct)
	{
		var r = await Mediator.Send(new ToggleBranchActiveCommand(id), ct);
		if (r.IsError)
			return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true, isActive = r.Value });
	}
}