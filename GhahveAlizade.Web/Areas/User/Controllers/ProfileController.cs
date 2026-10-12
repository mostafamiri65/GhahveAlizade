using GhahveAlizade.Application.Features.UserPanel.Commands.UpdateMyProfile;
using GhahveAlizade.Application.Features.UserPanel.Queries.GetMyProfile;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.User.Controllers;

public class ProfileController : UserBaseController
{
	[HttpGet]
	public async Task<IActionResult> Index(CancellationToken ct)
	{
		ViewData["Title"] = "پروفایل من";
		var r = await Mediator.Send(new GetMyProfileQuery(CurrentUserId), ct);
		if (r.IsError)
		{
			TempData["ErrorMessage"] = r.FirstError.Description;
			return RedirectToAction("Index", "Dashboard");
		}

		var model = new ProfileViewModel
		{
			FirstName = r.Value.FirstName,
			LastName = r.Value.LastName,
			Email = r.Value.Email,
			NationalCode = r.Value.NationalCode,
			BirthDate = r.Value.BirthDate,
			Gender = r.Value.Gender,
			AcceptsMarketing = r.Value.AcceptsMarketing,
			AvatarUrl = r.Value.AvatarUrl,
			Tier = r.Value.Tier,
			CurrentPoints = r.Value.CurrentPoints
		};
		return View(model);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Update(ProfileViewModel model, CancellationToken ct)
	{
		if (!ModelState.IsValid)
		{
			var p = await Mediator.Send(new GetMyProfileQuery(CurrentUserId), ct);
			model.AvatarUrl = p.IsError ? null : p.Value.AvatarUrl;
			model.Tier = p.IsError ? null : p.Value.Tier;
			return View(nameof(Index), model);
		}

		var r = await Mediator.Send(new UpdateMyProfileCommand(
			CurrentUserId, model.FirstName, model.LastName, model.Email,
			model.NationalCode, model.BirthDate, model.Gender, model.AcceptsMarketing), ct);

		if (r.IsError)
		{
			foreach (var e in r.Errors)
				ModelState.AddModelError(e.Code ?? string.Empty, e.Description);
			return View(nameof(Index), model);
		}

		TempData["SuccessMessage"] = "پروفایل با موفقیت ذخیره شد.";
		return RedirectToAction(nameof(Index));
	}
}

public class ProfileViewModel
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? Email { get; set; }
	public string? NationalCode { get; set; }
	public DateOnly? BirthDate { get; set; }
	public string? Gender { get; set; }
	public bool AcceptsMarketing { get; set; }
	public string? AvatarUrl { get; set; }
	public string? Tier { get; set; }
	public int CurrentPoints { get; set; }
}
