using ErrorOr;
using GhahveAlizade.Application.Features.Support.Commands.AddTicketReply;
using GhahveAlizade.Application.Features.Support.Commands.UpdateTicket;
using GhahveAlizade.Application.Features.Support.Queries.GetTicketDetail;
using GhahveAlizade.Application.Features.Support.Queries.GetTicketsList;
using GhahveAlizade.Domain.Support;
using GhahveAlizade.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "SuperAdmin,Admin,Support")]
[Route("admin/[controller]/[action]")]
public class SupportTicketsController : AdminBaseController
{
	public async Task<IActionResult> Index(
		int pageNumber = 1, string? search = null, TicketStatus? status = null,
		TicketPriority? priority = null, CancellationToken ct = default)
	{
		ViewData["Title"] = "تیکت‌های پشتیبانی";
		ViewData["PageIcon"] = "bi-life-preserver";

		var r = await Mediator.Send(new GetTicketsListQuery(pageNumber, 20, search, status, priority), ct);
		if (r.IsError) return Problem(r.Errors);

		ViewBag.Search = search;
		ViewBag.Status = status;
		ViewBag.Priority = priority;
		return View(r.Value);
	}

	public async Task<IActionResult> Detail(long id, CancellationToken ct)
	{
		ViewData["Title"] = "جزئیات تیکت";
		ViewData["PageIcon"] = "bi-life-preserver";

		var r = await Mediator.Send(new GetTicketDetailQuery(id), ct);
		if (r.IsError)
		{
			TempData["ErrorMessage"] = r.FirstError.Description;
			return RedirectToAction(nameof(Index));
		}
		return View(r.Value);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Reply(long id, string body, bool isInternalNote, CancellationToken ct)
	{
		var adminId = User.GetUserId();
		if (adminId is null) return Challenge();

		var r = await Mediator.Send(new AddTicketReplyCommand(
			adminId.Value, id, body, IsFromAdmin: true, IsInternalNote: isInternalNote), ct);

		if (r.IsError)
			return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true, message = "پاسخ ثبت شد." });
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Update(
		long id, TicketStatus? status, TicketPriority? priority, long? assignToUserId, CancellationToken ct)
	{
		var adminId = User.GetUserId();
		if (adminId is null) return Challenge();

		var r = await Mediator.Send(new UpdateTicketCommand(adminId.Value, id, status, priority, assignToUserId), ct);
		if (r.IsError)
			return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true, message = "تیکت به‌روزرسانی شد." });
	}
}
