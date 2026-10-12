using GhahveAlizade.Application.Features.Support.Commands.AddTicketReply;
using GhahveAlizade.Application.Features.Support.Commands.CloseTicket;
using GhahveAlizade.Application.Features.Support.Commands.CreateTicket;
using GhahveAlizade.Application.Features.Support.Queries.GetMyTicketById;
using GhahveAlizade.Application.Features.Support.Queries.GetMyTickets;
using GhahveAlizade.Domain.Support;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.User.Controllers;

public class SupportController : UserBaseController
{
	public async Task<IActionResult> Index(
		int pageNumber = 1, TicketStatus? status = null, CancellationToken ct = default)
	{
		ViewData["Title"] = "پشتیبانی و تیکت‌ها";
		var r = await Mediator.Send(new GetMyTicketsQuery(CurrentUserId, pageNumber, 15, status), ct);
		if (r.IsError) return Problem(r.Errors);
		ViewBag.Status = status;
		return View(r.Value);
	}

	[HttpGet]
	public IActionResult Create()
	{
		ViewData["Title"] = "ثبت تیکت جدید";
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(
		string subject, string body, string category, long? relatedOrderId, CancellationToken ct)
	{
		if (!ModelState.IsValid)
			return View();

		var r = await Mediator.Send(
			new CreateTicketCommand(CurrentUserId, subject, body, category ?? "General", relatedOrderId), ct);

		if (r.IsError)
		{
			TempData["ErrorMessage"] = r.FirstError.Description;
			return View();
		}

		TempData["SuccessMessage"] = $"تیکت {r.Value.TicketNumber} ثبت شد.";
		return RedirectToAction(nameof(Detail), new { id = r.Value.TicketId });
	}

	public async Task<IActionResult> Detail(long id, CancellationToken ct)
	{
		ViewData["Title"] = "جزئیات تیکت";
		var r = await Mediator.Send(new GetMyTicketByIdQuery(CurrentUserId, id), ct);
		if (r.IsError)
		{
			TempData["ErrorMessage"] = r.FirstError.Description;
			return RedirectToAction(nameof(Index));
		}
		return View(r.Value);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Reply(long id, string body, CancellationToken ct)
	{
		var r = await Mediator.Send(new AddTicketReplyCommand(CurrentUserId, id, body), ct);
		TempData[r.IsError ? "ErrorMessage" : "SuccessMessage"] =
			r.IsError ? r.FirstError.Description : "پاسخ شما ثبت شد.";
		return RedirectToAction(nameof(Detail), new { id });
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Close(long id, CancellationToken ct)
	{
		var r = await Mediator.Send(new CloseTicketCommand(CurrentUserId, id), ct);
		TempData[r.IsError ? "ErrorMessage" : "SuccessMessage"] =
			r.IsError ? r.FirstError.Description : "تیکت بسته شد.";
		return RedirectToAction(nameof(Detail), new { id });
	}
}
