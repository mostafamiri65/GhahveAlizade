using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Seo.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Seo.Queries.GetSeoLandingPagesList;

public class GetSeoLandingPagesListQueryHandler
	: IRequestHandler<GetSeoLandingPagesListQuery, ErrorOr<PaginatedList<SeoLandingPageListItemDto>>>
{
	private readonly IAppDbContext _context;

	public GetSeoLandingPagesListQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<PaginatedList<SeoLandingPageListItemDto>>> Handle(
		GetSeoLandingPagesListQuery request, CancellationToken ct)
	{
		var query = _context.SeoLandingPages.AsNoTracking().Where(p => !p.IsDeleted);

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var s = request.Search.Trim();
			query = query.Where(p => p.Title.Contains(s) || p.Slug.Contains(s));
		}

		if (request.IsPublished.HasValue)
			query = query.Where(p => p.IsPublished == request.IsPublished.Value);

		query = request.SortDir?.ToLower() == "asc"
			? query.OrderBy(p => p.CreatedAtUtc)
			: query.OrderByDescending(p => p.CreatedAtUtc);

		return await PaginatedList<SeoLandingPageListItemDto>.CreateAsync(
			query.ProjectToType<SeoLandingPageListItemDto>(),
			request.PageNumber, request.PageSize, ct);
	}
}