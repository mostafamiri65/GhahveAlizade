using GhahveAlizade.Application.Features.Content.Commands.SaveBlogCategory;
using GhahveAlizade.Application.Features.Content.Queries.GetBlogCategoriesList;
using GhahveAlizade.Application.Features.Content.Queries.GetBlogCategoriesTree;
using GhahveAlizade.Application.Features.Content.Queries.GetBlogCategoryById;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using GhahveAlizade.Web.Areas.Admin.Models.Content;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class BlogCategoriesController : AdminBaseController
{
	[HasPermission(Permissions.Blog.ManageCategories)]
	public async Task<IActionResult> Index(
		int pageNumber = 1, int pageSize = 20, string? search = null,
		long? parentId = null, bool? isActive = null,
		string? sortBy = "displayOrder", string? sortDir = "asc",
		CancellationToken ct = default)
	{
		ViewData["Title"] = "دسته‌های وبلاگ";
		ViewData["PageIcon"] = "bi-folder";

		var result = await Mediator.Send(new GetBlogCategoriesListQuery(
			pageNumber, pageSize, search, parentId, isActive, sortBy, sortDir), ct);

		if (result.IsError) return Problem(result.Errors);

		ViewBag.Search = search;
		ViewBag.IsActive = isActive;
		return View(result.Value);
	}

	[HasPermission(Permissions.Blog.ManageCategories)]
	[HttpGet]
	public async Task<IActionResult> Create(CancellationToken ct)
	{
		ViewData["Title"] = "افزودن دسته وبلاگ";
		ViewData["PageIcon"] = "bi-plus-circle";

		var tree = await Mediator.Send(new GetBlogCategoriesTreeQuery(false), ct);

		return View(new BlogCategoryFormViewModel
		{
			IsActive = true,
			Categories = tree.IsError ? new() : tree.Value
		});
	}

	[HasPermission(Permissions.Blog.ManageCategories)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(BlogCategoryFormViewModel model, CancellationToken ct)
	{
		if (!ModelState.IsValid)
		{
			await ReloadTreeAsync(model, null, ct);
			return View(model);
		}

		var r = await Mediator.Send(new CreateBlogCategoryCommand(
			model.Name, model.Slug, model.Description, model.ImageUrl,
			model.ParentId, model.DisplayOrder, model.IsActive,
			model.MetaTitle, model.MetaDescription, model.CanonicalUrl), ct);

		if (r.IsError)
		{
			AddErrorsToModelState(r.Errors);
			await ReloadTreeAsync(model, null, ct);
			return View(model);
		}

		TempData["SuccessMessage"] = "دسته ایجاد شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Blog.ManageCategories)]
	[HttpGet]
	public async Task<IActionResult> Edit(long id, CancellationToken ct)
	{
		ViewData["Title"] = "ویرایش دسته وبلاگ";
		ViewData["PageIcon"] = "bi-pencil";

		var r = await Mediator.Send(new GetBlogCategoryByIdQuery(id), ct);
		if (r.IsError)
		{
			TempData["ErrorMessage"] = r.FirstError.Description;
			return RedirectToAction(nameof(Index));
		}

		var c = r.Value;
		var tree = await Mediator.Send(new GetBlogCategoriesTreeQuery(false, id), ct);

		return View(new BlogCategoryFormViewModel
		{
			Id = c.Id,
			Name = c.Name,
			Slug = c.Slug,
			Description = c.Description,
			ImageUrl = c.ImageUrl,
			ParentId = c.ParentId,
			DisplayOrder = c.DisplayOrder,
			IsActive = c.IsActive,
			MetaTitle = c.MetaTitle,
			MetaDescription = c.MetaDescription,
			CanonicalUrl = c.CanonicalUrl,
			Categories = tree.IsError ? new() : tree.Value
		});
	}

	[HasPermission(Permissions.Blog.ManageCategories)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(long id, BlogCategoryFormViewModel model, CancellationToken ct)
	{
		if (!ModelState.IsValid)
		{
			await ReloadTreeAsync(model, id, ct);
			return View(model);
		}

		var r = await Mediator.Send(new UpdateBlogCategoryCommand(
			id, model.Name, model.Slug, model.Description, model.ImageUrl,
			model.ParentId, model.DisplayOrder, model.IsActive,
			model.MetaTitle, model.MetaDescription, model.CanonicalUrl), ct);

		if (r.IsError)
		{
			AddErrorsToModelState(r.Errors);
			model.Id = id;
			await ReloadTreeAsync(model, id, ct);
			return View(model);
		}

		TempData["SuccessMessage"] = "دسته ویرایش شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Blog.ManageCategories)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(long id, CancellationToken ct)
	{
		var r = await Mediator.Send(new DeleteBlogCategoryCommand(id), ct);
		TempData[r.IsError ? "ErrorMessage" : "SuccessMessage"] =
			r.IsError ? r.FirstError.Description : "دسته حذف شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Blog.ManageCategories)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ToggleActive(long id, CancellationToken ct)
	{
		var r = await Mediator.Send(new ToggleBlogCategoryActiveCommand(id), ct);
		if (r.IsError) return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true, isActive = r.Value });
	}

	private async Task ReloadTreeAsync(BlogCategoryFormViewModel model, long? excludeId, CancellationToken ct)
	{
		var tree = await Mediator.Send(new GetBlogCategoriesTreeQuery(false, excludeId), ct);
		if (!tree.IsError) model.Categories = tree.Value;
	}
}