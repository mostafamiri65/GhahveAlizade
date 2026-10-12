using System.Text;
using GhahveAlizade.Application.Features.Newsletter.Commands.ToggleSubscriber;
using GhahveAlizade.Application.Features.Newsletter.Queries.GetNewsletterSubscribersList;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class NewsletterController : AdminBaseController
{
	[HasPermission(Permissions.Newsletter.View)]
	public async Task<IActionResult> Index(
		int pageNumber = 1, int pageSize = 20, string? search = null,
		bool? isActive = null, bool? isVerified = null,
		string? sortBy = "createdAt", string? sortDir = "desc",
		CancellationToken ct = default)
	{
		ViewData["Title"] = "خبرنامه";
		ViewData["PageIcon"] = "bi-envelope";

		var result = await Mediator.Send(new GetNewsletterSubscribersListQuery(
			pageNumber, pageSize, search, isActive, isVerified, sortBy, sortDir), ct);

		if (result.IsError) return Problem(result.Errors);

		ViewBag.Search = search;
		ViewBag.IsActive = isActive;
		ViewBag.IsVerified = isVerified;
		return View(result.Value);
	}

	[HasPermission(Permissions.Newsletter.Export)]
	[HttpGet]
	public async Task<IActionResult> Export(CancellationToken ct)
	{
		var result = await Mediator.Send(new GetNewsletterSubscribersListQuery(
			1, 100000, null, true, null, "createdAt", "desc"), ct);

		if (result.IsError) return Problem(result.Errors);

		var sb = new StringBuilder();
		sb.AppendLine("Email,FirstName,LastName,PhoneNumber,Source,SubscribedAt");

		foreach (var s in result.Value.Items)
		{
			sb.AppendLine($"{s.Email},{s.FirstName},{s.LastName},{s.PhoneNumber},{s.Source},{s.SubscribedAtUtc:yyyy-MM-dd}");
		}

		var bytes = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray();
		return File(bytes, "text/csv", $"newsletter_{DateTime.UtcNow:yyyyMMdd}.csv");
	}

	[HasPermission(Permissions.Newsletter.View)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ToggleActive(long id, CancellationToken ct)
	{
		var r = await Mediator.Send(new ToggleSubscriberActiveCommand(id), ct);
		if (r.IsError) return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true, isActive = r.Value });
	}

	[HasPermission(Permissions.Newsletter.View)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Verify(long id, CancellationToken ct)
	{
		var r = await Mediator.Send(new VerifySubscriberCommand(id), ct);
		if (r.IsError) return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true, message = "تایید شد." });
	}

	[HasPermission(Permissions.Newsletter.View)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(long id, CancellationToken ct)
	{
		var r = await Mediator.Send(new DeleteSubscriberCommand(id), ct);
		if (r.IsError) return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true });
	}
}