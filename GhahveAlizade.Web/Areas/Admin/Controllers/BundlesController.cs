using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Bundles.Commands.SaveBundle;
using GhahveAlizade.Application.Features.Bundles.Queries.GetBundleById;
using GhahveAlizade.Application.Features.Bundles.Queries.GetBundleFormData;
using GhahveAlizade.Application.Features.Bundles.Queries.GetBundlesList;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using GhahveAlizade.Web.Areas.Admin.Models.Bundles;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class BundlesController : AdminBaseController
{
	private readonly IFileStorageService _fileStorage;
	public BundlesController(IFileStorageService fileStorage) => _fileStorage = fileStorage;

	[HasPermission(Permissions.Bundles.View)]
	public async Task<IActionResult> Index(
		int pageNumber = 1, int pageSize = 20, string? search = null,
		bool? isActive = null, bool? isFeatured = null,
		CancellationToken ct = default)
	{
		ViewData["Title"] = "بسته‌های محصول";
		ViewData["PageIcon"] = "bi-boxes";

		var r = await Mediator.Send(new GetBundlesListQuery(
			pageNumber, pageSize, search, isActive, isFeatured), ct);

		if (r.IsError) return Problem(r.Errors);
		ViewBag.Search = search;
		ViewBag.IsActive = isActive;
		return View(r.Value);
	}

	[HasPermission(Permissions.Bundles.Create)]
	[HttpGet]
	public async Task<IActionResult> Create(CancellationToken ct)
	{
		ViewData["Title"] = "افزودن بسته";
		ViewData["PageIcon"] = "bi-plus-circle";

		var fd = await Mediator.Send(new GetBundleFormDataQuery(), ct);

		return View(new BundleFormViewModel
		{
			IsActive = true,
			FormData = fd.IsError ? new() : fd.Value
		});
	}

	[HasPermission(Permissions.Bundles.Create)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	[RequestSizeLimit(20 * 1024 * 1024)]
	public async Task<IActionResult> Create(
		BundleFormViewModel model, IFormFile? imageFile, CancellationToken ct)
	{
		if (imageFile is not null && imageFile.Length > 0)
		{
			await using var s = imageFile.OpenReadStream();
			var r = await _fileStorage.UploadAsync(s, imageFile.FileName, imageFile.ContentType, "bundles", ct);
			if (r.Success) model.ImageUrl = r.Url!;
		}

		if (!ModelState.IsValid)
		{
			await ReloadFormDataAsync(model, ct);
			return View(model);
		}

		var result = await Mediator.Send(new CreateBundleCommand(
			model.Name, model.Slug, model.Description, model.ShortDescription,
			model.ImageUrl, model.BundlePrice, model.CompareAtPrice, model.TotalItemsPrice,
			model.IsActive, model.IsFeatured, model.StartsAtUtc, model.ExpiresAtUtc,
			model.MetaTitle, model.MetaDescription, model.CanonicalUrl, model.OgImage,
			model.Items.Select(i => new BundleItemInput(i.ProductId, i.ProductVariantId, i.Quantity, i.DisplayOrder)).ToList()
		), ct);

		if (result.IsError)
		{
			AddErrorsToModelState(result.Errors);
			await ReloadFormDataAsync(model, ct);
			return View(model);
		}

		TempData["SuccessMessage"] = "بسته ایجاد شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Bundles.Edit)]
	[HttpGet]
	public async Task<IActionResult> Edit(long id, CancellationToken ct)
	{
		ViewData["Title"] = "ویرایش بسته";
		ViewData["PageIcon"] = "bi-pencil";

		var r = await Mediator.Send(new GetBundleByIdQuery(id), ct);
		if (r.IsError) { TempData["ErrorMessage"] = r.FirstError.Description; return RedirectToAction(nameof(Index)); }

		var fd = await Mediator.Send(new GetBundleFormDataQuery(), ct);
		var b = r.Value;

		return View(new BundleFormViewModel
		{
			Id = b.Id,
			Name = b.Name,
			Slug = b.Slug,
			Description = b.Description,
			ShortDescription = b.ShortDescription,
			ImageUrl = b.ImageUrl,
			BundlePrice = b.BundlePrice,
			CompareAtPrice = b.CompareAtPrice,
			TotalItemsPrice = b.TotalItemsPrice,
			IsActive = b.IsActive,
			IsFeatured = b.IsFeatured,
			StartsAtUtc = b.StartsAtUtc,
			ExpiresAtUtc = b.ExpiresAtUtc,
			MetaTitle = b.MetaTitle,
			MetaDescription = b.MetaDescription,
			CanonicalUrl = b.CanonicalUrl,
			OgImage = b.OgImage,
			Items = b.Items.Select(i => new BundleItemViewModel
			{
				Id = i.Id,
				ProductId = i.ProductId,
				ProductName = i.ProductName,
				ProductVariantId = i.ProductVariantId,
				VariantName = i.VariantName,
				Quantity = i.Quantity,
				DisplayOrder = i.DisplayOrder
			}).ToList(),
			FormData = fd.IsError ? new() : fd.Value
		});
	}

	[HasPermission(Permissions.Bundles.Edit)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	[RequestSizeLimit(20 * 1024 * 1024)]
	public async Task<IActionResult> Edit(
		long id, BundleFormViewModel model, IFormFile? imageFile, CancellationToken ct)
	{
		if (imageFile is not null && imageFile.Length > 0)
		{
			await using var s = imageFile.OpenReadStream();
			var r = await _fileStorage.UploadAsync(s, imageFile.FileName, imageFile.ContentType, "bundles", ct);
			if (r.Success) model.ImageUrl = r.Url!;
		}

		if (!ModelState.IsValid)
		{
			await ReloadFormDataAsync(model, ct);
			return View(model);
		}

		var result = await Mediator.Send(new UpdateBundleCommand(
			id, model.Name, model.Slug, model.Description, model.ShortDescription,
			model.ImageUrl, model.BundlePrice, model.CompareAtPrice, model.TotalItemsPrice,
			model.IsActive, model.IsFeatured, model.StartsAtUtc, model.ExpiresAtUtc,
			model.MetaTitle, model.MetaDescription, model.CanonicalUrl, model.OgImage,
			model.Items.Select(i => new BundleItemInput(i.ProductId, i.ProductVariantId, i.Quantity, i.DisplayOrder)).ToList()
		), ct);

		if (result.IsError)
		{
			AddErrorsToModelState(result.Errors);
			model.Id = id;
			await ReloadFormDataAsync(model, ct);
			return View(model);
		}

		TempData["SuccessMessage"] = "بسته ویرایش شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Bundles.Delete)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(long id, CancellationToken ct)
	{
		var r = await Mediator.Send(new DeleteBundleCommand(id), ct);
		TempData[r.IsError ? "ErrorMessage" : "SuccessMessage"] =
			r.IsError ? r.FirstError.Description : "بسته حذف شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Bundles.Edit)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ToggleActive(long id, CancellationToken ct)
	{
		var r = await Mediator.Send(new ToggleBundleActiveCommand(id), ct);
		if (r.IsError) return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true, isActive = r.Value });
	}

	private async Task ReloadFormDataAsync(BundleFormViewModel model, CancellationToken ct)
	{
		var fd = await Mediator.Send(new GetBundleFormDataQuery(), ct);
		if (!fd.IsError) model.FormData = fd.Value;
	}
}