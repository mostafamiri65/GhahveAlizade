using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Catalog.Products.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Products.Queries.GetProductsList;

public class GetProductsListQueryHandler
	: IRequestHandler<GetProductsListQuery, ErrorOr<PaginatedList<ProductListItemDto>>>
{
	private readonly IAppDbContext _context;

	public GetProductsListQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<PaginatedList<ProductListItemDto>>> Handle(
		GetProductsListQuery request,
		CancellationToken cancellationToken)
	{
		var query = _context.Products
			.AsNoTracking()
			.Where(p => !p.IsDeleted);

		// ============ فیلترها ============

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var search = request.Search.Trim();
			query = query.Where(p =>
				p.Name.Contains(search) ||
				p.Slug.Contains(search) ||
				(p.Sku != null && p.Sku.Contains(search)) ||
				(p.Barcode != null && p.Barcode.Contains(search)));
		}

		if (request.CategoryId.HasValue)
			query = query.Where(p => p.CategoryId == request.CategoryId.Value);

		if (request.BrandId.HasValue)
			query = query.Where(p => p.BrandId == request.BrandId.Value);

		if (request.CoffeeOriginId.HasValue)
			query = query.Where(p => p.CoffeeOriginId == request.CoffeeOriginId.Value);

		if (request.RoastProfileId.HasValue)
			query = query.Where(p => p.RoastProfileId == request.RoastProfileId.Value);

		if (request.CoffeeProcessId.HasValue)
			query = query.Where(p => p.CoffeeProcessId == request.CoffeeProcessId.Value);

		if (request.IsActive.HasValue)
			query = query.Where(p => p.IsActive == request.IsActive.Value);

		if (request.IsPublished.HasValue)
			query = query.Where(p => p.IsPublished == request.IsPublished.Value);

		if (request.IsFeatured.HasValue)
			query = query.Where(p => p.IsFeatured == request.IsFeatured.Value);

		if (request.HasVariants.HasValue)
			query = query.Where(p => p.HasVariants == request.HasVariants.Value);

		// فیلتر قیمت
		if (request.MinPrice.HasValue)
			query = query.Where(p =>
				(p.BasePrice.HasValue && p.BasePrice >= request.MinPrice.Value) ||
				p.Variants.Any(v => !v.IsDeleted && v.Price >= request.MinPrice.Value));

		if (request.MaxPrice.HasValue)
			query = query.Where(p =>
				(p.BasePrice.HasValue && p.BasePrice <= request.MaxPrice.Value) ||
				p.Variants.Any(v => !v.IsDeleted && v.Price <= request.MaxPrice.Value));

		// ============ مرتب‌سازی ============

		query = (request.SortBy?.ToLower(), request.SortDir?.ToLower()) switch
		{
			("name", "desc") => query.OrderByDescending(p => p.Name),
			("name", _) => query.OrderBy(p => p.Name),
			("price", "desc") => query.OrderByDescending(p => p.BasePrice),
			("price", _) => query.OrderBy(p => p.BasePrice),
			("soldcount", "desc") => query.OrderByDescending(p => p.SoldCount),
			("soldcount", _) => query.OrderBy(p => p.SoldCount),
			("viewcount", "desc") => query.OrderByDescending(p => p.ViewCount),
			("viewcount", _) => query.OrderBy(p => p.ViewCount),
			("createdat", "asc") => query.OrderBy(p => p.CreatedAtUtc),
			("createdat", _) => query.OrderByDescending(p => p.CreatedAtUtc),
			("updatedat", "asc") => query.OrderBy(p => p.UpdatedAtUtc),
			("updatedat", _) => query.OrderByDescending(p => p.UpdatedAtUtc),
			_ => query.OrderByDescending(p => p.CreatedAtUtc)
		};

		// ============ Projection با Mapster ============

		var projectedQuery = query.ProjectToType<ProductListItemDto>();

		return await PaginatedList<ProductListItemDto>.CreateAsync(
			projectedQuery, request.PageNumber, request.PageSize, cancellationToken);
	}
}