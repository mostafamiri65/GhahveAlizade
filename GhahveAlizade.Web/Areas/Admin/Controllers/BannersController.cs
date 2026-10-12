using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Marketing.Commands.SaveBanner;
using GhahveAlizade.Application.Features.Marketing.Queries.GetBannerById;
using GhahveAlizade.Application.Features.Marketing.Queries.GetBannerFormData;
using GhahveAlizade.Application.Features.Marketing.Queries.GetBannersList;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using GhahveAlizade.Web.Areas.Admin.Models.Marketing;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class BannersController : AdminBaseController
{
	private readonly IFileStorageService _fileStorage;

	public BannersController(IFileStorageService fileStorage) => _fileStorage = fileStorage;

	[HasPermission(Permissions.Marketing.ViewBanners)]
	public async Task<IActionResult> Index(
		int pageNumber = 1, int pageSize = 20, string? search = null,
		bool? isActive = null, string? sortBy = "displayOrder", string? sortDir = "asc",
		CancellationToken ct = default)
	{
		ViewData["Title"] = "بنرها";
		ViewData["PageIcon"] = "bi-image";

		var result = await Mediator.Send(new GetBannersListQuery(
			pageNumber, pageSize, search, isActive, sortBy, sortDir), ct);

		if (result.IsError) return Problem(result.Errors);

		ViewBag.Search = search;
		ViewBag.IsActive = isActive;
		return View(result.Value);
	}

	[HasPermission(Permissions.Marketing.ManageBanners)]
	[HttpGet]
	public async Task<IActionResult> Create(CancellationToken ct)
	{
		ViewData["Title"] = "افزودن بنر";
		ViewData["PageIcon"] = "bi-plus-circle";

		var formData = await Mediator.Send(new GetBannerFormDataQuery(), ct);

		return View(new BannerFormViewModel
		{
			IsActive = true,
			Target = "_self",
			FormData = formData.IsError ? new() : formData.Value
		});
	}

	[HasPermission(Permissions.Marketing.ManageBanners)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	[RequestSizeLimit(20 * 1024 * 1024)]
	public async Task<IActionResult> Create(
		BannerFormViewModel model,
		IFormFile? imageFile,
		IFormFile? mobileImageFile,
		CancellationToken ct)
	{
		// آپلود تصاویر
		if (imageFile is not null && imageFile.Length > 0)
		{
			await using var s = imageFile.OpenReadStream();
			var r = await _fileStorage.UploadAsync(s, imageFile.FileName, imageFile.ContentType, "banners", ct);
			if (r.Success) model.ImageUrl = r.Url!;
		}

		if (mobileImageFile is not null && mobileImageFile.Length > 0)
		{
			await using var s = mobileImageFile.OpenReadStream();
			var r = await _fileStorage.UploadAsync(s, mobileImageFile.FileName, mobileImageFile.ContentType, "banners", ct);
			if (r.Success) model.MobileImageUrl = r.Url!;
		}

		if (!ModelState.IsValid || string.IsNullOrEmpty(model.ImageUrl))
		{
			if (string.IsNullOrEmpty(model.ImageUrl))
				ModelState.AddModelError(nameof(model.ImageUrl), "آدرس تصویر الزامی است.");
			await ReloadFormDataAsync(model, ct);
			return View(model);
		}

		var result = await Mediator.Send(new CreateBannerCommand(
			model.Title, model.Subtitle, model.Description, model.ImageUrl,
			model.MobileImageUrl, model.AltText, model.LinkUrl, model.ButtonText,
			model.Target, model.StartsAtUtc, model.ExpiresAtUtc,
			model.IsActive, model.DisplayOrder,
			model.Placements.Select(p => new BannerPlacementInput(p.Placement, p.DisplayOrder, p.IsActive)).ToList()
		), ct);

		if (result.IsError)
		{
			AddErrorsToModelState(result.Errors);
			await ReloadFormDataAsync(model, ct);
			return View(model);
		}

		TempData["SuccessMessage"] = "بنر ایجاد شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Marketing.ManageBanners)]
	[HttpGet]
	public async Task<IActionResult> Edit(long id, CancellationToken ct)
	{
		ViewData["Title"] = "ویرایش بنر";
		ViewData["PageIcon"] = "bi-pencil";

		var result = await Mediator.Send(new GetBannerByIdQuery(id), ct);
		if (result.IsError)
		{
			TempData["ErrorMessage"] = result.FirstError.Description;
			return RedirectToAction(nameof(Index));
		}

		var formData = await Mediator.Send(new GetBannerFormDataQuery(), ct);
		var b = result.Value;

		return View(new BannerFormViewModel
		{
			Id = b.Id,
			Title = b.Title,
			Subtitle = b.Subtitle,
			Description = b.Description,
			ImageUrl = b.ImageUrl,
			MobileImageUrl = b.MobileImageUrl,
			AltText = b.AltText,
			LinkUrl = b.LinkUrl,
			ButtonText = b.ButtonText,
			Target = b.Target ?? "_self",
			StartsAtUtc = b.StartsAtUtc,
			ExpiresAtUtc = b.ExpiresAtUtc,
			IsActive = b.IsActive,
			DisplayOrder = b.DisplayOrder,
			Placements = b.Placements.Select(p => new BannerPlacementViewModel
			{
				Id = p.Id,
				Placement = (int)p.Placement,
				PlacementName = p.PlacementName ?? "",
				DisplayOrder = p.DisplayOrder,
				IsActive = p.IsActive
			}).ToList(),
			FormData = formData.IsError ? new() : formData.Value
		});
	}

	[HasPermission(Permissions.Marketing.ManageBanners)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	[RequestSizeLimit(20 * 1024 * 1024)]
	public async Task<IActionResult> Edit(
		long id,
		BannerFormViewModel model,
		IFormFile? imageFile,
		IFormFile? mobileImageFile,
		CancellationToken ct)
	{
		if (imageFile is not null && imageFile.Length > 0)
		{
			await using var s = imageFile.OpenReadStream();
			var r = await _fileStorage.UploadAsync(s, imageFile.FileName, imageFile.ContentType, "banners", ct);
			if (r.Success) model.ImageUrl = r.Url!;
		}

		if (mobileImageFile is not null && mobileImageFile.Length > 0)
		{
			await using var s = mobileImageFile.OpenReadStream();
			var r = await _fileStorage.UploadAsync(s, mobileImageFile.FileName, mobileImageFile.ContentType, "banners", ct);
			if (r.Success) model.MobileImageUrl = r.Url!;
		}

		if (!ModelState.IsValid)
		{
			await ReloadFormDataAsync(model, ct);
			return View(model);
		}

		var result = await Mediator.Send(new UpdateBannerCommand(
			id, model.Title, model.Subtitle, model.Description, model.ImageUrl,
			model.MobileImageUrl, model.AltText, model.LinkUrl, model.ButtonText,
			model.Target, model.StartsAtUtc, model.ExpiresAtUtc,
			model.IsActive, model.DisplayOrder,
			model.Placements.Select(p => new BannerPlacementInput(p.Placement, p.DisplayOrder, p.IsActive)).ToList()
		), ct);

		if (result.IsError)
		{
			AddErrorsToModelState(result.Errors);
			model.Id = id;
			await ReloadFormDataAsync(model, ct);
			return View(model);
		}

		TempData["SuccessMessage"] = "بنر ویرایش شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Marketing.ManageBanners)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(long id, CancellationToken ct)
	{
		var result = await Mediator.Send(new DeleteBannerCommand(id), ct);
		TempData[result.IsError ? "ErrorMessage" : "SuccessMessage"] =
			result.IsError ? result.FirstError.Description : "بنر حذف شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Marketing.ManageBanners)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ToggleActive(long id, CancellationToken ct)
	{
		var result = await Mediator.Send(new ToggleBannerActiveCommand(id), ct);
		if (result.IsError)
			return Json(new { success = false, message = result.FirstError.Description });
		return Json(new { success = true, isActive = result.Value });
	}

	private async Task ReloadFormDataAsync(BannerFormViewModel model, CancellationToken ct)
	{
		var fd = await Mediator.Send(new GetBannerFormDataQuery(), ct);
		if (!fd.IsError) model.FormData = fd.Value;
	}
}