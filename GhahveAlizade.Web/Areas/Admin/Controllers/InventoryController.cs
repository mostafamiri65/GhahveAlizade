using GhahveAlizade.Application.Features.Content.Dtos;
using GhahveAlizade.Application.Features.Inventory.Commands.AdjustInventory;
using GhahveAlizade.Application.Features.Inventory.Queries.GetInventoryItemById;
using GhahveAlizade.Application.Features.Inventory.Queries.GetInventoryItemsList;
using GhahveAlizade.Application.Features.Inventory.Queries.GetInventoryTransactions;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using GhahveAlizade.Web.Areas.Admin.Models.Inventory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class InventoryController : AdminBaseController
{
	[HasPermission(Permissions.Inventory.View)]
	public async Task<IActionResult> Index(
		int pageNumber = 1, int pageSize = 20, string? search = null,
		long? warehouseId = null, bool lowStockOnly = false,
		CancellationToken ct = default)
	{
		ViewData["Title"] = "مدیریت موجودی";
		ViewData["PageIcon"] = "bi-boxes";

		var r = await Mediator.Send(new GetInventoryItemsListQuery(
			pageNumber, pageSize, search, warehouseId, null, lowStockOnly), ct);

		if (r.IsError) return Problem(r.Errors);

		// انبارها برای Dropdown
		ViewBag.Warehouses = await GetWarehousesAsync(ct);
		ViewBag.Search = search;
		ViewBag.WarehouseId = warehouseId;
		ViewBag.LowStockOnly = lowStockOnly;

		return View(r.Value);
	}

	[HasPermission(Permissions.Inventory.View)]
	public async Task<IActionResult> Detail(long id, CancellationToken ct)
	{
		ViewData["Title"] = "جزئیات موجودی";
		ViewData["PageIcon"] = "bi-boxes";

		var r = await Mediator.Send(new GetInventoryItemByIdQuery(id), ct);
		if (r.IsError) { TempData["ErrorMessage"] = r.FirstError.Description; return RedirectToAction(nameof(Index)); }

		var tx = await Mediator.Send(new GetInventoryTransactionsQuery(id, 1, 50), ct);
		ViewBag.Transactions = tx.IsError ? null : tx.Value;

		return View(r.Value);
	}

	[HasPermission(Permissions.Inventory.Adjust)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Adjust(
		long id, decimal newQuantity, string? notes, CancellationToken ct)
	{
		var r = await Mediator.Send(new AdjustInventoryCommand(id, newQuantity, notes), ct);
		if (r.IsError) return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true, message = "موجودی به‌روزرسانی شد." });
	}

	[HasPermission(Permissions.Inventory.Adjust)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> SetReorderLevel(
		long id, decimal? reorderLevel, CancellationToken ct)
	{
		var r = await Mediator.Send(new SetReorderLevelCommand(id, reorderLevel), ct);
		if (r.IsError) return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true, message = "سطح هشدار ذخیره شد." });
	}

	private async Task<List<LookupItemDto>> GetWarehousesAsync(CancellationToken ct)
	{
		var res = await Mediator.Send(new GetWarehousesLookupQuery());
		return res.Value;
	}
}