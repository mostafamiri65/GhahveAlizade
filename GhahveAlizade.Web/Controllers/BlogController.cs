using ErrorOr;
using GhahveAlizade.Application.Features.Content.Dtos;
using GhahveAlizade.Application.Features.Content.Queries.GetBlogCategoriesList;
using GhahveAlizade.Application.Features.Content.Queries.GetBlogPostBySlug;
using GhahveAlizade.Application.Features.Content.Queries.GetBlogPostsList;
using GhahveAlizade.Web.Controllers;
using GhahveAlizade.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Controllers;

[Route("blog")]
public class BlogController : MvcBaseController
{
	public BlogController() { }

	// GET /blog
	[HttpGet("")]
	public async Task<IActionResult> Index(
		[FromQuery] BlogFilterViewModel filter,
		CancellationToken cancellationToken)
	{
		if (filter.PageNumber < 1) filter.PageNumber = 1;
		if (filter.PageSize is < 1 or > 24) filter.PageSize = 9;

		var result = await Mediator.Send(new GetBlogPostsListQuery(
			PageNumber: filter.PageNumber,
			PageSize: filter.PageSize.Value,
			Search: filter.Search,
			CategoryId: filter.CategoryId,
			IsPublished: true,
			SortBy: "publishedAt",
			SortDir: "desc"), cancellationToken);

		if (result.IsError)
			return HandleError(result.FirstError);

		var categories = await Mediator.Send(
			new GetBlogCategoriesListQuery(PageNumber: 1, PageSize: 50), cancellationToken);

		ViewData["Title"] = "وبلاگ قهوه";

		return View(new BlogListViewModel
		{
			Filter = filter,
			Posts = result.Value,
			Categories = categories.IsError
				? new List<CategoryOption>()
				: categories.Value.Items
					.Select(c => new CategoryOption(c.Id, c.Name))
					.ToList()
		});
	}

	// GET /blog/{slug}
	[HttpGet("{slug}")]
	public async Task<IActionResult> Details(string slug, CancellationToken cancellationToken)
	{
		var result = await Mediator.Send(new GetBlogPostBySlugQuery(slug), cancellationToken);

		if (result.IsError)
			return NotFound();

		var post = result.Value;
		ViewData["Title"] = post.MetaTitle ?? post.Title;
		ViewData["SeoData"] = post;

		return View(post);
	}
}