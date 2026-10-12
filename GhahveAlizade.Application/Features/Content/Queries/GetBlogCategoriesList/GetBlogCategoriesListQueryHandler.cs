using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Content.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Content.Queries.GetBlogCategoriesList;

public class GetBlogCategoriesListQueryHandler
	: IRequestHandler<GetBlogCategoriesListQuery, ErrorOr<PaginatedList<BlogCategoryListItemDto>>>
{
	private readonly IAppDbContext _context;

	public GetBlogCategoriesListQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<PaginatedList<BlogCategoryListItemDto>>> Handle(
		GetBlogCategoriesListQuery request, CancellationToken ct)
	{
		var query = _context.BlogCategories
			.AsNoTracking()
			.Include(c => c.Parent)
			.Where(c => !c.IsDeleted);

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var s = request.Search.Trim();
			query = query.Where(c => c.Name.Contains(s) || c.Slug.Contains(s));
		}

		if (request.ParentId.HasValue)
			query = query.Where(c => c.ParentId == request.ParentId.Value);

		if (request.IsActive.HasValue)
			query = query.Where(c => c.IsActive == request.IsActive.Value);

		query = (request.SortBy?.ToLower(), request.SortDir?.ToLower()) switch
		{
			("name", "desc") => query.OrderByDescending(c => c.Name),
			("name", _) => query.OrderBy(c => c.Name),
			("createdat", "desc") => query.OrderByDescending(c => c.CreatedAtUtc),
			("createdat", _) => query.OrderBy(c => c.CreatedAtUtc),
			_ => query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name)
		};

		return await PaginatedList<BlogCategoryListItemDto>.CreateAsync(
			query.ProjectToType<BlogCategoryListItemDto>(),
			request.PageNumber, request.PageSize, ct);
	}
}