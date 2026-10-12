using GhahveAlizade.Application.Features.Content.Commands.SaveBlogTag;
using GhahveAlizade.Application.Features.Content.Queries.GetBlogTagById;
using GhahveAlizade.Application.Features.Content.Queries.GetBlogTagsList;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using GhahveAlizade.Web.Areas.Admin.Models.Content;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class BlogTagsController : AdminBaseController
{
	[HasPermission(Permissions.Blog.ManageTags)]
	public async Task<IActionResult> Index(
		int pageNumber = 1, int pageSize = 20, string? search = null,
		string? sortBy = "name", string? sortDir = "asc",
		CancellationToken ct = default)
	{
		ViewData["Title"] = "برچسب‌های وبلاگ";
		ViewData["PageIcon"] = "bi-tag";

		var r = await Mediator.Send(new GetBlogTagsListQuery(
			pageNumber, pageSize, search, sortBy, sortDir), ct);

		if (r.IsError) return Problem(r.Errors);
		ViewBag.Search = search;
		return View(r.Value);
	}

	[HasPermission(Permissions.Blog.ManageTags)]
	[HttpGet]
	public IActionResult Create()
	{
		ViewData["Title"] = "افزودن برچسب";
		ViewData["PageIcon"] = "bi-plus-circle";
		return View(new BlogTagFormViewModel());
	}

	[HasPermission(Permissions.Blog.ManageTags)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(BlogTagFormViewModel model, CancellationToken ct)
	{
		if (!ModelState.IsValid) return View(model);

		var r = await Mediator.Send(new CreateBlogTagCommand(
			model.Name, model.Slug, model.Description), ct);

		if (r.IsError) { AddErrorsToModelState(r.Errors); return View(model); }

		TempData["SuccessMessage"] = "برچسب ایجاد شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Blog.ManageTags)]
	[HttpGet]
	public async Task<IActionResult> Edit(long id, CancellationToken ct)
	{
		ViewData["Title"] = "ویرایش برچسب";
		ViewData["PageIcon"] = "bi-pencil";

		var r = await Mediator.Send(new GetBlogTagByIdQuery(id), ct);
		if (r.IsError) { TempData["ErrorMessage"] = r.FirstError.Description; return RedirectToAction(nameof(Index)); }

		var t = r.Value;
		return View(new BlogTagFormViewModel { Id = t.Id, Name = t.Name, Slug = t.Slug, Description = t.Description });
	}

	[HasPermission(Permissions.Blog.ManageTags)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(long id, BlogTagFormViewModel model, CancellationToken ct)
	{
		if (!ModelState.IsValid) return View(model);

		var r = await Mediator.Send(new UpdateBlogTagCommand(id, model.Name, model.Slug, model.Description), ct);
		if (r.IsError) { AddErrorsToModelState(r.Errors); model.Id = id; return View(model); }

		TempData["SuccessMessage"] = "برچسب ویرایش شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Blog.ManageTags)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(long id, CancellationToken ct)
	{
		var r = await Mediator.Send(new DeleteBlogTagCommand(id), ct);
		TempData[r.IsError ? "ErrorMessage" : "SuccessMessage"] =
			r.IsError ? r.FirstError.Description : "برچسب حذف شد.";
		return RedirectToAction(nameof(Index));
	}
}