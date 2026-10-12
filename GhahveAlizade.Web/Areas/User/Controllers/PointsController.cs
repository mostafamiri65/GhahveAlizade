using GhahveAlizade.Application.Features.UserPanel.Queries.GetMyPoints;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.User.Controllers;

public class PointsController : UserBaseController
{
	public async Task<IActionResult> Index(CancellationToken ct)
	{
		ViewData["Title"] = "امتیاز و باشگاه مشتریان";
		var r = await Mediator.Send(new GetMyPointsQuery(CurrentUserId), ct);
		if (r.IsError) return Problem(r.Errors);
		return View(r.Value);
	}
}
