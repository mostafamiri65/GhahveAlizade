using GhahveAlizade.Application.Features.UserPanel.Commands.CancelMyOrder;
using GhahveAlizade.Application.Features.UserPanel.Queries.GetMyOrderById;
using GhahveAlizade.Application.Features.UserPanel.Queries.GetMyOrders;
using GhahveAlizade.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.User.Controllers;

[Route("user/orders/[action]")]
public class OrdersController : UserBaseController
{
	public async Task<IActionResult> Index(
		int pageNumber = 1, OrderStatus? status = null, CancellationToken ct = default)
	{
		ViewData["Title"] = "سفارش‌های من";
		var r = await Mediator.Send(new GetMyOrdersQuery(CurrentUserId, pageNumber, 10, status), ct);
		if (r.IsError) return Problem(r.Errors);
		ViewBag.Status = status;
		return View(r.Value);
	}

	public async Task<IActionResult> Detail(long id, CancellationToken ct)
	{
		ViewData["Title"] = "جزئیات سفارش";
		var r = await Mediator.Send(new GetMyOrderByIdQuery(CurrentUserId, id), ct);
		if (r.IsError)
		{
			TempData["ErrorMessage"] = r.FirstError.Description;
			return RedirectToAction(nameof(Index));
		}
		return View(r.Value);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Cancel(long id, string? reason, CancellationToken ct)
	{
		var r = await Mediator.Send(new CancelMyOrderCommand(CurrentUserId, id, reason), ct);
		TempData[r.IsError ? "ErrorMessage" : "SuccessMessage"] =
			r.IsError ? r.FirstError.Description : "سفارش لغو شد.";
		return RedirectToAction(nameof(Detail), new { id });
	}
}
