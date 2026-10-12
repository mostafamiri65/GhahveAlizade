using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Content.Commands.SaveBlogPost;
using GhahveAlizade.Application.Features.Content.Queries.GetBlogPostById;
using GhahveAlizade.Application.Features.Content.Queries.GetBlogPostFormData;
using GhahveAlizade.Application.Features.Content.Queries.GetBlogPostsList;
using GhahveAlizade.Infrastructure.Identity.Authorization;
using GhahveAlizade.Infrastructure.Persistence.Seed;
using GhahveAlizade.Web.Areas.Admin.Models.Content;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Areas.Admin.Controllers;

public class BlogPostsController : AdminBaseController
{
	private readonly IFileStorageService _fileStorage;

	public BlogPostsController(IFileStorageService fileStorage) => _fileStorage = fileStorage;

	[HasPermission(Permissions.Blog.ViewPosts)]
	public async Task<IActionResult> Index(
		int pageNumber = 1, int pageSize = 20, string? search = null,
		long? categoryId = null, bool? isPublished = null, bool? isFeatured = null,
		string? sortBy = "createdAt", string? sortDir = "desc",
		CancellationToken ct = default)
	{
		ViewData["Title"] = "مقالات";
		ViewData["PageIcon"] = "bi-file-text";

		var result = await Mediator.Send(new GetBlogPostsListQuery(
			pageNumber, pageSize, search, categoryId, isPublished, isFeatured, sortBy, sortDir), ct);

		if (result.IsError) return Problem(result.Errors);

		var fd = await Mediator.Send(new GetBlogPostFormDataQuery(), ct);
		if (!fd.IsError) ViewBag.Categories = fd.Value.Categories;

		ViewBag.Search = search;
		ViewBag.CategoryId = categoryId;
		ViewBag.IsPublished = isPublished;
		ViewBag.IsFeatured = isFeatured;
		return View(result.Value);
	}

	[HasPermission(Permissions.Blog.CreatePost)]
	[HttpGet]
	public async Task<IActionResult> Create(CancellationToken ct)
	{
		ViewData["Title"] = "افزودن مقاله";
		ViewData["PageIcon"] = "bi-plus-circle";

		var fd = await Mediator.Send(new GetBlogPostFormDataQuery(), ct);

		return View(new BlogPostFormViewModel
		{
			IsPublished = false,
			AllowComments = true,
			FormData = fd.IsError ? new() : fd.Value
		});
	}

	[HasPermission(Permissions.Blog.CreatePost)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	[RequestSizeLimit(20 * 1024 * 1024)]
	public async Task<IActionResult> Create(
		BlogPostFormViewModel model,
		IFormFile? featuredImageFile,
		CancellationToken ct)
	{
		if (featuredImageFile is not null && featuredImageFile.Length > 0)
		{
			await using var s = featuredImageFile.OpenReadStream();
			var r = await _fileStorage.UploadAsync(s, featuredImageFile.FileName, featuredImageFile.ContentType, "blog", ct);
			if (r.Success) model.FeaturedImageUrl = r.Url!;
		}

		if (!ModelState.IsValid)
		{
			await ReloadFormDataAsync(model, ct);
			return View(model);
		}

		var result = await Mediator.Send(new CreateBlogPostCommand(
			model.Title, model.Slug, model.Summary, model.Content,
			model.FeaturedImageUrl, model.FeaturedImageAlt,
			model.CategoryId, model.AuthorName,
			model.IsPublished, model.IsFeatured, model.AllowComments,
			model.MetaTitle, model.MetaDescription, model.MetaKeywords,
			model.CanonicalUrl, model.OgImage,
			model.TagIds), ct);

		if (result.IsError)
		{
			AddErrorsToModelState(result.Errors);
			await ReloadFormDataAsync(model, ct);
			return View(model);
		}

		TempData["SuccessMessage"] = "مقاله ایجاد شد.";
		return RedirectToAction(nameof(Edit), new { id = result.Value });
	}

	[HasPermission(Permissions.Blog.EditPost)]
	[HttpGet]
	public async Task<IActionResult> Edit(long id, CancellationToken ct)
	{
		ViewData["Title"] = "ویرایش مقاله";
		ViewData["PageIcon"] = "bi-pencil";

		var result = await Mediator.Send(new GetBlogPostByIdQuery(id), ct);
		if (result.IsError)
		{
			TempData["ErrorMessage"] = result.FirstError.Description;
			return RedirectToAction(nameof(Index));
		}

		var fd = await Mediator.Send(new GetBlogPostFormDataQuery(), ct);
		var p = result.Value;

		return View(new BlogPostFormViewModel
		{
			Id = p.Id,
			Title = p.Title,
			Slug = p.Slug,
			Summary = p.Summary,
			Content = p.Content,
			FeaturedImageUrl = p.FeaturedImageUrl,
			FeaturedImageAlt = p.FeaturedImageAlt,
			CategoryId = p.CategoryId,
			AuthorName = p.AuthorName,
			IsPublished = p.IsPublished,
			IsFeatured = p.IsFeatured,
			AllowComments = p.AllowComments,
			MetaTitle = p.MetaTitle,
			MetaDescription = p.MetaDescription,
			MetaKeywords = p.MetaKeywords,
			CanonicalUrl = p.CanonicalUrl,
			OgImage = p.OgImage,
			TagIds = p.TagIds,
			FormData = fd.IsError ? new() : fd.Value
		});
	}

	[HasPermission(Permissions.Blog.EditPost)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	[RequestSizeLimit(20 * 1024 * 1024)]
	public async Task<IActionResult> Edit(
		long id,
		BlogPostFormViewModel model,
		IFormFile? featuredImageFile,
		CancellationToken ct)
	{
		if (featuredImageFile is not null && featuredImageFile.Length > 0)
		{
			await using var s = featuredImageFile.OpenReadStream();
			var r = await _fileStorage.UploadAsync(s, featuredImageFile.FileName, featuredImageFile.ContentType, "blog", ct);
			if (r.Success) model.FeaturedImageUrl = r.Url!;
		}

		if (!ModelState.IsValid)
		{
			await ReloadFormDataAsync(model, ct);
			return View(model);
		}

		var result = await Mediator.Send(new UpdateBlogPostCommand(
			id, model.Title, model.Slug, model.Summary, model.Content,
			model.FeaturedImageUrl, model.FeaturedImageAlt,
			model.CategoryId, model.AuthorName,
			model.IsPublished, model.IsFeatured, model.AllowComments,
			model.MetaTitle, model.MetaDescription, model.MetaKeywords,
			model.CanonicalUrl, model.OgImage,
			model.TagIds), ct);

		if (result.IsError)
		{
			AddErrorsToModelState(result.Errors);
			model.Id = id;
			await ReloadFormDataAsync(model, ct);
			return View(model);
		}

		TempData["SuccessMessage"] = "مقاله ویرایش شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Blog.DeletePost)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete(long id, CancellationToken ct)
	{
		var r = await Mediator.Send(new DeleteBlogPostCommand(id), ct);
		TempData[r.IsError ? "ErrorMessage" : "SuccessMessage"] =
			r.IsError ? r.FirstError.Description : "مقاله حذف شد.";
		return RedirectToAction(nameof(Index));
	}

	[HasPermission(Permissions.Blog.PublishPost)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Publish(long id, CancellationToken ct)
	{
		var r = await Mediator.Send(new PublishBlogPostCommand(id), ct);
		if (r.IsError) return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true, isPublished = r.Value });
	}

	[HasPermission(Permissions.Blog.EditPost)]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ToggleFeatured(long id, CancellationToken ct)
	{
		var r = await Mediator.Send(new ToggleBlogPostFeaturedCommand(id), ct);
		if (r.IsError) return Json(new { success = false, message = r.FirstError.Description });
		return Json(new { success = true, isFeatured = r.Value });
	}

	private async Task ReloadFormDataAsync(BlogPostFormViewModel model, CancellationToken ct)
	{
		var fd = await Mediator.Send(new GetBlogPostFormDataQuery(), ct);
		if (!fd.IsError) model.FormData = fd.Value;
	}
}