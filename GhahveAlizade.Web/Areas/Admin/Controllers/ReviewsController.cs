using GhahveAlizade.Application.Features.Reviews.Commands.ApproveReview;
using GhahveAlizade.Application.Features.Reviews.Commands.DeleteReview;
using GhahveAlizade.Application.Features.Reviews.Commands.ReplyToReview;
using GhahveAlizade.Application.Features.Reviews.Queries.GetReviewById;
using GhahveAlizade.Application.Features.Reviews.Queries.GetReviewsList;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class ReviewsController : AdminBaseController
{
	[HasPermission(Permissions.Reviews.View)]
	public async Task<IActionResult> Index(
		int pageNumber = 1, int pageSize = 20, string? search = null,
		long? productId = null, int? rating = null, bool? isApproved = null,
		string? sortBy = "createdAt", string? sortDir = "desc",
		CancellationToken ct = default)
	{
		ViewData["Title"] = "مدیریت نظرات";
		ViewData["PageIcon"] = "bi-star";

		var result = await Mediator.Send(new GetReviewsListQuery(
			pageNumber, pageSize, search, productId, rating, isApproved, sortBy, sortDir), ct);

		if (result.IsError) return Problem(result.Errors);

		ViewBag.Search = search;
		ViewBag.Rating = rating;
		ViewBag.IsApproved = isApproved;
		ViewBag.ProductId = productId;

		return View(result.Value);
	}

	[HasPermission(Permissions.Reviews.View)]
	public async Task<IActionResult> Detail(long id, CancellationToken ct)
	{
		ViewData["Title"] = "جزئیات نظر";
		ViewData["PageIcon"] = "bi-star";

		var result = await Mediator.Send(new GetReviewByIdQuery(id), ct);
		if (result.IsError)
		{
			TempData["ErrorMessage"] = result.FirstError.Description;
			return RedirectToAction(nameof(Index));
		}

		return View(result.Value);
	}

	[HasPermission(Permissions.Reviews.Approve)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Approve(long id, CancellationToken ct)
	{
		var result = await Mediator.Send(new ApproveReviewCommand(id), ct);
		if (result.IsError)
			return Json(new { success = false, message = result.FirstError.Description });
		return Json(new
		{
			success = true,
			isApproved = result.Value,
			message = result.Value ? "تایید شد" : "رد تایید"
		});
	}

	[HasPermission(Permissions.Reviews.Delete)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(long id, CancellationToken ct)
	{
		var result = await Mediator.Send(new DeleteReviewCommand(id), ct);
		if (result.IsError)
			TempData["ErrorMessage"] = result.FirstError.Description;
		else
			TempData["SuccessMessage"] = "نظر حذف شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Reviews.Reply)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Reply(
		long reviewId, string content, bool isOfficial, CancellationToken ct)
	{
		if (string.IsNullOrWhiteSpace(content))
			return Json(new { success = false, message = "متن پاسخ الزامی است." });

		var result = await Mediator.Send(new ReplyToReviewCommand(
			reviewId, content, isOfficial), ct);

		if (result.IsError)
			return Json(new { success = false, message = result.FirstError.Description });
		return Json(new { success = true, message = "پاسخ ثبت شد." });
	}
}