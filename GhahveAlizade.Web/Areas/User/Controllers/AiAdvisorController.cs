using GhahveAlizade.Application.Features.AiAdvisor.Commands.SendAiMessage;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.User.Controllers;

public class AiAdvisorController : UserBaseController
{
	public IActionResult Index()
	{
		ViewData["Title"] = "مشاور هوشمند قهوه";
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Send(string message, CancellationToken ct)
	{
		var r = await Mediator.Send(new SendAiMessageCommand(CurrentUserId, message), ct);
		if (r.IsError)
			return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true, reply = r.Value.Reply });
	}
}
