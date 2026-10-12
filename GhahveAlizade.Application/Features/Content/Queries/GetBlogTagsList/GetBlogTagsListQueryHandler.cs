using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Content.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Content.Queries.GetBlogTagsList;

public class GetBlogTagsListQueryHandler
	: IRequestHandler<GetBlogTagsListQuery, ErrorOr<PaginatedList<BlogTagListItemDto>>>
{
	private readonly IAppDbContext _context;
	public GetBlogTagsListQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<PaginatedList<BlogTagListItemDto>>> Handle(
		GetBlogTagsListQuery request, CancellationToken ct)
	{
		var query = _context.BlogTags.AsNoTracking().Where(t => !t.IsDeleted);

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var s = request.Search.Trim();
			query = query.Where(t => t.Name.Contains(s) || t.Slug.Contains(s));
		}

		query = request.SortDir?.ToLower() == "desc"
			? query.OrderByDescending(t => t.Name)
			: query.OrderBy(t => t.Name);

		return await PaginatedList<BlogTagListItemDto>.CreateAsync(
			query.ProjectToType<BlogTagListItemDto>(),
			request.PageNumber, request.PageSize, ct);
	}
}