using GhahveAlizade.Application.Features.Settings.Commands.SaveSettings;
using GhahveAlizade.Application.Features.Settings.Queries.GetSettingsGrouped;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class SettingsController : AdminBaseController
{
	[HasPermission(Permissions.Settings.View)]
	public async Task<IActionResult> Index(CancellationToken ct)
	{
		ViewData["Title"] = "تنظیمات";
		ViewData["PageIcon"] = "bi-gear";

		var r = await Mediator.Send(new GetSettingsGroupedQuery(), ct);
		if (r.IsError) return Problem(r.Errors);

		return View(r.Value);
	}

	[HasPermission(Permissions.Settings.Edit)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Save(
		[FromForm] Dictionary<string, string?> values, CancellationToken ct)
	{
		var settings = values
			.Where(kv => kv.Key.StartsWith("setting_"))
			.Select(kv => new SettingValueInput(
				kv.Key.Substring("setting_".Length),
				kv.Value))
			.ToList();

		var r = await Mediator.Send(new SaveSettingsCommand(settings), ct);
		if (r.IsError) return Json(new { success = false, message = r.FirstError.Description });

		TempData["SuccessMessage"] = "تنظیمات ذخیره شد.";
		return Json(new { success = true, message = "تنظیمات ذخیره شد." });
	}

	[HasPermission(Permissions.Settings.Edit)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Upsert(
		string key, string? value, string? description,
		string? group, string valueType, bool isPublic, int displayOrder,
		CancellationToken ct)
	{
		var r = await Mediator.Send(new UpsertSettingCommand(
			key, value, description, group, valueType, isPublic, displayOrder), ct);

		if (r.IsError) return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true, message = "تنظیم ذخیره شد." });
	}

	[HasPermission(Permissions.Settings.Edit)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(long id, CancellationToken ct)
	{
		var r = await Mediator.Send(new DeleteSettingCommand(id), ct);
		if (r.IsError) return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true, message = "تنظیم حذف شد." });
	}
}