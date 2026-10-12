using ErrorOr;
using GhahveAlizade.Application.Features.Catalog.Categories.Commands.CreateCategory;
using GhahveAlizade.Application.Features.Catalog.Categories.Commands.DeleteCategory;
using GhahveAlizade.Application.Features.Catalog.Categories.Commands.ToggleCategoryActive;
using GhahveAlizade.Application.Features.Catalog.Categories.Commands.UpdateCategory;
using GhahveAlizade.Application.Features.Catalog.Categories.Queries.GetCategoriesList;
using GhahveAlizade.Application.Features.Catalog.Categories.Queries.GetCategoriesTree;
using GhahveAlizade.Application.Features.Catalog.Categories.Queries.GetCategoryById;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using Microsoft.AspNetCore.Mvc;
using GhahveAlizade.Web.Areas.Admin.Models.Categories;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class CategoriesController : AdminBaseController
{
	// ================ لیست ================

	[HasPermission(Permissions.Categories.View)]
	public async Task<IActionResult> Index(
		int pageNumber = 1,
		int pageSize = 20,
		string? search = null,
		long? parentId = null,
		bool? isActive = null,
		string? sortBy = "displayOrder",
		string? sortDir = "asc",
		CancellationToken cancellationToken = default)
	{
		ViewData["Title"] = "مدیریت دسته‌بندی‌ها";
		ViewData["PageIcon"] = "bi-diagram-3";

		var query = new GetCategoriesListQuery(
			pageNumber, pageSize, search, parentId, isActive, sortBy, sortDir);

		var result = await Mediator.Send(query, cancellationToken);

		if (result.IsError)
			return Problem(result.Errors);

		ViewBag.Search = search;
		ViewBag.IsActive = isActive;
		ViewBag.SortBy = sortBy;
		ViewBag.SortDir = sortDir;

		return View(result.Value);
	}

	// ================ ایجاد ================

	[HasPermission(Permissions.Categories.Create)]
	[HttpGet]
	public async Task<IActionResult> Create(CancellationToken cancellationToken)
	{
		ViewData["Title"] = "افزودن دسته‌بندی";
		ViewData["PageIcon"] = "bi-plus-circle";

		var treeResult = await Mediator.Send(
			new GetCategoriesTreeQuery(OnlyActive: false), cancellationToken);

		var model = new CategoryFormViewModel
		{
			Categories = treeResult.IsError ? new() : treeResult.Value,
			IsActive = true
		};

		return View(model);
	}

	[HasPermission(Permissions.Categories.Create)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(
		CategoryFormViewModel model,
		CancellationToken cancellationToken)
	{
		if (!ModelState.IsValid)
			return await ReturnCreateWithTreeAsync(model, cancellationToken);

		var command = new CreateCategoryCommand(
			Name: model.Name,
			Slug: model.Slug,
			Description: model.Description,
			ImageUrl: model.ImageUrl,
			IconUrl: model.IconUrl,
			ParentId: model.ParentId,
			DisplayOrder: model.DisplayOrder,
			IsActive: model.IsActive,
			IsFeatured: model.IsFeatured,
			MetaTitle: model.MetaTitle,
			MetaDescription: model.MetaDescription,
			MetaKeywords: model.MetaKeywords);

		var result = await Mediator.Send(command, cancellationToken);

		if (result.IsError)
		{
			AddErrorsToModelState(result.Errors);
			return await ReturnCreateWithTreeAsync(model, cancellationToken);
		}

		TempData["SuccessMessage"] = "دسته‌بندی با موفقیت ایجاد شد.";
		return RedirectToAction(nameof(Index));
	}

	private async Task<IActionResult> ReturnCreateWithTreeAsync(
		CategoryFormViewModel model, CancellationToken cancellationToken)
	{
		ViewData["Title"] = "افزودن دسته‌بندی";
		ViewData["PageIcon"] = "bi-plus-circle";

		var treeResult = await Mediator.Send(
			new GetCategoriesTreeQuery(OnlyActive: false), cancellationToken);

		model.Categories = treeResult.IsError ? new() : treeResult.Value;
		return View(model);
	}

	// ================ ویرایش ================

	[HasPermission(Permissions.Categories.Edit)]
	[HttpGet]
	public async Task<IActionResult> Edit(long id, CancellationToken cancellationToken)
	{
		ViewData["Title"] = "ویرایش دسته‌بندی";
		ViewData["PageIcon"] = "bi-pencil";

		var result = await Mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);

		if (result.IsError)
		{
			TempData["ErrorMessage"] = result.FirstError.Description;
			return RedirectToAction(nameof(Index));
		}

		var treeResult = await Mediator.Send(
			new GetCategoriesTreeQuery(OnlyActive: false, ExcludeId: id), cancellationToken);

		var model = new CategoryFormViewModel
		{
			Id = result.Value.Id,
			Name = result.Value.Name,
			Slug = result.Value.Slug,
			Description = result.Value.Description,
			ImageUrl = result.Value.ImageUrl,
			IconUrl = result.Value.IconUrl,
			ParentId = result.Value.ParentId,
			DisplayOrder = result.Value.DisplayOrder,
			IsActive = result.Value.IsActive,
			IsFeatured = result.Value.IsFeatured,
			MetaTitle = result.Value.MetaTitle,
			MetaDescription = result.Value.MetaDescription,
			MetaKeywords = result.Value.MetaKeywords,
			Categories = treeResult.IsError ? new() : treeResult.Value
		};

		return View(model);
	}

	[HasPermission(Permissions.Categories.Edit)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(
		long id,
		CategoryFormViewModel model,
		CancellationToken cancellationToken)
	{
		if (!ModelState.IsValid)
			return await ReturnEditWithTreeAsync(id, model, cancellationToken);

		var command = new UpdateCategoryCommand(
			Id: id,
			Name: model.Name,
			Slug: model.Slug,
			Description: model.Description,
			ImageUrl: model.ImageUrl,
			IconUrl: model.IconUrl,
			ParentId: model.ParentId,
			DisplayOrder: model.DisplayOrder,
			IsActive: model.IsActive,
			IsFeatured: model.IsFeatured,
			MetaTitle: model.MetaTitle,
			MetaDescription: model.MetaDescription,
			MetaKeywords: model.MetaKeywords);

		var result = await Mediator.Send(command, cancellationToken);

		if (result.IsError)
		{
			AddErrorsToModelState(result.Errors);
			return await ReturnEditWithTreeAsync(id, model, cancellationToken);
		}

		TempData["SuccessMessage"] = "دسته‌بندی با موفقیت ویرایش شد.";
		return RedirectToAction(nameof(Index));
	}

	private async Task<IActionResult> ReturnEditWithTreeAsync(
		long id, CategoryFormViewModel model, CancellationToken cancellationToken)
	{
		ViewData["Title"] = "ویرایش دسته‌بندی";
		ViewData["PageIcon"] = "bi-pencil";

		var treeResult = await Mediator.Send(
			new GetCategoriesTreeQuery(OnlyActive: false, ExcludeId: id), cancellationToken);

		model.Id = id;
		model.Categories = treeResult.IsError ? new() : treeResult.Value;
		return View(model);
	}

	// ================ حذف ================

	[HasPermission(Permissions.Categories.Delete)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
	{
		var result = await Mediator.Send(new DeleteCategoryCommand(id), cancellationToken);

		if (result.IsError)
		{
			TempData["ErrorMessage"] = result.FirstError.Description;
		}
		else
		{
			TempData["SuccessMessage"] = "دسته‌بندی با موفقیت حذف شد.";
		}

		return RedirectToAction(nameof(Index));
	}

	// ================ تغییر وضعیت ================

	[HasPermission(Permissions.Categories.Edit)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ToggleActive(long id, CancellationToken cancellationToken)
	{
		var result = await Mediator.Send(
			new ToggleCategoryActiveCommand(id), cancellationToken);

		if (result.IsError)
		{
			return Json(new { success = false, message = result.FirstError.Description });
		}

		return Json(new
		{
			success = true,
			isActive = result.Value,
			message = result.Value ? "فعال شد" : "غیرفعال شد"
		});
	}
}