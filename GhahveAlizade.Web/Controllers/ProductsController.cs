using ErrorOr;
using GhahveAlizade.Application.Features.Catalog.Brands.Queries.GetBrandsList;
using GhahveAlizade.Application.Features.Catalog.Categories.Queries.GetCategoriesList;
using GhahveAlizade.Application.Features.Catalog.Products.Queries.GetProductBySlug;
using GhahveAlizade.Application.Features.Catalog.Products.Queries.GetProductsList;
using GhahveAlizade.Web.Controllers;
using GhahveAlizade.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GhahveAlizade.Web.Controllers;

[Route("products")]
public class ProductsController : MvcBaseController
{
	public ProductsController(IMediator mediator) : base(mediator) { }

	// GET /products
	[HttpGet("")]
	public async Task<IActionResult> Index(
		[FromQuery] ProductFilterViewModel filter,
		CancellationToken cancellationToken)
	{
		if (filter.PageNumber < 1) filter.PageNumber = 1;
		if (filter.PageSize is < 1 or > 48) filter.PageSize = 12;

		var result = await Mediator.Send(new GetProductsListQuery(
			PageNumber: filter.PageNumber,
			PageSize: filter.PageSize.Value,
			Search: filter.Search,
			CategoryId: filter.CategoryId,
			BrandId: filter.BrandId,
			MinPrice: filter.MinPrice,
			MaxPrice: filter.MaxPrice,
			IsActive: true,
			IsPublished: true,
			SortBy: filter.SortBy ?? "createdAt",
			SortDir: filter.SortDir ?? "desc"), cancellationToken);

		if (result.IsError)
			return HandleError(result.FirstError);

		ViewBag.Categories = await GetCategoryOptions(cancellationToken);
		ViewBag.Brands = await GetBrandOptions(cancellationToken);

		ViewData["Title"] = string.IsNullOrWhiteSpace(filter.Search)
			? "فروشگاه قهوه"
			: $"جستجو: {filter.Search}";

		return View(new ProductListViewModel
		{
			Filter = filter,
			Products = result.Value
		});
	}

	// GET /products/{slug}
	[HttpGet("{slug}")]
	public async Task<IActionResult> Details(string slug, CancellationToken cancellationToken)
	{
		var result = await Mediator.Send(new GetProductBySlugQuery(slug), cancellationToken);

		if (result.IsError)
			return NotFound();

		var product = result.Value;
		ViewData["Title"] = product.MetaTitle ?? product.Name;
		ViewData["SeoData"] = product;

		return View(product);
	}

	private async Task<List<CategoryOption>> GetCategoryOptions(CancellationToken ct)
	{
		var result = await Mediator.Send(
			new GetCategoriesListQuery(PageNumber: 1, PageSize: 100, OnlyActive: true), ct);
		return result.IsError
			? new List<CategoryOption>()
			: result.Value.Items
				.Select(c => new CategoryOption(c.Id, c.Name))
				.ToList();
	}

	private async Task<List<CategoryOption>> GetBrandOptions(CancellationToken ct)
	{
		var result = await Mediator.Send(
			new GetBrandsListQuery(PageNumber: 1, PageSize: 100, IsActive: true), ct);
		return result.IsError
			? new List<CategoryOption>()
			: result.Value.Items
				.Select(b => new CategoryOption(b.Id, b.Name))
				.ToList();
	}
}