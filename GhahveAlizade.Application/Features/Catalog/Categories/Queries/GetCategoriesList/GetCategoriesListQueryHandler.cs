using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Catalog.Categories.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Categories.Queries.GetCategoriesList;

public class GetCategoriesListQueryHandler
	: IRequestHandler<GetCategoriesListQuery, ErrorOr<PaginatedList<CategoryListItemDto>>>
{
	private readonly IAppDbContext _context;

	public GetCategoriesListQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<PaginatedList<CategoryListItemDto>>> Handle(
		GetCategoriesListQuery request,
		CancellationToken cancellationToken)
	{
		var query = _context.Categories
			.AsNoTracking()
			.Where(c => !c.IsDeleted);

		// فیلترها
		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var search = request.Search.Trim();
			query = query.Where(c =>
				c.Name.Contains(search) ||
				c.Slug.Contains(search));
		}

		if (request.ParentId.HasValue)
			query = query.Where(c => c.ParentId == request.ParentId.Value);

		if (request.IsActive.HasValue)
			query = query.Where(c => c.IsActive == request.IsActive.Value);

		// مرتب‌سازی
		query = (request.SortBy?.ToLower(), request.SortDir?.ToLower()) switch
		{
			("name", "desc") => query.OrderByDescending(c => c.Name),
			("name", _) => query.OrderBy(c => c.Name),
			("createdat", "desc") => query.OrderByDescending(c => c.CreatedAtUtc),
			("createdat", _) => query.OrderBy(c => c.CreatedAtUtc),
			("displayorder", "desc") => query.OrderByDescending(c => c.DisplayOrder),
			_ => query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name)
		};

		// Projection مستقیم به DTO با شمارش
		var projectedQuery = query.Select(c => new CategoryListItemDto
		{
			Id = c.Id,
			Name = c.Name,
			Slug = c.Slug,
			ImageUrl = c.ImageUrl,
			ParentId = c.ParentId,
			ParentName = c.Parent != null ? c.Parent.Name : null,
			DisplayOrder = c.DisplayOrder,
			IsActive = c.IsActive,
			IsFeatured = c.IsFeatured,
			CreatedAtUtc = c.CreatedAtUtc,
			ChildrenCount = c.Children.Count(ch => !ch.IsDeleted),
			ProductCount = c.Products.Count(p => !p.IsDeleted)
		});

		var result = await PaginatedList<CategoryListItemDto>.CreateAsync(
			projectedQuery,
			request.PageNumber,
			request.PageSize,
			cancellationToken);

		return result;
	}
}