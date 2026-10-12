using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Content.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Content.Queries.GetBlogPostsList;

public class GetBlogPostsListQueryHandler
	: IRequestHandler<GetBlogPostsListQuery, ErrorOr<PaginatedList<BlogPostListItemDto>>>
{
	private readonly IAppDbContext _context;

	public GetBlogPostsListQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<PaginatedList<BlogPostListItemDto>>> Handle(
		GetBlogPostsListQuery request, CancellationToken ct)
	{
		var query = _context.BlogPosts
			.AsNoTracking()
			.Include(p => p.Category)
			.Include(p => p.PostTags).ThenInclude(pt => pt.BlogTag)
			.Where(p => !p.IsDeleted);

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var s = request.Search.Trim();
			query = query.Where(p =>
				p.Title.Contains(s) ||
				p.Slug.Contains(s) ||
				(p.Summary != null && p.Summary.Contains(s)));
		}

		if (request.CategoryId.HasValue)
			query = query.Where(p => p.CategoryId == request.CategoryId.Value);

		if (request.IsPublished.HasValue)
			query = query.Where(p => p.IsPublished == request.IsPublished.Value);

		if (request.IsFeatured.HasValue)
			query = query.Where(p => p.IsFeatured == request.IsFeatured.Value);

		query = (request.SortBy?.ToLower(), request.SortDir?.ToLower()) switch
		{
			("title", "asc") => query.OrderBy(p => p.Title),
			("title", _) => query.OrderByDescending(p => p.Title),
			("publishedat", "asc") => query.OrderBy(p => p.PublishedAtUtc),
			("publishedat", _) => query.OrderByDescending(p => p.PublishedAtUtc),
			("viewcount", "desc") => query.OrderByDescending(p => p.ViewCount),
			("viewcount", _) => query.OrderBy(p => p.ViewCount),
			("createdat", "asc") => query.OrderBy(p => p.CreatedAtUtc),
			_ => query.OrderByDescending(p => p.CreatedAtUtc)
		};

		return await PaginatedList<BlogPostListItemDto>.CreateAsync(
			query.ProjectToType<BlogPostListItemDto>(),
			request.PageNumber, request.PageSize, ct);
	}
}