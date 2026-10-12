using ErrorOr;
using GhahveAlizade.Application.Features.UserPanel.Queries.GetMyDashboard;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.User.Controllers;

public class DashboardController : UserBaseController
{
	public async Task<IActionResult> Index(CancellationToken ct)
	{
		ViewData["Title"] = "داشبورد من";
		var r = await Mediator.Send(new GetMyDashboardQuery(CurrentUserId), ct);
		if (r.IsError)
		{
			TempData["ErrorMessage"] = r.FirstError.Description;
			return View(new GhahveAlizade.Application.Features.UserPanel.Dtos.MyDashboardDto());
		}
		return View(r.Value);
	}
}
