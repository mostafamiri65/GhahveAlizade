using GhahveAlizade.Application.Features.UserPanel.Commands.ToggleMyWishlistItem;
using GhahveAlizade.Application.Features.UserPanel.Queries.GetMyWishlist;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.User.Controllers;

public class WishlistController : UserBaseController
{
	public async Task<IActionResult> Index(CancellationToken ct)
	{
		ViewData["Title"] = "علاقه‌مندی‌های من";
		var r = await Mediator.Send(new GetMyWishlistQuery(CurrentUserId), ct);
		if (r.IsError) return Problem(r.Errors);
		return View(r.Value);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Toggle(long productId, CancellationToken ct)
	{
		var r = await Mediator.Send(new ToggleMyWishlistItemCommand(CurrentUserId, productId), ct);
		if (r.IsError)
			return Json(new { success = false, message = r.FirstError.Description });

		return Json(new
		{
			success = true,
			added = r.Value,
			message = r.Value ? "به علاقه‌مندی‌ها اضافه شد." : "از علاقه‌مندی‌ها حذف شد."
		});
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Remove(long productId, CancellationToken ct)
	{
		var r = await Mediator.Send(new ToggleMyWishlistItemCommand(CurrentUserId, productId), ct);
		TempData[r.IsError ? "ErrorMessage" : "SuccessMessage"] =
			r.IsError ? r.FirstError.Description
			: r.Value ? "به علاقه‌مندی‌ها اضافه شد." : "از علاقه‌مندی‌ها حذف شد.";
		return RedirectToAction(nameof(Index));
	}
}
