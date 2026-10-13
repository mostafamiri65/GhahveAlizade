using GhahveAlizade.Application.Features.Support.Commands.CloseChatSession;
using GhahveAlizade.Application.Features.Support.Commands.SendChatMessage;
using GhahveAlizade.Application.Features.Support.Queries.GetMyChatHistory;
using GhahveAlizade.Application.Features.Support.Queries.GetOrCreateMyChatSession;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.User.Controllers;

public class ChatController : UserBaseController
{
	/// <summary>صفحه چت — جلسه باز را می‌گیرد/می‌سازد و ویو را نشان می‌دهد.</summary>
	public async Task<IActionResult> Index(CancellationToken ct)
	{
		ViewData["Title"] = "گفتگوی آنلاین با پشتیبانی";
		var r = await Mediator.Send(new GetOrCreateMyChatSessionQuery(CurrentUserId), ct);
		if (r.IsError)
		{
			TempData["ErrorMessage"] = r.FirstError.Description;
			return RedirectToAction("Index", "Dashboard");
		}
		ViewBag.SessionId = r.Value.Id;
		ViewBag.SessionStatus = r.Value.Status;
		return View();
	}

	/// <summary>Polling: پیام‌های جدید بعد از afterMessageId</summary>
	[HttpGet]
	public async Task<IActionResult> History(long sessionId, long? afterMessageId, CancellationToken ct)
	{
		var r = await Mediator.Send(new GetMyChatHistoryQuery(CurrentUserId, sessionId, afterMessageId), ct);
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
		var r = await Mediator.Send(new SendChatMessageCommand(CurrentUserId, sessionId, body), ct);
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
		var r = await Mediator.Send(new CloseChatSessionCommand(CurrentUserId, sessionId), ct);
		if (r.IsError)
			return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true });
	}
}
