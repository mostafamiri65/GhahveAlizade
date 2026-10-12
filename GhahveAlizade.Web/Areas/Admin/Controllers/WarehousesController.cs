using GhahveAlizade.Application.Features.Inventory.Commands.SaveWarehouse;
using GhahveAlizade.Application.Features.Inventory.Queries.GetWarehouseById;
using GhahveAlizade.Application.Features.Inventory.Queries.GetWarehousesList;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using GhahveAlizade.Web.Areas.Admin.Models.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class WarehousesController : AdminBaseController
{
	[HasPermission(Permissions.Inventory.ManageWarehouses)]
	public async Task<IActionResult> Index(
		int pageNumber = 1, int pageSize = 20, string? search = null,
		bool? isActive = null, CancellationToken ct = default)
	{
		ViewData["Title"] = "انبارها";
		ViewData["PageIcon"] = "bi-building";

		var r = await Mediator.Send(new GetWarehousesListQuery(
			pageNumber, pageSize, search, isActive), ct);

		if (r.IsError) return Problem(r.Errors);
		ViewBag.Search = search;
		ViewBag.IsActive = isActive;
		return View(r.Value);
	}

	[HasPermission(Permissions.Inventory.ManageWarehouses)]
	[HttpGet]
	public IActionResult Create()
	{
		ViewData["Title"] = "افزودن انبار";
		ViewData["PageIcon"] = "bi-plus-circle";
		return View(new WarehouseFormViewModel { IsActive = true });
	}

	[HasPermission(Permissions.Inventory.ManageWarehouses)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(WarehouseFormViewModel model, CancellationToken ct)
	{
		if (!ModelState.IsValid) return View(model);

		var r = await Mediator.Send(new CreateWarehouseCommand(
			model.Name, model.Code, model.Address, model.City, model.Province,
			model.PostalCode, model.Phone, model.ManagerName,
			model.IsActive, model.IsDefault), ct);

		if (r.IsError) { AddErrorsToModelState(r.Errors); return View(model); }

		TempData["SuccessMessage"] = "انبار ایجاد شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Inventory.ManageWarehouses)]
	[HttpGet]
	public async Task<IActionResult> Edit(long id, CancellationToken ct)
	{
		ViewData["Title"] = "ویرایش انبار";
		ViewData["PageIcon"] = "bi-pencil";

		var r = await Mediator.Send(new GetWarehouseByIdQuery(id), ct);
		if (r.IsError) { TempData["ErrorMessage"] = r.FirstError.Description; return RedirectToAction(nameof(Index)); }

		var w = r.Value;
		return View(new WarehouseFormViewModel
		{
			Id = w.Id,
			Name = w.Name,
			Code = w.Code,
			Address = w.Address,
			City = w.City,
			Province = w.Province,
			PostalCode = w.PostalCode,
			Phone = w.Phone,
			ManagerName = w.ManagerName,
			IsActive = w.IsActive,
			IsDefault = w.IsDefault
		});
	}

	[HasPermission(Permissions.Inventory.ManageWarehouses)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(long id, WarehouseFormViewModel model, CancellationToken ct)
	{
		if (!ModelState.IsValid) return View(model);

		var r = await Mediator.Send(new UpdateWarehouseCommand(
			id, model.Name, model.Code, model.Address, model.City, model.Province,
			model.PostalCode, model.Phone, model.ManagerName,
			model.IsActive, model.IsDefault), ct);

		if (r.IsError) { AddErrorsToModelState(r.Errors); model.Id = id; return View(model); }

		TempData["SuccessMessage"] = "انبار ویرایش شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Inventory.ManageWarehouses)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(long id, CancellationToken ct)
	{
		var r = await Mediator.Send(new DeleteWarehouseCommand(id), ct);
		TempData[r.IsError ? "ErrorMessage" : "SuccessMessage"] =
			r.IsError ? r.FirstError.Description : "انبار حذف شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Inventory.ManageWarehouses)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ToggleActive(long id, CancellationToken ct)
	{
		var r = await Mediator.Send(new ToggleWarehouseActiveCommand(id), ct);
		if (r.IsError) return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true, isActive = r.Value });
	}
}