using GhahveAlizade.Application.Features.Commerce.Commands.ChangeOrderStatus;
using GhahveAlizade.Application.Features.Commerce.Commands.UpdateAdminNotes;
using GhahveAlizade.Application.Features.Commerce.Queries.GetOrderById;
using GhahveAlizade.Application.Features.Commerce.Queries.GetOrdersList;
using GhahveAlizade.Domain.Enums;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class OrdersController : AdminBaseController
{
	[HasPermission(Permissions.Orders.View)]
	public async Task<IActionResult> Index(
		int pageNumber = 1, int pageSize = 20, string? search = null,
		OrderStatus? status = null,
		DateTime? fromDate = null, DateTime? toDate = null,
		decimal? minTotal = null, decimal? maxTotal = null,
		string? sortBy = "createdAt", string? sortDir = "desc",
		CancellationToken ct = default)
	{
		ViewData["Title"] = "سفارشات";
		ViewData["PageIcon"] = "bi-receipt";

		var r = await Mediator.Send(new GetOrdersListQuery(
			pageNumber, pageSize, search, status, fromDate, toDate,
			minTotal, maxTotal, sortBy, sortDir), ct);

		if (r.IsError) return Problem(r.Errors);

		ViewBag.Search = search;
		ViewBag.Status = status;
		ViewBag.FromDate = fromDate;
		ViewBag.ToDate = toDate;
		ViewBag.SortBy = sortBy;

		return View(r.Value);
	}

	[HasPermission(Permissions.Orders.ViewDetails)]
	public async Task<IActionResult> Detail(long id, CancellationToken ct)
	{
		ViewData["Title"] = "جزئیات سفارش";
		ViewData["PageIcon"] = "bi-receipt";

		var r = await Mediator.Send(new GetOrderByIdQuery(id), ct);
		if (r.IsError)
		{
			TempData["ErrorMessage"] = r.FirstError.Description;
			return RedirectToAction(nameof(Index));
		}

		return View(r.Value);
	}

	[HasPermission(Permissions.Orders.ChangeStatus)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ChangeStatus(
		long id, OrderStatus newStatus, string? note, CancellationToken ct)
	{
		var r = await Mediator.Send(new ChangeOrderStatusCommand(id, newStatus, note), ct);
		if (r.IsError) return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true, message = "وضعیت سفارش تغییر کرد." });
	}

	[HasPermission(Permissions.Orders.Edit)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> UpdateNotes(
		long id, string? adminNotes, CancellationToken ct)
	{
		var r = await Mediator.Send(new UpdateAdminNotesCommand(id, adminNotes), ct);
		if (r.IsError) return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true, message = "یادداشت ذخیره شد." });
	}

	[HasPermission(Permissions.Orders.Print)]
	public async Task<IActionResult> Print(long id, CancellationToken ct)
	{
		var r = await Mediator.Send(new GetOrderByIdQuery(id), ct);
		if (r.IsError)
		{
			TempData["ErrorMessage"] = r.FirstError.Description;
			return RedirectToAction(nameof(Index));
		}
		return View(r.Value);
	}
}