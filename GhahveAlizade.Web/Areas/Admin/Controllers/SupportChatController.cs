using GhahveAlizade.Application.Features.Support.Commands.CloseChatSession;
using GhahveAlizade.Application.Features.Support.Commands.SendChatMessage;
using GhahveAlizade.Application.Features.Support.Queries.GetChatHistory;
using GhahveAlizade.Application.Features.Support.Queries.GetChatSessionsList;
using GhahveAlizade.Domain.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "SuperAdmin,Admin,Support")]
[Route("admin/[controller]/[action]")]
public class SupportChatController : AdminBaseController
{
	public async Task<IActionResult> Index(
		int pageNumber = 1, ChatSessionStatus? status = null, CancellationToken ct = default)
	{
		ViewData["Title"] = "چت آنلاین";
		ViewData["PageIcon"] = "bi-chat-dots";

		var r = await Mediator.Send(new GetChatSessionsListQuery(pageNumber, 20, status), ct);
		if (r.IsError) return Problem(r.Errors);
		ViewBag.Status = status;
		return View(r.Value);
	}

	public async Task<IActionResult> Session(long id, CancellationToken ct)
	{
		ViewData["Title"] = "گفتگو";
		ViewData["PageIcon"] = "bi-chat-dots";

		var r = await Mediator.Send(new GetChatHistoryQuery(id), ct);
		if (r.IsError)
		{
			TempData["ErrorMessage"] = r.FirstError.Description;
			return RedirectToAction(nameof(Index));
		}
		ViewBag.SessionId = id;
		return View(r.Value);
	}

	[HttpGet]
	public async Task<IActionResult> Poll(long id, long? afterMessageId, CancellationToken ct)
	{
		var r = await Mediator.Send(new GetChatHistoryQuery(id, afterMessageId), ct);
		if (r.IsError)
			return Json(new { success = false, message = r.FirstError.Description });

		return Json(new
		{
			success = true,
			r.Value.SessionId,
			r.Value.Status,
			messages = r.Value.Messages.Select(m => new
			{
				m.Id,
				m.SenderName,
				m.IsFromAdmin,
				m.Body,
				createdAt = m.CreatedAtUtc.ToLocalTime().ToString("HH:mm")
			})
		});
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Send(long sessionId, string body, CancellationToken ct)
	{
		var adminId = User.GetUserId();
		if (adminId is null) return Challenge();

		var r = await Mediator.Send(new SendChatMessageCommand(adminId.Value, sessionId, body, IsFromAdmin: true), ct);
		if (r.IsError)
			return Json(new { success = false, message = r.FirstError.Description });

		return Json(new
		{
			success = true,
			message = new
			{
				r.Value.Id,
				r.Value.SenderName,
				r.Value.IsFromAdmin,
				r.Value.Body,
				createdAt = r.Value.CreatedAtUtc.ToLocalTime().ToString("HH:mm")
			}
		});
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Close(long sessionId, CancellationToken ct)
	{
		var adminId = User.GetUserId();
		if (adminId is null) return Challenge();

		var r = await Mediator.Send(new CloseChatSessionCommand(adminId.Value, sessionId, IsFromAdmin: true), ct);
		if (r.IsError)
			return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true });
	}
}
