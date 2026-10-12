using GhahveAlizade.Application.Features.Wholesale.Commands.UpdateWholesaleStatus;
using GhahveAlizade.Application.Features.Wholesale.Queries.GetWholesaleRequestById;
using GhahveAlizade.Application.Features.Wholesale.Queries.GetWholesaleRequestsList;
using GhahveAlizade.Domain.Enums;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class WholesaleController : AdminBaseController
{
	[HasPermission(Permissions.Wholesale.View)]
	public async Task<IActionResult> Index(
		int pageNumber = 1, int pageSize = 20, string? search = null,
		WholesaleRequestStatus? status = null, CancellationToken ct = default)
	{
		ViewData["Title"] = "عمده‌فروشی";
		ViewData["PageIcon"] = "bi-briefcase";

		var r = await Mediator.Send(new GetWholesaleRequestsListQuery(
			pageNumber, pageSize, search, status), ct);

		if (r.IsError) return Problem(r.Errors);

		ViewBag.Search = search;
		ViewBag.Status = status;
		return View(r.Value);
	}

	[HasPermission(Permissions.Wholesale.View)]
	public async Task<IActionResult> Detail(long id, CancellationToken ct)
	{
		ViewData["Title"] = "جزئیات درخواست";
		ViewData["PageIcon"] = "bi-briefcase";

		var r = await Mediator.Send(new GetWholesaleRequestByIdQuery(id), ct);
		if (r.IsError) { TempData["ErrorMessage"] = r.FirstError.Description; return RedirectToAction(nameof(Index)); }

		return View(r.Value);
	}

	[HasPermission(Permissions.Wholesale.Approve)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Approve(long id, string? reviewNotes, CancellationToken ct)
	{
		var r = await Mediator.Send(new UpdateWholesaleStatusCommand(
			id, WholesaleRequestStatus.Approved, reviewNotes), ct);

		if (r.IsError) return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true, message = "درخواست تایید شد." });
	}

	[HasPermission(Permissions.Wholesale.Reject)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Reject(long id, string? reviewNotes, CancellationToken ct)
	{
		var r = await Mediator.Send(new UpdateWholesaleStatusCommand(
			id, WholesaleRequestStatus.Rejected, reviewNotes), ct);

		if (r.IsError) return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true, message = "درخواست رد شد." });
	}

	[HasPermission(Permissions.Wholesale.Approve)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> SetStatus(long id, WholesaleRequestStatus status, string? reviewNotes, CancellationToken ct)
	{
		var r = await Mediator.Send(new UpdateWholesaleStatusCommand(id, status, reviewNotes), ct);
		if (r.IsError) return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true, message = "وضعیت ذخیره شد." });
	}
}