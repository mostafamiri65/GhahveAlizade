using GhahveAlizade.Application.Features.Catalog.Products.Commands.CreateProduct;
using GhahveAlizade.Application.Features.Catalog.Products.Commands.DeleteProduct;
using GhahveAlizade.Application.Features.Catalog.Products.Commands.PublishProduct;
using GhahveAlizade.Application.Features.Catalog.Products.Commands.ToggleProductActive;
using GhahveAlizade.Application.Features.Catalog.Products.Commands.UpdateProduct;
using GhahveAlizade.Application.Features.Catalog.Products.Queries.GetProductById;
using GhahveAlizade.Application.Features.Catalog.Products.Queries.GetProductFormData;
using GhahveAlizade.Application.Features.Catalog.Products.Queries.GetProductsList;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using GhahveAlizade.Web.Areas.Admin.Models.Products;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class ProductsController : AdminBaseController
{
	private readonly IFileStorageService _fileStorage;

	public ProductsController(IFileStorageService fileStorage)
		=> _fileStorage = fileStorage;

	// ================ لیست ================

	[HasPermission(Permissions.Products.View)]
	public async Task<IActionResult> Index(
		int pageNumber = 1,
		int pageSize = 20,
		string? search = null,
		long? categoryId = null,
		long? brandId = null,
		long? coffeeOriginId = null,
		long? roastProfileId = null,
		bool? isActive = null,
		bool? isPublished = null,
		bool? isFeatured = null,
		string? sortBy = "createdAt",
		string? sortDir = "desc",
		CancellationToken cancellationToken = default)
	{
		ViewData["Title"] = "مدیریت محصولات";
		ViewData["PageIcon"] = "bi-box-seam";

		var query = new GetProductsListQuery(
			pageNumber, pageSize, search,
			categoryId, brandId, coffeeOriginId, roastProfileId,
			null,isActive, isPublished, isFeatured,
			SortBy: sortBy, SortDir: sortDir);

		var result = await Mediator.Send(query, cancellationToken);

		if (result.IsError)
			return Problem(result.Errors);

		// برای فیلتر Dropdownها
		var formData = await Mediator.Send(new GetProductFormDataQuery(), cancellationToken);
		if (!formData.IsError)
		{
			ViewBag.Categories = formData.Value.Categories;
			ViewBag.Brands = formData.Value.Brands;
			ViewBag.CoffeeOrigins = formData.Value.CoffeeOrigins;
			ViewBag.RoastProfiles = formData.Value.RoastProfiles;
		}

		ViewBag.Search = search;
		ViewBag.CategoryId = categoryId;
		ViewBag.BrandId = brandId;
		ViewBag.CoffeeOriginId = coffeeOriginId;
		ViewBag.RoastProfileId = roastProfileId;
		ViewBag.IsActive = isActive;
		ViewBag.IsPublished = isPublished;
		ViewBag.IsFeatured = isFeatured;
		ViewBag.SortBy = sortBy;
		ViewBag.SortDir = sortDir;

		return View(result.Value);
	}

	// ================ جزئیات ================

	[HasPermission(Permissions.Products.View)]
	public async Task<IActionResult> Detail(long id, CancellationToken cancellationToken)
	{
		ViewData["Title"] = "جزئیات محصول";
		ViewData["PageIcon"] = "bi-eye";

		var result = await Mediator.Send(new GetProductByIdQuery(id), cancellationToken);

		if (result.IsError)
		{
			TempData["ErrorMessage"] = result.FirstError.Description;
			return RedirectToAction(nameof(Index));
		}

		return View(result.Value);
	}

	// ================ ایجاد ================

	[HasPermission(Permissions.Products.Create)]
	[HttpGet]
	public async Task<IActionResult> Create(CancellationToken cancellationToken)
	{
		ViewData["Title"] = "افزودن محصول";
		ViewData["PageIcon"] = "bi-plus-circle";

		var formDataResult = await Mediator.Send(new GetProductFormDataQuery(), cancellationToken);

		var model = new ProductFormViewModel();
		if (!formDataResult.IsError)
			model.FormData = formDataResult.Value;

		return View(model);
	}

	[HasPermission(Permissions.Products.Create)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	[RequestSizeLimit(50 * 1024 * 1024)] // 50 MB
	public async Task<IActionResult> Create(
		ProductFormViewModel model,
		CancellationToken cancellationToken)
	{
		// ============ آپلود تصاویر ============
		if (model.NewImages?.Any() == true)
		{
			foreach (var file in model.NewImages)
			{
				if (file.Length == 0) continue;

				await using var stream = file.OpenReadStream();
				var uploadResult = await _fileStorage.UploadAsync(
					stream, file.FileName, file.ContentType, "products", cancellationToken);

				if (uploadResult.Success && uploadResult.Url is not null)
				{
					model.Images.Add(new ProductImageViewModel
					{
						ImageUrl = uploadResult.Url,
						AltText = model.Name,
						DisplayOrder = model.Images.Count,
						IsPrimary = !model.Images.Any(),
						FileSizeBytes = uploadResult.SizeBytes
					});
				}
			}
		}

		// ============ ساخت Command ============
		var command = new CreateProductCommand(
			Name: model.Name,
			Slug: model.Slug,
			ShortDescription: model.ShortDescription,
			Description: model.Description,
			Sku: model.Sku,
			Barcode: model.Barcode,
			HasVariants: model.HasVariants,
			BasePrice: model.BasePrice,
			CompareAtPrice: model.CompareAtPrice,
			CostPrice: model.CostPrice,
			IsTaxable: model.IsTaxable,
			TaxRate: model.TaxRate,
			Weight: model.Weight,
			Length: model.Length,
			Width: model.Width,
			Height: model.Height,
			CategoryId: model.CategoryId,
			BrandId: model.BrandId,
			CoffeeOriginId: model.CoffeeOriginId,
			CoffeeProcessId: model.CoffeeProcessId,
			RoastProfileId: model.RoastProfileId,
			FlavorNoteIds: model.FlavorNoteIds,
			IsActive: model.IsActive,
			IsPublished: model.IsPublished,
			IsFeatured: model.IsFeatured,
			IsNew: model.IsNew,
			IsBestSeller: model.IsBestSeller,
			MetaTitle: model.MetaTitle,
			MetaDescription: model.MetaDescription,
			MetaKeywords: model.MetaKeywords,
			CanonicalUrl: model.CanonicalUrl,
			OgImage: model.OgImage,
			Variants: model.Variants.Select(v => new ProductVariantInput(
				v.Name, v.Sku, v.Barcode, v.AttributesJson,
				v.Price, v.CompareAtPrice, v.CostPrice, v.Weight,
				v.ImageUrl, v.DisplayOrder, v.IsActive, v.IsDefault)).ToList(),
			Images: model.Images.Select(i => new ProductImageInput(
				i.ImageUrl, i.ThumbnailUrl, i.AltText, i.Title,
				i.DisplayOrder, i.IsPrimary, i.FileSizeBytes, i.Width, i.Height)).ToList(),
			Attributes: model.Attributes.Select(a => new ProductAttributeInput(
				a.AttributeId, a.AttributeValueId, a.CustomValue, a.DisplayOrder)).ToList());

		var result = await Mediator.Send(command, cancellationToken);

		if (result.IsError)
		{
			AddErrorsToModelState(result.Errors);
			await ReloadFormDataAsync(model, cancellationToken);
			return View(model);
		}

		TempData["SuccessMessage"] = "محصول با موفقیت ایجاد شد.";
		return RedirectToAction(nameof(Edit), new { id = result.Value });
	}

	// ================ ویرایش ================

	[HasPermission(Permissions.Products.Edit)]
	[HttpGet]
	public async Task<IActionResult> Edit(long id, CancellationToken cancellationToken)
	{
		ViewData["Title"] = "ویرایش محصول";
		ViewData["PageIcon"] = "bi-pencil";

		var productResult = await Mediator.Send(new GetProductByIdQuery(id), cancellationToken);
		if (productResult.IsError)
		{
			TempData["ErrorMessage"] = productResult.FirstError.Description;
			return RedirectToAction(nameof(Index));
		}

		var formDataResult = await Mediator.Send(new GetProductFormDataQuery(), cancellationToken);
		var p = productResult.Value;

		var model = new ProductFormViewModel
		{
			Id = p.Id,
			Name = p.Name,
			Slug = p.Slug,
			ShortDescription = p.ShortDescription,
			Description = p.Description,
			Sku = p.Sku,
			Barcode = p.Barcode,
			HasVariants = p.HasVariants,
			BasePrice = p.BasePrice,
			CompareAtPrice = p.CompareAtPrice,
			CostPrice = p.CostPrice,
			IsTaxable = p.IsTaxable,
			TaxRate = p.TaxRate,
			Weight = p.Weight,
			Length = p.Length,
			Width = p.Width,
			Height = p.Height,
			CategoryId = p.CategoryId,
			BrandId = p.BrandId,
			CoffeeOriginId = p.CoffeeOriginId,
			CoffeeProcessId = p.CoffeeProcessId,
			RoastProfileId = p.RoastProfileId,
			FlavorNoteIds = p.FlavorNoteIds,
			IsActive = p.IsActive,
			IsPublished = p.IsPublished,
			IsFeatured = p.IsFeatured,
			IsNew = p.IsNew,
			IsBestSeller = p.IsBestSeller,
			MetaTitle = p.MetaTitle,
			MetaDescription = p.MetaDescription,
			MetaKeywords = p.MetaKeywords,
			CanonicalUrl = p.CanonicalUrl,
			OgImage = p.OgImage,
			Variants = p.Variants.Select(v => new ProductVariantViewModel
			{
				Id = v.Id,
				Name = v.Name,
				Sku = v.Sku,
				Barcode = v.Barcode,
				AttributesJson = v.AttributesJson,
				Price = v.Price,
				CompareAtPrice = v.CompareAtPrice,
				CostPrice = v.CostPrice,
				Weight = v.Weight,
				ImageUrl = v.ImageUrl,
				DisplayOrder = v.DisplayOrder,
				IsActive = v.IsActive,
				IsDefault = v.IsDefault
			}).ToList(),
			Images = p.Images.Select(i => new ProductImageViewModel
			{
				Id = i.Id,
				ImageUrl = i.ImageUrl,
				ThumbnailUrl = i.ThumbnailUrl,
				AltText = i.AltText,
				Title = i.Title,
				DisplayOrder = i.DisplayOrder,
				IsPrimary = i.IsPrimary,
				FileSizeBytes = i.FileSizeBytes,
				Width = i.Width,
				Height = i.Height
			}).ToList(),
			Attributes = p.Attributes.Select(a => new ProductAttributeViewModel
			{
				Id = a.Id,
				AttributeId = a.AttributeId,
				AttributeValueId = a.AttributeValueId,
				CustomValue = a.CustomValue,
				DisplayOrder = a.DisplayOrder,
				AttributeName = a.AttributeName,
				Value = a.Value
			}).ToList(),
			FormData = formDataResult.IsError ? new() : formDataResult.Value
		};

		return View(model);
	}

	[HasPermission(Permissions.Products.Edit)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	[RequestSizeLimit(50 * 1024 * 1024)]
	public async Task<IActionResult> Edit(
		long id,
		ProductFormViewModel model,
		CancellationToken cancellationToken)
	{
		// آپلود تصاویر جدید
		if (model.NewImages?.Any() == true)
		{
			foreach (var file in model.NewImages)
			{
				if (file.Length == 0) continue;

				await using var stream = file.OpenReadStream();
				var uploadResult = await _fileStorage.UploadAsync(
					stream, file.FileName, file.ContentType, "products", cancellationToken);

				if (uploadResult.Success && uploadResult.Url is not null)
				{
					model.Images.Add(new ProductImageViewModel
					{
						ImageUrl = uploadResult.Url,
						AltText = model.Name,
						DisplayOrder = model.Images.Count,
						IsPrimary = !model.Images.Any(),
						FileSizeBytes = uploadResult.SizeBytes
					});
				}
			}
		}

		var command = new UpdateProductCommand(
			Id: id,
			Name: model.Name,
			Slug: model.Slug,
			ShortDescription: model.ShortDescription,
			Description: model.Description,
			Sku: model.Sku,
			Barcode: model.Barcode,
			HasVariants: model.HasVariants,
			BasePrice: model.BasePrice,
			CompareAtPrice: model.CompareAtPrice,
			CostPrice: model.CostPrice,
			IsTaxable: model.IsTaxable,
			TaxRate: model.TaxRate,
			Weight: model.Weight,
			Length: model.Length,
			Width: model.Width,
			Height: model.Height,
			CategoryId: model.CategoryId,
			BrandId: model.BrandId,
			CoffeeOriginId: model.CoffeeOriginId,
			CoffeeProcessId: model.CoffeeProcessId,
			RoastProfileId: model.RoastProfileId,
			FlavorNoteIds: model.FlavorNoteIds,
			IsActive: model.IsActive,
			IsPublished: model.IsPublished,
			IsFeatured: model.IsFeatured,
			IsNew: model.IsNew,
			IsBestSeller: model.IsBestSeller,
			MetaTitle: model.MetaTitle,
			MetaDescription: model.MetaDescription,
			MetaKeywords: model.MetaKeywords,
			CanonicalUrl: model.CanonicalUrl,
			OgImage: model.OgImage,
			Variants: model.Variants.Select(v => new ProductVariantInput(
				v.Name, v.Sku, v.Barcode, v.AttributesJson,
				v.Price, v.CompareAtPrice, v.CostPrice, v.Weight,
				v.ImageUrl, v.DisplayOrder, v.IsActive, v.IsDefault)).ToList(),
			Images: model.Images.Select(i => new ProductImageInput(
				i.ImageUrl, i.ThumbnailUrl, i.AltText, i.Title,
				i.DisplayOrder, i.IsPrimary, i.FileSizeBytes, i.Width, i.Height)).ToList(),
			Attributes: model.Attributes.Select(a => new ProductAttributeInput(
				a.AttributeId, a.AttributeValueId, a.CustomValue, a.DisplayOrder)).ToList());

		var result = await Mediator.Send(command, cancellationToken);

		if (result.IsError)
		{
			AddErrorsToModelState(result.Errors);
			await ReloadFormDataAsync(model, cancellationToken);
			return View(model);
		}

		TempData["SuccessMessage"] = "محصول با موفقیت ویرایش شد.";
		return RedirectToAction(nameof(Index));
	}

	// ================ حذف ================

	[HasPermission(Permissions.Products.Delete)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
	{
		var result = await Mediator.Send(new DeleteProductCommand(id), cancellationToken);

		if (result.IsError)
			TempData["ErrorMessage"] = result.FirstError.Description;
		else
			TempData["SuccessMessage"] = "محصول با موفقیت حذف شد.";

		return RedirectToAction(nameof(Index));
	}

	// ================ تغییر وضعیت‌ها (AJAX) ================

	[HasPermission(Permissions.Products.Edit)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ToggleActive(long id, CancellationToken cancellationToken)
	{
		var result = await Mediator.Send(new ToggleProductActiveCommand(id), cancellationToken);

		if (result.IsError)
			return Json(new { success = false, message = result.FirstError.Description });

		return Json(new
		{
			success = true,
			isActive = result.Value,
			message = result.Value ? "فعال شد" : "غیرفعال شد"
		});
	}

	[HasPermission(Permissions.Products.Publish)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Publish(long id, CancellationToken cancellationToken)
	{
		var result = await Mediator.Send(new PublishProductCommand(id), cancellationToken);

		if (result.IsError)
			return Json(new { success = false, message = result.FirstError.Description });

		return Json(new
		{
			success = true,
			isPublished = result.Value,
			message = result.Value ? "منتشر شد" : "از انتشار خارج شد"
		});
	}

	// ================ Helper ================

	private async Task ReloadFormDataAsync(
		ProductFormViewModel model,
		CancellationToken cancellationToken)
	{
		var formDataResult = await Mediator.Send(
			new GetProductFormDataQuery(), cancellationToken);

		if (!formDataResult.IsError)
			model.FormData = formDataResult.Value;
	}
}